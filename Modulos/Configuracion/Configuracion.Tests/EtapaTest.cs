using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Configuracion.Tests
{
    public class EtapaTest
    {
        private string _nombre;
        private string _descripcion;
        private Usuario _usuario;
        private MotivoBaja _motivoBaja;
        private Randomizer _randomizer;

        [SetUp]
        public void Init()
        {
            _nombre = "Nombre de la etapa de prueba";
            _descripcion = "Esta es la descripción de la etapa de prueba";
            _usuario = new Usuario { Id = new Id(1), Cuil = "11111111111" };
            _motivoBaja = new MotivoBaja { Id = new Id(1), Descripcion = "Motivo prueba" };
            _randomizer = new Randomizer();
        }

        [Test]
        public void FallaAlRegistrarSinNombre()
        {
            Assert.That(() => new Etapa(_nombre, null), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarSinDescripcion()
        {
            Assert.That(() => new Etapa(null, _descripcion), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarConNombreMayorALoPermitido()
        {
            var nombreFueraDeRango = _randomizer.GetString(101);
            Assert.That(() => new Etapa(nombreFueraDeRango, _descripcion), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarConDescripcionMayorALoPermitido()
        {
            var descripcionFueraDeRango = _randomizer.GetString(201);
            Assert.That(() => new Etapa(_nombre, descripcionFueraDeRango), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void RegistraCorrectamente()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            Assert.AreEqual(_nombre, etapaDePrueba.Nombre);
            Assert.AreEqual(_descripcion, etapaDePrueba.Descripcion);
        }

        [Test]
        public void FallaAlModificarSinNombre()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            Assert.That(() => etapaDePrueba.Modificar(null, _descripcion, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarSinDescripcion()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            Assert.That(() => etapaDePrueba.Modificar(_nombre, null, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarConNombreMayorALoPermitido()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            Assert.That(() => etapaDePrueba.Modificar(_randomizer.GetString(101), _descripcion, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarConDescripcionMayorALoPermitido()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            Assert.That(() => etapaDePrueba.Modificar(_nombre, _randomizer.GetString(201), _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlModificarDadaDeBaja()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            etapaDePrueba.DarDeBaja(_motivoBaja, _usuario);
            Assert.That(() => etapaDePrueba.Modificar(_nombre, _descripcion, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void ModificaCorrectamente()
        {
            var etapaDePrueba = new Etapa("Nombre sin modificar", "Descripcion sin modificar");
            etapaDePrueba.Modificar(_nombre, _descripcion, _usuario);
            Assert.AreEqual(_nombre, etapaDePrueba.Nombre);
            Assert.AreEqual(_descripcion, etapaDePrueba.Descripcion);
        }

        [Test]
        public void FallaAlDarDeBajaYaDadoDeBaja()
        {
            var etapaDePrueba = new Etapa(_nombre, _descripcion);
            etapaDePrueba.DarDeBaja(_motivoBaja, _usuario);
            Assert.That(() => etapaDePrueba.DarDeBaja(_motivoBaja, _usuario), Throws.TypeOf<ModeloNoValidoException>());
        }
    }
}

