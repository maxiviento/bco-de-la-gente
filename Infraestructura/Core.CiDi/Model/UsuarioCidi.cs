﻿namespace Infraestructura.Core.CiDi.Model
{
    public class UsuarioCidi : IUsuario
    {
        public string CUIL { get; set; }
        public string CuilFormateado { get; set; }
        public string NroDocumento { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string NombreFormateado { get; set; }
        public string FechaNacimiento { get; set; }
        public string Id_Sexo { get; set; }
        public string PaiCodPais { get; set; }
        public int Id_Numero { get; set; }
        public int? Id_Estado { get; set; }
        public string Estado { get; set; }
        public string Email { get; set; }
        public string TelArea { get; set; }
        public string TelNro { get; set; }
        public string TelFormateado { get; set; }
        public string CelArea { get; set; }
        public string CelNro { get; set; }
        public string CelFormateado { get; set; }
        public string Empleado { get; set; }
        public string Id_Empleado { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaBloqueo { get; set; }
        public string IdAplicacionOrigen { get; set; }
        public string TieneRepresentados { get; set; }
        public Domicilio Domicilio { get; set; }
        public Representado Representado { get; set; }
        public RespuestaUsuario Respuesta { get; set; }
        public UsuarioCidi()
        {
            Domicilio = new Domicilio();
            Representado = new Representado();
            Respuesta = new RespuestaUsuario();
        }
    }
}
