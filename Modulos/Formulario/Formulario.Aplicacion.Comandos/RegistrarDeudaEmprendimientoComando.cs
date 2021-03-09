using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarDeudaEmprendimientoComando
    {
        public Id? Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public Id IdTipoDeudaEmprendimiento { get; set; }
    }
}