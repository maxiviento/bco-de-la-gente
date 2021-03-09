using System.Collections.Generic;
using Configuracion.Dominio;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Configuracion.Tests
{
    [TestFixture]
    public class ItemTest
    {
        private string _nombre;
        private string _descripcion;
        private Usuario _usuario;
        private MotivoBaja _motivoBaja;
        private Randomizer _randomizer;
        private IList<TipoItem> _tiposItem;
        private Item _itemPadre;
        private Recurso _recurso;

        [SetUp]
        public void Init()
        {
            _nombre = "Nombre del item de prueba";
            _descripcion = "Esta es la descripción del item de prueba";
            _usuario = new Usuario {Id = new Id(1), Cuil = "11111111111"};
            _tiposItem = new List<TipoItem>()
            {
                new TipoItem("Nombre", "Descripción")
            };
            _motivoBaja = new MotivoBaja {Id = new Id(1), Descripcion = "Motivo prueba"};
            _randomizer = new Randomizer();
            _recurso = new Recurso();
            _itemPadre = new Item();
        }

        [Test]
        public void FallaAlRegistrarSinNombre()
        {
            Assert.That(() => new Item(_nombre, null, _tiposItem, _usuario, _recurso, _itemPadre),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarSinDescripcion()
        {
            Assert.That(() => new Item(null, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarConNombreMayorALoPermitido()
        {
            var nombreFueraDeRango = _randomizer.GetString(101);
            Assert.That(() => new Item(nombreFueraDeRango, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarConDescripcionMayorALoPermitido()
        {
            var descripcionFueraDeRango = _randomizer.GetString(201);
            Assert.That(() => new Item(_nombre, descripcionFueraDeRango, _tiposItem, _usuario, _recurso, _itemPadre),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarItemConTiposItemNulo()
        {
            Assert.That(() => new Item(_nombre, _descripcion, null, _usuario, _recurso, _itemPadre), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarItemConTiposItemVacio()
        {
            Assert.That(() => new Item(_nombre, _descripcion, new List<TipoItem>(), _usuario, _recurso, _itemPadre),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void RegistraCorrectamente()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre);
            Assert.AreEqual(_nombre, itemDePrueba.Nombre);
            Assert.AreEqual(_descripcion, itemDePrueba.Descripcion);
        }
        [Test]
        public void RegistraCorrectamenteSinItemPadre()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem, _usuario, _recurso, null);
            Assert.AreEqual(_nombre, itemDePrueba.Nombre);
            Assert.AreEqual(_descripcion, itemDePrueba.Descripcion);
        }
        [Test]
        public void RegistraCorrectamenteSinRecurso()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem, _usuario, null, _itemPadre);
            Assert.AreEqual(_nombre, itemDePrueba.Nombre);
            Assert.AreEqual(_descripcion, itemDePrueba.Descripcion);
        }

        [Test]
        public void FallaAlModificarSinNombre()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre);
            Assert.That(() => itemDePrueba.Modificar(null, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre, false, false, null),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarSinDescripcion()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem);
            Assert.That(() => itemDePrueba.Modificar(_nombre, null, _tiposItem, _usuario, _recurso, _itemPadre, false, false, null),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarConNombreMayorALoPermitido()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem);
            Assert.That(() => itemDePrueba.Modificar(_randomizer.GetString(101), _descripcion, _tiposItem, _usuario, _recurso, _itemPadre, false, false, null),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarConDescripcionMayorALoPermitido()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem);
            Assert.That(() => itemDePrueba.Modificar(_nombre, _randomizer.GetString(201), _tiposItem, _usuario, _recurso, _itemPadre, false, false, null),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarDadoDeBaja()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem);
            itemDePrueba.DarDeBaja(_motivoBaja, _usuario);
            Assert.That(() => itemDePrueba.Modificar(_nombre, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre, false, false, null),
                Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void ModificaCorrectamente()
        {
            var itemDePrueba = new Item("Nombre sin modificar", "Descripcion sin modificar", _tiposItem);
            itemDePrueba.Modificar(_nombre, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre, false, false, null);
            Assert.AreEqual(_nombre, itemDePrueba.Nombre);
            Assert.AreEqual(_descripcion, itemDePrueba.Descripcion);
        }

        [Test]
        public void FallaAlDarDeBajaYaDadoDeBaja()
        {
            var itemDePrueba = new Item(_nombre, _descripcion, _tiposItem, _usuario, _recurso, _itemPadre);
            itemDePrueba.DarDeBaja(_motivoBaja, _usuario);
            Assert.That(() => itemDePrueba.DarDeBaja(_motivoBaja, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }
    }
}