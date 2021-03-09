using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class BancosRepositorio : NhRepositorio<Banco>, IBancosRepositorio
    {
        public BancosRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<Banco> ObtenerComboBancos()
        {
            var result = Execute("PR_OBTENER_BANCOS_COMBO")
                .ToListResult<Banco>();
            return result;
        }

        public IList<Sucursal> ObtenerComboSucursales(string idBanco)
        {
            return Execute("PR_OBTENER_SUC_BANCOS_COMBO")
                .AddParam(idBanco)
                .ToListResult<Sucursal>();
        }

        public Sucursal ObtenerSucursalFormulario(int idFormulario)
        {
            return Execute("PR_OBT_SUC_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<Sucursal>();
        }
    }
}