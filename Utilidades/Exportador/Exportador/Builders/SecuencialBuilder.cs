using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Utilidades.Exportador.Builders
{
    public class SecuencialBuilder<T> : Builder<T>
    {
        public SecuencialBuilder<T> AddColumn<TProperty>(string header, int from, int to,
            Expression<Func<T, TProperty>> propertyLambda)
        {
            var column = new SecuencialColumn()
            {
                From = from,
                To = to,
                Header = header
            };
            AddColumn(column, propertyLambda);
            return this;
        }

        public SecuencialBuilder<T> AddColumn<TProperty>(string header, int from, int to, char fillCharacter,
            Expression<Func<T, TProperty>> propertyLambda)
        {
            var column = new SecuencialColumn()
            {
                From = from,
                To = to,
                FillCharacter = fillCharacter,
                Header = header
            };
            AddColumn(column, propertyLambda);
            return this;
        }


        public override byte[] Generate(IList<T> data)
        {
            var str = GenerateAsString(data);
            var bytes = Encoding.ASCII.GetBytes(str);
            return bytes;
        }

        public override string GenerateAsString(IList<T> data)
        {
            var sbFile = new StringBuilder();
            foreach (var t in data)
            {
                var sbRaw = new StringBuilder();
                foreach (var column in Columns)
                {
                    var col = (SecuencialColumn) column;
                    var value = column.Property.GetValue(t);
                    var valueAsString = string.Empty;

                    if (value != null)
                    {
                        valueAsString = value.ToString();
                    }
                    
                    var width = (col.To - col.From);
                    if (valueAsString.Length>width)
                    {
                        valueAsString = valueAsString.Substring(0, width);
                    }
                    valueAsString = valueAsString.PadRight(width, col.FillCharacter.HasValue? col.FillCharacter.Value:' ');
                    sbRaw.Append(valueAsString);
                }

                sbFile.AppendLine(sbRaw.ToString());
            }

            return sbFile.ToString();
        }
    }
}