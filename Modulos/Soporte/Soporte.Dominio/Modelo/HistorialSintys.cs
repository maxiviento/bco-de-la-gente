using System;

namespace Soporte.Dominio.Modelo
{
    public class HistorialSintys
    {
        public string IdSexo { get; set; }
        public string CodPais { get; set; }
        public int IdNum { get; set; }
        public bool EsSolicitante { get; set; }
        public string NroDoc { get; set; }
        public string ActPeriodo { get; set; }
        public string ActMonto { get; set; }
        public string ActEmpleador { get; set; }
        public string ActCuit { get; set; }
        public string PasPeriodo { get; set; }
        public string PasDesc { get; set; }
        public string PasMonto { get; set; }
        public DateTime? Fallecido { get; set; }
        public int CantCuota { get; set; }
        public int CantLiq { get; set; }
        public int Periodo { get; set; }
    }
}
