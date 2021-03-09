namespace Infraestructura.Core.CiDi.Model
{
    public class Entrada
    {
        public int IdAplicacion { get; set; }
        public string Contrasenia { get; set; }
        public string HashCookie { get; set; }
        public string TokenValue { get; set; }
        public string TimeStamp { get; set; }
        public string CUIL { get; set; }
    }
}
