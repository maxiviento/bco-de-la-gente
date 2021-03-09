using System;
using System.Collections.Generic;
using System.Linq;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Dominio.Modelo
{
    public class DatoProvidencia : Entidad
    {
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreLinea { get; set; }
        public string DetalleLinea { get; set; }
        public string Destino { get; set; }
        public string Importe { get; set; }
        public string ImporteLetras { get; set; }
        public string Programa { get; set; }
        public string NombrePrograma { get; set; }
        public int Sticker { get; set; }
        public int NroFormulario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cuil { get; set; }
    }
}