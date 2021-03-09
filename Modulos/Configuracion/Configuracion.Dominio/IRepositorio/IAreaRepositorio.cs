using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IAreaRepositorio : IRepositorio<Area>
    {
        decimal RegistrarArea(Area area);
        bool ExisteAreaConMismoNombre(string nombre);
        Area ConsultarPorId(decimal id);
        Resultado<AreaResultado.Consulta> Consultar(ConsultarAreas consultarAreas);
        void DarDeBaja(Area area);
        void Modificar(Area area);
        IList<AreaResultado.Consulta> ConsultarAreasCombo();
    }
}