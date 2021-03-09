using Infraestructura.Core.DI.Modulos;
using Ninject.Modules;

namespace Infraestructura.Core.DI
{
    public class NinjectHttpModules
    {
        //Return Lists of Modules in the Application
        public static NinjectModule[] Modulos
        {
            get
            {
                return new NinjectModule[]
                {
                    new ModuloBaseDeDatos(),
                    new ModuloRepositorios(),
                    new ModuloServicios(),
                    new ModuloSesionUsuario()
                };
            }
        }

      }

}
