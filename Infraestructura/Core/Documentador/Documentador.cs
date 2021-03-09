using Infraestructura.Core.Filtros;
using Swashbuckle.Application;
using System;
using System.Net.Http;
using System.Web.Http;

namespace Infraestructura.Core.Documentador
{
    public class Documentador
    {
        public static void Habilitar(HttpConfiguration config)
        {
            config.EnableSwagger("docs-api/{apiVersion}", c =>
                {
                    c.OperationFilter<HandleFromUriParam>();
                    c.SingleApiVersion(DocSettings.Version(), DocSettings.TituloApi());
                    c.RootUrl(req =>
                        req.RequestUri.GetLeftPart(UriPartial.Authority) +
                        req.GetRequestContext().VirtualPathRoot.TrimEnd('/'));
                    c.UseFullTypeNameInSchemaIds();
                    // ^ para que no falle si distintos endpoints devuelven instancias de clases con igual contenido y distinto nombre
                })
                .EnableSwaggerUi($"{DocSettings.Url()}/{{*assetPath}}", c =>
                {
                    c.EnableApiKeySupport("Authorization", "header");
                });

        }
    }
}