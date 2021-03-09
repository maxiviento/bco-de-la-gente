using System;
using System.Collections.Generic;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo;

namespace Soporte.Dominio.IRepositorio
{
    public interface IRentaRepositorio : IRepositorio<DatoRenta>
    {
        Id RegistrarDatoRenta(decimal idFormularioLinea, decimal idPrestamo, RentasPrestamoPlano dato, Usuario usuario, DateTime fechaAlta, decimal idPrestamoRequisito);
        List<HistorialRentas> ObtenerHistorial(decimal IdHistorial);
        Resultado<DocumentacionResultado> ObtenerTodosHistorialesRentas(DocumentacionConsulta consulta);
    }
}