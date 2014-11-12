using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using SIPayloadProcessor.Classes;
using SIPayloadProcessor.Interfaces;

namespace SIPayloadProcessor.Tests
{
    [TestFixture]
    public class ConfigurationServices_Tests
    {
        private LoadFileService service;
        private Type type;

        [Test]
        [TestCase("SI", "SIRequestPayloadDirPath")]
        [TestCase("SI", "SIResponsePayloadDirPath")]
        public void Run_Successfully_Method_GetPathValueFromConfig(string platform, string direction)
        {
            //Arrange
            var implementedClass = new ConfigurationService();

            //Act
            var result = implementedClass.GetPathValueFromConfig(platform, direction);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
