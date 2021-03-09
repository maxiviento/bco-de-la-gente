using System;
using System.Linq;
using Identidad.Dominio.Modelo;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Pagos.Aplicacion.Servicios
{
    public class PuntoBancorRecuperoServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IRecuperoRepositorio _recuperoRepositorio;

        public PuntoBancorRecuperoServicio(
            ISesionUsuario sesionUsuario,
            IRecuperoRepositorio recuperoRepositorio)
        {
            _sesionUsuario = sesionUsuario;
            _recuperoRepositorio = recuperoRepositorio;
        }

        public ImportarArchivoRecuperoResultado PuntoBancorArchivoRecupero(string[] filas, decimal idCabeceraArchivo, string nombreArchivo)
        {
            var resultado = new ImportarArchivoRecuperoResultado();

            if (nombreArchivo.Substring(0,4) == "0184" )
            {
                resultado = ProcesarArchivoBancorConvenio0184(filas, idCabeceraArchivo, nombreArchivo);
            }

            if (nombreArchivo.Substring(0, 4) == "2686")
            {
                resultado = ProcesarArchivoBancorConvenio2686(filas, idCabeceraArchivo);
            }

            return resultado;
        }

        private ImportarArchivoRecuperoResultado ProcesarArchivoBancorConvenio0184(string[] filas, decimal idCabeceraArchivo, string nombreArchivo)
        {
            var resultado = new ImportarArchivoRecuperoResultado();

            var posicionFila = new decimal(0.0);

            foreach (var fila in filas)
            {
                var filaLimpia = LimpiarFila(fila);
                posicionFila++;
                if (filaLimpia.Length == 0)  // ignoro las filas en blanco
                {
                    continue;
                }
                if (filaLimpia.Length != 35)
                {
                    _recuperoRepositorio.RegistrarDetalleArchivoRecupero(idCabeceraArchivo, 0, 0, 0, default(DateTime), _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.LongitudFila35Banco);
                    resultado.CantIncons++;
                    continue;
                }

                //Verificar si la linea es numerica
                if (!fila.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").All(char.IsDigit))
                {
                    _recuperoRepositorio.RegistrarDetalleArchivoRecupero(idCabeceraArchivo, 0, 0, 0, default(DateTime), _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.CaracteresNoNumericos);
                    resultado.CantIncons++;
                    continue;
                }

                var nuevaFila = new FilaArchivoRecupero();
                try
                {
                    nuevaFila.IdCabecera = idCabeceraArchivo;
                    nuevaFila.NroFormulario = decimal.Parse(filaLimpia.Substring(6, 7));
                    nuevaFila.NroCuota = decimal.Parse(filaLimpia.Substring(14, 2));
                    nuevaFila.Fecha = DateTime.ParseExact(nombreArchivo.Substring(4, 6), "yyMMdd", null);
                    nuevaFila.Monto = decimal.Parse(filaLimpia.Substring(28, 5));
                    var montoDecimal = decimal.Parse("0," + filaLimpia.Substring(33, 2));
                    nuevaFila.Monto += montoDecimal;
                }
                catch (Exception)
                {
                    //Validar que existe el formulario
                    if (!FormularioExiste(nuevaFila.NroFormulario))
                    {
                        _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.NoSeEncontroFormulario);
                        resultado.CantIncons++;
                        resultado.MontoRechazado += nuevaFila.Monto;
                        continue;
                    }
                    //Validar que el formulario tiene plan de cuotas generado
                    if (!FormularioPoseePlanPago(nuevaFila.NroFormulario))
                    {
                        _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.PrestamoNoTienePlanPagoGenerado);
                        resultado.CantIncons++;
                        resultado.MontoRechazado += nuevaFila.Monto;
                        continue;
                    }
                    _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.ErrorDatoResultadoBanco);
                    resultado.CantIncons++;
                    continue;
                }
                var res = _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila);
                resultado = ControlarRespuestaDetalleRecupero(resultado, res, nuevaFila.Monto);
            }
            return resultado;
        }
       
        private ImportarArchivoRecuperoResultado ProcesarArchivoBancorConvenio2686(string[] filas, decimal idCabeceraArchivo)
        {
            var resultado = new ImportarArchivoRecuperoResultado();

            var posicionFila = new decimal(0.0);

            foreach (var fila in filas)
            {
                var nuevaFila = new FilaArchivoRecupero();
                try
                {
                    var filaLimpia = LimpiarFila(fila);
                    posicionFila++;
                    if (filaLimpia.Length == 0) // ignoro las filas en blanco
                    {
                        continue;
                    }

                    //Verificar si la linea es numerica
                    if (!fila.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").All(char.IsDigit))
                    {
                        _recuperoRepositorio.RegistrarDetalleArchivoRecupero(idCabeceraArchivo, 0, 0, 0, default(DateTime), _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.CaracteresNoNumericos);
                        resultado.CantIncons++;
                        continue;
                    }
                    
                    if (filaLimpia.Substring(0, 1).Equals("1") || filaLimpia.Substring(0, 1).Equals("3"))
                    {
                        continue;
                    }

                    nuevaFila.IdCabecera = idCabeceraArchivo;
                    nuevaFila.NroFormulario = decimal.Parse(filaLimpia.Substring(13, 7));
                    nuevaFila.NroCuota = decimal.Parse(filaLimpia.Substring(20, 2));
                    nuevaFila.Fecha = DateTime.ParseExact(filaLimpia.Substring(50, 6), "yyMMdd", null);
                    nuevaFila.Monto = decimal.Parse(filaLimpia.Substring(29, 8));
                    var montoDecimal = decimal.Parse("0," + filaLimpia.Substring(37, 2));
                    nuevaFila.Monto += montoDecimal;
                }
                catch (Exception)
                {
                    _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.ErrorDatoResultadoBanco);
                    resultado.CantIncons++;
                    continue;
                }
                //Validar que existe el formulario
                if (!FormularioExiste(nuevaFila.NroFormulario))
                {
                    _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.NoSeEncontroFormulario);
                    resultado.CantIncons++;
                    resultado.MontoRechazado += nuevaFila.Monto;
                    continue;
                }
                //Validar que el formulario tiene plan de cuotas generado
                if (!FormularioPoseePlanPago(nuevaFila.NroFormulario))
                {
                    _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.PrestamoNoTienePlanPagoGenerado);
                    resultado.CantIncons++;
                    resultado.MontoRechazado += nuevaFila.Monto;
                    continue;
                }
                var res = _recuperoRepositorio.RegistrarDetalleArchivoRecupero(nuevaFila.IdCabecera, nuevaFila.NroFormulario, nuevaFila.NroCuota, nuevaFila.Monto, nuevaFila.Fecha, _sesionUsuario.Usuario.Id.Valor, posicionFila);
                resultado = ControlarRespuestaDetalleRecupero(resultado, res, nuevaFila.Monto);
            }
            return resultado;
        }

        private static ImportarArchivoRecuperoResultado ControlarRespuestaDetalleRecupero(ImportarArchivoRecuperoResultado resultado, string respuesta, decimal monto)
        {
            switch (respuesta.ToUpper())
            {
                case "OK":
                    resultado.CantProc++;
                    resultado.MontoRecuperado += monto;
                    break;
                case "ESPECIAL":
                    resultado.CantEspec++;
                    resultado.MontoRecuperado += monto;
                    break;
                case "AMBAS":
                    resultado.CantEspec++;
                    resultado.CantProc++;
                    resultado.MontoRecuperado += monto;
                    break;
                case "INCONSISTENCIA":
                    resultado.CantIncons++;
                    break;
                default:
                    throw new Exception("Respuesta de la base desconocida.");
            }
            return resultado;
        }

        private string LimpiarFila(string fila)
        {
            fila = fila.TrimEnd('\r');
            fila = fila.TrimEnd(' ');
            return fila;
        }
        private bool FormularioPoseePlanPago(decimal nroFormulario)
        {
            return _recuperoRepositorio.ValidarPlanPagoGenerado(nroFormulario);
        }

        private bool FormularioExiste(decimal nroFormulario)
        {
            return _recuperoRepositorio.ValidarFormulario(nroFormulario) != null;
        }
    }
}
