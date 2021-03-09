using System;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class VigenciaParametroIdResultado
    {
        public VigenciaParametroIdResultado() { }

        public VigenciaParametroIdResultado(decimal id)
        {
            Id = id;
        }

        public virtual Decimal Id { get; set; }
    }
}
