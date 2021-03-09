using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Curso : Entidad
    {
        protected Curso()
        {
        }

        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual string Observaciones { get; protected set; }
        public virtual TipoCurso TipoCurso { get; protected set; }

        public void AgregarObservaciones(string observacion)
        {
            if (Id.Valor == 99 || Id.Valor == 999)
            {
                Observaciones = observacion;
            }
        }
    }
}