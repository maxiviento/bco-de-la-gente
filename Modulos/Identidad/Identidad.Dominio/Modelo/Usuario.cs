using System;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Identidad.Dominio.Modelo
{
    public class Usuario : Entidad
    {
        public Usuario() { }

        public Usuario(string cuil)
        {
            if (string.IsNullOrEmpty(cuil))
                throw new ModeloNoValidoException("El CUIL es requerido");
            if (cuil.Length > 11)
                throw new ModeloNoValidoException("El cuil no puede tener más de 11 caracteres");

            this.Cuil = cuil;
            this.FechaAlta = DateTime.Today;
        }

        public void Identificar(string nombre, string apellido, string email)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Email = email;
        }

        public void AsignarPerfil(Perfil perfil, Usuario usuarioModificacion)
        {
            if (perfil == null)
                throw new ModeloNoValidoException("Se debe seleccionar un perfil");

            if (usuarioModificacion == null)
                throw new ModeloNoValidoException("El usuario no puede ser nulo");

            this.Perfil = perfil;
            this.UsuarioModificacion = usuarioModificacion;
        }

        public void DarDeBaja(Usuario usuarioModificacion, MotivoDeBaja motivoBaja)
        {
            if (motivoBaja == null)
                throw new ModeloNoValidoException("Se debe seleccionar motivo");

            if (usuarioModificacion == null)
                throw new ModeloNoValidoException("El usuario no puede ser nulo");

            this.Perfil = null;
            this.UsuarioModificacion = usuarioModificacion;
            this.MotivoBaja = motivoBaja;
            this.FechaBaja = DateTime.Today;
        }

        public virtual string Cuil { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual string Email { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual MotivoDeBaja MotivoBaja { get; set; }
        public virtual Usuario UsuarioModificacion { get; set; }
    }

}