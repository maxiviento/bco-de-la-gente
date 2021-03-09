using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Formulario.Aplicacion.Consultas.Resultados;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using Pagos.Dominio.Modelo;
using Soporte.Dominio.Modelo;

namespace Pagos.Aplicacion.Servicios
{
    public class ArchivoTxtServicio
    {
        private readonly IPagosRepositorio _pagosRepositorio;

        public ArchivoTxtServicio(IPagosRepositorio pagosRepositorio)
        {
            _pagosRepositorio = pagosRepositorio;
        }

        public string GenerarTextoParaTxt(decimal idLote, int idTipoPago)
        {
            StringBuilder archivo = new StringBuilder();

            var prestamos = _pagosRepositorio.ObtenerDatosLote(idLote);

            var datosLote = _pagosRepositorio.ObtenerCabeceraDetalleLote(idLote);

            var idFormularioParaSoporte = ObtenerIdFormularioParaFecha(prestamos);

            var fechasLote = _pagosRepositorio.ObtenerFechaInicioFormulario(idFormularioParaSoporte);

            var convenio = ObtenerConvenioLote(idLote);

            archivo.Append(GeneraCabecera(datosLote, fechasLote, convenio));//Se agrega la cabecera al archivo con el salto de linea
            archivo.Append(GeneraDetalle(prestamos, convenio, idTipoPago));//Se agrega el detalle completo

            return archivo.ToString();
        }

        #region Generacion de Cabecera

        private string GeneraCabecera(DetalleLoteResultado datosLote, DatosFechaFormResultado fechasLote, string convenio)
        {
            StringBuilder cabecera = new StringBuilder();

            cabecera.Append("00");//identificacion
            cabecera.Append(fechasLote.FecInicioPago.ToString("MMyyyy"));//fecha inicio de pago
            cabecera.Append(CalcularRegistrosCabecera(datosLote.CantidadBeneficiarios));//cantidad de formularios que componen el lote
            cabecera.Append(CalcularMontoCabecera(datosLote.MontoLote));//monto total del lote
            cabecera.Append(CalcularRegistrosCompCabecera());//registros complementarios, sin informacion
            cabecera.Append(CalcularMontoCompCabecera());//monto complementario, sin informacion
            cabecera.Append(FechaInicioPagoCabecera(fechasLote.FecInicioPago));//fecha inicio cualquier formulario
            cabecera.Append(FechaFinPagoCabecera(fechasLote.FecFinPago));//fecha fin cualquier formulario
            cabecera.Append("020");//codigo banco
            cabecera.Append(convenio);//convenio
            cabecera.Append(UsoFuturo(953));//953 espacios blancos para uso futuro
            cabecera.Append("\n");//Salto de linea para comenzar el detalle

            return cabecera.ToString();
        }

        private string ObtenerConvenioLote(decimal idLote)
        {
            var convenio = _pagosRepositorio.ObtenerCabeceraDetalleLote(idLote).CodigoConvenio;
            
            return convenio.PadLeft(4, '0');
        }

        private string CalcularRegistrosCabecera(decimal? cantidad)
        {
            return cantidad.ToString().PadLeft(8, '0');
        }

        private string CalcularMontoCabecera(decimal? monto)
        {
            return monto.GetValueOrDefault().ToString().PadLeft(10, '0').PadRight(12, '0');
        }

        private string CalcularRegistrosCompCabecera()
        {
            string registros = "";

            return registros.PadLeft(8, '0');
        }

        private string CalcularMontoCompCabecera()
        {
            string monto = "";

            return monto.PadLeft(12, '0');
        }

        private string FechaInicioPagoCabecera(DateTime fechaLote)
        {
            return fechaLote.ToString("ddMMyyyy").PadLeft(8, '0');
        }
        private string FechaFinPagoCabecera(DateTime fechaLote)
        {
            return fechaLote.ToString("ddMMyyyy").PadLeft(8, '0');
        }
     
        #endregion

