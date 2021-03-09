using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DetalleMotivosRechazoResultado
    {
        public DetalleMotivosRechazoResultado()
        {
            IdsMotivosRechazo = new List<int>();
        }

        public DetalleMotivosRechazoResultado(IList<int> idsMotivosRechazo)
        {
            IdsMotivosRechazo = idsMotivosRechazo;
        }

        public IList<int> IdsMotivosRechazo { get; }

        public void NuevoMotivoRechazo(int id)
        {
            if (id == 0) return;
            if (IdsMotivosRechazo.Any(x => x == id)) return;
            IdsMotivosRechazo.Add(id);
        }

        public void NuevoMotivoRechazo(IList<int> ids)
        {
            if (ids == null) return;
            foreach (var id in ids)
            {
                if (id == 0) continue;
                if (IdsMotivosRechazo.Any(x => x == id)) continue;
                IdsMotivosRechazo.Add(id);
            }
        }

        public string IdsMotivoRechazoString => IdsMotivosRechazo.Count > 0 ? string.Join(";", IdsMotivosRechazo) : string.Empty;

    }
}
