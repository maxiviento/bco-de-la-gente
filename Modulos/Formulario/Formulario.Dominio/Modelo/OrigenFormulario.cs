using Infraestructura.Core.Comun.Dato;
using System;

namespace Formulario.Dominio.Modelo
{
    public class OrigenFormulario : Entidad
    {
        public static OrigenFormulario Web => new OrigenFormulario(1, "WEB");
        public static OrigenFormulario Ceder => new OrigenFormulario(2, "CEDER");
        public static OrigenFormulario Ong => new OrigenFormulario(3, "ONG");
        public static OrigenFormulario Bge => new OrigenFormulario(4, "BGE");
        public static OrigenFormulario Muni => new OrigenFormulario(5, "Municipio");
        public static OrigenFormulario IntComuna => new OrigenFormulario(6, "Intendente/Comuna");
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }

        protected OrigenFormulario()
        {
        }

        protected OrigenFormulario(int id, string nombre)
        {
            Id = new Id(id);
            Nombre = nombre;
        }

        public static OrigenFormulario ConId(int id)
        {
            switch (id)
            {
                case 1:
                    return Web;
                case 2:
                    return Ceder;
                case 3:
                    return Ong;
                case 4:
                    return Bge;
                case 5:
                    return Muni;
                case 6:
                    return IntComuna;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id),
                        "No existe Origen formulario para el ID solicitado");
            }
        }
    }
}