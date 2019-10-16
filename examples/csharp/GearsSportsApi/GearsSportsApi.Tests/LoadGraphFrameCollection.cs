using System;
using System.IO;
using Gears.Proto.Capture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GearsSportsApi.Tests
{
    /// <summary>
    ///     Examples related to graph data.
    /// </summary>
    [TestClass]
    public class LoadGraphFrameCollection
    {
        /// <summary>
        ///     Loading a graph frame collection from a file.
        /// </summary>
        [TestMethod]
        public void FromFile()
        {
            var filename = "GraphCollection.graphs";
            Assert.IsTrue(File.Exists(filename), "The file {0} does not exist", filename);

            GraphFrameCollection collection;
            using (var stream = File.OpenRead(filename))
            {
                collection = GraphFrameCollection.Parser.ParseFrom(stream);
            }

            Assert.IsNotNull(collection);
        }
    }
}
