using System;
using System.Collections.Generic;
using AppComunicacion.ApiModels;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class LocalidadResultado
    {
        public Id IdLocalidad { get; set; }
        public Id IdDepartamento { get; set; }
        public string Localidad { get; set; }
    }
}