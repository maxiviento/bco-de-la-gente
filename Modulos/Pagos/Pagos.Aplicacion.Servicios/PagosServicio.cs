using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ApiBatch.Base.QueueManager;
using ApiBatch.Operations.QueueManager;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using GrupoUnico.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Reportes;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Pagos.Aplicacion.Comandos;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Aplicacion.Servicios.Importacion;
using Pagos.Dominio.IRepositorio;
using Pagos.Dominio.Modelo;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Soporte.Aplicacion.Servicios;
using Utilidades.Importador;
using Convenio = Pagos.Aplicacion.Consultas.Resultados.Convenio;

namespace Pagos.Aplicacion.Servicios
{
    public class PagosServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IPagosRepositorio _pagosRepositorio;
        private readonly IPrestamoRepositorio _prestamoRepositorio;
        private readonly ArchivoTxtServicio _archivoTxtServicio;
        private readonly GrupoUnicoServicio _grupoUnicoServicio;

        public PagosServicio(
            ISesionUsuario sesionUsuario,
            IPagosRepositorio pagosRepositorio,
            IPrestamoRepositorio prestamoRepositorio,
            ArchivoTxtServicio archivoTxtServicio,
            GrupoUnicoServicio grupoUnicoServicio)
        {
            _sesionUsuario = sesionUsuario;
            _pagosRepositorio = pagosRepositorio;
            _prestamoRepositorio = prestamoRepositorio;
            _archivoTxtServicio = archivoTxtServicio;
            _grupoUnicoServicio = grupoUnicoServicio;
        }


        #region Bandeja de pagos

        public Resultado<BandejaPagosResultado> ConsultaBandeja(BandejaPagosConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaPagosConsulta {NumeroPagina = 0};
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            if (consulta.NroFormulario != null)
            {
                consulta.FechaInicioTramite = default(DateTime);
                consulta.FechaFinTramite = default(DateTime);
                consulta.IdsLineas = new List<int>();
                consulta.IdLocalidad = default(int?);
                consulta.IdLugarOrigen = default(int?);
                consulta.IdOrigen = default(int?);
                consulta.NroPrestamoChecklist = default(int?);
            }
            var resultado = _pagosRepositorio.ObtenerPagosBandeja(consulta);
            return resultado;
        }

        public Resultado<BandejaPagosResultado> ConsultaBandejaCompleta(BandejaPagosConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaPagosConsulta { NumeroPagina = 0 };
            }

