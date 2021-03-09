using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.Modelo
{
    public class TablaDefinida : Entidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaDesde { get; set; }
        public List<ParametroTablaDefinida> Parametros { get; set; }
    }
}
