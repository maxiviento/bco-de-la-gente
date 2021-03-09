using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utilidades.Exportador.Builders
{
    public class SeparatedValuesBuilder<T> : Builder<T>
    {
        private readonly string _separator;
        private readonly string _newLine;

        public SeparatedValuesBuilder(string separator)
        {
            _separator = separator;
            _newLine = Environment.NewLine;
        }

        public SeparatedValuesBuilder(string separator, string newLine)
        {
            _separator = separator;
            _newLine = newLine;
        }

        public SeparatedValuesBuilder<T> AddColumn<TProperty>(int position, Expression<Func<T, TProperty>> propertyLambda)
        {
            var column = new PositionalColumn()
            {
                Index = position
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

                var columns = Columns
                    .Select(c => (PositionalColumn) c)
                    .OrderBy(c => c.Index)
                    .ToList();
                
                var actual = 0;

                foreach (var column in columns)
                {
                    for (var i = 0; i < column.Index - actual; i++)
                    {
                        sbRaw.Append(_separator);
                    }

                    actual = column.Index;

                    var value = column.Property.GetValue(t);
                    var valueAsString = string.Empty;

                    if (value != null)
                    {
                        valueAsString = value.ToString();
                    }

                    sbRaw.Append(valueAsString);
                }

                sbFile.Append(sbRaw.ToString());
                sbFile.Append(_newLine);
            }

            return sbFile.ToString();
        }
    }
}
