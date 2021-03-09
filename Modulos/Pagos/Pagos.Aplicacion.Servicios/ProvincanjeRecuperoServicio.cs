using System;
using System.Linq;
using Identidad.Dominio.Modelo;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Pagos.Aplicacion.Servicios
{
    public class ProvincanjeRecuperoServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IRecuperoRepositorio _recuperoRepositorio;

        public ProvincanjeRecuperoServicio(
            ISesionUsuario sesionUsuario,
            IRecuperoRepositorio recuperoRepositorio)
        {
            _sesionUsuario = sesionUsuario;
            _recuperoRepositorio = recuperoRepositorio;
        }

        public ImportarArchivoRecuperoResultado ProvincanjeArchivoRecupero(string[] filas, decimal idCabeceraArchivo, string nombreArchivo)
        {
            var resultado = new ImportarArchivoRecuperoResultado();

            var posicionFila = new decimal(0.0);

            foreach (var fila in filas)
            {
                var filaLimpia = LimpiarFila(fila);
                posicionFila++;
                if (filaLimpia.Length == 0)  // no le doy bola a las filas en blanco
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
                var nuevaFila = new FilaArchivoRecupero();

                try
                {
                    if (nombreArchivo.Substring(0, 4).Equals("0184"))
                    {
                        if (filaLimpia.Length != 35)
                        {
                            _recuperoRepositorio.RegistrarDetalleArchivoRecupero(idCabeceraArchivo, 0, 0, 0, default(DateTime), _sesionUsuario.Usuario.Id.Valor, posicionFila, (decimal)MotivoRechazoEnum.LongitudFila35Banco);
                            resultado.CantIncons++;
                            continue;
                        }
                        nuevaFila = ParsearConConvenio0184(filaLimpia, idCabeceraArchivo);
                    }

                    if (nombreArchivo.Substring(0, 4).Equals("2686"))
                    {
                        if (filaLimpia.Substring(0,1).Equals("1") || filaLimpia.Substring(0, 1).Equals("3"))
                        {
                            continue;
                        }
                        nuevaFila = ParsearConConvenio2686(filaLimpia, idCabeceraArchivo);
                    }
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

        private bool FormularioPoseePlanPago(decimal nroFormulario)
        {
            return _recuperoRepositorio.ValidarPlanPagoGenerado(nroFormulario);
        }

        private bool FormularioExiste(decimal nroFormulario)
        {
            return _recuperoRepositorio.ValidarFormulario(nroFormulario) != null;
        }

        private FilaArchivoRecupero ParsearConConvenio0184(string filaLimpia, decimal idCabeceraArchivo)
        {
            var nuevaFila = new FilaArchivoRecupero();
            nuevaFila.IdCabecera = idCabeceraArchivo;
            nuevaFila.NroFormulario = decimal.Parse(filaLimpia.Substring(2, 7));
            nuevaFila.NroCuota = decimal.Parse(filaLimpia.Substring(9, 9));
            nuevaFila.Fecha = DateTime.ParseExact(filaLimpia.Substring(18, 8), "ddMMyyyy", null);
            nuevaFila.Monto = decimal.Parse(filaLimpia.Substring(26, 7));
            var montoDecimal = decimal.Parse("0," + filaLimpia.Substring(33, 2));
            nuevaFila.Monto += montoDecimal;
            return nuevaFila;
        }

        private FilaArchivoRecupero ParsearConConvenio2686(string filaLimpia, decimal idCabeceraArchivo)
        {
            var nuevaFila = new FilaArchivoRecupero();
            nuevaFila.IdCabecera = idCabeceraArchivo;
            nuevaFila.NroFormulario = decimal.Parse(filaLimpia.Substring(13, 7));
            nuevaFila.NroCuota = decimal.Parse(filaLimpia.Substring(20, 2));
            nuevaFila.Fecha = DateTime.ParseExact(filaLimpia.Substring(50, 6), "yyMMdd", null);
            nuevaFila.Monto = decimal.Parse(filaLimpia.Substring(29, 8));
            var montoDecimal = decimal.Parse("0," + filaLimpia.Substring(37, 2));
            nuevaFila.Monto += montoDecimal;
            return nuevaFila;
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
    }
}
