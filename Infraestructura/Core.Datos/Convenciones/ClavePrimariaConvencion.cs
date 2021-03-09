using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Infraestructura.Core.Datos.Convenciones
{
    public class ClavePrimariaConvencion : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(string.Format("ID_{0}", instance.EntityType.Name.ToUpper()));
        }
    }
}