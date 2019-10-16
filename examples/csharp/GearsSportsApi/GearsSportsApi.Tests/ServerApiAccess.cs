using System;
using System.IO;
using System.Net;
using Gears.Proto.Api.V1;
using Gears.Proto.Server;
using Gears.Proto.Capture;
using Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GearsSportsApi.Tests
{
    /// <summary>
    ///     Examples related to our grpc server.
    /// </summary>
    [TestClass]
    public class ServerApiAccess
    {
        /// <summary>
        ///     We lazy load the channel so that we can avoid the overhead of its
        ///     creation if no tests in the current run access it.
        /// </summary>
        private static readonly Lazy<Channel> _channel = new Lazy<Channel>(ChannelFactory);

        /// <summary>
        ///     The channel used during testing.
        /// </summary>
        public static Channel Channel => _channel.Value;

        private static Channel ChannelFactory()
        {
            return new Channel(
                "devrpc.gearssports.com",
                443,
                // Both our sandbox and production environments require a secure channel.
                new SslCredentials(
                    File.ReadAllText("public.pem")
                )
            );
        }

        /// <summary>
        ///     Since the channel can be used for multiple requests we don't clean it up until we are done with all tests.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (_channel.IsValueCreated)
                _channel.Value?.ShutdownAsync().Wait();
        }

        /// <summary>
        ///     Makes request to server to get an auth token that will need to be attached to all other requests.
        /// </summary>
        /// <returns>Response from AuthService.Token grpc method.</returns>
        private TokenResponse GetAuthToken()
        {
            // We expect credentialsFile to contain the json representation of a TokenRequest object.
            var credentialsFile = "TokenRequest.json";
            Assert.IsTrue(File.Exists(credentialsFile), "The file {0} does not exist", credentialsFile);
            var credentials = File.ReadAllText(credentialsFile).Trim();
            TokenRequest request = TokenRequest.Parser.ParseJson(credentials);

            // Assert that the loaded data is not what has been committed in git as
            // we don't commit valid api credentials.
            Assert.AreNotEqual(
                "Add your client id here.",
                request.ClientId,
                "You have not updated the client id in {0}",
                credentialsFile
            );
            Assert.AreNotEqual(
                "Add your client secret here.",
                request.ClientSecret,
                "You have not updated the client secret in {0}",
                credentialsFile
            );

            // Create client and call method to authenticate with our servers.
            AuthService.AuthServiceClient client = new AuthService.AuthServiceClient(Channel);
            return client.Token(request);
        }

        /// <summary>
        ///     Example of authentication needed to access the server.
        /// </summary>
        [TestMethod]
        public void Authenticate()
        {
            TokenResponse response = GetAuthToken();
            Assert.IsNotNull(response);

            // This is the auth token that you will need to access the other methods in our api.
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.AccessToken));

            // An auth token can be used until it expires. The field response.ExpiresIn
            // represents the number of seconds the token is good for.
            Assert.IsTrue(response.ExpiresIn > 0, "response.ExpiresIn > 0");
        }

        /// <summary>
        ///     Example of getting a list of captures from the server.
        /// </summary>
        [TestMethod]
        public void ListCaptures()
        {
            // Note that the authorization header must be added.
            var headers = new Metadata { { "authorization", "Bearer " + GetAuthToken().AccessToken } };
            var client = new CaptureService.CaptureServiceClient(Channel);
            var request = new ListCapturesRequest { PerPage = 2 };
            // see request.Filters for filtering options.
            ListCapturesResponse response = client.List(request, headers);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Page > 0, "response.Page > 0");
            Assert.AreEqual(request.PerPage, response.PerPage);
            Assert.IsTrue(response.TotalCount >= response.Captures.Count, "response.TotalCount >= response.Captures.Count");
            foreach (CaptureInfo captureInfo in response.Captures)
            {
                Assert.IsNotNull(captureInfo);
                Assert.IsTrue(Guid.TryParse(captureInfo.Id, out var captureId));
                Assert.AreNotEqual(Guid.Empty, captureId);

                // A CaptureInfo instance only contains basic info about a capture.
                // If you want the full capture object you can download it using the url in captureInfo.Url.
                // The url in captureInfo.Url should not be stored as it will expire a few minutes after it is generated.
                Assert.IsFalse(string.IsNullOrWhiteSpace(captureInfo.Url));

                try
                {
                    HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(captureInfo.Url);
                    HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

                    Capture capture;
                    capture = Capture.Parser.ParseFrom(aResponse.GetResponseStream());
                    Assert.IsNotNull(capture);
                    aResponse.Dispose();
                }
                catch(Exception)
                {
                    //Error handling
                }
            }
        }

        /// <summary>
        ///     Example of getting a list of players from the server.
        /// </summary>
        [TestMethod]
        public void ListPlayers()
        {
            // Note that the authorization header must be added.
            var headers = new Metadata { { "authorization", "Bearer " + GetAuthToken().AccessToken } };
            var client = new PlayerService.PlayerServiceClient(Channel);
            var request = new ListPlayersRequest { PerPage = 2 };
            // see request.Filters for filtering options.
            ListPlayersResponse response = client.List(request, headers);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Page > 0, "response.Page > 0");
            Assert.AreEqual(request.PerPage, response.PerPage);
            Assert.IsTrue(response.TotalCount >= response.Players.Count, "response.TotalCount >= response.Players.Count");
            foreach (var player in response.Players)
            {
                Assert.IsNotNull(player);
                Assert.IsTrue(Guid.TryParse(player.Id, out var playerId));
                Assert.AreNotEqual(Guid.Empty, playerId);
            }
        }
    }
}
