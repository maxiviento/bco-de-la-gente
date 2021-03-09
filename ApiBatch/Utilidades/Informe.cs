using Identidad.Dominio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiBatch.Utilidades
{
    public class Informe : Infraestructura.Core.Comun.Dato.Entidad
    {
        public Informe(string nombre, string path, TipoInforme tipoInforme, Usuario usuario)
        {
            Nombre = nombre;
            Path = path;
            TipoInforme = tipoInforme;
            Usuario = usuario;
            FechaCreacion = DateTime.Now;
        }

        public void Generar(string path)
        {
            Path = path;
        }

        public void Cancelar()
        {
        }

        public virtual string Nombre { get; protected set; }
        public virtual string Path { get; set; }
        public virtual TipoInforme TipoInforme { get; protected set; }
        public virtual DateTime FechaCreacion { get; protected set; }
        public virtual DateTime FechaModificacion { get; protected set; }
        public virtual Usuario Usuario { get; protected set; }
    }
}