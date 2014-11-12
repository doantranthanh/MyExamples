using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SIKCIPayloadProcessor.Enums;
using SIKCIPayloadProcessor.Interfaces;
using XLNLogger;

namespace SIKCIPayloadProcessor.Classes
{
    public abstract class SIAbstractProcessor : ISIPayloadProcessor 
    {  
        protected IXlnLogger Logger { get; set; } 
        protected string CollaborationName { get; set; }
        private const string NAME_SPACE = "http://model.inventory.empower.vnocore.com";

        protected SIAbstractProcessor(IXlnLogger logger)
        {
            Logger = logger;
        }

        protected XElement StripNameSpace(XElement root)
        {
	        return new XElement(
		        root.Name.LocalName,
		        root.HasElements ?
			        root.Elements().Select(StripNameSpace) :
			        (object)root.Value
	        );
        }

        protected void Save(string file, string content)
        {
            using (var writer = new StreamWriter(file, true))
            {
                writer.Write(content);
            }
        }

        protected void MoveToArchivedFolderOfTheDay(string archivedFolder, FileInfo payloadFileInfo, string preFix, string payloadType)
        {
            DateTime payloadTimeStamp = payloadFileInfo.CreationTime;
            string archiveFolderOfTheDay = payloadTimeStamp.Year + "-" + payloadTimeStamp.Month.ToString("00") + "-" + payloadTimeStamp.Day.ToString("00");
            string archivedPayloadsFinalDestination = archivedFolder + "\\" + archiveFolderOfTheDay;
            if (!Directory.Exists(archivedPayloadsFinalDestination))
            {
                Directory.CreateDirectory(archivedPayloadsFinalDestination);
            }
            payloadFileInfo.MoveTo(archivedPayloadsFinalDestination + "\\" + preFix + "-" + payloadFileInfo.Name);
        }

        protected IEnumerable<XElement> SplittingXml(FileInfo file, string field)
        {
            XDocument sourcedoc = XDocument.Load(file.FullName);
            var nodes = sourcedoc.DescendantNodes();
            var newDocs = from c in nodes.OfType<XElement>()
                          where c.Name.LocalName.EndsWith(field,StringComparison.InvariantCultureIgnoreCase)
                          select c;
            return newDocs;
        }

        protected void WriteXlmFilePerOrder(string destinationFolder, IEnumerable<XElement> newDocs, string element, XNamespace ns, XDocument sourcedoc)
        {
            var fileNumber = 0;
            var xElements = newDocs as XElement[] ?? newDocs.ToArray();
            foreach (var newDoc in xElements)
            {
                fileNumber++;
                GetFooterAndHeader(ref sourcedoc, ns);
                IEnumerable<XElement> appendedElements = sourcedoc.Descendants(ns + element);
                var firstOrDefault = appendedElements.FirstOrDefault();
                if (firstOrDefault != null) firstOrDefault.Add(new XElement(newDoc));
                var outputFile = GenerateOutputFile(destinationFolder, newDoc, CollaborationName, fileNumber);
                if (!String.IsNullOrEmpty(outputFile))
                {  
                    Save(outputFile, sourcedoc.ToString());
                }
                else
                {
                    Logger.Info(String.Format("There is no new created output file, check the log file"));
                }
            }
        }

        protected void GetFooterAndHeader(ref XDocument sourcedoc, XNamespace ns)
        {
            sourcedoc.Descendants()
                .Where(x => x.Name == ns + SplittedChildKey() || x.Name == SplittedChildKey())
                .Remove();
        }

        protected virtual XNamespace SetNameSpace()
        {
            return NAME_SPACE;
        }

        public abstract string GenerateOutputFile(string folder, XElement newDoc, string collaborationName, int fileNumber);

        public abstract bool IsProcessable(string collaborationName);

        public abstract string SplittedChildKey();

        public abstract string SplittedParentKey();

        protected virtual IEnumerable<XElement> GetStatus(XElement newDoc)
        {
            var status = from element in newDoc.Elements()
                         where String.Equals(element.Name.LocalName, "status", StringComparison.CurrentCultureIgnoreCase)
                         select element;
            return status;
        }

        public void Run(FileInfo[] payloadFiles, string destinationFolder, string archivedFolder)
        {
            foreach (var payloadFile in payloadFiles)
            {
                CollaborationName = payloadFile.ToString().Split('_')[2];
                if (IsProcessable(CollaborationName))
                {
                    XDocument sourcedoc = XDocument.Load(payloadFile.FullName);
                    string preFix = DateTime.Now.ToString("Hmmssfff");
                    Logger.Info(String.Format("Splitting files from original payload"));
                    var splittedDocs = SplittingXml(payloadFile, SplittedChildKey());
                    Logger.Info(String.Format("Construct files and save to source directories"));
                    WriteXlmFilePerOrder(destinationFolder, splittedDocs, SplittedParentKey(), SetNameSpace(), sourcedoc);
                    Logger.Info(String.Format("After finishing processing original file, then move this original file to Archived Folder"));
                    MoveToArchivedFolderOfTheDay(archivedFolder, payloadFile, preFix, Source.SI_INBOUND.ToString());
                }
            }
        }

    }
}
