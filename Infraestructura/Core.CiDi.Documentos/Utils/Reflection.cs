using System;
using System.Reflection;

namespace Core.CiDi.Documentos.Utils
{
    /// <summary>
    /// Clase static con metodos Reflection
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Extension de 'Object' que copia las propiedades del objeto de destino
        /// </summary>
        /// <param name="objOrigen">Objeto de origen.</param>
        /// <param name="objDestino">Objeto de destino.</param>
        public static void Copiar_Propiedades(this object objOrigen, object objDestino)
        {
            // Si alguno es nulo se lanza una excepcion
            if (objOrigen == null || objDestino == null)
                throw new Exception("Objetos de origen y/o destino son nulos");
            
            // Obtengo los tipos de cada objeto
            Type tipoDestino = objDestino.GetType();
            Type tipoOrigen = objOrigen.GetType();

            // Itero las propiedades de la instancia del objeto origen
            PropertyInfo[] propiedadesObjetoOrigen = tipoOrigen.GetProperties();
            foreach (PropertyInfo itemPropiedadObjOrigen in propiedadesObjetoOrigen)
            {
                if (!itemPropiedadObjOrigen.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = tipoDestino.GetProperty(itemPropiedadObjOrigen.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(itemPropiedadObjOrigen.PropertyType))
                {
                    continue;
                }
               
                // Seteo el valor una vez hecha las validaciones
                targetProperty.SetValue(objDestino, itemPropiedadObjOrigen.GetValue(objOrigen, null), null);
            }
        }
    }
}