using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using NHibernate;
using NHibernate.Exceptions;

namespace Infraestructura.Core.Datos.DSL
{
    public sealed partial class StoreProcedure
    {
        private int _positionParameter;
        private readonly IList<Parameter> _parameters;
        private readonly string _name;
        private readonly ISession _sesion;
        private readonly IConventionDefinition _definition;

        public StoreProcedure(string sp, ISession sesion)
        {
            if (sp == null)
                throw new ArgumentNullException("El nombre del procedimiento almacenado no puede ser nulo.");
            if (sesion == null)
                throw new ArgumentNullException("La sesion no puede ser nula.");
            if (!sesion.IsOpen)
                throw new ArgumentException("La sesion se encuentra cerrada.");
            _sesion = sesion;
            _parameters = new List<Parameter>();
            _positionParameter = 0;
            _name = sp;
            _definition = new DefinidorConvencion();
        }

        #region AddParam

        public StoreProcedure AddParam(string name, object param, Type type)
        {
            _parameters.Add(new Parameter(name, param, type));
            _positionParameter = -1;
            return this;
        }


        public StoreProcedure AddParam(object param, Type type)
        {
            _parameters.Add(new Parameter(_positionParameter, param, type));
            _positionParameter++;
            return this;
        }

        #endregion

        /**
         *  Agrega parametros, sin pasarlos a mayuculas por defecto
         * 
         * */

        public StoreProcedure AddParamInvariantCase(string param)
        {
            return AddParam(param.ToUpper(), typeof(string));
        }

        public StoreProcedure AddParamFromQuery<T>(T query)
        {
            if (query == null)
                throw new ArgumentNullException("El parametro no puede ser null");
            ExtractParameters(query);
            return this;
        }

        public IList<T> ToListResult<T>() where T : class
        {
            try
            {
                var query = BuildSqlQuery();
                query.SetResultTransformer(new OracleResultTransformer<T>(_definition, _sesion));
                PrintInformation(query, typeof(T));
                return query.List<T>();
            }
            catch (GenericADOException ex)
            {
                throw new ErrorOracleException("Error Base de datos", ex);
            }
        }

        public T ToUniqueResult<T>() where T : class
        {
            try
            {
                var query = BuildSqlQuery();
                query.SetResultTransformer(new OracleResultTransformer<T>());
                PrintInformation(query, typeof(T));
                return query.UniqueResult<T>();
            }
            catch (GenericADOException ex)
            {
                throw new ErrorOracleException("Error Base de datos", ex);
            }
        }

        public T ToEscalarResult<T>()
        {
            try
            {
                var query = BuildSqlQuery();
                query.SetResultTransformer(new OracleResultTransformer<ScalarValue<T>>());
                PrintInformation(query, typeof(T));
                var result = query.UniqueResult<ScalarValue<T>>();
                return result.Valor;
            }
            catch (GenericADOException ex)
            {
                throw new ErrorOracleException("Error Base de datos", ex);
            }
        }

        public SpResult ToSpResult()
        {
            var query = BuildSqlQuery();
            query.SetResultTransformer(new OracleResultTransformer<SpResult>());
            PrintInformation(query);
            var result = query.UniqueResult<SpResult>();
            ValidateSpSuccess(result.Mensaje, result.Error);
            return result;
        }

        public void ValidateSpSuccess(string mensaje, string oraErrorCode)
        {
            var spResult = SpResultadoEnum.Ok;
            var result = string.IsNullOrEmpty(mensaje) ? string.Empty : mensaje.ToUpper();
            if (!spResult.ToString().ToUpper().Equals(result))
            {
                if(_sesion.Transaction.IsActive)
                    _sesion.Transaction.Rollback();
                throw new ErrorOracleException(mensaje + ". " + oraErrorCode);
            }
        }

        public SpResult ToMessageResult()
        {
            var query = BuildSqlQuery();
            query.SetResultTransformer(new OracleResultTransformer<SpResult>());
            PrintInformation(query);
            var result = query.UniqueResult<SpResult>();
            ValidateSpMessageSuccess(result.Mensaje, result.Error);
            return result;
        }

        public void ValidateSpMessageSuccess(string mensaje, string oraErrorCode)
        {
            if (!string.IsNullOrEmpty(oraErrorCode))
            {
                if (_sesion.Transaction.IsActive)
                    _sesion.Transaction.Rollback();
                throw new ErrorOracleException(mensaje + ". " + oraErrorCode);
            }
        }

        public void JustExecute()
        {
            BuildSqlQuery().ExecuteUpdate();
        }

        private ISQLQuery BuildSqlQuery()
        {
            var sp = BuildStoreProcedureName();
            var query = _sesion.CreateSQLQuery(sp);
            foreach (var parameter in _parameters)
            {
                if (IsPosicional())
                    query.SetParameter(parameter.Position, parameter.Value, parameter.TypeParameter);
                else
                    query.SetParameter(parameter.Name, parameter.Value, parameter.TypeParameter);
            }
            return query;
        }

        private string BuildStoreProcedureName()
        {
            var stringBuilderParms = new StringBuilder();
            foreach (Parameter parameter in _parameters)
            {
                if (IsPosicional())
                {
                    stringBuilderParms.Append("? ,");
                }
                else
                {
                    stringBuilderParms.Append(string.Format("{0} => :{0} ,", parameter.Name));
                }
            }
            if (_parameters.Count > 0)
                stringBuilderParms.Remove(stringBuilderParms.Length - 1, 1);
            var parameters = stringBuilderParms.ToString();
            var finalSpName = string.Format("{{ call {0} ({1}) }}", _name, parameters);
            return finalSpName;
        }

        private bool IsPosicional()
        {
            return _parameters.All(x => x.Position != -1);
        }

        private void ExtractParameters<T>(T entidad)
        {
            foreach (var propertyInfo in entidad.GetType().GetProperties())
            {
                var hasInvariantCase = Attribute.IsDefined(propertyInfo, typeof(InvariantCaseAttribute));
                var hasSkepParameter = Attribute.IsDefined(propertyInfo, typeof(SkepParameterAttribute));

                if (hasSkepParameter) continue;

                var paramterValue = propertyInfo.GetValue(entidad);
                var parameterType = propertyInfo.PropertyType;
                if (!hasInvariantCase && paramterValue is string)
                {
                    paramterValue = ((string) paramterValue).ToUpper();
                }
                
                AddParamUndefined(propertyInfo.Name, paramterValue, parameterType);
            }
        }
        
        private void PrintInformation(ISQLQuery query, Type returnType = null)
        {
            Debug.WriteLine("********************[ Llamada a SP]**************************");
            Debug.WriteLine("");
            Debug.WriteLine(query.QueryString);
            Debug.WriteLine("");
            Debug.WriteLine("    - Con Parametros: ");
            Debug.WriteLine("");
            foreach (var parameter in _parameters)
            {
                if (IsPosicional())
                    Debug.WriteLine(string.Format("                    {0}) {1}", parameter.Position, parameter.Value));
                else Debug.WriteLine(string.Format("                    {0}) {1}", parameter.Name, parameter.Value));
            }
            Debug.WriteLine("    - Y Columnas: ");
            if (returnType == null)
            {
                Debug.WriteLine("");
                Debug.WriteLine("*******************[ Fin SP]***********************************");
                return;
            }
            foreach (var properyInfo in returnType.GetProperties())
            {
                Debug.WriteLine(string.Format("                    {0}", properyInfo.Name));
            }
            Debug.WriteLine("");
            Debug.WriteLine("*******************[ Fin SP]***********************************");
        }
    }
}