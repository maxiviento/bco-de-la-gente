using System.Configuration;


namespace Infraestructura.Core.CiDi.Configuration.Sections
{
    public class CidiEndpointCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CidiEndpoint();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CidiEndpoint)element).Key;
        }

        protected override string ElementName
        {
            get
            {
                return "endpoint";
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrEmpty(elementName) && elementName == "endpoint";
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public CidiEndpoint this[UrlCidiEnum key] => BaseGet(key) as CidiEndpoint;
    }
}
