using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public class GenerarObjetosTest
    {

        public class ObjetoA
        {
            protected ObjetoA()
            {
            }

            public Id Id { get; set; }
            public string Nombre { get; set; }
            public DateTime FechaAlta { get; set; }

            public ObjetoB ObjetoHijo { get; set; }

        }

        public class ObjetoB
        {
            protected ObjetoB()
            {
            }

            public Id Id { get; set; }
            public string Nombre { get; set; }
            public DateTime FechaAlta { get; set; }

            public ObjetoA ObjetoDobleHijo { get; set; }

        }



        [Test]
        public void GenerarObjetoTest()
        {
            var aliases = new string[] { "ObjetoHijo_ObjetoDobleHijo_Nombre", "ObjetoHijo_Id", "ObjetoHijo_Nombre" };
            var tuple = new object[] { "alejandro", new Id(1), "manuel" };

            ObjetoA instance = null;

            for (int i = 0; i < aliases.Length; i++)
            {
                Generate(aliases[i], tuple[i], ref instance);
            }

        }

        public void Generate<T>(string path, object value, ref T root) where T : class
        {
            //generar una cola, 
            var queue = new Queue<KeyValuePair<string, object>>();

            var properties = path.Split('_');
            foreach (var property in properties)
            {
                KeyValuePair<string, object> keyValue;
                keyValue = new KeyValuePair<string, object>(property, value);
                queue.Enqueue(keyValue);
            }

            GenerarObjeto(queue, ref root);

        }

        public object GenerarObjeto<T>(Queue<KeyValuePair<string, object>> queue, ref T root) where T : class
        {
            if (queue.Count == 0)
            {
                return null;
            }

            if (root == null)
            {
                root = (T)Activator.CreateInstance(typeof(T), true);
            }
            var keyValue = queue.Dequeue();

            var properyName = keyValue.Key;
            var property = root.GetType().GetProperty(properyName);
            var propertyType = property.PropertyType;

            object propertyValue = property.GetValue(root);


            if (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(Id))
            {
                if (propertyValue is Id && ((Id)propertyValue).IsDefault())
                {
                    propertyValue = keyValue.Value;
                }
                else
                {
                    propertyValue = propertyValue ?? keyValue.Value;
                }

            }
            else
            {
                propertyValue = propertyValue ?? Activator.CreateInstance(propertyType, true);
            }



            GenerarObjeto(queue, ref propertyValue);
            property.SetValue(root, propertyValue);
            return propertyValue;
        }
    }
}
