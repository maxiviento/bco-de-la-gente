using System;
using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class EstadoPrestamo : Entidad
    {
        public static EstadoPrestamo Creado => new EstadoPrestamo(1, "CREADO");
        public static EstadoPrestamo EvaluacionTecnica => new EstadoPrestamo(2, "EVALUACIÓN TÉCNICA");
        public static EstadoPrestamo Rechazado => new EstadoPrestamo(3, "RECHAZADO");
        public static EstadoPrestamo APagar => new EstadoPrestamo(4, "A PAGAR");
        public static EstadoPrestamo Comenzado => new EstadoPrestamo(5, "COMENZADO");
        public static EstadoPrestamo Anulado => new EstadoPrestamo(6, "ANULADO");
        public static EstadoPrestamo Finalizado => new EstadoPrestamo(7, "FINALIZADO");
        public static EstadoPrestamo APagarConLote => new EstadoPrestamo(9, "A PAGAR CON LOTE");
        public static EstadoPrestamo APagarConBanco => new EstadoPrestamo(10, "A PAGAR CON BANCO");
        public static EstadoPrestamo APagarEnviadoASuaf => new EstadoPrestamo(11, "A PAGAR ENVIADO A SUAF");
        public static EstadoPrestamo APagarConSuaf => new EstadoPrestamo(12, "A PAGAR CON SUAF");
        public static EstadoPrestamo ConPlanDeCuotas => new EstadoPrestamo(13, "CON PLAN DE CUOTAS");
        public static EstadoPrestamo Pagado => new EstadoPrestamo(14, "PAGADO");
        public static EstadoPrestamo Impago => new EstadoPrestamo(15, "IMPAGO");
        public static EstadoPrestamo ConPlanDeCuotasConImpagos => new EstadoPrestamo(16, "CON PLAN DE CUOTAS CON IMPAGOS");
        public static EstadoPrestamo Moroso3Y4 => new EstadoPrestamo(17, "MOROSO ENTRE 3 Y 4 MESES");
        public static EstadoPrestamo MorosoMas5 => new EstadoPrestamo(18, "MOROSO >= 5 MESES");
        public static EstadoPrestamo Reprogramado => new EstadoPrestamo(19, "REPROGRAMADO");

        public string Nombre { get; protected set; }
        public string Descripcion { get; protected set; }

        public EstadoPrestamo()
        {

        }

        public EstadoPrestamo(int id): this(id, null)
        {
        }

        protected EstadoPrestamo(int id, string descripcion)
        {
            Id = new Id(id);
            Descripcion = descripcion;
        }

        public static EstadoPrestamo ConId(int id)
        {
            switch (id)
            {
                case 1:
                    return Creado;
                case 2:
                    return EvaluacionTecnica;
                case 3:
                    return Rechazado;
                case 4:
                    return APagar;
                case 5:
                    return Comenzado;
                case 6:
                    return Anulado;
                case 7:
                    return Finalizado;
                case 9:
                    return APagarConLote;
                case 10:
                    return APagarConBanco;
                case 11:
                    return APagarEnviadoASuaf;
                case 12:
                    return APagarConSuaf;
                case 13:
                    return ConPlanDeCuotas;
                case 14:
                    return Pagado;
                case 15:
                    return Impago;
                case 16:
                    return ConPlanDeCuotasConImpagos;
                case 17:
                    return Moroso3Y4;
                case 18:
                    return MorosoMas5;
                case 19:
                    return Reprogramado;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id),
                        "No existe estado préstamo para el ID solicitado");
            }
        }

        public static EstadoPrestamo TransicionAceptarConId(int id)
            {
                switch (id)
                {
                    case 1:
                        return Comenzado;
                    case 4:
                        return Finalizado;
                    case 5:
                        return EvaluacionTecnica;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(id),
                            "No existe siguiente estado préstamo para el ID solicitado");
                }
        }

        public static EstadoPrestamo TransicionAceptarConEtapas(int idEstadoActual, IList<EtapaEstadoLineaResultado> etapasEstadosLinea)
        {
            var etapaEstado = etapasEstadosLinea.FirstOrDefault(x => x.IdEstadoActual == idEstadoActual);
            //if (etapaEstado == null) return TransicionAceptarConId(idEstadoActual);
            if (etapaEstado == null) return null; //El estado actual del préstamo no corresponde a un estado que transiciona de etapa

            return ConId((int) etapaEstado.IdEstadoSiguiente);
        }
    }
}
