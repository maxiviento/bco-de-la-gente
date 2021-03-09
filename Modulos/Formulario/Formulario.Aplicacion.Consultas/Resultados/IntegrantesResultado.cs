using System;
using Infraestructura.Core.Comun.Dato;
using AppComunicacion.ApiModels;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class IntegrantesResultado : ReporteFormularioResultado
    {
        // Datos Personales Resultado
        public string NroDocumento { get; set; }
        public int? IdNumero { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CodigoArea { get; set; }
        public string TelFijo { get; set; }
        public string CodigoAreaCelular { get; set; }
        public string TelCelular { get; set; }
        public string Mail { get; set; }
        public string SexoId { get; set; }
        public string NombreSexo { get; set; }
        public string CodigoPais { get; set; }
        public string Nacionalidad { get; set; }
        public string TipoDocumento { get; set; }
        public string Cuil { get; set; }
        public string FechaNacimiento { get; set; }
        public string Actividad { get; set; }
        public string TrabajaActualmente { get; set; }
        public string Edad { get; set; }
        //DOMICILIO
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Torre { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string Manzana { get; set; }
        public string Casa { get; set; }
        public string Barrio { get; set; }
        public string CodigoPostal { get; set; }
        public string DomicilioGrupoFamiliarLocalidad { get; set; }
        public string DomicilioGrupoFamiliarDepartamento { get; set; }
        //DS Integrantes

        public IntegrantesResultado(int orden)
        {
            this.Orden = orden;
        }

        public IntegrantesResultado(DatosPersonalesResultado persona, int orden, DatosContactoResultado datosContacto, string departamento, string localidad)
        {
            this.Apellido = persona.Apellido;
            this.Nombre = persona.Nombre;
            this.NombreSexo = persona.NombreSexo;
            this.CodigoPais = persona.CodigoPais;
            this.Nacionalidad = persona.Nacionalidad;
            this.TipoDocumento = persona.TipoDocumento;
            this.NroDocumento = persona.NroDocumento;
            this.SexoId = persona.SexoId;
            this.Cuil = persona.Cuil;
            this.FechaNacimiento = persona.FechaNacimiento;
            this.Actividad = persona.Actividad;
            this.TrabajaActualmente = persona.TrabajaActualmente;
            this.Edad = persona.Edad;
            this.Calle = persona.Calle;
            this.Numero = persona.Numero;
            this.Torre = persona.Torre;
            this.Piso = persona.Piso;
            this.Dpto = persona.Dpto;
            this.Manzana = persona.Manzana;
            this.CodigoPostal = persona.CodigoPostal;
            this.Casa = persona.Casa;
            this.Barrio = persona.Barrio;
            this.CodigoArea = datosContacto.CodigoArea;
            this.TelFijo = datosContacto.Telefono;
            this.CodigoAreaCelular = datosContacto.CodigoAreaCelular;
            this.TelCelular = datosContacto.Celular;
            this.Mail = datosContacto.Mail;
            this.Orden = orden;
            this.DomicilioGrupoFamiliarDepartamento = departamento;
            this.DomicilioGrupoFamiliarLocalidad = localidad;
        }
    }
}
