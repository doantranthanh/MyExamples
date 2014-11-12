using SIKCIPayloadProcessor.Interfaces;

namespace SIKCIPayloadProcessor.Classes
{
    public class ConfigurationService : IConfiguratonServices
    {
        public string GetPathValueFromConfig(string platform, string direction)
        {
            return GuiConfigReader.GetValue(platform, direction);
        }
    }
}
