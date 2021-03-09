using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Vinculo
    {
        public Vinculo()
        {
        }

        public Vinculo(string id)
        {
            Id = id;
        }

        public Vinculo(string id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}
