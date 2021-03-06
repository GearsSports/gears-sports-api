﻿using System;
using System.IO;
using Gears.Proto.Capture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GearsSportsApi.Tests
{
    /// <summary>
    ///     Examples related to captures.
    /// </summary>
    [TestClass]
    public class LoadCapture
    {
        /// <summary>
        ///     Loading a capture from a file.
        /// </summary>
        [TestMethod]
        public void FromFile()
        {
            var filename = "ExampleCapture.gpcap";
            Assert.IsTrue(File.Exists(filename), "The file {0} does not exist", filename);

            Capture capture;
            using (var stream = File.OpenRead(filename))
            {
                capture = Capture.Parser.ParseFrom(stream);
            }

            Assert.IsNotNull(capture);
            Assert.AreEqual(Guid.Parse("9c84191f-3712-4803-8a3b-432eb55a2bb1"), Guid.Parse(capture.Id));
            Assert.AreEqual(CaptureType.Golf, capture.Type);
            Assert.AreEqual(10, capture.Frames.Count);
        }
    }
}
