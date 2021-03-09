using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Configuracion.Aplicacion.Comandos;

namespace Datos.Repositorios.Formulario
{
    public class MotivoRechazoRepositorio : NhRepositorio<MotivoRechazo>, IMotivoRechazoRepositorio
    {
        public MotivoRechazoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<MotivoRechazo> ConsultarMotivosRechazo(Ambito ambito)
        {
            return Execute("PR_OBTENER_MOT_RECH_AMBITO")
                .AddParam(ambito.Id.Valor)
                .ToListResult<MotivoRechazo>();
        }
        public IList<MotivoRechazo> ConsultarMotivosRechazoPorAmbito(decimal idAmbito)
        {
            return Execute("PR_OBTENER_MOT_RECH_AMBITO")
                .AddParam(idAmbito)
                .ToListResult<MotivoRechazo>();
        }

        public MotivoRechazo ConsultarPorId(Id id, Ambito ambito)
        {
            var lista = Execute("PR_OBTENER_MOT_RECH_POR_ID")
                .AddParam(id)
                .ToListResult<MotivoRechazo>();

            return lista.FirstOrDefault(x => x.Id.Valor == id.Valor);
        }

        public Resultado<ConsultaMotivoRechazoResultado.Grilla> Consultar(ConsultaMotivoRechazo consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_MOTIVOS_RECHAZO")
                .AddParam(consulta.Abreviatura)
                .AddParam(consulta.AmbitoId)
                .AddParam(default(string))
                .AddParam(consulta.Automatico)
                .AddParam(consulta.IncluirDadosDeBaja)
                .AddParam(consulta.Codigo)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<ConsultaMotivoRechazoResultado.GrillaBD>();

            var consultaGrilla = new List<ConsultaMotivoRechazoResultado.Grilla>();
            foreach (var elem in elementos)
            {
                consultaGrilla.Add(new ConsultaMotivoRechazoResultado.Grilla
                {
                    Id = elem.Id,
                    Nombre = elem.Nombre,
                    Abreviatura = elem.Abreviatura,
                    Ambito = new Ambito((int)elem.IdAmbito, elem.NombreAmbito),
                    Descripcion = elem.Descripcion,
                    EsAutomatico = elem.EsAutomatico == "S",
                    FechaUltimaModificacion = elem.FechaUltimaModificacion,
                    IdMotivoBaja = elem.IdMotivoBaja,
                    Codigo = elem.Codigo
                });
            }

            var resultado = CrearResultado(consulta, consultaGrilla);
            return resultado;
        }

        public bool ExisteMotivoRechazoConMismoNombre(string nombre)
        {
            var existeArea = Execute("PR_EXISTE_MOTIVO_RECHAZO")
                .AddParam(nombre)
                .ToEscalarResult<string>();
            return existeArea == "S";
        }

        public bool ExisteMotivoRechazoConMismoNombre(int idAmbito, string nombre)
        {
            var validacion = Execute("PR_VALIDAR_RECHAZO_NOMBRE")
                .AddParam(idAmbito)
                .AddParam(nombre)
                .ToEscalarResult<string>();
            return validacion == "S";
        }

        public bool ExisteMotivoRechazoConMismaAbreviatura(int idAmbito, string abreviatura)
        {
            var validacion = Execute("PR_VALIDAR_RECHAZO_ABREV")
                .AddParam(idAmbito)
                .AddParam(abreviatura)
                .ToEscalarResult<string>();
            return validacion == "S";
        }

        public bool ExisteMotivoRechazoConMismoCodigo(int idAmbito, string codigo)
        {
            return Execute("PR_EXISTE_CODIGO_AMBITO")
                .AddParam(idAmbito)
                .AddParam(codigo)
                .ToEscalarResult<bool>();
        }

        public decimal RegistrarMotivoRechazo(MotivoRechazo motivo)
        {
            var resultadoSp = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MOTIVOS_RECHAZO")
                .AddParam(-1)
                .AddParam(motivo.Nombre)
                .AddParam(motivo.Descripcion)
                .AddParam(motivo.Abreviatura)
                .AddParam(motivo.Ambito.Id)
                .AddParam(motivo.EsAutomatico)
                .AddParam(-1)
                .AddParam(motivo.Codigo)
                .AddParam(motivo.UsuarioAlta.Id)
                .ToSpResult();
            return resultadoSp.Id.Valor;
        }

        public ConsultaMotivoRechazoResultado.Detallado ConsultarPorIdGeneral(Id idMotivo, Id idAmbito)
        {
            return Execute("PR_OBTENER_MOT_RECH_POR_ID")
                .AddParam(idMotivo)
                .AddParam(idAmbito)
                .ToUniqueResult<ConsultaMotivoRechazoResultado.Detallado>();
        }

        public bool Modificar(ModificacionMotivoRechazoComando comando, Usuario usuario)
        {
            var res = Execute("PR_ACTUALIZA_MOTIVO_RECHAZO")
                .AddParam(comando.Id)
                .AddParam(comando.NombreOriginal)
                .AddParam(comando.DescripcionOriginal)
                .AddParam(comando.AbreviaturaOriginal)
                .AddParam(comando.CodigoOriginal)
                .AddParam(comando.NombreNuevo)
                .AddParam(comando.DescripcionNueva)
                .AddParam(comando.AbreviaturaNueva)
                .AddParam(comando.CodigoNuevo)
                .AddParam(usuario.Id)
                .ToSpResult();
            return res.Mensaje == "OK";
        }

        public void DarDeBaja(MotivoRechazo motivo)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MOTIVOS_RECHAZO")
                .AddParam(motivo.Id)
                .AddParam(motivo.Nombre)
                .AddParam(motivo.Descripcion)
                .AddParam(motivo.Abreviatura)
                .AddParam(motivo.Ambito.Id)
                .AddParam(motivo.EsAutomatico)
                .AddParam(motivo.MotivoBaja.Id)
                .AddParam(motivo.Codigo)
                .AddParam(motivo.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public IList<Ambito> ConsultarAmbitosCombo()
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_AMBITOS")
                .ToListResult<Ambito>();
        }

        public List<ConsultaMotivoRechazoResultado.Grilla> ObtenerAbreviaturas()
        {
            var elementos = Execute("PR_OBTENER_MOTIVOS_RECHAZO")
                .AddParam(default(string))
                .AddParam(-1)
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam('S')
                .AddParam(default(string))
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<ConsultaMotivoRechazoResultado.GrillaBD>();

            var consultaGrilla = new List<ConsultaMotivoRechazoResultado.Grilla>();
            foreach (var elem in elementos)
            {
                consultaGrilla.Add(new ConsultaMotivoRechazoResultado.Grilla
                {
                    Id = elem.Id,
                    Nombre = elem.Nombre,
                    Abreviatura = elem.Abreviatura,
                    Ambito = new Ambito(int.Parse(elem.IdAmbito == null? 0.ToString() : elem.IdAmbito.ToString()), elem.NombreAmbito),
                    Descripcion = elem.Descripcion,
                    EsAutomatico = elem.EsAutomatico == "S",
                    FechaUltimaModificacion = elem.FechaUltimaModificacion,
                    IdMotivoBaja = elem.IdMotivoBaja,
                    Codigo = elem.Codigo
                });
            }
            
            return consultaGrilla;
        }
    }
}