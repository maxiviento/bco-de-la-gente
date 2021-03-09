using System;
using AppComunicacion.ApiModels;

namespace Infraestructura.Core.Comun
{
    public static class PersonaUnicaExtention
    {
        public static string Edad(this PersonaUnica persona)
        {
            var edadString = "";
            if (persona.FechaNacimiento.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - persona.FechaNacimiento.Value.Year;
                if (persona.FechaNacimiento > today.AddYears(-age)) age--;
                edadString = age.ToString();
            }
            return edadString;
        }

        public static string CalcularCuil(this PersonaUnica persona)
        {
            var sexo = persona.Sexo.IdSexo;
            var dni = persona.NroDocumento;
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
    }
}