using System.Collections.Generic;
using System.Data.SqlClient;
using SIKCIPayloadProcessor.Interfaces;
using XLN.Business.Factories;

namespace SIKCIPayloadProcessor.DAL
{
    public class SIKCIPayloadDataAccess : ISiKciPayloadDataAccess
    {
        public int GetFaultForSupplierReference(string cfltSupplierRef)
        {
            var command = SqlFactory.Manufacture(CRMStoredProcedure.GetFaultForSupplierReference,
                new List<SqlParameter>
                {
                    new SqlParameter("@CFLT_SUPPLIER_REF ", cfltSupplierRef),                                
                });

            return SqlExecuter.SelectOneScalar<int>(command);
        }

        public int GetCordKeyFromBtRef(string btReference)
        {
            var command = SqlFactory.Manufacture(WLR3StoredProcedure.GetCordKeyFromBtReference,
                new List<SqlParameter>
                {
                    new SqlParameter("@CORD_BT_REF ", btReference),                                
                });

            return SqlExecuter.SelectOneScalar<int>(command);
        }
    }
}