        #region Generacion de Detalle
        private string GeneraDetalle(IList<BandejaPagosResultado> prestamos, string convenio, int idTipoPago)
        {
            StringBuilder detalle = new StringBuilder();
            var listadoIds = prestamos.Select(x => x.Id).Distinct().ToList();

            foreach (var prestamosId in listadoIds)
            {
                var formularios = _pagosRepositorio.ObtenerFormulariosPrestamo(prestamosId);

                var cantidadFormulariosApoderados = formularios.Count(formulario => formulario.IdEstado == 5 && (formulario.TipoApoderado == (int) TipoApoderadoEnum.PertenecePeroNoApoderado || formulario.TipoApoderado == (int) TipoApoderadoEnum.EsApoderado));
                var tipoPago = TipoPagoArchivoTxt.ConId(idTipoPago);
                
                foreach (var formulario in formularios)
                {
                    if (formulario.IdEstado != 5)
                    {
                        continue; //si el formulario esta rechazado se salta la linea en el registro
                    }

                    if (formulario.TipoApoderado == (int) TipoApoderadoEnum.PertenecePeroNoApoderado)
                    {
                        continue;
                    }

                    detalle.Append("00"); //Grupo de pago
                    detalle.Append(FormatoFechaDetalle(formulario.FechaPago)); //Fecha de inicio de pago asociada al formulario
                    detalle.Append(FormatoFechaDetalle(formulario.FechaFinPago)); //Fecha de fin de pago asociada al formulario
                    detalle.Append(formulario.Cuil.PadLeft(11, '0')); //cuil beneficiario
                    detalle.Append("00"); //ex caja
                    detalle.Append("0"); //tipo beneficiario
                    detalle.Append(NumeroFormularioDetalle(formulario.NroFormulario)); //nro solicitud
                    detalle.Append("0"); //coparticipe
                    detalle.Append("0"); //digito verificador
                    detalle.Append("020"); //banco pagador
                    detalle.Append(SucursalBancoDetalle(formulario.IdFormulario)); //sucursal banco
                    detalle.Append(NombreDetalle(formulario.Beneficiario)); //nombre completo
                    detalle.Append(ObtenerTipoDoc(formulario.TipoDocumento)); //tipo documento solicitante
                    detalle.Append(NumeroDocumentoDetalle(formulario.NroDocumento)); //numero documento solicitante
                    detalle.Append("04"); //codigo provincia
                    detalle.Append(CuilApoderadoDetalle()); //sin informacion
                    detalle.Append(NombreApoderadoDetalle()); //sin informacion
                    detalle.Append(TipoDocumentoApoderadoDetalle()); //sin informacion
                    detalle.Append(DocumentoApoderadoDetalle()); //sin informacion
                    detalle.Append("04"); //codigo provincia
                    detalle.Append("001"); //codigo concepto 1
                    detalle.Append("000"); //subcodigo concepto 1
                    detalle.Append(ImporteConceptoDetalle(formulario.MontoFormulario, formulario.TipoApoderado, cantidadFormulariosApoderados)); //importe concepto 1
                    detalle.Append(ConceptosDetalle()); //cadena de conceptos
                    detalle.Append(ImportesDetalle(0,formulario.TipoApoderado,0)); //importe haberes
                    detalle.Append(ImportesDetalle(0, formulario.TipoApoderado, 0)); //importe descuento
                    detalle.Append(ImportesDetalle(0, formulario.TipoApoderado, 0)); //importe liquido
                    detalle.Append(CalcularImporteMayor(formulario.MontoFormulario)); //importe mayor
                    detalle.Append(PeriodoLiquidacionDetalle(formulario.FechaPago)); //periodo liquidacion
                    detalle.Append(tipoPago.Descripcion + "00"); //tipo pago/forma/tipo cuenta
                    detalle.Append(NumeroCuentaDetalle()); //numero de cuenta, sin informacion
                    detalle.Append(FechasProximasDetalle()); //fechas proximas de pago
                    detalle.Append(LeyendasDetalle()); //espacios para las leyendas
                    detalle.Append(DatosCompletaBancoDetalle()); //espacios que completa el banco
                    detalle.Append(ComisionDetalle()); //comision
                    detalle.Append(AfipDetalle()); //datos de afip y ur
                    detalle.Append(ImportesDetalle(formulario.MontoFormulario, formulario.TipoApoderado, cantidadFormulariosApoderados)); //importe neto a cobrar
                    detalle.Append(convenio); //convenio
                    detalle.Append(UsoFuturo(38)); //38 espacios en blanco para usar en un futuro
                    detalle.Append("\n"); //Salto de linea para comenzar el detalle
                }
            }

            return detalle.ToString();
        }

        private string ObtenerTipoDoc(string tipoDoc)
        {
            if (tipoDoc.Length != 1)
            {
                return "0";
            }
            return tipoDoc;
        }

        private string CalcularImporteMayor(decimal? monto)
        {
            if (monto.GetValueOrDefault() > 9999)
            {
                return "1";
            }
            return "0";
        }

        private string ObtenerConvenio(string convenio)
        {
            return convenio.PadLeft(4, '0');
        }

        private string FormatoFechaDetalle(DateTime fecha)
        {
            return fecha.ToString("ddMMyyyy").PadLeft(8, '0');
        }

        private DatosFechaFormResultado ObtenerFechaFormulario(int idFormulario)
        {
            return _pagosRepositorio.ObtenerFechaInicioFormulario(idFormulario);
        }
        
