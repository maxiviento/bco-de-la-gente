using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Core.CiDi.Documentos.Utils
{
    public class PDFHelper
    {
        #region Atributos

        /// <summary>
        /// Ubicación física de objetos que se incrustan en la documentación consultada.
        /// </summary>
        public String path;

        /// <summary>
        /// Caracter sepearador de los datos que conforman el codigo QR.
        /// </summary>
        private const char Separador_QR = ';';

        /// <summary>
        /// Cantidad de páginas asignadas para el preview de la documentación.
        /// </summary>
        private int CantidadPaginasPreview { get; set; }

        #endregion

        #region Metodos QR

        internal String Conformar_Cadena_QR_Documento(String pQr, char pOperacion)
        {
            String retCadenaQR = String.Empty;
            String[] arrQr = pQr.Split(';');

            if (arrQr != null && arrQr.Length > 0)
            {
                String pNombreUsuarioPropietarioDocumento = arrQr[0].ToString();
                String pCuilCuitUsuarioPropietarioDocumento = arrQr[1].ToString();
                String pFechaCreacion = arrQr[2].ToString();
                String pCuilCuitOperadorCreacionDocumento = arrQr[3].ToString();
                String pCuilCuitOperadorVisualizacionDescargaDocumento = arrQr[4].ToString();
                String pFechaVisualizacionDescargaDocumento = arrQr[5].ToString();

                if (pOperacion.Equals('V'))
                    retCadenaQR = String.Concat("El documento solicitado pertenece a ", pNombreUsuarioPropietarioDocumento,
                                                " (", pCuilCuitUsuarioPropietarioDocumento, "). ",
                                                "Fue guardado en el Centro de Documentación Digital el ", pFechaCreacion,
                                                " por el agente ", pCuilCuitOperadorCreacionDocumento,
                                                " y visualizado por ", pCuilCuitOperadorVisualizacionDescargaDocumento,
                                                " el dia ", pFechaVisualizacionDescargaDocumento.Substring(0, 10));
                else if (pOperacion.Equals('D'))
                    retCadenaQR = String.Concat("El documento solicitado pertenece a ", pNombreUsuarioPropietarioDocumento,
                                                " (", pCuilCuitUsuarioPropietarioDocumento, "). ",
                                                "Fue guardado en el Centro de Documentación Digital el ", pFechaCreacion,
                                                " por el agente ", pCuilCuitOperadorCreacionDocumento,
                                                " y descargado por ", pCuilCuitOperadorVisualizacionDescargaDocumento,
                                                " el dia ", pFechaVisualizacionDescargaDocumento.Substring(0, 10));
            }

            return retCadenaQR;
        }

        internal String Conformar_Cadena_QR_Documento_Vinculante_PFJ(String pQr, char pOperacion)
        {
            String retCadenaQR = String.Empty;
            String[] arrQr = pQr.Split(';');

            if (arrQr != null && arrQr.Length > 1)
            {
                String pNombreUsuarioPropietarioDocumento = arrQr[0].ToString();
                String pCuilCuitUsuarioPropietarioDocumento = arrQr[1].ToString();
                String pFechaCreacion = arrQr[2].ToString();
                String pNumeroExpediente = arrQr[3].ToString();
                String pCuilCuitOperadorCreacionDocumento = arrQr[4].ToString();
                String pCuilCuitOperadorVisualizacionDescargaDocumento = arrQr[5].ToString();
                String pFechaVisualizacionDescargaDocumento = arrQr[6].ToString();

                if (pOperacion.Equals('V'))
                    retCadenaQR = String.Concat("El documento solicitado pertenece a ", pNombreUsuarioPropietarioDocumento,
                                                " (", pCuilCuitUsuarioPropietarioDocumento, "). ",
                                                "Fue guardado en el Centro de Documentación Digital el ", pFechaCreacion,
                                                " con el número de expediente ", pNumeroExpediente,
                                                " por el agente ", pCuilCuitOperadorCreacionDocumento,
                                                " y visualizado por ", pCuilCuitOperadorVisualizacionDescargaDocumento,
                                                " el dia ", pFechaVisualizacionDescargaDocumento.Substring(0, 10));
                else if (pOperacion.Equals('D'))
                    retCadenaQR = String.Concat("El documento solicitado pertenece a ", pNombreUsuarioPropietarioDocumento,
                                                " (", pCuilCuitUsuarioPropietarioDocumento, "). ",
                                                "Fue guardado en el Centro de Documentación Digital el ", pFechaCreacion,
                                                " con el número de expediente ", pNumeroExpediente,
                                                " por el agente ", pCuilCuitOperadorCreacionDocumento,
                                                " y descargado por ", pCuilCuitOperadorVisualizacionDescargaDocumento,
                                                " el dia ", pFechaVisualizacionDescargaDocumento.Substring(0, 10));
            }

            return retCadenaQR;
        }

        internal String Conformar_Cadena_QR_Documento_No_Vinculante_PFJ(String pQr, char pOperacion)
        {
            String retCadenaQR = String.Empty;
            String[] arrQr = pQr.Split(';');

            if (arrQr != null && arrQr.Length > 1)
            {
                String pFechaCreacion = arrQr[0].ToString();
                String pNumeroExpediente = arrQr[1].ToString();
                String pCuilCuitOperadorCreacionDocumento = arrQr[2].ToString();
                String pCuilCuitOperadorVisualizacionDescargaDocumento = arrQr[3].ToString();
                String pFechaVisualizacionDescargaDocumento = arrQr[4].ToString();

                if (pOperacion.Equals('V'))
                    retCadenaQR = String.Concat("El documento solicitado fue guardado en el Centro de Documentación Digital el ", pFechaCreacion,
                                             " con el número de expediente ", pNumeroExpediente,
                                             " por el agente ", pCuilCuitOperadorCreacionDocumento,
                                             " y visualizado por ", pCuilCuitOperadorVisualizacionDescargaDocumento,
                                             " el dia ", pFechaVisualizacionDescargaDocumento.Substring(0, 10));
                else if (pOperacion.Equals('D'))
                    retCadenaQR = String.Concat("El documento solicitado fue guardado en el Centro de Documentación Digital el ", pFechaCreacion,
                                             " con el número de expediente ", pNumeroExpediente,
                                             " por el agente ", pCuilCuitOperadorCreacionDocumento,
                                             " y descargado por ", pCuilCuitOperadorVisualizacionDescargaDocumento,
                                             " el dia ", pFechaVisualizacionDescargaDocumento.Substring(0, 10));
            }

            return retCadenaQR;
        }

        protected string Cifrar(string pCadena)
        {
            var utiles = new Helper();
            return utiles.Cifrar(pCadena);
        }

        protected string Descifrar(string pCadena)
        {
            var utiles = new Helper();
            return utiles.Descifrar(pCadena);
        }

        #endregion

        #region Metodos

        public MemoryStream Obtener_Preview_Pdf(String pPathImages,
                                                byte[] pImagen,
                                                String pQr,
                                                char pOperacion)
        {
            path = pPathImages;
            Document document = new Document(PageSize.LEGAL, 20, 20, 20, 36);
            //PdfReader.unethicalreading = true;

            MemoryStream stream = new MemoryStream();

            PdfWriter pdfWrite = PdfWriter.GetInstance(document, stream);
            ITextEvents it = new ITextEvents();
            it.path = pPathImages;
            pdfWrite.PageEvent = it;
            pdfWrite.CloseStream = false;

            #region Atributos del documento

            //TODO Alvaro desharcodear
            document.AddAuthor("20339768146");
            document.AddSubject("Centro de Documentación Digital | Aplicación Modelo");
            document.AddCreator("Secretaría de Innovación y Modernización");
            document.AddProducer();

            #endregion

            document.Open();

            #region Cuerpo del Documento

            PdfContentByte cb = pdfWrite.DirectContent;
            PdfImportedPage page;
            PdfReader reader;

            try
            {
                reader = new PdfReader(pImagen);
                int pages = reader.NumberOfPages;
                CantidadPaginasPreview = 5;

                if (CantidadPaginasPreview < 1 || CantidadPaginasPreview > pages)
                {
                    CantidadPaginasPreview = pages;
                }

                // Recorrido por cada una de las páginas
                for (int i = 1; i <= CantidadPaginasPreview; i++)
                {
                    page = pdfWrite.GetImportedPage(reader, i);

                    float Scale;
                    if (page.Width > 800)
                    {
                        Scale = 0.7f;

                        var truePageWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                        var truePageHeight = document.PageSize.Height - document.TopMargin - document.BottomMargin;

                        var x = (truePageWidth - page.Width) / 2 + document.RightMargin;
                        var y = (truePageHeight - page.Height) / 2 + document.BottomMargin;

                        cb.AddTemplate(page, Scale, 0, 0, Scale, 0, (truePageHeight - page.Height) + document.BottomMargin);
                    }
                    else if (page.Width > 700)
                    {
                        cb.AddTemplate(page,
                                   PageSize.LEGAL.Width / reader.GetPageSize(i).Width,
                                   0, 0,
                                   PageSize.LEGAL.Height / reader.GetPageSize(i).Height,
                                   0, 0);
                    }
                    else if (page.Width > 600)
                    {
                        cb.AddTemplate(page,
                                   PageSize.LEGAL.Width / reader.GetPageSize(i).Width,
                                   0, 0,
                                   PageSize.LEGAL.Height / reader.GetPageSize(i).Height,
                                   0, 0);
                    }
                    else if (page.Width > 500)
                    {
                        Scale = 1f;

                        var truePageWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                        var truePageHeight = document.PageSize.Height - document.TopMargin - document.BottomMargin;

                        var x = (truePageWidth - page.Width) / 2 + document.RightMargin;
                        var y = (truePageHeight - page.Height) / 2 + document.BottomMargin;

                        cb.AddTemplate(page, Scale, 0, 0, Scale, 0, (truePageHeight - page.Height) + document.BottomMargin);

                        //Scale = 0.9f;

                        //var truePageWidth = document.PageSize.Width;
                        //var truePageHeight = document.PageSize.Height;

                        //cb.AddTemplate(page, Scale, 0, 0, Scale, 0, -72);
                    }

                    document.NewPage();
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            #endregion

            document.Close();

            stream.Flush();
            stream.Position = 0;

            return stream;
        }

        public byte[] Obtener_Preview_Pdf(byte[] pImagen, int paginaInicio, int paginaFinal)
        {
            PdfReader reader = null;
            Document docOriginal = null;
            PdfCopy pdfCopyProveedor = null;
            PdfImportedPage paginaImportada = null;
            MemoryStream retDocumento = new MemoryStream();

            reader = new PdfReader(pImagen);
            docOriginal = new Document(reader.GetPageSizeWithRotation(paginaInicio));
            pdfCopyProveedor = new PdfCopy(docOriginal, retDocumento);
            docOriginal.Open();

            for (int i = paginaInicio; i <= paginaFinal; i++)
            {
                paginaImportada = pdfCopyProveedor.GetImportedPage(reader, i);
                pdfCopyProveedor.AddPage(paginaImportada);
            }

            docOriginal.Close();
            reader.Close();

            return retDocumento.ToArray();
        }

        #endregion
    }
}