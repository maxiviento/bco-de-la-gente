using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Infraestructura.Core.Comun.Dato;
using Formulario.Aplicacion.Consultas.Consultas;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Identidad.Dominio.Modelo;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Reportes;
using Soporte.Aplicacion.Servicios;

namespace Formulario.Aplicacion.Servicios
{
    public class PrestamoServicio
    {
        private readonly IPrestamoRepositorio _prestamoRepositorio;
        private readonly IFormularioRepositorio _formularioRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IMotivoRechazoRepositorio _motivoRechazoRepositorio;
        private readonly DocumentacionBGEUtilServicio _documentacionBgeUtilServicio;

        public PrestamoServicio(IPrestamoRepositorio prestamoRepositorio,
            IFormularioRepositorio formularioRepositorio,
            IMotivoRechazoRepositorio motivoRechazoRepositorio,
            ISesionUsuario sesionUsuario,
            DocumentacionBGEUtilServicio documentacionBgeUtilServicio)
        {
            _prestamoRepositorio = prestamoRepositorio;
            _formularioRepositorio = formularioRepositorio;
            _motivoRechazoRepositorio = motivoRechazoRepositorio;
            _sesionUsuario = sesionUsuario;
            _documentacionBgeUtilServicio = documentacionBgeUtilServicio;
        }

        public PrestamoResultado.Detallado ConsultarPorId(decimal id)
        {
            var prestamo = _prestamoRepositorio.ConsultarPorId(id);
            return prestamo;
        }

        public IList<PrestamoResultado.Integrante> ConsultarIntegrantes(decimal id)
        {
            var prestamo = _prestamoRepositorio.ConsultarIntegrantesPrestamo(id);
            return prestamo;
        }

