namespace Formulario.Dominio.Modelo
{
    public class Domicilio
    {
        protected Domicilio()
        {
        }

        public Domicilio(string calle,
            int nroCalle,
            int nroTorre,
            int nroPiso,
            int manzana,
            string nroDpto,
            string casa,
            int idLocalidad,
            int idDepartamento,
            string barrio
        )
        {
            Calle = calle;
            NroCalle = nroCalle;
            NroTorre = nroTorre;
            NroPiso = nroPiso;
            Manzana = manzana;
            NroDpto = nroDpto;
            Casa = casa;
            IdLocalidad = idLocalidad;
            IdDepartamento = idDepartamento;
            Barrio = barrio;
        }

        public virtual string Calle { get; protected set; }
        public virtual int NroCalle { get; protected set; }
        public virtual int NroTorre { get; protected set; }
        public virtual int NroPiso { get; protected set; }
        public virtual int Manzana { get; protected set; }
        public virtual string NroDpto { get; protected set; }
        public virtual string Casa { get; protected set; }
        public virtual int IdLocalidad { get; protected set; }
        public virtual int IdDepartamento { get; protected set; }
        public virtual string Barrio { get; protected set; }
    }
}