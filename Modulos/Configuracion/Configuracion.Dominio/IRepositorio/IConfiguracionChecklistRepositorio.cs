using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IConfiguracionChecklistRepositorio : IRepositorio<VersionChecklist>
    {
        VersionChecklistResultado ObtenerVersionVigente(Id idLinea);
    }
}
