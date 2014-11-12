using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SIKCIPayloadProcessor.Interfaces;
using Xln.Business.Enumeration;
using XLN.Business.Factories;
using XLNLogger;

namespace SIKCIPayloadProcessor.Classes
{
    public class GetUnsolicitedCeaseProcessor : SIAbstractProcessor
    {
        private const string CHILD_KEY = "UnsolicitedCease";
        private const string PARRENT_KEY = "ceases";
        protected ISiKciPayloadDataAccess FaultDataAccessAgent { get; private set; }

         public GetUnsolicitedCeaseProcessor(IXlnLogger logger, ISiKciPayloadDataAccess payloadDataAccess)
             : base(logger)
        {
            FaultDataAccessAgent = payloadDataAccess;
        }

        public override string GenerateOutputFile(string folder, XElement newDoc, string collaborationName, int fileNumber)
        {
            string cordKey = string.Empty;
            string statusValue = string.Empty;

            var btRefenceValue = GetBtReference(newDoc).FirstOrDefault();;

            var statusDefaultValue = GetStatus(newDoc).FirstOrDefault();

            if (btRefenceValue != null)
            {
                cordKey = FaultDataAccessAgent.GetCordKeyFromBtRef(btRefenceValue.Value).ToString();
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
            return String.Equals(CollaborationNames.BCREQUESTCHANGEDUNSOLICITEDCEASES, collaborationName, StringComparison.CurrentCultureIgnoreCase);
        }

        public override string SplittedChildKey()
        {
            return CHILD_KEY;
        }

        public override string SplittedParentKey()
        {
            return PARRENT_KEY;
        }

        private static IEnumerable<XElement> GetBtReference(XElement newDoc)
        {
            var btRefence = from element in newDoc.Elements()
                              where element.Name.LocalName.Contains("sellersId")
                              select element;
            return btRefence;
        }
    }
}
