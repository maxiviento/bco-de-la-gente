using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Formulario.Aplicacion.Consultas.Consultas;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Pagos.Aplicacion.Comandos;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using Soporte.Aplicacion.Servicios;
using Soporte.Dominio.Modelo;

namespace Pagos.Aplicacion.Servicios
{
    public class RecuperoServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IRecuperoRepositorio _recuperoRepositorio;
        private readonly PagoFacilRecuperoServicio _pagoFacilRecuperoServicio;
        private readonly PuntoBancorRecuperoServicio _puntoBancorRecuperoServicio;
        private readonly ProvincanjeRecuperoServicio _provincanjeRecuperoServicio;

        public RecuperoServicio(
            ISesionUsuario sesionUsuario,
            IRecuperoRepositorio recuperoRepositorio,
            PagoFacilRecuperoServicio pagoFacilRecuperoServicio,
            PuntoBancorRecuperoServicio puntoBancorRecuperoServicio,
            ProvincanjeRecuperoServicio provincanjeRecuperoServicio)
        {
            _sesionUsuario = sesionUsuario;
            _recuperoRepositorio = recuperoRepositorio;
            _pagoFacilRecuperoServicio = pagoFacilRecuperoServicio;
            _puntoBancorRecuperoServicio = puntoBancorRecuperoServicio;
            _provincanjeRecuperoServicio = provincanjeRecuperoServicio;
        }

        #region Recupero
        public Resultado<BandejaRecuperoResultado> ConsultarBandeja(BandejaRecuperoConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaRecuperoConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            var res = _recuperoRepositorio.ObtenerBandejaArchivoRecupero(consulta);

            return res;
        }

