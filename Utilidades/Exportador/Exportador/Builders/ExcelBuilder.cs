using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using OfficeOpenXml;

namespace Utilidades.Exportador.Builders
{
    public class ExcelBuilder<T> : Builder<T>
    {
        private int StartX { get; set; }
        private int StartY { get; set; }

        private bool WithHeader { get; set; }

        public ExcelBuilder()
        {
            StartX = 0;
            StartY = 0;
        }

        public override byte[] Generate(IList<T> data)
        {
            byte[] excel;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Page");
                //headers
                for (int j = 0; j < Columns.Count; j++)
                {
                    var column = (ExcelColumn) Columns[j];

                    ws.Cells[StartX , StartY + j].Value = column.Title;
                }

                //data
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < Columns.Count; j++)
                    {
                        var column = (ExcelColumn)Columns[j];
                        var raw = data[i];
                        ws.Cells[StartX + i+1, StartY + j].Value = column.Property.GetValue(raw);
                    }
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                excel = pck.GetAsByteArray();
            }
            return excel;
        }

        public byte[] Generate(IList<T> data, byte[] excel)
        {
            var stream = new MemoryStream(excel);
            using (ExcelPackage pck = new ExcelPackage(stream))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets["Page"];

                //data

                var rows = ws.Dimension.Rows;
                
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < Columns.Count; j++)
                    {
                        var column = (ExcelColumn)Columns[j];
                        var raw = data[i];
                        ws.Cells[StartX + i + rows, StartY + j].Value = column.Property.GetValue(raw);
                    }
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                excel = pck.GetAsByteArray();
            }
            return excel;
        }


        public ExcelBuilder<T> AddColumn<TProperty>(string title, int index,
            Expression<Func<T, TProperty>> propertyLambda)
        {
            var column = new ExcelColumn()
            {
                Title = title,
                Index = index
            };

            AddColumn(column, propertyLambda);
            return this;
        }


        public ExcelBuilder<T> AddConfiguration(ExcelConfiguration excelConfiguration)
        {
            if (excelConfiguration == null)
            {
                throw new ArgumentException("la configuracion del excel no puede ser null");
            }

            StartX = excelConfiguration.FromX;
            StartY = excelConfiguration.FromY;

            return this;
        }

        public override string GenerateAsString(IList<T> data)
        {
            var bytes = Generate(data);
            return Convert.ToBase64String(bytes);
        }

        public string GenerateAsString(IList<T> data, string excel)
        {
            var oldBytes = Convert.FromBase64String(excel);
            
            var bytes = Generate(data, oldBytes);
            return Convert.ToBase64String(bytes);
        }
    }
}