        private string NumeroFormularioDetalle(string numero)
        {
            return numero.PadLeft(7, '0');
        }

        private string SucursalBancoDetalle(int idFormulario)
        {
            var formularios = _pagosRepositorio.ConsultaDatosFormulario(idFormulario);

            var sucursalFormulario = "";

            foreach (var formulario in formularios)
            {
                if (formulario.IdFormulario.Valor == idFormulario)
                {
                    sucursalFormulario = formulario.IdSucursal.GetValueOrDefault().ToString();
                }
            }

            return sucursalFormulario.PadLeft(3, '0');
        }

        private string NombreDetalle(string nombreCompleto)
        {
            var nombre = nombreCompleto.Split(',');
            var union = nombre[0] + nombre[1];

            if (union.Length > 27)
            {
                return union.Substring(0, 27);
            }

            return union.PadRight(27, ' ');
        }

        private string NumeroDocumentoDetalle(string nroDoc)
        {
            return nroDoc.PadLeft(8, '0');
        }

        private string CuilApoderadoDetalle()
        {
            var cuil = "";
            return cuil.PadLeft(11, '0');
        }

        private string NombreApoderadoDetalle()
        {
            var nombreCompleto = "";
            return nombreCompleto.PadRight(27, ' ');
        }

        private string TipoDocumentoApoderadoDetalle()
        {
            var tipoDoc = "0";
            return tipoDoc;
        }

        private string DocumentoApoderadoDetalle()
        {
            var doc = "";
            return doc.PadLeft(8, '0');
        }

        private string ImporteConceptoDetalle(decimal? importe, int tipoApoderado, int cantidadFormularios)
        {
            if (tipoApoderado == (int) TipoApoderadoEnum.EsApoderado)
            {
                importe *= cantidadFormularios;
            }
            var monto = importe.GetValueOrDefault().ToString().PadLeft(8, '0');
            return monto.PadRight(10, '0');
        }

        private string ConceptosDetalle()
        {
            var conceptos = "";
            return conceptos.PadLeft(384, '0');
        }

        private string ImportesDetalle(decimal? importe, int tipoApoderado, int cantidadFormularios)
        {
            if (tipoApoderado == (int)TipoApoderadoEnum.EsApoderado)
            {
                importe *= cantidadFormularios;
            }
            var monto = importe.GetValueOrDefault().ToString().PadLeft(10, '0');
            return monto.PadRight(12, '0');
        }

        private string PeriodoLiquidacionDetalle(DateTime fecha)
        {
            return fecha.ToString("MMyy").PadLeft(4,'0');
        }

        private string NumeroCuentaDetalle()
        {
            var numeroCuenta = "";
            return numeroCuenta.PadLeft(20, ' ');
        }

        private string FechasProximasDetalle()
        {
            var fechas = "";
            return fechas.PadLeft(16, '0');
        }

        private string LeyendasDetalle()
        {
            var numeroCuenta = "";
            return numeroCuenta.PadLeft(304, ' ');
        }

        private string DatosCompletaBancoDetalle()
        {
            var banco = "1";
            return banco.PadRight(27, '0');
        }

        private string ComisionDetalle()
        {
            return " ";
        }

        private string AfipDetalle()
        {
            var afip = "";
            return afip.PadLeft(23, '0');
        }

        private AgruparFormulario ObtenerDatosFormulario(int idFormulario)
        {
            var idAgrupamiento = _pagosRepositorio.ObtenerAgrupId(idFormulario);

            var listaForms = _pagosRepositorio.ObtenerListaForms((int)idAgrupamiento);

            foreach (var formulario in listaForms)
            {
                if (formulario.IdFormulario.Valor == idFormulario)
                {
                    return formulario;
                }
            }
            return null;
        }


        #endregion

        private string UsoFuturo(int espacios)
        {
            string usoFuturo = "";

            return usoFuturo.PadLeft(espacios, ' ');
        }

        private int ObtenerIdFormularioParaFecha(IEnumerable<BandejaPagosResultado> prestamos)
        {
            var idFormulario = 0;

            foreach (var prestamo in prestamos)
            {
                var formularios = _pagosRepositorio.ObtenerFormulariosPrestamo(prestamo.Id);

                foreach (var formulario in formularios)
                {
                    if (formulario.IdEstado != 5) continue;
                    idFormulario = formulario.IdFormulario;
                    break;
                }
            }
            return idFormulario;
        }

        public bool VerificarEstadoFormulario(decimal idLote)
        {
            var prestamos = _pagosRepositorio.ObtenerDatosLote(idLote);

            return prestamos.SelectMany(prestamo => _pagosRepositorio.ObtenerFormulariosPrestamo(prestamo.Id)).Any(formulario => formulario.IdEstado == 5);
        }
    }
}