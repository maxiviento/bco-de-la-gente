using System;
using Datos.Repositorios;
using Infraestructura.Core.Comun.Dato;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Infraestructura.Core.DI.Modulos
{
    public class ModuloRepositorios: NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(x => x.FromAssemblyContaining<UsuarioRepositorio>()
                      .SelectAllClasses().InheritedFrom(typeof(IRepositorio<>))
                      .BindAllInterfaces());/*
            Kernel.Bind(x => x.FromAssembliesMatching(DiSettings.RepositoriosDll()).SelectAllClasses()
            .InheritedFrom(typeof(IRepositorio<>)).BindAllInterfaces());*/
        }
    }
}
