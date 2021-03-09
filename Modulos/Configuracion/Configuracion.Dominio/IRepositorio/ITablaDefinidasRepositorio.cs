using System.Collections.Generic;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.IRepositorio
{
    public interface ITablaDefinidasRepositorio
    {
        IList<TablaDefinida> ObtenerTablasDefinidas();
        Resultado<TablaDefinida> ObtenerTablasDefinidasPaginadas(ConsultaTablasDefinidas consulta);
        TablaDefinida ObtenerDatosTablaDefinida(int id);
        Resultado<ParametroTablaDefinidaResult> ObtenerDatosTablaDefinida(ConsultaParametrosTablasDefinidas consulta);
        decimal Rechazar(ParametroTablaDefinida parametro);
        decimal Registrar(ParametroTablaDefinida parametro,int idTabla);
        ParametroTablaDefinidaResult ObtenerParametroTablaDefinida(int id);
        IList<ParametroTablaDefinidaResult> ObtenerParametrosTablaDefinida(int idTabla);
        IList<ParametroTablaDefinidaResult> ObtenerParametrosComboTablaDefinida(ConsultaParametrosTablasDefinidas consulta);

    }
}
