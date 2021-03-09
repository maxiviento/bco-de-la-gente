using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Linq;

namespace Core.CiDi.Documentos.Utils
{
    public class DocumentConverter
    {
        //private String pathDocumentos = String.Concat(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Path_Documentos"].ToString());
        
        #region Word to Pdf

        public byte[] Convert_Doc_To_Pdf(byte[] pDocumentoWord, String pPath, String pDocNombre)
        {
            byte[] retDocPdf = new byte[pDocumentoWord.Length];

            // Cierro instancias de Word previamente abiertas
            var openedWord = System.Diagnostics.Process.GetProcessesByName("WINWORD");
            if (openedWord.Any())
            {
                foreach (var itemWord in openedWord)
                {
                    try { itemWord.Kill(); }
                    catch { }
                }
            }

            // Create a new Microsoft Word application object
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();

            // C# doesn't have optional arguments so we'll need a dummy value
            object oMissing = System.Reflection.Missing.Value;

            // Write byte array Word file in specified directory
            File.WriteAllBytes(Path.Combine(pPath, pDocNombre), pDocumentoWord);

            DirectoryInfo dirInfo = new DirectoryInfo(pPath);
            FileInfo[] wordFiles = dirInfo.GetFiles("*.doc");

            word.ScreenUpdating = false;
            word.Visible = false;
            word.ScreenUpdating = false;

            foreach (FileInfo wordFile in wordFiles)
            {
                if (wordFile.Name.Equals(pDocNombre))
                {
                    // Cast as Object for word Open method
                    Object filename = (Object)wordFile.FullName;

                    // Use the dummy value as a placeholder for optional arguments
                    Document docx = word.Documents.Open(ref filename, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    docx.Activate();

                    object outputFileName = wordFile.FullName.Replace("DOC", "PDF");
                    object fileFormat = WdSaveFormat.wdFormatPDF;

                    // Save document into PDF Format
                    docx.SaveAs(ref outputFileName,
                        ref fileFormat, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                    // Close the Word document, but leave the Word application open.
                    // doc has to be cast to type _Document so that it will find the
                    // correct Close method.                
                    object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
                    ((_Document)docx).Close(ref saveChanges, ref oMissing, ref oMissing);
                    docx = null;

                    retDocPdf = File.ReadAllBytes(Convert.ToString(outputFileName));
                }
            }

            // word has to be cast to type _Application so that it will find
            // the correct Quit method.
            ((_Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
            word = null;

            return retDocPdf;
        }

        public byte[] Convert_Docx_To_Pdf(byte[] pDocumentoWord, String pPath, String pDocNombre)
        {
            byte[] retDocPdf = new byte[pDocumentoWord.Length];

            // Cierro instancias de Word previamente abiertas
            var openedWord = System.Diagnostics.Process.GetProcessesByName("WINWORD");
            if (openedWord.Any())
            {
                foreach (var itemWord in openedWord)
                {
                    try { itemWord.Kill(); }
                    catch { }
                }
            }

            // Create a new Microsoft Word application object
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();

            // C# doesn't have optional arguments so we'll need a dummy value
            object oMissing = System.Reflection.Missing.Value;

            // Write byte array Word file in specified directory
            File.WriteAllBytes(Path.Combine(pPath, pDocNombre), pDocumentoWord);

            DirectoryInfo dirInfo = new DirectoryInfo(pPath);
            FileInfo[] wordFiles = dirInfo.GetFiles("*.docx");

            word.ScreenUpdating = false;
            word.Visible = false;
            word.ScreenUpdating = false;

            foreach (FileInfo wordFile in wordFiles)
            {
                if (wordFile.Name.Equals(pDocNombre))
                {
                    // Cast as Object for word Open method
                    Object filename = (Object)wordFile.FullName;

                    // Use the dummy value as a placeholder for optional arguments
                    Document docx = word.Documents.Open(ref filename, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                    docx.Activate();

                    object outputFileName = wordFile.FullName.Replace("DOCX", "PDF");
                    object fileFormat = WdSaveFormat.wdFormatPDF;

                    // Save document into PDF Format
                    docx.SaveAs(ref outputFileName,
                        ref fileFormat, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                    // Close the Word document, but leave the Word application open.
                    // doc has to be cast to type _Document so that it will find the
                    // correct Close method.                
                    object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
                    ((_Document)docx).Close(ref saveChanges, ref oMissing, ref oMissing);
                    docx = null;

                    retDocPdf = File.ReadAllBytes(Convert.ToString(outputFileName));
                }
            }

            // word has to be cast to type _Application so that it will find
            // the correct Quit method.
            ((_Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
            word = null;

            return retDocPdf;
        }
        
        #endregion

        #region Excel to Pdf

        //public byte[] Convert_Xls_To_Pdf(String pNombreDocumento, byte[] pDocumentoWord)
        //{
            //var arrPdf = new byte[pDocumentoWord.Length];

            //// Create a new Microsoft Word application object
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //// C# doesn't have optional arguments so we'll need a dummy value
            //object oMissing = System.Reflection.Missing.Value;

            //// Get list of Word files in specified directory
            //File.WriteAllBytes(pathDocumentos + pNombreDocumento + ".xls", pDocumentoWord);

            //DirectoryInfo dirInfo = new DirectoryInfo(pathDocumentos);
            //FileInfo[] excelFiles = dirInfo.GetFiles("*.xls");

            //excel.Visible = false;
            //excel.ScreenUpdating = false;

            //foreach (FileInfo excelFile in excelFiles)
            //{
            //    // Cast as Object for excel Open method
            //    Object filename = (Object)excelFile.FullName;

            //    Workbook wb = excel.Workbooks.Open(excelFile.FullName);
            //    wb.Activate();

            //    object outputFileName = excelFile.FullName.Replace(".xls", ".pdf");
            //    object fileFormat = WdSaveFormat.wdFormatPDF;

            //    wb.SaveAs(outputFileName,
            //                fileFormat, oMissing, oMissing,
            //                oMissing, oMissing, XlSaveAsAccessMode.xlExclusive, oMissing,
            //                oMissing, oMissing, oMissing, oMissing);


            //    // Close the Word document, but leave the Word application open.
            //    // doc has to be cast to type _Document so that it will find the
            //    // correct Close method.                
            //    object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
            //    ((_Document)wb).Close(ref saveChanges, ref oMissing, ref oMissing);
            //    wb = null;
            //}

            //// Excel has to be cast to type _Application so that it will find
            //// the correct Quit method.
            //((Microsoft.Office.Interop.Excel._Application)excel).Quit();
            //excel = null;

            //arrPdf = File.ReadAllBytes(pathDocumentos + pNombreDocumento + ".pdf");

            //File.Delete(pathDocumentos + pNombreDocumento + ".xls");
            //File.Delete(pathDocumentos + pNombreDocumento + ".pdf");

            //return arrPdf;
        //}

        //public byte[] Convert_Xlsx_To_Pdf(String pNombreDocumento, byte[] pDocumentoWord)
        //{
        //    var arrPdf = new byte[pDocumentoWord.Length];

        //    // Create a new Microsoft Word application object
        //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

        //    // C# doesn't have optional arguments so we'll need a dummy value
        //    object oMissing = System.Reflection.Missing.Value;

        //    // Get list of Word files in specified directory
        //    File.WriteAllBytes(pathDocumentos + pNombreDocumento + ".xlsx", pDocumentoWord);

        //    DirectoryInfo dirInfo = new DirectoryInfo(pathDocumentos);
        //    FileInfo[] excelFiles = dirInfo.GetFiles("*.xlsx");

        //    excel.Visible = false;
        //    excel.ScreenUpdating = false;

        //    foreach (FileInfo excelFile in excelFiles)
        //    {
        //        // Cast as Object for excel Open method
        //        Object filename = (Object)excelFile.FullName;

        //        Workbook wb = excel.Workbooks.Open(excelFile.FullName);
        //        wb.Activate();

        //        object outputFileName = excelFile.FullName.Replace(".xlsx", ".pdf");
        //        object fileFormat = WdSaveFormat.wdFormatPDF;

        //        wb.SaveAs(outputFileName,
        //                    fileFormat, oMissing, oMissing,
        //                    oMissing, oMissing, XlSaveAsAccessMode.xlExclusive, oMissing,
        //                    oMissing, oMissing, oMissing, oMissing);

        //        // Close the Word document, but leave the Word application open.
        //        // doc has to be cast to type _Document so that it will find the
        //        // correct Close method.                
        //        object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
        //        ((_Document)wb).Close(ref saveChanges, ref oMissing, ref oMissing);
        //        wb = null;
        //    }

        //    // word has to be cast to type _Application so that it will find
        //    // the correct Quit method.
        //    ((Microsoft.Office.Interop.Excel._Application)excel).Quit();
        //    excel = null;

        //    arrPdf = File.ReadAllBytes(pathDocumentos + pNombreDocumento + ".pdf");

        //    File.Delete(pathDocumentos + pNombreDocumento + ".xlsx");
        //    File.Delete(pathDocumentos + pNombreDocumento + ".pdf");

        //    return arrPdf;
        //}

        //public bool ExportWorkbookToPdf(string workbookPath)
        //{
        //    // If either required string is null or empty, stop and bail out
        //    if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(pathDocumentos))
        //    {
        //        return false;
        //    }

        //    // Create COM Objects
        //    Microsoft.Office.Interop.Excel.Application excelApplication;
        //    Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

        //    // Create new instance of Excel
        //    excelApplication = new Microsoft.Office.Interop.Excel.Application();

        //    // Make the process invisible to the user
        //    excelApplication.ScreenUpdating = false;

        //    // Make the process silent
        //    excelApplication.DisplayAlerts = false;

        //    // Open the workbook that you wish to export to PDF
        //    excelWorkbook = excelApplication.Workbooks.Open(workbookPath);

        //    // If the workbook failed to open, stop, clean up, and bail out
        //    if (excelWorkbook == null)
        //    {
        //        excelApplication.Quit();

        //        excelApplication = null;
        //        excelWorkbook = null;

        //        return false;
        //    }

        //    var exportSuccessful = true;
        //    try
        //    {
        //        // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
        //        excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, pathDocumentos);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        // Mark the export as failed for the return value...
        //        exportSuccessful = false;

        //        // Do something with any exceptions here, if you wish...
        //        // MessageBox.Show...        
        //    }
        //    finally
        //    {
        //        // Close the workbook, quit the Excel, and clean up regardless of the results...
        //        excelWorkbook.Close();
        //        excelApplication.Quit();

        //        excelApplication = null;
        //        excelWorkbook = null;
        //    }

        //    // You can use the following method to automatically open the PDF after export if you wish
        //    // Make sure that the file actually exists first...
        //    if (System.IO.File.Exists(pathDocumentos))
        //    {
        //        var filePdf = File.ReadAllBytes(pathDocumentos);
        //    }

        //    return exportSuccessful;
        //}

        #endregion
    }
}