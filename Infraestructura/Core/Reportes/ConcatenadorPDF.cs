using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.IO;

namespace Infraestructura.Core.Reportes
{
    public class ConcatenadorPDF
    {
        public ConcatenadorPDF(string directorio = null, bool guardarArchivosMientrasSeAgregan = false)
        {
            Directorio = directorio;
            Reportes = new List<ReporteConcatenado>();
            Errores = new List<string>();
            SaveFilesAsAdded = guardarArchivosMientrasSeAgregan;
        }

        private List<ReporteConcatenado> Reportes { get; set; }
        private string Directorio { get; set; }
        public List<string> Errores { get; }
        private bool SaveFilesAsAdded { get; } //Si es true libera un poco de memoria RAM ya que los va guardando en disco en vez de mantenerlos en memoria

        public ConcatenadorPDF AgregarReporte(Reporte reportData, string fileName)
        {
            if(Reportes == null) Reportes = new List<ReporteConcatenado>();
            if (SaveFilesAsAdded)
            {
                GuardarReporteEnDirectorio(new ReporteConcatenado(reportData, fileName));
            }
            else
            {
                Reportes.Add(new ReporteConcatenado(reportData, fileName));
            }
            return this; 
        }

        /// Método para obtener el reporte concatenando todos los reportes que fueron agregados
        /// Si devuelve null debería eliminarse el directorio creado si es un directorio temporal
        /// Si tiene éxito devuelve el array de bytes del reporte concatenado 
        public byte[] ObtenerReporteConcatenadoEnPDF()
        {
            if (!SaveFilesAsAdded)
            {
                if (Reportes?.Count > 0)
                {
                    //foreach (var reporte in Reportes)
                    //{
                    //    GuardarReporteEnDirectorio(reporte);
                    //}
                    return ConcatenarPdfs(Reportes);
                }
                else
                {
                    Errores.Add("No se seleccionaron reportes.");
                    return null;
                }
            }

            List<string> archivos = GetFiles(Directorio);
            if (archivos.Count == 0)
            {
                Errores.Add("No se encontraron los archivos en el directorio para concatenar.");
                return null;
            }

            return ConcatenarPdfs(archivos);
        }

        public static void GuardarReporteEnDirectorio(Reporte reporte, string directorio, string nombreReporte)
        {
            File.WriteAllBytes(directorio + "\\" + nombreReporte + ".pdf", reporte.Content);
        }

        private void GuardarReporteEnDirectorio(ReporteConcatenado reporte)
        {
            File.WriteAllBytes(Directorio + "\\" + reporte.FileName + ".pdf", reporte.ReportData.Content);
        }

        public bool ExisteArchivoEnDirectorio(string fileName)
        {
            return File.Exists(Directorio + fileName + ".pdf");
        }

        public bool ExisteArchivoEnCola(string fileName)
        {
            if (Reportes == null) return false;
            if (Reportes.Count == 0) return false;
            return Reportes.FirstOrDefault(x => x.FileName == fileName) != null;
        }


        private static List<string> GetFiles(string directorioRaiz)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directorioRaiz);
            FileInfo[] fileInfos = dirInfo.GetFiles("*.pdf");
            List<string> list = new List<string>();
            foreach (FileInfo info in fileInfos)
            {
                //// HACK: Just skip the protected samples file...
                //if (info.Name.IndexOf("protected") == -1)
                //    list.Add(info.FullName);
                list.Add(info.FullName);
            }
            return list;
        }

        private static byte[] ConcatenarPdfs(List<string> files)
        {
            try
            {
                // Open the output document
                PdfDocument outputDocument = new PdfDocument();

                // Iterate files
                foreach (string file in files)
                {
                    // Open the document to import pages from it.
                    PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }
                }

                // Save the document...
                //string filename = nombreArchivoSalida + ".pdf";
                //outputDocument.Save(directorioSalida + filename);
                //// ...and start a viewer.
                //Process.Start(filename);

                MemoryStream stream = new MemoryStream();
                // Saves the document as stream
                outputDocument.Save(stream);
                outputDocument.Close();
                // Converts the PdfDocument object to byte form.
                byte[] docBytes = stream.ToArray();
                return docBytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static byte[] ConcatenarPdfs(List<ReporteConcatenado> reportes)
        {
            try
            {
                // Open the output document
                PdfDocument outputDocument = new PdfDocument();
                // Iterate files
                foreach (var file in reportes)
                {
                    // Open the document to import pages from it.
                    Stream streamReporte = new MemoryStream(file.ReportData.Content);
                    PdfDocument inputDocument = PdfReader.Open(streamReporte, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        //El número 20 corresponde a la cantidad de caracteres que hay en una hoja en blanco
                        var content = ContentReader.ReadContent(inputDocument.Pages[idx]);
                        if (content.Count > 20)
                        {
                            // Get the page from the external document...
                            PdfPage page = inputDocument.Pages[idx];
                            // ...and add it to the output document.
                            outputDocument.AddPage(page);
                        }
                    }
                }

                // Save the document...
                //string filename = nombreArchivoSalida + ".pdf";
                //outputDocument.Save(directorioSalida + filename);
                //// ...and start a viewer.
                //Process.Start(filename);

                MemoryStream stream = new MemoryStream();
                // Saves the document as stream
                outputDocument.Save(stream);
                outputDocument.Close();
                // Converts the PdfDocument object to byte form.
                byte[] docBytes = stream.ToArray();
                return docBytes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
