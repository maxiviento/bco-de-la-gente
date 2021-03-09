using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Soporte.Dominio.Modelo
{
    public class Parametro: Entidad
    {
        public Parametro() { }

        public Parametro(long idParametro, string nombre, string tipoValor)
        {
            if(string.IsNullOrEmpty(nombre)) { throw new ModeloNoValidoException("El nombre del parámetro es requerido");}

            if (string.IsNullOrEmpty(tipoValor)) { throw new ModeloNoValidoException("El tipo de valor del parámetro es requerido"); }
            
            if(nombre.Length > 150) { throw new ModeloNoValidoException("El nombre del parámetro no puede ser mayor a 150 caracteres");}

            if(tipoValor.Length > 50) { throw new ModeloNoValidoException("El tipo de valor del párametro no puede ser mayor a 50 caracteres");}

            IdParametro = idParametro;
            Nombre = nombre;
            TipoValor = tipoValor;
        }
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual IList<VigenciaParametro> VigenciaParametro { get; protected set; }
        public virtual long IdParametro { get; set; }
        public virtual string TipoValor { get; set; }
    }
}
