using System;

namespace Core.CiDi.Documentos.Entities.Ciudadano_Digital
{
    public class Documentacion
    {
        public int IdDocumento { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipo { get; set; }
        public String TipoDescripcion { get; set; }
        public String NombreTipo { get; set; }
        public String FechaCreacion { get; set; }
        public String FechaVencimiento { get; set; }
        public int IdUbicacion { get; set; }
        public String UbicacionFisica { get; set; }
        public String IdOperador { get; set; }
        public int IdOrganismo { get; set; }
        public String Organismo { get; set; }
        public byte[] Imagen { get; set; }
        public byte[] VistaPrevia { get; set; }
        public String Extension { get; set; }
        public String Descripcion { get; set; }
        public String Acumulable { get; set; }
        public String Repositorio { get; set; }
        public String CuilOperador { get; set; }
        public String NombreOperador { get; set; }
    }
}