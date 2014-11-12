using NUnit.Framework;
using SIPayloadProcessor.Classes;
using Rhino.Mocks;
using SIPayloadProcessor.Interfaces;


namespace SIPayloadProcessor.Tests
{
    [TestFixture]
    public class AppSingleController_Tests
    {
        [Test]
        public void Should_Return_One_Instance_At_The_Same_Time()
        {
            var appsingleController = new AppSingleController();

            var mock = MockRepository.GenerateMock<IAppSingleController>();

            mock.Stub(x => x.IsNewInstance).Return(true);

            var instance = appsingleController.CheckInstance();
            Assert.AreEqual(instance, false);
        }
    }
}
