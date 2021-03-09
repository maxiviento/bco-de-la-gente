using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Dominio.IRepositorio
{
    public interface IMontoDisponibleRepositorio : IRepositorio<MontoDisponible>
    {
        decimal Registrar(MontoDisponible montoDisponible);
        MontoDisponible ObtenerPorId(decimal id);
        MontoDisponible ObtenerPorNro(decimal nroMonto);
        bool DarDeBaja(MontoDisponible montoDisponible);
        bool ExisteMontoDisponibleConLaMismaDescripcion(string descripcion);
        EditarMontoDisponibleResultado Modificar(MontoDisponible montoDisponible);
        Resultado<BandejaMontoDisponibleResultado> ObtenerMontosDisponibleBandeja(BandejaMontoDisponibleConsulta consulta);
        IList<MontoDisponible> ConsultarMontoDisponiblesCombo();
        Resultado<MovimientoMontoResultado> ObtenerMovimientosMonto(MovimientosMontoConsulta consulta);
    }
}
