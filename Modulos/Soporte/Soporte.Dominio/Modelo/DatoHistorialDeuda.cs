using System;
using System.Collections.Generic;
using System.Linq;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Dominio.Modelo
{
    public class DatoHistorialDeuda : Entidad
    {
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreLinea { get; set; }
        public int NroPrestamo { get; set; }
        public string MotivosRechazo { get; set; }

        public List<int> ListadoMotivosRechazo => string.IsNullOrEmpty(MotivosRechazo) ? null : MotivosRechazo.Split(';').ToList().Select(int.Parse).ToList();
    }
}