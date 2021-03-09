using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface IEmprendimientoRepositorio
    {
        IList<TipoInmueble> ObtenerTiposInmueble();
        IList<TipoProyecto> ObtenerTiposProyecto();
        IList<SectorDesarrollo> ObtenerSectoresDesarrollo();
        IList<Actividad> ObtenerActividades(decimal idRubro);
        IList<Rubro> ObtenerRubros();
        IList<InstitucionComboResultado> ObtenerInstituciones();
        IList<Vinculo> ObtenerVinculos();
        IList<TipoOrganizacion> ObtenerTiposOrganizaciones();
        IList<ConsultarItemComercializacionResultado> ObtenerItemsComercializacion();

        bool GuardarMercadoComercializacion(decimal idFormulario, bool estimaCantidad, decimal cantidadClientes,
            string itemCheckeados, string formasPagoCompra, string formasPagoVenta, decimal usuario);

        IList<ItemsPrecioVentaResultado> ObtenerItemsPrecioVenta();
    }
}
