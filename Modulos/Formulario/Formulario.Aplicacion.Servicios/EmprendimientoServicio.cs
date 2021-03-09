using System;
using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Aplicacion.Servicios
{
    public class EmprendimientoServicio
    {
        private readonly IEmprendimientoRepositorio _emprendimientoRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public EmprendimientoServicio(IEmprendimientoRepositorio emprendimientoRepositorio, ISesionUsuario sesionUsuario)
        {
            _emprendimientoRepositorio = emprendimientoRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public IList<TipoInmueble> ObtenerTiposInmueble()
        {
            return _emprendimientoRepositorio.ObtenerTiposInmueble();
        }
        public IList<TipoProyecto> ObtenerTiposProyecto()
        {
            return _emprendimientoRepositorio.ObtenerTiposProyecto();
        }
        public IList<SectorDesarrollo> ObtenerSectoresDesarrollo()
        {
            return _emprendimientoRepositorio.ObtenerSectoresDesarrollo();
        }
        public IList<Actividad> ObtenerActividades(decimal idRubro)
        {
            return _emprendimientoRepositorio.ObtenerActividades(idRubro);
        }

        public IList<Rubro> ObtenerRubros()
        {
            return _emprendimientoRepositorio.ObtenerRubros();
        }

        public IList<InstitucionComboResultado> ObtenerInstituciones()
        {
            return _emprendimientoRepositorio.ObtenerInstituciones();
        }

        public IList<Vinculo> ObtenerVinculos()
        {
            return _emprendimientoRepositorio.ObtenerVinculos();
        }

        public IList<TipoOrganizacion> ObtenerTiposOrganizaciones()
        {
            return _emprendimientoRepositorio.ObtenerTiposOrganizaciones();
        }

        public IList<ConsultarItemComercializacionResultado> ObtenerItemsMercadoComercializacion()
        {
            var resultado = _emprendimientoRepositorio.ObtenerItemsComercializacion();
            foreach (var item in resultado)
            {
                if (item.Descripcion.Equals("OTROS"))
                {
                    item.Descripcion = "";
                }
            }
            return resultado;
        }

        public bool GuardarItemsMercadoComercializacion(GuardarMercadoComercializacionComando comando)
        {
            try
            {
                string[] itemsParam = { "" };
                comando.ItemsCheckeados.ForEach(solicitudCurso =>
                {
                    var items = solicitudCurso.Items;
                    items.Where(item => !item.Nombre.Equals("OTROS")).ToList().ForEach(item => itemsParam[0] += item.Id + "|");
                    var otros = items.SingleOrDefault(curso => curso.Nombre.Equals("OTROS"));
                    if (otros != null)
                    {
                        itemsParam[0] += otros.Id + "|" + solicitudCurso.Descripcion + "|";
                    }
                });

                var formaPagoCompraString = "";
                var formaPagoVentaString = "";

                var formasPagoCompra = comando.FormasPago.Where((x) => x.Tipo == "c");
                var formasPagoVenta = comando.FormasPago.Where((x) => x.Tipo == "v");

                formasPagoCompra.ToList().ForEach(
                    (x) => formaPagoCompraString += x.Id.ToString() + ';' + (string.IsNullOrEmpty(x.Valor) ? "0" : x.Valor) + ';');
                formasPagoVenta.ToList().ForEach(
                    (x) => formaPagoVentaString += x.Id.ToString() + ';' + (string.IsNullOrEmpty(x.Valor) ? "0" : x.Valor) + ';');

                formaPagoCompraString = formaPagoCompraString.TrimEnd(';');
                formaPagoVentaString = formaPagoVentaString.TrimEnd(';');

                return _emprendimientoRepositorio.
                    GuardarMercadoComercializacion(
                        comando.IdFormulario,
                        comando.EstimaClientes.Estima,
                        comando.EstimaClientes.Cantidad ?? 0,
                        itemsParam[0].TrimEnd('|'),
                        formaPagoCompraString,
                        formaPagoVentaString,
                    _sesionUsuario.Usuario.Id.Valor);
            }
            catch (Exception e)
            {
                throw new ErrorTecnicoException(e.ToString());
            }
        }

        public IList<ItemsPrecioVentaResultado> ObtenerItemsPrecioVenta()
        {
            return _emprendimientoRepositorio.ObtenerItemsPrecioVenta();
        }
    }
}