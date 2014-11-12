using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace SIKCIPayloadProcessor.Interfaces
{
    public interface ISIPayloadProcessor
    {
        void Run(FileInfo[] payloadFiles, string destinationFolder, string archivedFolder);       
    }
}
