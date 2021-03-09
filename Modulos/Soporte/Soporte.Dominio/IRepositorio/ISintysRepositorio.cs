using System;
using System.Collections.Generic;
using AppComunicacion.ApiModels;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using SintysWS.Modelo;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo;
using Domicilio = SintysWS.Modelo.Domicilio;

namespace Soporte.Dominio.IRepositorio
{
    public interface ISintysRepositorio : IRepositorio<DatoSintys>
    {
        void RegistrarDomicilio(List<Domicilio> domicilios, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        void RegistrarFallecimiento(Fallecido fallecido, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        void RegistrarEmpleoPresunto(List<EmpleoPresunto> empleos, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        decimal RegistrarDetalleDatoSintys(IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        void RegistrarEmpleoFormal(List<EmpleoFormal> empleos, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        void RegistrarPensionJubilacion(List<PensionJubilacion> pensiones, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        void RegistrarPensionNoContributiva(List<PensionNoContributiva> pensiones, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        void RegistrarDesempleo(Desempleo desempleo, IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante);
        List<HistorialSintys> ObtenerHistorialSintys(decimal idHistorial);
        decimal RegistrarCabeceraDatosSintys(string nroPrestamo, Id id, DateTime fechaActual, decimal idFormularioLinea);
        CabeceraHistorialSintys ObtenerCabeceraHistorialSintys(decimal idHistorial);
        Resultado<DocumentacionResultado> ObtenerTodosHistorialesSintys(DocumentacionConsulta consulta);
    }
}