using System;
using System.Text.RegularExpressions;
using Infraestructura.Core.Comun.Dato;
using NUnit.Framework;


namespace Core.Tests
{
    [TestFixture]
    public class ConvencionDefinicionTest
    {

        public class ClasePrueba
        {
            public Id Id { get; set; }
            public DateTime Fecha { get; set; }
            public string Descripcion { get; set; }
            public ClaseHija Hija { get; set; }

        }

        public class ClaseHija
        {
            
        }
        public bool IsLike(string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }

        [Test]
        public void EsParecidoA()
        {
            var entrada = "IdTipoCategoria";
            var aBuscar = "TipoCategoria";

            Assert.IsTrue(IsLike(entrada, aBuscar));
        }

      
    }
}
