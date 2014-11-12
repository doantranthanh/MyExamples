using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SIKCIPayloadProcessor.Classes;
using SIKCIPayloadProcessor.Interfaces;

namespace SIKCIPayloadProcessor.Tests
{
    [TestFixture]
    public class LoadFileService_Tests
    {
        private IConfiguratonServices _configuratonServices;
        private IFileServices _fileServices;
        private LoadFileService _service;
        private Type _type;

        [SetUp]
        public void SetUp()
        {
            _configuratonServices = MockRepository.GenerateMock<IConfiguratonServices>();
            _fileServices = MockRepository.GenerateMock<IFileServices>();
            _service = new LoadFileService(_configuratonServices,_fileServices);
            _type = _service.GetType();
        }

        [Test]
        public void Should_Have_Correct_Destination_Paths_Property()
        {
            //Act            
            var destinationPaths = _type.GetProperty("DestinationPath");
            var propertyType = destinationPaths.PropertyType.Name;
            //Assert
            Assert.IsNotNull(destinationPaths);
            Assert.AreEqual(propertyType, "Dictionary`2");
        }

        [Test]
        public void Should_Have_Correct_Source_Paths_Property()
        {
            //Act            
            var sourcePath = _type.GetProperty("SourcePath");
            var propertyType = sourcePath.PropertyType.Name;
            //Assert
            Assert.IsNotNull(sourcePath);
            Assert.AreEqual(propertyType, "Dictionary`2");
        }

        [Test]
        public void Should_Have_Correct_Source_InDirectory_Property()
        {
            //Act            
            var sourcePath = _type.GetProperty("SourceInDirectoryInfo");
            var propertyType = sourcePath.PropertyType.Name;
            //Assert
            Assert.IsNotNull(sourcePath);
            Assert.AreEqual(propertyType, "DirectoryInfo");
        }

        [Test]
        public void Should_Have_Correct_Source_OutDirectory_Property()
        {
            //Act            
            var sourcePath = _type.GetProperty("SourceOutDirectoryInfo");
            var propertyType = sourcePath.PropertyType.Name;
            //Assert
            Assert.IsNotNull(sourcePath);
            Assert.AreEqual(propertyType, "DirectoryInfo");
        }

        [Test]
        public void Should_Have_Correct_InBound_Payload_Property()
        {
            //Act            
            var inboundPayloadFiles = _type.GetProperty("InBoundPayloadFiles");
            var propertyType = inboundPayloadFiles.PropertyType.Name;
            //Assert
            Assert.IsNotNull(inboundPayloadFiles);
            Assert.AreEqual(propertyType, "FileInfo[]");
        }

        [Test]
        public void Should_Have_Correct_OutBound_Payload_Property()
        {
            //Act            
            var inboundPayloadFiles = _type.GetProperty("OutBoundPayloadFiles");
            var propertyType = inboundPayloadFiles.PropertyType.Name;
            //Assert
            Assert.IsNotNull(inboundPayloadFiles);
            Assert.AreEqual(propertyType, "FileInfo[]");
        }

        [Test]
        public void Should_Have_Perform_Method()
        {
            //Act
            var method = _type.GetMethod("PerformLoadingFiles");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "PerformLoadingFiles");
            Assert.AreEqual(methodType.Name, "Void");
        }

        [Test]
        public void Should_Have_Create_Path_Method()
        {
            //Act
            var method = _type.GetMethod("CreatePaths");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "CreatePaths");
            Assert.AreEqual(methodType.Name, "Void");
        }
      
        [Test]
        public void Should_Have_Create_DirectoryInfo_Method()
        {
            //Act
            var method = _type.GetMethod("CreateDirectoryInfo");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "CreateDirectoryInfo");
            Assert.AreEqual(methodType.Name, "Void");
        }

        [Test]
        public void Should_Have_Load_Files_Method()
        {
            //Act
            var method = _type.GetMethod("LoadFiles");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "LoadFiles");
            Assert.AreEqual(methodType.Name, "FileInfo[]");
        }

        [Test]
        public void Load_File_Service_Should_Run_As_Expected()
        {
            //Arrange   
            const string INBOUND_FOLDER = @"C:\trace\SI\IN";
            const string OUTBOUND_FOLDER = @"C:\trace\SI\OUT";
            _configuratonServices.Expect(x => x.GetPathValueFromConfig(Arg<string>.Is.Equal("SI"),Arg<string>.Is.Equal("SIRequestPayloadDirPath"))).Return
                (OUTBOUND_FOLDER);

            _configuratonServices.Expect(x => x.GetPathValueFromConfig(Arg<string>.Is.Equal("SI"), Arg<string>.Is.Equal("SIResponsePayloadDirPath"))).Return
                ((INBOUND_FOLDER));

            _fileServices.Expect(x => x.IsDirectoryExist(INBOUND_FOLDER)).Return(true);
            _fileServices.Expect(x => x.IsDirectoryExist(OUTBOUND_FOLDER)).Return(true);
            
            //Act
            //service.CreatePaths();
            //service.CreateDirectoryInfo();
            _service.PerformLoadingFiles();

            // Assert
    
            _configuratonServices.AssertWasCalled(x => x.GetPathValueFromConfig(Arg<string>.Is.Anything, Arg<string>.Is.Anything),
                y => y.Repeat.Times(4));

            _fileServices.AssertWasCalled(x => x.GetFileDirectory(Arg<string>.Is.Anything),
                y => y.Repeat.Times(2));

            _fileServices.AssertWasCalled(x => x.IsDirectoryExist(Arg<string>.Is.Anything),
                y => y.Repeat.Times(2));

            _fileServices.AssertWasCalled(x => x.LoadFilesFromDirectory(Arg<DirectoryInfo>.Is.Anything),
                y => y.Repeat.Times(2));
        }

        [Test]
        public void Should_Have_Get_Source_Folder_Method()
        {
            //Act
            var method = _type.GetMethod("GetSourceFolder");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "GetSourceFolder");
            Assert.AreEqual(methodType.Name, "String");
        }
      
        [Test]
        public void Should_Have_Get_Destination_Folder_Method()
        {
            //Act
            var method = _type.GetMethod("GetDestinationFolder");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "GetDestinationFolder");
            Assert.AreEqual(methodType.Name, "String");
        }

        [Test]
        public void Should_Have_Get_Inbound_Payload_Files_Method()
        {
            //Act
            var method = _type.GetMethod("GetInBoundPayloadFiles");
            var methodType = method.ReturnType;
            //Assert
            Assert.AreEqual(method.Name, "GetInBoundPayloadFiles");
            Assert.AreEqual(methodType.Name, "FileInfo[]");
        }
    }
}
