using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xln.Business.Enumeration;
using XLNLogger;

namespace SIKCIPayloadProcessor.Classes
{
    public class GetOrderMessageProcessor : SIAbstractProcessor
    {
        private const string CHILD_KEY = "OrderResponse";
        private const string PARRENT_KEY = "orderResponses";

        public GetOrderMessageProcessor(IXlnLogger logger) : base(logger) { }

        public override string GenerateOutputFile(string folder, XElement newDoc, string collaborationName, int fileNumber)
        {
            string cordKey = string.Empty;
            string statusValue = string.Empty;

            var caseRefenceValue = GetCaseRefence(newDoc).FirstOrDefault();

            var statusDefaultValue = GetStatus(newDoc).FirstOrDefault();

            if (caseRefenceValue != null)
            {
                cordKey = caseRefenceValue.Value;
            }

            if (statusDefaultValue != null)
            {
                statusValue = statusDefaultValue.Value;
            }

            var fileName = string.Format("{0}_{1}_{2}_{3}_{4}.xml", "SIIN", cordKey, collaborationName + "-" + statusValue, cordKey, DateTime.Now.ToString("yyyyMMddhhmmss") + "-" + fileNumber);
            var dir = Path.Combine(folder, fileName);
            return dir;
        }

        public override bool IsProcessable(string collaborationName)
        {
            return String.Equals(CollaborationNames.BCREQUESTORDERCHANGESMESSAGES, collaborationName, StringComparison.CurrentCultureIgnoreCase);
        }

        public override string SplittedChildKey()
        {
            return CHILD_KEY;
        }

        public override string SplittedParentKey()
        {
            return PARRENT_KEY;
        }

        private static IEnumerable<XElement> GetCaseRefence(XElement newDoc)
        {
            var caseRefence = from element in newDoc.Elements()
                              where element.Name.LocalName.Contains("caseReference")
                              select element;
            return caseRefence;
        }

    }
}
