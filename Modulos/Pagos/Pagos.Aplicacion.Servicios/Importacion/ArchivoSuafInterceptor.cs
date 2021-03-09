using System;
using System.Text.RegularExpressions;
using Pagos.Dominio.Modelo;
using Utilidades.Importador;

namespace Pagos.Aplicacion.Servicios.Importacion
{
    public class ArchivoSuafInterceptor : IImporterInterceptor<ImportarArchivoSuaf>
    {
        private const string Patron = "(\\b[0-9][0-9][0-9][0-9]\\/[0-9][0-9][0-9][0-9][0-9][0-9]\\b)";

        public bool Pre(int rowIndex, string[] columns)
        {
            throw new NotImplementedException();
        }

        public bool Pos(int rowIndex, ref ImportarArchivoSuaf archivoSuaf)
        {
            throw new NotImplementedException();
        }

        public int ValidarDevengadoSuaf(int rowIndex, ref ImportarArchivoSuaf archivoSuaf)
        {
            try
            {
                archivoSuaf.FechaDevengado = DateTime.FromOADate(long.Parse(archivoSuaf.FechaDevengadoString));
                archivoSuaf.Devengado = ObtenerDevengado(archivoSuaf.Devengado);

                return !string.IsNullOrEmpty(archivoSuaf.Devengado) && archivoSuaf.FechaDevengado <= DateTime.Today ? 1 : 2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 3;
            }
        }

        private static string ObtenerDevengado(string registroExcel)
        {
            var match = Regex.Match(registroExcel, Patron);
            var devengado = match.Value;
            return devengado;
        }
    }
}