using System;
using ApiBatch.Base;
using ApiBatch.Utilidades;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Infraestructura.Core.Reportes;
using Infraestructura.Core.Reportes.BarCode;
using Pagos.Aplicacion.Consultas.Resultados;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ApiBatch.GeneradoresArchivos
{
    public class ArchivoCuponera : GeneradorArchivos<Cuponera>
    {
        ConcatenadorPDF concatenador = new ConcatenadorPDF();

        public ArchivoCuponera()
        {
        }

        public ArchivoCuponera(IList<Cuponera> datos) : base(datos)
        {
        }

        public override IList<HttpFile> DefinirArchivo(string processName)
        {
            var listadoNroFormularios = ObtenerListadoFormularios();

            foreach (var nroFormulario in listadoNroFormularios)
            {
                GenerarReportes(nroFormulario);
            }

            var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();
            
            var file = new HttpFile()
            {
                FileName = processName + " " + DateTime.Now.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("HH") + "hs" + " " + DateTime.Now.ToString("mm") + "min",
                MediaType = MimeTypeUtils.Obtener_MimeType(MimeTypeUtils.FileExtension.Pdf),
                BufferArray = arrayBytesConcatenado
            };
            return new List<HttpFile>() {file};
        }

        private void GenerarReportes(int nroFormulario)
        {
            var dsCuponera = new List<ReporteCuponeraResultado>();
            foreach (var cuota in Datos)
            {
                if (cuota.NroFormulario == nroFormulario)
                {
                    int numPrestamo = cuota.NroFormulario;
                    string numeroPrestamo = numPrestamo.ToString("D8");
                    string numReferencia = numPrestamo.ToString("D13") + cuota.NroCuota.ToString("D3");

                    CodigoBarras codigoBarras = GenerarCodigoBarrasCuponera(numPrestamo, cuota.NroCuota,
                        cuota.VencimientoCuota, Decimal.Parse(cuota.MontoCuota));
                    dsCuponera.Add(new ReporteCuponeraResultado
                    {
                        NumeroPrestamo = numeroPrestamo,
                        Beneficiario = cuota.NombreCompletoSolicitante,
                        NumeroDocumento = cuota.NroDocumentoSolicitante,
                        LineaPrestamo = cuota.NombreLinea,
                        NumeroReferencia = numReferencia,
                        NumeroCuota = cuota.NroCuota.ToString(),
                        ImporteCuota = cuota.MontoCuota,
                        CodigoBarras = codigoBarras.GetBarCodeInBase64(),
                        DataCB = codigoBarras.Data,
                        FechaVencimiento = cuota.VencimientoCuota.ToString("dd/MM/yy")
                    });
                }
            }

            // ARMADO DEL REPORTE
            var reportData = new ReporteBuilder("ReportePagos_Cuponera.rdlc")
                .AgregarDataSource("DSReporteCuponera", dsCuponera)
                .Generar();

            concatenador.AgregarReporte(reportData, "Cuponera_" + nroFormulario);
        }

        #region Obtencion listado de nros formulario

        private List<int> ObtenerListadoFormularios()
        {
            return Datos.Select(res => res.NroFormulario).ToList().Distinct().ToList(); ;
        }

        #endregion

        #region Codigo de barras cuponera
        private CodigoBarras GenerarCodigoBarrasCuponera(int numeroPrestamo, int cuota, DateTime fechaVencimiento, decimal importe)
        {
            int importeEntero = 0;
            int importeDecimal = 0;
            string importeString = importe.ToString();

            importeEntero = int.Parse(importeString.Split(',')[0]);
            if (importeString.Contains(','))
            {
                importeDecimal = int.Parse(importeString.Split(',')[1]);
            }

            int convenio = 184;
            int grupo = 2;
            string fechaVen = fechaVencimiento.ToString("ddMMyy");

            StringBuilder data = new StringBuilder();
            data.Append(convenio.ToString("D4")) //convenio
                .Append(grupo.ToString("D2")) //grupo ?? => 02
                .Append(numeroPrestamo.ToString("D13"))
                .Append(cuota.ToString("D3"))
                .Append("200101") //vencimiento
                .Append(importeEntero.ToString("D7"))
                .Append(importeDecimal.ToString().PadRight(14, '0'));
            string verificador = CodigoVerificadorCodigoBarras(data.ToString());
            data.Append(verificador); //codigo verificador
            return new CodigoBarras(data.ToString());
        }

        private string CodigoVerificadorCodigoBarras(string codigo)
        {
            string vl_verif = "9713971397139713971397139713971397139713971397139713";
            int vl_1;
            int vl_2 = 0;
            int xl = codigo.Length;
            for (int c = 1; c < xl; c++)
            {
                vl_1 = int.Parse(codigo.Substring(c, 1)) * int.Parse(vl_verif.Substring(c, 1));
                vl_2 += vl_1;
            }
            string vl_t = STR(vl_2, 5, 0).Trim();
            vl_t = vl_t.Substring(vl_t.Length - 1, 1);

            string codigoVerificador = STR(10 - int.Parse(vl_t), 2, 0).Trim();
            return codigoVerificador.Substring(codigoVerificador.Length - 1, 1);
        }

        private static string STR(double d, int totalLen, int decimalPlaces)
        {
            int floor = (int)Math.Floor(d);
            int length = floor.ToString().Length;
            if (length > totalLen)
                throw new NotImplementedException();
            if (totalLen - length < decimalPlaces)
                decimalPlaces = totalLen - length;
            if (decimalPlaces < 0)
                decimalPlaces = 0;
            string str = Math.Round(d, decimalPlaces).ToString();
            if (str.StartsWith("0") && str.Length > 1 && totalLen - decimalPlaces - 1 <= 0)
                str = str.Remove(0, 1);

            return str.Substring(0, str.Length >= totalLen ? totalLen : str.Length);
        }

        #endregion
    }
}