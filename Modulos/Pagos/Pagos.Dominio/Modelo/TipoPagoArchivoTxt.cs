using System;
using Infraestructura.Core.Comun.Dato;

namespace Pagos.Dominio.Modelo
{
    public class TipoPagoArchivoTxt : Entidad
    {
        public static TipoPagoArchivoTxt PagoNormal => new TipoPagoArchivoTxt(0, "00");
        public static TipoPagoArchivoTxt PagoAguinaldo => new TipoPagoArchivoTxt(1, "01");
        public static TipoPagoArchivoTxt ReemplazoPagoNormal => new TipoPagoArchivoTxt(2, "02");
        public static TipoPagoArchivoTxt ReemplazoPagoAguinaldo => new TipoPagoArchivoTxt(3, "03");
        public static TipoPagoArchivoTxt SentenciaJudicial => new TipoPagoArchivoTxt(4, "04");
        public static TipoPagoArchivoTxt PrimerosPagos => new TipoPagoArchivoTxt(5, "05");
        public static TipoPagoArchivoTxt PagosAdicionalUno => new TipoPagoArchivoTxt(6, "06");
        public static TipoPagoArchivoTxt PagosAdicionalDos => new TipoPagoArchivoTxt(7, "07");
        public static TipoPagoArchivoTxt PagosAdicionalTres => new TipoPagoArchivoTxt(8, "08");
        public static TipoPagoArchivoTxt PagosAdicionalCuatro => new TipoPagoArchivoTxt(9, "09");

        public virtual string Descripcion { get; set; }

        protected TipoPagoArchivoTxt()
        {
        }

        protected TipoPagoArchivoTxt(int id, string descripcion)
        {
            Id = new Id(id);
            Descripcion = descripcion;
        }
        public static TipoPagoArchivoTxt ConId(int id)
        {
            switch (id)
            {
                case 0:
                    return PagoNormal;
                case 1:
                    return PagoAguinaldo;
                case 2:
                    return ReemplazoPagoNormal;
                case 3:
                    return ReemplazoPagoAguinaldo;
                case 4:
                    return SentenciaJudicial;
                case 5:
                    return PrimerosPagos;
                case 6:
                    return PagosAdicionalUno;
                case 7:
                    return PagosAdicionalDos;
                case 8:
                    return PagosAdicionalTres;
                case 9:
                    return PagosAdicionalCuatro;

                default:
                    throw new ArgumentOutOfRangeException(nameof(id),
                        "No existe el tipo de pago para el ID solicitado");
            }
        }
    }
}
