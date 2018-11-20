﻿using System;
using System.IO;
using Gears.Proto.Api.V1;
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
                new SslCredentials(
                    File.ReadAllText(Path.Combine("Data", "public.pem"))
                )
            );
        }

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
            var credentialsFile = Path.Combine("Data", "TokenRequest.json");
            Assert.IsTrue(File.Exists(credentialsFile), "The file {0} does not exist", credentialsFile);
            var credentials = File.ReadAllText(credentialsFile).Trim();
            var request = TokenRequest.Parser.ParseJson(credentials);

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
            var client = new AuthService.AuthServiceClient(Channel);
            return client.Token(request);
        }

        /// <summary>
        ///     Example of authentication needed to access the server.
        /// </summary>
        [TestMethod]
        public void Authenticate()
        {
            var response = GetAuthToken();
            Assert.IsNotNull(response);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.AccessToken));
            Assert.IsTrue(response.ExpiresIn > 0, "response.ExpiresIn > 0");
        }

        /// <summary>
        ///     Example of getting a list of captures from the server.
        /// </summary>
        [TestMethod]
        public void ListCaptures()
        {
            var headers = new Metadata { { "authorization", "Bearer " + GetAuthToken().AccessToken } };
            var client = new CaptureService.CaptureServiceClient(Channel);
            var request = new ListCapturesRequest { PerPage = 2 };
            var response = client.List(request, headers);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Page > 0, "response.Page > 0");
            Assert.AreEqual(request.PerPage, response.PerPage);
            Assert.IsTrue(response.TotalCount >= response.Captures.Count, "response.TotalCount >= response.Captures.Count");
        }

        /// <summary>
        ///     Example of getting a list of players from the server.
        /// </summary>
        [TestMethod]
        public void ListPlayers()
        {
            var headers = new Metadata { { "authorization", "Bearer " + GetAuthToken().AccessToken } };
            var client = new PlayerService.PlayerServiceClient(Channel);
            var request = new ListPlayersRequest { PerPage = 2 };
            var response = client.List(request, headers);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Page > 0, "response.Page > 0");
            Assert.AreEqual(request.PerPage, response.PerPage);
            Assert.IsTrue(response.TotalCount >= response.Players.Count, "response.TotalCount >= response.Players.Count");
        }
    }
}