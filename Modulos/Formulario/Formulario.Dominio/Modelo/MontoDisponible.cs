using System;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class MontoDisponible : Entidad
    {
        public string Descripcion { get; protected set; }
        public decimal? Monto { get; protected set; }
        public decimal? NroMonto { get; protected set; }
        public DateTime FechaDepositoBancario { get; protected set; }
        public DateTime FechaInicioPago { get; protected set; }
        public DateTime FechaFinPago { get; protected set; }
        public string IdBanco { get; protected set; }
        public string IdSucursal { get; protected set; }
        public DateTime FechaAlta { get; protected set; }
        public virtual Usuario UsuarioAlta { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual DateTime FechaUltimaModificacion { get; protected set; }
        public virtual Usuario UsuarioUltimaModificacion { get; protected set; }
        public decimal? Saldo { get; protected set; }


        protected MontoDisponible()
        {

        }

        public MontoDisponible(
            string descripcion,
            decimal monto,
            DateTime fechaDepositoBancario,
            DateTime fechaInicioPago,
            DateTime fechaFinPago,
            string idBanco,
            string idSucursal,
            Usuario usuarioAlta
            )
        {
            Descripcion = descripcion;
            Monto = monto;
            FechaDepositoBancario = fechaDepositoBancario;
            FechaInicioPago = fechaInicioPago;
            FechaFinPago = fechaFinPago;
            IdBanco = idBanco;
            IdSucursal = idSucursal;
            FechaAlta = DateTime.Now;
            UsuarioAlta = usuarioAlta;
            
        }

        public MontoDisponible DarDeBaja(MotivoBaja motivo, Usuario usuario)
        {
            ValidarBaja();

            UsuarioUltimaModificacion = usuario;
            MotivoBaja = motivo;
            FechaUltimaModificacion = DateTime.Now;
            return this;
        }

        private void ValidarBaja()
        {
            if (MotivoBaja != null && !MotivoBaja.Id.IsDefault())
            {
                throw new ModeloNoValidoException("El monto disponible ya esta dado de baja.");
            }
        }

        public MontoDisponible Modificar(string descripcion,
            decimal monto,
            DateTime fechaDepositoBancario,
            DateTime fechaInicioPago,
            DateTime fechaFinPago,
            string idBanco,
            string idSucursal,
            Usuario usuario)
        {
            ValidarBaja();

            Descripcion = descripcion;
            Monto = monto;
            FechaDepositoBancario = fechaDepositoBancario;
            FechaInicioPago = fechaInicioPago;
            FechaFinPago = fechaFinPago;
            IdBanco = idBanco;
            IdSucursal = idSucursal;

            UsuarioUltimaModificacion = usuario;
            FechaUltimaModificacion = DateTime.Now;
            return this;
        }

        private static void ValidarIntegridadCampos(
            string descripcion,
            decimal monto,
            DateTime fechaDepositoBancario,
            DateTime fechaInicioPago,
            DateTime fechaFinPago,
            string idBanco,
            string idSucursal)
        {
            if (string.IsNullOrEmpty(descripcion))
            {
                throw new ModeloNoValidoException("La descripción del monto disponible es requerida.");
            }

            if (descripcion.Length > 200)
            {
                throw new ModeloNoValidoException("La descripción del monto disponible no debe superar los 200 caracteres.");
            }

            if (monto <= 0)
            {
                throw new ModeloNoValidoException("Debe ingresar un monto válido.");
            }

            if (string.IsNullOrEmpty(idBanco))
            {
                throw new ModeloNoValidoException("El idBanco del monto disponible es requerido.");
            }

            if (string.IsNullOrEmpty(idSucursal))
            {
                throw new ModeloNoValidoException("El idSucursal del monto disponible es requerido.");
            }
        }
    }
}
