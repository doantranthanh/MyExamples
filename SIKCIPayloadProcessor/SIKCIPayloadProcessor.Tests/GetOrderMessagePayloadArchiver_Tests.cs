using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SIKCIPayloadProcessor.Classes;
using SIKCIPayloadProcessor.Interfaces;
using XLNLogger;


namespace SIKCIPayloadProcessor.Tests
{
    [TestFixture]
    public class GetOrderMessagePayloadArchiver_Tests
    {
        private IXlnLogger _logger;
        private ISIPayloadProcessor _processor;
        private GetOrderMessageProcessor _orderMessageProcessor;

        private FileInfo _mockPayloadFileInfo;
        
        [SetUp]
        public void SetUp()
        {
            _logger = MockRepository.GenerateMock<IXlnLogger>();
            _processor = MockRepository.GenerateMock<ISIPayloadProcessor>();
            _orderMessageProcessor = new GetOrderMessageProcessor(_logger);
        }

        [Test]
        [TestCase("SIIN_635497313837043797_BCREQUESTORDERCHANGESMESSAGES__20141024070949.xml", "BCREQUESTORDERCHANGESMESSAGES")]
        [TestCase("SIIN_635497313837043797_BCREQUESTTROUBLEREPORTINVENTORY__20141024070949.xml", "BCREQUESTTROUBLEREPORTINVENTORY")]
        public void Should_Return_Correct_Collaboration_Name(string payoadFile, string collaborationName)
        {
            var result = payoadFile.ToString().Split('_')[2];
            Assert.AreEqual(result, collaborationName);
        }

        [Test]
        [TestCase("BCREQUESTORDERCHANGESMESSAGES", true)]
        [TestCase("BCREQUESTTROUBLEREPORTINVENTORY", false)]
        public void Check_Is_Processable_On_Collaboration_Name(string collaborationName, bool expectedValue)
        {
            var result = _orderMessageProcessor.IsProcessable(collaborationName);
            Assert.AreEqual(result, expectedValue);
        }

    }
}
