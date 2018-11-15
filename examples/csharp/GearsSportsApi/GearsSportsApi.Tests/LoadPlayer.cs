using System.IO;
using Gears.Proto.Player;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GearsSportsApi.Tests
{
    /// <summary>
    ///     Examples related to player.
    /// </summary>
    [TestClass]
    public class LoadPlayer
    {
        /// <summary>
        ///     Loading a player from a file.
        /// </summary>
        [TestMethod]
        public void FromFile()
        {
            var filename = "Data\\Rickie Fowler\\PlayerInfo.bin";
            Assert.IsTrue(File.Exists(filename), "The file {0} does not exist", filename);

            Player player;
            using (var stream = File.OpenRead(filename))
            {
                player = Player.Parser.ParseFrom(stream);
            }

            Assert.IsNotNull(player);
            Assert.AreEqual("777dcf9b-d40f-4260-8009-016ffce2650c", player.Id);
            Assert.AreEqual(
                "{ \"id\": \"777dcf9b-d40f-4260-8009-016ffce2650c\", \"email\": " +
                "\"rickie.fowler@cobrapuma.com\", \"firstName\": \"Rickie\", " +
                "\"lastName\": \"Fowler\", \"gender\": \"MALE\", \"handedness\": " +
                "\"RIGHT_HANDED\", \"installId\": 3, \"gitHash\": \"deadbeef\", " +
                "\"createdAt\": \"2018-03-16T21:14:09Z\", \"updatedAt\": " +
                "\"2018-03-16T21:14:09Z\" }",
                player.ToString()
            );
        }
    }
}
