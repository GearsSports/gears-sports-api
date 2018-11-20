import os
import grpc
import unittest

from google.protobuf.json_format import Parse
from gears.proto.capture import CaptureType_pb2
from gears.proto.player.Player_pb2 import Player
from gears.proto.capture.Capture_pb2 import Capture
from gears.proto.api.v1.AuthService_pb2 import TokenRequest
from gears.proto.api.v1.AuthService_pb2_grpc import AuthServiceStub
from gears.proto.api.v1.PlayerService_pb2 import ListPlayersRequest
from gears.proto.api.v1.CaptureService_pb2 import ListCapturesRequest
from gears.proto.api.v1.PlayerService_pb2_grpc import PlayerServiceStub
from gears.proto.api.v1.CaptureService_pb2_grpc import CaptureServiceStub

# get absolute path to directory this file is in.
folder = os.path.dirname(os.path.realpath(__file__))


class TestLoading(unittest.TestCase):

    def test_load_capture(self):
        capture_file = os.path.join(folder, 'test_data', 'Rickie Fowler', '2013-10-07', '31 Capture.gpcap')
        self.assertTrue(os.path.exists(capture_file))
        capture = Capture()
        with open(capture_file, 'rb') as f:
            capture.ParseFromString(f.read())

        self.assertEqual('9c84191f-3712-4803-8a3b-432eb55a2bb1', capture.id)
        self.assertEqual(CaptureType_pb2.GOLF, capture.type)
        self.assertEqual(446, len(capture.frames))

    def test_load_player(self):
        player_file = os.path.join(folder, 'test_data', 'Rickie Fowler', 'PlayerInfo.bin')
        self.assertTrue(os.path.exists(player_file))
        player = Player()
        with open(player_file, 'rb') as f:
            player.ParseFromString(f.read())

        self.assertEqual('777dcf9b-d40f-4260-8009-016ffce2650c', player.id)


class TestServerApiAccess(unittest.TestCase):

    def setUp(self):
        token_request_file = os.path.join(folder, 'test_data', 'TokenRequest.json')
        self.assertTrue(os.path.exists(token_request_file))
        token_request = TokenRequest()
        with open(token_request_file, 'r') as f:
            token_request = Parse(f.read(), token_request)
        self.token_request = token_request

        cert_file = os.path.join(folder, 'test_data', 'public.pem')
        self.assertTrue(os.path.exists(cert_file))
        with open(cert_file, 'rb') as f:
            self.channel_credentials = grpc.ssl_channel_credentials(
                root_certificates=f.read()
            )
        self.channel = grpc.secure_channel('devrpc.gearssports.com:443', self.channel_credentials)

    def test_authenticate(self):
        auth_client = AuthServiceStub(self.channel)
        response = auth_client.Token(self.token_request)
        self.assertIsNotNone(response)
        self.assertIsNotNone(response.access_token)
        self.assertGreater(response.expires_in, 0)

    def test_list_players(self):
        auth_client = AuthServiceStub(self.channel)
        token_response = auth_client.Token(self.token_request)
        headers = (('authorization', 'Bearer %s' % token_response.access_token),)
        player_client = PlayerServiceStub(self.channel)
        request = ListPlayersRequest(per_page=2)
        response = player_client.List(request, metadata=headers)
        self.assertIsNotNone(response)
        self.assertGreater(response.page, 0)
        self.assertEqual(request.per_page, response.per_page)
        self.assertGreaterEqual(response.total_count, len(response.players))

    def test_list_captures(self):
        auth_client = AuthServiceStub(self.channel)
        token_response = auth_client.Token(self.token_request)
        headers = (('authorization', 'Bearer %s' % token_response.access_token),)
        capture_client = CaptureServiceStub(self.channel)
        request = ListCapturesRequest(per_page=2)
        response = capture_client.List(request, metadata=headers)
        self.assertIsNotNone(response)
        self.assertGreater(response.page, 0)
        self.assertEqual(request.per_page, response.per_page)
        self.assertGreaterEqual(response.total_count, len(response.captures))


if __name__ == '__main__':
    unittest.main()
