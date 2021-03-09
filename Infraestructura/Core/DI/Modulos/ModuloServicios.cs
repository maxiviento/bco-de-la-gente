using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Infraestructura.Core.DI.Modulos
{
    public class ModuloServicios: NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x => x.FromAssembliesMatching(DiSettings.ServiciosDll()).SelectAllClasses());
        }
    }
}
