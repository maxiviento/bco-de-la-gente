using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;
using Infraestructura.Core.Comun.Presentacion;
using NHibernate.Util;

namespace Datos.Repositorios.Configuracion
{
    public class ItemRepositorio : NhRepositorio<Item>, IItemRepositorio
    {
        public ItemRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarItem(Item item)
        {
            var tiposItem = PrepararTiposItem(item);
            var resultado = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_ITEMS")
                .AddParam(default(decimal?))
                .AddParam(item.ItemPadre?.Id)
                .AddParam(item.Nombre)
                .AddParam(item.Descripcion)
                .AddParam(item.Recurso?.Id)
                .AddParam(tiposItem)
                .AddParam(item.SubeArchivo)
                .AddParam(item.GeneraArchivo)
                .AddParam(item.TipoDocumentacion.Id)
                .AddParam(default(decimal?))
                .AddParam(item.UsuarioAlta.Id)
                .ToSpResult();
            item.Id = resultado.Id;
            return item.Id.Valor;
        }

        private string PrepararTiposItem(Item item)
        {
            var tiposItemSeleccionados = item.TiposItem;
            if (tiposItemSeleccionados == null || tiposItemSeleccionados.Count == 0) return string.Empty;

            string[] tiposItemParam = {""};
            tiposItemSeleccionados.ForEach(tipoItem => { tiposItemParam[0] += tipoItem.Id + ","; });

            return tiposItemParam[0].TrimEnd(',');
        }


        public Resultado<ItemResultado.Grilla> ConsultarItemsPorNombre(ItemConsultaPaginada itemConsulta)
        {
            itemConsulta.TamañoPagina++;

            var paginaDesde = itemConsulta.PaginaDesde == 0
                ? itemConsulta.PaginaDesde + 1
                : itemConsulta.PaginaDesde - itemConsulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1
                ? itemConsulta.PaginaHasta
                : itemConsulta.PaginaHasta - itemConsulta.NumeroPagina;

            var incluirDadosBaja = "NULL";
            var esSubItem = "NULL";
            var incluirHijos = "NULL";

            if (itemConsulta.IncluirDadosBaja.HasValue)
            {
                incluirDadosBaja = itemConsulta.IncluirDadosBaja.Value ? "S" : "N";
            }
            if (itemConsulta.EsSubItem.HasValue)
            {
                esSubItem = itemConsulta.EsSubItem.Value ? "S" : "N";
            }
            if (itemConsulta.IncluirHijos.HasValue)
            {
               incluirHijos= itemConsulta.IncluirHijos.Value ? "S" : "N";
            }


            var elementos = Execute("PR_OBTENER_ITEMS")
                .AddParam(itemConsulta.IdItem)
                .AddParam(incluirDadosBaja)
                .AddParam(itemConsulta.Recurso ?? -1)
                .AddParam(esSubItem)
                .AddParam(incluirHijos)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<ItemResultado.Grilla>();
            var resultado = CrearResultado(itemConsulta, elementos);

            return resultado;
        }

        public IList<ConsultarRecursoResultado> ConsultarRecursos
        {
            get
            {
                var resultado = Execute("PR_OBTENER_RECURSOS")
                    .ToListResult<ConsultarRecursoResultado>();
                return resultado;
            }
        }

        public IList<ItemResultado.Grilla> ConsultarItems()
        {
            var resultado = Execute("PR_OBTENER_ITEMS_PADRE")
                .ToListResult<ItemResultado.Grilla>();
            return resultado;
        }

        public IList<ItemResultado.Grilla> ConsultarItemsCombo()
        {
            var resultado = Execute("PR_OBTENER_ITEMS_DESCRIP")
                .ToListResult<ItemResultado.Grilla>();
            return resultado;
        }

        public bool ExisteItemConNombre(string nombreItem)
        {
            var resultado = Execute("PR_EXISTE_ITEM")
                                .AddParam(nombreItem)
                                .ToEscalarResult<string>() == "S";
            return resultado;
        }

        public Item ConsultarPorId(decimal id)
        {
            var resultado = Execute("PR_OBTENER_ITEM_POR_ID")
                .AddParam(id)
                .ToUniqueResult<Item>();
            return resultado;
        }

        public void DarDeBaja(Item item)
        {
            var tiposItem = PrepararTiposItem(item);
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_ITEMS")
                .AddParam(item.Id)
                .AddParam(item.ItemPadre?.Id)
                .AddParam(item.Nombre)
                .AddParam(item.Descripcion)
                .AddParam(item.Recurso?.Id)
                .AddParam(tiposItem)
                .AddParam(item.SubeArchivo)
                .AddParam(item.GeneraArchivo)
                .AddParam(item.TipoDocumentacion?.Id)
                .AddParam(item.MotivoBaja.Id)
                .AddParam(item.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public void Modificar(Item item)
        {
            var tiposItem = PrepararTiposItem(item);
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_ITEMS")
                .AddParam(item.Id)
                .AddParam(item.ItemPadre?.Id)
                .AddParam(item.Nombre)
                .AddParam(item.Descripcion)
                .AddParam(item.Recurso?.Id)
                .AddParam(tiposItem)
                .AddParam(item.SubeArchivo)
                .AddParam(item.GeneraArchivo)
                .AddParam(item.TipoDocumentacion.Id)
                .AddParam(default(long?))
                .AddParam(item.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public bool PoseeHijos(int IdItem)
        {
            return Execute("PR_OBTENER_HIJOS_ITEM")
                .AddParam(IdItem)
                .ToEscalarResult<bool>();
        }
    }
}