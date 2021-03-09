﻿using Formulario.Dominio.IRepositorio;
using Infraestructura.Core.Datos;
using NHibernate;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using DestinoFondos = Formulario.Dominio.Modelo.DestinoFondos;

namespace Datos.Repositorios.Formulario
{
    public class DestinosFondosRepositorio : NhRepositorio<DestinoFondos>, IDestinoFondosRepositorio
    {
        public DestinosFondosRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<DestinoFondos> ConsultarDestinosFondos()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_DESTINOS_FONDO")
                .ToListResult<DestinoFondos>();
            return result;
        }

        public IList<DestinoFondoSeleccionadoResultado> ConsultarDestinosFondosPorFormulario(int idFormulario)
        {
            return Execute("PR_OBTENER_DEST_FONDO_X_FORM")
                .AddParam(idFormulario)
                .ToListResult<DestinoFondoSeleccionadoResultado>();

        }
    }
}