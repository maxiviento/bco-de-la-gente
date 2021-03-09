using Identidad.Dominio.Modelo;
namespace Configuracion.Aplicacion.Comandos
{
    public class ModificacionMotivoRechazoComando
    {
        public long? Id { get; set; }
        public string NombreNuevo { get; set; }
        public string DescripcionNueva { get; set; }
        public string AbreviaturaNueva { get; set; }
        public string CodigoNuevo { get; set; }
        public string NombreOriginal { get; set; }
        public string DescripcionOriginal { get; set; }
        public string AbreviaturaOriginal { get; set; }
        public string CodigoOriginal { get; set; }
    }
}
