using System.IO;

namespace SIPayloadProcessor.Interfaces
{
    public interface ILoadFileServices
    {
        FileInfo[] GetInBoundPayloadFiles();
        string GetSourceFolder();
        string GetDestinationFolder();
        string GetArchivedFolder();
        FileInfo[] LoadFilesFromDirectory(DirectoryInfo source);
    }
}
