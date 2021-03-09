using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Configuracion.Tests
{
    [TestFixture]
    public class TipoItemTest
    {
        private string _nombre;
        private string _descripcion;
        private Randomizer _randomizer;

        [SetUp]
        public void Init()
        {
            _nombre = "Nombre del tipo de item de prueba";
            _descripcion = "Esta es la descripción del tipo de item de prueba";
           _randomizer = new Randomizer();
        }

        [Test]
        public void FallaAlRegistrarSinNombre()
        {
            Assert.That(() => new TipoItem(null, _descripcion), Throws.TypeOf<ModeloNoValidoException>());
        }

        [Test]
        public void FallaAlRegistrarSinDescripcion()
        {
            Assert.That(() => new TipoItem(_nombre, null), Throws.TypeOf<ModeloNoValidoException>());
        }
        [Test]
        public void FallaAlRegistrarConNombreMayorALoPermitido()
        {
            var nombreFueraDeRango = _randomizer.GetString(51);
            Assert.That(() => new TipoItem(_randomizer.GetString(51), _descripcion), Throws.TypeOf<ModeloNoValidoException>());
        }
        [Test]
        public void FallaAlRegistrarCoDescripcionMayorALoPermitido()
        {
            var nombreFueraDeRango = _randomizer.GetString(51);
            Assert.That(() => new TipoItem(_nombre, _randomizer.GetString(201)), Throws.TypeOf<ModeloNoValidoException>());
        }
    }
}
