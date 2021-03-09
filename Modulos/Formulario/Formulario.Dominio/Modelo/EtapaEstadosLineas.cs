using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class EtapaEstadosLineas : Entidad
    {
        public EtapaEstadosLineas() { }

        public EtapaEstadosLineas(EstadoPrestamo estadoAnterior, EstadoPrestamo estadoSiguiente, Etapa etapaAnterior, Etapa etapaSiguiente, LineaPrestamo linea, int orden)
        {
            EstadoAnterior = estadoAnterior;
            EstadoSiguiente = estadoSiguiente;
            EtapaAnterior = etapaAnterior;
            EtapaSiguiente = etapaSiguiente;
            Linea = linea;
            Orden = orden;
        }

        public EstadoPrestamo EstadoAnterior { get; set; }
        public EstadoPrestamo EstadoSiguiente { get; set; }
        public Etapa EtapaAnterior { get; set; }
        public Etapa EtapaSiguiente { get; set; }
        public LineaPrestamo Linea { get; set; }
        public int Orden { get; set; }
    }
}
