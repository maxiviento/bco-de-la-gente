using System;
using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Formulario.Dominio.Modelo;

namespace Api.Controllers.Formulario
{
    public class EmprendimientosController : ApiController
    {
        private readonly EmprendimientoServicio _emprendimientoServicio;

        public EmprendimientosController(EmprendimientoServicio emprendimientoServicio)
        {
            _emprendimientoServicio = emprendimientoServicio;
        }

        [Route("tipos-inmueble")]
        public IList<TipoInmueble> GetTiposInmueble()
        {
            return _emprendimientoServicio.ObtenerTiposInmueble();
        }

        [Route("tipos-proyecto")]
        public IList<TipoProyecto> GetTiposProyecto()
        {
            return _emprendimientoServicio.ObtenerTiposProyecto();
        }

        [Route("sectores-desarrollo")]
        public IList<SectorDesarrollo> GetSectoresDesarrollo()
        {
            return _emprendimientoServicio.ObtenerSectoresDesarrollo();
        }

        [Route("rubros")]
        public IList<Rubro> GetRubros()
        {
            return _emprendimientoServicio.ObtenerRubros();
        }

        [Route("actividades/{idRubro}")]
        [HttpGet]
        public IList<Actividad> GetActividades(decimal idRubro)
        {
            return _emprendimientoServicio.ObtenerActividades(idRubro);
        }

        [Route("instituciones")]
        public IList<InstitucionComboResultado> GetInstituciones()
        {
            return _emprendimientoServicio.ObtenerInstituciones();
        }

        [Route("vinculos")]
        public IList<Vinculo> GetVinculos()
        {
            return _emprendimientoServicio.ObtenerVinculos();
        }

        [Route("tipos-organizacion")]
        public IList<TipoOrganizacion> GetTiposOrganizaciones()
        {
            return _emprendimientoServicio.ObtenerTiposOrganizaciones();
        }
        
        [Route("obtener-items-comercializacion")]
        public IList<ConsultarItemComercializacionResultado> GetItemsMercadoComercializacion()
        {
            return _emprendimientoServicio.ObtenerItemsMercadoComercializacion();
        }

        [HttpPost]
        [Route("guardar-mercado-comercializacion")]
        public bool GuardarItemsMercadoComercializacion(GuardarMercadoComercializacionComando comando)
        {
            return _emprendimientoServicio.GuardarItemsMercadoComercializacion(comando);
        }

        [Route("items-precio-venta")]
        public IList<ItemsPrecioVentaResultado> GetItemsPrecioVenta()
        {
            return _emprendimientoServicio.ObtenerItemsPrecioVenta();
        }
    }
}