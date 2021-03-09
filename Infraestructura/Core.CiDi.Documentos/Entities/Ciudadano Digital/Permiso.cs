using System;

namespace Core.CiDi.Documentos.Entities.Ciudadano_Digital
{
    public class Permiso
    {
        public Int16 IdTipoDocumentacion { get; set; }

        public String NombreTipoDocumentacion { get; set; }

        public String Upload { get; set; }

        public String Discard { get; set; }

        public String Acumulable { get; set; }
    }
}