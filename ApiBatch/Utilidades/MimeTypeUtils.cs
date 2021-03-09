using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiBatch.Utilidades
{
    public class MimeTypeUtils
    {
        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
        {
            {"ai", "application/postscript"},
            {"aif", "audio/x-aiff"},
            {"aifc", "audio/x-aiff"},
            {"aiff", "audio/x-aiff"},
            {"asc", "text/plain"},
            {"atom", "application/atom+xml"},
            {"au", "audio/basic"},
            {"avi", "video/x-msvideo"},
            {"bcpio", "application/x-bcpio"},
            {"bin", "application/octet-stream"},
            {"cdf", "application/x-netcdf"},
            {"cgm", "image/cgm"},
            {"class", "application/octet-stream"},
            {"cpio", "application/x-cpio"},
            {"cpt", "application/mac-compactpro"},
            {"csh", "application/x-csh"},
            {"css", "text/css"},
            {"dcr", "application/x-director"},
            {"dif", "video/x-dv"},
            {"dir", "application/x-director"},
            {"djv", "image/vnd.djvu"},
            {"djvu", "image/vnd.djvu"},
            {"dll", "application/octet-stream"},
            {"dmg", "application/octet-stream"},
            {"dms", "application/octet-stream"},
            {"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {"docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {"dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {"dtd", "application/xml-dtd"},
            {"dv", "video/x-dv"},
            {"dvi", "application/x-dvi"},
            {"dxr", "application/x-director"},
            {"eps", "application/postscript"},
            {"etx", "text/x-setext"},
            {"exe", "application/octet-stream"},
            {"ez", "application/andrew-inset"},
            {"gram", "application/srgs"},
            {"grxml", "application/srgs+xml"},
            {"gtar", "application/x-gtar"},
            {"hdf", "application/x-hdf"},
            {"hqx", "application/mac-binhex40"},
            {"htm", "text/html"},
            {"html", "text/html"}, // no permitir importar este porque puede ser propensos a ataques XSS - Mili Zea Cárdenas
            {"ice", "x-conference/x-cooltalk"},
            {"ico", "image/x-icon"},
            {"ics", "text/calendar"},
            {"ief", "image/ief"},
            {"ifb", "text/calendar"},
            {"iges", "model/iges"},
            {"igs", "model/iges"},
            {"jnlp", "application/x-java-jnlp-file"},
            {"js", "application/x-javascript"},
            {"kar", "audio/midi"},
            {"latex", "application/x-latex"},
            {"lha", "application/octet-stream"},
            {"lzh", "application/octet-stream"},
            {"m3u", "audio/x-mpegurl"},
            {"m4a", "audio/mp4a-latm"},
            {"m4b", "audio/mp4a-latm"},
            {"m4p", "audio/mp4a-latm"},
            {"m4u", "video/vnd.mpegurl"},
            {"m4v", "video/x-m4v"},
            {"man", "application/x-troff-man"},
            {"mathml", "application/mathml+xml"},
            {"me", "application/x-troff-me"},
            {"mesh", "model/mesh"},
            {"mid", "audio/midi"},
            {"midi", "audio/midi"},
            {"mif", "application/vnd.mif"},
            {"mov", "video/quicktime"},
            {"movie", "video/x-sgi-movie"},
            {"mp2", "audio/mpeg"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mpe", "video/mpeg"},
            {"mpeg", "video/mpeg"},
            {"mpg", "video/mpeg"},
            {"mpga", "audio/mpeg"},
            {"ms", "application/x-troff-ms"},
            {"msh", "model/mesh"},
            {"mxu", "video/vnd.mpegurl"},
            {"nc", "application/x-netcdf"},
            {"oda", "application/oda"},
            {"ogg", "application/ogg"},
            {"pbm", "image/x-portable-bitmap"},
            {"pct", "image/pict"},
            {"pdb", "chemical/x-pdb"},
            {"pgm", "image/x-portable-graymap"},
            {"pgn", "application/x-chess-pgn"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {"ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {"ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {"pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {"potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {"ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {"ps", "application/postscript"},
            {"qt", "video/quicktime"},
            {"qti", "image/x-quicktime"},
            {"qtif", "image/x-quicktime"},
            {"ra", "audio/x-pn-realaudio"},
            {"ram", "audio/x-pn-realaudio"},
            {"ras", "image/x-cmu-raster"},
            {"rdf", "application/rdf+xml"},
            {"rgb", "image/x-rgb"},
            {"rm", "application/vnd.rn-realmedia"},
            {"roff", "application/x-troff"},
            {"rtf", "text/rtf"},
            {"rtx", "text/richtext"},
            {"sgm", "text/sgml"},
            {"sgml", "text/sgml"},
            {"sh", "application/x-sh"},
            {"shar", "application/x-shar"},
            {"silo", "model/mesh"},
            {"sit", "application/x-stuffit"},
            {"skd", "application/x-koan"},
            {"skm", "application/x-koan"},
            {"skp", "application/x-koan"},
            {"skt", "application/x-koan"},
            {"smi", "application/smil"},
            {"smil", "application/smil"},
            {"snd", "audio/basic"},
            {"so", "application/octet-stream"},
            {"spl", "application/x-futuresplash"},
            {"src", "application/x-wais-source"},
            {"sv4cpio", "application/x-sv4cpio"},
            {"sv4crc", "application/x-sv4crc"},
            {"svg", "image/svg+xml"},
            {"swf", "application/x-shockwave-flash"},
            {"t", "application/x-troff"},
            {"tar", "application/x-tar"},
            {"tcl", "application/x-tcl"},
            {"tex", "application/x-tex"},
            {"texi", "application/x-texinfo"},
            {"texinfo", "application/x-texinfo"},
            {"xsl", "application/xml"},
            {"tr", "application/x-troff"},
            {"tsv", "text/tab-separated-values"},
            {"ustar", "application/x-ustar"},
            {"vcd", "application/x-cdlink"},
            {"vrml", "model/vrml"},
            {"vxml", "application/voicexml+xml"},
            {"wav", "audio/x-wav"},
            {"wbmp", "image/vnd.wap.wbmp"},
            {"wbmxl", "application/vnd.wap.wbxml"},
            {"wml", "text/vnd.wap.wml"},
            {"wmlc", "application/vnd.wap.wmlc"},
            {"wmls", "text/vnd.wap.wmlscript"},
            {"wmlsc", "application/vnd.wap.wmlscriptc"},
            {"wrl", "model/vrml"},
            {"xbm", "image/x-xbitmap"},
            {"xht", "application/xhtml+xml"},
            {"xhtml", "application/xhtml+xml"},
            {"xml", "application/xml"},
            {"xpm", "image/x-xpixmap"},
            {"xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {"xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {"xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {"xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {"xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {"xslt", "application/xslt+xml"},
            {"xul", "application/vnd.mozilla.xul+xml"},
            {"xwd", "image/x-xwindowdump"},
            {"xyz", "chemical/x-xyz"},
            {FileExtension.Txt, "text/plain"},
            {FileExtension.Xls, "application/vnd.ms-excel"},
            {FileExtension.Xlsx, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {FileExtension.Doc, "application/msword"},
            {FileExtension.Docx, "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {FileExtension.Zip, "application/zip"},
            {FileExtension.Rar, "application/x-rar-compressed"},
            {FileExtension.Bmp, "image/bmp"},
            {FileExtension.Gif, "image/gif"},
            {FileExtension.Jp2, "image/jp2"},
            {FileExtension.Jpe, "image/jpeg"},
            {FileExtension.Jpeg, "image/jpeg"},
            {FileExtension.Jpg, "image/jpeg"},
            {FileExtension.Mac, "image/x-macpaint"},
            {FileExtension.Pic, "image/pict"},
            {FileExtension.Pict, "image/pict"},
            {FileExtension.Png, "image/png"},
            {FileExtension.Pnm, "image/x-portable-anymap"},
            {FileExtension.Pnt, "image/x-macpaint"},
            {FileExtension.Pntg, "image/x-macpaint"},
            {FileExtension.Ppm, "image/x-portable-pixmap"},
            {FileExtension.Tif, "image/tiff"},
            {FileExtension.Tiff, "image/tiff"},
            {FileExtension.Pdf, "application/pdf"},

            // Formatos libres
            {FileExtension.Odt, "application/vnd.oasis.opendocument.text" },
            {FileExtension.Odp, "application/vnd.oasis.opendocument.presentation" },
            {FileExtension.Ods, "application/vnd.oasis.opendocument.spreadsheet" },
        };

        public class FileExtension
        {
            public const string Xls = "xls";
            public const string Xlsx = "xlsx";
            public const string Txt = "txt";
            public const string Zip = "zip";
            public const string Rar = "rar";
            public const string Doc = "doc";
            public const string Docx = "docx";
            public const string Odt = "odt";
            public const string Odp = "odp";
            public const string Ods = "ods";
            public const string Bmp = "bmp";
            public const string Gif = "gif";
            public const string Jp2 = "jp2";
            public const string Jpe = "jpe";
            public const string Jpeg = "jpeg";
            public const string Jpg = "jpg";
            public const string Mac = "mac";
            public const string Pic = "pic";
            public const string Pict = "pict";
            public const string Png = "png";
            public const string Pnm = "pnm";
            public const string Pnt = "pnt";
            public const string Pntg = "pntg";
            public const string Ppm = "ppm";
            public const string Tif = "tif";
            public const string Tiff = "tiff";
            public const string Pdf = "pdf";

        }

        public static string Obtener_MimeType(string pFormato)
        {
            return MIMETypesDictionary.ContainsKey(pFormato.ToLower())
                ? MIMETypesDictionary[pFormato.ToLower()]
                : "unknown/unknown";
        }

        public static IEnumerable<string> Obtener_MimeTypes(IEnumerable<string> extensiones)
        {
            return extensiones.Select(Obtener_MimeType).ToList();
        }

        public static string Obtener_Extenxion(string mimeType)
        {
            var mime = MIMETypesDictionary.FirstOrDefault(m => m.Value == mimeType.ToLower());

            return !mime.Equals(default(KeyValuePair<string, string>))
                ? mime.Key
                : "unknown/unknown";
        }
    }
}