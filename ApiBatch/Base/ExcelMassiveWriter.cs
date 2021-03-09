using System.Collections.Generic;
using System.Web.Hosting;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ApiBatch.Base
{
    public abstract class ExcelMassiveWriter<T>
    {
        private OpenXmlWriter oxw;

        public abstract void WriteRow(int rowIndex,T t);

        public  void WriteExcel(List<T> data) {

            
            using (SpreadsheetDocument xl = SpreadsheetDocument.Create(HostingEnvironment.MapPath(@"~/Tmp/LargeFile.xlsx"), SpreadsheetDocumentType.Workbook))
            {
                 
                xl.AddWorkbookPart();
                WorksheetPart wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();

                oxw = OpenXmlWriter.Create(wsp);
                oxw.WriteStartElement(new Worksheet());
                oxw.WriteStartElement(new SheetData());
               
                for (int rowIndex = 1; rowIndex <= data.Count; ++rowIndex)
                {
                    CreateRow(oxw, rowIndex);

                    T dataItem = data[rowIndex - 1];

                    WriteRow(rowIndex, dataItem);
                    // this is for Row
                    oxw.WriteEndElement();
                }

                // this is for SheetData
                oxw.WriteEndElement();
                // this is for Worksheet
                oxw.WriteEndElement();
                oxw.Close();

                oxw = OpenXmlWriter.Create(xl.WorkbookPart);
                oxw.WriteStartElement(new Workbook());
                oxw.WriteStartElement(new Sheets());

                // you can use object initialisers like this only when the properties
                // are actual properties. SDK classes sometimes have property-like properties
                // but are actually classes. For example, the Cell class has the CellValue
                // "property" but is actually a child class internally.
                // If the properties correspond to actual XML attributes, then you're fine.
                oxw.WriteElement(new Sheet()
                {
                    Name = "Sheet1",
                    SheetId = 1,
                    Id = xl.WorkbookPart.GetIdOfPart(wsp)
                });

                // this is for Sheets
                oxw.WriteEndElement();
                // this is for Workbook
                oxw.WriteEndElement();
                oxw.Close();

                xl.Close();
            }
        }

        
        public static void CreateRow(OpenXmlWriter oxw, int index)
        {
            var oxa = new List<OpenXmlAttribute>();
            // this is the row index
            oxa.Add(new OpenXmlAttribute("r", null, index.ToString()));

            oxw.WriteStartElement(new Row(), oxa);
        }

        public  void WriteCell(int i, int j, string value)
        {
            var oxa = new List<OpenXmlAttribute>();
            // this is the data type ("t"), with CellValues.String ("str")
            oxa.Add(new OpenXmlAttribute("t", null, "str"));

            // it's suggested you also have the cell reference, but
            // you'll have to calculate the correct cell reference yourself.
            // Here's an example:
            //oxa.Add(new OpenXmlAttribute("r", null, "A1"));

            oxw.WriteStartElement(new Cell(), oxa);

            oxw.WriteElement(new CellValue(value));

            // this is for Cell
            oxw.WriteEndElement();
        }


    }
}