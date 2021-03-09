using Microsoft.Reporting.WebForms;

namespace Infraestructura.Core.Reportes
{
    public class Reporte
    {
        public Reporte() { }
        public Reporte(byte[] content, Warning[] warning, string[] streamIds,
            string mimeType, string encoding, string filenameExtentions)
        {
            Content = content;
            Warnings = warning;
            StreamIds = streamIds;
            MimeType = mimeType;
            Encoding = encoding;
            FilenameExtension = filenameExtentions;
        }

        public Reporte(byte[] content, string filenameExtentions,string mimeType)
        {
            Content = content;
            Warnings = new Warning[0];
            StreamIds = new string[0];
            MimeType = mimeType;
            Encoding = null;
            FilenameExtension = filenameExtentions;


        }

        public Warning[] Warnings { get; private set; }
        public string[] StreamIds { get; private set; }
        public string MimeType { get; private set; }
        public string Encoding { get; private set; }
        public string FilenameExtension { get; private set; }
        public byte[] Content { get; private set; }
    }
}
