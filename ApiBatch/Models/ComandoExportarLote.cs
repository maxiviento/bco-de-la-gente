using ApiBatch.Base;
using Infraestructura.Core.Comun.Dato;
using System.Collections.Generic;

namespace ApiBatch.Models
{
    public class ComandoExportarLote: IOperacionComando
    {
        public string Nombre { get; set; }

        public string EntidadId { get; set; }

        public IList<Id> Ids { set; get; }

        public bool UsarGrupoFamiliar { get; set; } = false;
        public string NumeroExpediente { get; set; }       
        public Id OrganismoId { set; get; }
        public Id AccionId { set; get; }
    }
}