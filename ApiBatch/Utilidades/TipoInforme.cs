using Infraestructura.Core.Comun.Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiBatch.Utilidades
{
    public class TipoInforme : Infraestructura.Core.Comun.Dato.Entidad
    {
        protected TipoInforme()
        {
        }

        protected TipoInforme(int id, string nombre)
        {
            Id = new Id(id);
            Nombre = nombre;
        }

        protected TipoInforme(int id, string nombre, string contentType)
        {
            Id = new Id(id);
            Nombre = nombre;
            ContentType = contentType;
        }

        public static TipoInforme ConId(int id)
        {
            switch (id)
            {
                case 5: return BENEFICIARIOS;
                default: return null;
            }
        }

        public override bool Equals(object obj) =>
            obj != null && Id.Equals(((TipoInforme)obj).Id);

        public override string ToString() =>
            Nombre;

        public static TipoInforme BENEFICIARIOS => new TipoInforme(5, "Beneficiarios", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        public virtual string Nombre { get; protected set; }
        public string ContentType { get; private set; }
    }
}