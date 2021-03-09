import { Component, OnDestroy, OnInit } from '@angular/core';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { LineaPrestamo } from '../shared/modelo/linea-prestamo.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { SexoDestinatario } from '../shared/modelo/destinatario-prestamo.model';
import { MotivoDestino } from '../../motivo-destino/shared/modelo/motivo-destino.model';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../shared/notificacion.service';
import { LineaDetalleComponent } from '../shared/linea-detalle/linea-detalle.component';
import { LineaItemsComponent } from '../shared/linea-items/linea-items.component';
import { IntegranteSocio } from '../shared/modelo/integrante-socio.model';
import { TipoGarantia } from '../shared/modelo/tipo-garantia.model';
import { TipoFinanciamiento } from '../shared/modelo/tipo-financiamiento.model';
import { TiposGarantiaService } from '../shared/tipos-garantia.service';
import { TiposInteresService } from '../shared/tipos-interes.service';
import { TiposFinanciamientoService } from '../shared/tipos-financiamiento.service';
import { IntegrantesService } from '../shared/integrantes.service';
import { TipoInteres } from '../shared/modelo/tipo-interes.model';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { AuthService } from '../../core/auth/auth.service';
import { Requisito } from '../shared/modelo/requisito-linea';
import { RequisitosResultado } from '../shared/modelo/resultado-requisitos.model';
import { Convenio } from '../../shared/modelo/convenio-model';
import { ProgramaCombo } from '../shared/modelo/programa-combo.model';
import { Localidad } from '../../shared/domicilio-linea/localidad.model';
import { Subscription } from 'rxjs';
import TituloBanco from '../../shared/titulo-banco';
import { OngLinea } from '../shared/modelo/ong-linea.model';
import { isNullOrUndefined } from 'util';
import { ModificacionOngLineaComando } from '../shared/modelo/modificacion-ong-linea-comando.model';


@Component({
  selector: 'bg-edicion-linea',
  templateUrl: './edicion-linea.component.html',
  styleUrls: ['./edicion-linea.component.scss'],
})

export class EdicionLineaComponent implements OnInit, OnDestroy {
  public fechaActual: Date = new Date(Date.now());
  public linea: LineaPrestamo = new LineaPrestamo();
  public form: FormGroup;
  public pantallaActual: string;
  public bandera: boolean = false;
  public idLineaUrl: any;
  public integrantes: IntegranteSocio[] = [];
  public financiamientos: TipoFinanciamiento[] = [];
  public intereses: TipoInteres[] = [];
  public garantias: TipoGarantia[] = [];
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public localidades: Localidad[] = [];
  public archivoLogo: any;
  public archivoPiePagina: any;
  public maxSize: number = 10;
  public requisitos: RequisitosResultado[] = [];
  public convenios: Convenio[] = [];
  public conveniosPago: Convenio[] = [];
  public conveniosRecupero: Convenio[] = [];
  public deptoLocalidad: boolean = false;
  public esEditable: boolean = true;
  public subscription: Subscription;
  public verOng: boolean = false;
  public lsOngAgregadas: OngLinea[] = [];
  public lsOngEliminadas: OngLinea[] = [];

  constructor(private lineaService: LineaService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private route: ActivatedRoute,
              private router: Router,
              private integranteService: IntegrantesService,
              private tipoFinanciamientoService: TiposFinanciamientoService,
              private tipoInteresService: TiposInteresService,
              private tipoGarantiaService: TiposGarantiaService,
              private sanitizer: DomSanitizer,
              private authService: AuthService,
              private titleService: Title) {
    this.titleService.setTitle('Editar línea de micro-préstamo ' + TituloBanco.TITULO);
    this.linea.detalleLineaPrestamo = [];
    this.linea.requisitos = [];
  }

  public ngOnInit() {
    this.subscription = this.lineaService.obtenerLocalidades().subscribe((localidades: Localidad[]) => {
        this.localidades = localidades;
      }
    );
    this.cargarCombosDetalles();
    this.buscarIdUrl();
    this.crearForm();
    if (!this.pantallaActual) {
      this.pantallaActual = 'linea';
    }
  }

