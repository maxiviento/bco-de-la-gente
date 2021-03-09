﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;


namespace Infraestructura.Core.Filtros
{
    public class HandleFromUriParam : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            string[] splitter = null;
            var fromUriParams = apiDescription.ActionDescriptor.GetParameters().
                Where(param => param.GetCustomAttributes<FromUriAttribute>().Any()).ToArray();

            foreach (var param in fromUriParams)
            {
                var fromUriAttribute = param.GetCustomAttributes<FromUriAttribute>().FirstOrDefault();

                // Check
                if (fromUriAttribute != null)
                {
                    var operationParam = operation.parameters;

                    foreach (var item in operationParam)
                    {
                        if (item.name.Contains(param.ParameterName))
                        {
                            splitter = item.name.Split('.');
                            item.name = splitter[splitter.Length - 1];
                        }
                    }
                }
            }
        }
    }
}
