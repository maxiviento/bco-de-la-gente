using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Configuracion.Dominio.IRepositorio;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using Soporte.Aplicacion.Servicios;

namespace Formulario.Aplicacion.Servicios
{
    public class LineaPrestamoServicio
    {
        private readonly ILineaPrestamoRepositorio _lineaPrestamoRepositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly IConfiguracionChecklistRepositorio _configuracionChecklistRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public LineaPrestamoServicio(ILineaPrestamoRepositorio lineaPrestamoRepositorio,
            IMotivoBajaRepositorio motivoBajaRepositorio, IConfiguracionChecklistRepositorio configuracionChecklistRepositorio, 
            ISesionUsuario sesionUsuario)
        {
            _lineaPrestamoRepositorio = lineaPrestamoRepositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _configuracionChecklistRepositorio = configuracionChecklistRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public DetalleLineaParaFormularioResultado DetalleLineaParaFormulario(int id)
        {
            return _lineaPrestamoRepositorio.DetalleLineaParaFormulario(id);
        }

        public IList<DetalleLineaParaFormularioResultado> ObtenerDetallesLinea()
        {
            return _lineaPrestamoRepositorio.ObtenerDetallesLinea();
        }

        // Obtiene cuadrantes por id detalle. usado desde formulario
        public List<CuadranteResultado> ObtenerCuadrantesPorLinea(decimal id)
        {
            return _lineaPrestamoRepositorio.ObtetenerCuadrantesPorLinea(id);
        }

        // Obtiene cuadrantes por id linea. usado desde configuracion de formularios
        public List<CuadranteResultado> ObtenerCuadrantesPorIdLinea(decimal idLinea)
        {
            return _lineaPrestamoRepositorio.ObtetenerCuadrantesPorDetalleLinea(idLinea);
        }

        // Obtiene todos los Convenios (Pago y Recupero)
        public IList<Convenio> ObtenerConvenios()
        {
            return _lineaPrestamoRepositorio.ObtenerConvenios();
        }

        public IList<LineaPrestamoResultado> ConsultarLineas()
        {
            var lineas = _lineaPrestamoRepositorio.ConsultarLineas();
            var lineasResultado = lineas.Select(linea => new LineaPrestamoResultado
                {
                    Id = linea.Id,
                    Nombre = linea.Nombre,
                    Descripcion = linea.Descripcion,
                    Objetivo = linea.Objetivo,
                    Color = linea.Color
                })
                .ToList();

            return lineasResultado;
        }

        public Id RegistrarLineaPrestamo(RegistrarLineaComando comando)
        {
            if (ExisteLineaConMismoNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException("Ya existe una linea con el nombre ingresado.");
            }
            var usuario = _sesionUsuario.Usuario;

            var requisitos = new List<RequisitoPrestamo>();
            var requisitoPrestamo = new RequisitoPrestamo();
            var items = comando.Requisitos.Select(requisito => new Item {Id = new Id(requisito.Item)}).ToList();

            requisitoPrestamo.AgregarItems(items);
            requisitos.Add(requisitoPrestamo);

            var sexoDestinatario = new SexoDestinatario {Id = new Id(comando.SexoDestinatario.Id ?? 0)};
            var motivoDestino = new MotivoDestino {Id = new Id(comando.MotivoDestino.Id ?? 0)};
            var programa = new Programa {Id = new Id(comando.Programa.Id ?? 0)};

            IList<DetalleLineaPrestamo> detalles =
                comando.DetalleLineaPrestamo.Select(CrearDetalleLineaPrestamo).ToList();

            var pathLogo = GuardarLogo(comando.Logo.Buffer, comando.Logo.FileName);
            var pathPieDePagina = GuardarPieDePagina(comando.PiePagina.Buffer, comando.PiePagina.FileName);

            var linea = new LineaPrestamo(
                comando.ConOng,
                comando.ConCurso,
                comando.ConPrograma,
                programa,
                comando.Nombre,
                comando.Descripcion,
                comando.Objetivo,
                requisitos,
                sexoDestinatario,
                comando.Color,
                pathLogo,
                pathPieDePagina,
                detalles,
                motivoDestino,
                usuario,
                comando.LocalidadIds,
                comando.DeptoLocalidad);

            var idLinea = _lineaPrestamoRepositorio.RegistrarLineaPrestamo(linea);

            if (comando.LsOng != null) { 
                foreach(var ong in comando.LsOng)
                {
                    _lineaPrestamoRepositorio.RegistrarOngLinea(-1, idLinea.Valor, ong.Id.Valor, ong.PorcentajeRecupero, ong.PorcentajePago, usuario.Id.Valor);
                }
            }

            var idDetalles = detalles.Select(detalle =>
                    _lineaPrestamoRepositorio.RegistrarDetalleLineaPrestamo(idLinea, detalle))
                .ToList();

            var listaItemsRequisitos = CrearStringRequisitos(comando.Requisitos);
            _lineaPrestamoRepositorio.AsignarRequisitosLinea(idLinea, listaItemsRequisitos, usuario.Id);

            return idLinea;
        }

        private bool ExisteLineaConMismoNombre(string nombre)
        {
            return _lineaPrestamoRepositorio.ExisteListaConMismoNombre(nombre);
        }

        private string CrearStringRequisitos(IEnumerable<RegistrarRequisitoComando> requisitos)
        {
            var itemsLista = "";
            var ultimoRequisitoPrestamo = requisitos.Last();

            foreach (var requisito in requisitos)
            {
                if (requisito == ultimoRequisitoPrestamo)
                {
                    itemsLista = itemsLista + requisito.Item;
                }
                else
                {
                    itemsLista = itemsLista + requisito.Item + ",";
                }
            }

            return itemsLista;
        }

        public DetalleLineaPrestamo CrearDetalleLineaPrestamo(RegistrarDetalleLineaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;

            var tipoIntegrante = new TipoIntegranteSocio {Id = new Id(comando.IntegranteSocio.Id ?? 0)};
            var tipoFinanciamiento = new TipoFinanciamiento {Id = new Id(comando.TipoFinanciamiento.Id ?? 0)};
            var tipoInteres = new TipoInteres {Id = new Id(comando.TipoInteres.Id ?? 0)};
            var tipoGarantia = new TipoGarantia {Id = new Id(comando.TipoGarantia.Id ?? 0)};
            //var convenioPago = new Convenio {Id = new Id(comando.ConvenioPago.Id ?? 0)};

            return new DetalleLineaPrestamo(
                tipoIntegrante,
                comando.MontoTope == null ? default(decimal) : decimal.Parse(comando.MontoTope),
                decimal.Parse(comando.MontoPrestable),
                int.Parse(comando.CantidadMaximaIntegrantes),
                int.Parse(comando.CantidadMinimaIntegrantes),
                tipoFinanciamiento,
                tipoInteres,
                int.Parse(comando.PlazoDevolucion),
                Convert.ToDecimal(comando.ValorCuotaSolidaria, CultureInfo.InvariantCulture),
                tipoGarantia,
                comando.Visualizacion,
                comando.EsApoderado,
                comando.ConvenioRecupero,
                comando.ConvenioPago,
                usuario);
        }

        public string GuardarLogo(byte[] fileBuffer, string fileName)
        {
            string rutaArchivo = FileUtil.GenerarRutaArchivoEnDirectorio(fileName, "~/LogosBGE/");

            FileStream fs = new FileStream(rutaArchivo, FileMode.Create);
            fs.Write(fileBuffer, 0, fileBuffer.Length);
            fs.Close();

            return rutaArchivo;
        }

        public string GuardarPieDePagina(byte[] fileBuffer, string fileName)
        {
            string rutaArchivo = FileUtil.GenerarRutaArchivoEnDirectorio(fileName, "~/PiesDePaginaBGE/");

            FileStream fs = new FileStream(rutaArchivo, FileMode.Create);
            fs.Write(fileBuffer, 0, fileBuffer.Length);
            fs.Close();

            return rutaArchivo;
        }

        public string RegistrarOrdenFormulario(RegistrarOrdenFormularioComando comando)
        {
            var ordenCuadrantes = new List<OrdenCuadrante>();
            foreach (var cuadrante in comando.Cuadrantes)
            {
                var orden = new OrdenCuadrante(cuadrante.Orden, new Cuadrante(cuadrante.IdCuadrante),
                    new TipoSalida(cuadrante.IdTipoSalida));
                ordenCuadrantes.Add(orden);
            }

            return _lineaPrestamoRepositorio.RegistrarOrdenCuadrantes(comando.IdDetalleLinea, ordenCuadrantes);
        }

        public IList<Cuadrante> ConsultarCuadrantes()
        {
            return _lineaPrestamoRepositorio.ConsultarCuadrantes();
        }

        public Resultado<LineaPrestamoGrillaResultado> ConsultarLineasPorFiltro(LineaPrestamoConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new LineaPrestamoConsulta {NumeroPagina = 0};
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            var lineasResultado = _lineaPrestamoRepositorio.ConsultarLineasPorFiltro(consulta);
            return lineasResultado;
        }

        public Resultado<DetalleLineaGrillaResultado> ConsultarDetallePorIdLinea(DetalleLineaConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new DetalleLineaConsulta {NumeroPagina = 0};
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            var detalleLineaResultado = _lineaPrestamoRepositorio.ConsultarDetallePorIdLinea(consulta);
            return detalleLineaResultado;
        }

        public Id DarDeBajaLinea(BajaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var motivoBaja = _motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja));
            var lineaResultado = _lineaPrestamoRepositorio.ConsultarLineaPorId(new Id(comando.IdLinea));
            if (lineaResultado.FechaBaja != null)
                throw new ModeloNoValidoException("La línea ya se encuentra dada de baja");
            var linea = new LineaPrestamo(new Id(comando.IdLinea), motivoBaja, usuario);
            return _lineaPrestamoRepositorio.DarDeBajaLineaPrestamo(linea);
        }

