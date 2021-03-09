using System;
using ApiBatch.Base;
using ApiBatch.Utilidades;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Infraestructura.Core.Reportes;
using Pagos.Aplicacion.Consultas.Resultados;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ApiBatch.GeneradoresArchivos
{
    public class ArchivoDocumentacionPago : GeneradorArchivos<DocumentacionPago>
    {
        private List<Caratula> listadoCaratula = new List<Caratula>();
        private List<ContratoMutuo> listadoContrato = new List<ContratoMutuo>();
        private List<Providencia> listadoProvidencia = new List<Providencia>();
        private List<Recibo> listadoRecibo = new List<Recibo>();
        private List<Pagare> listadoPagare = new List<Pagare>();
        private bool tieneCaratula = false;
        private bool tieneContrato = false;
        private bool tieneProvidencia = false;
        private bool tieneRecibo = false;
        private bool tienePagare = false;
        ConcatenadorPDF concatenador = new ConcatenadorPDF();

        public ArchivoDocumentacionPago()
        {
        }

        public ArchivoDocumentacionPago(IList<DocumentacionPago> datos) : base(datos)
        {
        }

        public override IList<HttpFile> DefinirArchivo(string processName)
        {
            //Metodo para dividir los datos en las diferentes listas
            DividirDatos();

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

        private void GenerarReportes(string nroFormulario)
        {
            var subreportesPagos = SubReporte.GetAllPagos().OrderBy(x => x.IdCuadrante.Valor);
            var subReportesOrdenados = new List<SubReporte>();
            var reporteContratoMutuo = new Reporte();

            // AGREGADO EN ORDEN DE LOS SUBREPORTES
            if (tieneContrato)
            {
                var contratoMutuoContent = GenerarReporteContratoMutuo(nroFormulario);
                reporteContratoMutuo = new Reporte(contratoMutuoContent, "pdf", "application/pdf");
            }

            foreach (var subreporte in subreportesPagos)
            {
                var subRep = ObtenerSubreporteDocumentacionPagos((int) subreporte.IdCuadrante.Valor, nroFormulario);
                if (subRep != null)
                    subReportesOrdenados.Add(subRep);
            }

            // ARMADO DEL REPORTE
            var reportData = new ReporteBuilder("ReportePagos.rdlc", "SubReportePagos")
                .SubReporteDS(subReportesOrdenados)
                .GenerarConSubReporte(false);

            if (subReportesOrdenados.Count > 0)
            {
                concatenador.AgregarReporte(reportData, "Pagos_" + nroFormulario);
            }

            if (tieneContrato)
            {
                concatenador.AgregarReporte(reporteContratoMutuo, "Pagos_" + nroFormulario);
            }
        }

        #region Division de listas

        private void DividirDatos()
        {
            //Listado con registros de caratula
            listadoCaratula = Datos.Where(x =>
            {
                if (x.IdTipoProceso == "9")
                {
                    tieneCaratula = true;
                    return true;
                }

                return false;
            }).ToList().Select(
                caratula =>
                {
                    var res = new Caratula
                    {
                        NroFormulario = caratula.NroFormulario,
                        FechaInicioPagos = caratula.FechaInicioPagos,
                        NroDocumentoSolicitante = caratula.NroDocumentoSolicitante,
                        NombreCompletoSolicitante = caratula.NombreCompletoSolicitante,
                        ValorPrestamo = caratula.ValorPrestamo,
                        DescripcionLinea = caratula.DescripcionLinea,
                        DomicilioCompletoSolicitante = caratula.DomicilioCompletoSolicitante,
                        NombreGarante = caratula.NombreGarante,
                        TelefonoSolicitante = caratula.TelefonoSolicitante,
                        TelefonoGarante = caratula.TelefonoGarante,
                        NombreSucursal = caratula.NombreSucursal,
                        IdSucursal = caratula.IdSucursal,
                        Localidad = caratula.Localidad,
                        Departamento = caratula.Departamento
                    };
                    Datos.Remove(caratula);
                    return res;
                }).ToList();

            //Listado con registros de providencia
            listadoProvidencia = Datos.Where(x =>
            {
                if (x.IdTipoProceso == "10")
                {
                    tieneProvidencia = true;
                    return true;
                }

                return false;
            }).ToList().Select(
                providencia =>
                {
                    var res = new Providencia
                    {
                        Destino = providencia.Destino,
                        NombreLinea = providencia.NombreLinea,
                        DescripcionLinea = providencia.DescripcionLinea,
                        ValorPrestamo = providencia.ValorPrestamo,
                        Localidad = providencia.Localidad,
                        Departamento = providencia.Departamento,
                        NroSticker = providencia.NroSticker,
                        NroFormulario = providencia.NroFormulario,
                        NombreCompletoSolicitante = providencia.NombreCompletoSolicitante,
                        Cuil = providencia.Cuil,
                        Fecha = providencia.Fecha
                    };
                    Datos.Remove(providencia);
                    return res;
                }).ToList();

            //Listado con registros de contrato mutuo
            listadoContrato = Datos.Where(x =>
            {
                if (x.IdTipoProceso == "11")
                {
                    tieneContrato = true;
                    return true;
                }

                return false;
            }).ToList().Select(
                contrato =>
                {
                    var res = new ContratoMutuo
                    {
                        CantidadFormularios = contrato.CantidadFormularios,
                        DatosSolicitantes = contrato.DatosSolicitantes,
                        DatosGarantes = contrato.DatosGarante,
                        NroFormulario = contrato.NroFormulario,
                        Fecha = contrato.Fecha,
                        NombreCompletoSolicitante = contrato.NombreCompletoSolicitante,
                        NroDocumentoSolicitante = contrato.NroDocumentoSolicitante,
                        Cuil = contrato.Cuil,
                        ValorPrestamo = contrato.ValorPrestamo,
                        DomicilioCompletoSolicitante = contrato.DomicilioCompletoSolicitante,
                        CantidadCuotas = contrato.CantidadCuotas,
                        MontoCuota = contrato.MontoCuota,
                        FechaPrimerVencimientoPago = contrato.FechaPrimerVencimientoPago,
                        EstadoCivilSolicitante = contrato.EstadoCivilSolicitante,
                        NombreLinea = contrato.NombreLinea,
                        DescripcionLinea = contrato.DescripcionLinea,
                        Destino = contrato.Destino
                    };
                    Datos.Remove(contrato);
                    return res;
                }).ToList();

            //Listado con registros de recibo
            listadoRecibo = Datos.Where(x =>
            {
                if (x.IdTipoProceso == "12")
                {
                    tieneRecibo = true;
                    return true;
                }

                return false;
            }).ToList().Select(
                recibo =>
                {
                    var res = new Recibo
                    {
                        NroFormulario = recibo.NroFormulario,
                        NombreCompletoSolicitante = recibo.NombreCompletoSolicitante,
                        NroDocumentoSolicitante = recibo.NroDocumentoSolicitante,
                        ValorPrestamo = recibo.ValorPrestamo,
                        DomicilioCompletoSolicitante = recibo.DomicilioCompletoSolicitante,
                        DescripcionLinea = recibo.DescripcionLinea,
                        Fecha = recibo.Fecha
                    };
                    Datos.Remove(recibo);
                    return res;
                }).ToList();

            //Listado con registros de pagare
            listadoPagare = Datos.Where(x =>
            {
                if (x.IdTipoProceso == "13")
                {
                    tienePagare = true;
                    return true;
                }

                return false;
            }).ToList().Select(
                pagare =>
                {
                    var res = new Pagare
                    {
                        NroFormulario = pagare.NroFormulario,
                        NombreCompletoSolicitante = pagare.NombreCompletoSolicitante,
                        NroDocumentoSolicitante = pagare.NroDocumentoSolicitante,
                        ValorPrestamo = pagare.ValorPrestamo,
                        DomicilioCompletoSolicitante = pagare.DomicilioCompletoSolicitante,
                        FechaVencimientoPlanPago = pagare.FechaVencimientoPlanPago,
                        DomicilioSucursalBancaria = pagare.DomicilioSucursalBancaria,
                        Fecha = pagare.Fecha
                    };
                    Datos.Remove(pagare);
                    return res;
                }).ToList();
        }

        #endregion

        #region Reporte contrato mutuo

        private byte[] GenerarReporteContratoMutuo(string nroFormulario)
        {
            var contratoMutuo = listadoContrato.FirstOrDefault(res => res.NroFormulario == nroFormulario);

            var mes = new CultureInfo("es-ES", false).DateTimeFormat.GetMonthName(contratoMutuo.Fecha.Month);
            string fechaString = $"{contratoMutuo.Fecha.Day} de {mes} de {contratoMutuo.Fecha.Year}";
            var dsContratoMutuo =
                new ReporteContratoMutuoResultado
                {
                    CantidadFormularios = contratoMutuo.CantidadFormularios.ToString(),
                    DatosSolicitantes = contratoMutuo.DatosSolicitantes,
                    NumeroFormulario = contratoMutuo.NroFormulario,
                    Fecha = fechaString,
                    SolicitanteNombre = contratoMutuo.NombreCompletoSolicitante,
                    SolicitanteDocumento = contratoMutuo.NroDocumentoSolicitante,
                    SolicitanteCuil = contratoMutuo.Cuil,
                    ValorPrestamo = contratoMutuo.ValorPrestamo,
                    SolicitanteDomicilioCompleto = contratoMutuo.DomicilioCompletoSolicitante,
                    ValorPrestamoString =
                        ConvertidorNumeroLetras.enletras(
                            contratoMutuo.ValorPrestamo),
                    FechaPrimerVencimientoPago = contratoMutuo.FechaPrimerVencimientoPago?.ToString("dd/MM/yyyy"),
                    FechaAnioLetras = ConvertidorNumeroLetras.enletras(contratoMutuo.Fecha.Year.ToString()),
                    FechaMesLetras = mes,
                    FechaDiaLetras = ConvertidorNumeroLetras.enletras(contratoMutuo.Fecha.Day.ToString()),
                    MontoCuota = contratoMutuo.MontoCuota != null ? contratoMutuo.MontoCuota : "-",
                    MontoCuotaString = contratoMutuo.MontoCuota != null
                        ? ConvertidorNumeroLetras.enletras(contratoMutuo.MontoCuota)
                        : "-",

                    Cuotas = contratoMutuo.CantidadCuotas != null ? contratoMutuo.CantidadCuotas : "-",
                    CuotasString = contratoMutuo.CantidadCuotas != null
                        ? ConvertidorNumeroLetras.enletras(contratoMutuo.CantidadCuotas)
                        : "-",
                    ListadoGarantes = contratoMutuo.DatosGarantes,
                    SolicitanteEstadoCivil = contratoMutuo.EstadoCivilSolicitante,
                    DescripcionLinea = contratoMutuo.DescripcionLinea,
                    NroLinea = contratoMutuo.NombreLinea,
                    Destino = contratoMutuo.Destino
                };

            var reporte = new ReporteBuilder("ReportePagos_ContratoMutuo.rdlc")
                .AgregarDataSource("DSContratoMutuo", dsContratoMutuo)
                .Generar();

            Stream streamReporte = new MemoryStream(reporte.Content);
            PdfDocument inputDocument = PdfReader.Open(streamReporte, PdfDocumentOpenMode.Import);
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage pdfPage = pdfDocument.AddPage(inputDocument.Pages[0]);
            XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
            XTextFormatter tf = new XTextFormatter(gfx);
            XFont font;

            var textoContrato = "Contrato de Mutuo N°: " + dsContratoMutuo.NumeroFormulario;
            var textoFinal =
                "CERTIFICO: que los datos y la firma consignados en el presente son verdaderas y corresponden a las personas firmantes.";
            if (contratoMutuo.CantidadFormularios > 1)
            {
                #region Contrato Mutuo Asociativo

                StringBuilder textoAsociativa = new StringBuilder();
                textoAsociativa.Append(
                    $"Ciudad de Córdoba,  {dsContratoMutuo.FechaDiaLetras}  día/s del mes de {dsContratoMutuo.FechaMesLetras} del año {dsContratoMutuo.FechaAnioLetras}, comparece " +
                    $"ante el funcionario actuante los {dsContratoMutuo.DatosSolicitantes}todos de la Provincia de Córdoba, en su carácter de beneficiarios, del programa Banco de " +
                    $"la Gente, N° de Solicitud {dsContratoMutuo.NumeroFormulario} cuya ejecución se encuentra a cargo de la Secretaría de Equidad y Promoción del Empleo, por el " +
                    $"cual percibe una AYUDA ECONÓMICA REINTEGRABLE, acordado mediante PROVIDENCIA N° {dsContratoMutuo.NumeroFormulario} de la Sra Secretaria de Equidad y Promoción del " +
                    $"Empleo, en el marco de las disposiciones contenidas en el Decreto 1791/15 y mod. Por Decreto 39/16. Art. 39. Inc. 14-. ratificado por ley N° 10337 y Decreto " +
                    $"186/2016, resolución 133/2016 y resolución 170/2019 por la suma de pesos {dsContratoMutuo.ValorPrestamoString} (${dsContratoMutuo.ValorPrestamo}), " +
                    $"el que deberá ser:  {dsContratoMutuo.NroLinea} - {dsContratoMutuo.DescripcionLinea} destinado a CONSUMO. Los beneficiarios en este acto manifiestan conocer " +
                    $"cabalmente los términos, condiciones y normativa del programa del cual resultan beneficiarios. Asimismo reconoce que la ayuda económico recibida" +
                    $" por el presente beneficio es de carácter “REINTEGRABLE”, asumiendo ante el superior Gobierno de la Provincia de Córdoba la responsabilidad de " +
                    $"efectivizar el reintegro de la ayuda económica de la que resulta/n beneficiario/s en {dsContratoMutuo.CuotasString} ({dsContratoMutuo.Cuotas}) cuotas consecutivas de pesos " +
                    $"{dsContratoMutuo.MontoCuotaString} (${dsContratoMutuo.MontoCuota}) cada una de ellas, las que deberán ser abonadas del (1) al (10) de cada mes, " +
                    $"mediante la cuponera de pagos que en este acto reciben, las que podrán ser abonadas en cualquiera de las sucursales del Banco Provincia de Córdoba S.A. " +
                    $"cuya primera cuota en consecuencia vence el {dsContratoMutuo.FechaPrimerVencimientoPago}. Los beneficiarios suscriben y se constituyen en carácter de " +
                    $"cotitulares y codeudores solidarios de las obligaciones que surgen del presente un PAGARÉ SIN PROTESTO a nombre del SUPERIOR GOBIERNO DE LA PROVINCIA " +
                    $"DE CÓRDOBA por igual monto que el beneficio acordado y con vencimiento idéntico al de la última cuota de devolución pactada para el presente beneficio. " +
                    $"La falta de pago en tiempo y forma de cualquiera de las cuotas previstas para el presente beneficio hará incurrir los mismos en mora automática, haciéndose " +
                    $"pasible de la aplicación de los intereses oratorios y punitorios correspondientes. Así mismo declaran conocer que la falta de pago de dos cuotas en " +
                    $"forma consecutiva, facultará al Superior Gobierno de la Provincia de Córdoba, sin necesidad de interpelación judicial o extrajudicial alguna a " +
                    $"solicitar la devolución del importe total entregado más los intereses que se devenguen por tal mora, ya sea por la vía administrativa y/o " +
                    $"judicial correspondiente y dar por caducados los plazos de las cuotas adeudadas, pudiendo reclamar el pago total de ellas como si fuesen " +
                    $"vencidas y exigibles, mediante el ejercicio de las acciones legales conducentes. Los beneficiarios asumen el compromiso de presentar la " +
                    $"cuponera de pagos, con la constancia de la cancelación total de la deuda, dentro de los 15 días del pago de la última cuota pactada. " +
                    $"Las manifestaciones realizadas por los beneficiarios revisten el CARÁCTER DE DECLARACIÓN JURADA. Por el presente los beneficiarios, " +
                    $"se constituyen en lisos y llanos pagadores de todas las obligaciones asumidas en el presente contrato, durante la duración del mismo y " +
                    $"aún después de vencido, hasta su total cumplimiento firmado de plena conformidad el presente instrumento. Los codeudores solidarios acuerdan " +
                    $"en designar como representante del grupo para intervenir solamente en el cobro del crédito objeto del presente contrato ante el Banco de la " +
                    $"Provincia de Córdoba al  {dsContratoMutuo.SolicitanteNombre} DNI:{dsContratoMutuo.SolicitanteDocumento}, con domicilio en calle " +
                    $"{dsContratoMutuo.SolicitanteDomicilioCompleto}. A todos los efectos legales, las partes se someten expresamente –ya sea para la interpretación " +
                    $"y cumplimiento de este contrato- a las leyes y tribunales de la Ciudad de Córdoba, renunciando a cualquier otra normativa o tribunal que por " +
                    $"razones de domicilio u otras circunstancias pudieran invocar las partes, constituyendo domicilio en los lugares arriba indicados, donde se " +
                    $"considerarán válidas todas las notificaciones y emplazamientos judiciales o extrajudiciales que se hagan. Se firman de plena conformidad dos " +
                    $"ejemplares de un mismo e idéntico tenor, en el lugar y fecha indicados supra. Ante el funcionario actuante que firma y da pie de lo actuado.");

                //Primer Titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoContrato, font, XBrushes.Black,
                    new XRect(60, 70, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);
                //Segundo titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString("AYUDA ECONÓMICA REINTEGRABLE", font, XBrushes.Black,
                    new XRect(60, 95, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);
                //Cuerpo
                tf.Alignment = XParagraphAlignment.Justify;
                font = new XFont("Calibri", 9);
                tf.DrawString(textoAsociativa.ToString(), font, XBrushes.Black,
                    new XRect(60, 115, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Texto Firma
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoFinal, font, XBrushes.Black,
                    new XRect(40, pdfPage.Height.Point - 50, pdfPage.Width.Point - 80, pdfPage.Height.Point - 10),
                    XStringFormats.TopLeft);

                #endregion
            }
            else
            {
                #region Contrato Mutuo Individual

                StringBuilder textoIndividual = new StringBuilder();
                textoIndividual.Append(
                    $"En la ciudad de Córdoba, a los {dsContratoMutuo.FechaDiaLetras} día/s del mes de {dsContratoMutuo.FechaMesLetras}" +
                    $" del año {dsContratoMutuo.FechaAnioLetras} comparece ante el funcionario actuante el Sr./Sra {dsContratoMutuo.SolicitanteNombre}" +
                    $" DNI {dsContratoMutuo.SolicitanteDocumento} (CUIL {dsContratoMutuo.SolicitanteCuil}), con domicilio real en calle {dsContratoMutuo.SolicitanteDomicilioCompleto}" +
                    $", Estado Civil {dsContratoMutuo.SolicitanteEstadoCivil}, en su carácter del programa Banco de la gente, N° {dsContratoMutuo.NumeroFormulario}" +
                    $", cuya ejecución se encuentra a cargo del Ministerio de Promoción del Empleo y de la Economía Familiar, por el cual percibe una AYUDA " +
                    $"ECONOMICA REINTEGRABLE, acordando mediante PROVIDENCIA {dsContratoMutuo.NumeroFormulario}, de la Ministra de Promoción del Empleo y de la Economía Familiar" +
                    $", en el marco de las disposiciones contenidas en el artículo 27°, incs. 14), 15) y 20) del Decreto 1615/19 39/16" +
                    $", por la suma de pesos {dsContratoMutuo.ValorPrestamoString}" +
                    $" (${dsContratoMutuo.ValorPrestamo}), el que deberá ser: ({dsContratoMutuo.NroLinea}) {dsContratoMutuo.DescripcionLinea}" +
                    $" destinado a {dsContratoMutuo.Destino}. EL beneficiario en este acto manifiesta conocer cabalmente los términos, condiciones y normativa del programa del " +
                    $"cual resulta beneficiario. Asimismo reconoce que la ayuda económica recibida por el presente beneficio es de carácter \"REINTEGRABLE\", " +
                    $"asumiendo ante el superior Gobierno de la Provincia de Córdoba la responsabilidad de efectivizar el reintegro de la ayuda económica de la " +
                    $"que resulta beneficiaria en {dsContratoMutuo.CuotasString} ({dsContratoMutuo.Cuotas}) cuotas consecutivas de pesos {dsContratoMutuo.MontoCuotaString} " +
                    $"(${dsContratoMutuo.MontoCuota}) cada una de ellas, las que deberán ser abonadas del (1) al (10) de cada mes, mediante la cuponera " +
                    $"de pagos que en este acto recibe, las que podrán ser abonadas en cualquiera de las sucursales del Banco Provincia de Córdoba S.A. cuya primera" +
                    $" cuota en consecuencia vence el {dsContratoMutuo.FechaPrimerVencimientoPago}. El beneficiario/a suscribe en garantía de las obligaciones que surgen del " +
                    $"presente un PAGARE SIN PROTESTO a nombre del SUPERIOR GOBIERNO DE LA PROVINCIA DE CORDOBA por igual monto que el beneficio acordado, y " +
                    $"con vencimiento idéntico al de la última cuota de devolución pactada para el presente beneficio. La falta de pago en tiempo y forma de " +
                    $"cualquiera de las cuotas previstas para el presente beneficio hará incurrir al mismo en mora automática, haciéndose pasible de la " +
                    $"aplicación de los intereses moratorios y punitorios correspondientes. Asimismo declara conocer que la falta de pago de dos cuotas en " +
                    $"forma consecutiva, facultará al Superior Gobierno de la Provincia de Córdoba, sin necesidad de interpelación judicial o extrajudicial " +
                    $"alguna, a solicitar la devolución del importe total entregado más los intereses que se devenguen por tal mora, ya sea por la vía " +
                    $"administrativa y/o judicial correspondiente y dar por caducados los plazos de las cuotas adeudadas, pudiendo reclamar el pago total de " +
                    $"ellas como si fuesen vencidas y exigibles, mediante el ejercicio de las acciones legales conducentes. Las manifestaciones hechas por el " +
                    $"beneficiario revisten el CARACTER DE DECLARACIÓN JURADA. A solicitud del beneficiario/a, el {dsContratoMutuo.ListadoGarantes} se " +
                    $"constituye en fiador/es solidarios, liso y llano pagador de todas las obligaciones asumidas por el beneficiario que surgen del " +
                    $"presente contrato durante la duración del mismo y aún después de vencido, hasta su total cumplimiento, haciéndose expresa renuncia a " +
                    $"los beneficios de excusión y división de deuda, firmando de plena conformidad del presente instrumento. A todos los efectos legales, " +
                    $"las partes y el fiador, se someten expresamente -ya sea para la interpretación y cumplimiento de este contrato- a las leyes y tribunales " +
                    $"de la Ciudad de Córdoba, renunciando a cualquier otra normativa o tribunal que por razones de domicilio u otras circunstancias pudieran " +
                    $"invocar las partes, constituyendo domicilio en los lugares arriba indicados, donde se considerarán válidas todas las notificaciones y " +
                    $"emplazamientos judiciales o extrajudiciales que se hagan. Se firman de plena conformidad dos ejemplares de un mismo e idéntico tenor, en " +
                    $"el lugar y fecha indicados supra, ante el funcionario actuante que firma y da pie de lo actuado.");

                //Primer Titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoContrato, font, XBrushes.Black,
                    new XRect(60, 70, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Segundo titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString("\"AYUDA ECONÓMICA REINTEGRABLE\"", font, XBrushes.Black,
                    new XRect(60, 95, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Cuerpo
                tf.Alignment = XParagraphAlignment.Justify;
                font = new XFont("Calibri", 10);
                tf.DrawString(textoIndividual.ToString(), font, XBrushes.Black,
                    new XRect(60, 115, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Texto Firma
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoFinal, font, XBrushes.Black,
                    new XRect(40, pdfPage.Height.Point - 45, pdfPage.Width.Point - 80, pdfPage.Height.Point - 10),
                    XStringFormats.TopLeft);

                #endregion
            }

            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            byte[] docBytes = stream.ToArray();

            listadoContrato.Remove(contratoMutuo);

            return docBytes;
        }

        #endregion

        #region Obtencion listado de nros formulario

        private List<string> ObtenerListadoFormularios()
        {
            //Obtengo listado de formularios con cualquier lista no vacia (todas tienen los mismos formularios)
            if (listadoCaratula.Count > 0)
            {
                return listadoCaratula.Select(res => string.Format(res.NroFormulario)).ToList();
            }

            if (listadoProvidencia.Count > 0)
            {
                return listadoProvidencia.Select(res => string.Format(res.NroFormulario)).ToList();
            }

            if (listadoRecibo.Count > 0)
            {
                return listadoRecibo.Select(res => string.Format(res.NroFormulario)).ToList();
            }

            if (listadoContrato.Count > 0)
            {
                return listadoContrato.Select(res => string.Format(res.NroFormulario)).ToList();
            }

            return listadoPagare.Select(res => string.Format(res.NroFormulario)).ToList();
        }

        #endregion

        private SubReporte ObtenerSubreporteDocumentacionPagos(int idReporte, string nroFormulario)
        {
            switch (idReporte)
            {
                case (int) SubReporte.Pagos.Caratula:
                {
                    if (listadoCaratula.Count > 0)
                    {
                        return GenerarSubReporteCaratula(nroFormulario);
                    }

                    return null;
                }
                case (int) SubReporte.Pagos.Providencia:
                {
                    if (listadoProvidencia.Count > 0)
                    {
                        return GenerarSubReporteProvidencia(nroFormulario);
                    }

                    return null;
                }
                case (int) SubReporte.Pagos.Recibo:
                {
                    if (listadoRecibo.Count > 0)
                    {
                        return GenerarSubReporteRecibo(nroFormulario);
                    }

                    return null;
                }
                case (int) SubReporte.Pagos.Pagare:
                {
                    if (listadoPagare.Count > 0)
                    {
                        return GenerarSubReportePagare(nroFormulario);
                    }

                    return null;
                }
                default:
                    return null;
            }
        }

        private SubReporte GenerarSubReporteCaratula(string nroFormulario)
        {
            var caratula = listadoCaratula.FirstOrDefault(res => res.NroFormulario == nroFormulario);

            var dsCaratula = new List<ReporteCaratulaResultado>
            {
                new ReporteCaratulaResultado
                {
                    NumeroFormulario = caratula?.NroFormulario,
                    Fecha = caratula?.FechaInicioPagos?.ToString("dd/MM/yyyy"),
                    SolicitanteNombre = caratula?.NombreCompletoSolicitante,
                    SolicitanteDocumento = caratula?.NroDocumentoSolicitante,
                    ValorPrestamo = caratula?.ValorPrestamo,
                    LineaPrestamo = caratula?.DescripcionLinea,
                    SolicitanteDomicilioCompleto = caratula?.DomicilioCompletoSolicitante,
                    GaranteNombre = caratula?.NombreGarante,
                    TelefonoTitular = caratula?.TelefonoSolicitante,
                    TelefonoGarante = caratula?.TelefonoGarante,
                    SucursalBancaria = caratula?.NombreSucursal,
                    CodigoSucursal = caratula?.IdSucursal,
                    Localidad = caratula?.Localidad,
                    Departamento = caratula?.Departamento
                }
            };

            listadoCaratula.Remove(caratula);

            return SubReporte.Caratula().AsignarDataSet(dsCaratula);
        }

        private SubReporte GenerarSubReporteProvidencia(string nroFormulario)
        {
            var providencia = listadoProvidencia.FirstOrDefault(res => res.NroFormulario == nroFormulario);

            var dsProvidencia = new List<ReporteProvidenciaResultado>()
            {
                new ReporteProvidenciaResultado
                {
                    Destino = providencia?.Destino,
                    Importe = providencia?.ValorPrestamo,
                    ImporteLetras = ConvertidorNumeroLetras.enletras(providencia?.ValorPrestamo),
                    Localidad = providencia?.Localidad,
                    Departamento = providencia?.Departamento,
                    NroSticker = providencia?.NroSticker,
                    NombreLinea = providencia?.NombreLinea,
                    DescripcionLinea = providencia?.DescripcionLinea,
                    NroFormulario = providencia?.NroFormulario,
                    NombreCompleto = providencia?.NombreCompletoSolicitante,
                    Cuil = providencia?.Cuil,
                    Fecha = providencia?.Fecha.ToString("dd/MM/yyyy"),
                }
            };

            listadoProvidencia.Remove(providencia);

            return SubReporte.Providencia().AsignarDataSet(dsProvidencia);
        }

        private SubReporte GenerarSubReporteRecibo(string nroFormulario)
        {
            var recibo = listadoRecibo.FirstOrDefault(res => res.NroFormulario == nroFormulario);

            var mes = new CultureInfo("es-ES", false).DateTimeFormat.GetMonthName(recibo.Fecha.Month);
            string fechaString = $"{recibo.Fecha.Day} de {mes} de {recibo.Fecha.Year}";

            var dsRecibo = new List<ReporteReciboResultado>()
            {
                new ReporteReciboResultado
                {
                    NumeroFormulario = recibo.NroFormulario,
                    Fecha = fechaString,
                    SolicitanteNombre = recibo.NombreCompletoSolicitante,
                    SolicitanteDocumento = recibo.NroDocumentoSolicitante,
                    ValorPrestamo = recibo.ValorPrestamo,
                    LineaPrestamo = recibo.DescripcionLinea,
                    SolicitanteDomicilioCompleto = recibo.DomicilioCompletoSolicitante,
                    ValorPrestamoString = ConvertidorNumeroLetras.enletras(recibo.ValorPrestamo)
                }
            };

            listadoRecibo.Remove(recibo);

            return SubReporte.Recibo().AsignarDataSet(dsRecibo);
        }

        private SubReporte GenerarSubReportePagare(string nroFormulario)
        {
            var pagare = listadoPagare.FirstOrDefault(res => res.NroFormulario == nroFormulario);

            var mes = new CultureInfo("es-ES", false).DateTimeFormat.GetMonthName(pagare.Fecha.Month);
            string fechaString = $"{pagare.Fecha.Day} de {mes} de {pagare.Fecha.Year}";

            var dsPagare = new List<ReportePagareResultado>()
            {
                new ReportePagareResultado
                {
                    NumeroFormulario = pagare.NroFormulario,
                    Fecha = fechaString,
                    FechaVencimientoPlanPago = pagare.FechaVencimientoPlanPago?.ToString("dd/MM/yyyy"),
                    SolicitanteNombre = pagare.NombreCompletoSolicitante,
                    SolicitanteDocumento = pagare.NroDocumentoSolicitante,
                    ValorPrestamo = pagare.ValorPrestamo,
                    SolicitanteDomicilioCompleto = pagare.DomicilioCompletoSolicitante,
                    SucursalMontoDisponible = pagare.DomicilioSucursalBancaria
                }
            };

            listadoPagare.Remove(pagare);

            return SubReporte.Pagare().AsignarDataSet(dsPagare);
        }
    }
}