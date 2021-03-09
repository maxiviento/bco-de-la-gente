using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Servicios
{
    /*
     * FROM T_TIPOS_INVERSION_EMP
     * 1    INVERSION REALIZADA
     * 2    NECESIDAD INVERSION
     */

    public class DetalleInversionEmprendimientoServicio
    {
        private readonly IDetalleInversionEmprendimientoRepositorio _detalleInversionEmprendimientoRepositorio;

        public DetalleInversionEmprendimientoServicio(
            IDetalleInversionEmprendimientoRepositorio detalleInversionEmprendimientoRepositorio)
        {
            _detalleInversionEmprendimientoRepositorio = detalleInversionEmprendimientoRepositorio;
        }

        public List<InversionRealizadaResultado> ObtenerInversionesRealizadasPorIdEmprendimiento(Id idEmprendimiento)
        {
            var detallesInversion =
                _detalleInversionEmprendimientoRepositorio.ObtenerDetallesInversionPorIdTipoInversion(idEmprendimiento,
                    TipoInversion.InversionRealizada.Id);
            return detallesInversion;
            // TODO: código agregado para unir por itemInversión (para que funcione hay que cambiar toda la estructura)
            //List<InversionRealizadaResultado> resultado = new List<InversionRealizadaResultado>();
            //foreach (var detalle in detallesInversion)
            //{
            //    bool esNuevo = detalle.CantidadNuevos > 0;
            //    bool esUsado = detalle.CantidadUsados > 0;
            //    var itemExistente = resultado.FirstOrDefault(x => x.IdItemInversion == detalle.IdItemInversion);
            //    if (itemExistente != null)
            //    {
            //        if (esNuevo)
            //        {
            //            itemExistente.CantidadNuevos = detalle.CantidadNuevos;
            //            itemExistente.PrecioNuevos = detalle.PrecioNuevos;
            //        }
            //        else if (esUsado)
            //        {
            //            itemExistente.CantidadUsados = detalle.CantidadUsados;
            //            itemExistente.PrecioUsados = detalle.PrecioUsados;
            //        }
            //        continue;
            //    }
            //    resultado.Add(detalle);
            //}

            //return resultado;
        }

        public List<InversionRealizadaResultado> ObtenerDetallesNecesidadInversionPorIdEmprendimiento(
            Id idEmprendimiento)
        {
            var detallesInversion =
                _detalleInversionEmprendimientoRepositorio.ObtenerDetallesInversionPorIdTipoInversion(idEmprendimiento,
                    TipoInversion.NecesidadInversion.Id);

            return detallesInversion;
        }

        public IList<FormulariosInversionRealizadaResultado> RegistrarInversionesRealizadas(Id idEmprendimiento,
            List<RegistrarInversionRealizadaComando> comandos)
        {
            return RegistrarDetalleInversionEmprendimiento(idEmprendimiento, comandos, TipoInversion.InversionRealizada.Id);
        }

        public IList<FormulariosInversionRealizadaResultado> RegistrarDetalleInversionEmprendimiento(Id idEmprendimiento,
            List<RegistrarInversionRealizadaComando> comandos, Id idTipoInversion)
        {
            var listaInversiones = new List<FormulariosInversionRealizadaResultado>();
            foreach (var comando in comandos)
            {
                if (comando.CantidadNuevos.HasValue && comando.CantidadNuevos > 0)
                {
                    var id = _detalleInversionEmprendimientoRepositorio.RegistrarDetalleInversion(comando.Id, idEmprendimiento,
                        idTipoInversion,
                        comando.IdItemInversion, comando.Observaciones, true, comando.PrecioNuevos ?? 0, comando.CantidadNuevos ?? 0);
                    comando.Id = new Id(id);
                }

                if (comando.CantidadUsados.HasValue && comando.CantidadUsados > 0)
                {
                    var id = _detalleInversionEmprendimientoRepositorio.RegistrarDetalleInversion(comando.Id, idEmprendimiento,
                        idTipoInversion,
                        comando.IdItemInversion, comando.Observaciones, false, comando.PrecioUsados ?? 0, comando.CantidadUsados ?? 0);
                    comando.Id = new Id(id);
                }

                listaInversiones.Add(new FormulariosInversionRealizadaResultado()
                {
                    Id = comando.Id,
                    IdItemInversion = comando.IdItemInversion,
                    Observaciones = comando.Observaciones,
                    CantidadNuevos = comando.CantidadNuevos,
                    CantidadUsados = comando.CantidadUsados,
                    PrecioNuevos = comando.PrecioNuevos,
                    PrecioUsados = comando.PrecioUsados
                });
            }
            return listaInversiones;
        }

        public void EliminarDetalleInversion(List<Id> idsParaEliminar)
        {
            foreach (var idDetalleInversion in idsParaEliminar)
            {
                _detalleInversionEmprendimientoRepositorio.EliminarDetallesInversion(idDetalleInversion);
            }
        }

        public List<ItemInversionResultado> ObtenerItemsInversion()
        {
            var resultados = _detalleInversionEmprendimientoRepositorio.ObtenerItemsInversion();

            return (List<ItemInversionResultado>)resultados;
        }

        public List<InversionRealizadaResultado> ObtenerDetallesInversionRealizadaParaReportes(Id idEmprendimiento,
            int orden)
        {
            var itemsInversion = ObtenerItemsInversion();
            if (idEmprendimiento.IsDefault())
            {
                var listaVacia = new List<InversionRealizadaResultado>();
                foreach (var itemInversion in itemsInversion)
                {
                    if (itemInversion.Id.Valor != 99)
                    {
                        listaVacia.Add(new InversionRealizadaResultado
                        {
                            Orden = orden,
                            Observaciones = itemInversion.Descripcion
                        });
                    }
                }

                for (int i = 0; i < 6; i++)
                {
                    listaVacia.Add(new InversionRealizadaResultado { Orden = orden });
                }

                return listaVacia;
            }

            var detallesInversion =
                _detalleInversionEmprendimientoRepositorio.ObtenerDetallesInversionPorIdTipoInversion(idEmprendimiento,
                    new Id(1));

            foreach (var inversionRealizada in detallesInversion)
            {
                inversionRealizada.Orden = orden;
                itemsInversion.Remove(new ItemInversionResultado { Id = inversionRealizada.IdItemInversion });
            }
            foreach (var itemInversion in itemsInversion)
            {
                if (itemInversion.Id.Valor != 99)
                {
                    detallesInversion.Add(new InversionRealizadaResultado
                    {
                        Orden = orden,
                        Observaciones = itemInversion.Descripcion
                    });
                }
            }

            return detallesInversion;
        }
    }
}