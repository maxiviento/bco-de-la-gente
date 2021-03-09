using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Dominio.Modelo
{
    public class CambiarGaranteComando
    {
        public string SexoId { get; set; }
        public string CodigoPais { get; set; }
        public string NroDocumento { get; set; }
        public int IdGaranteFormulario { get; set; }
        public int IdNumero { get; set; }
    }
}
