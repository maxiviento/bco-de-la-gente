using System;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using NHibernate.Util;
using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Comandos;
using Infraestructura.Core.Comun.Dato;
using Pagos.Aplicacion.Consultas.Resultados;
using System.Transactions;
using Infraestructura.Core.Datos.DSL;

namespace Datos.Repositorios.Formulario
{
    public class FormularioRepositorio : NhRepositorio<global::Formulario.Dominio.Modelo.Formulario>,
        IFormularioRepositorio
    {
        public FormularioRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal Registrar(global::Formulario.Dominio.Modelo.Formulario formulario)
        {
            var resultadoFormulario = Execute("PR_ABM_FORMULARIOS_LINEA")
                .AddParam(formulario.Id)
                .AddParam(formulario.DetalleLineaPrestamo.Id)
                .AddParam(formulario.Solicitante.SexoId)
                .AddParam(formulario.Solicitante.NroDocumento)
                .AddParam(formulario.Solicitante.CodigoPais)
                .AddParam(formulario.Solicitante.IdNumero.GetValueOrDefault())
                .AddParam(default(string))
                .AddParam(formulario.OrigenFormulario.Id)
                .AddParam(false)
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(formulario.FechaForm)
                .AddParam(default(string))
                .AddParam(formulario.EsApoderado.ToString())
                .AddParam(formulario.UsuarioAlta.Id)
                .ToSpResult();
            formulario.Id = resultadoFormulario.Id;
            return formulario.Id.Valor;
        }


        public BandejaCargaNumeroControlInternoResultado ObtenerFormulariosPorPrestamo(int idFormularioLinea)
        {
            return Execute("PR_OBTENER_FORM_CONTROL_I")
                .AddParam(idFormularioLinea)
                .ToUniqueResult<BandejaCargaNumeroControlInternoResultado>();
        }

        public DatosDomicilioIntegranteResultado ConsultaDatosDomicilioIntegrante(int idFormularioLinea)
        {
            return Execute("PR_OBTENER_PERS_DOM_GU_DOM")
                .AddParam(idFormularioLinea)
                .ToUniqueResult<DatosDomicilioIntegranteResultado>();
        }

        public IList<SituacionDomicilioIntegranteResultado> ObtenerSituacionDomicilioIntegrante(int idFormularioLinea)
        {
            return Execute("PR_OBTENER_PERS_DOM_GU")
                .AddParam(idFormularioLinea)
                .ToListResult<SituacionDomicilioIntegranteResultado>();
        }


        public decimal RegistrarSeguimientoAuditoria(SeguimientoAuditoriaComando seguimiento)
        {
            var spResult = Execute("PR_REGISTRA_SEGUIMIENTO_IMPRES")
                .AddParam(seguimiento.IdFormularioLinea)
                .AddParam(seguimiento.IdSeguimientoFormulario)
                .AddParam(seguimiento.IdAccion)
                .AddParam(seguimiento.IdPrestamoItem)
                .AddParam(seguimiento.IdUsuario)
                .AddParam(seguimiento.Observaciones)
                .ToSpResult();

            seguimiento.Id = spResult.Id;
            return seguimiento.Id.Valor;
        }

        public bool RegistrarSticker(decimal idFormulario, string nroSticker, Id idUsuario)
        {
            var spResult = Execute("PR_REGISTRA_STICKER")
                .AddParam(idFormulario)
                .AddParam(nroSticker)
                .AddParam(idUsuario.Valor)
                .ToEscalarResult<bool>();

            return spResult;
        }

        public decimal Rechazar(global::Formulario.Dominio.Modelo.Formulario formulario, IList<MotivoRechazo> mr)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var session = Sesion)
                using (var transaction = session.BeginTransaction())
                {
                    var resultadoFormulario = Execute("PR_ABM_FORMULARIOS_LINEA")
                        .AddParam(formulario.Id)
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(true)
                        .AddParam(default(decimal?))
                        .AddParam(default(decimal?))
                        .AddParam(formulario.FechaForm)
                        .AddParam(formulario.NumeroCaja)
                        .AddParam(default(bool))
                        .AddParam(formulario.UsuarioAlta.Id)
                        .ToSpResult();
                    formulario.Id = resultadoFormulario.Id;

                    foreach (MotivoRechazo motivoRechazo in mr)
                    {
                        this.RegistrarMotivosRechazo(formulario.Id.Valor, motivoRechazo);
                    }

                    transaction.Commit();
                }
                scope.Complete();
            }
            return formulario.Id.Valor;
        }

        public bool RegistrarMotivosRechazo(decimal idSeguimiento, MotivoRechazo motivo)
        {
            bool valido = false;
            var spResult = Execute("PR_REGISTRA_MOT_REC_FORM")
                .AddParam(idSeguimiento)
                .AddParam(motivo.Id)
                .AddParam(motivo.Observaciones)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                valido = true;
            }

            return valido;
        }

        public decimal RechazarFormularioConPrestamo(int idFormulario, bool esAsociativa, decimal idUsuario,
            string numeroCaja, IList<MotivoRechazo> mr)
        {
            decimal idSeguimientoFormulario;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var session = Sesion)
                using (var transaction = session.BeginTransaction())
                {
                    var resultadoFormulario = Execute("PR_REG_RECHAZOS_FORM_IMPAGO")
                        .AddParam(idFormulario)
                        .AddParam(esAsociativa)
                        .AddParam(idUsuario)
                        .AddParam(default(string))
                        .AddParam(numeroCaja)
                        .ToSpResult();

                    idSeguimientoFormulario = resultadoFormulario.Id.Valor;

                    foreach (MotivoRechazo motivoRechazo in mr)
                    {
                        this.RegistrarMotivosRechazo(idSeguimientoFormulario, motivoRechazo);
                    }

                    transaction.Commit();
                }
                scope.Complete();
            }
            return idSeguimientoFormulario;
        }

        public decimal DarDeBaja(global::Formulario.Dominio.Modelo.Formulario formulario)
        {
            var resultadoFormulario = Execute("PR_ABM_FORMULARIOS_LINEA")
                .AddParam(formulario.Id)
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .AddParam(false)
                .AddParam(default(decimal?))
                .AddParam(formulario.MotivoBaja.Id)
                .AddParam(formulario.FechaForm)
                .AddParam(default(decimal?))
                .AddParam(default(bool))
                .AddParam(formulario.UsuarioAlta.Id)
                .ToSpResult();
            formulario.Id = resultadoFormulario.Id;
            return formulario.Id.Valor;
        }

        public TipoIntegranteSocio ConsultarTipoIntegrante(decimal idFormulario)
        {
            return Execute("PR_OBTENER_TIPO_INTEG_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<TipoIntegranteSocio>();
        }

        public void RegistrarCambioDeEstado(decimal idFormulario, decimal idEstadoDestino, decimal idAgrupamiento,
            decimal idUsuario)
        {
            Execute("PR_ACTUALIZA_ESTADO_FORM")
                .AddParam(idFormulario)
                .AddParam(idEstadoDestino)
                .AddParam(idAgrupamiento)
                .AddParam(idUsuario)
                .JustExecute();
        }

        public DatosBasicosFormularioResultado ObtenerSolicitante(int idFormulario)
        {
            return Execute("PR_OBTENER_SOLICITANTE_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<DatosBasicosFormularioResultado>();
        }

        public IList<DatosPersonaResultado> ObtenerGarantes(int idFormulario)
        {
            return Execute("PR_OBTENER_GARANTES_X_FORM")
                .AddParam(idFormulario)
                .ToListResult<DatosPersonaResultado>();
        }

        public decimal ExisteFormularioParaSolicitante(DatosPersonaConsulta consulta)
        {
            var existeFormulario = Execute("PR_VALIDAR_SOLICITANTE")
                .AddParam(consulta.IdSexo)
                .AddParam(consulta.NroDocumento)
                .AddParam(consulta.CodigoPais)
                .AddParam(consulta.IdNumero)
                .ToUniqueResult<ValidacionSolicitanteResultado>();
            return existeFormulario.Valor;
        }

        public decimal ExisteFormularioParaSolicitanteReactivacion(DatosPersonaConsulta consulta)
        {
            var existeFormulario = Execute("PR_VALIDAR_SOLICITANTE_REAC")
                .AddParam(consulta.IdSexo)
                .AddParam(consulta.NroDocumento)
                .AddParam(consulta.CodigoPais)
                .AddParam(consulta.IdNumero)
                .ToUniqueResult<ValidacionSolicitanteResultado>();
            return existeFormulario.Valor;
        }

        public bool ExisteDeudaHistorica(DatosPersonaConsulta consulta)
        {
            var existe = Execute("PR_VALIDAR_DEUDA_ASOCIATIVA")
                .AddParam(consulta.IdSexo)
                .AddParam(consulta.NroDocumento)
                .ToEscalarResult<string>();
            return existe == "S";
        }

        public bool ExisteGrupoParaSolicitante(int idFormulario)
        {
            var existe = Execute("PR_VALIDAR_TIENE_GU")
                .AddParam(idFormulario)
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(decimal))
                .ToEscalarResult<string>();
            return existe == "S";
        }

        public bool ExisteGrupoParaPersona(DatosPersonaConsulta persona)
        {
            var existe = Execute("PR_VALIDAR_TIENE_GU")
                .AddParam(-1)
                .AddParam(persona.IdSexo)
                .AddParam(persona.NroDocumento)
                .AddParam(persona.CodigoPais)
                .AddParam(persona.IdNumero)
                .ToEscalarResult<string>();
            return existe == "S";
        }


        public decimal ExisteFormularioParaIntegranteGrupo(string idSexo, string nroDocumento, string codigoPais,
            int idNumero)
        {
            var existeFormulario = Execute("PR_VALIDAR_SOLICITANTE")
                .AddParam(idSexo)
                .AddParam(nroDocumento)
                .AddParam(codigoPais)
                .AddParam(idNumero)
                .ToUniqueResult<ValidacionSolicitanteResultado>();
            return existeFormulario.Valor;
        }

        public void RegistrarCursos(int idFormulario, decimal usuarioId, IList<SolicitudCurso> cursosSolicitados)
        {
            if (cursosSolicitados == null || cursosSolicitados.Count == 0) return;
            string[] cursosParam = {""};
            cursosSolicitados.ForEach(solicitudCurso =>
            {
                var cursos = solicitudCurso.Cursos;
                cursos.Where(curso => !curso.Nombre.Equals("OTROS")).ForEach(curso => cursosParam[0] += curso.Id + "|");
                var otros = cursos.SingleOrDefault(curso => curso.Nombre.Equals("OTROS"));
                if (otros != null)
                {
                    cursosParam[0] += otros.Id + "|" + solicitudCurso.Descripcion + "|";
                }
            });
            Execute("PR_REGISTRA_CURSOS")
                .AddParam(cursosParam[0].TrimEnd('|'))
                .AddParam(idFormulario)
                .AddParam(usuarioId)
                .JustExecute();
        }

        public void RegistrarDestinoFondos(int idFormulario, decimal idUsuario,
            OpcionDestinoFondos opcionDestinosFondos)
        {
            if (opcionDestinosFondos == null) return;

            var destinoFondos = opcionDestinosFondos.DestinoFondos;
            var destinoFondosParam = "";
            destinoFondos.ForEach(destinoFondo =>
            {
                destinoFondosParam += destinoFondo.Id + "|";
                if (destinoFondo.Id.Valor == 99)
                {
                    destinoFondosParam += opcionDestinosFondos.Observaciones + "|";
                }
            });

            Execute("PR_REGISTRA_DESTINOS_FONDO")
                .AddParam(destinoFondosParam.TrimEnd('|'))
                .AddParam(idFormulario)
                .AddParam(idUsuario)
                .JustExecute();
        }

        public void RegistrarCondicionesSolicitadas(int idFormulario, decimal idUsuario,
            CondicionesPrestamo condicionesSolicitadas)
        {
            var condiciones = condicionesSolicitadas;
            if (condiciones == null) return;
            //formato "id,valor,id,valor,etc..." requerido por el SP
            var condicionesPrestamoParam =
                $"1;{condiciones.MontoSolicitado};2;{condiciones.CantidadCuotas};3;{condiciones.MontoEstimadoCuota}";

            Execute("PR_REGISTRA_CONDIC_PRESTAMO")
                .AddParam(condicionesPrestamoParam)
                .AddParam(idFormulario)
                .AddParam(idUsuario)
                .JustExecute();
        }

        public string ModificarCondicionesSolicitadas(int idFormulario, decimal idUsuario,
            CondicionesPrestamo condicionesSolicitadas)
        {
                var resultado = Execute("PR_REGISTRAR_COND_PREST")
                    .AddParam(idFormulario)
                    .AddParam(condicionesSolicitadas.MontoSolicitado)
                    .AddParam(condicionesSolicitadas.CantidadCuotas)
                    .AddParam(idUsuario)
                    .ToSpResult();

                if (resultado.Mensaje == "OK")
                {
                    return "La operación se realizo con éxito.";
                }
                return "Error al registrar condiciones de préstamo";
        }

        public void RegistrarGarantes(int idFormulario, decimal usuarioId, List<Persona> garantes)
        {
            if (garantes == null || garantes.Count == 0)
                return;
            var cadenaGarantes = "";
            garantes.ForEach(
                g => cadenaGarantes += $"{g.SexoId},{g.NroDocumento},{g.CodigoPais},{g.IdNumero}");
            Execute("PR_REGISTRA_GARANTES")
                .AddParam(cadenaGarantes.TrimEnd(','))
                .AddParam(idFormulario)
                .AddParam(usuarioId)
                .JustExecute();
        }

        public Resultado<FormularioGrillaResultado> ObtenerFormulariosPorFiltros(FormularioGrillaConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_FORMULARIOS")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdOrigen)
                .AddParam(consulta.IdEstadoFormulario)
                .AddParam(consulta.NumeroFormulario)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.NumeroPrestamo ?? "-1")
                .AddParam(consulta.NumeroSticker ?? "-1")
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormularioGrillaResultado>();

            elementos.ForEach(formulario =>
            {
                formulario.PuedeConformarPrestamo = formulario.PuedeConformarPrestamo &&
                                                    formulario.IdEstado.Valor == EstadoFormulario.Iniciado.Id.Valor;
            });

            return CrearResultado(consulta, elementos);
        }

        public string ObtenerTotalizadorFormularios(FormularioGrillaConsulta consulta)
        {
            var total = Execute("PR_BANDEJA_FORMULARIOS_TOT")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdOrigen)
                .AddParam(consulta.IdEstadoFormulario)
                .AddParam(consulta.NumeroFormulario)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.NumeroPrestamo ?? "-1")
                .AddParam(consulta.NumeroSticker ?? "-1")
                .ToEscalarResult<string>();

            return total;
        }

        public CondicionesSolicitadasResultado ObtenerCondicionesDePrestamoPorFormulario(int idFormulario)
        {
            return Execute("PR_OBTENER_COND_PREST_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<CondicionesSolicitadasResultado>();
        }

        public CondicionesSolicitadasResultado ObtenerCondicionesDePrestamoPorFormularioApoderado(int idFormulario)
        {
            return Execute("PR_OBT_COND_PREST_X_FORM_AP")
                .AddParam(idFormulario)
                .ToUniqueResult<CondicionesSolicitadasResultado>();
        }

        public DatosContactoResultado ObtenerDatosContacto(DatosPersonaConsulta consulta)
        {
            return Execute("PR_OBTENER_COMUNICACIONES")
                .AddParam(consulta.IdSexo)
                .AddParam(consulta.NroDocumento)
                .AddParam(consulta.CodigoPais)
                // CAMBIAR EL DEFAULT DECIMAL POR EL DATO IDNUMERO 
                .AddParam(consulta.IdNumero)
                .ToUniqueResult<DatosContactoResultado>();
        }

        public string ActualizarDatosDeContacto(Persona persona)
        {
            var spResult = Execute("PR_GESTIONA_COMUNICACIONES")
                .AddParam(persona.SexoId)
                .AddParam(persona.NroDocumento)
                .AddParam(persona.CodigoPais)
                .AddParam(persona.IdNumero)
                .AddParam(persona.CodigoArea)
                .AddParam(persona.Telefono)
                .AddParam(persona.CodigoAreaCelular)
                .AddParam(persona.Celular)
                .AddParam(persona.Email)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public string ActualizaSucursal(List<int> idsFormularios, string idBanco, string idSucursal, decimal idUsuario)
        {
            string[] cadena = {""};
            idsFormularios.ForEach(id => { cadena[0] += id + ","; });
            var spResult = Execute("PR_ACTUALIZA_SUCURSAL_FORM")
                .AddParam(cadena[0].TrimEnd(','))
                .AddParam(idBanco)
                .AddParam(idSucursal)
                .AddParam(idUsuario)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public Resultado<FormularioFiltradoResultado> ConsultaFormulariosFiltros(FiltrosFormularioConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_FORM_GENERICA")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdPrestamo)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormularioFiltradoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public IList<FormularioFiltradoResultado> ConsultaIdsFormulariosFiltros(FiltrosFormularioConsulta consulta)
        {
            return Execute("PR_BANDEJA_FORM_GENERICA")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdPrestamo)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<FormularioFiltradoResultado>();
        }

        public string ConsultaTotalizadorDocumentacion(FiltrosFormularioConsulta consulta)
        {
            if (consulta == null)
            {
                return Execute("PR_BANDEJA_FORM_GENERICA_TOT")
                    .AddParam(-1)
                    .AddParam(-1)
                    .AddParam(-1)
                    .AddParam(-1)
                    .AddParam(default(string))
                    .AddParam(default(string))
                    .AddParam(-1)
                    .AddParam(-1)
                    .AddParam(-1)
                    .ToEscalarResult<string>();
            }
            return Execute("PR_BANDEJA_FORM_GENERICA_TOT")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdPrestamo)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(consulta.IdFormularioLinea)
                .ToEscalarResult<string>();
        }

        public string ConsultaTotalizadorSucursalBancaria(FiltrosFormularioConsulta consulta)
        {
            if (consulta == null)
            {
                return Execute("PR_BANDEJA_SUC_BANCARIA_TOT")
                    .AddParam(-1)
                    .AddParam(-1)
                    .AddParam(-1)
                    .AddParam(default(string))
                    .AddParam(default(string))
                    .AddParam(-1)
                    .AddParam(-1)
                    .ToEscalarResult<string>();
            }
            return Execute("PR_BANDEJA_SUC_BANCARIA_TOT")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .ToEscalarResult<string>();
        }

        public Resultado<FormularioFiltradoResultado> ConsultaFormulariosSucursalFiltros(
            FiltrosFormularioConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_SUC_BANCARIA")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormularioFiltradoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public IList<FormularioFiltradoResultado> ConsultaIdsFormulariosSucursalFiltros(
            FiltrosFormularioConsulta consulta)
        {
            var elementos = Execute("PR_BANDEJA_SUC_BANCARIA")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<FormularioFiltradoResultado>();

            return elementos;
        }

        public string RegistrarPatrimonioSolicitante(int idFormulario, decimal idUsuario, string patrimonioSolicitante)
        {
            var spResult = Execute("PR_REGISTRA_PATRIM_SOLIC")
                .AddParam(patrimonioSolicitante)
                .AddParam(idFormulario)
                .AddParam(idUsuario)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public PatrimonioSolicitanteResultado ObtenerPatrimonioSolicitante(int idFormulario)
        {
            return Execute("PR_OBTENER_PATR_DECL_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<PatrimonioSolicitanteResultado>();
        }

        public void RegistrarOrganizacionEmprendimiento(int idFormulario, decimal idUsuario,
            Emprendimiento emprendimiento)
        {
            throw new NotImplementedException();
        }

        public IList<ConsultaDeudaFormularioResultado> ObtenerDatosDeudaFormulario(int idFormulario)
        {
            return Execute("PR_OBTENER_DATOS_DEUDA")
                .AddParam(idFormulario)
                .ToListResult<ConsultaDeudaFormularioResultado>();
        }

        public decimal ActualizarDatosEmprendimiento(int idAgrupamiento, decimal idUsuario,
            Emprendimiento emprendimiento)
        {
            decimal? idActividad = emprendimiento.Actividad?.Id.Valor;
            DateTime? fechaInicioActividad = emprendimiento.Actividad?.FechaInicio;
            string tieneExperiencia = null;
            if (emprendimiento.TieneExperiencia != null)
                tieneExperiencia = emprendimiento.TieneExperiencia.Value ? "S" : "N";
            string hizoCursos = null;
            if (emprendimiento.HizoCursos != null) hizoCursos = emprendimiento.HizoCursos.Value ? "S" : "N";
            string pidioCredito = null;
            if (emprendimiento.PidioCredito != null) pidioCredito = emprendimiento.PidioCredito.Value ? "S" : "N";
            string creditoFueOtorgado = null;
            if (emprendimiento.CreditoFueOtorgado != null)
                creditoFueOtorgado = emprendimiento.CreditoFueOtorgado.Value ? "S" : "N";
            string cuitInstitucion = emprendimiento.InstitucionSolicitante;

            Id idEmprendimiento = Execute("PR_ABM_EMPRENDIMIENTO_X_AGRUP")
                .AddParam(emprendimiento.Id)
                .AddParam(idAgrupamiento)
                .AddParam(emprendimiento.IdVinculo)
                .AddParam(emprendimiento.TipoInmueble.Id)
                .AddParam(idActividad)
                .AddParam(fechaInicioActividad)
                .AddParam(emprendimiento.TipoProyecto.Id)
                .AddParam(emprendimiento.FechaActivacion)
                .AddParam(emprendimiento.SectorDesarrollo.Id)
                .AddParam(tieneExperiencia)
                .AddParam(emprendimiento.TiempoExperiencia)
                .AddParam(hizoCursos)
                .AddParam(pidioCredito)
                .AddParam(creditoFueOtorgado)
                .AddParam(cuitInstitucion)
                .AddParam(emprendimiento.TipoOrganizacion.Id)
                .AddParam(idUsuario)
                .ToSpResult()
                .Id;
            return idEmprendimiento.Valor;
        }

        public void RegistrarDatosComunicacionEmprendimiento(Emprendimiento emprendimiento, decimal idUsuario)
        {
            Execute("PR_GESTIONA_COMUNICACIONES_EMP")
                .AddParam(emprendimiento.Id)
                .AddParam(emprendimiento.NroCodArea)
                .AddParam(emprendimiento.NroTelefono)
                .AddParam(emprendimiento.Email)
                .JustExecute();
        }

        public EmprendimientoResultado ObtenerDatosEmprendimiento(int idFormulario)
        {
            var result = Execute("PR_OBTENER_EMPRENDIMIENTO_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<EmprendimientoResultado>();
            return result;
        }

        public void RegistrarMiembroEmprendimiento(Emprendimiento emprendimiento,
            MiembroEmprendimientoFormularioResultado miembro, decimal idUsuario)
        {
            Execute("PR_REGISTRA_MIEMBRO_EMP")
                .AddParam(emprendimiento.Id)
                .AddParam(miembro.Persona.SexoId)
                .AddParam(miembro.Persona.NroDocumento)
                .AddParam(miembro.Persona.CodigoPais)
                .AddParam(miembro.Persona.IdNumero)
                .AddParam(miembro.IdVinculo)
                .AddParam(miembro.Tarea)
                .AddParam(miembro.HorarioTrabajo)
                .AddParam(miembro.Remuneracion ?? 0)
                //.AddParam(idUsuario)
                .JustExecute();
        }

        public void ActualizarMiembroEmprendimiento(Emprendimiento emprendimiento,
            MiembroEmprendimientoFormularioResultado miembro, decimal idUsuario)
        {
            Execute("PR_ACTUALIZA_MIEMBRO_EMP")
                .AddParam(emprendimiento.Id)
                .AddParam(miembro.Persona.SexoId)
                .AddParam(miembro.Persona.NroDocumento)
                .AddParam(miembro.Persona.CodigoPais)
                .AddParam(miembro.Persona.IdNumero)
                .AddParam(miembro.IdVinculo)
                .AddParam(miembro.Tarea)
                .AddParam(miembro.HorarioTrabajo)
                .AddParam(miembro.Remuneracion ?? 0)
                //.AddParam(idUsuario)
                .JustExecute();
        }

        public void EliminarMiembroEmprendimiento(Emprendimiento emprendimiento,
            MiembroEmprendimientoFormularioResultado miembro, decimal idUsuario)
        {
            Execute("PR_ELIMINA_MIEMBRO_EMP")
                .AddParam(emprendimiento.Id)
                .AddParam(miembro.Persona.SexoId)
                .AddParam(miembro.Persona.NroDocumento)
                .AddParam(miembro.Persona.CodigoPais)
                .AddParam(miembro.Persona.IdNumero)
                .AddParam(miembro.IdVinculo)
                .AddParam(miembro.Tarea)
                .AddParam(miembro.HorarioTrabajo)
                .AddParam(miembro.Remuneracion)
                //.AddParam(idUsuario)
                .JustExecute();
        }

        public IList<MiembroEmprendimientoResultado> ObtenerMiembrosEmprendimiento(Emprendimiento emp)
        {
            return Execute("PR_OBTENER_MIEMBROS_EMP")
                .AddParam(emp.Id)
                .ToListResult<MiembroEmprendimientoResultado>();
        }

        public IList<FormularioFiltradoResultado> ConsultaFormulariosParaDeuda(FiltrosFormularioConsulta consulta)
        {
            var elementos = Execute("PR_BANDEJA_FORM_DEUDA")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdPrestamo)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<FormularioFiltradoResultado>();

            return elementos;
        }

        public IList<FormaPagoResultado> ObtenerFormasPagoPorFormulario(decimal idFormulario)
        {
            return Execute("PR_OBTENER_MERC_COM_FORM_PAGO")
                .AddParam(idFormulario)
                .ToListResult<FormaPagoResultado>();
        }

        public EstimaClientesResultado ObtenerEstimaClientesPorFormulario(decimal idFormulario)
        {
            return Execute("PR_OBTENER_MERC_COM_CLIENTES")
                .AddParam(idFormulario)
                .ToUniqueResult<EstimaClientesResultado>();
        }

        public IList<ItemsMercadoComerString> ObtenerItemsMercadoComercializacionPorFormulario(decimal idFormulario)
        {
            return Execute("PR_OBTENER_MERC_COM_ITEMS")
                .AddParam(idFormulario)
                .ToListResult<ItemsMercadoComerString>();
        }

        public IList<IngresoGrupoResultado> ObtenerIngresosGrupoFamiliar()
        {
            return Execute("PR_OBTENER_ING_GRUPO")
                .ToListResult<IngresoGrupoResultado>();
        }

        public IList<IngresoGrupoResultado> ObtenerIngresosGrupoFamiliarFormulario(int idFormulario)
        {
            return Execute("PR_OBTENER_ING_GRUPO_FORM")
                .AddParam(idFormulario)
                .ToListResult<IngresoGrupoResultado>();
        }

        public decimal ObtenerIngresoTotalGrupoFamiliar(int idGrupo)
        {
            var e = Execute("PR_CALCULA_INGRESOS_GRUPO")
                .AddParam(idGrupo)
                .ToEscalarResult<decimal?>();
            return e ?? 0;
        }

        public IList<EgresoGrupoResultado> ObtenerGastosGrupoFamiliar(int idGrupo)
        {
            return Execute("PR_OBTENER_GASTOS_GU")
                .AddParam(idGrupo)
                .ToListResult<EgresoGrupoResultado>();
        }

        public decimal RegistrarIngresosGrupoFamiliar(int idFormulario, string ingresos, string gastos,
            decimal idUsuario)
        {
            return Execute("PR_REGISTRA_INGRESOS_GRUPO")
                .AddParam(idFormulario)
                .AddParam(ingresos)
                .AddParam(gastos)
                .AddParam(idUsuario)
                .ToSpResult()
                .Id.Valor;
        }

        public Resultado<SituacionPersonasResultado> ObtenerSituacionPersonas(SituacionPersonasConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            if (consulta.TipoPersona == 0) consulta.TipoPersona = 4;

            var elementos = Execute("PR_OBTENER_PERS_BANCO")
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NumeroSticker)
                .AddParam(consulta.NumeroFormulario)
                .AddParam(consulta.NumeroPrestamo)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<SituacionPersonasResultado>();
            return CrearResultado(consulta, elementos);
        }

        public IList<SituacionPersonasResultadoVista> ObtenerVistaSituacionPersonas(SituacionPersonasConsulta consulta)
        {
            return Execute("PR_OBTENER_DEUDA_ASOCIATIVA")
                .AddParam(consulta.Dni)
                .ToListResult<SituacionPersonasResultadoVista>();
        }

        public Resultado<FormulariosSituacionResultado> ObtenerFormulariosSituacionPersonas(
            SituacionPersonasConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_PERS_FORM_BANCO")
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormulariosSituacionResultado>();
            return CrearResultado(consulta, elementos);
        }

        public decimal ActualizarDatosGeneralesCuadrantePrecioVenta(decimal? idProducto, decimal idEmprendimiento,
            decimal unidadesEstimadas, decimal gananciaEstimada, string producto)
        {
            return Execute("PR_REGISTRA_PRECIO_VENTA_PROD")
                .AddParam(idProducto ?? -1)
                .AddParam(idEmprendimiento)
                .AddParam(unidadesEstimadas)
                .AddParam(producto)
                .AddParam(gananciaEstimada)
                .ToSpResult()
                .Id.Valor;
        }

        public decimal RegistrarCostos(decimal? id, decimal idProducto, decimal? idTipoItem, decimal precioUnitario,
            decimal? valorMensual, string observacion)
        {
            return Execute("PR_REGISTRA_DET_PRECIO_VENTA")
                .AddParam(id)
                .AddParam(idProducto)
                .AddParam(idTipoItem)
                .AddParam(precioUnitario)
                .AddParam(valorMensual)
                .AddParam(observacion)
                .ToSpResult()
                .Id.Valor;
        }

        public decimal EliminarCostos(decimal id)
        {
            return Execute("PR_ELIMINA_DET_PRECIO_VENTA")
                .AddParam(id)
                .ToSpResult()
                .Id.Valor;
        }

        public PrecioVentaResultado VerCuadrantePrecioVenta(decimal idEmprendimiento)
        {
            return Execute("PR_OBTENER_PRE_VTA_GENERAL")
                .AddParam(idEmprendimiento)
                .ToUniqueResult<PrecioVentaResultado>();
        }

        public IList<ItemsPrecioVentaResultado> ObtenerCostoPrecioVentaPorFormulario(decimal idEmprendimiento)
        {
            return Execute("PR_OBTENER_PRE_VTA_DETALLE")
                .AddParam(idEmprendimiento)
                .ToListResult<ItemsPrecioVentaResultado>();
        }

        public IList<MotivosRechazoReferenciaResultado> ObtenerMotivosRechazoReferencia(string[] consulta)
        {
            IList<MotivosRechazoReferenciaResultado> elementos = new List<MotivosRechazoReferenciaResultado>();

            foreach (var id in consulta)
                if (!string.IsNullOrEmpty(id))
                    elementos.Add(
                        Execute("PR_OBTENER_MOT_RECH_REFER")
                            .AddParam(id)
                            .ToUniqueResult<MotivosRechazoReferenciaResultado>()
                    );

            return elementos;
        }

        public IList<MotivosRechazoFormularioResultado> ObtenerRechazosFormulario(decimal idFormulario)
        {
            return Execute("PR_OBTENER_MOT_RECH_X_FORM")
                .AddParam(idFormulario)
                .ToListResult<MotivosRechazoFormularioResultado>();
        }

        public IList<AgruparFormulario> ObtenerFormulariosConAgrupamiento(int idAgrupamiento)
        {
            var res = Execute("PR_OBTENER_FORM_AGRUPADOS")
                .AddParam(idAgrupamiento)
                .ToListResult<AgruparFormulario>();
            return res;
        }

        public IList<FormulariosSituacionResultado> ObtenerFormulariosCompletosSituacionPersonas(
            SituacionPersonasConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_PERS_FORM_BANCO")
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormulariosSituacionResultado>();

            foreach (var resultado in elementos)
            {
                if (resultado.IdRechFrom.Length > 0)
                    resultado.MotRechazoForm = ConcatMotivosRechazo(resultado.IdRechFrom.Split(','));

                if (resultado.IdRechPrest.Length > 0)
                    resultado.MotRechazoPrest = ConcatMotivosRechazo(resultado.IdRechPrest.Split(','));
            }

            return elementos;
        }

        public Id AgruparFormularios(string idsFormularios, Id idUsuario)
        {
            return Execute("PR_REGISTRA_AGRUPAMIENTO")
                .AddParam(idsFormularios)
                .AddParam(-1) // -1 significa que hara el agrupamiento
                .AddParam(idUsuario.Valor)
                .ToSpResult()
                .Id;
        }

        public ActualizarAgrupamientoResultado ActualizarAgrupamientoFormularios(Id idAgrupamiento,
            string idsFormularios, Id idUsuario)
        {
            return Execute("PR_ACTUALIZA_AGRUPAMIENTO")
                .AddParam(idsFormularios)
                .AddParam(idAgrupamiento)
                .AddParam(idUsuario.Valor)
                .ToUniqueResult<ActualizarAgrupamientoResultado>();
        }

        public decimal ObtenerAgrupamientoPorFormulario(decimal idFormulario)
        {
            return Execute("PR_OBTENER_AGRUP_DEL_FORM")
                       .AddParam(idFormulario)
                       .ToEscalarResult<decimal?>() ?? 0;
        }

        private string ConcatMotivosRechazo(string[] ids)
        {
            return string.Join(", ", ObtenerMotivosRechazoReferencia(ids).Select(x => x.Descripcion));
        }

        public decimal ObtenerIdAgrupamiento(int idFormulario)
        {
            var agrupamiento = Execute("PR_OBTENER_AGRUP_DEL_FORM")
                .AddParam(idFormulario)
                .ToEscalarResult<decimal?>();

            return agrupamiento ?? 0;
        }

        public bool ValidarEstadosFormulariosAgrupados(int idAgrupamiento, int idEstado)
        {
            var existe = Execute("PR_VALIDAR_ESTADO_AGRUPAM")
                .AddParam(idAgrupamiento)
                .AddParam(idEstado)
                .ToEscalarResult<string>();
            return existe == "S";
        }


        public decimal ObtenerAgrupamiento(Id idFormulario)
        {
            var res = Execute("PR_OBTENER_AGRUP_DEL_FORM")
                .AddParam(idFormulario.Valor)
                .ToEscalarResult<decimal?>();
            return res ?? 0;
        }

        public IList<IdsResult> ObtenerIdsFormulariosAgrupamiento(int idAgrupamiento)
        {
            return Execute("PR_OBTENER_FORM_AGRUPADOS_SD")
                .AddParam(idAgrupamiento)
                .ToListResult<IdsResult>();
        }

        public ImpresionConsultaFormularioResultado ObtenerImpresionConsulta(FormularioGrillaConsulta consulta)
        {
            var elementos = Execute("PR_BANDEJA_FORMULARIOS_IMP")
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(consulta.IdOrigen)
                .AddParam(default(string))
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.TipoPersona)
                .ToUniqueResult<ImpresionConsultaFormularioResultado>();
            return elementos;
        }

        public IList<FormularioGrillaResultado> ObtenerFormulariosReporte(FormularioGrillaConsulta consulta)
        {
            var elementos = Execute("PR_BANDEJA_FORMULARIOS")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdOrigen)
                .AddParam(consulta.IdEstadoFormulario)
                .AddParam(consulta.NumeroFormulario)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.NumeroPrestamo ?? "-1")
                .AddParam(consulta.NumeroSticker ?? "-1")
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<FormularioGrillaResultado>();

            return elementos;
        }

        public NroStickerResultado ObtenerNroSuac(decimal idFormulario)
        {
            return Execute("PR_OBTENER_NRO_SUAC")
                .AddParam(idFormulario)
                .ToUniqueResult<NroStickerResultado>();
        }

        #region ONG

        public IList<OngLineaResultado> ObtenerOngs(int idLinea)
        {
            return Execute("PR_OBTENER_ONGS_LINEA")
                .AddParam(idLinea)
                .ToListResult<OngLineaResultado>();
        }

        public decimal ObtenerNumeroGrupo(decimal? idOng, string nombreGrupo, decimal usuario)
        {
            return Execute("PR_ABM_GRUPOS_X_ONG")
                .AddParam(default(decimal?))
                .AddParam(idOng)
                .AddParam(nombreGrupo)
                .AddParam(default(decimal?))
                .AddParam(usuario)
                .ToSpResult().Id.Valor;
        }

        public void RegistrarOngParaFormulario(OngFormulario comando, decimal? idFormulario, decimal usuario)
        {
            Execute("PR_ABM_GRUPOS_X_ONG")
                .AddParam(comando.NumeroGrupo)
                .AddParam(comando.IdOng)
                .AddParam(comando.NombreGrupo)
                .AddParam(idFormulario)
                .AddParam(usuario)
                .ToEscalarResult<int>();
        }

        public FormularioFechaPagoResultado ObtenerFormularioFechaPago(decimal idFormulario)
        {
            return Execute("PR_OBTENER_FEC_INICIO_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<FormularioFechaPagoResultado>();
        }

        public OngFormulario ObtenerOngDeFormulario(int idFormulario)
        {
            return Execute("PR_OBT_GRUPO_ONG_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<OngFormulario>();
        }

        public IList<Reprogramacion> ObtenerHistorialReprogramacion(int idFormulario)
        {
            return Execute("PR_OBTENER_HIST_REPROGRAMA")
                .AddParam(idFormulario)
                //Tipo de proceso varios (2): reprogramación
                .AddParam(2)
                .ToListResult<Reprogramacion>();
        }

        public bool CambiarGaranteFormulario(CambiarGaranteComando comando, Id idUsuario)
        {
            var res = Execute("PR_ACTUALIZAR_GARANTE")
                .AddParam(comando.IdGaranteFormulario)
                .AddParam(comando.NroDocumento)
                .AddParam(comando.SexoId)
                .AddParam(comando.CodigoPais)
                .AddParam(comando.IdNumero)
                .AddParam(idUsuario.Valor)
                .ToSpResult();
            return res.Mensaje == "OK";
        }

        #endregion
    }
}