  public ngOnDestroy(): void {
    this.lineaService.asignarLocalidad([]);
  }

  public cargarCombosDetalles() {
    this.integranteService
      .consultarIntegrantes()
      .subscribe((integrante) => this.integrantes = integrante);
    this.tipoFinanciamientoService
      .consultarTiposFinanciamiento()
      .subscribe((financiamiento) => this.financiamientos = financiamiento);
    this.tipoInteresService
      .consultarTiposInteres()
      .subscribe((interes) => this.intereses = interes);
    this.tipoGarantiaService
      .consultarTiposGarantia()
      .subscribe((garantia) => this.garantias = garantia);
    this.lineaService.consultarConvenios()
      .subscribe((resultado) => {
        this.convenios = resultado;
        this.filtrarConvenios();
      });
  }

  private filtrarConvenios() {
    this.conveniosPago = this.convenios.filter((c) => c.idTipoConvenio === 1);
    this.conveniosRecupero = this.convenios.filter((c) => c.idTipoConvenio === 2);
  }

  public getUrlDescarga(path: string) {
    return this.sanitizer
      .bypassSecurityTrustResourceUrl(`/api/lineasprestamo/descargarArchivo?path=${path}&access_token=${this.authService.token()}`);
  }

  private crearForm() {
    this.form = this.fb.group({
      itemLineaForm: LineaItemsComponent.nuevoFormGroup(this.linea),
      detalleLineaForm: LineaDetalleComponent.nuevoFormGroup()
    });

    this.itemLineaForm
      .valueChanges
      .subscribe((value) => {
        if (value) {
          this.bandera = !this.esValido();
        }
      });

    let conOng = this.itemLineaForm.get('conOng');
    let conPrograma = this.itemLineaForm.get('conPrograma');
    let programa = (this.itemLineaForm.get('programa') as FormControl);
    let deptoLocalidad = this.itemLineaForm.get('deptoLocalidad');
    deptoLocalidad
      .valueChanges
      .subscribe(() => {
        this.linea.deptoLocalidad = deptoLocalidad.value;
        this.deptoLocalidad = deptoLocalidad.value;
      });
    if (programa.value == -1) {
      programa.setValue(null, {emitEvent: false});
    }
    conPrograma
      .valueChanges
      .subscribe(() => {
        if (conPrograma.value) {
          programa.enable();
          programa.setValidators(Validators.required);
        } else {
          programa.setValue(null, {emitEvent: false});
          programa.clearValidators();
          programa.disable();
        }
      });

    conOng
      .valueChanges
      .subscribe(() => {
        this.verOng = conOng.value;
        if (isNullOrUndefined(this.linea.lsOng)) {
          this.linea.lsOng = [];
        }
      });
  }

  private deshabiltarPrograma(): void {
    let conPrograma = this.itemLineaForm.get('conPrograma');
    let programa = this.itemLineaForm.get('programa');
    if (!this.linea.conPrograma) {
      programa.setValue(null, {emitEvent: false});
      programa.clearValidators();
      programa.disable();
    }
  }

  private consultarLinea() {
    this.lineaService.consultarLineaPorId(this.idLineaUrl)
      .subscribe((resultado) => {
        this.linea = resultado;
        this.linea.logoCargado = this.linea.logo;
        this.linea.piePaginaCargado = this.linea.piePagina;
        if (this.linea.deptoLocalidad) {
          this.deptoLocalidad = true;
          //this.consultarLocalidades();
        }
        if (this.linea.conOng) {
          this.consultarOngsCargadas();
          this.verOng = true;
        }
        this.consultarDetalle();
        this.consultarRequisitosCargados();
        this.deshabiltarPrograma();
      }, (errores) =>
        this.notificacionService.informar(errores, true));
  }

  private consultarLocalidades() {
    this.lineaService.consultarLocalidadesLineaPorId(this.idLineaUrl)
      .subscribe((resultado) => {
        this.localidades = resultado;
      }, (errores) =>
        this.notificacionService.informar(errores, true));
  }

