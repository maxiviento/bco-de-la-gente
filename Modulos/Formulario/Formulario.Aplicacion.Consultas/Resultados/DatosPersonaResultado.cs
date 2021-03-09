namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DatosPersonaResultado
    {
        public string SexoId { get; set; }
        public string CodigoPais { get; set; }
        public string NroDocumento { get; set; }
        public int? IdNumero { get; set; }
        public string CodigoArea { get; set; }
        public string Telefono { get; set; }
        public string CodigoAreaCelular { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int? IdGarante { get; set; }
        public bool esIgualQueOtraPersona(DatosPersonaResultado persona) {
            return this.obtenerUniqueHash().CompareTo(persona.obtenerUniqueHash()) == 0;
        }

        /*
        Este metodo tiene como objetivo armar una cadena unica entre los valores de IdSexo, NroDocumento, CodigoPais y Numero que permiten comparar contra otra persona
        Esto es debido a que una variacion en cualquiera de estos valores puede signifcar ser una persona distinta
        */
        public string obtenerUniqueHash() {
            return NroDocumento + SexoId.ToString() + CodigoPais + IdNumero.ToString();
        }

        public void SetDatosContacto(DatosContactoResultado datosContacto)
        {
            CodigoArea = datosContacto.CodigoArea;
            Telefono = datosContacto.Telefono;
            CodigoAreaCelular = datosContacto.CodigoAreaCelular;
            Celular = datosContacto.Celular;
            Email = datosContacto.Mail;
        }
    }
}