        public ImportarArchivoRecuperoResultado ImportarArchivoRecupero(ImportarArchivoRecuperoComando comando)
        {
            try
            {
                if (comando == null)
                {
                    throw new ModeloNoValidoException("El archivo es requerido");
                }

                if (ExisteNombreArchivoRecupero(comando.Archivo.FileName))
                {
                    throw new ModeloNoValidoException("Un archivo con el mismo nombre ya fue importado.");
                }

                if (!ValidarTipoArchivoConNombre(comando))
                {
                    throw new ModeloNoValidoException("El tipo de archivo no coincide con el seleccionado.");
                }

                var idCabeceraArchivo = _recuperoRepositorio.RegistrarCabeceraRecupero(comando.Archivo.FileName, _sesionUsuario.Usuario.Id.Valor, comando.IdTipoEntidad, comando.Convenio, comando.FechaRecupero);

                var data = Encoding.Default.GetString(comando.Archivo.Buffer);
                var filas = data.Split('\n');

                var resultado = new ImportarArchivoRecuperoResultado();

                switch (comando.IdTipoEntidad)
                {
                    case 1:
                        resultado = _pagoFacilRecuperoServicio.PagoFacilArchivoRecupero(filas, idCabeceraArchivo);
                        break;
                    case 2:
                        resultado = _pagoFacilRecuperoServicio.PagoFacilArchivoRecupero(filas, idCabeceraArchivo);
                        break;
                    case 3:
                        resultado = _puntoBancorRecuperoServicio.PuntoBancorArchivoRecupero(filas, idCabeceraArchivo, comando.Archivo.FileName);
                        break;
                    case 4:
                        resultado = _provincanjeRecuperoServicio.ProvincanjeArchivoRecupero(filas, idCabeceraArchivo, comando.Archivo.FileName);
                        break;
                }

                _recuperoRepositorio.ActualizarCabeceraRecupero(idCabeceraArchivo, resultado.CantTotal, resultado.CantProc, resultado.CantEspec, resultado.CantIncons, resultado.MontoRecuperado, resultado.MontoRechazado);
                _recuperoRepositorio.FinalizadoProcesoArchivoRecupero(idCabeceraArchivo, _sesionUsuario.Usuario.Id.Valor);
                return resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e.GetType() != typeof(ModeloNoValidoException))
                {
                    throw new ModeloNoValidoException("Hubo un problema al importar el archivo.");
                }

                throw;
            }
        }

        private static bool ValidarTipoArchivoConNombre(ImportarArchivoRecuperoComando comando)
        {
            return true;
            switch (comando.IdTipoEntidad)
            {
                case 1:
                    return comando.Archivo.FileName.Contains("ss");
                default:
                    //deberia sacar este? para los que no tienen validacion.
                    throw new ModeloNoValidoException("Tipo de archivo no definido.");
            }
        }

        private static ImportarArchivoRecuperoResultado ControlarRespuestaDetalleRecupero(ImportarArchivoRecuperoResultado resultado, string respuesta)
        {
            switch (respuesta.ToUpper())
            {
                case "OK":
                    resultado.CantProc++;
                    break;
                case "ESPECIAL":
                    resultado.CantEspec++;
                    break;
                case "AMBAS":
                    resultado.CantEspec++;
                    resultado.CantProc++;
                    break;
                case "INCONSISTENCIA":
                    resultado.CantIncons++;
                    break;
                default:
                    throw new Exception("Respuesta de la base desconocida.");
            }
            return resultado;
        }

        private bool ExisteNombreArchivoRecupero(string archivoFileName)
        {
            return _recuperoRepositorio.ValidarNombreArchivoRecupero(archivoFileName);
        }

        public Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoRecupero(VerArchivoInconsistenciaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new VerArchivoInconsistenciaConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _recuperoRepositorio.ConsultarInconsistenciaArchivoRecupero(consulta);
        }
        #endregion

        #region Resultado Banco

        public Resultado<BandejaResultadoBancoResultado> ConsultarBandejaResultadoBanco(BandejaRecuperoConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaRecuperoConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            var res = _recuperoRepositorio.ObtenerBandejaResultadoBanco(consulta);

            return res;
        }

        public ImportarArchivoResultadoBancoResultado ImportarArchivoResultadoBanco(ImportarArchivoResultadoBancoComando archivo)
        {
            try
            {
                if (archivo == null)
                {
                    throw new ModeloNoValidoException("El archivo es requerido");
                }

                var data = Encoding.Default.GetString(archivo.Archivo.Buffer);
                var filas = data.Split('\n');

                var filaCabecera = LimpiarFila(filas[0]);
                if (filaCabecera.Length != 70)
                {
                    throw new ModeloNoValidoException("El formato de la cabecera no es válido.");
                }

                var cabecera = new CabeceraResultadoBanco();
                try
                {
                    cabecera.ImporteTotal = decimal.Parse(filaCabecera.Substring(10, 10));
                    cabecera.IdBanco = filaCabecera.Substring(37, 3);
                    cabecera.Periodo = filaCabecera.Substring(40, 6);
                    cabecera.FormaPago = filaCabecera.Substring(57, 1);
                    cabecera.TipoPago = filaCabecera.Substring(58, 1);
                }
                catch (Exception ex)
                {
                    throw new ModeloNoValidoException("El formato de la cabecera no es válido.");
                }

                var idCabeceraArchivo = _recuperoRepositorio.RegistrarCabeceraResultadoBanco(cabecera.ImporteTotal, cabecera.IdBanco, cabecera.Periodo, cabecera.FormaPago, cabecera.TipoPago);
                var resultado = new ImportarArchivoResultadoBancoResultado();

                if (!CoincidenMontosCabecera(cabecera.ImporteTotal, filas))
                {
                    resultado.CoincidenMontos = false;
                    _recuperoRepositorio.RegistraRechazoCabeceraResultado(idCabeceraArchivo, (decimal)MotivoRechazoEnum.NoCoincidenImporteCabeceraConDetalle);
                    _recuperoRepositorio.FinalizadoProcesoArchivoResultadoBanco(idCabeceraArchivo, _sesionUsuario.Usuario.Id.Valor);
                    return resultado;
                }

                for (var i = 1; i < filas.Length; i++)
                {
                    var filaLimpia = LimpiarFila(filas[i]);
                    if (filaLimpia.Length == 0)  // no le doy bola a las filas en blanco
                    {
                        continue;
                    }
                    if (filaLimpia.Length != 70)
                    {
                        _recuperoRepositorio.RegistrarDetalleResultadoBanco(idCabeceraArchivo, "", "", 0, "", default(DateTime), "", i, _sesionUsuario.Usuario.Id.Valor, (decimal)MotivoRechazoEnum.LongitudFila70Banco);
                        resultado.HayError = true;
                        continue;
                    }

                    var nuevaFila = new FilaArchivoResultadoBanco();
                    try
                    {
                        nuevaFila.FormaPago = filaLimpia.Substring(24, 1);

                        nuevaFila.Importe = decimal.Parse(filaLimpia.Substring(10, 10));
                        nuevaFila.NroDocumento = (decimal.Parse(filaLimpia.Substring(36, 8))).ToString();
                        nuevaFila.Periodo = filaLimpia.Substring(44, 4);
                        nuevaFila.IdBanco = filaLimpia.Substring(49, 3);
                        nuevaFila.Agencia = filaLimpia.Substring(52, 3);
                        nuevaFila.FechaPago = "P".Equals(nuevaFila.FormaPago) ? DateTime.ParseExact(filaLimpia.Substring(25, 8), "ddMMyyyy", null) : DateTime.MinValue;

                        if (!TieneFormularioDni(nuevaFila.NroDocumento))
                        {
                            resultado.HayError = true;
                            _recuperoRepositorio.RegistrarDetalleResultadoBanco(idCabeceraArchivo, "", "", 0, "", default(DateTime), "", i, _sesionUsuario.Usuario.Id.Valor, (decimal)MotivoRechazoEnum.NoSeEncuentraFormularioConDni);
                            continue;
                        }

                        var estadoFormulario = EstadoCorrectoFormulario(nuevaFila.NroDocumento, nuevaFila.FormaPago == "P");

                        if (!estadoFormulario.EstadoCorrecto)
                        {
                            resultado.HayError = true;
                            _recuperoRepositorio.RegistrarDetalleResultadoBanco(idCabeceraArchivo, "", "", 0, "", default(DateTime), "", i, _sesionUsuario.Usuario.Id.Valor, estadoFormulario.MotivoRechazo);
                            continue;
                        }

                        if (nuevaFila.FormaPago != cabecera.FormaPago)
                        {
                            resultado.HayError = true;
                            _recuperoRepositorio.RegistrarDetalleResultadoBanco(idCabeceraArchivo, "", "", 0, "", default(DateTime), "", i, _sesionUsuario.Usuario.Id.Valor, (decimal)MotivoRechazoEnum.ErrorDatoResultadoBanco);
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        resultado.HayError = true;
                        _recuperoRepositorio.RegistrarDetalleResultadoBanco(idCabeceraArchivo, nuevaFila.IdBanco, nuevaFila.Agencia, nuevaFila.Importe, nuevaFila.NroDocumento, nuevaFila.FechaPago, nuevaFila.Periodo, i, _sesionUsuario.Usuario.Id.Valor, (decimal) MotivoRechazoEnum.ErrorDatoResultadoBanco);
                        continue;
                    }
                    // controlo la respuesta? solo hay inconsistencia por ahora, cuando no encuentra formulario para la fila 
                    var mensaje = _recuperoRepositorio.RegistrarDetalleResultadoBanco(idCabeceraArchivo, nuevaFila.IdBanco, nuevaFila.Agencia, nuevaFila.Importe, nuevaFila.NroDocumento, nuevaFila.FechaPago, nuevaFila.Periodo, i, _sesionUsuario.Usuario.Id.Valor);
                    if (mensaje.Equals("INCONSISTENCIA"))
                    {
                        resultado.HayError = true;
                    }
                }
                _recuperoRepositorio.FinalizadoProcesoArchivoResultadoBanco(idCabeceraArchivo, _sesionUsuario.Usuario.Id.Valor);
                return resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e.GetType() != typeof(ModeloNoValidoException))
                {
                    throw new ModeloNoValidoException("Hubo un problema al importar el archivo.");
                }

                throw;
            }
        }

        private bool CoincidenMontosCabecera(decimal montoTotal, string[] filas)
        {
            decimal montoDetalles = 0;

            for (var i = 1; i < filas.Length; i++)
            {
                var filaLimpia = LimpiarFila(filas[i]);

                if (filaLimpia.Length == 0)  // no le doy bola a las filas en blanco
                {
                    continue;
                }
                montoDetalles += decimal.Parse(filaLimpia.Substring(10, 10));
            }

            return montoTotal == montoDetalles;
        }

        private EstadoCorrectoFormularioResultado EstadoCorrectoFormulario(string dni, bool registraPagado)
        {
            var formulario = _recuperoRepositorio.ExisteFormularioParaSolicitante(dni);
            return ControladorEstado(formulario.EstadoFormulario, registraPagado);
        }

        private EstadoCorrectoFormularioResultado ControladorEstado(int estado, bool registraPagado)
        {
            switch (estado)
            {
                case 5: //estado formulario en prestamo
                    return new EstadoCorrectoFormularioResultado
                    {
                        EstadoCorrecto = true
                    };

                case 10: //estado formulario pagado
                    return new EstadoCorrectoFormularioResultado
                    {
                        EstadoCorrecto = false,
                        MotivoRechazo = (decimal)MotivoRechazoEnum.FormularioYaFuePagado
                    };
                case 11: //estado formulario impago
                    if (registraPagado)
                    {
                        return new EstadoCorrectoFormularioResultado
                        {
                            EstadoCorrecto = true
                        };
                    }
                    return new EstadoCorrectoFormularioResultado
                    {
                        EstadoCorrecto = false,
                        MotivoRechazo = (decimal)MotivoRechazoEnum.FormularioYaImpago
                    };
                default: //cualquier otro estado
                    return new EstadoCorrectoFormularioResultado
                    {
                        EstadoCorrecto = false,
                        MotivoRechazo = (decimal)MotivoRechazoEnum.EstadoFormularioIncorrecto
                    };
            }
        }

        private bool TieneFormularioDni(string dni)
        {
            return _recuperoRepositorio.ExisteFormularioParaSolicitante(dni).Valor >= 1;
        }

        public IList<Convenio> ObtenerConvenios(int idTipoConvenio)
        {
            var listaConvenios = _recuperoRepositorio.ObtenerConveniosPago();

            return listaConvenios.Where(x =>
            {
                x.Nombre = x.Nombre.PadLeft(4, '0');
                return x.IdTipoConvenio == idTipoConvenio;
            }).ToList();
        }

        public Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaResultadoBanco(VerArchivoInconsistenciaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new VerArchivoInconsistenciaConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _recuperoRepositorio.ConsultarInconsistenciaArchivoResultadoBanco(consulta);
        }
        #endregion

        private string LimpiarFila(string fila)
        {
            fila = fila.TrimEnd('\r');
            fila = fila.TrimEnd(' ');
            return fila;
        }

        public IList<ComboEntidadesRecuperoResultado> ConsultarComboEntidadesRecupero()
        {
            return _recuperoRepositorio.ConsultarComboEntidadesRecupero();
        }
    }
}
