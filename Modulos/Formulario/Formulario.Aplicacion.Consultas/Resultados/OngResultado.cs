using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppComunicacion.ApiModels;
using Infraestructura.Core.Comun;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class OngResultado : ReporteFormularioResultado
    {
        public OngResultado(int orden)
        {
            this.Orden = orden;
        }

        public OngResultado(int orden, decimal? idOng, decimal? idFormulario, string nombreGrupo, decimal? numeroGrupo)
        {
            this.Orden = orden;
            this.IdOng = idOng;
            this.IdFormulario = idFormulario;
            this.NombreGrupo = nombreGrupo;
            this.NumeroGrupo = numeroGrupo;
        }

        public decimal? IdOng { get; set; }
        public decimal? IdFormulario { get; set; }
        public string NombreGrupo { get; set; }
        public decimal? NumeroGrupo { get; set; }
    }
}
