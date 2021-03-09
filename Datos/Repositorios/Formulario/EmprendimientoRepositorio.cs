using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class EmprendimientoRepositorio : NhRepositorio<Emprendimiento>, IEmprendimientoRepositorio
    {
        public EmprendimientoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoInmueble> ObtenerTiposInmueble()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_INMUEBLE")
                .ToListResult<TipoInmueble>();
            return result;
        }

        public IList<SectorDesarrollo> ObtenerSectoresDesarrollo()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_SECTORES_DESARROLLO")
                .ToListResult<SectorDesarrollo>();
            return result;
        }

        public IList<TipoProyecto> ObtenerTiposProyecto()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_PROYECTO")
                .ToListResult<TipoProyecto>();
            return result;
        }

        public IList<Actividad> ObtenerActividades(decimal idRubro)
        {
            var result = Execute("PR_OBTENER_ACTIVIDADES")
                .AddParam(idRubro)
                .ToListResult<Actividad>();
            return result;
        }

        public IList<Rubro> ObtenerRubros()
        {
            var result = Execute("PR_OBTENER_RUBROS")
                .ToListResult<Rubro>();
            return result;
        }

        public IList<InstitucionComboResultado> ObtenerInstituciones()
        {
            var result = Execute("PR_OBTENER_INSTIT_FINANCIERAS")
                .ToListResult<InstitucionComboResultado>();
            return result;
        }

        public IList<Vinculo> ObtenerVinculos()
        {
            var result = Execute("PR_OBTENER_VINCULOS")
                .ToListResult<Vinculo>();
            return result;
        }

        public IList<TipoOrganizacion> ObtenerTiposOrganizaciones()
        {
            var result = Execute("PR_OBTENER_TIPOS_ORGANIZACION")
                .ToListResult<TipoOrganizacion>();
            return result;
        }

        public IList<ConsultarItemComercializacionResultado> ObtenerItemsComercializacion()
        {
            var result = Execute("PR_OBTENER_ITEM_DET_MER_COM")
                .ToListResult<ConsultarItemComercializacionResultado>();
            return result;
        }

        public bool GuardarMercadoComercializacion(List<decimal> itemsCheckeados, List<IdValorItem> formasPago)
        {
            throw new System.NotImplementedException();
        }

        public bool GuardarMercadoComercializacion(decimal idFormulario, bool estimaCantidad, decimal cantidadClientes,
            string itemCheckeados, string formasPagoCompra, string formasPagoVenta, decimal usuario)
        {
            var result = Execute("PR_REGISTRA_MERCADO_COMERCIAL")
            .AddParam(idFormulario)
            .AddParam(estimaCantidad)
            .AddParam(estimaCantidad ? cantidadClientes: default(decimal))
            .AddParam(itemCheckeados)
            .AddParam(formasPagoCompra)
            .AddParam(formasPagoVenta)
            .AddParam(usuario)
            .ToSpResult();
            return true;
        }

        public IList<ItemsPrecioVentaResultado> ObtenerItemsPrecioVenta()
        {
            var result = Execute("PR_OBTENER_TIPOS_COSTO")
                .ToListResult<ItemsPrecioVentaResultado>();
            return result;
        }
    }
}