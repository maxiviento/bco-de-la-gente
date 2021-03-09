using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IItemRepositorio
    {
        decimal RegistrarItem(Item item);
        Resultado<ItemResultado.Grilla> ConsultarItemsPorNombre(ItemConsultaPaginada itemConsulta);
        bool ExisteItemConNombre(string nombreItem);
        Item ConsultarPorId(decimal id);
        void DarDeBaja(Item item);
        void Modificar(Item item);
        IList<ConsultarRecursoResultado> ConsultarRecursos { get; }
        IList<ItemResultado.Grilla> ConsultarItems();
        IList<ItemResultado.Grilla> ConsultarItemsCombo();
        bool PoseeHijos(int idItem);
    }
}