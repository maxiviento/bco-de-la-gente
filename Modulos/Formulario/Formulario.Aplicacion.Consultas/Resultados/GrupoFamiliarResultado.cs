using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppComunicacion.ApiModels;
using Infraestructura.Core.Comun;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class GrupoFamiliarResultado: ReporteFormularioResultado
    {
        public GrupoFamiliarResultado() { }

        public GrupoFamiliarResultado(PersonaUnica persona)
        {
            var trabaja = "";
            if (!String.IsNullOrEmpty(CondicionLaboral))
            {
                trabaja = CondicionLaboral.ToLower() == "no trabaja" ? "NO" : "SI";
            }
            if (persona != null)
            {
                IdNumero = persona.Id_Numero;
                IdSexo = persona.Sexo.IdSexo;
                CodPais = persona.PaisTD.IdPais;
                NombreCompleto = persona.NombreCompleto;
                var tipo = persona.TipoDocumento?.Id ?? "ARG";
                TipoDocumento = tipo.Equals("ARG")? "DNI": "PASAPORTE";
                EstadoCivil = persona.EstadoCivil != null ? IdEstadoCivil(persona.EstadoCivil.Nombre) : "";
                Vinculo = IdVinculo(ObtenerVinculo(persona));
                Sexo = persona.Sexo.NombreCorto;
                Discapacidad = IdDiscapacidad(ObtenerDiscapacidad(persona));
                PoseeCertificadoDiscapacidad = "";
                NroDocumento = persona.NroDocumento;
                FechaNacimiento = persona.FechaNacimiento?.ToString("dd/MM/yyyy") ?? "";
                FechaDefuncion = persona.FechaDefuncion?.ToString("dd/MM/yyyy") ?? "";
                AsisteEstablecimientoEducativo = ObtenerAsistenciaEstablecimientoEducativo(persona);
                NivelAlcanzado = IdNivelAcademico(ObtenerNivelAcademico(persona));
                CondicionLaboral = IdCondicionLaboral(ObtenerCondicionLaboral(persona));
                Trabaja = trabaja;
                Edad = persona.Edad();
                Acargo = "";
                IngresosMensuales = ObtenerIngresoMensual(persona);
                ActividadLaboral = ObtenerActividadLaboral(persona);
            }
        }

        public int IdNumero { get; set; }
        public string IdSexo { get; set; }
        public string CodPais { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoDocumento { get; set; }
        public string EstadoCivil { get; set; }
        public string Vinculo { get; set; }
        public string Sexo { get; set; }
        public string Discapacidad { get; set; }
        public string PoseeCertificadoDiscapacidad { get; set; }
        public string NroDocumento { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaDefuncion { get; set; }
        public string AsisteEstablecimientoEducativo { get; set; }
        public string NivelAlcanzado { get; set; }
        public string Trabaja { get; set; }
        public string CondicionLaboral { get; set; }
        public string Edad { get; set; }
        public string Acargo { get; set; }
        public double? IngresosMensuales { get; set; }
        public string ActividadLaboral { get; set; }

        private string ObtenerVinculo(PersonaUnica persona)
        {
            var vinc = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2019");
            if (vinc == null)
                return "";
            return vinc.Descripcion;
        }

        private string ObtenerDiscapacidad(PersonaUnica persona)
        {
            var disc = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2014");
            return disc == null ? "NO" : disc.Descripcion;
        }

        private string ObtenerNivelAcademico(PersonaUnica persona)
        {
            var res = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2004");
            if (res == null)
                return "";
            return res.Descripcion;
        }

        private string ObtenerAsistenciaEstablecimientoEducativo(PersonaUnica persona)
        {
            var res = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2001");
            if (res == null)
                return "";
            return res.Descripcion.ToUpper() == "ASISTE" ? "SI" : "NO";
        }

        private string ObtenerCondicionLaboral(PersonaUnica persona)
        {
            var res = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2003");
            if (res == null)
                return "";
            return res.Descripcion;
        }

        private string ObtenerActividadLaboral(PersonaUnica persona)
        {
            var res = persona.Caracteristicas.FirstOrDefault(x => x.TipoCaracteristica.IdTipoCaracteristica == "2009");
            if (res == null)
                return "";
            return res.Descripcion;
        }

        private double ObtenerIngresoMensual(PersonaUnica persona)
        {
            //var ingreso = persona.Caracteristicas[0].Valor;
            var ingreso = persona.Caracteristicas.FirstOrDefault(x => x.IdCaracteristica == "INGRESO");
            if (string.IsNullOrEmpty(ingreso?.Valor))
                return 0;

            if (ingreso.Valor.Contains('.'))
                ingreso.Valor = ingreso.Valor.Replace('.', ',');

            return double.Parse(ingreso.Valor);
        }

        private string IdEstadoCivil(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion))
                return "";
            switch (descripcion)
            {
                case "SOLTERO":
                    return "1";
                case "CASADO":
                    return "2";
                case "DIVORCIADO":
                    return "3";
                case "SEPARADO":
                    return "4";
                case "VIUDO":
                    return "5";
                case "CONCUBINO":
                    return "6";
                default:
                    return "";
            }
        }

        private string IdVinculo(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion))
                return "";
            switch (descripcion)
            {
                case "CONYUGE/CONCUBINO":
                    return "1";
                case "HIJO/A":
                    return "2";
                case "PADRE/MADRE":
                    return "3";
                case "NIETO/A":
                    return "4";
                case "HERMANO/A":
                    return "5";
                case "OTRAS":
                    return "6";
                default:
                    return "";
            }
        }

        private string IdDiscapacidad(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion))
                return "";
            switch (descripcion)
            {
                case "NO":
                    return "1";
                case "FISICA":
                    return "2";
                case "MOTORA":
                    return "2";
                case "SENSORIAL":
                    return "3";
                case "MENTAL":
                    return "4";
                case "OTRAS":
                    return "5";
                default:
                    return "";
            }
        }

        private string IdNivelAcademico(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion))
                return "";
            switch (descripcion)
            {
                case "NINGUNO":
                    return "1";
                case "JARDIN":
                    return "2";
                case "PREESCOLAR":
                    return "3";
                case "PRIMARIO INC.":
                    return "4";
                case "PRIMARIO - SEXTO GRADO":
                    return "4";
                case "PRIMARIO COMPLETO":
                    return "5";
                case "SECUNDARIO INC.":
                    return "6";
                case "SECUNDARIO COMPLETO":
                    return "7";
                case "TERCIARIO INC.":
                    return "8";
                case "TERCIARIO COMPLETO":
                    return "9";
                case "UNIVERSITARIO INC.":
                    return "10";
                case "UNIVERSITARIO - SEXTO AÑO":
                    return "10";
                case "UNIVERSITARIO COMPLETO":
                    return "11";
                default:
                    return "";
            }
        }

        private string IdCondicionLaboral(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion))
                return "";
            switch (descripcion)
            {
                case "PATRON O EMPLEADOR":
                    return "1";
                case "POR CUENTA PROPIA":
                    return "2";
                case "OBRERO O EMPLEADO":
                    return "3";
                case "SERVICIO DOMESTICO":
                    return "4";
                case "JUBILADO O PENSIONADO":
                    return "5";
                case "BENEF. DE PROG. DE EMPLEO":
                    return "6";
                case "ASIGNACION UNIVERSAL POR HIJO":
                    return "7";
                case "BENEF. DE OTROS PROG. SOCIALES":
                    return "8";
                case "PERMANENTE":
                    return "9";
                default:
                    return "";
            }
        }
    }
}
