using System;
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
            var filename = "PlayerInfo.bin";
            Assert.IsTrue(File.Exists(filename), "The file {0} does not exist", filename);

            Player player;
            using (var stream = File.OpenRead(filename))
            {
                player = Player.Parser.ParseFrom(stream);
            }

            Assert.IsNotNull(player);
            Assert.AreEqual(Guid.Parse("777dcf9b-d40f-4260-8009-016ffce2650c"), Guid.Parse(player.Id));
            Assert.AreEqual(string.Empty, player.Email);
            Assert.AreEqual("Rickie", player.FirstName);
            Assert.AreEqual(string.Empty, player.MiddleName);
            Assert.AreEqual("Fowler", player.LastName);
            Assert.AreEqual(Gender.Male, player.Gender);
            Assert.AreEqual(Handed.RightHanded, player.Handedness);
            Assert.IsNotNull(player.CreatedAt);
            Assert.IsNotNull(player.UpdatedAt);
        }
    }
}
