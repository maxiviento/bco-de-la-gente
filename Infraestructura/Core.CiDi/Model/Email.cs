namespace Infraestructura.Core.CiDi.Model
{
    internal class Email
    {
        public int Id_App { get; set; }
        public string Pass_App { get; set; }
        public string Cuil { get; set; }
        public string Asunto { get; set; }
        public string Subtitulo { get; set; }
        public string Mensaje { get; set; }
        public string InfoDesc { get; set; }
        public string InfoDato { get; set; }
        public string InfoLink { get; set; }
        public string Firma { get; set; }
        public string Ente { get; set; }
        public string TokenValue { get; set; }
        public string TimeStamp { get; set; }
    }
}
