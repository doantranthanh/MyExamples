using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SIPayloadProcessor.Classes;
using SIPayloadProcessor.Interfaces;

namespace SIPayloadProcessor.Tests
{
    [TestFixture]
    public class FileServices_Tests
    {

        private FileService fileService;
        private Type type;
        private const string DIRECTORY_PATH = "";
        private string _directoryFile; 

        [SetUp]
        public void Setup()
        {           
            fileService = new FileService();
            type = fileService.GetType();
        }

        [Test]
        public void Should_Have_Is_Directory_Exists_Method()
        {
            //Act
            var method = type.GetMethod("IsDirectoryExist");
            var methodTye = method.ReturnType.Name;

            //Assert
            Assert.AreEqual(method.Name, "IsDirectoryExist");
            Assert.AreEqual(methodTye, "Boolean");
        }

        [Test]
        public void Should_Have_Get_File_Directorys_Method()
        {
            //Act
            var method = type.GetMethod("GetFileDirectory");
            var methodTye = method.ReturnType.Name;

            //Assert
            Assert.AreEqual(method.Name, "GetFileDirectory");
            Assert.AreEqual(methodTye, "DirectoryInfo");
        }

        [Test]
        public void Should_Have_Load_Files_From_Directorys_Method()
        {
            //Act
            var method = type.GetMethod("LoadFilesFromDirectory");
            var methodTye = method.ReturnType.Name;

            //Assert
            Assert.AreEqual(method.Name, "LoadFilesFromDirectory");
            Assert.AreEqual(methodTye, "FileInfo[]");
        }

        [Test]
        [TestCase(DIRECTORY_PATH,false)]
        public void Is_Directory_Exists_Method_Should_Work_Correctly(string directoryFile, bool expected)
        {
            //Act
            var result = fileService.IsDirectoryExist(directoryFile);

            //Assert
            Assert.AreEqual(result, expected);
        }

        [Test] 
        public void Get_File_Directory_Method_Should_Work_Correctly()
        {
            //Arrange
            _directoryFile = Directory.GetCurrentDirectory();
            //Act
            var result = fileService.GetFileDirectory(_directoryFile);

            //Assert
            Assert.IsNotNullOrEmpty(result.ToString());
        }

        [Test]
        public void Load_Files_From_Directory_Method_Should_Work_Correctly()
        {
            //Arrange
            _directoryFile = Directory.GetCurrentDirectory();
            var source = new DirectoryInfo(_directoryFile);

            //Act
            var result = fileService.LoadFilesFromDirectory(source);

            //Assert
            Assert.IsTrue(result.Any());
        }
    }
}
