using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoRequisito : Entidad
    {
        public TipoRequisito()
        {
        }

        public TipoRequisito(int id, string abreviatura, string descripcion)
        {
            Id =new Id(id);
            Abreviatura = abreviatura;
            Descripcion = descripcion;
        }

        public virtual string Abreviatura { get; protected set; }
        public virtual string Descripcion { get; protected set; }

        public static TipoRequisito Linea => new TipoRequisito(1, "L","Linea");
        public static TipoRequisito Checklist => new TipoRequisito(2, "C", "Checklist");
        public static TipoRequisito Todos => new TipoRequisito(3, "T", "Todos");
     
    }
}