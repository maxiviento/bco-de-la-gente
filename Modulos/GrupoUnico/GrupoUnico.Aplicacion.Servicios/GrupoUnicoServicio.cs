using System;
using System.Globalization;
using System.Linq;
using AppComunicacion.ApiModels;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.Comun.Excepciones;

namespace GrupoUnico.Aplicacion.Servicios
{
    public class GrupoUnicoServicio
    {
        private readonly ISesionUsuario _sesionUsuario;

        public GrupoUnicoServicio(ISesionUsuario sesionUsuario)
        {
            _sesionUsuario = sesionUsuario;
        }

        public Persona GetApiConsultaDatosCompleto(string sexo, string dni, string pais, int? idNumero)
        {
            PersonaUnica personaUnica = (PersonaUnica)CallApiObject(sexo, dni, pais, idNumero, ApiPersonas.ApiConsultaDatosCompleto);

            var hash = _sesionUsuario.CiDiHash;

            if (string.IsNullOrEmpty(personaUnica.CUIL))
                personaUnica.CUIL = CalcularCuil(sexo, dni);

            UsuarioCidi cuentaCidi = ApiCuenta.ObtenerUsuarioPorCuil(hash, personaUnica.CUIL);

            var persona = new Persona
            {
                Nombre = personaUnica.Nombre,
                Apellido = personaUnica.Apellido,
                NombreSexo = personaUnica.Sexo.Nombre,
                SexoId = personaUnica.Sexo.IdSexo,
                IdNumero = personaUnica.Id_Numero,
                CodigoPais = personaUnica.PaisTD.IdPais,
                Nacionalidad = personaUnica.Nacionalidad?.Nacionalidad ?? "",
                TipoDocumento = personaUnica.TipoDocumento?.Nombre ?? "",
                NroDocumento = personaUnica.NroDocumento,
                FechaNacimiento = personaUnica.FechaNacimiento,
                DomicilioGrupoFamiliar = personaUnica.DomicilioGrupoFamiliar?.DireccionCompleta ?? "",
                DomicilioGrupoFamiliarLocalidad = personaUnica.DomicilioGrupoFamiliar?.Localidad?.Nombre ?? "",
                DomicilioGrupoFamiliarDepartamento = personaUnica.DomicilioGrupoFamiliar?.Departamento?.Nombre ?? "",
                Cuil = personaUnica.CUIL,
                CodigoArea = cuentaCidi.TelArea,
                Telefono = cuentaCidi.TelNro,
                CodigoAreaCelular = cuentaCidi.CelArea,
                Celular = cuentaCidi.CelNro,
                Email = cuentaCidi.Email
            };

            return persona;
        }

        private string CalcularCuil(string sexo, string dni)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || dni.Length > 8)
                return null;

            int verificacion;

            if (!int.TryParse(dni, out verificacion))
                return null;

            if (verificacion < 10000000)
            {
                int n = 8 - dni.Length;
                for (int i = 0; i < n; i++)
                {
                    dni = string.Concat("0", dni);
                }
            }
            string prefijo;

            switch (sexo)
            {
                case "01":
                    prefijo = "20";
                    break;
                case "02":
                    prefijo = "27";
                    break;
                default:
                    return null;
            }

            string cuil = string.Concat(prefijo, dni);

            int control = (
                              int.Parse(cuil[0].ToString()) * 5 +
                              int.Parse(cuil[1].ToString()) * 4 +
                              int.Parse(cuil[2].ToString()) * 3 +
                              int.Parse(cuil[3].ToString()) * 2 +
                              int.Parse(cuil[4].ToString()) * 7 +
                              int.Parse(cuil[5].ToString()) * 6 +
                              int.Parse(cuil[6].ToString()) * 5 +
                              int.Parse(cuil[7].ToString()) * 4 +
                              int.Parse(cuil[8].ToString()) * 3 +
                              int.Parse(cuil[9].ToString()) * 2
                          ) % 11;

            string posfijo;

            switch (control)
            {
                case 0:
                    posfijo = "0";
                    break;
                case 1:
                    prefijo = "23";
                    posfijo = sexo == "01" ? "9" : "4";
                    break;
                default:
                    posfijo = (11 - control).ToString();
                    break;
            }

            cuil = string.Concat(prefijo, dni, posfijo);

