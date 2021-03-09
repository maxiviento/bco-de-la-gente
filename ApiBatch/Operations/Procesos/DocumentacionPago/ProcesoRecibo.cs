using ApiBatch.Base;
using ApiBatch.Base.QueueManager;
using ApiBatch.GeneradoresArchivos;
using ApiBatch.Infraestructure;
using ApiBatch.Models;
using ApiBatch.Operations.QueueManager;
using Infraestructura.Core.Comun.Dato;
using NHibernate;

namespace ApiBatch.Operations.Procesos
{
    public class ProcesoRecibo: QueueManagerProcess<DocumentacionPago>
    {

        public ProcesoRecibo(IStatelessSession _statelessSession) : base(_statelessSession)
        {
        }
        public ProcesoRecibo(IStatelessSession _statelessSession, ProcesoResultado process,
            ArchivoDocumentacionPago generador, string dir, bool mismoArchivo) : base(_statelessSession, process, generador, dir, mismoArchivo)
        {            
        }        

        public override string GetStoreProcedureCall(OperacionEntrada cmd)
        {
            var spStringCall = "";
            return spStringCall;
        }
    }
}