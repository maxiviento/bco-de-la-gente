using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.IRepositorio;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Servicios
{
    public class DepartamentoServicio
    {
        private readonly IDepartamentoRepositorio _departamentoRepositorio;

        public DepartamentoServicio(IDepartamentoRepositorio departamentoRepositorio)
        {
            _departamentoRepositorio = departamentoRepositorio;
        }

        public IList<ClaveValorResultado<string>> Consultar()
        {
            return _departamentoRepositorio.Consultar().Select(departamento =>
                new ClaveValorResultado<string>(departamento.Id.ToString(), departamento.Descripcion)).ToList();
        }
    }
}