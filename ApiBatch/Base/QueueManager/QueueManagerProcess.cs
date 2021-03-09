using ApiBatch.GeneradoresArchivos;
using ApiBatch.Infraestructure;
using ApiBatch.Models;
using ApiBatch.Operations.QueueManager;
using ApiBatch.Utilidades;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using NHibernate;
using System;
using System.Collections.Generic;
using HttpFile = ApiBatch.Utilidades.HttpFile;

namespace ApiBatch.Base.QueueManager
{

    public class QueueManagerProcess<T> : Operation<OperacionEntrada, CreateInformeSalida, Resultado>, IQueueManagerProcess where T : class
    {
        public readonly string DIR_PROCESO;
        public bool ARCHIVO_UNICO { get; }
        public ProcesoResultado Process { get; set; }

        protected IGeneradorArchivos<T> GeneradorDeArchivos { get; set; }

        public QueueManagerProcess(IStatelessSession _statelessSession) : base(_statelessSession)
        {
        }
        public QueueManagerProcess(IStatelessSession _statelessSession, ProcesoResultado process,
            GeneradorArchivos<T> generador, string dir, bool archivoUnico) : base(_statelessSession)
        {
            Process = process;
            DIR_PROCESO = dir;
            ARCHIVO_UNICO = archivoUnico;
            GeneradorDeArchivos = generador;
        }

        public QueueManagerProcess(IStatelessSession _statelessSession, ProcesoResultado process,
            string dir, bool archivoUnico) : base(_statelessSession)
        {
            Process = process;
            DIR_PROCESO = dir;
            ARCHIVO_UNICO = archivoUnico;
        }

        //public override CreateInformeSalida Execute(OperacionEntrada cmd)
        //{
        //    var response = QueueProcess(cmd);
        //    if (!string.IsNullOrEmpty(response.Error))
        //    {
        //        throw new Exception(response.Error);
        //    }
        //    return new CreateInformeSalida() { };
        //}

        public string GenerateFile(string nombreArchivo)
        {
            try
            {
                string path = "";
                if (Process.AcumuloResultados)
                {
                    GeneradorDeArchivos.AgregarDatos(GetReportData());
                }
                else
                {
                    GeneradorDeArchivos.Datos = GetReportData();
                }

                if (Process.EsUltimo) {
                    var archivos = GeneradorDeArchivos.DefinirArchivo(nombreArchivo);
                    path = GuardarArchivo(archivos);
                }
                return path;
            }
            catch (Exception e)
            {
                throw new ModeloNoValidoException(
                    $"Error al generar el archivo: {e.InnerException?.Message ?? e.Message}");
            }
        }

        public virtual IList<T> GetReportData()
        {
            var datos = StatelessSession.RunSP(Process.TxSp)
                .AddParam(Process.IdProceso)
                .ToListResult<T>();
            return datos;
        }

        public void EndProcess(string filePath)
        {
            Process.IdEstado = new Id(5);
            StatelessSession.RunSP("PR_ACTUALIZA_PROCESO_BATCH")
                .AddParam(Process.IdProceso)
                .AddParam(Process.IdEstado)
                .AddParam(filePath)
                .ToSpResult();
        }


        public static ProcesoResultado FindNext(IStatelessSession session)
        {
            var response = session.RunSP("PR_OBTENER_PROC_A_EJEC_BATCH")
                    .ToUniqueResult<ProcesoResultado>();
            return response;
        }

        public static void ProcessSp(IStatelessSession session, ProcesoResultado process)
        {
            session.RunSP("PR_EJECUTAR_PROCESO_BATCH")
                .AddParam(process.IdProceso)
                .JustExecute();
        }


        public static ProcesoResultado VerifyGroupProcessToGenerate(IStatelessSession session)
        {
            var response = session.RunSP("PR_OBT_GRUPO_PROC_A_GENE_BATCH")
                    .ToUniqueResult<ProcesoResultado>();
            return response;
        }

        public static IList<ProcesoResultado> VerifyProcessToGenerate(IStatelessSession session, ProcesoResultado process)
        {
            var response = session.RunSP("PR_OBTENER_PROC_A_GENE_BATCH")
                   .AddParam(process.NroGrupoProceso)
                   .ToListResult<ProcesoResultado>();
            return response;
        }

        public static ProcesoResultado UpdateProcess(IStatelessSession session, ProcesoResultado process)
        {
            var response = session.RunSP("PR_ACTUALIZA_PROCESO_BATCH")
                            .AddParam(process.IdProceso)
                            .AddParam(process.IdEstado)
                            .AddParam(process.TxPath)
                            .AddParam(process.IdUsuario)
                    .ToUniqueResult<ProcesoResultado>();
            return response;
        }

        public static void Error(IStatelessSession session, string message, Id processId)
        {
            session.RunSP("PR_REGISTRA_ERROR_PROC_BATCH ")
                            .AddParam(processId.Valor)
                            .AddParam(message)
                    .JustExecute();
        }

        public virtual string GetStoreProcedureCall(OperacionEntrada cmd) { throw new NotImplementedException(); }

        private string GuardarArchivo(IList<HttpFile> archivos)
        {
            string path = HttpFileUtils.GuardarArchivo(archivos[0], Variables.GetDirectoryPath(DIR_PROCESO));
            return path;
        }

        public string GuardarZip(IList<Infraestructura.Core.Formateadores.MultipartData.Infrastructure.HttpFile> archivos, string nombre, string directorio)
        {
            throw new NotImplementedException();
        }

        public override CreateInformeSalida Execute(OperacionEntrada cmd)
        {
            throw new NotImplementedException();
        }

        public bool archivoUnico()
        {
            return ARCHIVO_UNICO;
        }
    }
}