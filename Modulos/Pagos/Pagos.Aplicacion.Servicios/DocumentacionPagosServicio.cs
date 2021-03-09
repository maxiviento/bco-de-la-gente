using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using GrupoUnico.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Reportes;
using Infraestructura.Core.Reportes.BarCode;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Soporte.Aplicacion.Servicios;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;
using Formulario = Formulario.Dominio.Modelo.Formulario;

namespace Pagos.Aplicacion.Servicios
{
    public class DocumentacionPagosServicio
    {
        private readonly IProvidenciaRepositorio _providenciaRepositorio;
        private readonly IPagosRepositorio _pagosRepositorio;
        private readonly FormularioServicio _formularioServicio;
        private readonly BancosServicio _bancosServicio;
        private readonly GrupoUnicoServicio _grupoUnicoServicio;
        private readonly DocumentacionBGEUtilServicio _documentacionBgeUtilServicio;
        private readonly ILineaPrestamoRepositorio _lineaPrestamoRepositorio;
        private readonly IFormularioRepositorio _formularioRepositorio;
        private readonly IPrestamoRepositorio _prestamoRepositorio;

        public DocumentacionPagosServicio(IProvidenciaRepositorio pagosProvidencia, 
            IPagosRepositorio pagosRepositorio,
            FormularioServicio formularioServicio, 
            GrupoUnicoServicio grupoUnicoServicio,
            ILineaPrestamoRepositorio lineaPrestamoRepositorio, 
            DocumentacionBGEUtilServicio documentacionBgeUtilServicio,
            BancosServicio bancosServicio,
            IPrestamoRepositorio prestamoRepositorio)
        {
            _providenciaRepositorio = pagosProvidencia;
            _pagosRepositorio = pagosRepositorio;
            _formularioServicio = formularioServicio;
            _grupoUnicoServicio = grupoUnicoServicio;
            _lineaPrestamoRepositorio = lineaPrestamoRepositorio;
            _documentacionBgeUtilServicio = documentacionBgeUtilServicio;
            _bancosServicio = bancosServicio;
            _prestamoRepositorio = prestamoRepositorio;
        }

        #region Cuponera
        public async Task<ReporteResultado> ObtenerCuponera(ReportePagosConsulta consulta, string idUsuario)
        {
            if (consulta.IdsFormularios.Count == 0)
                return new ReporteResultado(new List<string>() { "No se encontraron formularios" });

            if (consulta.IdsFormularios.Count > 1)
            {
                RegistrarProcesoBatchCuponera(consulta, idUsuario);
                return null;
            }

            List<string> errores = new List<string>();
            List<string> motivosNoImpresion = new List<string>();
            string motivosNoImpresionPlan = "";
            string motivosNoImpresionApoderado = "";
            ConcatenadorPDF concatenador = new ConcatenadorPDF();

            foreach (var id in consulta.IdsFormularios)
            {
                var formulario = _formularioServicio.ObtenerPorId(id);
                if (formulario.TipoApoderado == (int) TipoApoderadoEnum.PertenecePeroNoApoderado)
                {
                    motivosNoImpresionApoderado ="Ninguno de los formularios seleccionados es apoderado.";
                    continue;
                }

                var consultaFormularios = new FiltrosFormularioConsulta { Dni = formulario.Solicitante.NroDocumento };
                IList<FormularioFiltradoResultado> formularios =
                    _formularioServicio.ConsultaFormulariosSinPaginar(consultaFormularios);
                var datosPrestamo = formularios.FirstOrDefault(x => x.IdFormulario.Valor == id);
                
                if (concatenador.ExisteArchivoEnDirectorio($"Pagos_{formulario.Solicitante.NroDocumento}.pdf"))
                    continue;

                // DATOS COMUNES
                var datosSolicitante = ObtenerDatosPersonalesCompleto(formulario.Solicitante);
                if (datosSolicitante == null)
                {
                    errores.Add($"Error al obtener los datos del beneficiario DNI = {formulario.Solicitante.NroDocumento} - ID Formulario = {formulario.Id}");
                    continue;
                }
                
                LineaPrestamoResultado datosLinea =
                    _lineaPrestamoRepositorio.ConsultarLineaPorId(new Id(formulario.DetalleLinea.LineaId));
                
                var cuotas = _pagosRepositorio.ObtenerDetallesPlanPagosFormulario(id.ToString());
                if (cuotas == null || cuotas?.Count == 0)
                {
                    motivosNoImpresionPlan = "Ninguno de los formularios seleccionados tiene plan de cuotas.";
                    continue;
                }

                cuotas = cuotas.OrderBy(x => x.NroCuota).ToList();

                string nombreCompletoSolicitante = $"{datosSolicitante.Nombre} {datosSolicitante.Apellido}";
                //El numero 22 significa la cantidad de caracteres maximo permitido para que el nombre no desface a la cuponera
                 if (nombreCompletoSolicitante.Length > 22)
                {
                    nombreCompletoSolicitante = nombreCompletoSolicitante.Substring(0 , 22);
                }
                var dsCuponera = new List<ReporteCuponeraResultado>();
                foreach (var cuota in cuotas)
                {
                    int numPrestamo = datosPrestamo != null
                        ? datosPrestamo.NroFormulario ?? 0
                        : formulario.Id;
                    string numeroPrestamo = numPrestamo.ToString("D8");
                    string numReferencia = numPrestamo.ToString("D13") + cuota.NroCuota.ToString("D3");
                    
                    CodigoBarras codigoBarras = GenerarCodigoBarrasCuponera(numPrestamo, cuota.NroCuota, cuota.FechaCuota, cuota.MontoCuota ?? 0);
                    dsCuponera.Add(new ReporteCuponeraResultado
                    {
                        NumeroPrestamo = numeroPrestamo,
                        Beneficiario = nombreCompletoSolicitante,
                        NumeroDocumento = datosSolicitante.NroDocumento,
                        LineaPrestamo = datosLinea.Nombre,
                        NumeroReferencia = numReferencia,
                        NumeroCuota = cuota.NroCuota.ToString(),
                        ImporteCuota = cuota.MontoCuota.ToString(),
                        CodigoBarras = codigoBarras.GetBarCodeInBase64(),
                        DataCB = codigoBarras.Data,
                        FechaVencimiento = cuota.FechaCuota.ToString("dd/MM/yy")
                    });
                }

                // ARMADO DEL REPORTE
                var reportData = new ReporteBuilder("ReportePagos_Cuponera.rdlc")
                    .AgregarDataSource("DSReporteCuponera", dsCuponera)
                    .Generar();

                concatenador.AgregarReporte(reportData, "Cuponera_" + formulario.Solicitante.NroDocumento);
            }
            // MERGE DE REPORTES CON PDFSharp
            var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();

            if (arrayBytesConcatenado == null)
            {
                if (motivosNoImpresionApoderado.Length > 0)
                {
                    motivosNoImpresion.Add(motivosNoImpresionApoderado);
                }
                if (motivosNoImpresionPlan.Length > 0)
                {
                    motivosNoImpresion.Add(motivosNoImpresionPlan);
                }
                return new ReporteResultado(motivosNoImpresion);
            }

            return new ReporteResultado(_documentacionBgeUtilServicio.GenerarArchivoReporteBGE(arrayBytesConcatenado, TipoArchivo.PDF, "CuponeraPagos", consulta.IdPrestamo, consulta.IdLote));
        }
        public async Task<ReporteResultado> ObtenerCuponerasNoImpresas(ReportePagosConsulta consulta)
        {
            if (consulta.IdsFormularios.Count == 0)
                return null;

            var formulariosSinPlanDeCuotas = "";
            var formulariosNoApoderado = "";
            StringBuilder archivo = new StringBuilder();
            archivo.AppendLine("No es posible imprimir las siguientes cuponeras: \n");

            foreach (var id in consulta.IdsFormularios)
            {
                var formulario = _formularioServicio.ObtenerPorId(id);

                var consultaFormularios = new FiltrosFormularioConsulta { Dni = formulario.Solicitante.NroDocumento };
                IList<FormularioFiltradoResultado> formularios =
                    _formularioServicio.ConsultaFormulariosSinPaginar(consultaFormularios);
                var datosPrestamo = formularios.FirstOrDefault(x => x.IdFormulario.Valor == id);

                if (formulario.TipoApoderado == (int)TipoApoderadoEnum.PertenecePeroNoApoderado)
                {
                    formulariosNoApoderado += datosPrestamo.NroFormulario + " ";
                }

                var cuotas = _pagosRepositorio.ObtenerDetallesPlanPagosFormulario(id.ToString());
                if (cuotas == null || cuotas?.Count == 0)
                {
                    formulariosSinPlanDeCuotas += datosPrestamo.NroFormulario + " ";
                }

            }
            if (formulariosNoApoderado.Length > 0)
            {
                archivo.AppendLine("- Por no ser apoderados: \n");
                archivo.AppendLine("  " + formulariosNoApoderado + "\r\n");
            }
            if (formulariosSinPlanDeCuotas.Length > 0)
            {
                archivo.AppendLine("- Por no poseer plan de cuotas: \n");
                archivo.Append("  " + formulariosSinPlanDeCuotas);    
            }
            if (formulariosNoApoderado.Length > 0 || formulariosSinPlanDeCuotas.Length > 0)
            {
                var nombreArchivo = "Cuponeras no impresas";
                return GenerarReporteFormulariosInvalidos(archivo, nombreArchivo);
            }
             return null;
        }

