using System.Configuration;

namespace Infraestructura.Core.CiDi.Configuration.Sections
{
    public class CidiEnvironmentCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CidiEnvironment();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CidiEnvironment)element).Nombre;
        }

        protected override string ElementName => "environment";

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrEmpty(elementName) && elementName == "environment";
        }

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        public CidiEnvironment this[int index] => BaseGet(index) as CidiEnvironment;

        public new CidiEnvironment this[string key] => BaseGet(key) as CidiEnvironment;
    }
}
