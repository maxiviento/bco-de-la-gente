using System;
using NUnit.Framework;
using Utilidades.Importador.Tests.Stubs;

namespace Utilidades.Importador.Tests
{
    [TestFixture]
    public class ImporterTest
    {
        public class Inteceptor : IImporterInterceptor<SecuencialData.Secuencial>
        {
            public bool Pre(int rowIndex, string[] columns)
            {
                return true;
            }

            public bool Pos(int rowIndex, ref SecuencialData.Secuencial t)
            {
                throw new NotImplementedException();
            }

            public int ValidarDevengadoSuaf(int rowIndex, ref SecuencialData.Secuencial t)
            {
                throw new NotImplementedException();
            }

            public bool Pos(int rowIndex, SecuencialData.Secuencial t)
            {
                return rowIndex%2 == 0;
            }
        }

        public class DateTimeConverter : IConverter
        {
            public bool CanConvert(Type type)
            {
                return type == typeof(DateTime);
            }

            public object Convert(Type type, object value)
            {
                return DateTime.Now;
            }
        }

        [Test]
        public void Definition()
        {
            //var exito = new List<SecuencialData.Secuencial>();
            //var error = new List<SecuencialData.Secuencial>();
            //var entry = SecuencialData.GetSecuencialLote();

            //Importer.FromSecuencial<SecuencialData.Secuencial>()
                
            //    .AddColumn(0, 15, u => u.IdResultado)
            //    .AddColumn(16, 26, u => u.Cuil)
            //    .AddColumn(37, 47, u => u.FechaDesdeDateTime)
            //    .AddColumn(58, 84, u => u.InstitutoMedico)
            //    .AddColumn(104, 114, u => u.FechaHastaA)

            //    .AddConverter(new DateTimeConverter())
            //    .SetInterceptor(new Inteceptor())
                
            //    .Generate(entry, out exito, out error);
        }
    }
}