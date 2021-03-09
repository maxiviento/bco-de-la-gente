using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class OrdenCuadrante : Entidad
    {
        public virtual int Orden { get; protected set; }
        public virtual Cuadrante Cuadrante { get; protected set; }
        public virtual TipoSalida TipoSalida { get; protected set; }

        public OrdenCuadrante()
        {
        }

        public OrdenCuadrante(int orden, Cuadrante cuadrante, TipoSalida tipoSalida)
        {
            if (orden == null)
                throw new ModeloNoValidoException("El cuadrante debe tener un orden");
            Orden = orden;
            if (cuadrante != null)
                Cuadrante = cuadrante;
            if (tipoSalida != null)
                TipoSalida = tipoSalida;
        }
    }
}