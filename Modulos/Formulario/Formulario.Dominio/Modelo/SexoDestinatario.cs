using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class SexoDestinatario : Entidad
    {
        public static SexoDestinatario Masculino => new SexoDestinatario(1, "MASCULINO", "MASCULINO");
        public static SexoDestinatario Femenino => new SexoDestinatario(2, "FEMENINO", "FEMENINO");
        public static SexoDestinatario Ambos => new SexoDestinatario(3, "AMBOS", "AMBOS");
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }

        public SexoDestinatario()
        {
        }

        public SexoDestinatario(int id, string nombre, string descripcion)
        {
            Id = new Id(id);
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public static SexoDestinatario ConId(int id)
        {
            switch (id)
            {
                case 1:
                    return Masculino;
                case 2:
                    return Femenino;
                case 3:
                    return Ambos;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id),
                        "No existe Sexo destinatario formulario para el ID solicitado");
            }
        }
    }
}