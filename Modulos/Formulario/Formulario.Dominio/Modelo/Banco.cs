using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Banco : Entidad
    {
        protected Banco()
        {
        }

        public Banco(Id id, string descripcion, Id idDepartamento)
        {
            Id = id;
            Descripcion = descripcion;
            IdDepartamento = idDepartamento;
            IdBanco = id.Valor.ToString();
        }

        public virtual string IdBanco { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual Id IdDepartamento { get; protected set; }
    }
}