        public ReporteResultado GenerarReporteFormulariosInvalidos(StringBuilder archivo, string nombreArchivo)
        {
            var reporte = new ReporteDocumentosPagoResultado
            {
                Blob = Encoding.ASCII.GetBytes(archivo.ToString()),
                FileName = nombreArchivo
            };
            return new ReporteResultado(_documentacionBgeUtilServicio.GenerarArchivoReporteBGE(reporte.Blob, TipoArchivo.Txt, reporte.FileName));
        }

        #region Codigo de barras cuponera
        private CodigoBarras GenerarCodigoBarrasCuponera(int numeroPrestamo, int cuota, DateTime fechaVencimiento, decimal importe)
        {
            int importeEntero = 0;
            int importeDecimal = 0;
            string importeString = importe.ToString();

            importeEntero = int.Parse(importeString.Split(',')[0]);
            if (importeString.Contains(','))
               {
                   importeDecimal = int.Parse(importeString.Split(',')[1]);
               }
            
            int convenio = 184;
            int grupo = 2;
            string fechaVen = fechaVencimiento.ToString("ddMMyy");

            StringBuilder data = new StringBuilder();
            data.Append(convenio.ToString("D4")) //convenio
                .Append(grupo.ToString("D2")) //grupo ?? => 02
                .Append(numeroPrestamo.ToString("D13"))
                .Append(cuota.ToString("D3"))
                .Append("200101") //vencimiento
                .Append(importeEntero.ToString("D7"))
                .Append(importeDecimal.ToString().PadRight(14,'0'));
            string verificador = CodigoVerificadorCodigoBarras(data.ToString());
            data.Append(verificador); //codigo verificador
            return new CodigoBarras(data.ToString());
        }

        private string CodigoVerificadorCodigoBarras(string codigo)
        {
            string vl_verif = "9713971397139713971397139713971397139713971397139713";
            int vl_1;
            int vl_2 = 0;
            int xl = codigo.Length;
            for (int c = 1; c < xl; c++)
            {
                vl_1 = int.Parse(codigo.Substring(c, 1)) * int.Parse(vl_verif.Substring(c, 1));
                vl_2 += vl_1;
            }
            string vl_t = STR(vl_2, 5, 0).Trim();
            vl_t = vl_t.Substring(vl_t.Length - 1, 1);

            string codigoVerificador = STR(10 - int.Parse(vl_t), 2, 0).Trim();
            return codigoVerificador.Substring(codigoVerificador.Length - 1, 1);
        }

        private static string STR(double d, int totalLen, int decimalPlaces)
        {
            int floor = (int)Math.Floor(d);
            int length = floor.ToString().Length;
            if (length > totalLen)
                throw new NotImplementedException();
            if (totalLen - length < decimalPlaces)
                decimalPlaces = totalLen - length;
            if (decimalPlaces < 0)
                decimalPlaces = 0;
            string str = Math.Round(d, decimalPlaces).ToString();
            if (str.StartsWith("0") && str.Length > 1 && totalLen - decimalPlaces - 1 <= 0)
                str = str.Remove(0, 1);

            return str.Substring(0, str.Length >= totalLen ? totalLen : str.Length);
        }
        
        #endregion

