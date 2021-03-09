using System;
using System.Text.RegularExpressions;
using Infraestructura.Core.Comun.Dato;

namespace Infraestructura.Core.Datos
{
    public class DefinidorConvencion : IConventionDefinition
    {
        public string FromDatabaseColum(Type type, string originalColumnName)
        {
            //if (type == typeof(string))
            //    return string.Format("N_{0}", ReplaceUpperCaseWithUnderscore(originalColumName));

            if (type == typeof(bool))
                return originalColumnName.Replace("ES_", "");

            if (type == typeof(DateTime))
                return originalColumnName.Replace("FEC_", "");

            if (type == typeof(Id))
                return originalColumnName.Replace("ID_", "");

            return string.Empty;
        }

        public string ToDatabaseColumn(Type type, string originalColumName)
        {
           
            if (type == typeof(string))
                return string.Format("N_{0}", ReplaceUpperCaseWithUnderscore(originalColumName));

            if (type == typeof(bool))
                return string.Format("ES_{0}", ReplaceUpperCaseWithUnderscore(originalColumName));

            if (type == typeof(DateTime))
                return string.Format("FEC_{0}", ReplaceUpperCaseWithUnderscore(originalColumName));

            if (type == typeof(Id))
                return string.Format("ID_{0}", ReplaceUpperCaseWithUnderscore(originalColumName));

            return string.Empty;
        }

        private string ReplaceUpperCaseWithUnderscore(string input)
        {
            string replaced = Regex.Replace(input, @"(?<!_)([A-Z])", "_$1");
            return replaced;
        }
    }
}
