import os
import grpc
import uuid
import unittest
import urllib.request

from google.protobuf.json_format import Parse
from gears.proto.capture import CaptureType_pb2
from gears.proto.player.Player_pb2 import Player
from gears.proto.capture.Capture_pb2 import Capture
from gears.proto.capture.GraphFrame_pb2 import GraphFrameCollection
from gears.proto.player import Gender_pb2, Handed_pb2
from gears.proto.server.CaptureInfo_pb2 import CaptureInfo
from gears.proto.api.v1.AuthService_pb2_grpc import AuthServiceStub
from gears.proto.api.v1.PlayerService_pb2 import ListPlayersRequest
from gears.proto.api.v1.CaptureService_pb2 import ListCapturesRequest
from gears.proto.api.v1.PlayerService_pb2_grpc import PlayerServiceStub
from gears.proto.api.v1.CaptureService_pb2_grpc import CaptureServiceStub
from gears.proto.api.v1.AuthService_pb2 import TokenRequest, TokenResponse

# get absolute path to directory this file is in.
folder = os.path.dirname(os.path.realpath(__file__))

# get path to folder containing common example data.
test_data_folder = os.path.realpath(os.path.join(folder, '..', '..', 'data'))


def str_is_uuid(uuid_string: str) -> bool:
    """
    Checks if the provided string is a uuid
    :param uuid_string: The string to test.
    :return: True if the provided string is a uuid, otherwise false
    """
    try:
        uuid.UUID(uuid_string)
        return True
    except ValueError:
        return False


class TestLoading(unittest.TestCase):

    def test_load_capture(self):
        # This example shows how to load a capture from a file.
        capture_file = os.path.join(test_data_folder, 'Rickie Fowler', '2013-10-07', '31 Capture.gpcap')
        self.assertTrue(os.path.exists(capture_file))
        capture = Capture()
        with open(capture_file, 'rb') as f:
            capture.ParseFromString(f.read())

        self.assertEqual(uuid.UUID('9c84191f-3712-4803-8a3b-432eb55a2bb1'), uuid.UUID(capture.id))
        self.assertEqual(CaptureType_pb2.GOLF, capture.type)
        self.assertEqual(10, len(capture.frames))

    def test_load_player(self):
        # This example shows how to load a player from a file.
        player_file = os.path.join(test_data_folder, 'Rickie Fowler', 'PlayerInfo.bin')
        self.assertTrue(os.path.exists(player_file))
        player = Player()
        with open(player_file, 'rb') as f:
            player.ParseFromString(f.read())

        self.assertEqual(uuid.UUID('777dcf9b-d40f-4260-8009-016ffce2650c'), uuid.UUID(player.id))
        self.assertEqual('', player.email)
        self.assertEqual('Rickie', player.first_name)
        self.assertEqual('', player.middle_name)
        self.assertEqual('Fowler', player.last_name)
        self.assertEqual(Gender_pb2.MALE, player.gender)
        self.assertEqual(Handed_pb2.RIGHT_HANDED, player.handedness)
        self.assertIsNotNone(player.created_at)
        self.assertIsNotNone(player.updated_at)

    def test_load_graph_data(self):
        # This example shows how to load graph data from a file
        graphs_file = os.path.join(test_data_folder, 'Rickie Fowler', '2013-10-07', '48208_10.graphs')
        self.assertTrue(graphs_file)
        graph_frame_collection = GraphFrameCollection()
        with open(graphs_file, 'rb') as f:
            graph_frame_collection.ParseFromString(f.read())

        self.assertGreaterEqual(len(graph_frame_collection.frames), 1)


class TestServerApiAccess(unittest.TestCase):

    def setUp(self):
        token_request_file = os.path.join(test_data_folder, 'TokenRequest.json')
        self.assertTrue(os.path.exists(token_request_file))
        self.token_request = TokenRequest()
        with open(token_request_file, 'r') as f:
            self.token_request = Parse(f.read(), self.token_request)

        cert_file = os.path.join(test_data_folder, 'public.pem')
        self.assertTrue(os.path.exists(cert_file))
        with open(cert_file, 'rb') as f:
            self.channel_credentials = grpc.ssl_channel_credentials(
                root_certificates=f.read()
            )

        # both our sandbox and production environments require a secure channel
        self.channel = grpc.secure_channel('devrpc.gearssports.com:443', self.channel_credentials)

    def test_authenticate(self):
        # This example show how to authenticate with our servers.
        auth_client = AuthServiceStub(self.channel)
        response = auth_client.Token(self.token_request)
        self.assertIsNotNone(response)
        self.assertIsInstance(response, TokenResponse)

        # This is the auth token that you will need to access the other methods in our api
        self.assertIsNotNone(response.access_token)

        # An auth token can be used until it expires. The field response.expires_in
        # represents the number of seconds the token is good for.
        self.assertGreater(response.expires_in, 0)

    def test_list_players(self):
        # This example shows how to get a list of players from our server.
        # Note that the authorization header must be added.
        auth_client = AuthServiceStub(self.channel)
        token_response = auth_client.Token(self.token_request)
        headers = (('authorization', 'Bearer %s' % token_response.access_token),)
        player_client = PlayerServiceStub(self.channel)
        request = ListPlayersRequest(per_page=2)
        # see request.filters for filtering options.
        response = player_client.List(request, metadata=headers)
        self.assertIsNotNone(response)
        self.assertGreater(response.page, 0)
        self.assertEqual(request.per_page, response.per_page)
        self.assertGreaterEqual(response.total_count, len(response.players))
        for player in response.players:
            self.assertIsInstance(player, Player)
            self.assertTrue(str_is_uuid(player.id))

    def test_list_captures(self):
        # This example shows how to get a list of capture info objects from our server.
        # Note that the authorization header must be added.
        auth_client = AuthServiceStub(self.channel)
        token_response = auth_client.Token(self.token_request)
        headers = (('authorization', 'Bearer %s' % token_response.access_token),)
        capture_client = CaptureServiceStub(self.channel)
        request = ListCapturesRequest(per_page=2)
        # see request.filters for filtering options.
        response = capture_client.List(request, metadata=headers)
        self.assertIsNotNone(response)
        self.assertGreater(response.page, 0)
        self.assertEqual(request.per_page, response.per_page)
        self.assertGreaterEqual(response.total_count, len(response.captures))
        for capture_info in response.captures:
            self.assertIsInstance(capture_info, CaptureInfo)
            self.assertTrue(str_is_uuid(capture_info.id))
            # A CaptureInfo instance only contains basic info about a capture.
            # If you want the full capture object you can download it using the url in capture_info.url.
            # The url in capture_info.url should not be stored as it will expire a few minutes after it is generated.
            self.assertIsNotNone(capture_info.url)

            capture = Capture()
            f = urllib.request.urlopen(capture_info.url)
            capture.ParseFromString(f.read())
            self.assertIsNotNone(capture.id)


if __name__ == '__main__':
    unittest.main()
