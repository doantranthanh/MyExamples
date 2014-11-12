using System.IO;

namespace SIKCIPayloadProcessor.Interfaces
{
    public interface IFileServices
    {
        bool IsDirectoryExist(string fileDirectory);
        DirectoryInfo GetFileDirectory(string inputKey);
        FileInfo[] LoadFilesFromDirectory(DirectoryInfo source);
    }
}
