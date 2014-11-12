using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIKCIPayloadProcessor.Interfaces
{
    public interface ISiKciPayloadDataAccess
    {
        int GetFaultForSupplierReference(string cfltSupplierRef);
        int GetCordKeyFromBtRef(string btReference);
    }
}
