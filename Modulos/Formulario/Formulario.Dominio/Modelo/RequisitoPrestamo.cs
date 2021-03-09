using System;
using System.Collections.Generic;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class RequisitoPrestamo : Entidad
    {
        public RequisitoPrestamo()
        {
        }

        public RequisitoPrestamo(
            Item item,
            Area area,
            Etapa etapa,
            int nroOrden)
        {
            Area = area;
            Item = item;
            Etapa = etapa;
            NroOrden = nroOrden;
        }

        public RequisitoPrestamo(Id id)
        {
            Id = id;
        }

        public virtual DateTime FechaBaja { get; protected set; }
        public virtual DateTime FechaUltimaModificacion { get; protected set; }
        public virtual IList<Item> Items { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual Usuario UsuarioUltimaModificacion { get; protected set; }
        public virtual TipoItem TipoItem { get; protected set; }
        public virtual Area Area { get; protected set; }
        public virtual Item Item { get; protected set; }
        public virtual Etapa Etapa { get; protected set; }
        public virtual int NroOrden { get; protected set; }

        public void AgregarItems(IList<Item> items)
        {
            Items = items;
        }
    }
}