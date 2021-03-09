namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class DetallesAdenda
    {
        public int IdLote { get; set; }
        public decimal NroDetalle { get; set; }
        public int? NroPrestamoChecklist { get; set; }
        public bool Agrega { get; set; }
    }
}