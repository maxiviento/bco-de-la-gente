using System;
using System.Collections.Generic;
using System.Globalization;
namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ReporteDeudaGrupoConvivienteResultado
    {
        public ReporteDeudaGrupoConvivienteResultado() { }

        public ReporteDeudaGrupoConvivienteResultado(GrupoFamiliarResultado persona)
        {
            IdNumero = persona.IdNumero;
            IdSexo = persona.IdSexo;
            CodPais = persona.CodPais;
            NombreCompleto = persona.NombreCompleto;
            TipoDocumento = persona.TipoDocumento;
            NumeroDocumento = persona.NroDocumento;
            Sexo = persona.Sexo;
            FechaNacimiento = persona.FechaNacimiento;
            FechaDefuncion = persona.FechaDefuncion;
            Edad = persona.Edad;
        }

        public int IdNumero { get; set; }
        public string IdSexo { get; set; }
        public string CodPais { get; set; }
        public string NombreCompleto { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string NumeroFormulario { get; set; }
        public string IdFormulario { get; set; }
        public string FechaUltimoMovimiento { get; set; }
        public string PrestamoBeneficio { get; set; }
        public string Importe { get; set; }
        public string CantCuotasPagas { get; set; }
        public string ImpCuotasPagas { get; set; }
        public string CantCuotasImpagas { get; set; }
        public string ImpCuotasImpagas { get; set; }
        public string CantCuotasVencidas { get; set; }
        public string ImpCuotasVencidas { get; set; }
        public string CantCuotas { get; set; }
        public string ImporteCuota { get; set; }
        public string MotivoBaja { get; set; }
        public string Estado { get; set; }
        public long IdEstado { get; set; }
        public string FechaDefuncion { get; set; }

        public ReporteDeudaGrupoConvivienteResultado CargarDatosDeuda(ConsultaDeudaFormularioResultado datos)
        {
            ImporteCuota = datos.ImporteCuota.GetValueOrDefault().ToString(CultureInfo.CurrentCulture);
            CantCuotasPagas = datos.CantidadCuotasPagas != null ? datos.CantidadCuotasPagas.ToString() : string.Empty;
            CantCuotasImpagas = datos.CantidadCuotasImpagas != null ? datos.CantidadCuotasImpagas.ToString() : string.Empty;
            CantCuotasVencidas = datos.CantidadCuotasVencidas != null ? datos.CantidadCuotasVencidas.ToString() : string.Empty;
            ImpCuotasImpagas = RedondearImportes(datos.ImporteCuota.GetValueOrDefault() * datos.CantidadCuotasImpagas.GetValueOrDefault());
            ImpCuotasPagas = RedondearImportes(datos.ImporteCuota.GetValueOrDefault() * datos.CantidadCuotasPagas.GetValueOrDefault());
            ImpCuotasVencidas = RedondearImportes(datos.ImporteCuota.GetValueOrDefault() * datos.CantidadCuotasVencidas.GetValueOrDefault());
            string motivosRechazo = AppendMotivosRechazo(datos.MotivosRechazo);
            MotivoBaja = !string.IsNullOrEmpty(motivosRechazo) ? motivosRechazo : string.Empty;
            Estado = !string.IsNullOrEmpty(datos.Estado) ? datos.Estado : string.Empty;
            IdEstado = datos.IdEstadoFormulario;
            return this;
        }

        private string RedondearImportes(decimal importe)
        {
            double decimales = (double) importe % 1;
            string res = "";

            if (decimales < 0.5)
            {
                res = Math.Round(importe).ToString();
            }

            if (decimales >= 0.5)
            {
                res = Math.Ceiling(importe).ToString();
            }

            return res;
        }

        private string AppendMotivosRechazo(List<int> idsMotivoRechazo)
        {
            string motivosBaja = "";
            if(idsMotivoRechazo == null) return motivosBaja;
            for (int i = 0; i < idsMotivoRechazo.Count; i++)
            {
                string motivo = idsMotivoRechazo[i].ToString();
                if (!string.IsNullOrEmpty(motivo))
                {
                    motivosBaja += motivo;
                    if (i != idsMotivoRechazo.Count - 1) motivosBaja += ", ";
                }
            }
            return motivosBaja;
        }
    }
}
