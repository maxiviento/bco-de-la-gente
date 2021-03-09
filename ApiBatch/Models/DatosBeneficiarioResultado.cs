using ApiBatch.Operations.QueueManager;
using System;
using ApiBatch.Base.QueueManager;

namespace ApiBatch.Models
{
    public class DatosBeneficiarioResultado: IDatos
    {
        public string InscripcionId { set; get; }
        public string Apellido { set; get; }
        public string Nombre { set; get; }
        public string NumeroDocumento { set; get; }
        public string Sexo { get; set; }
        public string Direccion { set; get; }
        public string Departamento { set; get; }
        public string Telefono { set; get; }
        public string Localidad { set; get; }
        public string EstadoInscripcion { get; set; }
        public string EstadoInscripcionId { get; set; }
        public string ServicioSolicitadoIdAgua { get; set; }
        public string ServicioSolicitadoIdLuz { get; set; }
        public string ServicioSolicitadoIdRentas { get; set; }
        public string BeneficioOtorgadoAgua { set; get; }
        public string BeneficioOtorgadoLuz { set; get; }
        public string BeneficioOtorgadoRentas { set; get; }
        public string NumeroResolucionAgua { set; get; }
        public string NumeroResolucionLuz { set; get; }
        public string NumeroResolucionRentas { set; get; }
        public DateTime FechaResolucionAgua { set; get; }
        public DateTime FechaResolucionLuz { set; get; }
        public DateTime FechaResolucionRentas { set; get; }
        public string NumeroFacturacionAgua { set; get; }
        public string NumeroFacturacionLuz { set; get; }
        public string NumeroFacturacionRentas { set; get; }
        public string PrestadorAgua { get; set; }
        public string PrestadorLuz { get; set; }
        public string PrestadorRentas { get; set; }

        public virtual string FechaResolucionAguaSinHora
        {
            get
            {
                if (FechaResolucionAgua.Equals(DateTime.MinValue))
                {
                    return string.Empty;
                }
                return FechaResolucionAgua.ToShortDateString();
            }
        }

        public virtual string FechaResolucionLuzSinHora
        {
            get
            {
                if (FechaResolucionLuz.Equals(DateTime.MinValue))
                {
                    return string.Empty;
                }
                return FechaResolucionLuz.ToShortDateString();
            }
        }

        public virtual string FechaResolucionRentasSinHora
        {
            get
            {
                if (FechaResolucionRentas.Equals(DateTime.MinValue))
                {
                    return string.Empty;
                }
                return FechaResolucionRentas.ToShortDateString();
            }
        }
    }
}