        public Id DarDeBajaDetalle(BajaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var motivoBaja = _motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja));
            var detalleResultado = _lineaPrestamoRepositorio.ConsultarDetallePorId(new Id(comando.IdDetalle));
            if (detalleResultado.FechaBaja != null)
                throw new ModeloNoValidoException("El detalle de la línea ya se encuentra dado de baja");
            var detalle = new DetalleLineaPrestamo(new Id(comando.IdDetalle), motivoBaja, usuario);
            return _lineaPrestamoRepositorio.DarDeBajaDetalleLineaPrestamo(new Id(comando.IdLinea), detalle);
        }

        public Id ModificarLinea(ModificarLineaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;

            var requisitos = new List<RequisitoPrestamo>();
            var items = new List<Item>();
            var requisitoPrestamo = new RequisitoPrestamo();
            foreach (var requisito in comando.Requisitos)
            {
                items.Add(new Item {Id = new Id(requisito.Item)});
            }
            requisitoPrestamo.AgregarItems(items);
            requisitos.Add(requisitoPrestamo);

            var sexoDestinatario = new SexoDestinatario {Id = new Id(comando.SexoDestinatario.Id ?? 0)};
            var motivoDestino = new MotivoDestino {Id = new Id(comando.MotivoDestino.Id ?? 0)};
            var programa = new Programa {Id = new Id(comando.Programa.Id ?? 0)};

            string pathLogo;
            if (String.IsNullOrEmpty(comando.LogoCargado))
            {
                if (comando.Logo.Buffer == null)
                {
                    throw new ErrorTecnicoException("Debe seleccionar un archivo para el logo de la línea de préstamo");
                }
                pathLogo = GuardarLogo(comando.Logo.Buffer, comando.Logo.FileName);
            }
            else
            {
                pathLogo = comando.LogoCargado;
            }

            string pathPieDePagina;
            if (String.IsNullOrEmpty(comando.PiePaginaCargado))
            {
                if (comando.PiePagina.Buffer == null)
                {
                    throw new ErrorTecnicoException(
                        "Debe seleccionar un archivo para el pie de página de la línea de préstamo");
                }
                pathPieDePagina = GuardarPieDePagina(comando.PiePagina.Buffer, comando.PiePagina.FileName);
            }
            else
            {
                pathPieDePagina = comando.PiePaginaCargado;
            }

            var linea = new LineaPrestamo()
                .ModificarLineaPrestamo(
                    new Id(comando.Id ?? 0),
                    comando.ConOng,
                    comando.ConCurso,
                    comando.ConPrograma,
                    programa,
                    comando.Nombre,
                    comando.Descripcion,
                    comando.Objetivo,
                    requisitos,
                    sexoDestinatario,
                    comando.Color,
                    pathLogo,
                    pathPieDePagina,
                    motivoDestino,
                    usuario,
                    comando.LocalidadIds,
                    comando.DeptoLocalidad);

            var idLinea = _lineaPrestamoRepositorio.ModificarLineaPrestamo(linea);
            var listaItems = CrearStringRequisitos(comando.Requisitos);
            _lineaPrestamoRepositorio.AsignarRequisitosLinea(idLinea, listaItems, usuario.Id);
            return idLinea;
        }

        // Agrega o modifica un detalle de la línea de préstamo
        public Id ActualizarDetalle(DetalleLineaPrestamoComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;

            var tipoIntegrante = new TipoIntegranteSocio {Id = new Id(comando.IntegranteSocio.Id ?? 0)};
            var tipoFinanciamiento = new TipoFinanciamiento {Id = new Id(comando.TipoFinanciamiento.Id ?? 0)};
            var tipoInteres = new TipoInteres {Id = new Id(comando.TipoInteres.Id ?? 0)};
            var tipoGarantia = new TipoGarantia {Id = new Id(comando.TipoGarantia.Id ?? 0)};


            DetalleLineaPrestamo detalle;
            if (comando.Id == null)
            {
                detalle = new DetalleLineaPrestamo(
                    tipoIntegrante,
                    comando.MontoTope == null ? default(Decimal) : Decimal.Parse(comando.MontoTope),
                    Decimal.Parse(comando.MontoPrestable),
                    int.Parse(comando.CantidadMaximaIntegrantes),
                    int.Parse(comando.CantidadMinimaIntegrantes),
                    tipoFinanciamiento,
                    tipoInteres,
                    int.Parse(comando.PlazoDevolucion),
                    Convert.ToDecimal(comando.ValorCuotaSolidaria, CultureInfo.InvariantCulture),
                    tipoGarantia,
                    comando.Visualizacion,
                    comando.Apoderado,
                    comando.ConvenioRecupero,
                    comando.ConvenioPago,
                    usuario);
            }
            else
            {
                detalle = new DetalleLineaPrestamo()
                    .ModificarDetalleLinea(
                        new Id(comando.Id ?? 0),
                        tipoIntegrante,
                        comando.MontoTope == null ? default(Decimal) : Decimal.Parse(comando.MontoTope),
                        Decimal.Parse(comando.MontoPrestable),
                        Int32.Parse(comando.CantidadMaximaIntegrantes),
                        Int32.Parse(comando.CantidadMinimaIntegrantes),
                        tipoFinanciamiento,
                        tipoInteres,
                        Int32.Parse(comando.PlazoDevolucion),
                        Convert.ToDecimal(comando.ValorCuotaSolidaria, CultureInfo.InvariantCulture),
                        tipoGarantia,
                        comando.Visualizacion,
                        comando.Apoderado,
                        comando.ConvenioRecupero,
                        comando.ConvenioPago,
                        usuario);
            }
            Id idDetalle =
                _lineaPrestamoRepositorio.ActualizarDetalleLineaPrestamo(new Id(comando.LineaId ?? 0), detalle);

            return idDetalle;
        }


        public IList<RequisitosLineaResultado> ConsultarRequisitosPorIdLinea(Id lineaId, bool incluyeItemsCheckList)
        {
            var requisitosBase = _lineaPrestamoRepositorio.ConsultarRequisitosPorIdLinea(lineaId, incluyeItemsCheckList);
            var nombreLinea = "";

            if (requisitosBase.Count > 0)
            {
                nombreLinea = requisitosBase.First().NombreLinea;
            }

            var requisitosAgrupadosPorItem = requisitosBase.GroupBy(resultado => resultado.IdItem)
                .Select(resultadoAgrupado => new RequisitosLineaResultado
                {
                    NombreLinea = nombreLinea,
                    NombreItem = resultadoAgrupado.Select(item => item.NombreItem).First(),
                    IdItem = resultadoAgrupado.Key,
                    TiposItems = resultadoAgrupado.Select(tipoItem => new RequisitosLineaResultado.TiposItem
                    {
                        Id = tipoItem.IdTipoItem,
                        Nombre = tipoItem.NombreTipoItem
                    })
                })
                .ToList();

            return requisitosAgrupadosPorItem;
        }

        public DetalleLineaResultado ConsultarDetallePorId(Id detalleId)
        {
            return _lineaPrestamoRepositorio.ConsultarDetallePorId(detalleId);
        }

        public IList<DetalleLineaCombo> ObtenerDetallesLineaCombo(decimal lineaId)
        {
            return _lineaPrestamoRepositorio.ObtenerDetallesLineaCombo(lineaId);
        }

        public LineaPrestamoResultado ConsultarLineaPorId(Id id)
        {
            return _lineaPrestamoRepositorio.ConsultarLineaPorId(id);
        }

        public IList<OngLinea> ObtenerOngsLinea(Id idLinea)
        {
            var resultados = _lineaPrestamoRepositorio.ObtenerOngsLinea(idLinea);
            var lsOng = new List<OngLinea>();

            foreach(var res in resultados)
            {
                var ong = new OngLinea()
                {
                    Id = res.IdOng,
                    IdLineaOng = res.IdLineaOng.Value,
                    Nombre = res.NombreOng,
                    PorcentajePago = res.PorcentajePrestamo.Value,
                    PorcentajeRecupero = res.PorcentajeRecupero.Value
                };
                lsOng.Add(ong);
            }
            return lsOng;
        }            

        public IList<LocalidadResultado> ConsultarLocalidadesLineaPorId(Id id)
        {
            var localidades = _lineaPrestamoRepositorio.ConsultarLocalidadesLineaPorId(id);

            return localidades;
        }

        public IList<DetalleLineaGrillaResultado> ConsultarDetallePorIdLineaSinPaginar(Id lineaId)
        {
            return _lineaPrestamoRepositorio.ConsultarDetallePorIdLineaSinPaginar(lineaId);
        }

        public byte[] DescargarArchivo(string path)
        {
            if (!FileUtil.ExisteDirectorio(path))
            {
                throw new ModeloNoValidoException("No se puede encontrar la ruta de acceso al archivo.");
            }
            try
            {
                var archivo = File.ReadAllBytes(path);
                return archivo;
            }
            catch (Exception e)
            {
                Trace.Write(e.StackTrace);
                throw new ModeloNoValidoException("No se puede encontrar el archivo que desea descargar.");
            }
        }

        public IList<RequisitosCuadranteResultado> ConsultarRequisitosParaCuadrantePorIdLinea(Id lineaId, bool incluyeItemsCheckList)
        {
            var requisitosBase = _lineaPrestamoRepositorio.ConsultarRequisitosPorIdLinea(lineaId, incluyeItemsCheckList);
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

        public IList<LineaParaComboResultado> ConsultarLineaParaCombo(bool multiple)
        {
            IList<LineaParaComboResultado> lista = new List<LineaParaComboResultado>();
            if (multiple)
            {
                var lineas = _lineaPrestamoRepositorio.ConsultarLineaParaCombo();
                foreach (var linea in lineas)
                {
                    linea.DadoDeBaja = linea.DadaDeBaja == "S";
                    bool cargada = lista.Any(x => x.Id == linea.Id);
                    if(!cargada)
                        lista.Add(linea);
                }
            }
            else
            {
                var lineas = _lineaPrestamoRepositorio.ConsultarLineas();
                foreach (var linea in lineas)
                {
                    lista.Add(new LineaParaComboResultado
                    {
                        Id = (int) linea.Id.Valor,
                        Descripcion = linea.Nombre + " - " + linea.Descripcion
                    });
                }

            }
            return lista;
        }

        public IList<ItemResultado.BandejaConfiguracionChecklist> ObtenerItemsConfiguracionChecklist(
            ConsultaConfiguracionChecklist consulta)
        {
            var items = _lineaPrestamoRepositorio.ObtenerItemsConfiguracionChecklists(consulta);
            return items;
        }

        public string RegistrarConfiguracionChecklist(ConfiguracionChecklistComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var listaRequisitos = comando.Requisitos.Select((item) => new RequisitoPrestamo(
                new Item(new Id(item.IdItem)),
                new Area(new Id(item.IdArea)),
                new Etapa(new Id(item.IdEtapa)),
                item.NroOrden));

            var etapasEstados = comando.EtapasEstados.Select(x => new EtapaEstadosLineas
            {
                Id = new Id(x.Id ?? -1),
                EtapaAnterior = new Etapa(new Id(x.IdEtapaActual ?? -1)),
                EstadoAnterior = new EstadoPrestamo((int)x.IdEstadoActual),
                EtapaSiguiente = new Etapa(new Id(x.IdEtapaSiguiente ?? -1)),
                EstadoSiguiente = new EstadoPrestamo((int)x.IdEstadoSiguiente),
                Linea = new LineaPrestamo { Id = new Id(comando.IdLinea) },
                Orden = (int) x.Orden
            });

            var versionActual = _configuracionChecklistRepositorio.ObtenerVersionVigente(new Id(comando.IdLinea));

            // El registro de items será el que valide si la versión está en uso y en caso de estarlo generará una nueva versión
            var res = _lineaPrestamoRepositorio.RegistrarConfiguracionChecklist(
                new Id(comando.IdLinea),
                new Id(comando.IdEtapa),
                listaRequisitos,
                usuario);

            // Consulto la versión vigente ya que quizá el registro de items generó una nueva versión
            var version = _configuracionChecklistRepositorio.ObtenerVersionVigente(new Id(comando.IdLinea));
            foreach (var etapaEstado in etapasEstados)
            {
                _lineaPrestamoRepositorio.RegistrarEtapaEstadoLinea(etapaEstado, version?.Id ?? new Id(-1), usuario);
            }

            // Si la versión sigue siendo la misma se eliminan los registros que se eliminaron en la pantalla
            if (versionActual != null && version != null && versionActual.Id == version.Id)
            {
                // para registrar las eliminaciones de registros de etapa_estado se llama a otro sp y se envian los ids de los eliminados
                foreach (var id in comando.IdsEtapasEliminadas)
                {
                    _lineaPrestamoRepositorio.EliminarEtapaEstadoLinea(new Id(id), usuario);
                }
            }

            return res;
        }

        public VersionChecklistResultado ObtenerVersionChecklistVigente(decimal idLinea)
        {
            return _configuracionChecklistRepositorio.ObtenerVersionVigente(new Id(idLinea));
        }

        public IEnumerable<LineaParaComboResultado> ConsultarNombresLineas()
        {
            var resultadoDb = _lineaPrestamoRepositorio.ConsultarLineasParaFiltrarPorTexto();
            return resultadoDb.Select(item => new LineaParaComboResultado
                {
                    Descripcion = item.Nombre,
                    DadoDeBaja = item.IdMotivoBaja.Valor != 0
                })
                .ToList();
        }

        public IList<EtapaEstadoLineaResultado> ObtenerEtapasxEstadosLinea(long idLinea, long? idPrestamo)
        {
            return _lineaPrestamoRepositorio.ObtenerEtapasEstadosLinea(idLinea, idPrestamo)
                .OrderBy(x => x.Orden)
                .ToList();
        }

        public IList<OngComboResultado> ObtenerOngs()
        {
            return _lineaPrestamoRepositorio.ObtenerOngs();
        }

        public IList<OngComboResultado> ObtenerOngsPorNombre(string nombre)
        {
            return _lineaPrestamoRepositorio.ObtenerOngsPorNombre(nombre);
        }

        public void ModificarOngLinea(ModificacionOngLineaComando comando)
        {
            var idLinea = comando.IdLinea;
            var idUsuario = _sesionUsuario.Usuario.Id.Valor;

            foreach (var ong in comando.LsOngAgregadas)
            {
                _lineaPrestamoRepositorio.RegistrarOngLinea(-1, idLinea, ong.Id.Valor, ong.PorcentajeRecupero, ong.PorcentajePago, idUsuario);
            }

            foreach(var ong in comando.LsOngQuitadas)
            {
                _lineaPrestamoRepositorio.RegistrarOngLinea(ong.IdLineaOng, -1, ong.Id.Valor, ong.PorcentajeRecupero, ong.PorcentajePago, idUsuario);
            }
        }
    }
}