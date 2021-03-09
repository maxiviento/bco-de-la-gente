using System;
using System.Collections.Generic;

namespace Core.CiDi.Documentos.Entities.Ciudadano_Digital
{
    public class Usuario
    {
        public String CUIL { get; set; }
        public String CuilFormateado { get; set; }
        public String NroDocumento { get; set; }
        public String Apellido { get; set; }
        public String Nombre { get; set; }
        public String NombreFormateado { get; set; }
        public String FechaNacimiento { get; set; }
        public String Id_Sexo { get; set; }
        public String PaiCodPais { get; set; }
        public int Id_Numero { get; set; }
        public int? Id_Estado { get; set; }
        public String Estado { get; set; }
        public String Email { get; set; }
        public String TelArea { get; set; }
        public String TelNro { get; set; }
        public String TelFormateado { get; set; }
        public String CelArea { get; set; }
        public String CelNro { get; set; }
        public String CelFormateado { get; set; }
        public String Empleado { get; set; }
        public String Id_Empleado { get; set; }
        public String FechaRegistro { get; set; }
        public String FechaBloqueo { get; set; }
        public String IdAplicacionOrigen { get; set; }
        public Domicilio Domicilio { get; set; }
        public String TieneRepresentados { get; set; }
        public String CodigoIngresado { get; set; }
        public String Constatado { get; set; }
        public Representado Representado { get; set; }
        public Respuesta Respuesta { get; set; }
        public List<String> Roles { get; set; }
        public List<Permiso> Permisos { get; set; }
        public List<Ubicacion> Ubicaciones { get; set; }

        public Usuario()
        {
            Domicilio = new Domicilio();
            Representado = new Representado();
            Respuesta = new Respuesta();
            Roles = new List<String>();
            Permisos = new List<Permiso>();
            Ubicaciones = new List<Ubicacion>();
        }
    }
}