using Infraestructura.Core.Comun.Dato;

namespace Identidad.Dominio.Modelo
{
    public class MotivoDeBaja : Entidad
    {
        protected MotivoDeBaja()
        {
        }

        public MotivoDeBaja(int id, string nombre, Ambito ambito)
        {
            this.Id = new Id(id);
            this.Nombre = nombre;
            this.Ambito = ambito;
        }

        public virtual string Nombre { get; protected set; }
        public virtual Ambito Ambito { get; protected set; }
    }
}