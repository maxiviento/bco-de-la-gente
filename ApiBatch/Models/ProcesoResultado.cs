using ApiBatch.Base.QueueManager;
using ApiBatch.Operations.QueueManager;
using Infraestructura.Core.Comun.Dato;

namespace ApiBatch.Models
{
    public class ProcesoResultado
    {
        public Id IdProceso { get; set; }
        public Id IdEstado { get; set; }
        public string TxSp { get; set; }
        public string TxPath { get; set; }
        public Id IdUsuario { get; set; }
        public int IdTipoProceso { get; set; }
        public string Abreviatura { get; set; }
        public Id NroGrupoProceso { get; set; }
        public bool Generado { get; set; }
        public bool AcumuloResultados { get; set; }
        public bool EsUltimo { get; set; }
        public bool EsPrimero { get; set; }
    }
}