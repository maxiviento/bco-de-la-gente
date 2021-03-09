/*using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Newtonsoft.Json;


namespace SintysWS.Test
{
    [TestClass]
    public class DatosSintysTest
    {
       private readonly SintysServicioWs _sintysServicio;

        public DatosSintysTest()
        {
            _sintysServicio = new SintysServicioWs();
        }

        [TestInitialize]
        public void Init()
        {

            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();

        }

        [TestMethod]
        public void ObtenerPersonaFisica()
        {
            var resultado = _sintysServicio.ObtenerPersonaFisica(
                "23898665");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerPersonaIdentificada()
        {
            var resultado = _sintysServicio.ObtenerPersonaIdentificada("16286084");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));

        }

        [TestMethod]
        public void ObtenerPensionNoContributiba()
        {
            var resultado = _sintysServicio.ObtenerPersonaIdentificada("7856285");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerDomicilio()
        {
            var resultado = _sintysServicio.ObtenerDomicilio("13610856");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerEmpleoFormalConMonto()
        {
            var resultado = _sintysServicio.ObtenerEmpleoFormalConMonto("31474866");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerFallecidos()
        {
            var resultado = _sintysServicio.ObtenerFallecido("27187346");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerJubilacionesYPensionesConMonto()
        {
            var resultado = _sintysServicio.ObtenerJubilacionPension("4297398");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerEmpleoPresunto()
        {
            var resultado = _sintysServicio.ObtenerEmpleoPresunto("7265564");

            Assert.IsTrue(resultado.Count > 0);
            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }

        [TestMethod]
        public void ObtenerDesempleo()
        {
            var resultado = _sintysServicio.ObtenerDesempleos("23926343");

            Trace.WriteLine("== Response: Ok ==");
            Trace.WriteLine(JsonConvert.SerializeObject(resultado, Formatting.Indented));
        }
    }
}*/