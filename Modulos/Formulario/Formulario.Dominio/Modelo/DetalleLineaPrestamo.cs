using System;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class DetalleLineaPrestamo : Entidad
    {
        public virtual TipoIntegranteSocio TipoIntegranteSocio { get; protected set; }
        public virtual decimal? MontoTopeIntegrante { get; protected set; }
        public virtual decimal MontoPrestable { get; protected set; }
        public virtual int CantidadMaxIntegrante { get; protected set; }
        public virtual int CantidadMinIntegrante { get; protected set; }
        public virtual TipoFinanciamiento TipoFinanciamiento { get; protected set; }
        public virtual TipoInteres TipoInteres { get; protected set; }
        public virtual int PlazoDevolucion { get; protected set; }
        public virtual decimal ValorCuotaSolidaria { get; protected set; }
        public virtual TipoGarantia TipoGarantia { get; protected set; }
        public virtual string Visualizacion { get; protected set; }
        public virtual Usuario UsuarioModificacion { get; protected set; }
        public virtual MotivoBaja MotivoDeBaja { get; protected set; }
        public virtual bool Apoderado{ get; protected set; }
        public virtual Convenio ConvenioRecupero { get; protected set; }
        public virtual Convenio ConvenioPago { get; protected set; }



        public DetalleLineaPrestamo()
        {
        }

        public DetalleLineaPrestamo(
            TipoIntegranteSocio tipoIntegranteSocio,
            decimal? montoTopeIntegrante,
            decimal montoPrestable,
            int cantidadMaxIntegrante,
            int cantidadMinIntegrante,
            TipoFinanciamiento tipoFinanciamiento,
            TipoInteres tipoInteres,
            int plazoDevolucion,
            decimal valorCuotaSolidaria,
            TipoGarantia tipoGarantia,
            string visualizacion,
            bool apoderado,
            Convenio convenioRecupero,
            Convenio convenioPago,
            Usuario usuario)
        {
            ValidarDatos(tipoIntegranteSocio, montoTopeIntegrante, montoPrestable, cantidadMaxIntegrante,
                cantidadMinIntegrante, tipoFinanciamiento, tipoInteres, plazoDevolucion,
                valorCuotaSolidaria, tipoGarantia, visualizacion, convenioRecupero, convenioPago, usuario);
            TipoIntegranteSocio = tipoIntegranteSocio;
            MontoTopeIntegrante = montoTopeIntegrante;
            MontoPrestable = montoPrestable;
            CantidadMaxIntegrante = cantidadMaxIntegrante;
            CantidadMinIntegrante = cantidadMinIntegrante;
            TipoFinanciamiento = tipoFinanciamiento;
            TipoInteres = tipoInteres;
            PlazoDevolucion = plazoDevolucion;
            ValorCuotaSolidaria = valorCuotaSolidaria;
            TipoGarantia = tipoGarantia;
            Visualizacion = visualizacion;
            UsuarioModificacion = usuario;
            Apoderado = apoderado;
            ConvenioPago = convenioPago;
            ConvenioRecupero = convenioRecupero;
            ValidarDetalleLinea();
        }

        public void ValidarDetalleLinea()
        {
            switch ((int) TipoIntegranteSocio.Id.Valor)
            {
                case 1:
                {
                    if (CantidadMinIntegrante != 1 || CantidadMaxIntegrante != 1)
                        throw new ModeloNoValidoException("Sólo puede haber un integrante en las líneas individuales.");
                    break;
                }
                case 2:
                {
                    ValidarCantidadIntegrantes();
                    break;
                }
                case 3:
                {
                    ValidarCantidadIntegrantes();
                    ValidarMontos();
                    break;
                }
            }
        }

        public void ValidarMontos()
        {
            if (MontoTopeIntegrante != default(decimal))
            {
                if (MontoTopeIntegrante > MontoPrestable)
                {
                    throw new ModeloNoValidoException(
                        "El monto tope por socio integrante no puede ser mayor al monto prestable.");
                }
            }
        }

        public void ValidarCantidadIntegrantes()
        {
            var cantidadMinimaParametro = 2; //TODO Buscar parámetros en BD
            var cantidadMaximaParametro = 5;

            if (CantidadMinIntegrante < cantidadMinimaParametro)
            {
                throw new ModeloNoValidoException(
                    $"La cantidad mínima de integrantes no puede ser inferior a {cantidadMinimaParametro}");
            }

            if (CantidadMaxIntegrante > cantidadMaximaParametro)
            {
                throw new ModeloNoValidoException(
                    $"La cantidad máxima de integrantes no puede ser superior a {cantidadMaximaParametro}");
            }

            if (CantidadMinIntegrante > CantidadMaxIntegrante)
            {
                throw new ModeloNoValidoException(
                    "La cantidad mínima de integrantes no puede superar a la cantidad máxima de integrantes.");
            }
        }

        public void ValidarDatos(TipoIntegranteSocio tipoIntegranteSocio,
            decimal? montoTopeIntegrante,
            decimal montoPrestable,
            int cantidadMaxIntegrante,
            int cantidadMinIntegrante,
            TipoFinanciamiento tipoFinanciamiento,
            TipoInteres tipoInteres,
            int plazoDevolucion,
            decimal valorCuotaSolidaria,
            TipoGarantia tipoGarantia,
            string visualizacion,
            Convenio convenioRecupero,
            Convenio convenioPago,
            Usuario usuario)
        {
            if (tipoIntegranteSocio == null)
                throw new ModeloNoValidoException("Debe seleccionar un Integrante/Socio.");

            if (tipoFinanciamiento == null)
                throw new ModeloNoValidoException("Debe seleccionar un Tipo de Financiamiento.");

            if (tipoInteres == null)
                throw new ModeloNoValidoException("Debe seleccionar un Tipo de Interés.");

            if (tipoGarantia == null)
                throw new ModeloNoValidoException("Debe seleccionar un Tipo de Garantía.");

            if (montoPrestable == default(decimal))
                throw new ModeloNoValidoException("El monto prestable es requerido.");

            if (montoPrestable <= 0)
                throw new ModeloNoValidoException("El monto prestable debe ser mayor a 0 (cero).");

            if (valorCuotaSolidaria == default(decimal))
                throw new ModeloNoValidoException("El valor estimado de cuota solidaria es requerido.");

            if (cantidadMinIntegrante == default(int))
                throw new ModeloNoValidoException("La cantidad mínima de integrantes es requerida.");

            if (cantidadMaxIntegrante == default(int))
                throw new ModeloNoValidoException("La cantidad máxima de integrantes es requerida.");

            if (plazoDevolucion == default(int))
                throw new ModeloNoValidoException("El plazo de devolución máximo es requerido.");

            if (plazoDevolucion <= 0)
                throw new ModeloNoValidoException("El plazo de devolución debe ser mayor a 0 (cero).");

            if (usuario == null)
                throw new ModeloNoValidoException("Debe haber un usuario de modificación");

            if (visualizacion == null)
                throw new ModeloNoValidoException("La visualización del detalle es requerido.");
            if (convenioRecupero == null)
            {
                throw new ModeloNoValidoException("El convenio recupero es requerido.");
            }
            if (convenioPago == null)
            {
                throw new ModeloNoValidoException("El convenio pago es requerido.");
            }
        }

        public DetalleLineaPrestamo ModificarDetalleLinea(Id id,
            TipoIntegranteSocio tipoIntegranteSocio,
            decimal? montoTopeIntegrante,
            decimal montoPrestable,
            int cantidadMaxIntegrante,
            int cantidadMinIntegrante,
            TipoFinanciamiento tipoFinanciamiento,
            TipoInteres tipoInteres,
            int plazoDevolucion,
            decimal valorCuotaSolidaria,
            TipoGarantia tipoGarantia,
            string visualizacion,
            bool apoderado,
            Convenio convenioRecupero,
            Convenio convenioPago,
            Usuario usuario)
        {
            ValidarDatos(tipoIntegranteSocio, montoTopeIntegrante, montoPrestable, cantidadMaxIntegrante,
                cantidadMinIntegrante, tipoFinanciamiento, tipoInteres, plazoDevolucion,
                valorCuotaSolidaria, tipoGarantia, visualizacion, convenioRecupero, convenioPago, usuario);
            Id = id;
            TipoIntegranteSocio = tipoIntegranteSocio;
            MontoTopeIntegrante = montoTopeIntegrante;
            MontoPrestable = montoPrestable;
            CantidadMaxIntegrante = cantidadMaxIntegrante;
            CantidadMinIntegrante = cantidadMinIntegrante;
            TipoFinanciamiento = tipoFinanciamiento;
            TipoInteres = tipoInteres;
            PlazoDevolucion = plazoDevolucion;
            ValorCuotaSolidaria = valorCuotaSolidaria;
            Visualizacion = visualizacion;
            TipoGarantia = tipoGarantia;
            ConvenioPago = convenioPago;
            ConvenioRecupero = convenioRecupero;
            UsuarioModificacion = usuario;
            Apoderado = apoderado;
            ValidarDetalleLinea();
            return this;
        }

        public DetalleLineaPrestamo(Id id, MotivoBaja motivoBaja, Usuario usuario)
        {
            Id = id;
            MotivoDeBaja = motivoBaja;
            UsuarioModificacion = usuario;
        }
    }
}