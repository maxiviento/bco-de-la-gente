namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class EtapaEstadoLineaResultado
    {
        public EtapaEstadoLineaResultado() { }

        public EtapaEstadoLineaResultado(long orden, long? idEtapaActual, long idEstadoActual, long? idEtapaSiguiente, long idEstadoSiguiente, long idLineaPrestamo)
        {
            Orden = orden;
            IdEtapaActual = idEtapaActual;
            IdEstadoActual = idEstadoActual;
            IdEtapaSiguiente = idEtapaSiguiente;
            IdEstadoSiguiente = idEstadoSiguiente;
            IdLineaPrestamo = idLineaPrestamo;
        }

        public long Orden { get; set; }
        public long? IdEtapaActual { get; set; }
        public long IdEstadoActual { get; set; }
        public long? IdEtapaSiguiente { get; set; }
        public long IdEstadoSiguiente { get; set; }
        public long IdLineaPrestamo { get; set; }
        public long? Id { get; set; }
    }
}
