namespace Formulario.Aplicacion.Comandos
{
    public class RequisitoComando
    {
        public long IdPrestamo { get; set; }
        public long Id { get; set; }
        public bool EsSolicitante { get; set; }
        public bool EsGarante { get; set; }
        public bool EsSolicitanteGarante { get; set; }
        public string Observaciones { get; set; }
    }
}