using System.Configuration;

namespace Infraestructura.Core.CiDi.Configuration.Sections
{
    public class CidiEndpoint :  ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public UrlCidiEnum Key
        {
            get
            {
                return (UrlCidiEnum)this["key"];
            }
            set
            {
                this["key"] = value;
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}
