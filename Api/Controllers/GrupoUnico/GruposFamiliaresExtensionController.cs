using System;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using AppComunicacion.ApiModels;
using Formulario.Aplicacion.Comandos;
using Infraestructura.Core.CiDi.Api;

namespace Api.Controllers.GrupoUnico
{
    public class GruposFamiliaresExtensionController : GruposFamiliaresController
    {
        [Route("consulta-ingresos-grupo")]
        public object GetApiConsultaIngresosGrupoFamiliar(string sexo, string dni, string pais)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar)CallApiObject(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);

            if (grupoResultado.Grupo != null)
            {
                foreach (var integrante in grupoResultado.Grupo.Integrantes)
                {
                    var ingresos = integrante.Caracteristicas.Where(c => c.Valor != "").ToList();

                    var caracteristicaIngresos = new Caracteristica
                    {
                        Descripcion = "Ingresos",
                        IdCaracteristica = "INGRESO"
                    };
                    var valorSumarizado = ingresos.Sum(ingreso => double.Parse(ingreso.Valor));

                    caracteristicaIngresos.Valor = valorSumarizado.ToString(new CultureInfo("en-US"));
                    integrante.Caracteristicas.Add(caracteristicaIngresos);
                }
            }

            return grupoResultado;
        }

        [Route("consulta-existencia-grupo")]
        public bool GetApiConsultaExistenciaGrupoFamiliar(string sexo, string dni, string pais)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar)CallApiObject(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);
            return grupoResultado.Grupo != null;
        }

        [Route("consulta-existencia-grupo-integrantes")]
        [HttpPost]
        public bool GetApiConsultaExistenciaGrupoFamiliarIntegrantes([FromBody] ConsultarGrupoFamiliarComando comando)
        {
            foreach (var integrante in comando.Integrantes)
            {
                var grupo = (RespuestaAPIGrupoFamiliar)CallApiObject(integrante.Sexo, integrante.NroDocumento, integrante.Pais, integrante.IdNumero, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);
                if (grupo.Grupo == null)
                {
                    return false; //Si algun integrante no tiene grupo = false
                }
            }
            return true; //Si todos los integrantes tienen grupo = true
        }

        [Route("obtener-id-grupo")]
        public int GetApiObtenerIdGrupoFamiliar(string sexo, string dni, string pais)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar)CallApiObject(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);
            return (int)grupoResultado.Grupo.IdGrupo;
        }
    }
}