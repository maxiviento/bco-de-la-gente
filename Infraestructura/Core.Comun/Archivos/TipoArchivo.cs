namespace Infraestructura.Core.Comun.Archivos
{
    public class TipoArchivo
    {
        private TipoArchivo(string value, string type, string extension)
        {
            Value = value;
            Type = type;
            Extension = extension;
        }

        public string Value { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }

        public static TipoArchivo PDF => new TipoArchivo("PDF", "application/pdf", ".pdf");
        public static TipoArchivo Excel => new TipoArchivo("Excel", "application/vnd.ms-excel", ".xls");
        public static TipoArchivo ExcelNew => new TipoArchivo("Excel", "application/vnd.ms-excel", ".xlsx");
        public static TipoArchivo Zip => new TipoArchivo("ZIP", "application/zip", ".zip");
        public static TipoArchivo Txt => new TipoArchivo("txt", "application/txt", ".txt");
    }
}