            if (consulta.NroFormulario != null)
            {
                consulta.FechaInicioTramite = default(DateTime);
                consulta.FechaFinTramite = default(DateTime);
                consulta.IdsLineas = new List<int>();
                consulta.IdLocalidad = default(int?);
                consulta.IdLugarOrigen = default(int?);
                consulta.IdOrigen = default(int?);
                consulta.NroPrestamoChecklist = default(int?);
            }
            var resultado = _pagosRepositorio.ObtenerPagosBandejaCompleta(consulta);
            return resultado;
        }

        public List<MontoDisponibleSimularLoteResultado> ConsultarMontosDisponibles(MontosDisponibleConsulta consulta)
        {
            return _pagosRepositorio.ConsultarMontosDisponibles(consulta.Monto).ToList();
        }
        
        public TasasLoteResultado ObtenerIvaComision()
        {
            return new TasasLoteResultado
            {
                Iva = decimal.Parse(ParametrosSingleton.Instance.GetValue("16")),
                Comision = decimal.Parse(ParametrosSingleton.Instance.GetValue("17"))
            };
        }

        public string ConfirmarLote(ConfirmarLoteComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            var idsAgrupamientos = string.Join(",", comando.IdAgrupamientosSeleccionados);
            var idsPrestamos = string.Join(",", comando.IdPrestamosSeleccionados);
            foreach (var idPrestamos in comando.IdPrestamosSeleccionados)
            {
                var estadoAgrupamiento = decimal.Parse(_prestamoRepositorio.ConsultarEstadoPrestamo(idPrestamos).IdEstado);
                if (estadoAgrupamiento == EstadoPrestamo.APagarConSuaf.Id.Valor) continue;
                throw new ErrorTecnicoException(
                    $"No se pudo generar el lote, debido a que uno o mas préstamos ya pertenecen a un lote.");
                
            }
            _pagosRepositorio.RegistrarFechaPagoFormularios(idsAgrupamientos, comando.FechaPago, comando.FechaFinPago, comando.Modalidad,
                comando.Elemento, _sesionUsuario.Usuario.Id.Valor);

            return _pagosRepositorio.ConfirmarLote(comando.IdPrestamosSeleccionados, comando.NombreLote,
                comando.IdMontoDisponible, comando.Monto, comando.Comision, comando.Iva, usuario.Id.Valor, comando.Convenio, comando.MesesGracia, comando.IdTipoLote);
        }
        public string AgregarPrestamoLote(AgregarPrestamoComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
  

            return _pagosRepositorio.AgregarPrestamoLote(comando.IdLote, comando.IdsPrestamo, comando.IdMonto, comando.Monto, usuario.Id.Valor);
        }

        public string ConfirmarLoteAdenda(ConfirmarLoteComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            _pagosRepositorio.RegistrarFechaPagoFormulariosAdenda(comando.IdLoteSuaf, comando.FechaPago, comando.FechaFinPago, comando.Modalidad,
                comando.Elemento, _sesionUsuario.Usuario.Id.Valor);

            return _pagosRepositorio.ConfirmarLoteAdenda(comando.IdLoteSuaf, comando.NombreLote,
                comando.IdMontoDisponible, comando.Monto, 0, 0, usuario.Id.Valor, comando.Convenio, comando.MesesGracia, comando.IdTipoLote);
        }

        public Resultado<BandejaAdendaResultado> ConsultaBandejaAdenda(BandejaAdendaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaAdendaConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            if (consulta.NroFormulario != null)
            {
                consulta.IdsLineas = new List<int>();
                consulta.IdOrigen = default(int?);
                consulta.NroPrestamoChecklist = default(int?);
            }
            return _pagosRepositorio.ObtenerBandejaAdenda(consulta);
        }

        public decimal SeleccionarTodosParaAdenda(BandejaAdendaConsulta consulta, string idUsuario)
        {
            var listadoPrestamosCompleto = _pagosRepositorio.ObtenerPrestamosCompletosParaAdenda(consulta);

            decimal nroDetalle = -1;

            if (consulta.SeleccionarTodos)
            {
                //hay que agregar todos los prestamos
                foreach (var prestamo in listadoPrestamosCompleto)
                {
                    var detalle = new DetallesAdenda
                    {
                        IdLote = consulta.idLote,
                        NroDetalle = consulta.nroDetalle,
                        NroPrestamoChecklist = prestamo.NroPrestamo
                    };
                    nroDetalle = _pagosRepositorio.AgregarPrestamoAdenda(detalle, idUsuario);
                    consulta.nroDetalle = nroDetalle;
                }
            }
            else
            {
                //hay que eliminar todos los prestamos
                foreach (var prestamo in listadoPrestamosCompleto)
                {
                    var detalle = new DetallesAdenda
                    {
                        IdLote = consulta.idLote,
                        NroDetalle = consulta.nroDetalle,
                        NroPrestamoChecklist = prestamo.NroPrestamo
                    };
                    nroDetalle = _pagosRepositorio.QuitarPrestamoAdenda(detalle);
                }
            }
            return nroDetalle;
        }

        public decimal ModificarDetalleAdenda(DetallesAdenda detalle, string idUsuario)
        {
            decimal nroDetalle = -1;

            if (detalle.Agrega)
            {
                nroDetalle = _pagosRepositorio.AgregarPrestamoAdenda(detalle, idUsuario);
            }
            else
            {
                nroDetalle = _pagosRepositorio.QuitarPrestamoAdenda(detalle);
            }

            return nroDetalle;
        }

        public Resultado<FormulariosSeleccionadosAdendaResultado> ObtenerFormulariosParaAdenda(FormulariosSeleccionadosAdendaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new FormulariosSeleccionadosAdendaConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            
            return _pagosRepositorio.ObtenerFormulariosParaAdenda(consulta);
        }

        public IList<FormularioPrestamoResultado> ObtenerFormulariosPorPrestamo(int idPrestamo)
        {
            var listaFormularios = _pagosRepositorio.ObtenerFormulariosPrestamo(idPrestamo);
            foreach (var formulario in listaFormularios)
            {
                formulario.Estado = EstadoFormulario.ConId(formulario.IdEstado).Descripcion;
                if (formulario.IdEstado == 4) formulario.MontoFormulario = 0;
            }

            return listaFormularios;
        }

        public decimal ObtenerTotalLote(decimal idLoteSuaf)
        {
           return _pagosRepositorio.ObtenerTotalLote(idLoteSuaf);
        }

        public bool HabilitadoAdenda(decimal idLoteSuaf)
        {
            return _pagosRepositorio.HabilitadoAdenda(idLoteSuaf);
        }

        public FormularioFechaPagoResultado ObtenerFechasPago(decimal idLote)
        {
            var fechasPagoResultado = _pagosRepositorio.ObtenerFechasFormularioLote(idLote)[0];
            // Obtengo el convenio que tiene asociado el lote
            fechasPagoResultado.ConvenioPago = _pagosRepositorio.ObtenerCabeceraDetalleLote(idLote).IdConvenio;

            return fechasPagoResultado;
        }

        public bool ValidarLotePago(decimal idLote)
        {
            return _pagosRepositorio.ValidarLotePago(idLote); 
        }

        #endregion

        #region Bandeja lotes
        public Resultado<BandejaLotesResultado> ConsultaBandejaLotes(BandejaLotesConsulta consulta)
        {
            if (consulta == null)            
                consulta = new BandejaLotesConsulta { NumeroPagina = 0};
            
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            var aux = _pagosRepositorio.ObtenerBandejaLotes(consulta);
            return aux;
        }

        public Resultado<FormularioChequeGrillaResultado> ConsultaBandejaCheques(FormularioGrillaChequeConsulta consulta)
        {
            if (consulta == null)
                consulta = new FormularioGrillaChequeConsulta { NumeroPagina = 0 };

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            var aux = _pagosRepositorio.ObtenerBandejaCheques(consulta, _sesionUsuario.Usuario.Id.Valor);
            return aux;
        }

        public string ConsultaTotalizadorCheques(FormularioGrillaChequeConsulta consulta)
        {
            if (consulta == null)
                consulta = new FormularioGrillaChequeConsulta { NumeroPagina = 0 };

            return _pagosRepositorio.ConsultaTotalizadorCheques(consulta, _sesionUsuario.Usuario.Id.Valor);
        }

        public bool ActualizarDatosCheque(CargaDatosChequeComando comando)
        {
            return _pagosRepositorio.ActualizarDatosCheque(comando.IdFormulario, comando.NroCheque, comando.FechaVencimientoCheque,
                _sesionUsuario.Usuario.Id.Valor);
        }
        public string ConsultaTotalizadorLotes(BandejaLotesConsulta consulta)
        {
            if (consulta == null)
                consulta = new BandejaLotesConsulta { NumeroPagina = 0 };

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();
            return _pagosRepositorio.ObtenerTotalizadorLotes(consulta);
        }

        public string LiberarLote(decimal idLote)
        {
            return _pagosRepositorio.LiberarLote(idLote, _sesionUsuario.Usuario.Id.Valor);
        }

        public PermiteLiberarLoteResultado PermiteLiberarLote(decimal idLote)
        {
            return _pagosRepositorio.PermiteLiberarLote(idLote);
        }

        public DetalleLoteResultado ObtenerDetalleLote(decimal idLote)
        {
            var cabecera = _pagosRepositorio.ObtenerCabeceraDetalleLote(idLote);
            cabecera.FechaCreacionString = cabecera.FechaCreacion.ToString("dd/MM/yyyy");
            return cabecera;
        }

        public Resultado<BandejaPagosResultado> ObtenerPrestamosDetalleLote(PrestamosDetalleLoteConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new PrestamosDetalleLoteConsulta {NumeroPagina = 0};
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _pagosRepositorio.ObtenerPrestamosDetalleLote(consulta);
        }

        public IList<HistorialLoteResultado> ObtenerHistorialDetalleLote(decimal idLote)
        {
            return _pagosRepositorio.ObtenerHistorialDetalleLote(idLote);
        }

        public string DesagruparLote(DesagruparLoteComando comando)
        {
            return _pagosRepositorio.DesagruparLote(comando.IdLote, comando.IdPrestamosDesagrupados,
                _sesionUsuario.Usuario.Id.Valor);
        }

        public ReporteExcelBancoResultado ObtenerExcelBanco(decimal idLote)
        {
            var reportData = new ReporteBuilder("ReporteBancoExcel.rdlc")
                .AgregarDataSource("DataSet1", _pagosRepositorio.ObtenerDatosExcelBanco(idLote, _sesionUsuario.Usuario.Id.Valor))
                .SeleccionarFormato("Excel")
                .Generar();

            return new ReporteExcelBancoResultado
            {
                Blob = reportData.Content,
                FileName = "reporteBanco " + DateTime.Now.ToString("MM-dd-yyyy") + ".xls"
            };
        }

        public decimal RegistrarGeneracionChequeLote(decimal idLote)
        {
            return _pagosRepositorio.RegistrarChequeLote(idLote, _sesionUsuario.Usuario.Id.Valor);
        }

        public ReporteExcelBancoResultado ValidarProvidenciaLote(decimal idLote)
        {
            var listadoFormularios = _pagosRepositorio.ValidarProvidenciaLote(idLote);
            if (listadoFormularios.Length > 0)
            {
                StringBuilder archivo = new StringBuilder();

                archivo.Append("Los formularios que no poseen providencia son: ");
                archivo.Append(listadoFormularios);

                return new ReporteExcelBancoResultado
                {
                    Blob = Encoding.UTF8.GetBytes(archivo.ToString()),
                    FileName = "Formularios sin providencia.txt"
                };
            }
            return null;
        }

        public ReporteExcelBancoResultado GenerarArchivoTxt(decimal idLote, int idTipoPago)
        {
            var archivo = _archivoTxtServicio.GenerarTextoParaTxt(idLote, idTipoPago);

            return new ReporteExcelBancoResultado
            {
                Blob = Encoding.UTF8.GetBytes(archivo),
                FileName = "ArchivoText " + DateTime.Now.ToString("MM-dd-yyyy") + ".txt"
            };
        }
        public bool VerificarEstadoFormulario(decimal idLote) => _archivoTxtServicio.VerificarEstadoFormulario(idLote);

        public ReporteExcelBancoResultado ObtenerNotaPago(CrearNotaBancoConsulta consulta)
        {
            var datos = _pagosRepositorio.ObtenerDatosNota(consulta.IdLote);

            var fechaActual = DateTime.Today;
            var mes = new CultureInfo("es-ES", false).DateTimeFormat.GetMonthName(fechaActual.Month);
            var fecha = fechaActual.Day + " de " + mes + " de " + fechaActual.Year;

            datos.FechaNota = fecha;
            datos.MontoLetra = ConvertidorNumeroLetras.enletras(datos.MontoNumero.ToString());
            datos.Nombre = consulta.Nombre;
            datos.Cc = consulta.Cc;

            var reportData = new ReporteBuilder("ReporteNotaPago.rdlc")
                .AgregarDataSource("DataSet1", datos)
                .Generar();
            var reporte = new ReporteBuilder("ReporteNotaPago.rdlc")
            .AgregarDataSource("DataSet1", datos)
            .Generar();

            Stream streamReporte = new MemoryStream(reporte.Content);
            PdfDocument inputDocument = PdfReader.Open(streamReporte, PdfDocumentOpenMode.Import);
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage pdfPage = pdfDocument.AddPage(inputDocument.Pages[0]);
            XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
            XTextFormatter tf = new XTextFormatter(gfx);
            XFont font;


            StringBuilder textoJustificado = new StringBuilder();
            textoJustificado.Append(
                $"                                Tengo el agrado de dirigirme a Ud. a los efectos de autorizar " +
                $"la emisión de mandamientos correspondientes al Programa Banco de la Gente y el " +
                $"débito en nuestra cuenta corriente N° {datos.CuentaCorriente} por un importe de total Pesos  " +
                $"{datos.MontoLetra} (${datos.MontoNumero}) más comisiones e impuestos, con el fin de atender los pagos " +
                $"cuyos detalle se adjunta en el archivo txt anexo. (Según convenio). ");

            tf.Alignment = XParagraphAlignment.Justify;
            font = new XFont("Calibri", 11);
            tf.DrawString(textoJustificado.ToString(), font, XBrushes.Black,
                        new XRect(60, 215, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);


            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            return new ReporteExcelBancoResultado
            {
                Blob = stream.ToArray(),
                FileName = "notaBanco " + DateTime.Now.ToString("MM-dd-yyyy") + ".pdf"
            };
        }


        public IList<ComboLotesResultado> ConsultaComboLotes(decimal? tipoLote)
        {
            return _pagosRepositorio.ObtenerComboLotes(tipoLote);
        }

        public IList<LineaAdendaResultado> ObtenerLineasAdenda(decimal nroDetalle)
        {
            return _pagosRepositorio.ObtenerLineasAdenda(nroDetalle);
        }

        public bool GenerarAdenda(GenerarAdendaComando comando)
        {
            return _pagosRepositorio.GenerarAdenda(comando.NroDetalle, comando.Comando, _sesionUsuario.Usuario.Id.Valor);
        }

        #endregion

        #region BandejaCambioEstado

        public Resultado<BandejaCambioEstadoResultado> ConsultarBandejaCambioEstado(BandejaCambioEstadoConsulta consulta)
        {
            if (consulta == null)
                consulta = new BandejaCambioEstadoConsulta { NumeroPagina = 0 };

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            
            return _pagosRepositorio.ConsultarBandejaCambioEstado(consulta); ;
        }

        public string ConsultarTotalizadorCambioEstado(BandejaCambioEstadoConsulta consulta)
        {
            if (consulta == null)
                consulta = new BandejaCambioEstadoConsulta { NumeroPagina = 0 };

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _pagosRepositorio.ObtenerTotalizadorCambioEstado(consulta); ;
        }

        public bool CambiarEstadoFormulario(decimal idFormulario)
        {
            var usuario = _sesionUsuario.Usuario;
            return _pagosRepositorio.CambiarEstadoFormulario(idFormulario, usuario.Id);
        }

        #endregion

        #region Informes Banco

        public ReporteExcelBancoResultado GenerarExcel(InformePagosConsulta comando, string idUsuario)
        {
            IList<ReporteCuadroCreditosResultado> reporteCuadroCreditos = new List<ReporteCuadroCreditosResultado>();
            IList<ReporteCuadroPagados> reporteCuadroPagados = new List<ReporteCuadroPagados>();
            IList<ReporteHistoricoPagados> reporteHistoricoPagados = new List<ReporteHistoricoPagados>();
            IList<ReporteCuadroPagados> reporteProyectadosPagos = new List<ReporteCuadroPagados>();
            IList<ReporteHistoricoPagados> reporteProyectadosDeptos = new List<ReporteHistoricoPagados>();
            IList<ExportacionPrestamo> exportacionPrestamo = new List<ExportacionPrestamo>();
            IList<ExportacionRecupero> exportacionRecupero = new List<ExportacionRecupero>();
            InformePagosConsulta informePagosConsulta = comando;
            var nombreArchivo = "Informe Banco de la gente ";

            if (comando.IdsInformes.Contains(1))
            {
                reporteCuadroCreditos =
                    _pagosRepositorio.ObtenerExcelCuadroCreditos(comando);
                informePagosConsulta.CreditosHabilitado = true;
                if (reporteCuadroCreditos.Count == 0)
                {
                    var _reporteCuadrocreditos = new ReporteCuadroCreditosResultado();
                    reporteCuadroCreditos = _reporteCuadrocreditos.crearVacio();
                }
            }

            if (comando.IdsInformes.Contains(2))
            {
                reporteCuadroPagados =
                    _pagosRepositorio.ObtenerExcelCuadroPagados(comando, true);
                informePagosConsulta.PagadosHabilitado = true;
                if (reporteCuadroPagados.Count == 0)
                {
                    var _reportCuadroPagado = new ReporteCuadroPagados();
                    reporteCuadroPagados = _reportCuadroPagado.crearVacio();
                }

                foreach (var reportdata in reporteCuadroPagados)
                {
                    reportdata.Departamento = RevisarDeptoLocSinAsignar(reportdata.Departamento);
                    reportdata.Localidad = RevisarDeptoLocSinAsignar(reportdata.Localidad);
                }
            }

            if (comando.IdsInformes.Contains(3))
            {
                reporteHistoricoPagados =
                    _pagosRepositorio.ObtenerExcelHistoricoPagados(comando, true);
                informePagosConsulta.HistoricoHabilitado = true;
                if (reporteHistoricoPagados.Count == 0)
                {
                    var _reporteHistoricoPagados = new ReporteHistoricoPagados();
                    reporteHistoricoPagados = _reporteHistoricoPagados.crearVacio();
                }

                foreach (var reportdata in reporteHistoricoPagados)
                {
                    reportdata.Departamento = RevisarDeptoLocSinAsignar(reportdata.Departamento);
                    reportdata.Localidad = RevisarDeptoLocSinAsignar(reportdata.Localidad);
                }
            }

            if (comando.IdsInformes.Contains(4))
            {
                reporteProyectadosPagos =
                    _pagosRepositorio.ObtenerExcelCuadroPagados(comando, false);
                informePagosConsulta.ProyectadosPagosHabilitado = true;
                if (reporteProyectadosPagos.Count == 0)
                {
                    var _reporteProyectadosPagos = new ReporteCuadroPagados();
                    reporteProyectadosPagos = _reporteProyectadosPagos.crearVacio();
                }

                foreach (var reportdata in reporteProyectadosPagos)
                {
                    reportdata.Departamento = RevisarDeptoLocSinAsignar(reportdata.Departamento);
                    reportdata.Localidad = RevisarDeptoLocSinAsignar(reportdata.Localidad);
                }
            }

            if (comando.IdsInformes.Contains(5))
            {
                reporteProyectadosDeptos =
                    _pagosRepositorio.ObtenerExcelHistoricoPagados(comando, false);
                informePagosConsulta.ProyectadosDtosHabilitado = true;
                if (reporteProyectadosDeptos.Count == 0)
                {
                    var _reporteProyectadosDeptos = new ReporteHistoricoPagados();
                    reporteProyectadosDeptos = _reporteProyectadosDeptos.crearVacio();
                }

                foreach (var reportdata in reporteProyectadosDeptos)
                {
                    reportdata.Departamento = RevisarDeptoLocSinAsignar(reportdata.Departamento);
                    reportdata.Localidad = RevisarDeptoLocSinAsignar(reportdata.Localidad);
                }
            }

            //if (comando.IdsInformes.Contains(6))
            //{
            //    exportacionPrestamo =
            //        _pagosRepositorio.ObtenerExportacionPrestamos(comando);
            //    informePagosConsulta.ExportacionPrestamosHabilitado = true;
            //    if (exportacionPrestamo.Count == 0)
            //    {
            //        exportacionPrestamo = ExportacionPrestamo.CrearVacio();
            //    }
            //    nombreArchivo = "Exportación de datos ";
            //}
            if (comando.IdsInformes.Contains(6))
            {
                //Se genera un proceso batch
                var txSp = "PR_BATCH_GENERAR_REP_PRESTAMOS('" + comando.FechaDesde.ToString("dd/MM/yyyy") + "','" + comando.FechaHasta.ToString("dd/MM/yyyy") + "')";

                int tipoProcesoBatch = (int)TiposProcesosEnum.ExportacionPrestamo;

                var resultado = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, tipoProcesoBatch, -1);
                
            }
            if (comando.IdsInformes.Contains(7))
            {
                //Se genera un proceso batch
                var txSp = "PR_BATCH_GENERAR_ARC_RECUPERO('" + comando.FechaDesde.ToString("dd/MM/yyyy") + "','" + comando.FechaHasta.ToString("dd/MM/yyyy") + "')";

                int tipoProcesoBatch = (int) TiposProcesosEnum.ExportacionRecupero;

                var resultado = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, tipoProcesoBatch, -1);
                
            }
            if (comando.IdsInformes.Contains(6) || comando.IdsInformes.Contains(7)) return null;

            var reportData = new ReporteBuilder("ReportePagos_InformesBanco.rdlc")
                .AgregarDataSource("DSInformes", informePagosConsulta)
                .AgregarDataSource("DSCuadroCreditos", reporteCuadroCreditos)
                .AgregarDataSource("DSPagados", reporteCuadroPagados)
                .AgregarDataSource("DSHistoricoPagos", reporteHistoricoPagados)
                .AgregarDataSource("DSCuadroProyectados", reporteProyectadosPagos)
                .AgregarDataSource("DSProyectadosDeptosLoc", reporteProyectadosDeptos)
                .AgregarDataSource("DSExportacionPrestamo", exportacionPrestamo)
                .AgregarDataSource("DSExportacionRecupero", exportacionRecupero)
                .SeleccionarFormato("Excel")
                .Generar();

            return new ReporteExcelBancoResultado
            {
                Blob = reportData.Content,
                FileName = nombreArchivo + DateTime.Now.ToString("MM-dd-yyyy") + ".xls"
            };

        }

        private string RevisarDeptoLocSinAsignar(string origen)
        {
            return string.IsNullOrEmpty(origen) || origen.Equals("-") ? "SIN DATOS" : origen;
        }

        #endregion

        #region Plan de Cuotas

        public Resultado<GrillaPlanPagosResultado> ConsultaFormularios(FiltrosFormularioConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new FiltrosFormularioConsulta {NumeroPagina = 0};
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            
            return _pagosRepositorio.ConsultaFormulariosFiltrosEnLote(consulta);
        }

        public IList<GrillaPlanPagosResultado> ConsultaFormulariosSinPaginar(FiltrosFormularioConsulta consulta)
        {
            return _pagosRepositorio.ConsultaIdsFormulariosFiltrosEnLote(consulta);
        }

        public IEnumerable<PeriodoPlanPagoComboResultado> PeriodosPlanPagosCombo()
        {
            return _pagosRepositorio.ObtenerPeriodosDePlanDePagos();
        }

        public bool ActualizarPlanDePagos(GenerarPlanPagosComando comando)
        {
            var validacion = comando.Validar();
            if (validacion != null)
                throw new ModeloNoValidoException(validacion);

            string ids = string.Join(",", comando.IdsFormularios);

            return _pagosRepositorio.ActualizarPlanPagos(ids, comando.MesesGracia, comando.FechaPago,
                _sesionUsuario.Usuario.Id.Valor);
        }

        public IList<PlanDePagoResultado> ObtenerDetallesPlanPagoFormulario(DetallesPlanDePagoConsulta consulta)
        {
            string ids = string.Join(",", consulta.IdsFormularios);
            var cuotas = _pagosRepositorio.ObtenerDetallesPlanPagosFormulario(ids);
            var detalles = cuotas.OrderBy(x => x.NroFormulario).ThenBy(x => x.NroCuota).ToList();
            var plan = new List<PlanDePagoResultado>();
            foreach (var detalle in detalles)
            {
                var planCorrespondiente = plan.FirstOrDefault(x => x.CantCuotas == detalle.CantCuotas && ValidarMonto(x.MontoCuota, detalle.MontoCuota));
                bool existePlan = planCorrespondiente != null;
                if (existePlan)
                {
                    var formulario = planCorrespondiente.Formularios.FirstOrDefault(x => x.NroFormulario == detalle.NroFormulario);
                    bool existeFormulario = formulario != null;
                    if (existeFormulario)
                    {
                        formulario.Detalles.Add(detalle);
                    }
                    else
                    {
                        planCorrespondiente.Formularios.Add(new FormularioPlanDePagoResultado
                        {
                            NroFormulario = detalle.NroFormulario,
                            CantCuotas = detalle.CantCuotas,
                            Detalles = new List<DetallePlanDePagoGrillaResultado>() { detalle }
                        });
                    }
                }
                else
                {
                    var formulario = new FormularioPlanDePagoResultado
                    {
                        NroFormulario = detalle.NroFormulario,
                        CantCuotas = detalle.CantCuotas,
                        Detalles = new List<DetallePlanDePagoGrillaResultado>() { detalle }
                    };
                    plan.Add(new PlanDePagoResultado
                    {
                        CantCuotas = detalle.CantCuotas,
                        MontoCuota = detalle.MontoCuota,
                        Formularios = new List<FormularioPlanDePagoResultado>() { formulario }
                    });
                }
                
            }

            return plan;
        }
        public bool ValidarMonto(decimal? monto, decimal? detalleMonto)
        {
            var limiteSuperior = detalleMonto.GetValueOrDefault() + 1;
            var limiteInferior = detalleMonto.GetValueOrDefault() - 1;
            return monto >= limiteInferior && monto <= limiteSuperior;
        }

        public ReporteExcelBancoResultado ImprimirPlanCuotas(DetallesPlanDePagoConsulta consulta)
        {
            string ids = string.Join(",", consulta.IdsFormularios);
            IList<DetallePlanDePagoGrillaResultado> planCuotas = _pagosRepositorio.ObtenerDetallesPlanPagosFormulario(ids).OrderBy(x => x.NroFormulario).ThenBy(x => x.NroCuota).ToList();
            IList<DatosPersonalesResultado> personas = new List<DatosPersonalesResultado>();

            foreach (var idFormulario in consulta.IdsFormularios)
            {
                DatosBasicosFormularioResultado persona = _pagosRepositorio.ObtenerSolicitante(idFormulario);
                Persona datosSolicitante = _grupoUnicoServicio.GetApiConsultaDatosCompleto(persona.SexoId, persona.NroDocumento, persona.CodigoPais, persona.IdNumero);
                
                personas.Add(new DatosPersonalesResultado
                {
                    Apellido = datosSolicitante.Apellido,
                    Nombre = datosSolicitante.Nombre,
                    NroDocumento = datosSolicitante.NroDocumento
                });
            }
            
            var reportData = new ReporteBuilder("ReportePagos_PlanCuotas.rdlc")
                .AgregarDataSource("DSPlanCuotas", planCuotas)
                .AgregarDataSource("DSPersonas", personas)
                .Generar();

            var reporte = new ReporteExcelBancoResultado();
            reporte.Blob = reportData.Content;

            if (consulta.IdsFormularios.Count == 1)
            {
                reporte.FileName = "Plan de cuotas - " + personas[0].NroDocumento + " - " + DateTime.Now.ToString("MM-dd-yyyy") +
                                   ".pdf";
            }
            else
            {
                reporte.FileName = "Plan de cuotas - " + DateTime.Now.ToString("MM-dd-yyyy") +
                                   ".pdf";
            }
            
            return reporte;
        }

        #endregion

        #region Bandeja Suaf

        public Resultado<BandejaSuafResultado> ConsultarBandejaSuaf(BandejaSuafConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaSuafConsulta {NumeroPagina = 0};
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            if (consulta.IdLote != 0)
            {
                consulta.FechaDesde = new DateTime(1, 1, 1);
                consulta.FechaHasta = new DateTime(1, 1, 1);
            }

            return _pagosRepositorio.ObtenerBandejaSuaf(consulta);
        }

        public string ConsultarTotalizadorSuaf(BandejaSuafConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaSuafConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            if (consulta.IdLote != 0)
            {
                consulta.FechaDesde = new DateTime(1, 1, 1);
                consulta.FechaHasta = new DateTime(1, 1, 1);
            }

            return _pagosRepositorio.ObtenerTotalizadorSuaf(consulta);
        }

        public Resultado<BandejaFormulariosSuafResultado> ConsultarBandejaFormulariosSuaf(
            BandejaFormulariosSuafConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaFormulariosSuafConsulta {NumeroPagina = 0};
            }

            if (consulta.IdLoteSuaf != null)
            {
                consulta.FechaDesde = new DateTime(1, 1, 1);
                consulta.FechaHasta = new DateTime(1, 1, 1);
            }

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            var res = _pagosRepositorio.ObtenerBandejaFormulariosSuaf(consulta);
            res.Elementos = res.Elementos.OrderBy(f => f.NroFormulario).ToList();
            return res;
        }


        public Resultado<BandejaFormulariosSuafResultado> ConsultarBandejaFormulariosSuafSeleccionarTodos(
            BandejaFormulariosSuafConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaFormulariosSuafConsulta { NumeroPagina = 0 };
            }

            if (consulta.IdLoteSuaf != null)
            {
                consulta.FechaDesde = new DateTime(1, 1, 1);
                consulta.FechaHasta = new DateTime(1, 1, 1);
            }

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();
            var resultado = _pagosRepositorio.ObtenerBandejaFormulariosSuafSeleccionarTodos(consulta);
            return resultado;
        }

        public decimal RegistrarLoteSuaf(RegistrarLoteSuafComando comando)
        {
            comando.NombreLote = comando.NombreLote + " " + DateTime.Now.ToString("dd/MM/yyyy HH-mm") + "hs";

            return _pagosRepositorio.RegistrarLoteSuaf(comando.IdPrestamosSeleccionados, comando.NombreLote,
                _sesionUsuario.Usuario.Id.Valor).Valor;
        }

        public ReporteExcelBancoResultado ObtenerExcelSuaf(decimal idLote)
        {
            var reportData = new ReporteBuilder("ReporteSuafExcel.rdlc")
                .AgregarDataSource("DataSet", _pagosRepositorio.GenerarExcelSuaf(idLote))
                .SeleccionarFormato("Excel")
                .Generar();

            return new ReporteExcelBancoResultado
            {
                Blob = reportData.Content,
                FileName = "suaf " + DateTime.Now.ToString("dd-MM-yyyy") + ".xls"
            };
        }

        public ReporteExcelBancoResultado ObtenerExcelActivacionMasiva(decimal idLote)
        {
            var datosActivacion = _pagosRepositorio.DatosExcelActivacionMasiva(idLote);
            var reportData = new ReporteBuilder("ReporteActivacionMasivaExcel.rdlc")
                .AgregarDataSource("DataSet", obtenerDsExcelActivacionMasiva(datosActivacion))
                .SeleccionarFormato("Excel")
                .Generar();

            return new ReporteExcelBancoResultado
            {
                Blob = reportData.Content,
                FileName = "base_beneficiarios_importar" + ".xls"
            };
        }

        private IList<DSExcelActivacionMasiva> obtenerDsExcelActivacionMasiva(IList<FilaExcelActivacionMasiva> datos)
        {
            var dataSet = new List<DSExcelActivacionMasiva>();

            foreach (var fila in datos)
            {
                dataSet.Add(new DSExcelActivacionMasiva
                {
                    Cuil = fila.CuilSolicitante,
                    NombreCompleto = fila.NombreYApellidoSolicitante,
                    FechaInicioAct = DateTime.Now.ToString("dd/MM/yyyy"),
                    Calle = "SAN MARTIN",
                    NroCasa = "875",
                    Piso = "0",
                    Dpto = "0",
                    Barrio = "CENTRO",
                    Ciudad = fila.Localidad,
                    CodPostal = "0",
                    IdProvincia = "3",
                    Telefono = "0",
                    Fax = "0",
                    Celular = "0",
                    Mail = "noinforma@gmail.com",
                    Mail2 = "",
                    CalleCpc = "SAN MARTIN",
                    NroCasaCpc = "875",
                    PisoCpc = "0",
                    DptoCpc = "0",
                    BarrioCpc = "CENTRO",
                    CiudadCpc = fila.Localidad,
                    CodPostalCpc = "0",
                    IdProvinciaCpc = "3",
                    TelefonoCpc = "0",
                    CelularCpc = "0",
                    FaxCpc = "0"
                });
            }
            return dataSet;
        }

        public IList<ComboLotesResultado> ObtenerComboLotesSuaf()
        {
            return _pagosRepositorio.ObtenerLotesSuaf().ToList();
        }
        public IList<ComboTiposPagoResultado> ObtenerComboTipoPago()
        {
            return _pagosRepositorio.ObtenerTipoPago().ToList();
        }

        public bool CargaDevengadoManual(CargaDevengadoComando comando)
        {
            return _pagosRepositorio.CargaDevengado(comando.NroFormulario, comando.Devengado, DateTime.Now,
                _sesionUsuario.Usuario.Id.Valor);
        }

        public bool BorrarDevengadoManual(CargaDevengadoComando comando)
        {
            return _pagosRepositorio.BorrarDevengado(comando.NroFormulario, _sesionUsuario.Usuario.Id.Valor);
        }

        public IList<ComboLotesResultado> ObtenerModalidades()
        {
            return _pagosRepositorio.ObtenerModalidadesPago();
        }

        public IList<ComboLotesResultado> ObtenerElementos()
        {
            return _pagosRepositorio.ObtenerElementosPago();
        }

        public IList<Convenio> ObtenerConvenios(int idTipoConvenio)
        {
            var listaConvenios = _pagosRepositorio.ObtenerConveniosPago();

            return listaConvenios.Where(x => x.IdTipoConvenio == idTipoConvenio).ToList();
        }

        public bool ActualizarModalidadPago(ActualizaModalidadComando comando)
        {
            _pagosRepositorio.RegistrarConvenioLote(comando.IdLote, comando.ConvenioPago);
          
            return _pagosRepositorio.ActualizarModalidadPago(comando.IdLote, comando.FechaPago, comando.FechaFinPago, comando.ModalidadPago, comando.ElementoPago, comando.MesesGracia, comando.GeneraPlanCuotas, _sesionUsuario.Usuario.Id.Valor);
        }

        #endregion

        #region Importar Archivo SUAF

        public ImportacionSuafResultado ImportarArchivoSuaf(RegistrarExcelSuafComando comando, Id idUsuario)
        {
            try
            {
                if (comando == null)
                {
                    throw new ModeloNoValidoException("El archivo SUAF es requerido");
                }

                if (!ValidarLoteSuaf(comando.Archivo.Buffer, comando.LoteId))
                {
                    throw new ModeloNoValidoException("El archivo no se corresponde al lote seleccionado.");
                }

                List<ImportarArchivoSuaf> registrosDevengados;
                List<ImportarArchivoSuaf> registrosNoProcesados;
                List<ImportarArchivoSuaf> errores;

                Importer.FromExcel<ImportarArchivoSuaf>()
                    .AddColumn(3, x => x.NumeroFormulario)
                    .AddColumn(4, x => x.FechaDevengadoString)
                    .AddColumn(13, x => x.Devengado)
                    .SetInterceptor(new ArchivoSuafInterceptor())
                    .LeerArchivoSuaf(comando.Archivo.Buffer, out registrosDevengados, out registrosNoProcesados,
                        out errores);

                int cantidadDuplicados, varAuxiliar;
                ActualizarDevengado(registrosDevengados, idUsuario, ref registrosNoProcesados, ref registrosDevengados, out cantidadDuplicados,
                    true);
                ActualizarDevengado(registrosNoProcesados, idUsuario, ref registrosNoProcesados,
                    ref registrosDevengados, out varAuxiliar);
                ActualizarEstadosPrestamos(comando.LoteId, idUsuario);

                var resultado = new ImportacionSuafResultado
                {
                    CantidadDevengados = registrosDevengados.Count - cantidadDuplicados,
                    CantidadNoProcesados = registrosNoProcesados.Count
                };

                return resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e.GetType() != typeof(ModeloNoValidoException))
                {
                    throw new ModeloNoValidoException("Hubo un problema al importar el archivo. \n" + e.Message);
                }

                throw;
            }
        }

        public bool ValidarLoteSuaf(byte[] arraBytes, Id idLote)
        {
            try
            {
                var registros = new List<ImportarArchivoSuaf>();
                var registrosNoProcesados = new List<ImportResult<ImportarArchivoSuaf>>();

                Importer.FromExcel<ImportarArchivoSuaf>()
                    .AddColumn(3, x => x.NumeroFormulario)
                    .SetInterceptor(new ValidadorSuafInterceptor(idLote, this))
                    .Generate(arraBytes, out registros, out registrosNoProcesados);

                if (registros.Any())
                {
                    return true;
                }

                return false;
            }
            catch (ModeloNoValidoException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool ValidarLoteSuafPorNumeroFormulario(Id idLote, Id idFormulario)
        {
            var resultado = _pagosRepositorio.ValidarLoteSuafPorNumeroFormulario(idFormulario, idLote);
            return resultado;
        }

        public void ActualizarDevengado(List<ImportarArchivoSuaf> registros, Id idUsuario,
            ref List<ImportarArchivoSuaf> registrosNoProcesados, ref List<ImportarArchivoSuaf> registrosDevengados,
            out int cantidadDuplicados, bool sonDevengados = false)
        {
            cantidadDuplicados = 0;
            foreach (var registro in registros)
            {
                try
                {
                    var actualizaDevengado = _pagosRepositorio.CargaDevengado(
                        registro.NumeroFormulario,
                        registro.Devengado,
                        registro.FechaDevengado,
                        idUsuario.Valor);

                    if (sonDevengados && !actualizaDevengado)
                    {
                        registrosNoProcesados.Add(registro);
                        cantidadDuplicados++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public void ActualizarEstadosPrestamos(Id idLote, Id idUsuario)
        {
            _pagosRepositorio.ActualizarEstadosPrestamos(idLote, idUsuario);
        }

        #endregion
    }
}
