using System.Configuration;

namespace Infraestructura.Core.CiDi.Configuration.Sections
{
    public class CidiEnvironment : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Nombre
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("id_application", IsRequired = true)]
        public int IdApplication
        {
            get
            {
                return (int)this["id_application"];
            }
            set
            {
                this["id_application"] = value;
            }
        }

        [ConfigurationProperty("client_secret", IsRequired = true)]
        public string ClientSecret
        {
            get
            {
                return (string)this["client_secret"];
            }
            set
            {
                this["client_secret"] = value;
            }
        }

        [ConfigurationProperty("client_key", IsRequired = true)]
        public string ClientKey
        {
            get
            {
                return (string)this["client_key"];
            }
            set
            {
                this["client_key"] = value;
            }
        }

        [ConfigurationProperty("env_prod", IsRequired = true)]
        public bool EnvProd
        {
            get
            {
                return (bool)this["env_prod"];
            }
            set
            {
                this["env_prod"] = value;
            }
        }

        [ConfigurationProperty("endpoints", IsDefaultCollection = true,
        IsKey = false, IsRequired = true)]
        public CidiEndpointCollection Endpoints
        {
            get
            {
                return base["endpoints"] as CidiEndpointCollection;
            }

            set
            {
                base["endpoints"] = value;
            }
        }
    }
}