            return cuil;
        }

        protected delegate object CallApiWithObjectResultDelegate(string cookieHash, string sexo, string dni,
            string pais, int? idNumero);

        protected object CallApiObject(string sexo, string dni, string pais, int? idNumero, CallApiWithObjectResultDelegate handler)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais))
                throw new ModeloNoValidoException("el sexo id, el dni y el país son obligatorios.");

            var hash = _sesionUsuario.CiDiHash;

            object objeto = null;
            try
            {
                objeto = handler(hash, sexo, dni, pais, idNumero);
            }
            catch (ApplicationException e)
            {
                throw new ModeloNoValidoException("Error en aplicativo Grupo Único. Persona no encontrada.");
            }
            if (objeto == null)
            {
                throw new ModeloNoValidoException("No se encontró la persona.");
            }
            return objeto;
        }

        public object GetApiConsultaIngresosGrupoFamiliar(string sexo, string dni, string pais, int? idNumero)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar)CallApiObject(sexo, dni, pais, idNumero, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);

            if (grupoResultado.Grupo != null)
            {
                foreach (var integrante in grupoResultado.Grupo.Integrantes)
                {
                    var datosPersonalesCompletos = DatosPersonaUnica(integrante.Sexo.IdSexo, integrante.NroDocumento, integrante.PaisTD.IdPais, integrante.Id_Numero);

                    integrante.EstadoCivil = datosPersonalesCompletos.EstadoCivil;
                    integrante.FechaNacimiento = datosPersonalesCompletos.FechaNacimiento;
                    integrante.FechaDefuncion = datosPersonalesCompletos.FechaDefuncion;

                    var ingresos = integrante.Caracteristicas.Where(c => c.Valor != "").ToList();

                    var caracteristicaIngresos = new Caracteristica
                    {
                        Descripcion = "Ingresos",
                        IdCaracteristica = "INGRESO"
                    };

                    if (ingresos.Count == 0) continue;

                    var valorSumarizado = ingresos.Sum(ingreso => string.IsNullOrEmpty(ingreso.Valor) ? 0 : double.Parse(ingreso.Valor));

                    if (Math.Abs(valorSumarizado) < 0.001) continue;

                    caracteristicaIngresos.Valor = valorSumarizado.ToString(new CultureInfo("en-US"));
                    integrante.Caracteristicas.Add(caracteristicaIngresos);
                }
            }

            return grupoResultado;
        }

        public DatosPersonalesResultado GetApiConsultaDatosPersonales(string sexo, string dni, string pais, int? idNumero)
        {
            PersonaUnica personaUnica = (PersonaUnica)CallApiObject(sexo, dni, pais, idNumero, ApiPersonas.ApiConsultaDatosCompleto);

            var hash = _sesionUsuario.CiDiHash;

            if (string.IsNullOrEmpty(personaUnica.CUIL))
                personaUnica.CUIL = CalcularCuil(sexo, dni);

            UsuarioCidi cuentaCidi = ApiCuenta.ObtenerUsuarioPorCuil(hash, personaUnica.CUIL);

            if (cuentaCidi != null)
            {
                if (personaUnica.FechaNacimiento == null)
                {
                    DateTime dateValue;
                    if (DateTime.TryParse(cuentaCidi.FechaNacimiento, out dateValue))
                        personaUnica.FechaNacimiento = DateTime.ParseExact(cuentaCidi.FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }

            return new DatosPersonalesResultado(personaUnica, cuentaCidi);
        }

        private PersonaUnica DatosPersonaUnica(string sexo, string dni, string pais, int? idNumero)
        {
            PersonaUnica personaUnica = (PersonaUnica)CallApiObject(sexo, dni, pais, idNumero, ApiPersonas.ApiConsultaDatosCompleto);

            var hash = _sesionUsuario.CiDiHash;

            if (string.IsNullOrEmpty(personaUnica.CUIL))
                personaUnica.CUIL = CalcularCuil(sexo, dni);

            UsuarioCidi cuentaCidi = ApiCuenta.ObtenerUsuarioPorCuil(hash, personaUnica.CUIL);

            if (cuentaCidi != null)
            {
                if (personaUnica.FechaNacimiento == null)
                {
                    DateTime dateValue;
                    if (DateTime.TryParse(cuentaCidi.FechaNacimiento, out dateValue))
                        personaUnica.FechaNacimiento = DateTime.ParseExact(cuentaCidi.FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }

            return personaUnica;
        }

        public GrupoFamiliar ObtenerGrupoFamiliar(string sexo, string dni, string pais, int? idNumero)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar)CallApiObject(sexo, dni, pais, idNumero, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);
            return grupoResultado.Grupo;
        }

    }
}
