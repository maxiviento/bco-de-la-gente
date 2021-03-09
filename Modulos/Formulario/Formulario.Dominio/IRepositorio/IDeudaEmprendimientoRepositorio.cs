using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.IRepositorio
{
    public interface IDeudaEmprendimientoRepositorio : IRepositorio<DeudaEmprendimiento>
    {
        IList<TipoDeudaEmprendimiento> ObtenerTiposDeudaEmprendimiento();
        IList<DeudaEmprendimientoResultado> ObtenerDeudasEmprendimientoPorIdEmprendimiento(Id idEmprendimiento);
        void RegistrarDeudaEmprendimiento(Id idEmprendimiento, Id idUsuario, string deudas);
    }
}