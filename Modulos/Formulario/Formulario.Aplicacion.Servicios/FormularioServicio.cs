using System;
using System.CodeDom.Compiler;
using Configuracion.Dominio.IRepositorio;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppComunicacion.ApiModels;
using GrupoUnico.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Reportes;
using Soporte.Aplicacion.Servicios;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;
using static System.Int32;
using EmprendimientoComando = Formulario.Aplicacion.Comandos.EmprendimientoComando;

namespace Formulario.Aplicacion.Servicios
{
    public class FormularioServicio
    {
        private readonly IFormularioRepositorio _formularioRepositorio;
        private readonly ILineaPrestamoRepositorio _lineaPrestamoRepositorio;
        private readonly ICursoRepositorio _cursoRepositorio;
        private readonly IDestinoFondosRepositorio _destinoFondosRepositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly IMotivoRechazoRepositorio _motivoRechazoRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly GrupoUnicoServicio _grupoUnicoServicio;
        private readonly PrestamoServicio _prestamoServicio;
        private readonly IParametrosRepositorio _parametrosRepositorio;
        private readonly IEmprendimientoRepositorio _emprendimientoRepositorio;
        private readonly DeudaEmprendimientoServicio _deudaEmprendimientoServicio;
        private readonly InversionEmprendimientoServicio _inversionEmprendimientoServicio;
        private readonly DetalleInversionEmprendimientoServicio _detalleInversionEmprendimientoServicio;
        private readonly DocumentacionBGEUtilServicio _documentacionBgeUtilServicio;

        public FormularioServicio(IFormularioRepositorio formularioRepositorio,
            ILineaPrestamoRepositorio lineaPrestamoRepositorio, ICursoRepositorio cursoRepositorio,
            IDestinoFondosRepositorio destinoFondosRepositorio, IMotivoBajaRepositorio motivoBajaRepositorio,
            IMotivoRechazoRepositorio motivoRechazoRepositorio, ISesionUsuario sesionUsuario,
            GrupoUnicoServicio grupoUnicoServicio, PrestamoServicio prestamoServicio,
            IParametrosRepositorio parametrosRepositorio,
            IEmprendimientoRepositorio emprendimientoRepositorio,
            DeudaEmprendimientoServicio deudaEmprendimientoServicio,
            InversionEmprendimientoServicio inversionEmprendimientoServicio,
            DetalleInversionEmprendimientoServicio detalleInversionEmprendimientoServicio)
        {
            _formularioRepositorio = formularioRepositorio;
            _lineaPrestamoRepositorio = lineaPrestamoRepositorio;
            _cursoRepositorio = cursoRepositorio;
            _destinoFondosRepositorio = destinoFondosRepositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _sesionUsuario = sesionUsuario;
            _motivoRechazoRepositorio = motivoRechazoRepositorio;
            _grupoUnicoServicio = grupoUnicoServicio;
            _prestamoServicio = prestamoServicio;
            _parametrosRepositorio = parametrosRepositorio;
            _emprendimientoRepositorio = emprendimientoRepositorio;
            _deudaEmprendimientoServicio = deudaEmprendimientoServicio;
            _inversionEmprendimientoServicio = inversionEmprendimientoServicio;
            _detalleInversionEmprendimientoServicio = detalleInversionEmprendimientoServicio;
        }

        public IdFormularioAgrupamiento Registrar(RegistrarFormularioComando comando)
        {
            if (comando.Solicitante.NroDocumento == null && comando.Solicitante.CodigoPais == null && comando.Solicitante.SexoId == null)
            {
                foreach (var s in comando.Integrantes)
                {
                    if (s.Solicitante)
                    {
                        comando.Solicitante = new DatosPersonaComando
                        {
                            CodigoPais = s.Pais,
                            NroDocumento = s.NroDocumento,
                            SexoId = s.Sexo,
                            IdNumero = s.IdNumero
                        };
                    }
                }
            }
            var detalleLineaPrestamo = new DetalleLineaPrestamo { Id = new Id(comando.DetalleLinea.Id) };
            var solicitante = ObtenerPersona(comando.Solicitante);
            var origenFormulario = OrigenFormulario.ConId(comando.IdOrigen);
            var usuario = _sesionUsuario.Usuario;
            var formulario = new Dominio.Modelo.Formulario(new Id(comando.Id.GetValueOrDefault()), detalleLineaPrestamo,
                solicitante, origenFormulario, usuario, comando.FechaForm);
            if (!comando.DetalleLinea.Apoderado)
            {
                formulario.EsApoderado = (int)TipoApoderadoEnum.NoPerteneceLineaApoderada;
            }
            _formularioRepositorio.Registrar(formulario);

            var idAgrupamiento = _formularioRepositorio.ObtenerAgrupamientoPorFormulario(formulario.Id.Valor);
            if (idAgrupamiento == 0)
            {
                idAgrupamiento = _formularioRepositorio.AgruparFormularios(formulario.Id.ToString(), usuario.Id)
                    .Valor;
            }
            var idFormAgrup = new IdFormularioAgrupamiento()
            {
                IdFormulario = formulario.Id.Valor,
                IdAgrupamiento = idAgrupamiento
            };
            return idFormAgrup;
        }

        public IList<FormularioXDni> RegistrarFormularios(RegistrarFormularioComando comando)
        {
            var formulariosId = new List<FormularioXDni>();
            var usuario = _sesionUsuario.Usuario;

            foreach (var integrante in comando.Integrantes)
            {
                if (comando.DetalleLinea.Apoderado)
                {
                    var apoderado = integrante.EsApoderado.GetValueOrDefault();
                    if (apoderado != (int) TipoApoderadoEnum.EsApoderado)
                    {
                        integrante.EsApoderado = (int) TipoApoderadoEnum.PertenecePeroNoApoderado;
                    }
                }
                else
                {
                    integrante.EsApoderado = (int) TipoApoderadoEnum.NoPerteneceLineaApoderada;
                }
                var detalleLineaPrestamo = new DetalleLineaPrestamo { Id = new Id(comando.DetalleLinea.Id) };
                var solicitante = ObtenerPersona(new DatosPersonaComando()
                {
                    SexoId = integrante.Sexo,
                    CodigoPais = integrante.Pais,
                    NroDocumento = integrante.NroDocumento,
                    IdNumero = integrante.IdNumero
                });
                var origenFormulario = OrigenFormulario.ConId(comando.IdOrigen);
                var formulario = new Dominio.Modelo.Formulario(new Id(integrante.IdFormulario.GetValueOrDefault()),
                    detalleLineaPrestamo,
                    solicitante, origenFormulario, usuario, comando.FechaForm);
                formulario.EsApoderado = integrante.EsApoderado;

                _formularioRepositorio.Registrar(formulario);

                integrante.IdFormulario = (int?)formulario.Id.Valor;

                var actualizacion = _formularioRepositorio.ActualizarAgrupamientoFormularios(
                    comando.IdAgrupamiento,
                    formulario.Id.ToString(),
                    usuario.Id);

                if (actualizacion.Error.Length > 0)
                {
                    throw new AgrupamientoException(actualizacion.Error);
                }

                formulariosId.Add(new FormularioXDni()
                {
                    IdFormulario = formulario.Id,
                    NroDocumento = integrante.NroDocumento
                });
            }


            return formulariosId;
        }

        public BandejaCargaNumeroControlInternoResultado ConsultarBandejaCargaNumeroControlInterno(
            int idFormularioLinea)
        {
            return _formularioRepositorio.ObtenerFormulariosPorPrestamo(idFormularioLinea);
        }

        public IList<GrupoFamiliarDomicilioResultado> ConsultaSituacionDomicilioIntegrante(
            int idFormularioLinea)
        {

            List<GrupoFamiliarDomicilioResultado> listadoGrupos = new List<GrupoFamiliarDomicilioResultado>();
            IList<SituacionDomicilioIntegranteResultado> lsIntegrantes = _formularioRepositorio.ObtenerSituacionDomicilioIntegrante(idFormularioLinea);

            foreach (var integrante in lsIntegrantes)
            {
                if (listadoGrupos.Find(x => x.IdGrupoUnico == integrante.IdGrupoUnico) == null)
                {
                    GrupoFamiliarDomicilioResultado grupo = new GrupoFamiliarDomicilioResultado();
                    grupo.IdGrupoUnico = integrante.IdGrupoUnico;
                    List<SituacionDomicilioIntegranteResultado> lsPersonas = new List<SituacionDomicilioIntegranteResultado>();
                    grupo.ListadoPersonas = lsPersonas;
                    grupo.ListadoPersonas.Add(integrante);
                    listadoGrupos.Add(grupo);
                }
                else
                {
                    listadoGrupos.Find(x => x.IdGrupoUnico == integrante.IdGrupoUnico).ListadoPersonas.Add(integrante);
                }
            }


            return listadoGrupos;
        }

        public DatosDomicilioIntegranteResultado ConsultaDatosDomicilioIntegrante(
            int idFormularioLinea)
        {
            return _formularioRepositorio.ConsultaDatosDomicilioIntegrante(idFormularioLinea);
        }

        public DatosFormularioResultado ObtenerCondicionesDePrestamoPorFormulario(int id)
        {
            var datosBasicos = _formularioRepositorio.ObtenerSolicitante(id);
            var formularioResultado = new DatosFormularioResultado
            {
                Id = id,
                IdOrigen = datosBasicos.IdOrigen,
                IdEstado = datosBasicos.IdEstado,
                DetalleLinea = _lineaPrestamoRepositorio.DetalleLineaParaFormulario(datosBasicos.Id),
                CondicionesSolicitadas = _formularioRepositorio.ObtenerCondicionesDePrestamoPorFormulario(id),
            };
            return formularioResultado;
        }

        public DatosFormularioResultado ObtenerPorId(int id)
        {
            var datosBasicos = _formularioRepositorio.ObtenerSolicitante(id);

            var idAgrupamiento = _formularioRepositorio.ObtenerAgrupamientoPorFormulario(id);

            if (!string.IsNullOrEmpty(datosBasicos.MotivoRechazoPrestamo))
                datosBasicos.MotivoRechazo = datosBasicos.MotivoRechazo + " - " + datosBasicos.MotivoRechazoPrestamo;

            var datosEmprendimiento = _formularioRepositorio.ObtenerDatosEmprendimiento(id);


            var garantes = _formularioRepositorio.ObtenerGarantes(id);

            //TODO: Añadir método en el repositorio que traiga los datos de la ONG para un cuadrante específico.

            var ongDeFormulario = _formularioRepositorio.ObtenerOngDeFormulario(id);
            if (ongDeFormulario != null)
            {
                ongDeFormulario.IdOng = ongDeFormulario.IdOngLinea;
            }

            foreach (var garante in garantes)
            {
                var datosContacto = _formularioRepositorio.ObtenerDatosContacto(new DatosPersonaConsulta { NroDocumento = garante.NroDocumento, CodigoPais = garante.CodigoPais, IdSexo = garante.SexoId, IdNumero = garante.IdNumero == null ? "0" : garante.IdNumero.ToString() });
                garante.SetDatosContacto(datosContacto);
            }


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
                SolicitudesCurso = ObtenerCursosPorFormulario(id),
                DestinosFondos = ObtenerDestinoFondosPorFormulario(id),
                CondicionesSolicitadas = _formularioRepositorio.ObtenerCondicionesDePrestamoPorFormulario(id),
                Garantes = garantes,
                DatosEmprendimiento = datosEmprendimiento,
                MotivoRechazo = datosBasicos.MotivoRechazo.Replace(",;", " ").Replace(';', ' ').Replace(',', ' '),
                Observaciones = datosBasicos.Observaciones,
                NroSticker = datosBasicos.NroSticker,
                ObservacionPrestamo = datosBasicos.ObservacionPrestamo,
                NombreSucursalBancaria = datosBasicos.NombreSucursalBancaria,
                DomicilioSucursalBancaria = datosBasicos.DomicilioSucursalBancaria,
                FechaInicioPagos = datosBasicos.FechaInicioPagos,
                FechaPrimerVencimientoPago = datosBasicos.FechaPrimerVencimientoPago,
                FechaVencimientoPlanPago = datosBasicos.FechaVencimientoPlanPago,
                PatrimonioSolicitante = ObtenerPatrimonioSolicitante(id),
                IngresosGrupo = ObtenerIngresosGrupoFamiliarFormulario(id),
                MiembrosEmprendimiento = datosEmprendimiento == null ? null : ObtenerMiembrosEmprendimiento(datosEmprendimiento.Id),
                DeudasEmprendimiento = datosEmprendimiento == null ? null : _deudaEmprendimientoServicio.ObtenerDeudasEmprendimientoPorIdEmprendimiento(datosEmprendimiento.Id),
                MercadoComercializacion = datosEmprendimiento == null ? null : ObtenerMercadoComercializacion(id),
                InversionesRealizadas = datosEmprendimiento == null ? null : _detalleInversionEmprendimientoServicio.ObtenerInversionesRealizadasPorIdEmprendimiento(datosEmprendimiento.Id),
                NecesidadInversion = datosEmprendimiento == null ? null : _inversionEmprendimientoServicio.ObtenerNecesidadInversionPorIdEmprendimiento(datosEmprendimiento.Id),
                PrecioVenta = datosEmprendimiento == null ? null : ObtenerPrecioVenta(datosEmprendimiento.Id.Valor),
                IdAgrupamiento = idAgrupamiento,
                FechaForm = datosBasicos.FechaForm.GetValueOrDefault(),
                Integrantes = ObtenerFormulariosConAgrupamiento((int)idAgrupamiento),
                DatosONG = ongDeFormulario,
                NumeroCaja = datosBasicos.NumeroCaja,
                TipoApoderado = datosBasicos.TipoApoderado,
                Destino = datosEmprendimiento == null ? "CONSUMO" : datosEmprendimiento.Destino
            };

            return formularioResultado;
        }

