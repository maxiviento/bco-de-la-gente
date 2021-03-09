using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class ConsultarGrupoFamiliarComando
    {
        public IEnumerable<DatosIntegranteComando> Integrantes { get; set; }
    }
}