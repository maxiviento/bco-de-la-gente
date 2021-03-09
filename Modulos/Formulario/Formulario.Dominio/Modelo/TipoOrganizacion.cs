using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoOrganizacion: Entidad
    {
        public TipoOrganizacion() { }
        public TipoOrganizacion(int id)
        {
            Id = new Id(id);
        }
        public TipoOrganizacion(int id, string nombre, string descripcion)
        {
            Id = new Id(id);
            Nombre = nombre;
            Descripcion = descripcion;
        }
        public string Nombre { get; protected set; }
        public string Descripcion { get; protected set; }
    }
}
