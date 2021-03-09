using Infraestructura.Core.Comun.Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBatch.Base
{
    public interface IOperacionComando
    {
        IList<Id> Ids
        {
            get;
            set;
        }
    }
}
