using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Configuracion.Aplicacion.Servicios
{
    public class ItemServicio
    {
        private readonly IItemRepositorio _itemRepositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly ITipoItemRepositorio _tipoItemRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly ITipoDocumentacionRepositorio _tipoDocumentacionRepositorio;

        public ItemServicio(IItemRepositorio itemRepositorio, IMotivoBajaRepositorio motivoBajaRepositorio,
            ITipoItemRepositorio tipoItemRepositorio, ISesionUsuario sesionUsuario,
            ITipoDocumentacionRepositorio tipoDocumentacionRepositorio)
        {
            _itemRepositorio = itemRepositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _tipoItemRepositorio = tipoItemRepositorio;
            _sesionUsuario = sesionUsuario;
            _tipoDocumentacionRepositorio = tipoDocumentacionRepositorio;
        }

        public NuevoItemResultado RegistrarItem(ItemComando comando)
        {
            if (ExisteItemConNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException("El item que intenta registrar ya se encuentra registrado");
            }

            Usuario usuario = _sesionUsuario.Usuario;

            var tiposItem = ObtenerTiposItem(comando);

            var tipoDocumentacion = new TipoDocumentacion();

            if (comando.IdTipoDocumentacionCdd != null)
            {
                tipoDocumentacion = ObtenerTipoDocumentacion(new Id((long) comando.IdTipoDocumentacionCdd));
            }

            var item = new Item(comando.Nombre, comando.Descripcion, tiposItem, usuario, comando.SubeArchivo,
                comando.GeneraArchivo, tipoDocumentacion);

            if (comando.IdRecurso != null)
            {
                var recurso = ObtenerRecurso(comando.IdRecurso.Value);
                item.Recurso = recurso;
            }

            if (comando.IdItemPadre != null)
            {
                item.ItemPadre = _itemRepositorio.ConsultarPorId(comando.IdItemPadre.Value);
            }

            return new NuevoItemResultado {Id = _itemRepositorio.RegistrarItem(item)};
        }

        public TipoDocumentacion ObtenerTipoDocumentacion(Id idTipoDocumentacion)
        {
            var tipoDocumentacion = _tipoDocumentacionRepositorio.ConsultarTipoDocumentacionPorId(idTipoDocumentacion);

            return tipoDocumentacion;
        }

        public Recurso ObtenerRecurso(decimal? idRecurso)
        {
            if (idRecurso == null) return null;

            var recursosResultado = _itemRepositorio.ConsultarRecursos.ToList();
            var recursoResultado = recursosResultado.First(c => c.Id.Valor == idRecurso);
            return new Recurso(recursoResultado.Id, recursoResultado.Nombre, recursoResultado.Descripcion,
                recursoResultado.Url);
        }

        private IList<TipoItem> ObtenerTiposItem(ItemComando comando)
        {
            if (comando.TiposItem == null || comando.TiposItem.Count == 0) return null;

            var todosLosItems = _tipoItemRepositorio.ConsultarTiposItem().ToList();
            return comando.TiposItem.Select(tipoItem => todosLosItems.First(c => c.Id.Valor == tipoItem.Id))
                .ToList();
        }

        public bool ExisteItemConNombre(string nombre)
        {
            return _itemRepositorio.ExisteItemConNombre(nombre);
        }

        public Resultado<ItemResultado.Grilla> ConsultarItemsPorNombre(ItemConsultaPaginada itemConsulta)
        {
            if (itemConsulta == null)
            {
                itemConsulta = new ItemConsultaPaginada {NumeroPagina = 0};
            }

            itemConsulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _itemRepositorio.ConsultarItemsPorNombre(itemConsulta);
        }

        public IList<ItemResultado.Grilla> ConsultarItems()
        {
            return _itemRepositorio.ConsultarItems();
        }

        public IList<ItemResultado.Grilla> ConsultarItemsCombo()
        {
            return _itemRepositorio.ConsultarItemsCombo();
        }

        public IList<ConsultarRecursoResultado> ConsultarRecursos()
        {
            return _itemRepositorio.ConsultarRecursos;
        }


        public ItemResultado.Detalle ConsultarItemPorId(long idItem)
        {
            var item = _itemRepositorio.ConsultarPorId(idItem);
            var tiposItem = _tipoItemRepositorio.ConsultarTiposItemPorItem(idItem);

            var tiposItemResultado = tiposItem.Select(tipoItem => new TipoItemResultado
                    {
                        Id = (int) tipoItem.Id.Valor,
                        Nombre = tipoItem.Descripcion
                    }
                )
                .ToList();

            Item itemPadre = null;

            if (item.ItemPadre != null)
            {
                itemPadre = _itemRepositorio.ConsultarPorId(item.ItemPadre.Id.Valor);
            }

            Recurso recurso = null;

            if (item.Recurso != null)
            {
                recurso = ObtenerRecurso(item.Recurso.Id.Valor);
            }

            TipoDocumentacion tipoDocumentacion = null;

            if (item.TipoDocumentacion != null)
            {
                tipoDocumentacion = ObtenerTipoDocumentacion(item.TipoDocumentacion.Id);
            }

            return new ItemResultado.Detalle
            {
                Id = item.Id,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                IdUsuarioAlta = item.UsuarioAlta.Id,
                CuilUsuarioAlta = item.UsuarioAlta.Cuil,
                IdMotivoBaja = item.MotivoBaja?.Id,
                NombreMotivoBaja = item.MotivoBaja?.Descripcion,
                FechaUltimaModificacion = item.FechaUltimaModificacion,
                IdUsuarioUltimaModificacion = item.UsuarioUltimaModificacion.Id,
                CuilUsuarioUltimaModificacion = item.UsuarioUltimaModificacion.Cuil,
                TiposItem = tiposItemResultado,
                IdItemPadre = itemPadre?.Id,
                NombreItemPadre = itemPadre?.Nombre,
                IdRecurso = recurso?.Id,
                NombreRecurso = recurso?.Nombre,
                UrlRecurso = recurso?.Url,
                SubeArchivo = item.SubeArchivo,
                GeneraArchivo = item.GeneraArchivo,
                IdTipoDocumentacionCdd = tipoDocumentacion?.Id,
                DescripcionTipoDocumentacion = tipoDocumentacion?.Descripcion
            };
        }

        public void Modificar(int id, ItemComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var item = _itemRepositorio.ConsultarPorId(id);
            if (item.Nombre != comando.Nombre && _itemRepositorio.ExisteItemConNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException("Los datos que se intentan modificar ya existen para otro item");
            }

            var tiposItem = ObtenerTiposItem(comando);

            var recurso = ObtenerRecurso(comando.IdRecurso);

            Item itemPadre = null;
            if (comando.IdItemPadre != null)
                itemPadre = _itemRepositorio.ConsultarPorId(comando.IdItemPadre.Value);

            var tipoDocumentacion = new TipoDocumentacion();

            if (comando.IdTipoDocumentacionCdd != null)
            {
                tipoDocumentacion = ObtenerTipoDocumentacion(new Id((long) comando.IdTipoDocumentacionCdd));
            }

            item.Modificar(comando.Nombre, comando.Descripcion, tiposItem, usuario, recurso,
                itemPadre, comando.SubeArchivo, comando.GeneraArchivo, tipoDocumentacion);
            _itemRepositorio.Modificar(item);
        }

        public void DarDeBaja(int id, DarDeBajaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var item = _itemRepositorio.ConsultarPorId(id);
            item.DarDeBaja(_motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja)), usuario);
            _itemRepositorio.DarDeBaja(item);
        }

        public bool PoseeHijos(int idItem)
        {
            return _itemRepositorio.PoseeHijos(idItem);
        }
    }
}