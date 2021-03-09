using System;
using System.Collections.Generic;
using AppComunicacion.ApiModels;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using SintysWS.Modelo;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;
using Domicilio = SintysWS.Modelo.Domicilio;

namespace Datos.Repositorios.Soporte
{
    public class SintysRepositorio : NhRepositorio<DatoSintys>, ISintysRepositorio
    {
        public SintysRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarCabeceraDatosSintys(string nroPrestamo, Id id, DateTime fechaActual, decimal idFormularioLinea)
        {
            var resultado = Execute("PR_REGISTRA_DATOS_SINTYS")
                .AddParam(nroPrestamo)
                .AddParam(id)
                .AddParam(fechaActual)
                .AddParam(idFormularioLinea)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public void RegistrarDesempleo(Desempleo desempleo, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            if (desempleo != null)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_DESEMPLEOS_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(desempleo.CantidadCuotas)
                    .AddParam(desempleo.CantCuotasLiquidadas)
                    .AddParam(desempleo.Periodo)
                    .AddParam(desempleo.BaseOrigen)
                    .ToSpResult();
            }
        }

        public void RegistrarDomicilio(List<Domicilio> domicilios, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            foreach (var domicilio in domicilios)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_DOMICILIOS_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(domicilio.Provincia)
                    .AddParam(domicilio.Localidad)
                    .AddParam(domicilio.CodigoPostal)
                    .AddParam(domicilio.Calle)
                    .AddParam(domicilio.Numero)
                    .AddParam(domicilio.Piso)
                    .AddParam(domicilio.Depto)
                    .AddParam(domicilio.BaseOrigen)
                    .ToSpResult();
            }
        }

        public void RegistrarEmpleoPresunto(List<EmpleoPresunto> empleos, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            foreach (var empleo in empleos)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_EMPLEOS_P_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(empleo.CuitEmpleador)
                    .AddParam(empleo.Empleador)
                    .AddParam(empleo.Periodo)
                    .AddParam(empleo.BaseOrigen)
                    .ToSpResult();
            }
        }

        public void RegistrarEmpleoFormal(List<EmpleoFormal> empleos, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            foreach (var empleo in empleos)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_EMPLEOS_f_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(empleo.CuitEmpleador)
                    .AddParam(empleo.DenominacionEmpleador)
                    .AddParam(empleo.Periodo)
                    .AddParam(empleo.SituacionLaboral)
                    .AddParam(empleo.ActividadTrabajador)
                    .AddParam(empleo.Provincia)
                    .AddParam(empleo.Monto?.ToString() ?? "")
                    .AddParam(empleo.BaseOrigen)
                    .ToSpResult();
            }
        }

        public void RegistrarFallecimiento(Fallecido fallecido, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            if (fallecido != null)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_FALLECIDO_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(fallecido.FechaFallecimiento)
                    .AddParam(fallecido.Acta)
                    .AddParam(fallecido.Tomo)
                    .AddParam(fallecido.Folio)
                    .AddParam(fallecido.AnioActa)
                    .AddParam(fallecido.BaseOrigen)
                    .ToSpResult();
            }
        }

        public void RegistrarPensionJubilacion(List<PensionJubilacion> pensiones, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            foreach (var pension in pensiones)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_JUBILAC_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(pension.TipoBeneficio)
                    .AddParam(pension.DescripcionBeneficio)
                    .AddParam(pension.FechaAlta)
                    .AddParam(pension.Monto)
                    .AddParam(pension.Periodo)
                    .AddParam(pension.BaseOrigen)
                    .ToSpResult();
            }
        }

        public void RegistrarPensionNoContributiva(List<PensionNoContributiva> pensiones, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            foreach (var pension in pensiones)
            {
                var idDetalle = RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
                Execute("PR_REGISTRA_PENSIONES_SINTYS")
                    .AddParam(idDetalle)
                    .AddParam(pension.TipoBeneficio)
                    .AddParam(pension.DescripcionBeneficio)
                    .AddParam(pension.FechaAlta)
                    .AddParam(pension.Periodo)
                    .AddParam(pension.BaseOrigen)
                    .ToSpResult();
            }
        }

        public decimal RegistrarDetalleDatoSintys(IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {
            var resultado = Execute("PR_REGISTRA_DET_DATOS_SINTYS")
                .AddParam(idCabecera)
                .AddParam(integrante.Sexo.IdSexo)
                .AddParam(integrante.NroDocumento)
                .AddParam(integrante.PaisTD.IdPais)
                .AddParam(integrante.Id_Numero)
                .AddParam(delSolicitante)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public CabeceraHistorialSintys ObtenerCabeceraHistorialSintys(decimal idHistorial)
        {
            return (CabeceraHistorialSintys)
                Execute("PR_OBTENER_DATOS_SINTYS_CAB")
                    .AddParam(idHistorial)
                    .ToUniqueResult<CabeceraHistorialSintys>();
        }

        public List<HistorialSintys> ObtenerHistorialSintys(decimal idHistorial)
        {
            return (List<HistorialSintys>)
                Execute("PR_OBTENER_DET_DATOS_SINTYS")
                    .AddParam(idHistorial)
                    .ToListResult<HistorialSintys>();
        }

        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesSintys(DocumentacionConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_DATOS_SINTYS")
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<DocumentacionResultado>();
            return CrearResultado(consulta, elementos);
        }

    }
}