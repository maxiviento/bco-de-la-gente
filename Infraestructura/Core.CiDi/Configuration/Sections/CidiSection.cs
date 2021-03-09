using System.Configuration;

namespace Infraestructura.Core.CiDi.Configuration.Sections
{
    public class CidiSection : ConfigurationSection
    {

        [ConfigurationProperty("env", IsRequired = true)]
        public string Env
        {
            get
            {
                return (string)this["env"];
            }
            set
            {
                this["env"] = value;
            }
        }

        [ConfigurationProperty("environments", IsDefaultCollection = true,
        IsKey = false, IsRequired = true)]
        public CidiEnvironmentCollection Environments
        {
            get
            {
                return base["environments"] as CidiEnvironmentCollection;
            }

            set
            {
                base["environments"] = value;
            }
        }
    }

}
