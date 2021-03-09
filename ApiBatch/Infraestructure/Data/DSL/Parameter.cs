using System;
using System.Text.RegularExpressions;

using NHibernate;

namespace ApiBatch.Infraestructure.Data.DSL
{
    public class Parameter
    {

        public string Name { get; set; }
        public object Value { get; set; }
        public int Position { get; set; }
        public NHibernate.Type.IType TypeParameter { get; set; }
        
        public Parameter(string name, object value, Type typeParameter)
        {
            this.Name = $"P{ReplaceUpperCaseWithUnderscore(name)}".ToUpper();
            this.Value = value;
            this.Position = -1;
            this.TypeParameter = GetNHibernateType(typeParameter);
        }
        public Parameter(int position, object value, Type typeParameter)
        {
            this.Position = position;
            this.Value = value;
            this.TypeParameter = GetNHibernateType(typeParameter); 
        }

        private static string ReplaceUpperCaseWithUnderscore(string input)
        {
            return Regex.Replace(input, @"(?<!_)([A-Z])", "_$1");
        }

        private static NHibernate.Type.IType GetNHibernateType(Type valueType)
        {
            if (TypeChecker.IsInteger(valueType) || TypeChecker.IsId(valueType))
                return NHibernateUtil.Int64;
            if (TypeChecker.IsDecimal(valueType))
                return NHibernateUtil.Double;
            if (TypeChecker.IsText(valueType))
                return NHibernateUtil.String;
            if (TypeChecker.IsBoolean(valueType))
                return NHibernateUtil.Character;
            if (TypeChecker.IsCharacter(valueType))
                return NHibernateUtil.Character;
            return TypeChecker.IsDate(valueType) ? NHibernateUtil.DateTime : null;
        }
    }
}