  private consultarDetalle() {
    this.lineaService.consultarDetallePorIdLineaSinPaginar(this.idLineaUrl)
      .subscribe((resultado) => {
        this.linea.detalleLineaPrestamo = resultado;
        this.agregarNombresCombos();
        this.crearForm();
      }, (errores) =>
        this.notificacionService.informar(errores, true));
  }

  private consultarRequisitosCargados(): void {
    this.lineaService.consultarRequisitosPorLinea(this.idLineaUrl)
      .subscribe((resultado) => {
        this.linea.requisitos = resultado.map((res) => new Requisito(res.idItem));
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  private consultarOngsCargadas(): void {
    this.lineaService.consultarOngPorLinea(this.idLineaUrl)
      .subscribe((resultado) => {
        this.linea.lsOng = resultado;
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  private buscarIdUrl() {
    this.route.params.subscribe((param: Params) => {
      this.idLineaUrl = param['id'];
      this.consultarLinea();
    });
  }

  private agregarNombresCombos(): void {
    this.linea.detalleLineaPrestamo.forEach((detalle) => {
      detalle.integranteSocio = new IntegranteSocio(detalle.idSocioIntegrante, detalle.nombreSocioIntegrante);
      detalle.tipoGarantia = new TipoGarantia(detalle.idTipoGarantia, detalle.nombreTipoGarantia);
      detalle.tipoFinanciamiento = new TipoFinanciamiento(detalle.idTipoFinanciamiento, detalle.nombreTipoFinanciamiento);
      detalle.tipoInteres = new TipoInteres(detalle.idTipoInteres, detalle.nombreTipoInteres);
      detalle.convenioPago = new Convenio(detalle.id, detalle.nombreConvPag);
    });
  }

  public esValido(): boolean {
    return (this.itemLineaForm.valid
      && this.linea.detalleLineaPrestamo && this.linea.detalleLineaPrestamo.length > 0
      && this.linea.requisitos && this.linea.requisitos.length > 0);
  }

  public get itemLineaForm(): FormGroup {
    return this.form.get('itemLineaForm') as FormGroup;
  }

  public archivoSeleccionadoLogo(archivo: File): void {
    let tamanioArchivo = ((archivo.size / 1024) / 1024);
    if (tamanioArchivo < this.maxSize) {
      this.archivoLogo = archivo;
    }
  }

  public archivoSeleccionadoPiePagina(archivo: File): void {
    let tamanioArchivo = ((archivo.size / 1024) / 1024);
    if (tamanioArchivo < this.maxSize) {
      this.archivoPiePagina = archivo;
    }
  }

  public prepararLinea() {
    this.linea.logo = undefined;
    this.linea.piePagina = undefined;
    this.obtenerLinea();
    if (this.archivoLogo) {
      this.linea.logo = this.archivoLogo;
      this.linea.logoCargado = undefined;
    }

    if (this.archivoPiePagina) {
      this.linea.piePagina = this.archivoPiePagina;
      this.linea.piePaginaCargado = undefined;
    }

    if (!this.linea.logoCargado && !this.linea.logo) {
      this.notificacionService.informar(['Debe cargar un archivo como logo/imagen cabecera de línea.']);
      return;
    }
    if (!this.linea.piePaginaCargado && !this.linea.piePagina) {
      this.notificacionService.informar(['Debe cargar un archivo como firmantes/imagen pie de línea.']);
      return;
    }
    if (this.itemLineaForm.get('conPrograma') && !this.itemLineaForm.get('programa')) {
      this.notificacionService.informar(['Debe seleccionar un programa.']);
      return
    }
    if (!this.linea.requisitos) {
      this.notificacionService.informar(['Debe asignar al menos un requisito']);
      return;
    }
    if (this.linea.deptoLocalidad && (!(this.localidadIds.length > 0) || !(this.departamentoIds.length > 0))) {
      this.notificacionService.informar(['Debe seleccionar al menos un departamento o localidad.']);
      return;
    }
    if (this.linea.conOng) {
      if (!this.ongValida()) {
        this.notificacionService.informar(['Debe asignar al menos una ONG a la línea.']);
      } else {
        this.obtenerLinea();
        this.modificarLinea();
      }
    }
    this.obtenerLinea();
    this.modificarLinea();
  }

  private modificarLinea(): void {
    this.linea.localidadIds = this.itemLineaForm.value.deptoLocalidad ? this.localidadIds.join(',') : undefined;
    this.lineaService.modificarLinea(this.linea)
      .subscribe(() => {
        this.notificacionService
          .informar(['La línea de préstamo se modificó con éxito.'])
          .result
          .then(() => this.router.navigate(['/consulta-linea', this.idLineaUrl]));
      }, (errores) => this.notificacionService.informar(errores, true));

    if (this.linea.conOng) {
      if (this.lsOngEliminadas.length || this.lsOngAgregadas.length) {
        let comando = new ModificacionOngLineaComando(this.linea.id, this.lsOngAgregadas, this.lsOngEliminadas);
        this.lineaService.modificarOngLinea(comando).subscribe();
      }
    }
  }

  private obtenerLinea() {
    let itemLineaModel = this.itemLineaForm.value;

    this.linea.conOng = itemLineaModel.conOng;
    this.linea.conCurso = itemLineaModel.conCurso;
    this.linea.conPrograma = itemLineaModel.conPrograma;
    this.linea.id = itemLineaModel.id;
    this.linea.nombre = itemLineaModel.nombre;
    this.linea.descripcion = itemLineaModel.descripcion;
    this.linea.sexoDestinatario = new SexoDestinatario(itemLineaModel.destinatario);
    this.linea.motivoDestino = new MotivoDestino(itemLineaModel.motivoDestino);
    this.linea.objetivo = itemLineaModel.objetivo;
    this.linea.color = itemLineaModel.color;
    if (!this.linea.conPrograma || itemLineaModel.programa == null) {
      this.linea.programa = new ProgramaCombo(-1, 'Sin Programa');
      this.linea.idPrograma = -1;
    } else {
      this.linea.programa = new ProgramaCombo(itemLineaModel.programa);
      this.linea.idPrograma = this.linea.programa.id;
    }
  }

  public modificarPantalla(pantallaActual: string) {
    switch (pantallaActual) {
      case 'linea': {
        this.pantallaActual = 'linea';
        break;
      }
      case 'requisitos': {
        this.pantallaActual = 'requisitos';
        break;
      }
      case 'ong': {
        this.pantallaActual = 'ong';
        break;
      }
      default:
    }
  }

  public guardarLinea(datos: AbstractControl, pantalla: string) {
    if (datos) {
      this.itemLineaForm.get('programa').enable();
      this.itemLineaForm.setValue(datos.value);
      this.obtenerLinea();
    }
    if (pantalla == 'requisitos') {
      this.modificarPantalla('requisitos');
    } else {
      this.modificarPantalla('ong');
    }
  }

  public agregarRequisitos(requisitos: Requisito []) {
    this.linea.requisitos = requisitos;
    this.bandera = !this.esValido();
    this.modificarPantalla('linea');
    this.obtenerLinea();
    this.form.get('programa').disable();
    this.crearForm();
  }

  public agregarOng(lsOng: OngLinea[]) {
    this.linea.lsOng = lsOng;
    this.bandera = !this.esValido();
    this.modificarPantalla('linea');
    this.obtenerLinea();
    this.crearForm();
  }

  public cargarListaOngAgregadas(lista: OngLinea[]) {
    this.lsOngAgregadas = lista;
  }

  public cargarListaOngEliminadas(lista: OngLinea[]) {
    this.lsOngEliminadas = lista;
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades;
  }

  public ongValida(): boolean {
    if (this.verOng) {
      return !!this.linea.lsOng.length;
    } else {
      this.linea.lsOng = [];
      return true;
    }
  }
}
