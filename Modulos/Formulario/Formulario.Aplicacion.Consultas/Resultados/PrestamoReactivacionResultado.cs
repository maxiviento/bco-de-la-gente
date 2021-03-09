using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class PrestamoReactivacionResultado
    {
        public string ApellidoSolicitante { get; set; }
        public string NombreSolicitante { get; set; }
        public decimal? IdFormulario { get; set; }
        public string NroFormulario { get; set; }
        public decimal? IdPrestamo { get; set; }
        public string NroPrestamo { get; set; }
        public string NroCaja { get; set; }
        public string IdSexo { get; set; }
        public string NumeroDocumento { get; set; }
        public string CodigoPais { get; set; }
        public string IdNumero { get; set; }
        public string IdSexoGarante { get; set; }
        public string NumeroDocumentoGarante { get; set; }
        public string CodigoPaisGarante { get; set; }
        public string IdNumeroGarante { get; set; }
    }
}
