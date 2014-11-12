using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SIPayloadProcessor.Interfaces
{
    public interface IFileProcessing
    {
        void GetFooterAndHeader(ref XDocument sourcedoc, XNamespace ns);
        string GenerateOutputFile(string folder, XElement newDoc, string collaborationName);
        IEnumerable<XElement> SplittingXml(FileInfo file, string field);
        void WriteXlmFilePerOrder(string destinationFolder, IEnumerable<XElement> newDocs, string element, XNamespace ns, XDocument sourcedoc);
        void MoveToArchivedFolderOfTheDay(string archivedFolder, FileInfo payloadFileInfo, string preFix, string payloadType);
    }
}
