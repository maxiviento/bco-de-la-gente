namespace Soporte.Dominio.Modelo
{
    public class RentasPrestamoPlano
    {
        public string ApellidoNombre { get; set; }
        public string NroDocumento { get; set; }
        public string Sexo { get; set; }
        public string CodigoPais { get; set; }
        public string NroSticker { get; set; }
        public string CondicionEdad { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
        public string Linea { get; set; }
        public string NroPrestamo { get; set; }
        public bool EsSolicitante { get; set; }
        public string GF_ApellidoNombre { get; set; }
        public string GF_NroDocumento { get; set; }
        public string GF_Edad { get; set; }
        public string GF_Objeto { get; set; }
        public int GF_Modelo { get; set; }
        public string GF_Marca { get; set; }
        public decimal GF_BaseImponible { get; set; }
        public decimal GF_Porcentaje { get; set; }
        public string GF_Estado { get; set; }
        public decimal GF_Superficie { get; set; }
        public string GF_Domicilio { get; set; }
        public int GF_IdNumero { get; set; }
        public string GF_IdSexo { get; set; }
        public string GF_IdPais { get; set; }
        public bool EnabledAlert { get; set; }
    }
}