using System;
using System.Text;

namespace Formulario.Dominio.Modelo
{
    public class Persona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string SexoId { get; set; }
        public string NombreSexo { get; set; }
        public string CodigoPais { get; set; }
        public string Nacionalidad { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaDefuncion { get; set; }

        public string Edad
        {
            get
            {
                var edadString = "";
                if (FechaNacimiento.HasValue)
                {
                    var fechaCalculo = FechaDefuncion?? DateTime.Today;
                    var age = fechaCalculo.Year - FechaNacimiento.Value.Year;
                    if (FechaNacimiento > fechaCalculo.AddYears(-age)) age--;
                    edadString = age.ToString();
                }
                return edadString;
            }
        }

        public int DomicilioIdVin { get; set; }
        public string DomicilioGrupoFamiliar { get; set; }
        public string DomicilioGrupoFamiliarLocalidad { get; set; }
        public string DomicilioGrupoFamiliarDepartamento { get; set; }
        public int DomicilioRealIdVin { get; set; }
        public string DomicilioReal { get; set; }
        public string DomicilioRealLocalidad { get; set; }
        public string DomicilioRealDepartamento { get; set; }
        public string Barrio { get; set; }
        public string Cuil { get; set; }
        public string CodigoArea { get; set; }
        public string Telefono { get; set; }
        public string CodigoAreaCelular { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int? IdNumero { get; set; }

        public bool EsMismaPersona(Persona otraPersona)
        {
            return NroDocumento == otraPersona.NroDocumento &&
                   SexoId == otraPersona.SexoId &&
                   CodigoPais == otraPersona.CodigoPais;
        }

        public string NombreCompleto
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Apellido))
                    stringBuilder.Append(this.Apellido);
                if (!string.IsNullOrEmpty(this.Nombre))
                    stringBuilder.Append(", " + this.Nombre);
                return stringBuilder.ToString();
            }
        }
    }
}