using System;

namespace Core.CiDi.Documentos.Entities
{
    public class Documentacion
    {
        public Int32 Id_Documento { get; set; }
        public String N_Catalogo { get; set; }
        public String Extension { get; set; }
        public String Peso_MB { get; set; }
        public String Paginas { get; set; }
        public DateTime? Vigencia { get; set; }
        public String N_Usuario { get; set; }
        public String N_Constatado { get; set; }
        public String N_Descripcion { get; set; }  
        
        //public int IdDocumento { get; set; }
        //public int IdUsuario { get; set; }
        //public int IdTipo { get; set; }
        //public String TipoDescripcion { get; set; }
        //public String NombreTipo { get; set; }
        //public String FechaCreacion { get; set; }
        //public String FechaVencimiento { get; set; }
        //public int IdUbicacion { get; set; }
        //public String UbicacionFisica { get; set; }
        //public String IdOperador { get; set; }
        //public int IdOrganismo { get; set; }
        //public String Organismo { get; set; }
        //public byte[] Imagen { get; set; }
        //public byte[] VistaPrevia { get; set; }
        //public String Extension { get; set; }
        //public String Descripcion { get; set; }
        //public String Acumulable { get; set; }
        //public String Repositorio { get; set; }
        //public String CuilOperador { get; set; }
        //public String NombreOperador { get; set; }
    }
}