using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo;

namespace Soporte.Dominio.IRepositorio
{
    public interface IParametrosRepositorio
    {

        Resultado<ConsultarParametrosResultado> ConsultarPorFiltros(ConsultaParametro consulta);

        VigenciaParametro RegistrarVigenciaParametro(VigenciaParametro parametro);
        VigenciaParametro ActualizarVigenciaExistente(VigenciaParametro parametro);

        VigenciaParametro ObtenerVigenciaParametroPorId(long idParametro);

        IEnumerable<Parametro> ObtenerParametros();

        VigenciaParametro ObtenerValorVigenciaParametroPorFecha(Id idParametro, DateTime? fecha);

        ConsultarParametrosResultado ExisteVigenciaEnFecha(Id idParametro, DateTime? fechaDesde);
    }
}
