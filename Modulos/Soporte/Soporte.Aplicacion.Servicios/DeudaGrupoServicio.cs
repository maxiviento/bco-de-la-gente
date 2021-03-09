using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AppComunicacion.ApiModels;
using Core.CiDi.Documentos.Entities.Errores;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using GrupoUnico.Aplicacion.Servicios;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Aplicacion.Servicios;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Reportes;
using Microsoft.SqlServer.Server;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Soporte.Aplicacion.Servicios
{
    public class DeudaGrupoServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IFormularioRepositorio _formularioRepositorio;
        private readonly IDeudaGrupoRepositorio _deudaGrupoRepositorio;
        private readonly IDestinoFondosRepositorio _destinoFondosRepositorio;
        private readonly ILineaPrestamoRepositorio _lineaPrestamoRepositorio;
        private readonly IMotivoRechazoRepositorio _motivoRechazoRepositorio;
        private readonly UsuarioServicio _usuarioServicio;
        private readonly GrupoUnicoServicio _grupoUnicoServicio;
        private readonly DocumentacionBGEUtilServicio _documentacionBgeUtilServicio;
        private readonly int _edadDesde = int.Parse(ParametrosSingleton.Instance.GetValue("6"));
        private readonly int _edadHasta = int.Parse(ParametrosSingleton.Instance.GetValue("5"));

        public DeudaGrupoServicio(ISesionUsuario sesionUsuario, IFormularioRepositorio formularioRepositorio,
            GrupoUnicoServicio grupoUnicoServicio, ILineaPrestamoRepositorio lineaPrestamoRepositorio,
            IDeudaGrupoRepositorio deudaGrupoRepositorio, UsuarioServicio usuarioServ,
            IMotivoRechazoRepositorio motivoRechazoRepositorio, IDestinoFondosRepositorio destinoFondosRepositorio,
            DocumentacionBGEUtilServicio documentacionBgeUtilServicio)
        {
            _sesionUsuario = sesionUsuario;
            _formularioRepositorio = formularioRepositorio;
            _deudaGrupoRepositorio = deudaGrupoRepositorio;
            _usuarioServicio = usuarioServ;
            _motivoRechazoRepositorio = motivoRechazoRepositorio;
            _destinoFondosRepositorio = destinoFondosRepositorio;
            _documentacionBgeUtilServicio = documentacionBgeUtilServicio;
            _grupoUnicoServicio = grupoUnicoServicio;
            _lineaPrestamoRepositorio = lineaPrestamoRepositorio;
        }

        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesDeudaGrupo(DocumentacionConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new DocumentacionConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = 5;

            var resultados = _deudaGrupoRepositorio.ObtenerTodosHistorialesDeudaGrupo(consulta);
            foreach (var resultado in resultados.Elementos)
            {
                resultado.NombreArchivo = $"CONTROLAR DEUDA GRUPO CONVIVIENTE - {resultado.FechaAlta:d}";
                resultado.Extension = "PDF";
            }

            return resultados;
        }

        public string ProcesarDomicilioGrupo(DatosPersonalesResultado datos)
        {
            StringBuilder res = new StringBuilder();
            res.AppendIf(!string.IsNullOrEmpty(datos.Calle), datos.Calle + " ");
            res.AppendIf(!string.IsNullOrEmpty(datos.Numero), datos.Numero + " ");
            res.AppendIf(!string.IsNullOrEmpty(datos.Torre), "Torre: " + datos.Torre);
            res.AppendIf(!string.IsNullOrEmpty(datos.Piso), "Piso: " + datos.Piso);
            res.AppendIf(!string.IsNullOrEmpty(datos.Dpto), "Dpto: " +  datos.Dpto);
            res.AppendIf(!string.IsNullOrEmpty(datos.Manzana), "Manzana: " + datos.Manzana);
            res.AppendIf(!string.IsNullOrEmpty(datos.Barrio), "Barrio: " + datos.Barrio);
            return res.ToString();
        }
        
        public ReporteResultado ObtenerReporteDeudaGrupoConviviente(FiltrosFormularioConsulta cons)
        {
            ConsultaDeudaGrupoConviviente consulta = new ConsultaDeudaGrupoConviviente(cons);
            var erroresConsulta = consulta.Validar();
            var esAlerta = false;
            if (erroresConsulta.Count > 0)
                return new ReporteResultado(erroresConsulta);

            var consultaFormularios = consulta.EsPorDocumento
                ? new FiltrosFormularioConsulta { Dni = consulta.NroDocumento }
                : new FiltrosFormularioConsulta { NroFormulario = cons.NroFormulario };

            IList<FormularioFiltradoResultado> formularios = _formularioRepositorio.ConsultaFormulariosParaDeuda(consultaFormularios);
            var ultimoFormulario = formularios.OrderByDescending(x => x.NroPrestamo).FirstOrDefault();
            if (ultimoFormulario == null)
                return new ReporteResultado(consulta.EsPorDocumento ? "La persona buscada no posee ningún formulario registrado." : "No se encuentran los datos del préstamo buscado.");

            var formulario = ObtenerPorId((int)ultimoFormulario.IdFormulario.Valor);
            string nroPrestamo = ultimoFormulario.NroPrestamo?.ToString() ?? formulario.Id.ToString();

            decimal? idHistorial = null;
            var fechaConsulta = DateTime.Today;

            ConcatenadorPDF concatenador = new ConcatenadorPDF();
            int cont = 1;
            DetalleMotivosRechazoResultado motivosRechazo = new DetalleMotivosRechazoResultado();

            if (!consulta.EsPorDocumento)
            {
                idHistorial = GuardarDatoHistorial(nroPrestamo, _sesionUsuario.Usuario, formulario, fechaConsulta, (int)ultimoFormulario.IdFormulario.Valor);
            }

            // SOLICITANTE
            var datosSolicitante = ObtenerDatosPersonalesCompleto(formulario.Solicitante);

            datosSolicitante.DomicilioDeuda = ProcesarDomicilioGrupo(datosSolicitante);

            var grupoFamiliar = (RespuestaAPIGrupoFamiliar)_grupoUnicoServicio.GetApiConsultaIngresosGrupoFamiliar(
                formulario.Solicitante.SexoId, formulario.Solicitante.NroDocumento,
                formulario.Solicitante.CodigoPais, formulario.Solicitante.IdNumero);

            var ds = ObtenerDSDeudaGrupoFamiliar(grupoFamiliar, motivosRechazo);

            if (ds != null && ds.Count > 0)
            {
                foreach (ReporteDeudaGrupoConvivienteResultado integrante in ds)
                {
                    if (integrante.CantCuotasImpagas != null && decimal.Parse(integrante.CantCuotasImpagas) > 0
                        || integrante.ImpCuotasImpagas != null && decimal.Parse(integrante.ImpCuotasImpagas) > 0)
                    {
                        esAlerta = true;
                    }
                }
            }

            if (cons.IdPrestamoItem != null)
            {
                ActualizarEstadoAlerta(cons.IdPrestamoItem, esAlerta);
            }

            var reportData = GenerarReporteDeudaGrupoConviviente(ultimoFormulario, formulario, datosSolicitante,
            ds, grupoFamiliar.Grupo != null ? grupoFamiliar.Grupo.IdGrupo : null, true, idHistorial, fechaConsulta, ultimoFormulario.IdFormulario.Valor, esAlerta);

            concatenador.AgregarReporte(reportData, $"DeudaGrupoConviviente_{cont}_SOLICITANTE={formulario.Solicitante.NroDocumento}");

            if (formulario.Garantes != null && formulario.Garantes.Count() == 1) //Si es linea individual
            {
                cont++;
                // GARANTES
                foreach (var garante in formulario.Garantes)
                {
                    DatosPersonalesResultado datosGarante = ObtenerDatosPersonalesCompleto(garante);

                    var grupoFamiliarGarante =
                        (RespuestaAPIGrupoFamiliar)_grupoUnicoServicio.GetApiConsultaIngresosGrupoFamiliar(
                            garante.SexoId, garante.NroDocumento, garante.CodigoPais, garante.IdNumero);

                    var dsGarante = ObtenerDSDeudaGrupoFamiliar(grupoFamiliarGarante, motivosRechazo);

                    var reportDataGarante = GenerarReporteDeudaGrupoConviviente(ultimoFormulario, formulario, datosGarante,
                        dsGarante, grupoFamiliarGarante.Grupo != null ? grupoFamiliarGarante.Grupo.IdGrupo : null
                        , false, idHistorial, fechaConsulta, ultimoFormulario.IdFormulario.Valor, false);

                    concatenador.AgregarReporte(reportDataGarante, $"DeudaGrupoConviviente_{cont}_GARANTE={garante.NroDocumento}");
                    cont++;
                }
            }
            if (idHistorial.HasValue)
            {
                ActualizarMotivosRechazoHistorial(idHistorial.Value, motivosRechazo);
            }
            // DETALLE DE MOTIVOS RECHAZOS
            //Reporte rptDetalleMotivosRechazo = GenerarReporteDetalleMotivosRechazo(motivosRechazo);
            //if (rptDetalleMotivosRechazo != null)
            //    concatenador.AgregarReporte(rptDetalleMotivosRechazo, "DetalleMotivosRechazo");

            // MERGE DE REPORTES CON PDFSharp
            var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();

            if (arrayBytesConcatenado == null)
                return new ReporteResultado(concatenador.Errores);
            return new ReporteResultado(_documentacionBgeUtilServicio.GenerarArchivoReporteBGE(arrayBytesConcatenado, TipoArchivo.PDF, "DeudaGrupoConviviente", new Persona { Nombre = datosSolicitante.Nombre, Apellido = datosSolicitante.Apellido },(int) ultimoFormulario.NroFormulario));
        }

        private void ActualizarMotivosRechazoHistorial(decimal idHistorial, DetalleMotivosRechazoResultado motivosRechazo)
        {
            _deudaGrupoRepositorio.ActualizarMotivosRechazoHistorial(idHistorial,
                motivosRechazo.IdsMotivoRechazoString);
        }

        private decimal GuardarDatoHistorial(string nroPrestamo, Usuario usuario, DatosFormularioResultado formulario, DateTime fechaConsulta, decimal idFormularioLinea)
        {
            return _deudaGrupoRepositorio.RegistrarDatoHistorial(nroPrestamo, usuario.Id, formulario.DetalleLinea.LineaId, fechaConsulta, idFormularioLinea);
        }

        private decimal ActualizarEstadoAlerta(decimal? idPrestamoItem, bool esAlerta)
        {
            return _deudaGrupoRepositorio.ActualizarEstadoAlerta(idPrestamoItem, esAlerta);
        }


        private Reporte GenerarReporteDeudaGrupoConviviente(FormularioFiltradoResultado formulario, DatosFormularioResultado datosFormulario,
            DatosPersonalesResultado datos, List<ReporteDeudaGrupoConvivienteResultado> ds, long? idGrupoConviviente,
        bool esSolicitante, decimal? idHistorial, DateTime fechaConsulta, decimal idFormularioLinea, bool esAlerta)
        {
            if (idHistorial.HasValue)
            {
                GuardarHistorial(formulario, datosFormulario, datos, ds, idGrupoConviviente, esSolicitante, (decimal)idHistorial, idFormularioLinea);
            }
            try
            {
                return new ReporteBuilder("ReporteDeudaGrupoConviviente.rdlc")
                    .AgregarDataSource("DSDomicilioActual", ds)
                    .AgregarParametro("Linea", $"{datosFormulario.DetalleLinea.Nombre} - {datosFormulario.DetalleLinea.Descripcion}")
                    .AgregarParametro("NroPrestamo", formulario.NroPrestamo?.ToString() ?? "")
                    .AgregarParametro("Fecha", fechaConsulta.ToString("dd/MM/yyyy"))
                    .AgregarParametro("Usuario", $"{_sesionUsuario.Usuario.Nombre} {_sesionUsuario.Usuario.Apellido}")
                    .AgregarParametro("Nombre", $"{datos.Apellido}, {datos.Nombre}")
                    .AgregarParametro("NroDocumento", datos.NroDocumento)
                    .AgregarParametro("NroSticker", datosFormulario.NroSticker)
                    .AgregarParametro("DomicilioActual", datos.DomicilioDeuda)
                    .AgregarParametro("NroGrupoConviviente", idGrupoConviviente != null ? idGrupoConviviente.ToString() : "")
                    .AgregarParametro("Localidad", datos.DomicilioGrupoFamiliarLocalidad)
                    .AgregarParametro("Departamento", datos.DomicilioGrupoFamiliarDepartamento)
                    .AgregarParametro("esSolicitante", esSolicitante ? "S" : "N")
                    .AgregarParametro("GeneraAlerta", esAlerta ? "S" : "N")
                    .Generar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void GuardarHistorial(FormularioFiltradoResultado formulario, DatosFormularioResultado datosFormulario, DatosPersonalesResultado datos,
            List<ReporteDeudaGrupoConvivienteResultado> ds, long? idGrupoConviviente, bool esSolicitante, decimal idHistorial, decimal idFormularioLinea)
        {
            var idCabecera = _deudaGrupoRepositorio.RegistrarCabeceraHistorial(
                datos.IdNumero, datos.SexoId, datos.CodigoPais, datos.NroDocumento,
                datosFormulario.NroSticker,
                datos.IdVinDomicilio,
                //datos.DomicilioCompleto,
                //idGrupoConviviente?.ToString() ?? "",
                //datos.DomicilioGrupoFamiliarLocalidad,
                //datos.DomicilioGrupoFamiliarDepartamento,
                esSolicitante,
                idHistorial);

            foreach (var detalle in ds)
            {
                _deudaGrupoRepositorio.RegistrarDetalleHistorial(
                    idCabecera,
                    detalle.IdNumero,
                    detalle.IdSexo,
                    detalle.CodPais,
                    //detalle.NombreCompleto,
                    detalle.TipoDocumento,
                    detalle.NumeroDocumento,
                    detalle.Sexo,
                    detalle.FechaNacimiento,
                    detalle.Edad,
                    detalle.NumeroFormulario,
                    detalle.FechaUltimoMovimiento,
                    detalle.PrestamoBeneficio,
                    detalle.Importe,
                    detalle.CantCuotas,
                    detalle.CantCuotasPagas,
                    detalle.CantCuotasImpagas,
                    detalle.CantCuotasVencidas,
                    detalle.MotivoBaja,
                    detalle.IdEstado,
                    detalle.FechaDefuncion,
                    detalle.IdFormulario);
            }
        }

        public DatosFormularioResultado ObtenerPorId(int id)
        {
            var datosBasicos = _formularioRepositorio.ObtenerSolicitante(id);

            if (!string.IsNullOrEmpty(datosBasicos.MotivoRechazoPrestamo))
                datosBasicos.MotivoRechazo = datosBasicos.MotivoRechazo + " - " + datosBasicos.MotivoRechazoPrestamo;

            var formularioResultado = new DatosFormularioResultado
            {
                Id = id,
                IdOrigen = datosBasicos.IdOrigen,
                IdEstado = datosBasicos.IdEstado,
                DetalleLinea = _lineaPrestamoRepositorio.DetalleLineaParaFormulario(datosBasicos.Id),
                Solicitante = new DatosPersonaResultado
                {
                    CodigoPais = datosBasicos.CodigoPais,
                    IdNumero = datosBasicos.IdNumero,
                    NroDocumento = datosBasicos.NroDocumento,
                    SexoId = datosBasicos.SexoId
                },
                DestinosFondos = ObtenerDestinoFondosPorFormulario(id),
                CondicionesSolicitadas = _formularioRepositorio.ObtenerCondicionesDePrestamoPorFormularioApoderado(id),
                Garantes = _formularioRepositorio.ObtenerGarantes(id),
                NroSticker = datosBasicos.NroSticker
            };
            return formularioResultado;
        }

        private DatosPersonalesResultado ObtenerDatosPersonalesCompleto(DatosPersonaResultado persona)
        {
            var resultado =
                _grupoUnicoServicio.GetApiConsultaDatosPersonales(persona.SexoId, persona.NroDocumento,
                    persona.CodigoPais, persona.IdNumero);

            return resultado;
        }

        private OpcionDestinosFondoResultado ObtenerDestinoFondosPorFormulario(int idFormulario)
        {
            var destinoFondosCargados = _destinoFondosRepositorio.ConsultarDestinosFondosPorFormulario(idFormulario);
            var opcionDestinoFondos = new OpcionDestinosFondoResultado
            {
                DestinosFondo = new List<DestinoFondoResultado>()
            };
            foreach (var destinoFondoSeleccionadoResultado in destinoFondosCargados)
            {
                opcionDestinoFondos.DestinosFondo.Add(new DestinoFondoResultado
                {
                    Id = destinoFondoSeleccionadoResultado.Id.Valor
                });
                if (destinoFondoSeleccionadoResultado.Nombre.Equals("OTROS"))
                    opcionDestinoFondos.Observaciones = destinoFondoSeleccionadoResultado.Observaciones;
            }
            return opcionDestinoFondos;
        }

        public List<ReporteDeudaGrupoConvivienteResultado> ObtenerDSDeudaGrupoFamiliar(RespuestaAPIGrupoFamiliar grupoFamiliar, DetalleMotivosRechazoResultado motivosRechazo)
        {
            List<GrupoFamiliarResultado> dsGrupoFamiliar = new List<GrupoFamiliarResultado>();
            if (grupoFamiliar.Grupo != null)
            {
                foreach (var integrante in grupoFamiliar.Grupo.Integrantes)
                {
                    GrupoFamiliarResultado persona = new GrupoFamiliarResultado(integrante);
                    dsGrupoFamiliar.Add(persona);
                }
            }
            else
            {
                GrupoFamiliarResultado persona = new GrupoFamiliarResultado();
                persona.NroDocumento = grupoFamiliar.Persona.NroDocumento;
                persona.NombreCompleto = grupoFamiliar.Persona.NombreCompleto;
                persona.Sexo = grupoFamiliar.Persona.Sexo.NombreCorto;
                persona.TipoDocumento = grupoFamiliar.Persona.TipoDocumento.Id;
                persona.FechaNacimiento = grupoFamiliar.Persona.FechaNacimiento?.ToString("dd/MM/yyyy") ?? "";
                persona.FechaDefuncion = grupoFamiliar.Persona.FechaDefuncion?.ToString("dd/MM/yyyy") ?? "";
                if (!string.IsNullOrEmpty(persona.FechaNacimiento))
                {
                    persona.Edad = Edad(grupoFamiliar.Persona);
                }

                dsGrupoFamiliar.Add(persona);
            }

            var ds = new List<ReporteDeudaGrupoConvivienteResultado>();
            foreach (var persona in dsGrupoFamiliar)
            {
                var consultaFormularios = new FiltrosFormularioConsulta { Dni = persona.NroDocumento };
                IList<FormularioFiltradoResultado> formularios = _formularioRepositorio.ConsultaFormulariosParaDeuda(consultaFormularios);
                //Si no posee formularios registrados muestra datos por defecto de la persona
                if (formularios.Count == 0)
                {
                    ds.Add(new ReporteDeudaGrupoConvivienteResultado(persona));
                    continue;
                }

                //Si posee formularios registrados se cargan sus datos
                foreach (var formulario in formularios.OrderByDescending(x => x.NroPrestamo))
                {
                    DatosFormularioResultado datosFormulario = ObtenerPorId((int)formulario.IdFormulario.Valor);
                    var datosDeuda = ConsultarDatosDeudaFormulario((int)formulario.IdFormulario.Valor);
                    motivosRechazo.NuevoMotivoRechazo(datosDeuda.MotivosRechazo);
                    datosDeuda.ImporteCuota = datosFormulario.CondicionesSolicitadas.MontoEstimadoCuota;

                    ds.Add(new ReporteDeudaGrupoConvivienteResultado
                    {
                        IdNumero = persona.IdNumero,
                        IdSexo = persona.IdSexo,
                        CodPais = persona.CodPais,
                        NombreCompleto = persona.NombreCompleto,
                        TipoDocumento = persona.TipoDocumento,
                        NumeroDocumento = persona.NroDocumento,
                        Sexo = persona.Sexo,
                        FechaNacimiento = persona.FechaNacimiento,
                        FechaDefuncion = persona.FechaDefuncion,
                        FechaUltimoMovimiento = formulario.FechaUltimoMovimiento.HasValue? formulario.FechaUltimoMovimiento.GetValueOrDefault().ToString("dd/MM/yyyy"): "",
                        Edad = persona.Edad,
                        NumeroFormulario = formulario.NroFormulario.ToString(),
                        PrestamoBeneficio = datosFormulario.DetalleLinea.Descripcion + $" ({datosFormulario.DetalleLinea.Nombre}) ",
                        Importe = datosFormulario.CondicionesSolicitadas.MontoSolicitado.ToString(),
                        CantCuotas = datosFormulario.CondicionesSolicitadas.CantidadCuotas.ToString(),
                        IdFormulario = datosFormulario.Id.ToString()
                    }.CargarDatosDeuda(datosDeuda));
                }
            }

            var docIntegrante = grupoFamiliar.Persona.NroDocumento;
            var dsPrincipal = ds.Where(x => x.NumeroDocumento == docIntegrante);
            var dsRestoGrupo = ds.Where(x => x.NumeroDocumento != docIntegrante).OrderBy(x => x.NombreCompleto);
            var res = dsPrincipal.Concat(dsRestoGrupo).ToList();
            return res;
        }

        private string AgruparDestinosFondos(List<DestinoFondoResultado> destinosFondos)
        {
            string res = "";
            if (destinosFondos == null || destinosFondos.Count == 0) return res;

            var destinosFondosPosibles = _destinoFondosRepositorio.ConsultarDestinosFondos();
            for (int i = 0; i < destinosFondos.Count; i++)
            {
                var destino = destinosFondosPosibles.FirstOrDefault(x => x.Id.Valor == destinosFondos[i].Id);
                if (destino != null)
                {
                    res += destino.Descripcion;
                    if (i != destinosFondos.Count - 1) res += ", ";
                }
            }

            return res;
        }

        public string Edad(PersonaUnica persona)
        {
            var edadString = "";
            if (persona.FechaNacimiento.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - persona.FechaNacimiento.Value.Year;
                if (persona.FechaNacimiento > today.AddYears(-age)) age--;
                edadString = age.ToString();
            }
            return edadString;
        }

        private ConsultaDeudaFormularioResultado ConsultarDatosDeudaFormulario(int idFormulario)
        {
            var consulta = _formularioRepositorio.ObtenerDatosDeudaFormulario(idFormulario);
            int elementos = consulta.ToList().Count;
            if (elementos == 0) return new ConsultaDeudaFormularioResultado();
            if (elementos == 1)
            {
                consulta[0].MotivosRechazo = new List<int>();
                if (consulta[0].MotivoRechazo != 0)
                    consulta[0].MotivosRechazo.Add(consulta[0].MotivoRechazo);
                return consulta[0];
            }
            ConsultaDeudaFormularioResultado res = consulta[0];
            res.MotivosRechazo = new List<int>();
            foreach (var x in consulta)
            {
                if (x.MotivoRechazo != 0)
                    res.MotivosRechazo.Add(x.MotivoRechazo);
            }
            return res;
        }

        private Reporte GenerarReporteDetalleMotivosRechazo(DetalleMotivosRechazoResultado motivos)
        {
            if (motivos.IdsMotivosRechazo.Count == 0) return null;
            List<ReporteDetalleMotivosRechazoResultado> ds = new List<ReporteDetalleMotivosRechazoResultado>();

            foreach (var id in motivos.IdsMotivosRechazo.OrderBy(x => x).ToList())
            {
                var motivo = _motivoRechazoRepositorio.ConsultarPorId(new Id(id), Ambito.FORMULARIO);
                if (motivo != null)
                    ds.Add(new ReporteDetalleMotivosRechazoResultado((int)motivo.Id.Valor, motivo.Nombre, motivo.Descripcion, motivo.Abreviatura));
            }

            return new ReporteBuilder("ReporteDetalleMotivosRechazo.rdlc")
                .AgregarDataSource("DSDetalleMotivosRechazo", ds)
                .Generar();
        }

        public string ObtenerHistorialDeudaGrupo(decimal idDocumento, string hash)
        {
            var datoHistorial = _deudaGrupoRepositorio.ObtenerDatoHistorialDeudaGrupo(idDocumento);
            var usuario = _usuarioServicio.ObtenerUsuarioPorId(new Id(datoHistorial.IdUsuario));
            var cabeceras = _deudaGrupoRepositorio.ObtenerCabeceraHistorialDeudaGrupo(datoHistorial.Id.Valor);


            DetalleMotivosRechazoResultado motivosRechazo = new DetalleMotivosRechazoResultado();

            ConcatenadorPDF concatenador = new ConcatenadorPDF();
            int cont = 1;
            var esAlerta = false;
            foreach (var cabecera in cabeceras)
            {
                var detalles = _deudaGrupoRepositorio.ObtenerDetalleHistorialDeuda(cabecera.Id);

                foreach (var detalle in detalles)
                {
                    if (!string.IsNullOrEmpty(detalle.CantCuotas) && !string.IsNullOrEmpty(detalle.Importe))
                    {
                        detalle.ImporteCuota = (decimal.Parse(detalle.Importe) / decimal.Parse(detalle.CantCuotas)).ToString("#.##");

                    }
                    if (!string.IsNullOrEmpty(detalle.CantCuotasImpagas) && decimal.Parse(detalle.CantCuotasImpagas) > 0
                        || !string.IsNullOrEmpty(detalle.ImpCuotasImpagas) && decimal.Parse(detalle.ImpCuotasImpagas) > 0)
                    {
                        esAlerta = true;
                    }
                    if (detalle.Sexo == "01") detalle.Sexo = "M";
                    if (detalle.Sexo == "02") detalle.Sexo = "F";
                    if (!string.IsNullOrEmpty(detalle.FechaNacimiento))
                    {
                        detalle.FechaNacimiento = DateTime.Parse(detalle.FechaNacimiento).ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(detalle.FechaUltimoMovimiento))
                    {
                        detalle.FechaUltimoMovimiento = DateTime.Parse(detalle.FechaUltimoMovimiento).ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(detalle.FechaDefuncion))
                    {
                        detalle.FechaDefuncion = DateTime.Parse(detalle.FechaDefuncion).ToString("dd/MM/yyyy");
                    }

                    if (!string.IsNullOrEmpty(detalle.CantCuotasImpagas) && !detalle.CantCuotasImpagas.Equals("0"))
                    {
                        detalle.ImpCuotasImpagas =
                            RedondearImportes(decimal.Parse(detalle.ImporteCuota) * decimal.Parse(detalle.CantCuotasImpagas));
                    }

                    if (!string.IsNullOrEmpty(detalle.CantCuotasPagas) && !detalle.CantCuotasPagas.Equals("0"))
                    {
                        detalle.ImpCuotasPagas =
                            RedondearImportes(decimal.Parse(detalle.ImporteCuota) * decimal.Parse(detalle.CantCuotasPagas));
                    }

                    if (!string.IsNullOrEmpty(detalle.CantCuotasVencidas) && !detalle.CantCuotasVencidas.Equals("0"))
                    {
                        detalle.ImpCuotasVencidas =
                            RedondearImportes(decimal.Parse(detalle.ImporteCuota) * decimal.Parse(detalle.CantCuotasVencidas));
                    }
                }

                var detalleIntegrante = detalles.Where(x => x.NumeroDocumento == cabecera.NroDocumento);
                var detalleGrupo = detalles.Where(x => x.NumeroDocumento != cabecera.NroDocumento)
                    .OrderBy(x => x.NombreCompleto);

                var reportData = GenerarHistorialDeudaGrupoConviviente(datoHistorial, usuario, cabecera, detalleIntegrante.Concat(detalleGrupo).ToList(), esAlerta);
                concatenador.AgregarReporte(reportData,
                    $"DeudaGrupoConviviente_{cont}");
                cont++;
            }

            motivosRechazo.NuevoMotivoRechazo(datoHistorial.ListadoMotivosRechazo);
            Reporte rptDetalleMotivosRechazo = GenerarReporteDetalleMotivosRechazo(motivosRechazo);
            if (rptDetalleMotivosRechazo != null)
                concatenador.AgregarReporte(rptDetalleMotivosRechazo, "DetalleMotivosRechazo");
            //TODO: MERGE DE REPORTES CON PDFSharp
            var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();
            //string nombreArchivo = "ReporteDeudaGrupoConviviente_" + datoHistorial.NroPrestamo;
            string url = Convert.ToBase64String(arrayBytesConcatenado);
            return $"data:application/pdf;base64,{url}";
        }

        private string RedondearImportes(decimal importe)
        {
            double decimales = (double)importe % 1;
            string res = "";

            if (decimales < 0.5)
            {
                res = Math.Round(importe).ToString();
            }

            if (decimales >= 0.5)
            {
                res = Math.Ceiling(importe).ToString();
            }

            return res;
        }

        private Reporte GenerarHistorialDeudaGrupoConviviente(DatoHistorialDeuda datoHistorial, UsuarioResultado usuario, CabeceraHistorialDeudaGrupo cabecera, List<ReporteDeudaGrupoConvivienteResultado> detalles, bool esAlerta)
        {
            return new ReporteBuilder("ReporteDeudaGrupoConviviente.rdlc")
                .AgregarDataSource("DSDomicilioActual", detalles)
                .AgregarParametro("Linea", datoHistorial.NombreLinea)
                .AgregarParametro("NroPrestamo", datoHistorial.NroPrestamo)
                .AgregarParametro("Fecha", datoHistorial.Fecha.ToString("dd/MM/yyyy"))
                .AgregarParametro("Usuario", usuario.Nombre + " " + usuario.Apellido)
                .AgregarParametro("Nombre", cabecera.NombreCompleto)
                .AgregarParametro("NroDocumento", cabecera.NroDocumento)
                .AgregarParametro("NroSticker", cabecera.NroSticker)
                .AgregarParametro("DomicilioActual", cabecera.Domicilio)
                .AgregarParametro("NroGrupoConviviente", cabecera.NroGrupoConviviente)
                .AgregarParametro("Localidad", cabecera.Localidad)
                .AgregarParametro("Departamento", cabecera.Departamento)
                .AgregarParametro("esSolicitante", cabecera.EsSolicitante ? "S" : "N")
                .AgregarParametro("GeneraAlerta", esAlerta ? "S" : "N")
                .Generar();
        }
    }
}