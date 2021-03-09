using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers.Pagos
{
    public class BancosController : ApiController
    {
        private readonly BancosServicio _bancosServicio;

        public BancosController(BancosServicio bancosServicio)
        {
            _bancosServicio = bancosServicio;
        }

        [HttpGet]
        [Route("obtener-combo-bancos")]
        public IList<ClaveValorResultado<string>> GetComboBanco()
        {
            return _bancosServicio.ObtenerComboBancos();
        }

        [HttpGet]
        [Route("obtener-combo-sucursales/{idBanco:decimal}")]
        public IList<ClaveValorResultado<string>> GetComboSucursales(string idBanco)
        {
            return _bancosServicio.ObtenerComboSucursales(idBanco);
        }
    }
}