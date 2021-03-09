using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Configuracion.Tests
{
    public class AreaTest
    {
        private string _nombre;
        private string _descripcion;
        private Usuario _usuario;
        private MotivoBaja _motivoBaja;
        private Randomizer _randomizer;

        [SetUp]
        public void Init()
        {
            _nombre = "Nombre del área de prueba";
            _descripcion = "Esta es la descripción del área de prueba";
            _usuario = new Usuario {Id = new Id(1), Cuil = "11111111111"};
            _motivoBaja = new MotivoBaja { Id = new Id(1), Descripcion = "Motivo prueba" };
            _randomizer = new Randomizer();
        }

        [Test]
        public void FallaAlRegistrarSinNombre()
        {
            Assert.That(() => new Area(_nombre, null), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarSinDescripcion()
        {
            Assert.That(() => new Area(null, _descripcion), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarConNombreMayorALoPermitido()
        {
            var nombreFueraDeRango = _randomizer.GetString(101);
            Assert.That(() => new Area(nombreFueraDeRango, _descripcion), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarConDescripcionMayorALoPermitido()
        {
            var descripcionFueraDeRango = _randomizer.GetString(201);
            Assert.That(() => new Area(_nombre, descripcionFueraDeRango), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void RegistraCorrectamente()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            Assert.AreEqual(_nombre, areaDePrueba.Nombre);
            Assert.AreEqual(_descripcion, areaDePrueba.Descripcion);
        }

        [Test]
        public void FallaAlModificarSinNombre()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            Assert.That(() => areaDePrueba.Modificar(null, _descripcion, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarSinDescripcion()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            Assert.That(() => areaDePrueba.Modificar(_nombre, null, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarConNombreMayorALoPermitido()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            Assert.That(() => areaDePrueba.Modificar(_randomizer.GetString(101), _descripcion, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarConDescripcionMayorALoPermitido()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            Assert.That(() => areaDePrueba.Modificar(_nombre, _randomizer.GetString(201), _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarDadaDeBaja()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            areaDePrueba.DarDeBaja(_motivoBaja, _usuario);
            Assert.That(() => areaDePrueba.Modificar(_nombre, _descripcion, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void ModificaCorrectamente()
        {
            var areaDePrueba = new Area("Nombre sin modificar", "Descripcion sin modificar");
            areaDePrueba.Modificar(_nombre, _descripcion, _usuario);
            Assert.AreEqual(_nombre, areaDePrueba.Nombre);
            Assert.AreEqual(_descripcion, areaDePrueba.Descripcion);
        }

        [Test]
        public void FallaAlDarDeBajaYaDadoDeBaja()
        {
            var areaDePrueba = new Area(_nombre, _descripcion);
            areaDePrueba.DarDeBaja(_motivoBaja, _usuario);
            Assert.That(() => areaDePrueba.DarDeBaja(_motivoBaja, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }
    }
}