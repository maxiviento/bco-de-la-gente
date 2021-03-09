using System;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;

namespace Soporte.Aplicacion.Servicios
{
    public class DocumentacionBGEUtilServicio
    {
        public ArchivoBase64 GenerarArchivoReporteBGE(byte[] buffer, TipoArchivo tipo, string nombreInforme, Persona solicitante, int nroFormulario)
        {
            tipo = tipo ?? TipoArchivo.PDF;
            nombreInforme = nombreInforme ?? "Reporte";
            return new ArchivoBase64(Convert.ToBase64String(buffer), tipo, GenerarNombreArchivoBGE(nombreInforme, solicitante, nroFormulario));
        }

        public ArchivoBase64 GenerarArchivoReporteBGE(byte[] buffer, TipoArchivo tipo, string nombreInforme, int? idPrestamo = null, int? idLote = null, int? nroFormulario = null)
        {
            tipo = tipo ?? TipoArchivo.PDF;
            nombreInforme = nombreInforme ?? "Reporte";
            return new ArchivoBase64(Convert.ToBase64String(buffer), tipo, GenerarNombreArchivoBGE(nombreInforme, idPrestamo, idLote, nroFormulario));
        }

        public static string GenerarNombreArchivoBGE(string nombreInforme, Persona solicitante, int nroFormulario)
        {
            return $"{nombreInforme}_{nroFormulario}_{solicitante.Apellido}{solicitante.Nombre}";
        }

        public static string GenerarNombreArchivoBGE(string nombreInforme, int? idPrestamo = null, int? idLote = null, int? nroFormulario = null)
        {
            if (idPrestamo == -1)
            {
                return $"{nombreInforme}";
            }
            return $"{nombreInforme}_{GenerarIdentifiacionInformeBGE(idPrestamo, idLote, nroFormulario)}";
        }

        private static string GenerarIdentifiacionInformeBGE(int? idPrestamo, int? idLote, int? nroFormulario)
        {
            if (idPrestamo.HasValue)
            { return $"Prestamo={idPrestamo}"; }
            else if (idLote.HasValue)
            { return $"Lote={idLote}"; }
            else if (nroFormulario.HasValue)
            { return $"Formulario={nroFormulario}"; }
            return "XXXX";
        }
    }
}
