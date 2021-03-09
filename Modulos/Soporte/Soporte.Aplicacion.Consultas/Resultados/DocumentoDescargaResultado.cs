namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class DocumentoDescargaResultado
    {
        public string FileName { get; set; }
        public byte[] Blob { get; set; }
        public string Extension { get; set; }
    }
}