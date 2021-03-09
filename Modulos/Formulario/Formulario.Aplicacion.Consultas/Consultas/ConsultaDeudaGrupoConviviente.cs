using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class ConsultaDeudaGrupoConviviente
    {
        public ConsultaDeudaGrupoConviviente() { }

        public ConsultaDeudaGrupoConviviente(FiltrosFormularioConsulta consulta)
        {
            IdFormularioLinea = (int?)consulta.NroFormulario;
            NroDocumento = consulta.Dni;
        }

        public int? IdFormularioLinea { get; set; }
        public string NroDocumento { get; set; }

        public bool EsPorDocumento => IdFormularioLinea == null;

        public List<string> Validar()
        {
            List<string> errores = new List<string>();
            if (EsPorDocumento)
            {
                if(string.IsNullOrEmpty(NroDocumento))
                    errores.Add("El número de documento es requerido.");
                return errores;
            }

            if(IdFormularioLinea == null)
                errores.Add("El formulario es requerido.");

            return errores;
        }
    }
}
