using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SIKCIPayloadProcessor.Interfaces;
using Xln.Business.Enumeration;
using XLN.Business.Factories;
using XLNLogger;

namespace SIKCIPayloadProcessor.Classes
{
    public class GetFaultChangesProcessor : SIAbstractProcessor
    {
        private const string CHILD_KEY = "TroubleReportResponse";
        private const string PARRENT_KEY = "troubleReportResponses";
        protected ISiKciPayloadDataAccess FaultDataAccessAgent { get; private set; }

        public GetFaultChangesProcessor(IXlnLogger logger, ISiKciPayloadDataAccess payloadDataAccess) : base(logger)
        {
            FaultDataAccessAgent = payloadDataAccess;
        }

        public override string GenerateOutputFile(string folder, XElement newDoc, string collaborationName,int fileNumber)
        {
            string cfltKey = string.Empty;
            string statusValue = string.Empty;

            var troubleReportReferenceValue = GetTroubleReportReference(newDoc).FirstOrDefault();
            var statusDefaultValue = GetStatus(newDoc).FirstOrDefault();

            if (troubleReportReferenceValue != null)
            {
                cfltKey = FaultDataAccessAgent.GetFaultForSupplierReference(troubleReportReferenceValue.Value).ToString();
            }

            if (statusDefaultValue != null)
            {
                statusValue = statusDefaultValue.Value;
            }

            var fileName = PopulateFileName(collaborationName, cfltKey, statusValue, troubleReportReferenceValue, fileNumber);

            var dir = PopulateDirectory(folder, cfltKey, fileName);

            return dir;
        }

        private string PopulateFileName(string collaborationName, string cfltKey, string statusValue, XElement troubleReportReferenceValue, int fileNumber)
        {
            string fileName = string.Empty;
            if (!String.IsNullOrEmpty(cfltKey))
            {
                fileName = string.Format("{0}_{1}_{2}_{3}_{4}.xml", "SIIN", cfltKey, collaborationName + "-" + statusValue, cfltKey, DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + fileNumber);
            }
            else
            {
                if (troubleReportReferenceValue != null)
                    Logger.Info(String.Format("There is no fault record for {0}", troubleReportReferenceValue.Value));
            }
            return fileName;
        }

        public override bool IsProcessable(string collaborationName)
        {
            return String.Equals(CollaborationNames.BCREQUESTTROUBLEREPORTINVENTORY, collaborationName, StringComparison.CurrentCultureIgnoreCase);
        }

        public override string SplittedChildKey()
        {
            return CHILD_KEY;
        }

        public override string SplittedParentKey()
        {
            return PARRENT_KEY;
        }

        private static string PopulateDirectory(string folder, string cfltKey, string fileName)
        {
            string dir = string.Empty;

            if (!String.IsNullOrEmpty(cfltKey))
            {
                dir = Path.Combine(folder, fileName);
            }
            return dir;
        }

        private static IEnumerable<XElement> GetTroubleReportReference(XElement newDoc)
        {
            var troubleReportReference = from element in newDoc.Elements()
                                         where element.Name.LocalName.Contains("troubleReportReference")
                                         select element;
            return troubleReportReference;
        }
       
    }
}
