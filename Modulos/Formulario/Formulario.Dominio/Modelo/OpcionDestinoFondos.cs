using Infraestructura.Core.Comun.Excepciones;
using System.Collections.Generic;
using System.Linq;

namespace Formulario.Dominio.Modelo
{
    public sealed class OpcionDestinoFondos
    {
        public IList<DestinoFondos> DestinoFondos { get; private set; }
        public string Observaciones { get; private set; }

        private OpcionDestinoFondos()
        {
        }

        public OpcionDestinoFondos(IList<DestinoFondos> destinoFondos, string observaciones) : this()
        {
            if (destinoFondos == null || destinoFondos.Count == 0)
                throw new ModeloNoValidoException("Una solicitud debe tener al menos un destino de fondos");
            if (destinoFondos.Any(c => c.Descripcion.Equals("OTROS")) ^ !string.IsNullOrEmpty(observaciones))
                throw new ModeloNoValidoException(
                    "Una solicitud con destino de fondos \"OTROS\" debe venir acompañada de una observación");

            DestinoFondos = destinoFondos;
            Observaciones = observaciones;
        }
    }
}