using System.Collections.Generic;
using System.Diagnostics;
using Infraestructura.Core.Comun.Excepciones;
using SintysWS.Modelo;
using SintysWS.Utils;

namespace SintysWS
{
    public class SintysServicioWs
    {
        public List<PersonaFisica> ObtenerPersonaFisica(string documentoPersona, string sexo)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetPersonaFisica)
                .AddFiltro(ParametrosEntrada.NumeroDocumento, documentoPersona)
                .AddFiltro(ParametrosEntrada.Sexo, sexo)
                .Ejecutar<List<PersonaFisica>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<PersonaFisica> ObtenerPersonaIdentificada(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetPersonaIdentificada)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<PersonaFisica>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<Domicilio> ObtenerDomicilio(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetDomicilio)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<Domicilio>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<Fallecido> ObtenerFallecido(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetFallecido)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<Fallecido>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<PensionJubilacion> ObtenerJubilacionPension(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetJubilacionYPensionConMonto)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<PensionJubilacion>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<PensionNoContributiva> ObtenerPensionNoContributiva(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetPensionNoContributiva)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<PensionNoContributiva>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<EmpleoPresunto> ObtenerEmpleoPresunto(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetEmpleoPresunto)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<EmpleoPresunto>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<Desempleo> ObtenerDesempleos(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetDesempleo)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<Desempleo>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }

        public List<EmpleoFormal> ObtenerEmpleoFormalConMonto(string idPersona)
        {
            var response = new SintysRequest(SintysRequest.Operaciones.GetEmpleoFormalConMonto)
                .AddParametro(ParametrosEntrada.Id, idPersona)
                .Ejecutar<List<EmpleoFormal>>();

            if (response.Ok)
            {
                return response.Resultado;
            }

            throw new ErrorTecnicoException(response.Error);
        }
    }
}