        #endregion

        #region Documentacion Pagos

        public async Task<ReporteResultado> ObtenerReportePagos(ReportePagosConsulta consulta, string idUsuario)
        {
            if (consulta.IdsFormularios.Count == 0)
                return new ReporteResultado(new List<string>() {"No se encontraron formularios"});

            var fecha = consulta.Fecha;

            if (consulta.IdsFormularios.Count > 1)
            {
                RegistrarProcesoBatchDocumentacion(consulta, idUsuario);
                return null;
            }

            List<string> errores = new List<string>();
            ConcatenadorPDF concatenador = new ConcatenadorPDF();
    
            foreach (var id in consulta.IdsFormularios)
            {
                var formulario = _formularioServicio.ObtenerPorId(id);

                if (formulario.IdEstado != EstadoFormulario.Rechazado.Id.Valor)
                {
                    var consultaFormularios = new FiltrosFormularioConsulta {Dni = formulario.Solicitante.NroDocumento};
                    IList<FormularioFiltradoResultado> formularios =
                        _formularioServicio.ConsultaFormulariosSinPaginar(consultaFormularios);
                    var datosPrestamo = formularios.FirstOrDefault(x => x.IdFormulario.Valor == id);

                    
                    var consultaFormularioPrestamo = new FiltrosFormularioConsulta
                    {
                        NroPrestamo = datosPrestamo.NroPrestamo,
                    };
                    
                    IList<FormularioFiltradoResultado> formulariosAsociativo =
                            _formularioServicio.ConsultaFormulariosSinPaginar(consultaFormularioPrestamo);
                    

                    if (concatenador.ExisteArchivoEnDirectorio($"Pagos_{formulario.Solicitante.NroDocumento}.pdf"))
                        continue;

                    // DATOS COMUNES
                    var datosSolicitante = ObtenerDatosPersonalesCompleto(formulario.Solicitante);
                    if (datosSolicitante == null)
                    {
                        errores.Add(
                            $"Error al obtener los datos del beneficiario DNI = {formulario.Solicitante.NroDocumento} - ID Formulario = {formulario.Id}");
                        continue;
                    }

                    List<DatosPersonalesResultado> garantes = new List<DatosPersonalesResultado>();
                    foreach (var garante in formulario.Garantes)
                    {
                        var datos = ObtenerDatosPersonalesCompleto(garante);
                        if (datos == null)
                        {
                            errores.Add(
                                $"Error al obtener los datos del garante DNI = {garante.NroDocumento} - ID Formulario = {formulario.Id}");
                        }
                        else
                        {
                            garantes.Add(datos);
                        }
                    }

                    if (consulta.FechaAprobacion)
                    {
                        var fechaAprobacion = _prestamoRepositorio.ObtenerFechaAprobacion(
                            _prestamoRepositorio.ObtenerIdPrestamo(id).IdPrestamo).FechAprobacion;
                        fecha = fechaAprobacion;
                    }
                    LineaPrestamoResultado datosLinea =
                        _lineaPrestamoRepositorio.ConsultarLineaPorId(new Id(formulario.DetalleLinea.LineaId));

                    // DATOS DE LOS SUBREPORTES Y ORDEN
                    //var subreportesPagos = _lineaPrestamoRepositorio.ObtetenerCuadrantesPorIdLinea(formulario.DetalleLinea.LineaId).Where(x => x.IdTipoSalida != 1).OrderBy(x => x.Orden);
                    var subreportesPagos = SubReporte.GetAllPagos().OrderBy(x => x.IdCuadrante.Valor);
                    var subReportesOrdenados = new List<SubReporte>();
                    var reporteContratoMutuo = new Reporte();
                    // AGREGADO EN ORDEN DE LOS SUBREPORTES
                    if (consulta.IdsReportesPagos.Contains((int)SubReporte.Pagos.ContratoMutuo) && formulario.TipoApoderado != (int)TipoApoderadoEnum.PertenecePeroNoApoderado)
                    {
                        var contratoMutuoContent = GenerarReporteContratoMutuo(datosSolicitante, garantes, datosPrestamo,
                            formulario, formulariosAsociativo, fecha);
                        reporteContratoMutuo = new Reporte(contratoMutuoContent, "pdf", "application/pdf");
                    }
                    foreach (var subreporte in subreportesPagos)
                    {
                        if (!consulta.IdsReportesPagos.Contains((int)subreporte.IdCuadrante.Valor)) continue;
                        if (subreporte.IdCuadrante.Valor != (int) SubReporte.Pagos.Caratula &&
                            formulario.TipoApoderado == (int) TipoApoderadoEnum.PertenecePeroNoApoderado) continue;
                        
                            var subRep = ObtenerSubreporteDocumentacionPagos((int)subreporte.IdCuadrante.Valor,
                                datosSolicitante, garantes, datosPrestamo, formulario, datosLinea, formulariosAsociativo, fecha);
                            if (subRep != null)
                                subReportesOrdenados.Add(subRep);
                    }

                    // ARMADO DEL REPORTE
                    var reportData = new ReporteBuilder("ReportePagos.rdlc", "SubReportePagos")
                        .SubReporteDS(subReportesOrdenados)
                        .GenerarConSubReporte(false);

                    if (subReportesOrdenados.Count > 0)
                        concatenador.AgregarReporte(reportData, "Pagos_" + formulario.Solicitante.NroDocumento);

                    if (formulario.TipoApoderado != (int)TipoApoderadoEnum.PertenecePeroNoApoderado && consulta.IdsReportesPagos.Contains((int)SubReporte.Pagos.ContratoMutuo))
                        concatenador.AgregarReporte(reporteContratoMutuo, "Pagos_" + formulario.Solicitante.NroDocumento);
                    
                } else return new ReporteResultado(new List<string>() { "El formulario seleccionado está en estado rechazado" });
            }
            // MERGE DE REPORTES CON PDFSharp
            var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();

            if (arrayBytesConcatenado == null)
                return new ReporteResultado(concatenador.Errores);
            return new ReporteResultado(_documentacionBgeUtilServicio.GenerarArchivoReporteBGE(arrayBytesConcatenado, TipoArchivo.PDF, "Pagos", consulta.IdPrestamo, consulta.IdLote, consulta.IdFormularioLinea));
        }

