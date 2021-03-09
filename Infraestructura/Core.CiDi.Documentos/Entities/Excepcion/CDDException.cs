namespace Core.CiDi.Documentos.Entities.Excepcion
{
    public class CDDException : System.Exception
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Constructor parametrizado.
        /// </summary>
        /// <param name="_error_code">Código de error.</param>
        /// <param name="_error_description">Descripción de error.</param>
        public CDDException(string _error_code, string _error_description)
        {
            this.ErrorCode = _error_code;
            this.ErrorDescription = _error_description;
        }
    }
}