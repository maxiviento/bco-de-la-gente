//using System;
//using System.Collections.Generic;
//using NUnit.Framework;
//using Utilidades.Exportador;

//namespace Exportador.Test
//{
//    [TestFixture]
//    public class ExporterTest
//    {

//        [Test]
//        public void Definition()
//        {
//            var data = StubData.GetStubData();
//            var resultado = Exporter.ToSecuencial<Ejemplo>()
//                 .AddColumn("column1", 0, 10, c => c.Entero)
//                 .AddColumn("column2", 11, 16, '$', u => u.String)
//                 .GenerateAsString(data);



//        }
//    }
//}