        private void RegistrarProcesoBatchCuponera(ReportePagosConsulta consulta, string idUsuario)
        {
            var nroGrupo = -1;

            if (consulta.FechaAprobacion)
            {
                consulta.Fecha = DateTime.MinValue;
            }

            var txSp = "PR_BATCH_GENERAR_REP_CUPONERA()";

            nroGrupo = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, 8, nroGrupo);

            string listadoFormularios = string.Join(",", consulta.IdsFormularios);

            _pagosRepositorio.RegistrarDetallesProcesoBatch(nroGrupo, listadoFormularios, idUsuario);
        }


        private void RegistrarProcesoBatchDocumentacion(ReportePagosConsulta consulta, string idUsuario)
        {
            var nroGrupo = -1;

            if (consulta.FechaAprobacion)
            {
                consulta.Fecha = DateTime.MinValue;
            }

            foreach (var idReporte in consulta.IdsReportesPagos)
            {
                //Caratula = 1,
                //Recibo = 2,
                //Pagare = 3,
                //Providencia = 4,
                //ContratoMutuo = 5


                switch (idReporte)
                {
                    case 1:
                    {
                        var txSp = "PR_BATCH_GENERAR_REP_CURATULA()";

                        nroGrupo = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, 9, nroGrupo);
                        break;
                    }
                    case 2:
                    {
                        var txSp = "PR_BATCH_GENERAR_REP_RECIBO('" + consulta.Fecha.ToString("dd/MM/yyyy") + "')";

                        nroGrupo = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, 12, nroGrupo);
                        break;
                    }
                    case 3:
                    {
                        var txSp = "PR_BATCH_GENERAR_REP_PAGARE('" + consulta.Fecha.ToString("dd/MM/yyyy") + "')";

                        nroGrupo = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, 13, nroGrupo);
                        break;
                    }
                    case 4:
                    {
                        var txSp = "PR_BATCH_GENERAR_REP_PROVIDEN('" + consulta.Fecha.ToString("dd/MM/yyyy") + "')";

                        nroGrupo = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, 10, nroGrupo);
                        break;
                    }
                    case 5:
                    {
                        var txSp = "PR_BATCH_GENERAR_REP_CON_MUT('" + consulta.Fecha.ToString("dd/MM/yyyy") + "')";

                        nroGrupo = _pagosRepositorio.RegistrarProceshoBatch(txSp, idUsuario, 11, nroGrupo);
                        break;
                    }
                }
            }
            string listadoFormularios = string.Join(",", consulta.IdsFormularios);
            
            _pagosRepositorio.RegistrarDetallesProcesoBatch(nroGrupo, listadoFormularios, idUsuario);
        }

        public async Task<ReporteResultado> ObtenerReportePagosNoImpresos(ReportePagosConsulta consulta)
        {
            if (consulta.IdsFormularios.Count == 0)
                return null;

            string formulariosNoApoderado = "";
            StringBuilder archivo = new StringBuilder();
            archivo.AppendLine("No es posible imprimir los siguientes formularios por no ser apoderados: \n");
            foreach (var id in consulta.IdsFormularios)
            {
                var formulario = _formularioServicio.ObtenerPorId(id);

                if (formulario.IdEstado != EstadoFormulario.Rechazado.Id.Valor)
                {
                    var consultaFormularios = new FiltrosFormularioConsulta { Dni = formulario.Solicitante.NroDocumento };
                    IList<FormularioFiltradoResultado> formularios =
                        _formularioServicio.ConsultaFormulariosSinPaginar(consultaFormularios);
                    var datosPrestamo = formularios.FirstOrDefault(x => x.IdFormulario.Valor == id);
                    var subreportesPagos = SubReporte.GetAllPagos().OrderBy(x => x.IdCuadrante.Valor);

                    foreach (var subreporte in subreportesPagos)
                    {
                        if (!consulta.IdsReportesPagos.Contains((int)subreporte.IdCuadrante.Valor)) continue;
                        if (subreporte.IdCuadrante.Valor != (int)SubReporte.Pagos.Caratula &&
                            formulario.TipoApoderado == (int)TipoApoderadoEnum.PertenecePeroNoApoderado)
                        {
                            if (!formulariosNoApoderado.Contains(datosPrestamo.NroFormulario.ToString()))
                            formulariosNoApoderado += datosPrestamo.NroFormulario + " ";
                       }
                    }
                    if (formulario.TipoApoderado == (int) TipoApoderadoEnum.PertenecePeroNoApoderado &&
                        consulta.IdsReportesPagos.Contains((int) SubReporte.Pagos.ContratoMutuo))
                    {

                        if (!formulariosNoApoderado.Contains(datosPrestamo.NroFormulario.ToString()))
                            formulariosNoApoderado += datosPrestamo.NroFormulario + " ";
                    }
                }
            }
            if (formulariosNoApoderado.Length > 0)
            {
                archivo.AppendLine(" " + formulariosNoApoderado);
                var nombreArchivo = "Documentación no impresa";
                return GenerarReporteFormulariosInvalidos(archivo, nombreArchivo);
            } 

            return null;
        }

        private SubReporte ObtenerSubreporteDocumentacionPagos(int id, DatosPersonalesResultado datosSolicitante,
            List<DatosPersonalesResultado> garantes, FormularioFiltradoResultado datosPrestamo,
            DatosFormularioResultado formulario, LineaPrestamoResultado datosLinea,IList<FormularioFiltradoResultado> formulariosAsociativo, DateTime fecha)
        {
            var datosBasicos = _providenciaRepositorio.ObtenerSolicitante(formulario.Id);
            var datosProvidencia = _providenciaRepositorio.ObtenerDatosParaProvidencia(formulario.Id);
            datosProvidencia.ImporteLetras = ConvertidorNumeroLetras.enletras(datosProvidencia.Importe).ToLower();

            var mes = new CultureInfo("es-ES", false).DateTimeFormat.GetMonthName(fecha.Month);
            string fechaString = $"{fecha.Day} de {mes} de {fecha.Year}";

            string nombreCompletoSolicitante = $"{datosSolicitante.Apellido} {datosSolicitante.Nombre}";

            string nombreCompletoGarante = "";
            string telefonoGarante = "";
            string domicilioGaranteCompleto = "";
            string garanteDNI = "";
            if (garantes.Count > 0)
            {
                nombreCompletoGarante = $"{garantes[0].Apellido} {garantes[0].Nombre}";
                telefonoGarante = !string.IsNullOrEmpty(garantes[0].CodigoArea)
                    ? $"({garantes[0].CodigoArea}) {garantes[0].Telefono}"
                    : garantes[0].Telefono;
            }

            string numeroFormulario = datosPrestamo?.NroFormulario.ToString() ?? formulario.Id.ToString();

            switch (id)
            {
                case (int) SubReporte.Pagos.Caratula:
                {
                    var idSucursal = _bancosServicio.ObtenersucursalFormulario(formulario.Id).Id;

                    var dsCaratula = new List<ReporteCaratulaResultado>
                    {
                        new ReporteCaratulaResultado
                        {
                            NumeroFormulario = numeroFormulario,
                            Fecha = formulario.FechaInicioPagos?.ToString("dd/MM/yyyy"),
                            SolicitanteNombre = nombreCompletoSolicitante,
                            SolicitanteDocumento = datosSolicitante.NroDocumento,
                            ValorPrestamo = formulario.CondicionesSolicitadas.MontoSolicitado?.ToString() ?? "",
                            LineaPrestamo = datosLinea.Descripcion,
                            SolicitanteDomicilioCompleto = datosSolicitante.DomicilioCompleto,
                            GaranteNombre = nombreCompletoGarante,
                            TelefonoTitular = !string.IsNullOrEmpty(datosSolicitante.CodigoArea)
                                ? $"({datosSolicitante.CodigoArea}) {datosSolicitante.Telefono}"
                                : datosSolicitante.Telefono,
                            TelefonoGarante = telefonoGarante,
                            SucursalBancaria = formulario.NombreSucursalBancaria,
                            CodigoSucursal = idSucursal,
                            Localidad = datosSolicitante.DomicilioGrupoFamiliarLocalidad,
                            Departamento = datosSolicitante.DomicilioGrupoFamiliarDepartamento
                        }
                    };
                    return SubReporte.Caratula().AsignarDataSet(dsCaratula);
                }
                case (int)SubReporte.Pagos.Providencia:
                {
                        var dsProvidencia = new List<ReporteProvidenciaResultado>()
                    {
                        new ReporteProvidenciaResultado
                        {
                            Destino = datosProvidencia.Destino,
                            Importe = datosProvidencia.Importe,
                            ImporteLetras = datosProvidencia.ImporteLetras,
                            Localidad = datosProvidencia.Localidad,
                            Departamento = datosProvidencia.Departamento,
                            NroSticker = datosBasicos.NroSticker,
                            NombreLinea = datosLinea.Nombre,
                            DescripcionLinea = datosLinea.Descripcion,
                            NroFormulario = numeroFormulario,
                            NombreCompleto = nombreCompletoSolicitante,
                            Cuil = datosSolicitante.Cuil,
                            Fecha = fecha.ToString("dd/MM/yyyy"),
                        }
                    };
                        _providenciaRepositorio.RegistrarProvidencia(formulario.Id);
                        return SubReporte.Providencia().AsignarDataSet(dsProvidencia);
                    }
                case (int)SubReporte.Pagos.Recibo:
                    {
                        var dsRecibo = new List<ReporteReciboResultado>()
                    {
                        new ReporteReciboResultado
                        {
                            NumeroFormulario = numeroFormulario,
                            Fecha = fechaString,
                            SolicitanteNombre = nombreCompletoSolicitante,
                            SolicitanteDocumento = datosSolicitante.NroDocumento,
                            ValorPrestamo = formulario.CondicionesSolicitadas.MontoSolicitado?.ToString() ?? "",
                            LineaPrestamo = datosLinea.Descripcion,
                            SolicitanteDomicilioCompleto = datosSolicitante.DomicilioCompleto,
                            ValorPrestamoString =
                                ConvertidorNumeroLetras.enletras(
                                    formulario.CondicionesSolicitadas.MontoSolicitado?.ToString() ?? "")
                        }
                    };
                        return SubReporte.Recibo().AsignarDataSet(dsRecibo);
                    }
                case (int)SubReporte.Pagos.Pagare:
                    {
                        var dsPagare = new List<ReportePagareResultado>()
                    {
                        new ReportePagareResultado
                        {
                            NumeroFormulario = numeroFormulario,
                            Fecha = fechaString,
                            FechaVencimientoPlanPago = formulario.FechaVencimientoPlanPago?.ToString("dd/MM/yyyy"),
                            SolicitanteNombre = nombreCompletoSolicitante,
                            SolicitanteDocumento = datosSolicitante.NroDocumento,
                            ValorPrestamo = formulario.CondicionesSolicitadas.MontoSolicitado?.ToString() ?? "",
                            SolicitanteDomicilioCompleto = datosSolicitante.DomicilioCompleto,
                            SucursalMontoDisponible = formulario.DomicilioSucursalBancaria
                        }
                    };
                        return SubReporte.Pagare().AsignarDataSet(dsPagare);
                    }
                default:
                    return null;
            }
        }

        private string ObtenerDatosSolicitantes(IList<FormularioFiltradoResultado> formulariosAsociativo)
        {
            string res = "";


            foreach (var formulario in formulariosAsociativo)
            {
                var formularioSolicitante = _formularioServicio.ObtenerPorId((int) formulario.IdFormulario.Valor).Solicitante;
                var solicitante = ObtenerDatosPersonalesCompleto(formularioSolicitante);
                if (formulario.IdEstadoFormulario != EstadoFormulario.Rechazado.Id.Valor)
                {
                    string nombreSolicitante = solicitante.Nombre + " " + solicitante.Apellido;
                    string documentoSolicitante = solicitante.NroDocumento;
                    string domicilioSolicitante = solicitante.DomicilioCompleto;
                    string estadoCivilSolicitante = solicitante.EstadoCivil;

                    res += $"Sr/Sra: {nombreSolicitante}, DNI N°: {documentoSolicitante}, Domicilio: {domicilioSolicitante}, Estado Civil: {estadoCivilSolicitante}";
                    if (formulario.NroFormulario != (formulariosAsociativo.Count - 1)) res += ", ";
                }
            }
            return res;

        }

        private string ObtenerListadoGarantes(List<DatosPersonalesResultado> garantes)
        {
            string res = "";
            for (int i = 0; i <= garantes.Count - 1; i++)
            {
                DatosPersonalesResultado garante = garantes[i];
                string nombre = garante.Nombre + " " + garante.Apellido;
                string documento = garante.NroDocumento;
                string domicilio = garante.DomicilioCompleto;

                res += $"Sr/Sra: {nombre}, DNI N°: {documento}, Domicilio: {domicilio}";
                if (i != (garantes.Count - 1)) res += ", ";
            }

            return res;
        }

        private byte[] GenerarReporteContratoMutuo(DatosPersonalesResultado datosSolicitante,
            List<DatosPersonalesResultado> garantes, FormularioFiltradoResultado datosPrestamo,
            DatosFormularioResultado formulario, IList<FormularioFiltradoResultado> formulariosAsociativo, DateTime fecha)
        {
            var mes = new CultureInfo("es-ES", false).DateTimeFormat.GetMonthName(fecha.Month);
            string fechaString = $"{fecha.Day} de {mes} de {fecha.Year}";
            string numeroFormulario = datosPrestamo?.NroFormulario.ToString() ?? formulario.Id.ToString();
            string nombreCompletoSolicitante = $"{datosSolicitante.Apellido}, {datosSolicitante.Nombre}";
            var dsContratoMutuo =
                        new ReporteContratoMutuoResultado
                        {
                            CantidadFormularios = formulariosAsociativo.Count.ToString(),
                            DatosSolicitantes = ObtenerDatosSolicitantes(formulariosAsociativo) ,
                            NumeroFormulario = numeroFormulario,
                            Fecha = fechaString,
                            SolicitanteNombre = nombreCompletoSolicitante,
                            SolicitanteDocumento = datosSolicitante.NroDocumento,
                            SolicitanteCuil = datosSolicitante.Cuil,
                            ValorPrestamo = formulario.CondicionesSolicitadas.MontoSolicitado?.ToString(),
                            SolicitanteDomicilioCompleto = datosSolicitante.DomicilioCompleto,
                            ValorPrestamoString =
                                ConvertidorNumeroLetras.enletras(
                                    formulario.CondicionesSolicitadas.MontoSolicitado?.ToString()),
                            FechaPrimerVencimientoPago = formulario.FechaPrimerVencimientoPago?.ToString("dd/MM/yyyy"),
                            FechaAnioLetras = ConvertidorNumeroLetras.enletras(fecha.Year.ToString()),
                            FechaMesLetras = mes,
                            FechaDiaLetras = ConvertidorNumeroLetras.enletras(fecha.Day.ToString()),
                            MontoCuota =
                                formulario.CondicionesSolicitadas.MontoEstimadoCuota != null
                                    ? Math.Round((double) formulario.CondicionesSolicitadas.MontoEstimadoCuota, 2).ToString()
                                    : "-",
                            MontoCuotaString =  formulario.CondicionesSolicitadas.MontoEstimadoCuota != null
                                    ? ConvertidorNumeroLetras.enletras(formulario.CondicionesSolicitadas.MontoEstimadoCuota.ToString())
                                    : "-",

                            Cuotas = formulario.CondicionesSolicitadas.CantidadCuotas != null
                                ? formulario.CondicionesSolicitadas.CantidadCuotas.ToString()
                                : "-",
                            CuotasString = formulario.CondicionesSolicitadas.CantidadCuotas != null
                                    ? ConvertidorNumeroLetras.enletras(formulario.CondicionesSolicitadas.CantidadCuotas.ToString())
                                    : "-",
                            ListadoGarantes = ObtenerListadoGarantes(garantes),
                            SolicitanteEstadoCivil = datosSolicitante.EstadoCivil,
                            DescripcionLinea = formulario.DetalleLinea.Descripcion,
                            NroLinea =  formulario.DetalleLinea.Nombre,
                            Destino = formulario.Destino
                    };

            var reporte = new ReporteBuilder("ReportePagos_ContratoMutuo.rdlc")
            .AgregarDataSource("DSContratoMutuo", dsContratoMutuo)
            .Generar();

            Stream streamReporte = new MemoryStream(reporte.Content);
            PdfDocument inputDocument = PdfReader.Open(streamReporte, PdfDocumentOpenMode.Import);
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage pdfPage = pdfDocument.AddPage(inputDocument.Pages[0]);
            XGraphics gfx = XGraphics.FromPdfPage(pdfPage);
            XTextFormatter tf = new XTextFormatter(gfx);
            XFont font;

            var textoContrato = "Contrato de Mutuo N°: " + dsContratoMutuo.NumeroFormulario;
            var textoFinal = "CERTIFICO: que los datos y la firma consignados en el presente son verdaderas y corresponden a las personas firmantes.";
            if (formulariosAsociativo.Count > 1)
            {
                #region Contrato Mutuo Asociativo

                StringBuilder textoAsociativa = new StringBuilder();
                textoAsociativa.Append($"Ciudad de Córdoba,  {dsContratoMutuo.FechaDiaLetras}  día/s del mes de {dsContratoMutuo.FechaMesLetras} del año {dsContratoMutuo.FechaAnioLetras}, comparece " +
                                       $"ante el funcionario actuante los {dsContratoMutuo.DatosSolicitantes}todos de la Provincia de Córdoba, en su carácter de beneficiarios, del programa Banco de " +
                                       $"la Gente, N° de Solicitud {dsContratoMutuo.NumeroFormulario} cuya ejecución se encuentra a cargo de la Secretaría de Equidad y Promoción del Empleo, por el " +
                                       $"cual percibe una AYUDA ECONÓMICA REINTEGRABLE, acordado mediante PROVIDENCIA N° {dsContratoMutuo.NumeroFormulario} de la Sra Secretaria de Equidad y Promoción del " +
                                       $"Empleo, en el marco de las disposiciones contenidas en el Decreto 1791/15 y mod. Por Decreto 39/16. Art. 39. Inc. 14-. ratificado por ley N° 10337 y Decreto " +
                                       $"186/2016, resolución 133/2016 y resolución 170/2019 por la suma de pesos {dsContratoMutuo.ValorPrestamoString} (${dsContratoMutuo.ValorPrestamo}), " +
                                       $"el que deberá ser:  {dsContratoMutuo.NroLinea} - {dsContratoMutuo.DescripcionLinea} destinado a CONSUMO. Los beneficiarios en este acto manifiestan conocer " +
                                       $"cabalmente los términos, condiciones y normativa del programa del cual resultan beneficiarios. Asimismo reconoce que la ayuda económico recibida" +
                                       $" por el presente beneficio es de carácter “REINTEGRABLE”, asumiendo ante el superior Gobierno de la Provincia de Córdoba la responsabilidad de " +
                                       $"efectivizar el reintegro de la ayuda económica de la que resulta/n beneficiario/s en {dsContratoMutuo.CuotasString} ({dsContratoMutuo.Cuotas}) cuotas consecutivas de pesos " +
                                       $"{dsContratoMutuo.MontoCuotaString} (${dsContratoMutuo.MontoCuota}) cada una de ellas, las que deberán ser abonadas del (1) al (10) de cada mes, " +
                                       $"mediante la cuponera de pagos que en este acto reciben, las que podrán ser abonadas en cualquiera de las sucursales del Banco Provincia de Córdoba S.A. " +
                                       $"cuya primera cuota en consecuencia vence el {dsContratoMutuo.FechaPrimerVencimientoPago}. Los beneficiarios suscriben y se constituyen en carácter de " +
                                       $"cotitulares y codeudores solidarios de las obligaciones que surgen del presente un PAGARÉ SIN PROTESTO a nombre del SUPERIOR GOBIERNO DE LA PROVINCIA " +
                                       $"DE CÓRDOBA por igual monto que el beneficio acordado y con vencimiento idéntico al de la última cuota de devolución pactada para el presente beneficio. " +
                                       $"La falta de pago en tiempo y forma de cualquiera de las cuotas previstas para el presente beneficio hará incurrir los mismos en mora automática, haciéndose " +
                                       $"pasible de la aplicación de los intereses oratorios y punitorios correspondientes. Así mismo declaran conocer que la falta de pago de dos cuotas en " +
                                       $"forma consecutiva, facultará al Superior Gobierno de la Provincia de Córdoba, sin necesidad de interpelación judicial o extrajudicial alguna a " +
                                       $"solicitar la devolución del importe total entregado más los intereses que se devenguen por tal mora, ya sea por la vía administrativa y/o " +
                                       $"judicial correspondiente y dar por caducados los plazos de las cuotas adeudadas, pudiendo reclamar el pago total de ellas como si fuesen " +
                                       $"vencidas y exigibles, mediante el ejercicio de las acciones legales conducentes. Los beneficiarios asumen el compromiso de presentar la " +
                                       $"cuponera de pagos, con la constancia de la cancelación total de la deuda, dentro de los 15 días del pago de la última cuota pactada. " +
                                       $"Las manifestaciones realizadas por los beneficiarios revisten el CARÁCTER DE DECLARACIÓN JURADA. Por el presente los beneficiarios, " +
                                       $"se constituyen en lisos y llanos pagadores de todas las obligaciones asumidas en el presente contrato, durante la duración del mismo y " +
                                       $"aún después de vencido, hasta su total cumplimiento firmado de plena conformidad el presente instrumento. Los codeudores solidarios acuerdan " +
                                       $"en designar como representante del grupo para intervenir solamente en el cobro del crédito objeto del presente contrato ante el Banco de la " +
                                       $"Provincia de Córdoba al  {dsContratoMutuo.SolicitanteNombre} DNI:{dsContratoMutuo.SolicitanteDocumento}, con domicilio en calle " +
                                       $"{dsContratoMutuo.SolicitanteDomicilioCompleto}. A todos los efectos legales, las partes se someten expresamente –ya sea para la interpretación " +
                                       $"y cumplimiento de este contrato- a las leyes y tribunales de la Ciudad de Córdoba, renunciando a cualquier otra normativa o tribunal que por " +
                                       $"razones de domicilio u otras circunstancias pudieran invocar las partes, constituyendo domicilio en los lugares arriba indicados, donde se " +
                                       $"considerarán válidas todas las notificaciones y emplazamientos judiciales o extrajudiciales que se hagan. Se firman de plena conformidad dos " +
                                       $"ejemplares de un mismo e idéntico tenor, en el lugar y fecha indicados supra. Ante el funcionario actuante que firma y da pie de lo actuado.");

                //Primer Titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoContrato, font, XBrushes.Black,
                            new XRect(60, 70, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);
                //Segundo titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString("AYUDA ECONÓMICA REINTEGRABLE", font, XBrushes.Black,
                            new XRect(60, 95, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);
                //Cuerpo
                tf.Alignment = XParagraphAlignment.Justify;
                font = new XFont("Calibri", 9);
                tf.DrawString(textoAsociativa.ToString(), font, XBrushes.Black,
                            new XRect(60, 115, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Texto Firma
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoFinal, font, XBrushes.Black,
                           new XRect(40, pdfPage.Height.Point - 50, pdfPage.Width.Point - 80, pdfPage.Height.Point - 10), XStringFormats.TopLeft);
                #endregion
            }
            else
            {
                #region Contrato Mutuo Individual

                StringBuilder textoIndividual = new StringBuilder();
                textoIndividual.Append($"En la ciudad de Córdoba, a los {dsContratoMutuo.FechaDiaLetras} día/s del mes de {dsContratoMutuo.FechaMesLetras}" +
                                       $" del año {dsContratoMutuo.FechaAnioLetras} comparece ante el funcionario actuante el Sr./Sra {dsContratoMutuo.SolicitanteNombre}" +
                                       $" DNI {dsContratoMutuo.SolicitanteDocumento} (CUIL {dsContratoMutuo.SolicitanteCuil}), con domicilio real en calle {dsContratoMutuo.SolicitanteDomicilioCompleto}" +
                                       $", Estado Civil {dsContratoMutuo.SolicitanteEstadoCivil}, en su carácter del programa Banco de la gente, N° {dsContratoMutuo.NumeroFormulario}" +
                                       $", cuya ejecución se encuentra a cargo del Ministerio de Promoción del Empleo y de la Economía Familiar, por el cual percibe una AYUDA " +
                                       $"ECONOMICA REINTEGRABLE, acordando mediante PROVIDENCIA {dsContratoMutuo.NumeroFormulario}, de la Ministra de Promoción del Empleo y de la Economía Familiar" +
                                       $", en el marco de las disposiciones contenidas en el artículo 27°, incs. 14), 15) y 20) del Decreto 1615/19 39/16" +
                                       $", por la suma de pesos {dsContratoMutuo.ValorPrestamoString}" +
                                       $" (${dsContratoMutuo.ValorPrestamo}), el que deberá ser: ({dsContratoMutuo.NroLinea}) {dsContratoMutuo.DescripcionLinea}" +
                                       $" destinado a {dsContratoMutuo.Destino}. EL beneficiario en este acto manifiesta conocer cabalmente los términos, condiciones y normativa del programa del " +
                                       $"cual resulta beneficiario. Asimismo reconoce que la ayuda económica recibida por el presente beneficio es de carácter \"REINTEGRABLE\", " +
                                       $"asumiendo ante el superior Gobierno de la Provincia de Córdoba la responsabilidad de efectivizar el reintegro de la ayuda económica de la " +
                                       $"que resulta beneficiaria en {dsContratoMutuo.CuotasString} ({dsContratoMutuo.Cuotas}) cuotas consecutivas de pesos {dsContratoMutuo.MontoCuotaString} " +
                                       $"(${dsContratoMutuo.MontoCuota}) cada una de ellas, las que deberán ser abonadas del (1) al (10) de cada mes, mediante la cuponera " +
                                       $"de pagos que en este acto recibe, las que podrán ser abonadas en cualquiera de las sucursales del Banco Provincia de Córdoba S.A. cuya primera" +
                                       $" cuota en consecuencia vence el {dsContratoMutuo.FechaPrimerVencimientoPago}. El beneficiario/a suscribe en garantía de las obligaciones que surgen del " +
                                       $"presente un PAGARE SIN PROTESTO a nombre del SUPERIOR GOBIERNO DE LA PROVINCIA DE CORDOBA por igual monto que el beneficio acordado, y " +
                                       $"con vencimiento idéntico al de la última cuota de devolución pactada para el presente beneficio. La falta de pago en tiempo y forma de " +
                                       $"cualquiera de las cuotas previstas para el presente beneficio hará incurrir al mismo en mora automática, haciéndose pasible de la " +
                                       $"aplicación de los intereses moratorios y punitorios correspondientes. Asimismo declara conocer que la falta de pago de dos cuotas en " +
                                       $"forma consecutiva, facultará al Superior Gobierno de la Provincia de Córdoba, sin necesidad de interpelación judicial o extrajudicial " +
                                       $"alguna, a solicitar la devolución del importe total entregado más los intereses que se devenguen por tal mora, ya sea por la vía " +
                                       $"administrativa y/o judicial correspondiente y dar por caducados los plazos de las cuotas adeudadas, pudiendo reclamar el pago total de " +
                                       $"ellas como si fuesen vencidas y exigibles, mediante el ejercicio de las acciones legales conducentes. Las manifestaciones hechas por el " +
                                       $"beneficiario revisten el CARACTER DE DECLARACIÓN JURADA. A solicitud del beneficiario/a, el {dsContratoMutuo.ListadoGarantes} se " +
                                       $"constituye en fiador/es solidarios, liso y llano pagador de todas las obligaciones asumidas por el beneficiario que surgen del " +
                                       $"presente contrato durante la duración del mismo y aún después de vencido, hasta su total cumplimiento, haciéndose expresa renuncia a " +
                                       $"los beneficios de excusión y división de deuda, firmando de plena conformidad del presente instrumento. A todos los efectos legales, " +
                                       $"las partes y el fiador, se someten expresamente -ya sea para la interpretación y cumplimiento de este contrato- a las leyes y tribunales " +
                                       $"de la Ciudad de Córdoba, renunciando a cualquier otra normativa o tribunal que por razones de domicilio u otras circunstancias pudieran " +
                                       $"invocar las partes, constituyendo domicilio en los lugares arriba indicados, donde se considerarán válidas todas las notificaciones y " +
                                       $"emplazamientos judiciales o extrajudiciales que se hagan. Se firman de plena conformidad dos ejemplares de un mismo e idéntico tenor, en " +
                                       $"el lugar y fecha indicados supra, ante el funcionario actuante que firma y da pie de lo actuado.");

                //Primer Titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoContrato, font, XBrushes.Black,
                            new XRect(60, 70, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Segundo titulo
                font = new XFont("Calibri", 14, XFontStyle.Bold);
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString("\"AYUDA ECONÓMICA REINTEGRABLE\"", font, XBrushes.Black,
                           new XRect(60, 95, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Cuerpo
                tf.Alignment = XParagraphAlignment.Justify;
                font = new XFont("Calibri", 10);
                tf.DrawString(textoIndividual.ToString(), font, XBrushes.Black,
                           new XRect(60, 115, pdfPage.Width.Point - 110, pdfPage.Height.Point - 10), XStringFormats.TopLeft);

                //Texto Firma
                tf.Alignment = XParagraphAlignment.Center;
                tf.DrawString(textoFinal, font, XBrushes.Black,
                           new XRect(40, pdfPage.Height.Point - 45, pdfPage.Width.Point - 80, pdfPage.Height.Point - 10), XStringFormats.TopLeft);


                #endregion
            }

            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            byte[] docBytes = stream.ToArray();
            return docBytes;

        }

        private DatosPersonalesResultado ObtenerDatosPersonalesCompleto(DatosPersonaResultado persona)
        {
            try
            {
                var datosSolicitante = _grupoUnicoServicio.GetApiConsultaDatosCompleto(persona.SexoId,
                    persona.NroDocumento, persona.CodigoPais, persona.IdNumero);
                var consultaDatosContacto = new DatosPersonaConsulta
                {
                    IdSexo = persona.SexoId,
                    NroDocumento = persona.NroDocumento,
                    CodigoPais = persona.CodigoPais,
                    IdNumero = datosSolicitante.IdNumero.ToString()
                };
                var datosContactoSolicitante = _formularioServicio.ObtenerDatosContacto(consultaDatosContacto);

                var resultado =
                    _grupoUnicoServicio.GetApiConsultaDatosPersonales(persona.SexoId, persona.NroDocumento,
                        persona.CodigoPais, persona.IdNumero);

                resultado.CodigoArea = datosContactoSolicitante.CodigoArea;
                resultado.Telefono = datosContactoSolicitante.Telefono;
                resultado.CodigoAreaCelular = datosContactoSolicitante.CodigoAreaCelular;
                resultado.Celular = datosContactoSolicitante.Celular;
                resultado.Email = string.IsNullOrEmpty(resultado.Email)
                    ? datosContactoSolicitante.Mail
                    : resultado.Email;

                return resultado;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion


    }
}
