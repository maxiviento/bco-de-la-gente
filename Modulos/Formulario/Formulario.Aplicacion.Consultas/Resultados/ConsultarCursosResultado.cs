namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ConsultarCursosResultado
    {
        public virtual decimal Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual decimal IdTipoCurso { get; set; }
        public virtual string NombreTipoCurso { get; set; }
    }
}