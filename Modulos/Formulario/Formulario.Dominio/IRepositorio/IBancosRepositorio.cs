using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface IBancosRepositorio
    {
        IList<Banco>  ObtenerComboBancos();
        IList<Sucursal> ObtenerComboSucursales(string idBanco);
        Sucursal ObtenerSucursalFormulario(int idFormulario);
    }
}
