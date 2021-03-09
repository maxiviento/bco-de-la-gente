using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Excel;
using Excel.Log;

namespace Utilidades.Importador.Builders
{
    public class ExcelBuilder<T> : Builder<T>
    {
        public ExcelBuilder()
        {
            _columns = new List<Column>();
        }

        private IList<Column> _columns { get; set; }

        public ExcelBuilder<T> AddColumn<TProperty>(int i, Expression<Func<T, TProperty>> propertyLambda)
        {
            var column = new PositionalColumn()
            {
                Index = i
            };

            AddColumn(column, propertyLambda);

            return this;
        }

        private IExcelDataReader GetExcelDataFrom2003(Stream stream)
        {
            IExcelDataReader reader = null;
            try
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            catch (Exception ex)
            {
                ex.Log();
            }

            return reader;
        }

        private IExcelDataReader GetExcelDataFrom2007(Stream stream)
        {
            IExcelDataReader reader = null;
            try
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            catch (Exception ex)
            {
                ex.Log();
            }

            return reader;
        }


        public override void Generate(byte[] arrayBytes, out List<T> success, out List<ImportResult<T>> withErrors)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(arrayBytes, 0, arrayBytes.Length);

            success = new List<T>();
            withErrors = new List<ImportResult<T>>();
            var data = new List<string>();
            var excelReader = GetExcelDataFrom2003(stream);

            if (excelReader == null)
            {
                excelReader = GetExcelDataFrom2007(stream);
            }

            if (excelReader != null && excelReader.IsValid)
            {
                var mustSkipFirst = excelReader.IsFirstRowAsColumnNames = true;
                var index = mustSkipFirst ? 0 : 1;
                while (excelReader.Read())
                {
                    index++;
                    if (mustSkipFirst)
                    {
                        mustSkipFirst = false;
                        continue;
                    }

                    var rowObject = Activator.CreateInstance<T>();

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        var column = (PositionalColumn) Columns[i];
                        var columnValue = excelReader.GetString(column.Index);

                        object objValue = null;
                        var converter = Converters.SingleOrDefault(x => x.CanConvert(column.PropertyType));
                        if (converter != null)
                        {
                            objValue = converter.Convert(column.PropertyType, columnValue);
                        }
                        else
                        {
                            objValue = TypeConverter.ConvertTo(column.PropertyType, columnValue);
                        }

                        column.Property.SetValue(rowObject, objValue);
                    }

                    if (Interceptor == null || Interceptor.Pos(index, ref rowObject))
                    {
                        success.Add(rowObject);
                    }
                    else
                    {
                        var error = new ImportResult<T>()
                        {
                            Raw = rowObject
                        };
                        withErrors.Add(error);
                    }
                }
            }
        }

        public override void LeerArchivoSuaf(byte[] arrayBytes, out List<T> devengados, out List<T> noProcesados,
            out List<T> conErrores)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(arrayBytes, 0, arrayBytes.Length);

            devengados = new List<T>();
            noProcesados = new List<T>();
            conErrores = new List<T>();

            var excelReader = GetExcelDataFrom2003(stream);

            if (excelReader == null)
            {
                excelReader = GetExcelDataFrom2007(stream);
            }

            if (excelReader != null && excelReader.IsValid)
            {
                var mustSkipFirst = excelReader.IsFirstRowAsColumnNames = true;
                var index = mustSkipFirst ? 0 : 1;
                while (excelReader.Read())
                {
                    index++;
                    if (mustSkipFirst)
                    {
                        mustSkipFirst = false;
                        continue;
                    }

                    var rowObject = Activator.CreateInstance<T>();

                    foreach (var columna in Columns)
                    {
                        var column = (PositionalColumn) columna;
                        var columnValue = excelReader.GetString(column.Index);

                        var converter = Converters.SingleOrDefault(x => x.CanConvert(column.PropertyType));
                        var objValue = converter != null
                            ? converter.Convert(column.PropertyType, columnValue)
                            : TypeConverter.ConvertTo(column.PropertyType, columnValue);

                        column.Property.SetValue(rowObject, objValue);
                    }

                    switch (Interceptor.ValidarDevengadoSuaf(index, ref rowObject))
                    {
                        case 1:
                            devengados.Add(rowObject);
                            break;
                        case 2:
                            noProcesados.Add(rowObject);
                            break;
                        case 3:
                            conErrores.Add(rowObject);
                            break;
                        default:
                            conErrores.Add(rowObject);
                            break;
                    }
                }
            }
        }

        public override void GenerateFirstRowOnly(byte[] arrayBytes, out List<T> success,
            out List<ImportResult<T>> withErrors)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(arrayBytes, 0, arrayBytes.Length);

            success = new List<T>();
            withErrors = new List<ImportResult<T>>();
            var data = new List<string>();
            var excelReader = GetExcelDataFrom2003(stream);

            if (excelReader == null)
            {
                excelReader = GetExcelDataFrom2007(stream);
            }

            if (excelReader != null && excelReader.IsValid)
            {
                var mustSkipFirst = excelReader.IsFirstRowAsColumnNames = true;
                var index = mustSkipFirst ? 0 : 1;

                while (excelReader.Read())
                {
                    if (mustSkipFirst)
                    {
                        mustSkipFirst = false;
                        index++;
                        continue;
                    }

                    var rowObject = Activator.CreateInstance<T>();

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        var column = (PositionalColumn) Columns[i];
                        var columnValue = excelReader.GetString(column.Index);

                        object objValue = null;
                        var converter = Converters.SingleOrDefault(x => x.CanConvert(column.PropertyType));
                        if (converter != null)
                        {
                            objValue = converter.Convert(column.PropertyType, columnValue);
                        }
                        else
                        {
                            objValue = TypeConverter.ConvertTo(column.PropertyType, columnValue);
                        }

                        column.Property.SetValue(rowObject, objValue);
                    }

                    if (Interceptor == null || Interceptor.Pos(index, ref rowObject))
                    {
                        success.Add(rowObject);
                    }
                    else
                    {
                        var error = new ImportResult<T>()
                        {
                            Raw = rowObject
                        };
                        withErrors.Add(error);
                    }

                    if (index == 1)
                    {
                        break;
                    }

                    index++;
                }
            }
        }
    }
}