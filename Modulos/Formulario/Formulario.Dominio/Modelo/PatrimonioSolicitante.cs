namespace Formulario.Dominio.Modelo
{
    public sealed class PatrimonioSolicitante
    {
        public bool InmueblePropio { get; private set; }
        public decimal ValorInmueble { get; private set; }
        public bool VehiculoPropio { get; private set; }
        public decimal CantidadVehiculos { get; private set; }
        public string ModeloVehiculos { get; private set; }
        public decimal ValorVehiculos { get; private set; }
        public decimal ValorDeudas { get; private set; }

        private PatrimonioSolicitante()
        {
        }

        public PatrimonioSolicitante(bool inmueblePropio, decimal valorInmueble, bool vehiculoPropio, decimal cantidadVehiculos,
            string modeloVehiculos, decimal valorVehiculos, decimal valorDeudas)
            : this()
        {
            InmueblePropio = inmueblePropio;
            ValorInmueble = valorInmueble;
            VehiculoPropio = vehiculoPropio;
            CantidadVehiculos = cantidadVehiculos;
            ModeloVehiculos = modeloVehiculos;
            ValorVehiculos = valorVehiculos;
            ValorDeudas = valorDeudas;
        }
    }
}