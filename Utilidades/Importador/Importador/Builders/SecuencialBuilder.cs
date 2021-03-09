using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utilidades.Importador.Builders
{
    public class SecuencialBuilder<T> : Builder<T>
    {
        public SecuencialBuilder<T> AddColumn<TProperty>(int from, int to, Expression<Func<T, TProperty>> propertyLambda)
        {
            var column = new SecuencialColumn()
            {
                From = from,
                To = to
            };
            AddColumn(column, propertyLambda);
            return this;
        }


        public override void Generate(byte[] dataArray, out List<T> success, out List<ImportResult<T>> withErrors)
        {
            if (dataArray == null) throw new ArgumentNullException(nameof(dataArray));

            var data = Encoding.ASCII.GetString(dataArray);

            success = new List<T>();
            withErrors = new List<ImportResult<T>>();
            var rows = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < rows.Length - 1; i++)
            {
                var row = rows[i];
                var rowObject = Activator.CreateInstance<T>();

                if (string.IsNullOrEmpty(row) || !string.IsNullOrEmpty(row) && row.Contains('\n'))
                {
                    continue;
                }

                if (Interceptor == null || Interceptor.Pre(i, new[] { row }))

                    foreach (var column1 in Columns)
                    {
                        var column = (SecuencialColumn)column1;
                        var columnLengh = column.To - column.From;

                        if (columnLengh > row.Length)
                        {
                            throw new ApplicationException("La logintud definida es mayor a la de la fila");
                        }

                        var columnValue = row.Substring(column.From, columnLengh);
                        if (!string.IsNullOrEmpty(columnValue))
                        {
                            columnValue = columnValue.Trim();
                        }

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

                        column1.Property.SetValue(rowObject, objValue);
                    }


                if (Interceptor == null || Interceptor.Pos(i, ref rowObject))
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

        public override void GenerateFirstRowOnly(byte[] arrayBytes, out List<T> success, out List<ImportResult<T>> withErrors)
        {
            throw new NotImplementedException();
        }

        public override void LeerArchivoSuaf(byte[] arrayBytes, out List<T> devengados, out List<T> noProcesados, out List<T> conErrores)
        {
            throw new NotImplementedException();
        }
    }
}