using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DatosEmprendimientoResultado: ReporteFormularioResultado
    {
        public DatosEmprendimientoResultado()
        {
            CodigoArea = "            ";
        }

        public DatosEmprendimientoResultado(EmprendimientoResultado emp, string actividad, int orden = 0)
        {
            Calle = emp.Calle;
            Numero = emp.NroCalle?.ToString();
            Torre = emp.Torre?.ToString();
            Piso = emp.NroPiso?.ToString();
            Dpto = emp.NroDpto;
            Manzana = emp.Manzana?.ToString();
            CodigoPostal = emp.CodPostal?.ToString();
            Barrio = emp.Barrio;
            Email = emp.Email;
            CodigoArea = emp.NroCodArea?.ToString();
            Telefono = emp.NroTelefono?.ToString();
            Actividad = actividad;
            Casa = emp.Casa;
            TipoProyecto = emp.IdTipoProyecto;
            FechaActivo = emp.FechaActivo?.ToString("dd/MM/yyyy");
            SectorDesarrollo = emp.IdSectorDesarrollo;
            PoseeExperiencia = emp.TieneExperiencia ? "S" : "N";
            TiempoExperiencia = emp.TiempoExperiencia;
            RealizoCursos = emp.HizoCursos ? "S" : "N";
            PidioCredito = emp.PidioCredito ? "S" : "N";
            OtorgaronCredito = emp.CreditoFueOtorgado ? "S" : "N";
            InstitucionSolicitud = emp.InstitucionSolicitante;
            Localidad = emp.Localidad;
            Departamento = emp.Departamento;
            Orden = orden;
        }

        //DOMICILIO
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Torre { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string Manzana { get; set; }
        public string Casa { get; set; }
        public string Barrio { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }

        //DATOS DE CONTACTO
        public string CodigoArea { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }


        public string Actividad { get; set; }
        public int TipoProyecto { get; set; }
        public string FechaActivo { get; set; }
        public int SectorDesarrollo { get; set; }
        public string PoseeExperiencia { get; set; }
        public string TiempoExperiencia { get; set; }
        public string RealizoCursos { get; set; }
        public string PidioCredito { get; set; }
        public string OtorgaronCredito { get; set; }
        public string InstitucionSolicitud { get; set; }
    }
}
