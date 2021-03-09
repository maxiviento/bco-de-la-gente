namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ImportarArchivoResultadoBancoResultado
    {
        public bool CoincidenMontos { get; set; }
        public bool HayError { get; set; }


        public ImportarArchivoResultadoBancoResultado()
        {
            CoincidenMontos = true;
            HayError = false;
        }
}
}