        public Resultado<FormularioGrillaResultado> ObtenerFormulariosPorFiltros(FormularioGrillaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new FormularioGrillaConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));
            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            return _formularioRepositorio.ObtenerFormulariosPorFiltros(consulta);
        }
        public string ObtenerTotaliziadorFormularios(FormularioGrillaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new FormularioGrillaConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));
            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            return _formularioRepositorio.ObtenerTotalizadorFormularios(consulta);
        }

        public decimal RegistrarEnvio(RegistrarFormularioComando comando)
        {
            return RegistrarCambioDeEstado(comando, EstadoFormulario.Completado.Id.Valor);
        }

        public decimal RegistrarSeguimientoAuditoria(SeguimientoAuditoriaComando comando)
        {
            comando.IdUsuario = _sesionUsuario.Usuario.Id.Valor;
            return _formularioRepositorio.RegistrarSeguimientoAuditoria(comando);
        }

        public decimal RegistrarInicio(RegistrarFormularioComando comando)
        {
            return RegistrarCambioDeEstado(comando, EstadoFormulario.Iniciado.Id.Valor);
        }

        public decimal RegistrarRechazoFormulario(RechazarFormularioComando comando)
        {
            if (comando.MotivosRechazo == null || comando.MotivosRechazo.Count <= 0)
                throw new ModeloNoValidoException("Debe seleccionar al menos un motivo de rechazo");

            var usuario = _sesionUsuario.Usuario;

            var formulario = new Dominio.Modelo.Formulario(new Id(comando.IdFormulario), comando.NumeroCaja, usuario);

            return _formularioRepositorio.Rechazar(formulario, comando.MotivosRechazo);
        }

        public decimal RegistrarRechazoFormularioConPrestamo(RechazarFormularioComando comando)
        {
            if (comando.MotivosRechazo == null || comando.MotivosRechazo.Count <= 0)
                throw new ModeloNoValidoException("Debe seleccionar al menos un motivo de rechazo");

            var usuario = _sesionUsuario.Usuario;

            var formulario = new Dominio.Modelo.Formulario(new Id(comando.IdFormulario), comando.NumeroCaja, usuario);

            return this._formularioRepositorio.RechazarFormularioConPrestamo(comando.IdFormulario, comando.EsAsociativa,
                usuario.Id.Valor, comando.NumeroCaja, comando.MotivosRechazo);
        }

        public decimal DarDeBajaFormulario(RechazarFormularioComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            var _motivoBaja = _motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja ?? 0));
            var formulario = new Dominio.Modelo.Formulario(new Id(comando.IdFormulario), _motivoBaja, usuario);

            var id = _formularioRepositorio.DarDeBaja(formulario);
            _formularioRepositorio.RegistrarCambioDeEstado(comando.IdFormulario, EstadoFormulario.Eliminado.Id.Valor, default(decimal),
                usuario.Id.Valor);

            return id;
        }

        public void RechazarGrupoFormularios(RechazarFormularioComando comando)
        {
            if (comando.MotivosRechazo == null || comando.MotivosRechazo.Count <= 0)
            {
                throw new ModeloNoValidoException("Debe seleccionar al menos un motivo de rechazo");
            }

            var usuario = _sesionUsuario.Usuario;

            var formulario = new Dominio.Modelo.Formulario(new Id(comando.IdFormulario), comando.NumeroCaja, usuario);

            _formularioRepositorio.Rechazar(formulario, comando.MotivosRechazo);

        }

        private decimal RegistrarCambioDeEstado(RegistrarFormularioComando comando, decimal idEstado)
        {
            var idFormulario = Registrar(comando).IdFormulario;
            var usuario = _sesionUsuario.Usuario;
            _formularioRepositorio.RegistrarCambioDeEstado(idFormulario, idEstado, comando.IdAgrupamiento.Valor, usuario.Id.Valor);
            return idFormulario;
        }

        private static Persona ObtenerPersona(DatosPersonaComando comando)
        {
            return new Persona
            {
                NroDocumento = comando.NroDocumento,
                CodigoPais = comando.CodigoPais,
                SexoId = comando.SexoId,
                IdNumero = comando.IdNumero
            };
        }

        public void ActualizarCursos(int idFormulario, IList<SolicitudCursoComando> comando)
        {
            foreach (var solicitudCursoComando in comando)
            {
                solicitudCursoComando.Descripcion = solicitudCursoComando.Descripcion?.Replace("|", "") ?? null;
            }

            var cursosSolicitados = ObtenerCursosSeleccionados(comando);
            _formularioRepositorio.RegistrarCursos(idFormulario, _sesionUsuario.Usuario.Id.Valor, cursosSolicitados);
        }
        public void ActualizarCursosAsociativas(int idAgrupamiento, IList<SolicitudCursoComando> comando)
        {
            foreach (var solicitudCursoComando in comando)
            {
                solicitudCursoComando.Descripcion = solicitudCursoComando.Descripcion?.Replace("|", "") ?? null;
            }
            var cursosSolicitados = ObtenerCursosSeleccionados(comando);
            var idFormularios = _formularioRepositorio.ObtenerIdsFormulariosAgrupamiento(idAgrupamiento);

            foreach (var idFormulario in idFormularios)
            {
                _formularioRepositorio.RegistrarCursos(idFormulario.Id, _sesionUsuario.Usuario.Id.Valor, cursosSolicitados);
            }
        }

        private IList<SolicitudCurso> ObtenerCursosSeleccionados(IList<SolicitudCursoComando> comando)
        {
            if (comando == null || comando.Count == 0) return null;
            var todosLosCursos = _cursoRepositorio.Consultar().ToList();
            return (from solicitudCurso in comando
                    let cursosSeleccionados =
                    solicitudCurso.Cursos.Select(curso => todosLosCursos.First(c => c.Id.Valor == curso.Id)).ToList()
                    select new SolicitudCurso(cursosSeleccionados, solicitudCurso.Descripcion)).ToList();
        }

        public void ActualizarDestinosFondos(int idFormulario, OpcionDestinoFondosComando comando)
        {
            comando.Observaciones = comando.Observaciones.Replace("|", "");
            var opcionDestinosFondos = ObtenerDestinoFondos(comando);
            _formularioRepositorio.RegistrarDestinoFondos(idFormulario, _sesionUsuario.Usuario.Id.Valor,
                opcionDestinosFondos);
        }

        public void ActualizarDestinosFondosAsociativas(int idAgrupamiento, OpcionDestinoFondosComando comando)
        {
            if (!string.IsNullOrEmpty(comando.Observaciones))
            {
                comando.Observaciones = comando.Observaciones.Replace("|", "");
            }
            var opcionDestinosFondos = ObtenerDestinoFondos(comando);
            var idFormularios = _formularioRepositorio.ObtenerIdsFormulariosAgrupamiento(idAgrupamiento);
            foreach (var idFormulario in idFormularios)
            {
                _formularioRepositorio.RegistrarDestinoFondos(idFormulario.Id, _sesionUsuario.Usuario.Id.Valor, opcionDestinosFondos);
            }
        }

        private OpcionDestinoFondos ObtenerDestinoFondos(OpcionDestinoFondosComando opcionDestinoFondos)
        {
            if (opcionDestinoFondos?.DestinosFondo == null ||
                opcionDestinoFondos.DestinosFondo.Count == 0) return null;
            var todosLosDestinosFondos = _destinoFondosRepositorio.ConsultarDestinosFondos();
            var destinoFondos =
                opcionDestinoFondos.DestinosFondo.Select(df => todosLosDestinosFondos.First(d => d.Id.Valor == df.Id))
                    .ToList();
            return new OpcionDestinoFondos(destinoFondos, opcionDestinoFondos.Observaciones);
        }

        public void ActualizarCondicionesSolicitadas(int idFormulario, CondicionesSolicitadasComando comando)
        {
            if (!comando.CantidadCuotas.HasValue || !comando.MontoEstimadoCuota.HasValue ||
                !comando.MontoSolicitado.HasValue) return;
            var condicionesPrestamo = new CondicionesPrestamo(comando.MontoSolicitado.Value,
                comando.CantidadCuotas.Value, comando.MontoEstimadoCuota.Value);
            _formularioRepositorio.RegistrarCondicionesSolicitadas(idFormulario, _sesionUsuario.Usuario.Id.Valor,
                condicionesPrestamo);
        }

        public string ModificarCondicionesSolicitadas(int idFormulario, CondicionesSolicitadasComando comando)
        {
            var condicionesPrestamo = new CondicionesPrestamo(comando.MontoSolicitado.Value,
                comando.CantidadCuotas.Value);
            return _formularioRepositorio.ModificarCondicionesSolicitadas(idFormulario, _sesionUsuario.Usuario.Id.Valor,
                condicionesPrestamo);
        }

        public void ActualizarCondicionesSolicitadasAsociativas(int idAgrupamiento, CondicionesSolicitadasComando comando)
        {
            if (!comando.CantidadCuotas.HasValue || !comando.MontoEstimadoCuota.HasValue ||
                !comando.MontoSolicitado.HasValue) return;
            var condicionesPrestamo = new CondicionesPrestamo(comando.MontoSolicitado.Value,
                comando.CantidadCuotas.Value, comando.MontoEstimadoCuota.Value);
            var idFormularios = _formularioRepositorio.ObtenerIdsFormulariosAgrupamiento(idAgrupamiento);
            foreach (var idFormulario in idFormularios)
            {
                _formularioRepositorio.RegistrarCondicionesSolicitadas(idFormulario.Id, _sesionUsuario.Usuario.Id.Valor, condicionesPrestamo);
            }
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

        private IEnumerable<SolicitudCursoResultado> ObtenerCursosPorFormulario(int idFormulario)
        {
            var cursosSeleccionados =
                _cursoRepositorio.ConsultarCursosPorFormulario(idFormulario).OrderBy(c => c.TipoCurso.Id.Valor);
            if (!cursosSeleccionados.Any()) return new List<SolicitudCursoResultado>();
            var tipoCursoActual = cursosSeleccionados.First().TipoCurso.Id;
            var solicitudCursoActual = new SolicitudCursoResultado
            {
                Cursos = new List<ConsultarCursosResultado>(),
                NombreTipoCurso = cursosSeleccionados.First().TipoCurso.Nombre
            };

            var solicitudesCurso = new List<SolicitudCursoResultado> { solicitudCursoActual };
            foreach (var cursoSeleccionado in cursosSeleccionados)
            {
                if (!tipoCursoActual.Equals(cursoSeleccionado.TipoCurso.Id))
                {
                    solicitudCursoActual = new SolicitudCursoResultado
                    {
                        Cursos = new List<ConsultarCursosResultado>(),
                        NombreTipoCurso = cursoSeleccionado.TipoCurso.Nombre
                    };
                    solicitudesCurso.Add(solicitudCursoActual);
                }
                solicitudCursoActual.Cursos.Add(new ConsultarCursosResultado { Id = cursoSeleccionado.Id.Valor });
                if (cursoSeleccionado.Nombre.Equals("OTROS"))
                    solicitudCursoActual.Descripcion = cursoSeleccionado.Observaciones;
                tipoCursoActual = cursoSeleccionado.TipoCurso.Id;
            }
            return solicitudesCurso;
        }

        public void ActualizarGarantes(int idFormulario, List<DatosPersonaComando> comando)
        {
            var garantes = ObtenerGarantes(comando);
            _formularioRepositorio.RegistrarGarantes(idFormulario, _sesionUsuario.Usuario.Id.Valor, garantes);
        }

        public decimal ActualizarDatosEmprendimiento(int idAgrupamiento, EmprendimientoComando comando)
        {
            var emprendimiento = new Emprendimiento(
                comando.IdVinculo,
                comando.Email,
                comando.NroCodArea,
                comando.NroTelefono,
                comando.IdTipoInmueble,
                (int)comando.IdTipoProyecto,
                comando.IdSectorDesarrollo,
                comando.FechaActivo,
                comando.TieneExperiencia,
                comando.TiempoExperiencia,
                new Actividad(comando.IdActividad != 0 ? comando.IdActividad : -1, null, comando.FechaInicioActividad, null),
                comando.HizoCursos,
                comando.CursoInteres,
                comando.PidioCredito,
                comando.CreditoFueOtorgado,
                comando.InstitucionSolicitante,
                null,
                comando.IdTipoOrganizacion ?? -1
            );

            emprendimiento.Id = new Id(comando.Id != 0 ? comando.Id : -1);
            decimal idEmprendimiento = _formularioRepositorio.ActualizarDatosEmprendimiento(idAgrupamiento, _sesionUsuario.Usuario.Id.Valor,
                emprendimiento);
            emprendimiento.Id = new Id(idEmprendimiento);
            _formularioRepositorio.RegistrarDatosComunicacionEmprendimiento(emprendimiento, _sesionUsuario.Usuario.Id.Valor);
            return emprendimiento.Id.Valor;
        }

        private static List<Persona> ObtenerGarantes(List<DatosPersonaComando> comando)
        {
            var garantes = new List<Persona>();
            if (comando == null) return garantes;
            garantes.AddRange(comando.Select(ObtenerPersona));
            return garantes;
        }

        public decimal ExisteFormularioEnCursoParaPersona(DatosPersonaConsulta consulta)
        {
            return _formularioRepositorio.ExisteFormularioParaSolicitante(consulta);
        }

        public decimal ExisteFormularioEnCursoParaPersonaReactivacion(DatosPersonaConsulta consulta)
        {
            return _formularioRepositorio.ExisteFormularioParaSolicitanteReactivacion(consulta);
        }

        public bool ExisteDeudaHistorica(DatosPersonaConsulta consulta)
        {
            return _formularioRepositorio.ExisteDeudaHistorica(consulta);
        }

        public bool ExisteGrupoParaSolicitante(int idFormulario)
        {
            return _formularioRepositorio.ExisteGrupoParaSolicitante(idFormulario);
        }

        public bool ExisteGrupoParaPersona(DatosPersonaConsulta persona)
        {
            return _formularioRepositorio.ExisteGrupoParaPersona(persona);
        }

        public List<string> ExisteFormularioEnCursoParaIntegranteGrupo(DatosPersonaConsulta consulta)
        {
            List<string> integrantesConFormulario = new List<string>();
            var grupoFamiliar = _grupoUnicoServicio.ObtenerGrupoFamiliar(consulta.IdSexo, consulta.NroDocumento,
                consulta.CodigoPais, Parse(consulta.IdNumero));

            if (grupoFamiliar != null)
            {
                foreach (var miembro in grupoFamiliar.Integrantes)
                {
                    if (_formularioRepositorio.ExisteFormularioParaIntegranteGrupo(miembro.Sexo.IdSexo, miembro.NroDocumento, miembro.PaisTD.IdPais, miembro.Id_Numero) >= 1)
                    {
                        integrantesConFormulario.Add(miembro.Apellido + ", " + miembro.Nombre);
                    }
                }
            }
            return integrantesConFormulario;
        }

        public List<string> MiembroGrupoPerteneceAgrupamiento(GrupoFamiliarAgrupadosConsulta consulta)
        {
            List<string> familiaEnAgrupamiento = new List<string>();
            var grupoFamiliar = _grupoUnicoServicio.ObtenerGrupoFamiliar(consulta.IdSexo, consulta.NroDocumento,
                consulta.CodigoPais, Parse(consulta.IdNumero));

            var miembrosAgrupamiento = _formularioRepositorio.ObtenerFormulariosConAgrupamiento(consulta.IdAgrupamiento);

            if (grupoFamiliar != null)
            {
                foreach (var familiar in grupoFamiliar.Integrantes)
                {
                    foreach (var miembro in miembrosAgrupamiento)
                    {
                        if (miembro.NroDocumento == familiar.NroDocumento && miembro.IdNumero.GetValueOrDefault() == familiar.Id_Numero && miembro.Sexo == familiar.Sexo.IdSexo)
                        {
                            familiaEnAgrupamiento.Add(miembro.Apellido + ", " + miembro.Nombre);
                            break;
                        }
                    }
                }
            }
            return familiaEnAgrupamiento;
        }

        public DatosContactoResultado ObtenerDatosContacto(DatosPersonaConsulta consulta)
        {
            return _formularioRepositorio.ObtenerDatosContacto(consulta);
        }

        public string ActualizarDatosContacto(ActualizarDatosDeContactoComando comando)
        {
            Persona persona = new Persona
            {
                SexoId = comando.IdSexo,
                NroDocumento = comando.NroDocumento,
                CodigoPais = comando.CodigoPais,
                IdNumero = comando.IdNumero,
                CodigoArea = comando.CodAreaTelefono,
                Telefono = comando.NroTelefono,
                CodigoAreaCelular = comando.CodAreaCelular,
                Celular = comando.NroCelular,
                Email = comando.Mail
            };
            return _formularioRepositorio.ActualizarDatosDeContacto(persona);
        }

        public async Task<string> ObtenerReporteFormulario(int id)
        {
            string motivoError = "";
            try
            {
                motivoError = "obtener los datos del formulario";
                var formulario = ObtenerPorId(id);

                //DATOS FORMULARIO PRINCIPAL
                motivoError = "obtener los datos de la línea";
                LineaPrestamoResultado datosLinea =
                    _lineaPrestamoRepositorio.ConsultarLineaPorId(new Id(formulario.DetalleLinea.LineaId));
                var dsFormulario = new List<LineaPrestamoResultado>() { datosLinea };

                string observacionesRechazo = formulario.Observaciones;

                if (string.IsNullOrEmpty(observacionesRechazo) && !string.IsNullOrEmpty(formulario.ObservacionPrestamo))
                    observacionesRechazo = formulario.ObservacionPrestamo;

                //IMAGEN ENCABEZADO
                string rutaImagenEncabezado = !string.IsNullOrEmpty(datosLinea.Logo) ? datosLinea.Logo : "";

                //TODO: DATOS DE LOS CUADRANTES Y ORDEN
                motivoError = "obtener los cuadrantes";
                var cuadrantesLinea = _lineaPrestamoRepositorio
                    .ObtetenerCuadrantesPorDetalleLinea(formulario.DetalleLinea.Id)
                    .Where(x => x.IdTipoSalida != 1)
                    .OrderBy(x => x.Orden);

                //VALIDACION DE EDADES
                motivoError = "obtener edades de los integrantes del formulario";
                List<DatosPersonaResultado> integrantes =  formulario.Integrantes.Select(integrante => new DatosPersonaResultado
                {
                    SexoId = integrante.Sexo,
                    CodigoPais = integrante.Pais,
                    NroDocumento = integrante.NroDocumento,
                    IdNumero = integrante.IdNumero,
                    CodigoArea = integrante.CodigoArea,
                    Telefono = integrante.TelFijo,
                    CodigoAreaCelular = integrante.CodigoAreaCelular,
                    Celular = integrante.TelCelular,
                    Email = integrante.Mail
                }).ToList();

                //Agrego los garantes
                foreach (var garante in formulario.Garantes)
                {
                    if (integrantes.FirstOrDefault(x => x.esIgualQueOtraPersona(garante)) == null)
                        integrantes.Add(garante);
                }
                var dsValidacionEdades =
                    ValidarEdadIntegrantes(integrantes);

                //TODO: ARMADO DEL REPORTE
                motivoError = "obtener los datos de los cuadrantes";
                var subReportesOrdenados = new List<SubReporte>();

                int orden = 1;
                //AGREGADO EN ORDEN DE LOS SUBREPORTES
                foreach (var cuadrante in cuadrantesLinea)
                {
                    var subRep = new SubReporte();
                    if (cuadrante.IdTipoSalida == 3)
                    {
                        subRep = ObtenerSubReporteParaReportePorId(cuadrante.Id, formulario, orden);
                    }
                    else
                    {
                        subRep = ObtenerSubReporteParaReporteVacioPorId(cuadrante.Id, orden, formulario.DetalleLinea.LineaId);
                    }
                    if (subRep != null)
                    {
                        subReportesOrdenados.Add(subRep);
                        orden++;
                    }
                }

                //ASIGNANDO PIE DE FORMULARIO
                var dsPie = new List<ReportePieDePaginaFormularioResultado>()
                {
                    new ReportePieDePaginaFormularioResultado {UrlImagenPie = datosLinea.PiePagina}
                };
                motivoError = "generar el reporte";
                var subReportePie = new SubReporte(new Id(0), "ReporteFormulario_Pie", "SubReportePie",
                    "ReporteFormulario_Pie.rdlc", new List<string>() { "DSDetalleLinea", "DSPie" });
                subReportePie.AsignarDataSet(dsFormulario)
                    .AsignarDataSet(dsPie);
                subReportesOrdenados.Add(subReportePie);


                var reportData = new ReporteBuilder("ReporteFormulario.rdlc", "SubReporteFormulario")
                    .AgregarDataSource("DSReporteFormulario", dsFormulario)
                    .AgregarDataSource("DSValidacionEdad", dsValidacionEdades)
                    .AgregarParametro("pImagenEncabezado", rutaImagenEncabezado)
                    .AgregarParametro("pMotivosRechazo", formulario.MotivoRechazo)
                    .AgregarParametro("pObservacionesRechazo", observacionesRechazo)
                    .AgregarParametro("pSUAC", formulario.NroSticker ?? "")
                    .AgregarParametro("pMostrarValidacionEdades", dsValidacionEdades.Count > 0)
                    .SubReporteDS(subReportesOrdenados)
                    .GenerarConSubReporte();

                var url = ReporteBuilder.GenerarUrlReporte(reportData, "ReporteFormulario");
                return url;
            }
            catch (Exception e)
            {
                throw new ErrorTecnicoException("Error al " + motivoError, e);
            }
        }

        #region Obtencion de SubReportes

        private SubReporte ObtenerSubReporteParaReportePorId(long id, DatosFormularioResultado formulario, int orden)
        {
            switch (id)
            {
                case 1:
                    return SubReporte.DatosPersonales().AsignarDataSet(ObtenerDSDatosPersonales(formulario, orden));
                case 2:
                    return SubReporte.GrupoFamiliar()
                        .AsignarDataSet(ObtenerDSGrupoFamiliar(formulario.Solicitante.SexoId,
                            formulario.Solicitante.NroDocumento,
                            formulario.Solicitante.CodigoPais, orden, formulario.Solicitante.IdNumero));
                case 3:
                    {
                        var destinosFondosPosibles = ObtenerDSDestinoFondos(formulario, orden);
                        var dsDestinosFondos = new List<DestinoFondoReporteResultado>();
                        var dsDestinosFondos2 = new List<DestinoFondoReporteResultado>();
                        var dsDestinosFondos3 = new List<DestinoFondoReporteResultado>();
                        decimal calculoElementos = (decimal)destinosFondosPosibles.Count / 3;
                        int cantPorTabla = (int)Math.Ceiling(calculoElementos);
                        int i = 1;
                        int tabla = 1;
                        foreach (var df in destinosFondosPosibles)
                        {
                            if (tabla == 1)
                                dsDestinosFondos.Add(df);
                            if (tabla == 2)
                                dsDestinosFondos2.Add(df);
                            if (tabla == 3)
                                dsDestinosFondos3.Add(df);

                            if (i == cantPorTabla)
                            {
                                i = 1;
                                tabla++;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        if (i != 1)
                        {
                            while (i <= cantPorTabla)
                            {
                                if (tabla == 1)
                                    dsDestinosFondos.Add(new DestinoFondoReporteResultado { Orden = orden });
                                if (tabla == 2)
                                    dsDestinosFondos2.Add(new DestinoFondoReporteResultado { Orden = orden });
                                if (tabla == 3)
                                    dsDestinosFondos3.Add(new DestinoFondoReporteResultado { Orden = orden });
                                i++;
                            }
                        }

                        return SubReporte.DestinoFondos()
                            .AsignarDataSet(dsDestinosFondos)
                            .AsignarDataSet(dsDestinosFondos2)
                            .AsignarDataSet(dsDestinosFondos3);
                    }
                case 4:
                    return SubReporte.CondicionesMicroprestamo()
                        .AsignarDataSet(ObtenerDSCondicionesMicroPrestamo(formulario, orden));
                case 5:
                    {
                        var dataSets = ObtenerDSCursos(formulario, orden);
                        return SubReporte.Cursos()
                            .AsignarDataSet(dataSets.CursosConSalidaLaboral)
                            .AsignarDataSet(dataSets.CursosDeCapacitacion)
                            .AsignarDataSet(dataSets.CursosConSalidaLaboral2)
                            .AsignarDataSet(dataSets.CursosDeCapacitacion2)
                            .AsignarDataSet(dataSets.NombreDescripcion);
                    }
                case 6:
                    return SubReporte.DatosGarante().AsignarDataSet(ObtenerDSGarante(formulario, orden));
                case 7:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);
                        var actividades = _emprendimientoRepositorio.ObtenerActividades(datosEmp.IdRubro.GetValueOrDefault());
                        var tiposProyectos = _emprendimientoRepositorio.ObtenerTiposProyecto();
                        var tiposInmuebles = _emprendimientoRepositorio.ObtenerTiposInmueble();
                        var sectoresDesarrollo = _emprendimientoRepositorio.ObtenerSectoresDesarrollo();
                        string actividad = "";
                        if (datosEmp.IdActividad != 0)
                        {
                            var act = actividades.FirstOrDefault(x => x.Id.Valor == datosEmp.IdActividad);
                            actividad = act != null ? act.Nombre : "";
                        }
                        var dsDatosEmprendimiento = new List<DatosEmprendimientoResultado>() { new DatosEmprendimientoResultado(datosEmp, actividad, orden) };
                        var dsTiposProyecto = new List<ItemReporteResultado>();
                        foreach (var obj in tiposProyectos)
                        {
                            dsTiposProyecto.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), obj.Id.Valor == datosEmp.IdTipoProyecto));
                        }
                        var dsTiposInmuebles = new List<ItemReporteResultado>();
                        foreach (var obj in tiposInmuebles)
                        {
                            dsTiposInmuebles.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), obj.Id.Valor == datosEmp.IdTipoInmueble));
                        }
                        var dsSectoresDesarrollo = new List<ItemReporteResultado>();
                        foreach (var obj in sectoresDesarrollo)
                        {
                            dsSectoresDesarrollo.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), obj.Id.Valor == datosEmp.IdSectorDesarrollo));
                        }
                        return SubReporte.DatosEmprendimiento()
                            .AsignarDataSet(dsDatosEmprendimiento)
                            .AsignarDataSet(dsTiposProyecto)
                            .AsignarDataSet(dsTiposInmuebles)
                            .AsignarDataSet(dsSectoresDesarrollo);
                    }
                case 8:
                    {
                        var dsPatrimonioSolicitante = new List<PatrimonioSolicitanteResultado>();

                        var patrimonio = _formularioRepositorio.ObtenerPatrimonioSolicitante(formulario.Id);

                        patrimonio.Orden = orden;
                        patrimonio.PropiedadVehiculos = patrimonio.VehiculoPropio ? "S" : "N";
                        patrimonio.PropiedadInmueble = patrimonio.InmueblePropio ? "S" : "N";

                        dsPatrimonioSolicitante.Add(patrimonio);

                        var subReporte = SubReporte.PatrimonioSolicitante()
                            .AsignarDataSet(dsPatrimonioSolicitante);

                        return subReporte;
                    }
                case 9:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);
                        var miembros = ObtenerMiembrosEmprendimiento(datosEmp.Id);
                        var tiposOrganizaciones = _emprendimientoRepositorio.ObtenerTiposOrganizaciones();
                        var dsTiposOrganizaciones = new List<ItemReporteResultado>();
                        foreach (var obj in tiposOrganizaciones)
                        {
                            dsTiposOrganizaciones.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), obj.Id.Valor == datosEmp.IdTipoOrganizacion));
                        }
                        List<OrganizacionEmprendimientoResultado> dsOrganizacionEmprendimiento = new List<OrganizacionEmprendimientoResultado>();
                        foreach (var miembro in miembros)
                        {
                            string antecedentesLaborales = miembro.AntecedentesLaborales != null ? (miembro.AntecedentesLaborales.Value ? "S" : "N") : "";
                            dsOrganizacionEmprendimiento.Add(new OrganizacionEmprendimientoResultado(miembro.Persona.NombreCompleto, miembro.Vinculo, miembro.Persona.Edad, miembro.Tarea, miembro.HorarioTrabajo, miembro.Remuneracion, antecedentesLaborales, orden));
                        }
                        return SubReporte.OrganizacionEmprendimiento()
                            .AsignarDataSet(dsOrganizacionEmprendimiento)
                            .AsignarDataSet(dsTiposOrganizaciones);
                    }

                case 10:
                    var itemsMercadoVacios = _emprendimientoRepositorio.ObtenerItemsComercializacion();
                    var itemMercadoSeleccionados = new List<int>();
                    foreach (var itemsPorCat in formulario.MercadoComercializacion.ItemsPorCategoria)
                    {
                        itemMercadoSeleccionados.AddRange(itemsPorCat.Items);
                    }

                    var lsItemsReporteConCategoria = new List<ItemReporteConCategoriaResultado>();
                    foreach (var item in itemsMercadoVacios)
                    {
                        var nuevoItemReporte = new ItemReporteConCategoriaResultado
                        {
                            IdCategoria = (decimal)item.IdCategoria,
                            Nombre = item.Nombre,
                            NombreCategoria = item.NombreTipo,
                            Seleccionado = itemMercadoSeleccionados.Exists(x => item.Id == x),
                            Orden = orden
                        };

                        lsItemsReporteConCategoria.Add(nuevoItemReporte);
                    }

                    var formasPagoFormulario = new List<FormaPagoItem>();
                    foreach (var formas in formulario.MercadoComercializacion.FormasPago.OrderBy(x => x.Id))
                    {
                        var formaPago = new FormaPagoItem();
                        formaPago.Id = formas.Id ?? 0;
                        formaPago.Tipo = formas.Tipo;
                        formaPago.Valor = formas.Valor;

                        formasPagoFormulario.Add(formaPago);
                    }

                    var lsEstimas = new List<EstimaClienteReporteResultado>();
                    var estimaForm = new EstimaClienteReporteResultado
                    {
                        Estima = formulario.MercadoComercializacion.EstimaClientes.Estima
                    };
                    if (estimaForm.Estima)
                    {
                        estimaForm.Cantidad = formulario.MercadoComercializacion.EstimaClientes.Cantidad ?? 0;
                    }
                    else
                    {
                        estimaForm.Cantidad = 0;
                    }

                    lsEstimas.Add(estimaForm);


                    return SubReporte.MercadoComercializacion()
                        .AsignarDataSet(lsItemsReporteConCategoria)
                        .AsignarDataSet(formasPagoFormulario)
                        .AsignarDataSet(lsEstimas);

                case 11:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);

                        var inversion = ObtenerDSInversionRealizada(datosEmp, orden);

                        if (inversion.Count < 1)
                        {
                            inversion = ObtenerInversionesVacias(orden);
                        }

                        var subReporte = SubReporte.InversionRealizada()
                            .AsignarDataSet(inversion);

                        return subReporte;
                    }
                case 12:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);

                        var deuda = _deudaEmprendimientoServicio.ObtenerDeudasParaReporte(datosEmp.Id, orden);

                        if (deuda.Count < 1)
                        {
                            deuda = _deudaEmprendimientoServicio.ObtenerDeudasParaReporte(new Id(), orden);
                        }

                        var subReporte = SubReporte.DeudaEmprendimiento()
                            .AsignarDataSet(deuda);

                        return subReporte;
                    }
                case 13:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);

                        var necesidadInversion = _inversionEmprendimientoServicio.ObtenerNecesidadInversionParaReporte(datosEmp.Id, orden);

                        var subReporte = SubReporte.NecesidadInversion()
                            .AsignarDataSet(new List<NecesidadInversionResultado> { necesidadInversion })
                            .AsignarDataSet(necesidadInversion.InversionesRealizadas);

                        return subReporte;
                    }
                case 15:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);
                        var datosPrecioVenta = ObtenerPrecioVenta(datosEmp.Id.Valor);
                        var dsCostosVariables = datosPrecioVenta.Costos.Where(x => x.IdTipo == 1);
                        var dsCostosFijos = datosPrecioVenta.Costos.Where(x => x.IdTipo == 2);

                        //TODO desde aca se saca cuando se arregle el #error del precio venta unitario en el reporte
                        var costosUnitariosProd = new decimal();
                        var gananciaEst = datosPrecioVenta.GananciaEstimada ?? 0;
                        //var unidEst = datosPrecioVenta.UnidadesEstimadas ?? 0;
                        foreach (var costo in datosPrecioVenta.Costos)
                        {
                            costosUnitariosProd += costo.PrecioUnitario ?? 0;
                        }
                        var precioUnitario = costosUnitariosProd + gananciaEst;
                        //TODO borrar hasta aca, el precio de venta unitario que se pasa como ultimo parametro y el atributo en la clase PrecioVentaResultado.

                        var dsPrecioVenta = new List<PrecioVentaResultado>();
                        var pv = new PrecioVentaResultado(datosPrecioVenta.Producto, datosPrecioVenta.UnidadesEstimadas, datosPrecioVenta.IdProducto, datosPrecioVenta.GananciaEstimada ?? 0, orden, precioUnitario);
                        dsPrecioVenta.Add(pv);

                        return SubReporte.PrecioVenta()
                        .AsignarDataSet(dsPrecioVenta)
                        .AsignarDataSet(dsCostosVariables)
                        .AsignarDataSet(dsCostosFijos);
                    }
                case 16:
                    {
                        var datosEmprendimiento = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);
                        var datosPrecioVenta = ObtenerPrecioVenta(datosEmprendimiento.Id.Valor);

                        //Precio de venta unitario = Total de costos variables + Total de costos fijos + Ganancia estimada;

                        decimal? totalCostos = datosPrecioVenta.Costos.Aggregate<ItemsPrecioVentaResultado, decimal?>(0, (current, costo) => current + costo.PrecioUnitario);

                        totalCostos += datosPrecioVenta.GananciaEstimada;

                        var dataSetCostos = datosPrecioVenta.Costos;

                        var dsPrecioVenta = new List<PrecioVentaResultado>
                    {
                        new PrecioVentaResultado(datosPrecioVenta.Producto,
                            datosPrecioVenta.UnidadesEstimadas,
                            datosPrecioVenta.IdProducto,
                            datosPrecioVenta.GananciaEstimada ?? 0,
                            orden, totalCostos)
                    };

                        return SubReporte.ResultadoMensual()
                        .AsignarDataSet(dsPrecioVenta)
                        .AsignarDataSet(dataSetCostos);
                    }
                case 17:
                    {
                        var datosEmp = _formularioRepositorio.ObtenerDatosEmprendimiento(formulario.Id);
                        var datosPrecioVenta = ObtenerPrecioVenta(datosEmp.Id.Valor);
                        var unidadesEst = datosPrecioVenta.UnidadesEstimadas;
                        var costosUnitariosProd = new decimal();
                        var gananciaEst = datosPrecioVenta.GananciaEstimada ?? 0;
                        //var unidEst = datosPrecioVenta.UnidadesEstimadas ?? 0;
                        foreach (var costo in datosPrecioVenta.Costos)
                        {
                            costosUnitariosProd += costo.PrecioUnitario ?? 0;
                        }
                        var precioUnitario = costosUnitariosProd + gananciaEst;


                        var ingresosGrupo = ObtenerIngresosGrupoFamiliarFormulario(formulario.Id);

                        // 0 = valor del emprendimiento
                        ingresosGrupo[0].Valor = unidadesEst * precioUnitario;

                        //TODO agregar validacion grupo null
                        var grupoFamiliar = _grupoUnicoServicio.ObtenerGrupoFamiliar(formulario.Solicitante.SexoId,
                            formulario.Solicitante.NroDocumento, formulario.Solicitante.CodigoPais, formulario.Solicitante.IdNumero);

                        var ingresosGrupoFamiliar = _formularioRepositorio.ObtenerIngresoTotalGrupoFamiliar((int)grupoFamiliar.IdGrupo);


                        ingresosGrupo[1].Valor = ingresosGrupoFamiliar;

                        var gastosGrupoFamiliar = _formularioRepositorio.ObtenerGastosGrupoFamiliar((int)grupoFamiliar.IdGrupo);

                        List<IngresoGrupoResultado> dsIngresos = new List<IngresoGrupoResultado>();
                        List<EgresoGrupoResultado> dsGastos = new List<EgresoGrupoResultado>();

                        foreach (var gasto in gastosGrupoFamiliar)
                        {
                            dsGastos.Add(gasto);
                        }
                        foreach (var ing in ingresosGrupo)
                        {
                            ing.Orden = orden;
                            ing.Descripcion = CapFirst(ing.Descripcion);
                            if (ing.IdConcepto == (int)TipoConcepto.Enum.Ingreso)
                                dsIngresos.Add(ing);
                        }

                        //Unifico la cantidad de registros que tiene cada dataset para que las tablas tengan la misma cantidad de filas
                        if (dsIngresos.Count > dsGastos.Count)
                        {
                            while (dsGastos.Count < dsIngresos.Count)
                                dsGastos.Add(new EgresoGrupoResultado() { Id = new Id(1) });
                        }
                        if (dsGastos.Count > dsIngresos.Count)
                        {
                            while (dsIngresos.Count < dsGastos.Count)
                                dsIngresos.Add(new IngresoGrupoResultado { IdConcepto = (int)TipoConcepto.Enum.Ingreso });
                        }

                        return SubReporte.IngresosYGastosActuales()
                                .AsignarDataSet(dsIngresos)
                                .AsignarDataSet(dsGastos);
                    }
                case 21:
                    {
                        var requisitos =
                            ConsultarRequisitosParaCuadrantePorIdLinea(new Id(formulario.DetalleLinea.LineaId));
                        var dsRequisitosSolicitante = requisitos.Where(x => x.EsSolicitante);
                        var dsRequisitosGarante = requisitos.Where(x => x.EsGarante);
                        return SubReporte.Requisitos()
                            .AsignarDataSet(dsRequisitosSolicitante)
                            .AsignarDataSet(dsRequisitosGarante);
                    }
                case 22:
                    {
                        return SubReporte.Integrantes().AsignarDataSet(ObtenerDSIntegrantes(formulario.Id, orden, formulario.Integrantes.ToList(), formulario.Solicitante));
                    }
                case 23:
                    {
                        return SubReporte.Ong().AsignarDataSet(ObtenerDSOng(formulario.Id, orden));
                    }
                default:
                    return null;
            }
        }

        private List<InversionRealizadaResultado> ObtenerInversionesVacias(int orden)
        {
            var resultados = new List<InversionRealizadaResultado>();
            for (var i = 0; i < 6; i++)
            {
                resultados.Add(new InversionRealizadaResultado { Orden = orden});
            }

            return resultados;
        }

        private string CapFirst(string cadena)
        {
            return cadena.Substring(0, 1).ToUpper() + cadena.Substring(1).ToLower();
        }

        private SubReporte ObtenerSubReporteParaReporteVacioPorId(long id, int orden, decimal idLinea)
        {
            switch (id)
            {
                case 1:
                    return SubReporte.DatosPersonales()
                        .AsignarDataSet(new List<DatosPersonalesResultado>()
                        {
                            new DatosPersonalesResultado
                            {
                                CodigoArea = "            ",
                                CodigoAreaCelular = "            ",
                                Orden = orden
                            }
                        });
                case 2:
                    {
                        List<GrupoFamiliarResultado> dsGrupoFamiliar = new List<GrupoFamiliarResultado>();
                        for (int i = 0; i < 9; i++)
                        {
                            dsGrupoFamiliar.Add(new GrupoFamiliarResultado() { Orden = orden });
                        }
                        return SubReporte.GrupoFamiliar().AsignarDataSet(dsGrupoFamiliar);
                    }
                case 3:
                    {
                        var destinosFondosPosibles = _destinoFondosRepositorio.ConsultarDestinosFondos();
                        var dsDestinosFondos = new List<DestinoFondoReporteResultado>();
                        var dsDestinosFondos2 = new List<DestinoFondoReporteResultado>();
                        var dsDestinosFondos3 = new List<DestinoFondoReporteResultado>();
                        decimal calculoElementos = (decimal)destinosFondosPosibles.Count / 3;
                        int cantPorTabla = (int)Math.Ceiling(calculoElementos);
                        int i = 1;
                        int tabla = 1;
                        foreach (var df in destinosFondosPosibles)
                        {
                            if (tabla == 1)
                                dsDestinosFondos.Add(
                                    new DestinoFondoReporteResultado(df.Nombre, CapFirst(df.Descripcion), "", false)
                                    {
                                        Orden = orden
                                    });
                            if (tabla == 2)
                                dsDestinosFondos2.Add(
                                    new DestinoFondoReporteResultado(df.Nombre, CapFirst(df.Descripcion), "", false)
                                    {
                                        Orden = orden
                                    });
                            if (tabla == 3)
                                dsDestinosFondos3.Add(
                                    new DestinoFondoReporteResultado(df.Nombre, CapFirst(df.Descripcion), "", false)
                                    {
                                        Orden = orden
                                    });

                            if (i == cantPorTabla)
                            {
                                i = 1;
                                tabla++;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        if (i != 1)
                        {
                            while (i <= cantPorTabla)
                            {
                                if (tabla == 1)
                                    dsDestinosFondos.Add(new DestinoFondoReporteResultado { Orden = orden });
                                if (tabla == 2)
                                    dsDestinosFondos2.Add(new DestinoFondoReporteResultado { Orden = orden });
                                if (tabla == 3)
                                    dsDestinosFondos3.Add(new DestinoFondoReporteResultado { Orden = orden });
                                i++;
                            }
                        }

                        return SubReporte.DestinoFondos()
                            .AsignarDataSet(dsDestinosFondos)
                            .AsignarDataSet(dsDestinosFondos2)
                            .AsignarDataSet(dsDestinosFondos3);
                    }
                case 4:
                    return SubReporte.CondicionesMicroprestamo()
                        .AsignarDataSet(
                            new List<CondicionesSolicitadasResultado>
                            {
                                new CondicionesSolicitadasResultado() {Orden = orden}
                            });
                case 5:
                    {
                        var cursos = _cursoRepositorio.Consultar();
                        List<SolicitudCursoReporteResultado> dsCursosSalidaLaboral =
                            new List<SolicitudCursoReporteResultado>();
                        List<SolicitudCursoReporteResultado> dsCursosCapacitacion =
                            new List<SolicitudCursoReporteResultado>();
                        List<SolicitudCursoReporteResultado> dsCursosSalidaLaboral2 =
                            new List<SolicitudCursoReporteResultado>();
                        List<SolicitudCursoReporteResultado> dsCursosCapacitacion2 =
                            new List<SolicitudCursoReporteResultado>();

                        var cursosFiltradosSalidaLaboral = cursos.Where(x => x.TipoCurso.Id.Valor == 1).ToList();
                        var cursosFiltradosCapacitacion = cursos.Where(x => x.TipoCurso.Id.Valor == 2).ToList();

                        decimal calculoElementos = (decimal)cursosFiltradosSalidaLaboral.Count / 2;
                        int cantPorTabla = (int)Math.Ceiling(calculoElementos);
                        int i = 1;
                        int tabla = 1;

                        foreach (var curso in cursosFiltradosSalidaLaboral)
                        {
                            if (tabla == 1)
                                dsCursosSalidaLaboral.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                                    curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, false)
                                { Orden = orden });
                            if (tabla == 2)
                                dsCursosSalidaLaboral2.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                                    curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, false)
                                { Orden = orden });

                            if (i == cantPorTabla)
                            {
                                i = 1;
                                tabla++;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        if (i != 1)
                        {
                            while (i <= cantPorTabla)
                            {
                                if (tabla == 1)
                                    dsCursosSalidaLaboral.Add(new SolicitudCursoReporteResultado { Orden = orden });
                                if (tabla == 2)
                                    dsCursosSalidaLaboral2.Add(new SolicitudCursoReporteResultado { Orden = orden });
                                i++;
                            }
                        }

                        calculoElementos = (decimal)cursosFiltradosCapacitacion.Count / 2;
                        cantPorTabla = (int)Math.Ceiling(calculoElementos);
                        i = 1;
                        tabla = 1;

                        foreach (var curso in cursosFiltradosCapacitacion)
                        {
                            if (tabla == 1)
                                dsCursosCapacitacion.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                                    curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, false)
                                { Orden = orden });
                            if (tabla == 2)
                                dsCursosCapacitacion2.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                                    curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, false)
                                { Orden = orden });

                            if (i == cantPorTabla)
                            {
                                i = 1;
                                tabla++;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        if (i != 1)
                        {
                            while (i <= cantPorTabla)
                            {
                                if (tabla == 1)
                                    dsCursosCapacitacion.Add(new SolicitudCursoReporteResultado { Orden = orden });
                                if (tabla == 2)
                                    dsCursosCapacitacion2.Add(new SolicitudCursoReporteResultado { Orden = orden });
                                i++;
                            }
                        }

                        List<NombreDescripcionOtros> dsNombreDescripcionOtros = new List<NombreDescripcionOtros>();

                        return SubReporte.Cursos()
                            .AsignarDataSet(dsCursosSalidaLaboral)
                            .AsignarDataSet(dsCursosCapacitacion)
                            .AsignarDataSet(dsCursosSalidaLaboral2)
                            .AsignarDataSet(dsCursosCapacitacion2)
                            .AsignarDataSet(dsNombreDescripcionOtros);
                    }
                case 6:
                    return SubReporte.DatosGarante()
                        .AsignarDataSet(new List<DatosPersonalesResultado>()
                        {
                            new DatosPersonalesResultado
                            {
                                CodigoArea = "            ",
                                CodigoAreaCelular = "            ",
                                Orden = orden
                            }
                        });
                case 7:
                    {
                        var tiposProyectos = _emprendimientoRepositorio.ObtenerTiposProyecto();
                        var tiposInmuebles = _emprendimientoRepositorio.ObtenerTiposInmueble();
                        var sectoresDesarrollo = _emprendimientoRepositorio.ObtenerSectoresDesarrollo();
                        var dsDatosEmprendimiento = new List<DatosEmprendimientoResultado>() { new DatosEmprendimientoResultado { Orden = orden } };
                        var dsTiposProyecto = new List<ItemReporteResultado>();
                        foreach (var obj in tiposProyectos)
                        {
                            dsTiposProyecto.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), false));
                        }
                        var dsTiposInmuebles = new List<ItemReporteResultado>();
                        foreach (var obj in tiposInmuebles)
                        {
                            dsTiposInmuebles.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), false));
                        }
                        var dsSectoresDesarrollo = new List<ItemReporteResultado>();
                        foreach (var obj in sectoresDesarrollo)
                        {
                            dsSectoresDesarrollo.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), false));
                        }
                        return SubReporte.DatosEmprendimiento()
                            .AsignarDataSet(dsDatosEmprendimiento)
                            .AsignarDataSet(dsTiposProyecto)
                            .AsignarDataSet(dsTiposInmuebles)
                            .AsignarDataSet(dsSectoresDesarrollo);
                    }
                case 8:
                    {
                        var dsPatrimonioSolicitante = new List<PatrimonioSolicitanteResultado>();

                        var patrimonio = new PatrimonioSolicitanteResultado()
                        {
                            Orden = orden,
                            PropiedadInmueble = "",
                            PropiedadVehiculos = ""
                        };

                        dsPatrimonioSolicitante.Add(patrimonio);

                        return SubReporte.PatrimonioSolicitante()
                            .AsignarDataSet(dsPatrimonioSolicitante);
                    }
                case 9:
                    {
                        var tiposOrganizaciones = _emprendimientoRepositorio.ObtenerTiposOrganizaciones();
                        var dsTiposOrganizaciones = new List<ItemReporteResultado>();
                        foreach (var obj in tiposOrganizaciones)
                        {
                            dsTiposOrganizaciones.Add(new ItemReporteResultado(CapFirst(obj.Descripcion), false));
                        }
                        List<OrganizacionEmprendimientoResultado> dsOrganizacionEmprendimiento = new List<OrganizacionEmprendimientoResultado>();
                        for (int i = 0; i < 6; i++)
                        {
                            dsOrganizacionEmprendimiento.Add(new OrganizacionEmprendimientoResultado() { Orden = orden });
                        }
                        return SubReporte.OrganizacionEmprendimiento()
                            .AsignarDataSet(dsOrganizacionEmprendimiento)
                            .AsignarDataSet(dsTiposOrganizaciones);
                    }
                case 10:
                    var lsItemsMercadoVacios = _emprendimientoRepositorio.ObtenerItemsComercializacion();

                    var lsItemsReporteConCategoria = new List<ItemReporteConCategoriaResultado>();
                    foreach (var item in lsItemsMercadoVacios)
                    {
                        var nuevoItemReporte = new ItemReporteConCategoriaResultado
                        {
                            IdCategoria = item.IdCategoria ?? 0,
                            Nombre = item.Nombre,
                            NombreCategoria = item.NombreTipo,
                            Seleccionado = false,
                            Orden = orden
                        };
                        lsItemsReporteConCategoria.Add(nuevoItemReporte);
                    }

                    var lsformasPago = new List<FormaPagoItem>();
                    for (var i = 1; i <= 5; i++)
                    {
                        var formaPago = new FormaPagoItem();
                        formaPago.Id = i;
                        formaPago.Tipo = "1";
                        formaPago.Valor = "";

                        lsformasPago.Add(formaPago);
                    }

                    for (var i = 1; i <= 5; i++)
                    {
                        var formaPago = new FormaPagoItem();
                        formaPago.Id = i;
                        formaPago.Tipo = "2";
                        formaPago.Valor = "";

                        lsformasPago.Add(formaPago);
                    }
                    var lsEstimas = new List<EstimaClienteReporteResultado>();
                    var estima = new EstimaClienteReporteResultado();
                    estima.Estima = false;
                    estima.Cantidad = -1;

                    lsEstimas.Add(estima);

                    return SubReporte.MercadoComercializacion()
                        .AsignarDataSet(lsItemsReporteConCategoria)
                        .AsignarDataSet(lsformasPago)
                        .AsignarDataSet(lsEstimas);
                case 11:
                    {
                        var subReporte = SubReporte.InversionRealizada()
                            .AsignarDataSet(_detalleInversionEmprendimientoServicio.ObtenerDetallesInversionRealizadaParaReportes(new Id(), orden));

                        return subReporte;
                    }
                case 12:
                    {
                        var subReporte = SubReporte.DeudaEmprendimiento()
                            .AsignarDataSet(_deudaEmprendimientoServicio.ObtenerDeudasParaReporte(new Id(), orden));

                        return subReporte;
                    }
                case 13:
                    {
                        var necesidadInversion = _inversionEmprendimientoServicio.ObtenerNecesidadInversionParaReporte(new Id(), orden);

                        var subReporte = SubReporte.NecesidadInversion()
                            .AsignarDataSet(new List<NecesidadInversionResultado> { necesidadInversion })
                            .AsignarDataSet(necesidadInversion.InversionesRealizadas);

                        return subReporte;
                    }
                case 15:
                    {
                        var dsPrecioVenta = new List<PrecioVentaResultado>();
                        var pv = new PrecioVentaResultado(default(string), default(decimal?), default(decimal?), default(decimal), orden, 0);
                        dsPrecioVenta.Add(pv);

                        var dsCostosVariables = new List<ItemsPrecioVentaResultado>();
                        for (var i = 1; i < 7; i++)
                        {
                            dsCostosVariables.Add(new ItemsPrecioVentaResultado(default(decimal?), default(decimal?), 1, default(decimal?), default(decimal?), i.ToString()));
                        }
                        dsCostosVariables.Add(new ItemsPrecioVentaResultado(default(decimal?), 99, 1, default(decimal?), default(decimal?), "1"));

                        var dsCostosFijos = new List<ItemsPrecioVentaResultado>();
                        for (var i = 1; i < 7; i++)
                        {
                            dsCostosFijos.Add(new ItemsPrecioVentaResultado(default(decimal?), default(decimal?), 2, default(decimal?), default(decimal?), i.ToString()));
                        }
                        dsCostosFijos.Add(new ItemsPrecioVentaResultado(default(decimal?), 999, 2, default(decimal?), default(decimal?), "1"));

                        return SubReporte.PrecioVenta()
                                 .AsignarDataSet(dsPrecioVenta)
                                 .AsignarDataSet(dsCostosVariables)
                                 .AsignarDataSet(dsCostosFijos);
                    }
                case 16:
                    {
                        // REPORTE VACÍO

                        //Se tienen los datos del precio de venta

                        var datosPrecioVenta = new PrecioVentaResultado(default(string),
                            default(decimal?), default(decimal?),
                            default(decimal), orden, default(decimal?))
                        {
                            Costos = new List<ItemsPrecioVentaResultado>()
                        };


                        //Obtiene los datos de los 2 tipos de costo, yo los necesito a ambos asi que no hace falta dividir.

                        var dsCostos = datosPrecioVenta.Costos;

                        var dsResultadoMensual = new List<PrecioVentaResultado> { datosPrecioVenta };

                        for (var i = 1; i <= 7; i++)
                        {
                            dsCostos.Add(
                                new ItemsPrecioVentaResultado(default(decimal?),
                                    default(decimal?), default(decimal?),
                                    default(decimal?), default(decimal?), default(string)));
                        }

                        return SubReporte.ResultadoMensual()
                        .AsignarDataSet(dsResultadoMensual)
                        .AsignarDataSet(dsCostos);
                    }
                case 17:
                    {
                        var ingresosGrupo = ObtenerIngresosGrupoFamiliar();
                        List<IngresoGrupoResultado> dsIngresos = new List<IngresoGrupoResultado>();
                        List<EgresoGrupoResultado> dsGastos = new List<EgresoGrupoResultado>();


                        foreach (var ing in ingresosGrupo)
                        {
                            ing.Orden = orden;
                            ing.Descripcion = CapFirst(ing.Descripcion);
                            if (ing.IdConcepto == (int)TipoConcepto.Enum.Ingreso)
                                dsIngresos.Add(ing);
                        }

                        //Se agrega porque los conceptos se obtienen de gobierno a travez de un id de grupo
                        dsGastos.Add(new EgresoGrupoResultado() { Id = new Id(1), Concepto = "Alquiler" });
                        dsGastos.Add(new EgresoGrupoResultado() { Id = new Id(1), Concepto = "Créditos" });
                        dsGastos.Add(new EgresoGrupoResultado() { Id = new Id(1), Concepto = "Otros" });
                        dsGastos.Add(new EgresoGrupoResultado() { Id = new Id(1), Concepto = "Servicios" });


                        //Unifico la cantidad de registros que tiene cada dataset para que las tablas tengan la misma cantidad de filas
                        if (dsIngresos.Count > dsGastos.Count)
                        {
                            while (dsGastos.Count < dsIngresos.Count)
                                dsGastos.Add(new EgresoGrupoResultado() { Id = new Id(1) });
                        }
                        if (dsGastos.Count > dsIngresos.Count)
                        {
                            while (dsIngresos.Count < dsGastos.Count)
                                dsIngresos.Add(new IngresoGrupoResultado { IdConcepto = (int)TipoConcepto.Enum.Ingreso });
                        }

                        return SubReporte.IngresosYGastosActuales()
                                .AsignarDataSet(dsIngresos)
                                .AsignarDataSet(dsGastos);
                    }
                case 21:
                    {
                        var requisitos = ConsultarRequisitosParaCuadrantePorIdLinea(new Id(idLinea));
                        var dsRequisitosSolicitante = requisitos.Where(x => x.EsSolicitante);
                        var dsRequisitosGarante = requisitos.Where(x => x.EsGarante);
                        return SubReporte.Requisitos()
                            .AsignarDataSet(dsRequisitosSolicitante)
                            .AsignarDataSet(dsRequisitosGarante);
                    }
                case 22:
                    {
                        var dsIntegrantes = new List<IntegrantesResultado>(4);
                        dsIntegrantes.Add(new IntegrantesResultado(orden));
                        dsIntegrantes.Add(new IntegrantesResultado(orden));
                        dsIntegrantes.Add(new IntegrantesResultado(orden));
                        dsIntegrantes.Add(new IntegrantesResultado(orden));

                        return SubReporte.Integrantes().AsignarDataSet(dsIntegrantes);
                    }
                case 23:
                    {
                        return SubReporte.Ong().AsignarDataSet(ObtenerDSOng(-1, orden));
                    }
                default:
                    return null;
            }
        }

        #endregion

        #region Data Sources SubReportes

        private List<DatosPersonalesResultado> ObtenerDSDatosPersonales(DatosFormularioResultado formulario, int orden)
        {
            DatosPersonalesResultado persona = ObtenerDatosPersonalesCompleto(formulario.Solicitante);
            persona.Orden = orden;
            return new List<DatosPersonalesResultado>() { persona };
        }

        private List<GrupoFamiliarResultado> ObtenerDSGrupoFamiliar(string sexo, string dni, string pais, int orden, int? idNumero)
        {
            var grupoFamiliar = (RespuestaAPIGrupoFamiliar)_grupoUnicoServicio.GetApiConsultaIngresosGrupoFamiliar(
                sexo,
                dni, pais, idNumero);

            List<GrupoFamiliarResultado> dsGrupoFamiliar = new List<GrupoFamiliarResultado>();
            if (grupoFamiliar.Grupo != null)
            {
                foreach (var integrante in grupoFamiliar.Grupo.Integrantes)
                {
                    GrupoFamiliarResultado persona = new GrupoFamiliarResultado(integrante) { Orden = orden };
                    dsGrupoFamiliar.Add(persona);
                }
            }
            else
            {
                GrupoFamiliarResultado persona = new GrupoFamiliarResultado { Orden = orden };
                dsGrupoFamiliar.Add(persona);
            }
            return dsGrupoFamiliar;
        }

        private List<InversionRealizadaResultado> ObtenerDSInversionRealizada(EmprendimientoResultado datosEmp, int orden)
        {
            var dsInversionRealizada = new List<InversionRealizadaResultado>();

            var inversiones = _detalleInversionEmprendimientoServicio.ObtenerDetallesInversionRealizadaParaReportes(datosEmp.Id, orden);

            foreach (var inv in inversiones)
            {
                if (inv.CantidadNuevos > 0 || inv.CantidadUsados > 0)
                {
                    dsInversionRealizada.Add(inv);
                }
            }

            return dsInversionRealizada;
        }

        private List<DestinoFondoReporteResultado> ObtenerDSDestinoFondos(DatosFormularioResultado formulario,
            int orden)
        {
            var destinosFondosPosibles = _destinoFondosRepositorio.ConsultarDestinosFondos();
            var dsDestinosFondos = new List<DestinoFondoReporteResultado>();
            foreach (var df in destinosFondosPosibles)
            {
                DestinoFondoResultado dfAux =
                    formulario.DestinosFondos.DestinosFondo.FirstOrDefault(x => x.Id == df.Id.Valor);
                bool seleccionado = dfAux != null;
                dsDestinosFondos.Add(new DestinoFondoReporteResultado(df.Nombre, CapFirst(df.Descripcion),
                    formulario.DestinosFondos.Observaciones, seleccionado)
                { Orden = orden });
            }
            return dsDestinosFondos;
        }

        private List<CondicionesSolicitadasResultado> ObtenerDSCondicionesMicroPrestamo(
            DatosFormularioResultado formulario, int orden)
        {
            CondicionesSolicitadasResultado condiciones = formulario.CondicionesSolicitadas;
            condiciones.Orden = orden;
            return new List<CondicionesSolicitadasResultado> { condiciones };
        }

        private AgrupacionCursosReporteResultado ObtenerDSCursos(DatosFormularioResultado formulario, int orden)
        {
            var cursos = _cursoRepositorio.Consultar();
            List<SolicitudCursoReporteResultado> dsCursosSalidaLaboral = new List<SolicitudCursoReporteResultado>();
            List<SolicitudCursoReporteResultado> dsCursosCapacitacion = new List<SolicitudCursoReporteResultado>();
            List<SolicitudCursoReporteResultado> dsCursosSalidaLaboral2 = new List<SolicitudCursoReporteResultado>();
            List<SolicitudCursoReporteResultado> dsCursosCapacitacion2 = new List<SolicitudCursoReporteResultado>();
            List<NombreDescripcionOtros> dsNombreDescripcionOtros = new List<NombreDescripcionOtros>();


            var solicitudesCursosSalidaLaboral =
                formulario.SolicitudesCurso.FirstOrDefault(x => x.NombreTipoCurso == "CURSOS CON SALIDA LABORAL");
            var solicitudesCursosCapacitacion =
                formulario.SolicitudesCurso.FirstOrDefault(x => x.NombreTipoCurso == "CURSOS DE CAPACITACIÓN");

            var cursosFiltradosSalidaLaboral = cursos.Where(x => x.TipoCurso.Id.Valor == 1).ToList();
            var cursosFiltradosCapacitacion = cursos.Where(x => x.TipoCurso.Id.Valor == 2).ToList();

            decimal calculoElementos = (decimal)cursosFiltradosSalidaLaboral.Count / 2;
            int cantPorTabla = (int)Math.Ceiling(calculoElementos);
            int i = 1;
            int tabla = 1;

            foreach (var curso in cursosFiltradosSalidaLaboral)
            {
                ConsultarCursosResultado solicitoCurso =
                    solicitudesCursosSalidaLaboral?.Cursos.FirstOrDefault
                    (x => x.Id == curso.Id.Valor);
                if (tabla == 1)
                    dsCursosSalidaLaboral.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                        curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, solicitoCurso != null)
                    { Orden = orden });
                if (tabla == 2)
                    dsCursosSalidaLaboral2.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                        curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, solicitoCurso != null)
                    { Orden = orden });

                if (i == cantPorTabla)
                {
                    i = 1;
                    tabla++;
                }
                else
                {
                    i++;
                }
            }

            dsNombreDescripcionOtros.Add(new NombreDescripcionOtros(solicitudesCursosSalidaLaboral?.Descripcion, solicitudesCursosCapacitacion?.Descripcion));

            if (i != 1)
            {
                while (i <= cantPorTabla)
                {
                    if (tabla == 1)
                        dsCursosSalidaLaboral.Add(new SolicitudCursoReporteResultado { Orden = orden });
                    if (tabla == 2)
                        dsCursosSalidaLaboral2.Add(new SolicitudCursoReporteResultado { Orden = orden });
                    i++;
                }
            }

            calculoElementos = (decimal)cursosFiltradosCapacitacion.Count / 2;
            cantPorTabla = (int)Math.Ceiling(calculoElementos);
            i = 1;
            tabla = 1;

            foreach (var curso in cursosFiltradosCapacitacion)
            {
                ConsultarCursosResultado solicitoCurso =
                    solicitudesCursosCapacitacion?.Cursos.FirstOrDefault(x => x.Id == curso.Id.Valor);
                if (tabla == 1)
                    dsCursosCapacitacion.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                        curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, solicitoCurso != null)
                    { Orden = orden });
                if (tabla == 2)
                    dsCursosCapacitacion2.Add(new SolicitudCursoReporteResultado(CapFirst(curso.Nombre),
                        curso.TipoCurso.Id.Valor, curso.TipoCurso.Nombre, solicitoCurso != null)
                    { Orden = orden });

                if (i == cantPorTabla)
                {
                    i = 1;
                    tabla++;
                }
                else
                {
                    i++;
                }
            }

            if (i != 1)
            {
                while (i <= cantPorTabla)
                {
                    if (tabla == 1)
                        dsCursosCapacitacion.Add(new SolicitudCursoReporteResultado { Orden = orden });
                    if (tabla == 2)
                        dsCursosCapacitacion2.Add(new SolicitudCursoReporteResultado { Orden = orden });
                    i++;
                }
            }

            return new AgrupacionCursosReporteResultado
            {
                CursosConSalidaLaboral = dsCursosSalidaLaboral,
                CursosDeCapacitacion = dsCursosCapacitacion,
                CursosConSalidaLaboral2 = dsCursosSalidaLaboral2,
                CursosDeCapacitacion2 = dsCursosCapacitacion2,
                NombreDescripcion = dsNombreDescripcionOtros
            };
        }

        private List<DatosPersonalesResultado> ObtenerDSGarante(DatosFormularioResultado formulario, int orden)
        {
            DatosPersonaResultado garante;
            if (formulario.Garantes.ToList().Count > 0)
            {
                garante = formulario.Garantes.ToList()[0];
            }
            else
            {
                garante = null;
            }
            var garantePersona = garante != null
                ? ObtenerDatosPersonalesCompleto(garante)
                : new DatosPersonalesResultado { CodigoArea = "            ", CodigoAreaCelular = "            " };
            garantePersona.Orden = orden;
            return new List<DatosPersonalesResultado>() { garantePersona };
        }

        #endregion

        private DatosPersonalesResultado ObtenerDatosPersonalesCompleto(DatosPersonaResultado persona)
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
            var datosContactoSolicitante = ObtenerDatosContacto(consultaDatosContacto);

            var resultado =
                _grupoUnicoServicio.GetApiConsultaDatosPersonales(persona.SexoId, persona.NroDocumento,
                    persona.CodigoPais, persona.IdNumero);

            resultado.CodigoArea = datosContactoSolicitante.CodigoArea;
            resultado.Telefono = datosContactoSolicitante.Telefono;
            resultado.CodigoAreaCelular = datosContactoSolicitante.CodigoAreaCelular;
            resultado.Celular = datosContactoSolicitante.Celular;
            resultado.Email = string.IsNullOrEmpty(resultado.Email) ? datosContactoSolicitante.Mail : resultado.Email;
            resultado.DomicilioGrupoFamiliarLocalidad = datosSolicitante.DomicilioGrupoFamiliarLocalidad;
            resultado.DomicilioGrupoFamiliarDepartamento = datosSolicitante.DomicilioGrupoFamiliarDepartamento;
            resultado.DomicilioGrupoFamiliar = datosSolicitante.DomicilioGrupoFamiliar;

            return resultado;
        }

        public IList<RequisitosCuadranteResultado> ConsultarRequisitosParaCuadrantePorIdLinea(Id lineaId)
        {
            var requisitosBase = _lineaPrestamoRepositorio.ConsultarRequisitosPorIdLinea(lineaId, true);
            var lista = new List<RequisitosCuadranteResultado>();

            foreach (var requisitoDb in requisitosBase)
            {
                if (requisitoDb.IdTipoItem.Valor == 2 || requisitoDb.IdTipoItem.Valor == 3)
                {
                    var requisitoResultado = new RequisitosCuadranteResultado
                    {
                        Descripcion = requisitoDb.NombreItem,
                        EsSolicitante = requisitoDb.IdTipoItem.Valor == 2,
                        EsGarante = requisitoDb.IdTipoItem.Valor == 3
                    };
                    lista.Add(requisitoResultado);
                }
            }
            return lista;
        }
        public IList<IntegrantesResultado> ObtenerDSIntegrantes(int idFormulario, int orden, List<AgruparFormulario> integrantes, DatosPersonaResultado solicitante)
        {
            var lista = new List<IntegrantesResultado>();
            integrantes = integrantes.Where((x) => x.NroDocumento != solicitante.NroDocumento).ToList();

            for (int i = 0; i < integrantes.Count; i++)
            {
                var datosDomicilio = _grupoUnicoServicio.GetApiConsultaDatosCompleto
                (integrantes[i].Sexo,
                    integrantes[i].NroDocumento,
                    integrantes[i].Pais,
                    integrantes[i].IdNumero);

                var datosPersonales = _grupoUnicoServicio.GetApiConsultaDatosPersonales(
                        integrantes[i].Sexo,
                        integrantes[i].NroDocumento,
                        integrantes[i].Pais,
                        integrantes[i].IdNumero);

                var datosContacto = ObtenerDatosContacto(new DatosPersonaConsulta
                {
                    IdSexo = datosPersonales.SexoId,
                    NroDocumento = datosPersonales.NroDocumento,
                    CodigoPais = datosPersonales.CodigoPais,
                    IdNumero = datosPersonales.IdNumero.ToString()
                });

                var integranteResultado = new IntegrantesResultado(datosPersonales,
                    orden, datosContacto,
                    datosDomicilio.DomicilioGrupoFamiliarDepartamento,
                    datosDomicilio.DomicilioGrupoFamiliarLocalidad);
                lista.Add(integranteResultado);

            }
            return lista;
        }
        public IList<OngResultado> ObtenerDSOng(int idFormulario, int orden)
        {
            var ongF = _formularioRepositorio.ObtenerOngDeFormulario(idFormulario);
            OngResultado ong;

            if (ongF != null)
            {
                ong = new OngResultado(orden, ongF.IdOng, idFormulario, ongF.NombreGrupo, ongF.NumeroGrupo);
            }
            else
            {
                ong = new OngResultado(orden);
            }
            var lista = new List<OngResultado>();
            lista.Add(ong);
            return lista;
        }

        public async Task<string> ObtenerReporteFormularioVacio(decimal? idDetalleLinea, decimal? idLinea)
        {
            string motivoError = "";
            try
            {
                //DATOS FORMULARIO PRINCIPAL
                motivoError = "obtener los datos de la línea";
                LineaPrestamoResultado datosLinea = _lineaPrestamoRepositorio.ConsultarLineaPorId(new Id(idLinea.GetValueOrDefault()));
                var dsFormulario = new List<LineaPrestamoResultado>() { datosLinea };

                //IMAGEN ENCABEZADO
                string rutaImagenEncabezado = !string.IsNullOrEmpty(datosLinea.Logo) ? datosLinea.Logo : "";

                //TODO: DATOS DE LOS CUADRANTES Y ORDEN
                motivoError = "obtener los cuadrantes";
                var cuadrantesLinea = _lineaPrestamoRepositorio.ObtetenerCuadrantesPorDetalleLinea(idDetalleLinea.GetValueOrDefault())
                    .Where(x => x.IdTipoSalida != 1)
                    .OrderBy(x => x.Orden);

                //TODO: ARMADO DEL REPORTE
                var subReportesOrdenados = new List<SubReporte>();

                int orden = 1;
                //AGREGADO EN ORDEN DE LOS SUBREPORTES
                foreach (var cuadrante in cuadrantesLinea)
                {
                    var subRep = ObtenerSubReporteParaReporteVacioPorId(cuadrante.Id, orden, idLinea.GetValueOrDefault());
                    if (subRep != null)
                    {
                        subReportesOrdenados.Add(subRep);
                        orden++;
                    }
                }

                //ASIGNANDO PIE DE FORMULARIO
                var dsPie = new List<ReportePieDePaginaFormularioResultado>()
                {
                    new ReportePieDePaginaFormularioResultado {UrlImagenPie = datosLinea.PiePagina}
                };
                motivoError = "generar el reporte";
                var subReportePie = new SubReporte(new Id(0), "ReporteFormulario_Pie", "SubReportePie",
                    "ReporteFormulario_Pie.rdlc", new List<string>() { "DSDetalleLinea", "DSPie" });
                subReportePie.AsignarDataSet(dsFormulario)
                    .AsignarDataSet(dsPie);
                subReportesOrdenados.Add(subReportePie);


                var reportData = new ReporteBuilder("ReporteFormulario.rdlc", "SubReporteFormulario")
                    .AgregarDataSource("DSReporteFormulario", dsFormulario)
                    .AgregarDataSource("DSValidacionEdad", new List<ValidacionEdadResultado>())
                    .AgregarParametro("pImagenEncabezado", rutaImagenEncabezado)
                    .AgregarParametro("pMotivosRechazo", "")
                    .AgregarParametro("pObservacionesRechazo", "")
                    .AgregarParametro("pSUAC", "")
                    .AgregarParametro("pMostrarValidacionEdades", false)
                    .SubReporteDS(subReportesOrdenados)
                    .GenerarConSubReporte();

                var url = ReporteBuilder.GenerarUrlReporte(reportData, "ReporteFormulario_" + datosLinea.Nombre);
                return url;
            }
            catch (Exception e)
            {
                throw new ModeloNoValidoException("Error al " + motivoError);
            }
        }

        private List<ValidacionEdadResultado> ValidarEdadIntegrantes(IList<DatosPersonaResultado> integrantes)
        {
            List<ValidacionEdadResultado> resultado = new List<ValidacionEdadResultado>();
            var vigenciaParametroEdadMinima =
                _parametrosRepositorio.ObtenerValorVigenciaParametroPorFecha(new Id(6), null);
            var vigenciaParametroEdadMaxima =
                _parametrosRepositorio.ObtenerValorVigenciaParametroPorFecha(new Id(5), null);
            var edadMinima = Parse(vigenciaParametroEdadMinima.Valor);
            var edadMaxima = Parse(vigenciaParametroEdadMaxima.Valor);

            foreach (var integrante in integrantes)
            {
                var persona = ObtenerDatosPersonalesCompleto(integrante);

                if (string.IsNullOrEmpty(persona.FechaDefuncion))
                {
                    if (string.IsNullOrEmpty(persona.Edad))
                    {
                        if (string.IsNullOrEmpty(persona.FechaNacimiento))
                        {
                            resultado.Add(
                                new ValidacionEdadResultado("El integrante " + persona.Apellido + ", " +
                                                            persona.Nombre +
                                                            " no tiene fecha de nacimiento."));
                        }
                    }
                    else
                    {
                        var edad = Parse(persona.Edad);
                        if (edad > edadMaxima || edad < edadMinima)
                        {
                            resultado.Add(
                                new ValidacionEdadResultado("El integrante " + persona.Apellido + ", " +
                                                            persona.Nombre +
                                                            " tiene " + edad +
                                                            " años. No cumple con las edades definidas en los parámetros."));
                        }
                    }
                }
                else
                {
                    resultado.Add(
                        new ValidacionEdadResultado("El integrante " + persona.Apellido + ", " +
                                                    persona.Nombre +
                                                    " se encuentra registrado como FALLECIDO."));
                }
            }
            return resultado;
        }

        public string ActualizaSucursal(ActualizaSucursalComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            return _formularioRepositorio.ActualizaSucursal(comando.IdsFormularios, comando.IdBanco, comando.IdSucursal,
                usuario.Id.Valor);
        }

        public Resultado<FormularioFiltradoResultado> ConsultaFormularios(FiltrosFormularioConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new FiltrosFormularioConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _formularioRepositorio.ConsultaFormulariosFiltros(consulta);
        }

        public IList<FormularioFiltradoResultado> ConsultaFormulariosSinPaginar(FiltrosFormularioConsulta consulta)
        {
            return _formularioRepositorio.ConsultaIdsFormulariosFiltros(consulta);
        }

        public string ConsultaTotalizadorSucursalBancaria(FiltrosFormularioConsulta consulta)
        {
            return _formularioRepositorio.ConsultaTotalizadorSucursalBancaria(consulta);
        }


        public string ConsultaTotalizadorDocumentacion(FiltrosFormularioConsulta consulta)
        {
            return _formularioRepositorio.ConsultaTotalizadorDocumentacion(consulta);
        }

        public Resultado<FormularioFiltradoResultado> ConsultaFormulariosSucursal(FiltrosFormularioConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new FiltrosFormularioConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _formularioRepositorio.ConsultaFormulariosSucursalFiltros(consulta);
        }

        public IList<FormularioFiltradoResultado> ConsultaFormulariosSucursalSinPaginar(FiltrosFormularioConsulta consulta)
        {
            return _formularioRepositorio.ConsultaIdsFormulariosSucursalFiltros(consulta);
        }

        public string ActualizarPatrimonioSolicitante(int idFormulario, PatrimonioSolicitanteComando comando)
        {
            var patrimonioSolicitante = new PatrimonioSolicitante(comando.InmueblePropio, comando.ValorInmueble ?? 0, comando.VehiculoPropio,
                comando.CantVehiculos ?? 0, comando.ModeloVehiculos, comando.ValorVehiculos ?? 0, comando.ValorDeudas ?? 0);
            return _formularioRepositorio.RegistrarPatrimonioSolicitante(
                idFormulario, _sesionUsuario.Usuario.Id.Valor, ArmarStringParaBdPatrimonioSolicitante(patrimonioSolicitante));
        }

        public string ActualizarPatrimonioSolicitanteAsociativas(int idAgrupamiento, PatrimonioSolicitanteComando comando)
        {
            var patrimonioSolicitante = new PatrimonioSolicitante(comando.InmueblePropio, comando.ValorInmueble ?? 0, comando.VehiculoPropio,
                comando.CantVehiculos ?? 0, comando.ModeloVehiculos, comando.ValorVehiculos ?? 0, comando.ValorDeudas ?? 0);

            var idFormularios = _formularioRepositorio.ObtenerIdsFormulariosAgrupamiento(idAgrupamiento);
            var res = "";
            foreach (var idFormulario in idFormularios)
            {
                res = _formularioRepositorio.RegistrarPatrimonioSolicitante(idFormulario.Id, _sesionUsuario.Usuario.Id.Valor, ArmarStringParaBdPatrimonioSolicitante(patrimonioSolicitante));
            }
            return res;
        }

        private static string ArmarStringParaBdPatrimonioSolicitante(PatrimonioSolicitante comando)
        {
            var str = "1;" + (comando.InmueblePropio ? "S" : "N") + ';';
            str += "2;" + comando.ValorInmueble + ";";
            str += "3;" + (comando.VehiculoPropio ? "S" : "N") + ';';
            str += "4;" + comando.CantidadVehiculos + ";";
            str += "5;" + comando.ModeloVehiculos + ";";
            str += "6;" + comando.ValorVehiculos + ";";
            str += "7;" + comando.ValorDeudas;
            return str;
        }

        public PatrimonioSolicitanteResultado ObtenerPatrimonioSolicitante(int idFormulario)
        {
            return _formularioRepositorio.ObtenerPatrimonioSolicitante(idFormulario);
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


        public List<ReporteDeudaGrupoConvivienteResultado> ObtenerDSDeudaGrupoFamiliar(DatosFormularioResultado formulario, RespuestaAPIGrupoFamiliar grupoFamiliar)
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
                dsGrupoFamiliar.Add(persona);
            }

            var ds = new List<ReporteDeudaGrupoConvivienteResultado>();
            foreach (var persona in dsGrupoFamiliar)
            {
                ds.Add(new ReporteDeudaGrupoConvivienteResultado
                {
                    NombreCompleto = persona.NombreCompleto,
                    TipoDocumento = "DNI",
                    NumeroDocumento = persona.NroDocumento,
                    Sexo = persona.Sexo,
                    FechaNacimiento = persona.FechaNacimiento,
                    Edad = persona.Edad,
                    NumeroFormulario = formulario.Id.ToString(),
                    FechaUltimoMovimiento = " - ",
                    //TODO modificar tamaño de campo
                    PrestamoBeneficio = formulario.DetalleLinea.Descripcion + " (" + formulario.DetalleLinea.Nombre + ")",
                    Importe = formulario.CondicionesSolicitadas.MontoEstimadoCuota.ToString(),
                    CantCuotasPagas = " - ",
                    CantCuotasImpagas = " - ",
                    CantCuotasVencidas = " - ",
                    MotivoBaja = " - ",
                    Estado = " - ",
                    FechaDefuncion = " - "

                });
            }

            return ds;
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

                    ds.Add(new ReporteDeudaGrupoConvivienteResultado
                    {
                        NombreCompleto = persona.NombreCompleto,
                        TipoDocumento = "DNI",
                        NumeroDocumento = persona.NroDocumento,
                        Sexo = persona.Sexo,
                        FechaNacimiento = persona.FechaNacimiento,
                        Edad = persona.Edad,
                        NumeroFormulario = formulario.NroFormulario.ToString(),
                        //FechaUltimoMovimiento = " - ",
                        PrestamoBeneficio = datosFormulario.DetalleLinea.Descripcion + " (" + datosFormulario.DetalleLinea.Nombre + ")",
                        Importe = datosFormulario.CondicionesSolicitadas.MontoEstimadoCuota.ToString(),
                        //CantCuotasPagas = " - ",
                        //CantCuotasImpagas = " - ",
                        //CantCuotasVencidas = " - ",
                        //MotivoBaja = " - ",
                        //Estado = " - ",
                        //FechaDefuncion = " - "

                    }.CargarDatosDeuda(datosDeuda));
                }
            }

            return ds;
        }

        public IList<ItemInversionResultado> ObtenerItemsInversion()
        {
            var resultados = _detalleInversionEmprendimientoServicio.ObtenerItemsInversion();
            return resultados;
        }

        public IList<FuenteFinanciamientoResultado> ObtenerFuentesFinanciamiento()
        {
            var resultados = _inversionEmprendimientoServicio.ObtenerFuentesFinanciamiento();
            return resultados;
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

        private ConsultaDeudaFormularioResultado ConsultarDatosDeudaFormulario(int idFormulario)
        {
            var consulta = _formularioRepositorio.ObtenerDatosDeudaFormulario(idFormulario);
            int elementos = consulta.ToList().Count;
            if (elementos == 0) return new ConsultaDeudaFormularioResultado();
            if (elementos == 1)
            {
                consulta[0].MotivosRechazo = new List<int>() { consulta[0].MotivoRechazo };
                return consulta[0];
            }
            ConsultaDeudaFormularioResultado res = consulta[0];
            res.MotivosRechazo = new List<int>();
            foreach (var x in consulta)
            {
                res.MotivosRechazo.Add(x.MotivoRechazo);
            }
            return res;
        }

        public EmprendimientoResultado ObtenerDatosEmprendimientoPorFormulario(int id)
        {
            return _formularioRepositorio.ObtenerDatosEmprendimiento(id);
        }

        private IList<MiembroEmprendimientoFormularioResultado> ObtenerMiembrosEmprendimiento(Id idEmprendimiento)
        {
            List<MiembroEmprendimientoFormularioResultado> res = new List<MiembroEmprendimientoFormularioResultado>();
            var miembrosDb = _formularioRepositorio.ObtenerMiembrosEmprendimiento(new Emprendimiento((int)idEmprendimiento.Valor, -1));
            var vinculos = _emprendimientoRepositorio.ObtenerVinculos();
            foreach (var miembro in miembrosDb)
            {
                DatosPersonalesResultado per = new DatosPersonalesResultado
                {
                    CodigoPais = miembro.CodigoPais,
                    NroDocumento = miembro.NroDocumento,
                    SexoId = miembro.IdSexo
                };
                per = _grupoUnicoServicio.GetApiConsultaDatosPersonales(per.SexoId, per.NroDocumento, per.CodigoPais, per.IdNumero);
                MiembroEmprendimientoFormularioResultado miembroEmp = new MiembroEmprendimientoFormularioResultado(per, miembro.IdVinculo, vinculos.FirstOrDefault(x => x.Id == miembro.IdVinculo)?.Nombre, miembro.Tarea, miembro.HorarioTrabajo, miembro.Remuneracion, null, false);
                miembroEmp.Id = miembro.Id;
                res.Add(miembroEmp);
            }
            return res;
        }

        public void ActualizarDeudaEmprendimiento(int idFormulario, List<RegistrarDeudaEmprendimientoComando> comandos)
        {
            var datosEmprendimiento = _formularioRepositorio.ObtenerDatosEmprendimiento(idFormulario);
            if (datosEmprendimiento == null)
            {
                throw new ModeloNoValidoException("Es necesario cargar un emprendimiento primero.");
            }
            _deudaEmprendimientoServicio.ActualizarDeudaEmprendimiento((int)datosEmprendimiento.Id.Valor, comandos,
                (int)_sesionUsuario.Usuario.Id.Valor);
        }

        public IList<FormulariosInversionRealizadaResultado> ActualizarInversionesRealizadas(int idFormulario, List<RegistrarInversionRealizadaComando> comandos)
        {
            var datosEmprendimiento = _formularioRepositorio.ObtenerDatosEmprendimiento(idFormulario);
            if (datosEmprendimiento == null)
            {
                throw new ModeloNoValidoException("Es necesario cargar un emprendimiento primero.");
            }
            return _detalleInversionEmprendimientoServicio.RegistrarInversionesRealizadas(datosEmprendimiento.Id, comandos);
        }

        public void ActualizarNecesidadesInversion(int idFormulario, RegistrarNecesidadInversionComando comandos)
        {
            var datosEmprendimiento = _formularioRepositorio.ObtenerDatosEmprendimiento(idFormulario);
            if (datosEmprendimiento == null)
            {
                throw new ModeloNoValidoException("Es necesario cargar un emprendimiento primero.");
            }
            _inversionEmprendimientoServicio.RegistrarNecesidadInversion(datosEmprendimiento.Id, comandos);
        }

        public void ActualizarDatosOrganizacionEmprendimiento(int idFormulario,
            OrganizacionEmprendimientoComando comando)
        {
            if (comando.Id == 0) throw new ModeloNoValidoException("El id es requerido");
            var emprendimiento = new Emprendimiento(comando.Id, comando.IdTipoOrganizacion ?? -1);
            decimal idUsuario = _sesionUsuario.Usuario.Id.Valor;
            var miembrosDb = ObtenerMiembrosEmprendimiento(emprendimiento.Id);

            if (comando.IdTipoOrganizacion.HasValue)
            {
                _formularioRepositorio.ActualizarDatosEmprendimiento(idFormulario, idUsuario, emprendimiento);
            }

            bool seEliminaron = false;
            foreach (var miembro in miembrosDb)
            {
                bool sigueEstando = comando.Miembros.FirstOrDefault(x => x.Persona.EsMismaPersona(miembro.Persona)) != null;
                if (!sigueEstando)
                {
                    //BAJA
                    _formularioRepositorio.EliminarMiembroEmprendimiento(emprendimiento, miembro, idUsuario);
                    seEliminaron = true;
                }
            }

            //Si se eliminaron miembros actualiza el listado de miembros sin los eliminados
            if (seEliminaron)
                miembrosDb = ObtenerMiembrosEmprendimiento(emprendimiento.Id);

            foreach (var miembro in comando.Miembros)
            {
                if (miembro.EsSolicitante) miembro.IdVinculo = null;
                bool existeMiembro = miembrosDb.FirstOrDefault(x => x.Persona.EsMismaPersona(miembro.Persona)) != null;
                if (existeMiembro)
                {
                    //MODIFICACION
                    _formularioRepositorio.ActualizarMiembroEmprendimiento(emprendimiento, miembro, idUsuario);
                    continue;
                }
                //ALTA
                _formularioRepositorio.RegistrarMiembroEmprendimiento(emprendimiento, miembro, idUsuario);
            }
        }

        public IList<IngresoGrupoResultado> ObtenerIngresosGrupoFamiliar()
        {
            return _formularioRepositorio.ObtenerIngresosGrupoFamiliar();
        }

        public IList<IngresoGrupoResultado> ObtenerIngresosGrupoFamiliarFormulario(int idFormulario)
        {
            var ingresos = _formularioRepositorio.ObtenerIngresosGrupoFamiliar();
            var ingresosFormulario = _formularioRepositorio.ObtenerIngresosGrupoFamiliarFormulario(idFormulario);

            foreach (var ing in ingresosFormulario)
            {
                var ingGenerico = ingresos.FirstOrDefault(x => x.Id == ing.Id);
                if (ingGenerico != null)
                {
                    ingGenerico.Valor = ing.Valor;
                }
            }

            return ingresos;
        }

        public IList<EgresoGrupoResultado> ObtenerGastosGrupoFamiliar(int idGrupo)
        {
            return _formularioRepositorio.ObtenerGastosGrupoFamiliar(idGrupo);
        }

        public decimal ObtenerIngresoTotalGrupoFamiliar(int idGrupo)
        {
            return _formularioRepositorio.ObtenerIngresoTotalGrupoFamiliar(idGrupo);
        }

        public decimal RegistrarIngresosGrupoFamiliar(RegistrarIngresosGrupoFamiliarComando comando)
        {
            StringBuilder ingresos = new StringBuilder();
            StringBuilder gastos = new StringBuilder();
            char separador = ';';

            foreach (var ing in comando.IngresosGrupo)
            {
                switch (ing.IdConcepto)
                {
                    case (int)TipoConcepto.Enum.Ingreso:
                        if (!string.IsNullOrEmpty(ingresos.ToString()))
                            ingresos.Append(separador);
                        ingresos.Append(ing.Id)
                                .Append(separador)
                                .Append(ing.Valor ?? 0);
                        break;
                    case (int)TipoConcepto.Enum.Gasto:
                        if (!string.IsNullOrEmpty(gastos.ToString()))
                            gastos.Append(separador);
                        gastos.Append(ing.Id)
                                .Append(separador)
                                .Append(ing.Valor ?? 0);
                        break;
                }
            }

            return _formularioRepositorio.RegistrarIngresosGrupoFamiliar(comando.IdFormulario, ingresos.ToString(), gastos.ToString(), _sesionUsuario.Usuario.Id.Valor);
        }

        private MercadoComercializacionResultado ObtenerMercadoComercializacion(int idFormulario)
        {
            var formasPago = _formularioRepositorio.ObtenerFormasPagoPorFormulario(idFormulario);
            var estima = _formularioRepositorio.ObtenerEstimaClientesPorFormulario(idFormulario);
            var itemsMercadoComer = ItemsMercadoComercializacionAObjeto(_formularioRepositorio.ObtenerItemsMercadoComercializacionPorFormulario(idFormulario));
            return new MercadoComercializacionResultado() { FormasPago = formasPago, EstimaClientes = estima, ItemsPorCategoria = itemsMercadoComer };
        }

        private static IEnumerable<ItemsMercadoComerResultado> ItemsMercadoComercializacionAObjeto(IEnumerable<ItemsMercadoComerString> listaString)
        {
            var lista = new List<ItemsMercadoComerResultado>();
            foreach (var itemString in listaString)
            {
                var objeto = new ItemsMercadoComerResultado
                {
                    Descripcion = itemString.Otros,
                    TipoItem = itemString.TipoItem,
                    Items = new List<int>()
                };
                var listaItems = itemString.Items.Split(',');
                foreach (var idItem in listaItems)
                {
                    objeto.Items.Add(Parse(idItem));
                }
                lista.Add(objeto);
            }

            return lista;
        }

        public Resultado<SituacionPersonasResultado> ObtenerSituacionPersona(SituacionPersonasConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new SituacionPersonasConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _formularioRepositorio.ObtenerSituacionPersonas(consulta);
        }
        public IList<SituacionPersonasResultadoVista> ObtenerVistaSituacionPersona(SituacionPersonasConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new SituacionPersonasConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _formularioRepositorio.ObtenerVistaSituacionPersonas(consulta);
        }

        public Resultado<FormulariosSituacionResultado> ObtenerFormulariosSituacionPersona(SituacionPersonasConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new SituacionPersonasConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _formularioRepositorio.ObtenerFormulariosSituacionPersonas(consulta);
        }

        public IList<MotivosRechazoFormularioResultado> ObtenerRechazosFormulario(decimal idFormulario)
        {
            var listaMotivos = _formularioRepositorio.ObtenerRechazosFormulario(idFormulario);
            return listaMotivos.OrderBy(x => x.IdSeguimientoFormulario).ToList();
        }

        public decimal ActualizarCuadrantePrecioVenta(int idFormulario, ActualizarPrecioVentaComando comando)
        {
            var idProducto = _formularioRepositorio.ActualizarDatosGeneralesCuadrantePrecioVenta(comando.IdProducto, comando.IdEmprendimiento, comando.UnidadesEstimadas, comando.GananciaEstimada ?? 0, comando.Producto);
            foreach (var costo in comando.Costos)
            {
                _formularioRepositorio.RegistrarCostos(costo.Id, idProducto, costo.IdItem, costo.PrecioUnitario, costo.ValorMensual, costo.Detalle);
            }
            foreach (var idCosto in comando.CostosAEliminar)
            {
                _formularioRepositorio.EliminarCostos(idCosto);
            }

            return idProducto;
        }

        public PrecioVentaResultado ObtenerPrecioVenta(decimal idEmprendimiento)
        {
            var resultado = _formularioRepositorio.VerCuadrantePrecioVenta(idEmprendimiento);
            if (resultado == null) return null;

            //Se hace el seteo de ID producto desde el "precio venta" porque es mas representativo, buscando evitar reinterpretaciones a futuro ante cualquier cambio.
            resultado.IdProducto = resultado.PrecioVenta;
            resultado.PrecioVenta = null;
            var costosSinNombre = _formularioRepositorio.ObtenerCostoPrecioVentaPorFormulario(idEmprendimiento);
            resultado.Costos = ObtenerNombreCostosPrecioVenta(costosSinNombre);
            return resultado;
        }

        private IList<ItemsPrecioVentaResultado> ObtenerNombreCostosPrecioVenta(IList<ItemsPrecioVentaResultado> costos)
        {
            var itemsPrecioVenta = _emprendimientoRepositorio.ObtenerItemsPrecioVenta();
            foreach (var costo in costos)
            {
                costo.Nombre = itemsPrecioVenta.First((x) => x.Id == costo.IdItem).Nombre;
            }
            return costos;
        }

        public IList<ItemsPrecioVentaResultado> ObtenerCostosPrecioVenta(int idFormulario)
        {
            var datosEmprendimiento = _formularioRepositorio.ObtenerDatosEmprendimiento(idFormulario);
            var resultado = ObtenerPrecioVenta(datosEmprendimiento.Id.Valor);
            return resultado.Costos;
        }

        public IList<MotivosRechazoReferenciaResultado> ObtenerMotivosRechazoReferencia(
            MotivosRechazoReferenciaConsulta consulta)
        {
            string[] consultas;

            if (consulta != null)
                consultas = consulta.Ids.Split(';');
            else
                return new List<MotivosRechazoReferenciaResultado>();

            return _formularioRepositorio.ObtenerMotivosRechazoReferencia(consultas);
        }

        public IList<AgruparFormulario> ObtenerFormulariosConAgrupamiento(int idAgrupamiento)
        {
            var listadoAgrupado = _formularioRepositorio.ObtenerFormulariosConAgrupamiento(idAgrupamiento);

            foreach (var integrante in listadoAgrupado)
            {
                integrante.Estado = EstadoFormulario.ConId(integrante.IdEstado.GetValueOrDefault()).Descripcion;
            }
            return listadoAgrupado;
        }

        public ArchivoBase64 ObtenExcelPersonaResultado(SituacionPersonasConsulta consultaPersonas)
        {
            consultaPersonas.TamañoPagina = Parse(ParametrosSingleton.Instance.GetValue("4"));


            var reportData = new ReporteBuilder("ReporteSituacionPersona.rdlc")
                .AgregarDataSource("CabeceraPersona", _formularioRepositorio.ObtenerSituacionPersonas(consultaPersonas).Elementos)
                .AgregarDataSource("CuerpoFormularios",
                    _formularioRepositorio.ObtenerFormulariosCompletosSituacionPersonas(consultaPersonas))
                .SeleccionarFormato("Excel")
                .Generar();

            return new ArchivoBase64(Convert.ToBase64String(reportData.Content), TipoArchivo.Excel, $"reporte_situacion_personas_{consultaPersonas.Dni}");
        }

        public string ObtenerEstadoConId(int idEstado)
        {
            return EstadoFormulario.ConId(idEstado).Descripcion;
        }

        public decimal ObtenerIdAgrupamiento(int idFormulario)
        {
            return _formularioRepositorio.ObtenerIdAgrupamiento(idFormulario);
        }

        public bool ValidarEstadosFormulariosAgrupados(int idAgrupamiento)
        {
            return _formularioRepositorio.ValidarEstadosFormulariosAgrupados(idAgrupamiento, 2); //2= Completado
        }


        public IList<OngComboResultado> ObtenerOngs(int idLinea)
        {
            var resultado = _formularioRepositorio.ObtenerOngs(idLinea);
            var lsOngComboResultado = new List<OngComboResultado>();
            foreach (var res in resultado)
            {
                var ong = new OngComboResultado()
                {
                    IdEntidad = Decimal.ToInt32(res.IdLineaOng.Value),
                    Nombre = res.NombreOng
                };
                lsOngComboResultado.Add(ong);
            }
            return lsOngComboResultado;
        }

        public decimal ObtenerNumeroGrupo(OngFormulario comando)
        {
            return _formularioRepositorio.ObtenerNumeroGrupo(comando.IdOng, comando.NombreGrupo, _sesionUsuario.Usuario.Id.Valor);
        }

        public void RegistrarOngParaFormulario(int idAgrupamiento, OngFormulario comando)
        {
            var idFormularios = _formularioRepositorio.ObtenerIdsFormulariosAgrupamiento(idAgrupamiento);
            foreach (var idFormulario in idFormularios)
            {
                _formularioRepositorio.RegistrarOngParaFormulario(comando, idFormulario.Id, _sesionUsuario.Usuario.Id.Valor);
            }
        }

        public bool CargarNumerosControlInterno(CargarNumerosControlInternoComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            return _formularioRepositorio.RegistrarSticker(comando.IdFormulario, comando.NumeroSticker, usuario.Id);
        }

        public FormularioFechaPagoResultado ObtenerFormularioFechaPago(decimal idFormulario)
        {
            return _formularioRepositorio.ObtenerFormularioFechaPago(idFormulario);
        }

        public decimal ObtenerTiempoReprogramacion()
        {
            var vigenciaParametroFechaReprogramacion = _parametrosRepositorio.ObtenerValorVigenciaParametroPorFecha(new Id(21), null);
            return Decimal.Parse(vigenciaParametroFechaReprogramacion.Valor);
        }

        public IList<Reprogramacion> ObtenerHistorialReprogramacion(int idFormulario)
        {
            return _formularioRepositorio.ObtenerHistorialReprogramacion(idFormulario);
        }

        public bool CambiarGaranteFormulario(CambiarGaranteComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            return _formularioRepositorio.CambiarGaranteFormulario(comando, usuario.Id);
        }

        public ArchivoBase64 ObtenExcelBandejaformularios(FormularioGrillaConsulta consulta)
        {
            var datosGrilla = _formularioRepositorio.ObtenerFormulariosReporte(consulta);

            var reportData = new ReporteBuilder("ReporteFormulario_BandejaExcel.rdlc")
                .AgregarDataSource("DSBandejaFormularios", datosGrilla)
                .SeleccionarFormato("Excel")
                .Generar();

            return new ArchivoBase64(Convert.ToBase64String(reportData.Content), TipoArchivo.Excel, $"reporte_bandeja_formularios_excel");
        }


        public ReporteResultado ObtenPDFBandejaformularios(FormularioGrillaConsulta consulta)
        {
            var noAplica = "-";
            var datosGrilla = _formularioRepositorio.ObtenerFormulariosReporte(consulta);
            var datosCombo = _formularioRepositorio.ObtenerImpresionConsulta(consulta);
            var nombreReporte = "reporte_bandeja_formularios";

            var fechaDesde = consulta.FechaDesde.ToString("dd/MM/yyyy");
            var fechaHasta = consulta.FechaHasta.ToString("dd/MM/yyyy");
            var contadorLocalidad = consulta.LocalidadIds?.Count(x => x == ',') + 1;
            var contadorDepartamento = consulta.DepartamentoIds?.Count(x => x == ',') + 1;
            var contadorEstado = consulta.IdEstadoFormulario?.Count(x => x == ',') + 1;
            var datosConsulta = new ImpresionConsultaFormularioResultado()
            {
                FechaDesde = fechaDesde.Equals("01/01/0001") ? noAplica : fechaDesde,
                FechaHasta = fechaHasta.Equals("01/01/0001") ? noAplica : fechaHasta,
                Nombre = string.IsNullOrEmpty(consulta.Nombre) ? noAplica : consulta.Nombre,
                Apellido = string.IsNullOrEmpty(consulta.Apellido) ? noAplica : consulta.Apellido,
                Dni = string.IsNullOrEmpty(consulta.Dni) ? noAplica : consulta.Dni,
                Cuil = string.IsNullOrEmpty(consulta.Cuil) ? noAplica : consulta.Cuil,
                NumeroFormulario = string.IsNullOrEmpty(consulta.NumeroFormulario) ? noAplica : consulta.NumeroFormulario,
                NroPrestamo = string.IsNullOrEmpty(consulta.NumeroPrestamo) ? noAplica : consulta.NumeroPrestamo,
                NroSticker = string.IsNullOrEmpty(consulta.NumeroSticker) ? noAplica : consulta.NumeroSticker,
                Localidad = string.IsNullOrEmpty(consulta.LocalidadIds) ? noAplica : contadorLocalidad + " localidades seleccionadas",
                Departamento = string.IsNullOrEmpty(consulta.DepartamentoIds) ? noAplica : contadorDepartamento + " departamentos seleccionados",
                OrigenFormulario = string.IsNullOrEmpty(datosCombo.OrigenFormulario) ? noAplica : datosCombo.OrigenFormulario,
                EstadoFormulario = string.IsNullOrEmpty(consulta.IdEstadoFormulario) ? noAplica : contadorEstado + " estados seleccionados",
                Linea = string.IsNullOrEmpty(datosCombo.Linea) ? noAplica : datosCombo.Linea,
                TipoPersonaDescripcion = string.IsNullOrEmpty(datosCombo.TipoPersonaDescripcion) ? noAplica : datosCombo.TipoPersonaDescripcion,
            };

            var reportData = new ReporteBuilder("ReporteFormulario_Bandeja.rdlc")
                .AgregarDataSource("DSBandejaFormularios", datosGrilla)
                .AgregarDataSource("DSBandejaFormulariosConsulta", datosConsulta)
                .Generar();

            return new ReporteResultado(new ArchivoBase64(Convert.ToBase64String(reportData.Content), TipoArchivo.PDF,
                $"reporte_bandeja_formularios"));
        }

        public string ObtenerNroSuac(decimal idFormulario)
        {
            return _formularioRepositorio.ObtenerNroSuac(idFormulario).NroSticker;

        }

    }
}
