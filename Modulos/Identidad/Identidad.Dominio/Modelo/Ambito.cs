using Infraestructura.Core.Comun.Dato;

namespace Identidad.Dominio.Modelo
{
    public class Ambito : Entidad
    {
        protected Ambito()
        {
        }

        public Ambito(int id, string nombre)
        {
            this.Id = new Id(id);
            this.Nombre = nombre;
        }
        //Ambitos de la base de datos de Seguridad
        public static Ambito USUARIO => new Ambito(1, "Usuario");
        public static Ambito PERFIL => new Ambito(2, "Perfil");

        //Ambitos de la base de datos de BGE
        public static Ambito FORMULARIO => new Ambito(1, "Formulario");
        public static Ambito LINEA => new Ambito(2, "Linea");
        public static Ambito AREA => new Ambito(3, "Area");
        public static Ambito ITEM => new Ambito(4, "Item");
        public static Ambito ETAPA => new Ambito(5, "Etapa");
        public static Ambito PRESTAMO => new Ambito(6, "Prestamo");
        public static Ambito TABLA_DEFINIDA => new Ambito(10, "Parametro_Tabla_Definida");
        public static Ambito CHECKLIST => new Ambito(12,"Checklist");

        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; set; }
    }
}