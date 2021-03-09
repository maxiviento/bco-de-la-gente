using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.Comun;
using System.Linq;
using System.Text;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DatosPersonalesResultado : ReporteFormularioResultado
    {
        public DatosPersonalesResultado() { }

        public DatosPersonalesResultado(PersonaUnica persona, UsuarioCidi usuarioCidi)
        {
            Apellido = persona.Apellido;
            Nombre = persona.Nombre;
            NombreSexo = persona.Sexo.Nombre;
            CodigoPais = persona.PaisTD.IdPais;
            Nacionalidad = persona.PaisTD.Nombre;
            TipoDocumento = persona.TipoDocumento.Nombre;
            NroDocumento = persona.NroDocumento;
            SexoId = persona.Sexo.IdSexo;
            IdNumero = persona.Id_Numero;
            Cuil = persona.CuilFormateado;
            FechaNacimiento = persona.FechaNacimientoFormateada;
            FechaDefuncion = persona.FechaDefuncion?.ToString("dd/MM/yyyy") ?? "";
            Edad = persona.Edad();
            Calle = "";
            Casa = persona.DomicilioReal?.Lote ?? persona.DomicilioLegal?.Lote ?? "";
            Numero = "";
            Torre = "";
            Piso = "";
            Dpto = "";
            Manzana = "";
            EstadoCivil = persona.EstadoCivil.Nombre;
            if (persona.DomicilioGrupoFamiliar != null)
            {
                IdVinDomicilio = persona.DomicilioGrupoFamiliar.IdVin;
                Calle = persona.DomicilioGrupoFamiliar.Calle != null ? persona.DomicilioGrupoFamiliar.Calle.Nombre : "";
                Numero = persona.DomicilioGrupoFamiliar.Altura;
                Torre = persona.DomicilioGrupoFamiliar.Torre;
                Piso = persona.DomicilioGrupoFamiliar.Piso;
                Dpto = persona.DomicilioGrupoFamiliar.Dpto;
                Manzana = persona.DomicilioGrupoFamiliar.Manzana;
                Casa = persona.DomicilioGrupoFamiliar.Lote;
                Barrio = persona.DomicilioGrupoFamiliar.Barrio != null ? persona.DomicilioGrupoFamiliar.Barrio.Nombre : "";
                CodigoPostal = persona.DomicilioGrupoFamiliar.CodigoPostal;
                DomicilioGrupoFamiliarLocalidad = persona.DomicilioGrupoFamiliar.Localidad.Nombre ?? "";
                DomicilioGrupoFamiliarDepartamento = persona.DomicilioGrupoFamiliar.Departamento.Nombre ?? "";
            }
            //CodigoArea = datosContactoSolicitante.CodigoArea;
            //Telefono = datosContactoSolicitante.Telefono;
            //CodigoAreaCelular = datosContactoSolicitante.CodigoAreaCelular;
            //Celular = datosContactoSolicitante.Celular;
            Email = usuarioCidi != null ? usuarioCidi.Email : "";
            Actividad = ObtenerActividadLaboral(persona);

            var condicionLaboral = ObtenerCondicionLaboral(persona);
            if (!string.IsNullOrEmpty(condicionLaboral))
                TrabajaActualmente = condicionLaboral.ToLower().Contains("no trabaja") ? "NO" : "SI";
            else
                TrabajaActualmente = "";

            NumeroVerificacionCIDI = "";
            PoseeCIDI = usuarioCidi != null ? "SI" : "NO";
        }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdNumero { get; set; }
        public string SexoId { get; set; }
        public string NombreSexo { get; set; }
        public string CodigoPais { get; set; }
        public string Nacionalidad { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Cuil { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaDefuncion { get; set; }
        public string Edad { get; set; }
        public string EstadoCivil { get; set; }

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

        public int IdVinDomicilio { get; set; }
        public string DomicilioGrupoFamiliar { get; set; }
        public string DomicilioGrupoFamiliarLocalidad { get; set; }
        public string DomicilioGrupoFamiliarDepartamento { get; set; }
        public string DomicilioDeuda { get; set; }

        //DATOS DE CONTACTO
        public string CodigoArea { get; set; }
        public string Telefono { get; set; }
        public string CodigoAreaCelular { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }


        public string Actividad { get; set; }
        public string TrabajaActualmente { get; set; }
        public string PoseeCIDI { get; set; }
        public string NumeroVerificacionCIDI { get; set; }

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

        public bool EsMismaPersona(DatosPersonalesResultado otraPersona)
        {
            return NroDocumento == otraPersona.NroDocumento &&
                   SexoId == otraPersona.SexoId &&
                   CodigoPais == otraPersona.CodigoPais;
        }

        public string ObtenerCondicionLaboral(PersonaUnica persona)
        {
            var res = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2003");
            if (res == null)
                return "";
            return res.Descripcion;
        }

        public string ObtenerActividadLaboral(PersonaUnica persona)
        {
            var res = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2009");
            if (res == null)
                return "";
            return res.Descripcion;
        }

        public string DomicilioCompleto
        {
            get
            {
                string res = "";
                bool iniciado = false;
                if (!string.IsNullOrEmpty(Calle))
                {
                    res += Calle;
                    iniciado = true;
                }
                if (!string.IsNullOrEmpty(Numero) && iniciado) res += " " + Numero;

                if (!string.IsNullOrEmpty(Torre))
                {
                    res += iniciado ? ", Torre: " + Torre : "Torre: " + Torre;
                }
                if (!string.IsNullOrEmpty(Piso))
                {
                    res += iniciado ? ", Piso: " + Piso : "Piso: " + Piso;
                }
                if (!string.IsNullOrEmpty(Dpto))
                {
                    res += iniciado ? ", Dpto: " + Dpto : "Dpto: " + Dpto;
                }
                if (!string.IsNullOrEmpty(Manzana))
                {
                    res += iniciado ? ", Manzana: " + Manzana : "Manzana: " + Manzana;
                }
                if (!string.IsNullOrEmpty(Casa))
                {
                    res += iniciado ? ", Casa: " + Casa : "Casa: " + Casa;
                }

                if (!string.IsNullOrEmpty(Barrio))
                {
                    res += iniciado ? " – Barrio " + Barrio : " Barrio " + Barrio;
                }
                if (!string.IsNullOrEmpty(DomicilioGrupoFamiliarLocalidad))
                {
                    res += iniciado ? " – Localidad " + DomicilioGrupoFamiliarLocalidad : " Localidad " + DomicilioGrupoFamiliarLocalidad;
                }

                if (!string.IsNullOrEmpty(DomicilioGrupoFamiliarDepartamento))
                {
                    res += iniciado ? " – Departamento " + DomicilioGrupoFamiliarDepartamento : " Departamento " + DomicilioGrupoFamiliarDepartamento;
                }
                return res;
            }
        }
    }
}
