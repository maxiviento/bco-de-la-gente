using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;

namespace Api.Controllers
{
    public class FormulariosController : ApiController
    {
        [HttpGet]
        public InformacionLineaResultado GetInformacionLinea() //averiguar que criterio se usa para identificar la linea
        {
            return new InformacionLineaResultado
            {
                CantidadMaximaCuotas = 25,
                MontoMaximo = 5000,
                Cuadrantes = ObtenerCuadrantesPorLinea()
            };
        }

        private static List<CuadranteResultado> ObtenerCuadrantesPorLinea()
        {
            return new List<CuadranteResultado>
            {
                new CuadranteResultado()
                {
                    Orden = 1,
                    Nombre = "Persona"
                },
                new CuadranteResultado()
                {
                    Orden = 2,
                    Nombre = "Grupo"
                },
                new CuadranteResultado()
                {
                    Orden = 3,
                    Nombre = "Condiciones"
                },
                new CuadranteResultado()
                {
                    Orden = 4,
                    Nombre = "Destino"
                },
                new CuadranteResultado()
                {
                    Orden = 5,
                    Nombre = "Cursos"
                },
                new CuadranteResultado()
                {
                    Orden = 6,
                    Nombre = "Pie"
                }
            }.OrderBy(x => x.Orden).ToList();  //acordarse de ordenar cuando sea traidor de la base de datos
        }
    }
}
