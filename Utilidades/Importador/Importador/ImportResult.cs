namespace Utilidades.Importador
{
    public class ImportResult<T>
    {
        public T Raw { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

    }
}
