using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SIKCIPayloadProcessor.Interfaces;

namespace SIKCIPayloadProcessor.Classes
{
    public class FileService: IFileServices
    {
        public bool IsDirectoryExist(string fileDirectory)
        {
            return Directory.Exists(fileDirectory);
        }

        public DirectoryInfo GetFileDirectory(string inputKey)
        {
            return new DirectoryInfo(inputKey);
        }

        public FileInfo[] LoadFilesFromDirectory(DirectoryInfo source)
        {
            return source.GetFiles();
        }
    }
}
