namespace Infraestructura.Core.Comun.Archivos
{
    public class ArchivoBase64
    {
        public ArchivoBase64(string archivo, TipoArchivo tipo, string nombreSinExtension)
        {
            Archivo = archivo;
            Tipo = tipo;
            Nombre = nombreSinExtension;
        }

        public string Archivo { get; set; }
        public TipoArchivo Tipo { get; set; }
        public string Nombre { get; set; }

        public string NombreConExtension
        {
            get
            {
                string res = "";
                if (!string.IsNullOrEmpty(Nombre) && !string.IsNullOrEmpty(Tipo?.Extension))
                {
                    return Nombre + Tipo.Extension;
                }
                return res;
            }
        }
    }
}
