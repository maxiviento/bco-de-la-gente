using System;
using System.Collections.Generic;

namespace Core.CiDi.Documentos.Entities.Ciudadano_Digital
{
    public class Respuesta 
    {
        public String Resultado { get; set; }
        public String CodigoError { get; set; }
        public String SesionHash { get; set; }
    }

	public class RespuestaListDoc : Respuesta
    {
        public String ExisteUsuario { get; set; }
        public String Denominacion { get; set; }
        public String Fecha { get; set; }
        public String Tipo { get; set; }
        public List<Documentacion> Documentos { get; set; }

        public RespuestaListDoc()
        {
            Documentos = new List<Documentacion>();
        }
    }
	
    public class RespuestaDoc : Respuesta
    {
        public Documentacion Documentacion { get; set; }

        public RespuestaDoc()
        {
            Documentacion = new Documentacion();
        }
    }

    public class RespuestaDocInsercion : Respuesta
    {
        public Int32 IdDocumento { get; set; }
        public Boolean Eliminar_CDD { get; set; }
    }
}