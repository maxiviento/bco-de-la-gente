using System.Collections.Generic;
using Pagos.Aplicacion.Consultas.Consultas;

namespace Pagos.Aplicacion.Comandos
{
    public class RegistrarLoteSuafComando
    {
        public List<int> IdPrestamosSeleccionados { get; set; }
        public string NombreLote { get; set; }
        public BandejaFormulariosSuafConsulta Consulta { get; set; }
    }
}
