namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DetalleLineaParaFormularioResultado
    {
        public int Id { get; set; }
        public int LineaId { get; set; }
        public int TipoIntegranteId { get; set; }
        public int MontoPrestable { get; set; }
        public int MontoTopeIntegrante { get; set; }
        public int CantidadMaximaIntegrantes { get; set; }
        public int CantidadMinimaIntegrantes { get; set; }
        public int PlazoDevolucionMaximo { get; set; }
        public int ValorCuotaSocioIndep { get; set; }
        public int ValorCuotaSocioAsoc { get; set; }
        public int TipoGarantiaId { get; set; }
        public string Visualizacion { get; set; }
        public bool ConOng { get; set; }
        public bool ConCurso { get; set; }
        public string ConCursoStr { get; set; }
        public bool ConPrograma { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string SexoDestinatarioId { get; set; }
        public string Color { get; set; }
        public string Logo { get; set; }
        public bool Apoderado { get; set; }
    }
}