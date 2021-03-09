namespace ApiBatch.Utilidades
{
    public class Archivo
    {
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string Buffer { get; set; }
        public string NombrePrestador { get; set; }
        public byte[] BufferArrayBytes { get; set; }
        public string RutaArchivo { get; set; }

        public Archivo() { }

        public Archivo(string fileName, string mediaType, string buffer, string nombrePrestador = "", byte[] bufferArrayBytes = null, string rutaArchivo = "")
        {
            FileName = fileName;
            MediaType = mediaType;
            Buffer = buffer;
            NombrePrestador = nombrePrestador;
            BufferArrayBytes = bufferArrayBytes;
            RutaArchivo = rutaArchivo;
        }
    }

}