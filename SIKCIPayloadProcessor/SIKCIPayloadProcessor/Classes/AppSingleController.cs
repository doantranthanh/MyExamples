using System.Threading;
using SIKCIPayloadProcessor.Interfaces;

namespace SIKCIPayloadProcessor.Classes
{
    public class AppSingleController: IAppSingleController
    {
        public AppSingleController()
        {
            IsNewInstance = CheckInstance();
        }

        public bool CheckInstance()
        {
            bool result;
            new Mutex(true, "SIPayloadArchiver", out result);
            return result;
        }

        public bool IsNewInstance { get; private set; }

    }
}
