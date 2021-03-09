using System;
using Configuracion.Dominio.Modelo;

namespace Formulario.Dominio.Modelo
{
   public class DetallePrestamo
    {
        public DetallePrestamo() { }

        public virtual TipoGarantia TipoGarantia { get; protected set; }
        public virtual TipoItem TipoItem { get; protected set; }
        public virtual RequisitoPrestamo Requisito { get; protected set; }
        public virtual DateTime FechaRecibido { get; protected set; }
        public virtual bool Solicitante { get; protected set; }
        public virtual bool Garante { get; protected set; }
        public virtual bool SolicitGarante { get; protected set; }


        public DetallePrestamo(
            TipoGarantia tipoGarantia,
            TipoItem tipoItem,
            RequisitoPrestamo requisito,
            bool solicitante,
            bool garante,
            bool solicitGarante)
        {
            TipoGarantia = tipoGarantia;
            TipoItem = tipoItem;
            Requisito = requisito;
            Solicitante = solicitante;
            Garante = garante;
            SolicitGarante = solicitGarante;

        }
      
    }
}
