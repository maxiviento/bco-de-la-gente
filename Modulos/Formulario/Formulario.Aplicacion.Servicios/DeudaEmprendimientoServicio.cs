using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Servicios
{
    public class DeudaEmprendimientoServicio
    {
        private readonly IDeudaEmprendimientoRepositorio _deudaEmprendimientoRepositorio;

        public DeudaEmprendimientoServicio(IDeudaEmprendimientoRepositorio deudaEmprendimientoRepositorio)
        {
            _deudaEmprendimientoRepositorio = deudaEmprendimientoRepositorio;
        }

        public IList<DeudaEmprendimientoResultado> ObtenerDeudasEmprendimientoPorIdEmprendimiento(Id idEmprendimiento)
        {
            var resultados =
                _deudaEmprendimientoRepositorio.ObtenerDeudasEmprendimientoPorIdEmprendimiento(idEmprendimiento);

            return resultados;
        }

        public List<TipoDeudaResultado> ObtenerTiposDeuda()
        {
            var tiposDeuda = _deudaEmprendimientoRepositorio.ObtenerTiposDeudaEmprendimiento();
            var tiposDeudaResultado = tiposDeuda.Select(
                tipoDeuda => new TipoDeudaResultado()
                {
                    Id = tipoDeuda.Id,
                    Descripcion = tipoDeuda.Descripcion,
                    Nombre = tipoDeuda.Nombre
                }).ToList();

            return tiposDeudaResultado;
        }

        public void ActualizarDeudaEmprendimiento(
            int idEmprendimiento,
            List<RegistrarDeudaEmprendimientoComando> comandos,
            int idUsuario)
        {
            var str = "";

            foreach (var comando in comandos)
            {
                str += comando.IdTipoDeudaEmprendimiento + ";" + comando.Monto + ";";
            }

            _deudaEmprendimientoRepositorio.RegistrarDeudaEmprendimiento(new Id(idEmprendimiento), new Id(idUsuario),
                str);
        }

        public List<DeudaEmprendimientoResultado> ObtenerDeudasParaReporte(Id idEmprendimiento, int orden)
        {
            var resultados = new List<DeudaEmprendimientoResultado>();

            if (idEmprendimiento.IsDefault())
            {
                for (var i = 0; i < 6; i++)
                    resultados.Add(new DeudaEmprendimientoResultado {Orden = orden});
            }
            else
            {
                resultados =
                    (List<DeudaEmprendimientoResultado>) _deudaEmprendimientoRepositorio
                        .ObtenerDeudasEmprendimientoPorIdEmprendimiento(idEmprendimiento);
                foreach (var deuda in resultados) deuda.Orden = orden;
            }

            return resultados;
        }
    }
}