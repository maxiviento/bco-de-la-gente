using System;
using System.Linq;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Configuracion
{
    public class ConfiguracionChecklistRepositorio : NhRepositorio<VersionChecklist>, IConfiguracionChecklistRepositorio
    {
        public ConfiguracionChecklistRepositorio(ISession sesion) : base(sesion)
        {
        }

        public VersionChecklistResultado ObtenerVersionVigente(Id idLinea)
        {
            return Execute("PR_OBTENER_VERSION_LINEA")
                .AddParam(idLinea)
                .ToUniqueResult<VersionChecklistResultado>();
        }
    }
}