        public Resultado<BandejaPrestamoResultado> ObtenerPrestamosPorFiltros(BandejaPrestamosConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaPrestamosConsulta { NumeroPagina = 0 };
            }

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            return _prestamoRepositorio.ObtenerPrestamosPorFiltros(consulta);
        }
        public string ObtenerTotalziadorPrestamos(BandejaPrestamosConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaPrestamosConsulta { NumeroPagina = 0 };
            }

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            return _prestamoRepositorio.ObtenerTotalziadorPrestamos(consulta);
        }

        public RegistrarPrestamoResultado GenerarPrestamo(int id)
        {
            // Valido agrupamiento
            var agrupamiento = _prestamoRepositorio.ConsultarAgrupamiento(id);
            if (agrupamiento == null)
                throw new ErrorTecnicoException($"No se encontró el agrupamiento {id}");
            //Valido el estado de los formularios
            if (agrupamiento.IdEstado == EstadoFormulario.Iniciado.Id.Valor)
            {
            // Valido existencia de tabla etapa_estado_línea
            var etapasEstadoLinea = _prestamoRepositorio.ObtenerEtapasEstadosLinea((long) agrupamiento.IdLinea.Valor);
            if (etapasEstadoLinea == null || etapasEstadoLinea.Count == 0)
            {
                throw new ErrorTecnicoException(
                    $"No se encuentra configuradas las etapas y estados de la línea {agrupamiento.Nombre}");
            }

            // Genero el préstamo
            var idPrestamo = _prestamoRepositorio.GenerarPrestamo(id, _sesionUsuario.Usuario.Id);
            var datosPrestamo = _prestamoRepositorio.ConsultarDatosPrestamo(idPrestamo);
            var actualizacionEtapa = _prestamoRepositorio.ActualizarEtapaPrestamo(new Prestamo(new Id(idPrestamo)));
            if (actualizacionEtapa != "OK")
                throw new ErrorTecnicoException(actualizacionEtapa);
            return new RegistrarPrestamoResultado(idPrestamo, decimal.Parse(datosPrestamo.NumeroPrestamo));
            }
            return new RegistrarPrestamoResultado();
        }

        public bool ValidarFormularioCanceladoParaGarante(DatosPersonaResultado garante,
            List<DatosPersonaResultado> solicitantes, int idLinea)
        {
            bool permiteAgrupacion = false;
            FormularioCanceladoParaPrestamo formulario =
                _prestamoRepositorio.ValidarFormularioCanceladoParaGarante(garante, idLinea);
            if (formulario != null)
            {
                if (formulario.IdEstado.Valor == EstadoFormulario.Rechazado.Id.Valor)
                {
                    List<DatosPersonaResultado> garantesDelFormulario =
                        new List<DatosPersonaResultado>(
                            _formularioRepositorio.ObtenerGarantes((int)formulario.Id.Valor));
                    garantesDelFormulario = garantesDelFormulario != null
                        ? garantesDelFormulario
                        : new List<DatosPersonaResultado>();
                    permiteAgrupacion = garantesDelFormulario.TrueForAll(_garante =>
                    {
                        var coindicencia =
                            solicitantes.Find(solicitante => solicitante.esIgualQueOtraPersona(_garante));
                        return coindicencia != null;
                    });
                }
            }

            return false;
        }

        public Resultado<BandejaConformarPrestamoResultado> ObtenerFormulariosPorFiltros(
            BandejaConformarPrestamoConsulta consulta)
        {
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            var formularios = _prestamoRepositorio.ObtenerFormulariosPorFiltros(consulta);
            foreach (var formulario in formularios.Elementos)
            {
                var solicitante = _formularioRepositorio.ObtenerSolicitante((int)formulario.Id.Valor);
                if (formulario.Solicitante != null && solicitante != null)
                {
                    formulario.Solicitante.SexoId = solicitante.SexoId;
                    formulario.Solicitante.CodigoPais = solicitante.CodigoPais;
                    formulario.Solicitante.NroDocumento = solicitante.NroDocumento;
                    formulario.Solicitante.IdNumero = solicitante.IdNumero;
                }

                var datosBasicos = _formularioRepositorio.ObtenerSolicitante((int)formulario.Id.Valor);
                if (formulario.Solicitante == null)
                {
                    formulario.Solicitante = new DatosPersonaResultado();
                }

                if (datosBasicos != null)
                {
                    formulario.Solicitante.SexoId = datosBasicos.SexoId;
                    formulario.Solicitante.CodigoPais = datosBasicos.CodigoPais;
                    formulario.Solicitante.NroDocumento = datosBasicos.NroDocumento;
                    formulario.Solicitante.IdNumero = datosBasicos.IdNumero;
                }

                formulario.Garantes = _formularioRepositorio.ObtenerGarantes((int)formulario.Id.Valor);
            }

            return formularios;
        }

        public IList<RequisitoResultado.Detallado> ConsultarRequisitosPrestamo(decimal id)
        {
            var requisitos = _prestamoRepositorio.ConsultarRequisitosPrestamo(id);
            return requisitos;
        }


        public void ActualizarRequisitos(PrestamoComando comando)
        {
            foreach (var requisito in comando.Requisitos)
            {
                var detalle = new DetallePrestamo(
                    null,
                    null,
                    new RequisitoPrestamo(new Id(requisito.Id)),
                    requisito.EsSolicitante,
                    requisito.EsGarante,
                    requisito.EsSolicitanteGarante);

                var resultado = _prestamoRepositorio.ActualizarRequisitosChecklist(comando.Id, detalle, comando.IdFormularioLinea);
                if (resultado != "OK")
                    throw new ErrorTecnicoException(resultado);
            }
        }

        public string AceptarCheckList(PrestamoComando comando)
        {
            ActualizarRequisitos(comando);

            EstadoPrestamo estado = EstadoPrestamo.ConId((int)comando.IdEstado);
            var datosPrestamo = new ActualizarDatosPrestamoComando();
            string resultadoActualizar;

            datosPrestamo.IdPrestamo = comando.Id;
            datosPrestamo.Observaciones = "CAMBIO DE ETAPA";
            datosPrestamo.TotalFolios = comando.TotalFolios;
            datosPrestamo.ActualizarEtapa = true;
            datosPrestamo.IdFormularioLinea = comando.IdFormularioLinea;

            var etapasEstadosLinea = ObtenerEtapasxEstadosLinea(comando.IdLinea ?? 2);

            // Si es estado creado y aceptar, genera 2 seguimientos (Comenzado y Ev Tecnica)
            if (comando.IdEstado == 1 || comando.IdEstado == 5)
            {
                estado = EstadoPrestamo.ConId(5);
                datosPrestamo.IdEstadoPrestamo = estado.Id.Valor;
                var resultadoActualizar1 = ActualizarDatosPrestamo(datosPrestamo, true);

                //estado = EstadoPrestamo.TransicionAceptarConId(5);
                estado = EstadoPrestamo.TransicionAceptarConEtapas((int)estado.Id.Valor, etapasEstadosLinea);
                datosPrestamo.IdEstadoPrestamo = estado.Id.Valor;
                var resultadoActualizar2 = ActualizarDatosPrestamo(datosPrestamo, true);

                if (resultadoActualizar1 != "OK" || resultadoActualizar2 != "OK")
                    throw new ErrorTecnicoException(resultadoActualizar1);
            }
            else
            {
                // Si no es estado creado y es aceptar, se hace una transicion de estado
                var ultimoSeguimiento =
                    ObtenerUltimoSeguimiento(new SeguimientosPrestamoConsulta { IdPrestamo = comando.Id.ToString() });
                //TODO: se crea un seguimiento en el mismo estado en el que estaba con las observaciones si hubo un cambio
                if (ultimoSeguimiento != null && ultimoSeguimiento.Observaciones != comando.Observaciones)
                {
                    datosPrestamo.IdEstadoPrestamo = estado.Id.Valor;
                    resultadoActualizar = ActualizarDatosPrestamo(datosPrestamo, false);
                    if (resultadoActualizar != "OK")
                        throw new ErrorTecnicoException(resultadoActualizar);
                }

                estado = EstadoPrestamo.TransicionAceptarConEtapas((int)comando.IdEstado, etapasEstadosLinea);
                if (estado == null)
                    throw new ErrorTecnicoException("El estado actual del préstamo no puede transicionar de etapa dada la configuración de la línea.");

                datosPrestamo.IdEstadoPrestamo = estado.Id.Valor;
                resultadoActualizar = ActualizarDatosPrestamo(datosPrestamo, true);
                if (resultadoActualizar != "OK")
                    throw new ErrorTecnicoException(resultadoActualizar);
            }
            return "OK";
        }

        public string GuardarCheckList(PrestamoComando comando)
        {
            ActualizarRequisitos(comando);

            EstadoPrestamo estado = EstadoPrestamo.ConId((int)comando.IdEstado);
            var datosPrestamo = new ActualizarDatosPrestamoComando();
            string resultadoActualizar;

            datosPrestamo.IdPrestamo = comando.Id;
            datosPrestamo.Observaciones = comando.Observaciones;
            datosPrestamo.TotalFolios = comando.TotalFolios;
            datosPrestamo.ActualizarEtapa = false;
            datosPrestamo.IdFormularioLinea = comando.IdFormularioLinea;

            // Si es estado creado y guardar, actualiza el prestamo a Comenzado
            if (comando.IdEstado == 1)
            {
                estado = EstadoPrestamo.ConId(5);
                datosPrestamo.IdEstadoPrestamo = estado.Id.Valor;

                resultadoActualizar = ActualizarDatosPrestamo(datosPrestamo, false);
                if (resultadoActualizar != "OK")
                    throw new ErrorTecnicoException(resultadoActualizar);
            }

            // Si no es estado creado y es guardar, se genera un seguimiento en el mismo estado
            if (comando.IdEstado != 1)
            {
                datosPrestamo.IdEstadoPrestamo = estado.Id.Valor;
                resultadoActualizar = ActualizarDatosPrestamo(datosPrestamo, false);
                if (resultadoActualizar != "OK")
                    throw new ErrorTecnicoException(resultadoActualizar);
            }
            return "OK";
        }


        public IList<RequisitoResultado.Cargado> ConsultarRequisitosCargados(decimal id, decimal idFormularioLinea)
        {
            var requisitos = _prestamoRepositorio.ConsultarRequisitosCargados(id, idFormularioLinea);
            return requisitos;
        }

        public PrestamoResultado.Datos ConsultaDatosPrestamo(decimal id)
        {
            var prestamo = _prestamoRepositorio.ConsultarDatosPrestamo(id);
            if (prestamo.IdTipoIntegrante.Equals(1)) prestamo.EsSolicGarante = _prestamoRepositorio.EsSolicitanteGarante(id);
            return prestamo;
        }

        public IList<PrestamoResultado.Garante> ConsultaDatosGarantePrestamo(decimal id)
        {
            var garante = _prestamoRepositorio.ObtenerGarantesPrestamo(id);
            return garante;
        }

        public string ActualizarDatosPrestamo(ActualizarDatosPrestamoComando comando, bool actualizoEtapa)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var estado = new EstadoPrestamo((int)comando.IdEstadoPrestamo);
            var seguimientos = new List<SeguimientoPrestamo>();
            seguimientos.Add(new SeguimientoPrestamo(usuario, comando.Observaciones, estado));
            var prestamo = new Prestamo(new Id(comando.IdPrestamo), comando.TotalFolios, seguimientos);
            var resultadoPrestamo = _prestamoRepositorio.ActualizarDatosPrestamo(prestamo);
            if (resultadoPrestamo != "OK")
                throw new ErrorTecnicoException(resultadoPrestamo);
            foreach (var seguimiento in prestamo.Seguimientos)
            {
                var resultadoSeguimiento =
                    _prestamoRepositorio.ActualizarSeguimientoPrestamo(prestamo.Id.Valor, seguimiento, comando.IdFormularioLinea);
                if (resultadoSeguimiento != "OK")
                    throw new ErrorTecnicoException(resultadoPrestamo);
            }

            if (actualizoEtapa)
            {
                var actualizacionEtapa = _prestamoRepositorio.ActualizarEtapaPrestamo(prestamo);
                if (actualizacionEtapa != "OK")
                    throw new ErrorTecnicoException(actualizacionEtapa);
            }

            return "OK";
        }

        private PrestamoResultado.Seguimiento ObtenerUltimoSeguimiento(SeguimientosPrestamoConsulta consulta)
        {
            var seguimientos = _prestamoRepositorio.ListarSeguimientosPrestamo(consulta);
            if (seguimientos == null || seguimientos.Count == 0)
                return null;

            return seguimientos[0];
        }

        public Resultado<PrestamoResultado.Seguimiento> ConsultarSeguimientos(SeguimientosPrestamoConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new SeguimientosPrestamoConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = 5;


            return _prestamoRepositorio.ConsultarSeguimientosPrestamo(consulta);
        }
        
        public decimal RegistrarRechazoPrestamo(RechazarPrestamoComando comando)
        {
            if (comando.MotivosRechazo == null || comando.MotivosRechazo.Count <= 0)
                throw new ModeloNoValidoException("Debe seleccionar al menos un motivo de rechazo");

            var usuario = _sesionUsuario.Usuario;
            var prestamo = new Prestamo(new Id(comando.IdPrestamo), usuario, comando.NumeroCaja);
            
            return this._prestamoRepositorio.Rechazar(prestamo, comando.MotivosRechazo);
        }

        public IList<ClaveValorResultado<string>> ConsultarEstadosPrestamo()
        {
            var estados = _prestamoRepositorio.ConsultarEstadosPrestamo();
            var estadosResultados = estados.Select(estado => new ClaveValorResultado<string>(
                    estado.Id.ToString(),
                    estado.Descripcion
                ))
                .ToList();
            return estadosResultados;
        }

        public IList<EtapaEstadoLineaResultado> ObtenerEtapasxEstadosLinea(long idLinea)
        {
            return _prestamoRepositorio.ObtenerEtapasEstadosLinea(idLinea);
        }

        public PrestamoResultado.EncabezadoArchivos ObtenerEncabezadoPrestamoArchivos(long idPrestamo)
        {
            var resultado = _prestamoRepositorio.ObtenerEncabezadoPrestamoArchivos(idPrestamo);
            return resultado;
        }

        public IList<AgruparFormulario> ObtenerFormulariosPorAgrumiento(int idAgrupamiento)
        {
            return _prestamoRepositorio.ObtenerFormulariosPorAgrupamiento(idAgrupamiento);
        }

        public FechaAprobacionResultado ObtenerFechaAprobacion(int idPrestamo)
        {
            return _prestamoRepositorio.ObtenerFechaAprobacion(idPrestamo);
        }

        public decimal ObtenerIdPrestamo(int idFormulario)
        {
            var prestamo = _prestamoRepositorio.ObtenerIdPrestamo(idFormulario);
            return prestamo.IdPrestamo;
        }

        public bool ActualizarFechaPagoFormulario(ActualizarFechaPagoFormularioComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            return _prestamoRepositorio.ActualizarFechaPagoFormulario(comando.IdFormulario, comando.NuevaFecha, comando.FechaFinNueva, usuario.Id);
        }

        public bool RegistrarRechazoReactivacion(RegistrarRechazoReactivacionPrestamoComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            return _prestamoRepositorio.RegistrarRechazoReactivacion(comando.IdPrestamo, comando.MotivosRechazo, comando.NumeroCaja, usuario.Id);
        }

        public bool RegistrarReactivacion(RegistrarReactivacionPrestamoComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            return _prestamoRepositorio.RegistrarReactivacion(comando.IdFormulario,comando.IdPrestamo, comando.Observacion, usuario.Id);
        }

        public PrestamoReactivacionResultado ObtenerDatosPrestamoReactivacion(decimal idPrestamo)
        {
            return _prestamoRepositorio.ObtenerDatosPrestamoReactivacion(idPrestamo);
        }

        public IList<MotivosRechazoPrestamoResultado> ObtenerRechazosPrestamo(decimal idPrestamo)
        {
            var listaMotivos = _prestamoRepositorio.ObtenerRechazosPrestamo(idPrestamo);
            return listaMotivos.OrderBy(x => x.IdSeguimientoPrestamo).ToList();
        }

        public bool ActualizarNumeroCaja(NumeroCajaComando comando)
        {
            var usuario = _sesionUsuario.Usuario;

            return _prestamoRepositorio.ActualizarNumeroCaja(comando.IdFormularioLinea, comando.NumeroCaja, usuario.Id);
        }

        public ArchivoBase64 ObtenExcelBandejaPrestamos(BandejaPrestamosConsulta consulta)
        {
            var datosGrilla = _prestamoRepositorio.ObtenerPrestamosReporte(consulta);

            var reportData = new ReporteBuilder("ReportePrestamo_BandejaExcel.rdlc")
                .AgregarDataSource("DSBandejaPrestamoResultado", datosGrilla)
                .SeleccionarFormato("Excel")
                .Generar();

            return new ArchivoBase64(Convert.ToBase64String(reportData.Content), TipoArchivo.Excel, $"reporte_bandeja_prestamos_excel");
        }


        public ReporteResultado ObtenPDFBandejaPrestamos(BandejaPrestamosConsulta consulta)
        {
            var noAplica = "-";
            var datosGrilla = _prestamoRepositorio.ObtenerPrestamosReporte(consulta);
            var datosCombo = _prestamoRepositorio.ObtenerNombresComboPrestamo(consulta);
            var contadorLocalidad = consulta.LocalidadIds?.Count(x => x == ',') + 1 ?? 0;
            var contadorDepartamento = consulta.DepartamentoIds?.Count(x => x == ',') + 1 ?? 0;
            var contadorEstado = consulta.IdEstadoPrestamo?.Count(x => x  == ',' ) +1 ?? 0;
            var datosConsulta = new InformeBandejaPrestamosConsulta()
            {
                Nombre = consulta.Nombre ?? noAplica,
                Apellido = consulta.Apellido ?? noAplica,
                Cuil = consulta.Cuil ?? noAplica,
                NroFormulario = consulta.NroFormulario ?? noAplica,
                NroPrestamo = consulta.NroPrestamo ?? noAplica,
                NroSticker = consulta.NroSticker ?? noAplica,
                Dni = consulta.Dni ?? noAplica,
                FechaHasta = consulta.FechaHasta.ToString("dd/MM/yyyy") != "" ? consulta.FechaHasta.ToString("dd/MM/yyyy") : noAplica,
                QuiereReactivar = consulta.QuiereReactivar ? "Si" : noAplica,
                FechaDesde = consulta.FechaDesde.ToString("dd/MM/yyyy") != "" ? consulta.FechaDesde.ToString("dd/MM/yyyy") : noAplica,
                Linea = datosCombo.Linea != "" ? datosCombo.Linea : noAplica,
                Usuario = datosCombo.Usuario != "" ? datosCombo.Usuario : noAplica,
                Localidad = consulta.LocalidadIds != null ? contadorLocalidad + " localidades seleccionadas" : noAplica,
                EstadoPrestamo = consulta.IdEstadoPrestamo != null ? contadorEstado + " estados seleccionados" : noAplica,
                Departamento = consulta.DepartamentoIds != null ? contadorDepartamento + " departamentos seleccionados" : noAplica,
                OrigenFormulario = datosCombo.OrigenFormulario != "" ? datosCombo.OrigenFormulario : noAplica,
                TipoPersonaDescripcion = datosCombo.TipoPersonaDescripcion != "" ? datosCombo.TipoPersonaDescripcion : noAplica,
            };

            var nombreReporte = "reporte_bandeja_prestamo_pdf";

            var reportData = new ReporteBuilder("ReportePrestamo_Bandeja.rdlc")
                .AgregarDataSource("DSBandejaPrestamoResultado", datosGrilla)
                .AgregarDataSource("DSBandejaPrestamoConsulta", datosConsulta)

                .Generar();

            return new ReporteResultado(new ArchivoBase64(Convert.ToBase64String(reportData.Content), TipoArchivo.PDF,
                $"reporte_bandeja_prestamos"));
        }

        public DocumentoDescargaResultado GenerarTxtConformarPrestamo(string idAgrupamiento,bool generado)
        {

            var prestamos = _prestamoRepositorio.ObtenerPrestamosArchivoConsulta(idAgrupamiento);

            StringBuilder archivo = new StringBuilder();
            string nombreArchivo = "Préstamos";
            if (generado)
            {
                nombreArchivo += " generados";
                
                archivo.AppendLine("Préstamos generados: \n");

                foreach (var prestamo in prestamos)
                {
                    archivo.AppendLine(
                        $"\n- Nro formulario:{prestamo.NroFormulario} - Apellido y nombre: {prestamo.Apellido}, {prestamo.Nombre} - CUIL: {prestamo.Cuil} - Nro préstamo: {prestamo.NroPrestamo}\n");
                }
            }
            else
            {
                nombreArchivo += " no generados";
                
                archivo.AppendLine("Los siguientes formularios no están en iniciado: \n");

                foreach (var prestamo in prestamos)
                {
                    archivo.AppendLine(
                        $"\n- Nro formulario:{prestamo.NroFormulario} - Apellido y nombre: {prestamo.Apellido}, {prestamo.Nombre} - CUIL: {prestamo.Cuil}\n");
                }
            }

            return new DocumentoDescargaResultado
            {
                Blob = Encoding.UTF8.GetBytes(archivo.ToString()),
                FileName = nombreArchivo + ".txt"
            };
           
        }
    }
}