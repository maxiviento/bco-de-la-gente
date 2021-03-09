using System.Web.Http;
using Infraestructura.Core.CiDi;
using Infraestructura.Core.DI;
using Infraestructura.Core.Documentador;
using Infraestructura.Core.Filtros;
using Infraestructura.Core.Formateadores;
using Infraestructura.Core.Manejadores;
using Infraestructura.Core.Proveedores;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(Api.Startup))]

namespace Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if (STAGING || TESTING || UTN )
            log4net.Config.XmlConfigurator.Configure();
#endif
            ConfigureWebApi(app, new HttpConfiguration());
/*

#if TESTING 
           HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif*/
        }

        public void ConfigureWebApi(IAppBuilder app, HttpConfiguration config)
        {
            Documentador.Habilitar(config);
            RegistrarRutas(config);
            Manejadores.Registrar(config);
            Formateadores.Registrar(config);
            Filtros.Registrar(config);
            Injector.Registrar(config);

            app.UseCiDi();
            app.UseWebApi(config);
        }

        public void RegistrarRutas(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(new RouteProvider());
            var routeTemplate = "{controller}/{id}";

#if DEBUG
            routeTemplate = "api/{controller}/{id}";
#endif

            config.Routes.MapHttpRoute(
                name: "api",
                routeTemplate: routeTemplate,
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}