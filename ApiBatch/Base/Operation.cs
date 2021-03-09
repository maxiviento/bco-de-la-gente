using ApiBatch.Infraestructure;
using ApiBatch.Utilidades;
using ICSharpCode.SharpZipLib.Zip;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiBatch.Base
{
    public abstract class Operation<TParams, TResult, TOutput> where TParams : class where TOutput : class
    {
        public Operation(IStatelessSession _statelessSession)
        {
            this.StatelessSession = _statelessSession;
        }


        protected ISessionFactory SessionFactory;
        protected IStatelessSession StatelessSession;
        public abstract TResult Execute(TParams cmd);


        public Resultado<R> ArmarResultado<C, R>(IList<R> elementos, C consulta)
            where C : Consulta
            where R : class
        {
            var resultado = new Resultado<R>()
            {
                Elementos = elementos,
                NumeroPagina = consulta.NumeroPagina,
                TamañoDePagina = consulta.TamañoPagina,
                TieneMasResultados = elementos.Count > consulta.TamañoPagina
            };

            if (resultado.TieneMasResultados)
                resultado.Elementos = resultado.Elementos.Take(resultado.TamañoDePagina).ToList();

            return resultado;
        }

        public string GuardarArchivo(Archivo archivo, string carpeta, string accion)
        {
            try
            {
                var directorio = $"Archivos\\{carpeta}\\";
                if (!string.IsNullOrEmpty(accion))
                    directorio += $"{accion}\\";

                var rutaArchivo = FileUtil.GenerarRutaArchivoEnDirectorio(archivo.FileName, directorio);

                byte[] fileBuffer;

                if (archivo.MediaType.Equals("text/plain"))
                {
                    fileBuffer = Encoding.ASCII.GetBytes(archivo.Buffer);
                    rutaArchivo += ".txt";
                }
                else
                {
                    fileBuffer = Convert.FromBase64String(archivo.Buffer);
                    rutaArchivo += ".xlsx";
                }

                var fs = new FileStream(rutaArchivo, FileMode.Create);
                fs.Write(fileBuffer, 0, fileBuffer.Length);
                fs.Close();

                return rutaArchivo;
            }
            catch (Exception e)
            {
                throw new ModeloNoValidoException($"Error al guardar el archivo: {e.Message}");
            }
        }

        public Usuario ConsultarUsuarioPorCuil(string cidiHash, string cuil)
        {
            var usuarioGuardado = StatelessSession.RunSP("PR_OBTENER_USUARIO")
                .AddParam((Id?)null)
                .AddParam(cuil)
                .ToUniqueResult<Usuario>();

            if (cuil == "11111111111")
                return new Usuario()
                {
                    Cuil = cuil,
                    Id = usuarioGuardado.Id
                };

            var usuarioCiDi = ApiCuenta.ObtenerUsuarioPorCuil(cidiHash, cuil);

            var usuario = new Usuario();

            if (usuarioCiDi != null)
            {
                if (string.IsNullOrEmpty(usuarioCiDi.CUIL)) return null;

                usuario.Apellido = usuarioCiDi.Apellido.ToUpper();
                usuario.Nombre = usuarioCiDi.Nombre.ToUpper();
                usuario.Email = usuarioCiDi.Email.ToUpper();
                usuario.Cuil = usuarioCiDi.CUIL;
            }

            if (usuarioGuardado != null)
            {
                usuario.Id = usuarioGuardado.Id;
            }

            return usuario;
        }

        /// <summary>
        /// Convierte un array de ids en array de string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">[1,2,3]</param>
        /// <returns>1,2,3</returns>
        private string ArrayComoString<T>(T[] ids)
        {
            var res = new StringBuilder();
            if (ids != null && ids.Length != 0)
            {
                foreach (var id in ids)
                {
                    res.Append(id + ",");
                }

                res.Remove(res.Length - 1, 1);
            }

            return res.ToString();
        }
    }
}