using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Servicios
{
    public class BancosServicio
    {
        private readonly IBancosRepositorio _bancosRepositorio;

        public BancosServicio(IBancosRepositorio bancosRepositorio)
        {
            _bancosRepositorio = bancosRepositorio;
        }

        public IList<ClaveValorResultado<string>> ObtenerComboBancos()
        {
            var bancos = _bancosRepositorio.ObtenerComboBancos();
            return bancos.Select(banco => new ClaveValorResultado<string>
            (
                banco.IdBanco,
                banco.Descripcion
            )).ToList();
        }

        public IList<ClaveValorResultado<string>> ObtenerComboSucursales(string idBanco)
        {
            var sucursales = _bancosRepositorio.ObtenerComboSucursales(idBanco);
            return sucursales.Where(x => x.Descripcion != "")  //Le puse este where porque habia sucursales sin descripcion
                .Select(sucursal => new ClaveValorResultado<string>
            (
                sucursal.Id,
                sucursal.Descripcion
            )).ToList();
        }

        public Sucursal ObtenersucursalFormulario(int idFormulario)
        {
            return _bancosRepositorio.ObtenerSucursalFormulario(idFormulario);
        }
    }
}
