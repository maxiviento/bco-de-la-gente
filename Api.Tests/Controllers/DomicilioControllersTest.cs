using Api.Controllers.GrupoUnico;
using Infraestructura.Core.CiDi.Api;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Api.Tests.Controllers
{
    [TestFixture]
    public class DomicilioControllersTest : GrupoFamiliarControllerBase
    {
        private string _sexo;
        private string _dni;
        private string _pais;
        private int _tipoDomicilio;

        [SetUp]
        public void Init()
        {
            _sexo = "02";
            _dni = "31479672";
            _pais = "ARG";
            _tipoDomicilio = 4;
        }

        [Test]
        public string GetApiConsultaGeneradoJson()
        {
            return CallApiEntidadConsultaStringJson(_sexo, _dni, _pais, _tipoDomicilio,
                ApiDomicilios.ApiConsultaDomicilioGeneradoJson);
        }
    }
}