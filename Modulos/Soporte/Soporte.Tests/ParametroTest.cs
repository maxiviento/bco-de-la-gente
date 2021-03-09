using Infraestructura.Core.Comun.Excepciones;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Soporte.Dominio.Modelo;

namespace Soporte.Tests
{
    [TestFixture]
    public class ParametroTest
    {
        private Randomizer _randomizer;
        private Parametro _parametro;

        [SetUp]
        public void Init()
        {
            _randomizer = new Randomizer();

            _parametro = new Parametro(1, _randomizer.GetString(4, "ABCDEFG"), _randomizer.GetString(4, "hijklmno"));
        }
    }
}
