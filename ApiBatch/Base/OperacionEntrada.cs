using ApiBatch.Base;
using ApiBatch.Base.QueueManager;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;


namespace ApiBatch.Operations.QueueManager
{
    public class OperacionEntrada
    {
        public IOperacionComando Comando { get; set; }
        public Usuario Usuario { get; set; }
        public TiposProcesosEnum TipoProceso { get; set; }
    }
}