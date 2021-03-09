using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;
using System.Collections.Generic;
using ApiBatch.Operations.QueueManager;
using ApiBatch.Models;

namespace ApiBatch.Base.QueueManager { 
    public interface IQueueManagerProcess
    {
        string GetStoreProcedureCall(OperacionEntrada cmd);        
        string GenerateFile(string nombreArchivo);
        string GuardarZip(IList<HttpFile> archivos, string nombre, string directorio);
        void EndProcess(string filePath);
        bool archivoUnico();
    }
}
