using System;
using System.Collections.Generic;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Datos.Repositorios.Soporte
{
    public class RentaRepositorio : NhRepositorio<DatoRenta>, IRentaRepositorio
    {
        public RentaRepositorio(ISession sesion) : base(sesion)
        {
        }

        public Id RegistrarDatoRenta(decimal idFormularioLinea,decimal idPrestamo, RentasPrestamoPlano dato, Usuario usuario, DateTime fechaAlta, decimal idPrestamoRequisito)
        {
            var result = Execute("PR_REGISTRA_DATOS_RENTAS")
                .AddParam(idPrestamo)
                .AddParam(dato.GF_Objeto)
                .AddParam(dato.GF_BaseImponible)
                .AddParam(dato.GF_Porcentaje)
                .AddParam(dato.GF_Modelo)
                .AddParam(dato.GF_Marca)
                .AddParam(dato.GF_Estado)
                .AddParam(dato.GF_Superficie)
                .AddParam(dato.GF_Domicilio)
                .AddParam(dato.GF_IdSexo)
                .AddParam(dato.GF_NroDocumento)
                .AddParam(dato.GF_IdPais)
                .AddParam(dato.GF_IdNumero)
                .AddParam(dato.EsSolicitante)
                .AddParam(usuario.Id)
                .AddParam(fechaAlta)
                .AddParam(idFormularioLinea)
                .AddParam(dato.EnabledAlert ? 'S' : 'N')
                .AddParam(idPrestamoRequisito)
                .ToSpResult();

            return result.Id;
        }

        public List<HistorialRentas> ObtenerHistorial(decimal idHistorial)
        {
            return (List<HistorialRentas>)
                Execute("PR_OBTENER_DET_DATOS_RENTAS")
                    .AddParam(idHistorial)
                    .ToListResult<HistorialRentas>();
        }

        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesRentas(DocumentacionConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_DATOS_RENTAS")
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<DocumentacionResultado>();
            return CrearResultado(consulta, elementos);
        }
    }
}