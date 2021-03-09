using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Programa: Entidad
    {

        public string Nombre { get; set; }

        public Programa() { }

        public Programa(string nombre)
        {
            Nombre = nombre;
        }
   }
}
