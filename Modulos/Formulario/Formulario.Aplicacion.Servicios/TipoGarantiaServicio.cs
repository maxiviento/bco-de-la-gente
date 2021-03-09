using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;

namespace Formulario.Aplicacion.Servicios
{
    public class TipoGarantiaServicio
    {
        private readonly ITipoGarantiaRepositorio _tipoGarantiaRepositorio;

        public TipoGarantiaServicio(ITipoGarantiaRepositorio tipoGarantiaRepositorio)
        {
            _tipoGarantiaRepositorio = tipoGarantiaRepositorio;
        }

        public IList<TipoGarantiaResultado> ConsultarTiposGarantias()
        {
            var tiposGarantia = _tipoGarantiaRepositorio.ConsultarTipoGarantias();
            var tiposGarantiaResultado = tiposGarantia.Select(gar => new TipoGarantiaResultado
            {
                Id = gar.Id,
                Descripcion = gar.Descripcion
            }).ToList();

            return tiposGarantiaResultado;
        }
    }
}