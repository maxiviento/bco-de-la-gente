using System;

namespace Core.CiDi.Documentos.Entities.Auditoria
{
    public class AuditoriaGeneral
    {
        public Int32 Id_Auditoria { get; set; }

        public Int32 Id_Documento { get; set; }

        public DateTime Fecha { get; set; }

        public String Usuario_Propietario { get; set; }

        public String Nombre_Apellido_Usuario_Propietario { get; set; }

        public string Resultado { get; set; }

        public Int16 Tipo_Operacion { get; set; }

        public Int32 Id_Aplicacion_Origen { get; set; }

        public String Descripcion { get; set; }

        public String Id_Documentacion { get; set; }

        public byte[] Imagen_Documentacion { get; set; }

        public Int32 Id_Catalogo { get; set; }

        public String N_Catalogo { get; set; }

        public String Extension { get; set; }

        public String Peso_MB { get; set; }

        public String Paginas { get; set; }

        public DateTime? Vigencia { get; set; }

        public Int32 ID_Usuario_Operador { get; set; }

        public String N_Usuario_Operador { get; set; }

        public String Apellido_Usuario_Operador { get; set; }

        public String Nombre_Usuario_Operador { get; set; }

        public String Num_Documento_Usuario_Operador { get; set; }

        public String Sexo_Usuario_Operador { get; set; }

        public String Email_Usuario_Operador { get; set; }

        public String N_Usuario_Solicitud { get; set; }
    }
}