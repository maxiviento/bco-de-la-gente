using System;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class ArchivoPrestamosConsulta : Consulta
    {
        public string NroPrestamo { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Cuil { get; set; }
        public string NroFormulario { get; set; }

    }
}
