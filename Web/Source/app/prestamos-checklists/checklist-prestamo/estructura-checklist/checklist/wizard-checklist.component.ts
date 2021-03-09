import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { PrestamoService } from '../../../../shared/servicios/prestamo.service';
import { IntegrantePrestamo } from '../../../../shared/modelo/integrante-prestamo.model';
import { RequisitoPrestamo } from '../../../shared/modelos/requisito-prestamo';
import { Area } from '../../../../areas/shared/modelo/area.model';
import { Etapa } from '../../../../etapas/shared/modelo/etapa.model';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ModalRechazoFormularioComponent } from '../../modal-rechazo-formulario/modal-rechazo-formulario.component';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Prestamo } from '../../../shared/modelos/prestamo.model';
import { SeguimientoPrestamo } from '../../../shared/modelos/seguimiento-prestamo';
import { Observable, Subject, Subscription } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../../../shared/paginacion/pagina-utils';
import { ConsultaSeguimientosPrestamo } from '../../../shared/modelos/consulta-seguimientos';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { ParametroService } from '../../../../soporte/parametro.service';
import { VigenciaParametro } from '../../../../shared/modelo/vigencia-parametro.model';
import { VigenciaParametroConsulta } from '../../../../shared/modelo/consultas/vigencia-parametro-consulta.model';
import { EtapaEstadoLinea } from '../../../shared/modelos/etapa-estado-linea.model';
import { ModalArchivoComponent } from '../../../../shared/modal-archivo/modal-archivo.component';
import { ConfiguracionChecklistService } from '../../../../configuracion-checklist/shared/configuracion-checklist.service';
import { LineaService } from '../../../../shared/servicios/linea-prestamo.service';
import { DetalleLineaPrestamo } from '../../../../lineas/shared/modelo/detalle-linea-prestamo.model';
import { ChecklistAceptarModel } from '../../../shared/modelos/checklist-aceptar.model';
import { DataSharedChecklistService } from '../data-shared-checklist.service';
import { ChecklistEditableModel } from '../../../shared/modelos/checklist-editable.model';
import { AuditoriaService } from '../../../../formularios/shared/auditoria.service';
import { Auditoria } from '../../../../shared/modelo/auditoria.modelo';
import { FormulariosService } from '../../../../formularios/shared/formularios.service';
import EstadosPrestamo from '../../../../formularios/shared/modelo/estado-prestamo.enum';

@Component({
  selector: 'bg-wizard-checklist',
  templateUrl: './wizard-checklist.component.html',
  styleUrls: ['./wizard-checklist.component.scss'],
  providers: [PrestamoService, AuditoriaService]
})
export class WizardChecklistComponent implements OnInit, OnDestroy {
  @Input() public editable: boolean;
  @Input() public valido: boolean;
  @Input() public tieneDeudaHistorica: boolean;
  @Input() public titulo: string;
  @Input() public tipoLinea: number;
  @Input() public idFormularioLinea: number;
  @Input() public cantidadFormularios: number;
  @Input() public integrantesPrestamo: IntegrantePrestamo [];
  @Input() public mensajeAvisoPersonas = '';
  @Output() public rechazoIntegrante = new EventEmitter<ChecklistAceptarModel>();
  @Output() public disminuyeCantidad = new EventEmitter<boolean>();
  @Output() public deshabilitarAceptar = new EventEmitter<ChecklistAceptarModel>();
  @Output() public editableEmitter = new EventEmitter<ChecklistEditableModel>();

  public requisitosPrestamo: RequisitoPrestamo [] = [];
  public requisitosCargados: RequisitoPrestamo [] = [];
  public etapasPrestamo: Etapa [] = [];
  public areasPrestamo: Area [] = [];
  public form: FormGroup;
  public idPrestamo: number;
  public deshabilitar: boolean = true;
  public tieneGrupo: boolean = false;
  public idEtapaVigente: number;
  public prestamo: Prestamo = new Prestamo();
  public seguimientos: SeguimientoPrestamo [] = [];
  private checkListAceptar: ChecklistAceptarModel = new ChecklistAceptarModel();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consultaSeguimientos: ConsultaSeguimientosPrestamo = new ConsultaSeguimientosPrestamo();
  public cantMinIntIndividual: VigenciaParametro;
  public cantMinIntAsociativo: VigenciaParametro;
  public etapasEstadosLinea: EtapaEstadoLinea[] = [];
  public idIntegranteSocio = 0;
  public subscription: Subscription;
  public etapaClick: any;
  public subscriptionEditable: Subscription;
  public subscriptionCantForms: Subscription;
  public subscriptionObservaciones: Subscription;
  public CAMBIO_DE_ETAPA = 'CAMBIO DE ETAPA';
  public estadosPrestamo = EstadosPrestamo;

  constructor(private prestamoService: PrestamoService,
              private configuracionChecklistService: ConfiguracionChecklistService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private notificacionService: NotificacionService,
              private fb: FormBuilder,
              private modalService: NgbModal,
              private parametroService: ParametroService,
              private lineaService: LineaService,
              private dataSharedChecklistService: DataSharedChecklistService,
              private auditoriaService: AuditoriaService,
              private formularioService: FormulariosService) {
  }

  public ngOnDestroy(): void {
    if (this.form && this.router.url.includes('actualizar-checklist') && this.editable) {
      let etapa = (this.form.get('etapas') as FormArray).controls.find((val) =>
        val.get('idEtapa').value == this.idEtapaVigente) as FormGroup;
      this.guardar(etapa, false);
      this.dataSharedChecklistService.modificarObservaciones('');
    }
  }

  public ngOnInit(): void {
    this.subscription = this.dataSharedChecklistService.getSubject().subscribe((deshabilitar: boolean) => {
        this.deshabilitar = deshabilitar;
      }
    );
    this.subscriptionEditable = this.dataSharedChecklistService.getSubjectEditable().subscribe((checklistEditableModel: ChecklistEditableModel []) => {
        if (checklistEditableModel) {
          let checkEditable = checklistEditableModel.find((p) => p.idFormularioLinea == this.idFormularioLinea);
          this.editable = checkEditable ? checkEditable.editable : true;
        }
      }
    );
    this.subscriptionCantForms = this.dataSharedChecklistService.getSubjectCantidadFormularios().subscribe((cantidad: number) => {
        if (cantidad) {
          this.cantidadFormularios = cantidad;
        }
      }
    );
    this.crearForm();
    this.checkListAceptar.idFormularioLinea = this.idFormularioLinea;
    this.formularioService.existeGrupoSolicitante(this.idFormularioLinea).subscribe((data) => {
      this.tieneGrupo = data;
    });
    this.activatedRoute.params.subscribe((params: Params) => {
      this.idPrestamo = params['id'];
      this.configurarPaginacion();
      let promesaDatos = new Promise((resolve) =>
        this.prestamoService.consultarDatosPrestamo(this.idPrestamo)
          .subscribe((datosPrestamo) => {
            this.prestamo = datosPrestamo;
            this.checkListAceptar.idEtapa = this.prestamo.idEtapaEstadoLinea;
            this.obtenerTipoSocioIntegranteLinea();
            // Consulta el array de etapas estados linea con el parametro del id de la linea del préstamo
            this.configuracionChecklistService.consultarEtapasEstadosLinea(this.prestamo.idLinea, this.idPrestamo)
              .subscribe((etapas) => {
                this.etapasEstadosLinea = etapas;
                return resolve();
              });
          }));
      let promesaRequisitos = new Promise((resolve) =>
        this.prestamoService.consultarRequisitos(this.idPrestamo)
          .subscribe((requisitos) => {
            this.requisitosPrestamo = requisitos;
            return resolve();
          }));
      let promesaRequisitosCargados = new Promise((resolve) =>
        this.prestamoService.consultarRequisitosCargados(this.idPrestamo, this.idFormularioLinea)
          .subscribe((requisitos) => {
            this.requisitosCargados = requisitos;
            return resolve();
          }));
      Promise.all([promesaDatos, promesaRequisitos, promesaRequisitosCargados])
        .then(() => {
          this.clasificarRequisitos();
          this.consultarSeguimientos(0);
        }).catch((error) => this.notificacionService.informar([error], true));
    });

    this.parametroService
      .obtenerVigenciaParametro(new VigenciaParametroConsulta(1))
      .subscribe((min) => {
        this.cantMinIntIndividual = min;
        this.parametroService
          .obtenerVigenciaParametro(new VigenciaParametroConsulta(2))
          .subscribe((max) => {
            this.cantMinIntAsociativo = max;
          });
      });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this.consultaSeguimientos.idPrestamo = this.idPrestamo;
        this.consultaSeguimientos.numeroPagina = params.numeroPagina;
        return this.prestamoService.consultarSeguimientoPrestamo(this.consultaSeguimientos);
      }).share();
    (<Observable<SeguimientoPrestamo[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((seguimientos) => {
        this.seguimientos = seguimientos;
      });
  }

  public consultarSeguimientos(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  private crearForm(): void {
    this.form = this.fb.group({
      totalFolios: [this.prestamo.totalFolios, CustomValidators.number],
      observaciones: ['', Validators.maxLength(500)],
      modalRechazo: [''],
      etapas: this.fb.array((this.etapasPrestamo || [])
        .map((etapa) => this.crearEtapasFormGroup(etapa)))
    });
  }

  private crearEtapasFormGroup(etapa: Etapa): FormGroup {
    return new FormGroup({
      idEtapa: new FormControl(etapa.id),
      nombreEtapa: new FormControl(etapa.nombre),
      ck_seleccionarTodos: new FormControl(false),
      colapsable: new FormControl(etapa.colapsar),
      requisitosEtapa: new FormArray((etapa.requisitosEtapa || [])
        .map((requisito) => {
          let reqCargado = this.requisitosCargados.find((req) => req.id == requisito.id);
          if (this.requisitosCargados && reqCargado) {
            requisito.esSolicitante = reqCargado.esSolicitante;
            requisito.esGarante = reqCargado.esGarante;
            requisito.esSolicitanteGarante = reqCargado.esSolicitanteGarante;
            requisito.idEtapa = reqCargado.idEtapa;
            requisito.idPrestamoRequisito = reqCargado.idPrestamoRequisito;
            requisito.esAlerta = reqCargado.esAlerta;
          }
          return this.crearRequisitosFormGroup(requisito);
        }))
    });
  }

  private setIdPrestamoEnUrl(urlRecurso: string, idPrestamoRequisito: number): string {
    if (urlRecurso.includes('condicion-economica') && idPrestamoRequisito) {
      localStorage.setItem('idPrestamoRequisito', idPrestamoRequisito.toString());
    }
    if (urlRecurso.includes('deuda-grupo-conviviente') && idPrestamoRequisito) {
      localStorage.setItem('idPrestamoRequisitoGC', idPrestamoRequisito.toString());
    }
    if (urlRecurso && urlRecurso.indexOf(':id') > 0) {
      urlRecurso = '#' + urlRecurso;
      return urlRecurso.replace(/:id/gi, this.idFormularioLinea.toString());
    }
    return urlRecurso;
  }

  private crearRequisitosFormGroup(requisito: RequisitoPrestamo): FormGroup {
    return new FormGroup({
      id: new FormControl(requisito.id),
      idPrestamo: new FormControl(requisito.idPrestamo),
      nombre: new FormControl(requisito.nombreItem),
      idItem: new FormControl(requisito.idItem),
      idArea: new FormControl(requisito.idArea),
      idEtapa: new FormControl(requisito.idEtapa),
      itemPadre: new FormControl(requisito.itemPadre),
      urlRecurso: new FormControl(this.setIdPrestamoEnUrl(requisito.urlRecurso, requisito.idPrestamoRequisito)),
      ck_garante: new FormControl(requisito.esGarante),
      ck_solicitante: new FormControl(requisito.esSolicitante),
      ck_solicitanteGarante: new FormControl(requisito.esSolicitanteGarante),
      subeArchivo: new FormControl(requisito.subeArchivo),
      generaArchivo: new FormControl(requisito.generaArchivo),
      idTipoDocumentacionCdd: new FormControl(requisito.idTipoDocumentacionCdd),
      idPrestamoRequisito: new FormControl(requisito.idPrestamoRequisito),
      esAlerta: new FormControl(requisito.esAlerta)
    });
  }

  public get etapasFormArray(): FormArray {
    return this.form.get('etapas') as FormArray;
  }

  public clasificarRequisitos(): void {
    this.requisitosPrestamo.forEach((requisito) => requisito.idPrestamo = this.idPrestamo);
    /*obtengo las etapas de los requisitos*/
    let unique = {};
    for (let i in this.requisitosPrestamo) {
      if (typeof (unique[this.requisitosPrestamo[i].idEtapa]) === 'undefined') {
        this.etapasPrestamo.push(new Etapa(this.requisitosPrestamo[i].idEtapa, this.requisitosPrestamo[i].nombreEtapa));
      }
      unique[this.requisitosPrestamo[i].idEtapa] = 0;
    }
    /*obtengo las areas de los requisitos*/
    let unique2 = {};
    for (let i in this.requisitosPrestamo) {
      if (typeof (unique2[this.requisitosPrestamo[i].idArea]) === 'undefined') {
        this.areasPrestamo.push(Area.construirAreaChecklist(
          this.requisitosPrestamo[i].idArea,
          this.requisitosPrestamo[i].nombreArea,
          this.requisitosPrestamo[i].idEtapa
        ));
      }
      unique2[this.requisitosPrestamo[i].idArea] = 0;
    }
    this.obtenerEtapasPrestamo();
    /* Se fija qué etapas mostrar en pantalla según el id de etapa_estado_linea del préstamo*/
    let etapaActual = this.etapasEstadosLinea.find((x) => x.id == this.prestamo.idEtapaEstadoLinea);
    this.idEtapaVigente = etapaActual.idEtapaActual;

    /* Por cada requisito en requisitos cargados debo setearle el ck a etapas prestamo*/
    this.requisitosCargados.forEach((aux) => {
      let indiceEtapa = this.etapasPrestamo.indexOf(this.etapasPrestamo.find((etapa) => etapa.id == aux.idEtapa));
      let requisito = this.etapasPrestamo[indiceEtapa].requisitosEtapa.find((req) => req.id == aux.id);
      let indiceReq = this.etapasPrestamo[indiceEtapa].requisitosEtapa.indexOf(requisito);
      this.etapasPrestamo[indiceEtapa].requisitosEtapa[indiceReq].esSolicitanteGarante = aux.esSolicitanteGarante;
      this.etapasPrestamo[indiceEtapa].requisitosEtapa[indiceReq].esGarante = aux.esGarante;
      this.etapasPrestamo[indiceEtapa].requisitosEtapa[indiceReq].esSolicitante = aux.esSolicitante;
    });

    /* Filtra las etapas del préstamo que debe mostrar nomas*/
    this.etapasPrestamo = this.etapasPrestamo.filter((etapa) => etapa.id <= this.idEtapaVigente);

    this.crearForm();

    let etapa = (this.form.get('etapas') as FormArray).controls.find((val) =>
      val.get('idEtapa').value == this.idEtapaVigente);
    if (etapa) {
      etapa.get('ck_seleccionarTodos').setValue(this.todosSeleccionados(etapa as FormGroup));
    }
  }

  private obtenerEtapasPrestamo(): void {
    this.etapasPrestamo
      .map((etapa) => etapa.requisitosEtapa =
        this.requisitosPrestamo
          .filter((req) => etapa.id == req.idEtapa)
          .map((r) =>
            RequisitoPrestamo.construirRequisitoChecklist(r.id, r.idItem, r.nombreItem, r.itemPadre, r.urlRecurso, r.idArea, this.idPrestamo, r.subeArchivo, r.generaArchivo, r.idTipoDocumentacionCdd, r.idPrestamoRequisito, r.esAlerta)));
  }

  public cancelar() {
    this.router.navigate(['/bandeja-prestamos']);
  }

  public rechazar() {
    if (this.estadoValidoRechazo()){
    let texto = 'Préstamo con cantidad mínima de integrantes. No puede rechazar el/los formulario/s. Para rechazar el/los formularios debe rechazar el Préstamo-Checklist.';
    let integrante = this.integrantesPrestamo.find((p) => p.idFormulario === this.idFormularioLinea);
    if (this.prestamo.idTipoIntegrante === 1 && this.cantidadFormularios <= parseInt(this.cantMinIntIndividual.valor, 10)) {
      this.notificacionService.informar([texto]);
      return;
    } else if (this.cantidadFormularios <= parseInt(this.cantMinIntIndividual.valor, 10)) {
      this.notificacionService.informar([texto]);
      return;
    } else if (integrante && integrante.tipoIntegrante == 2) {
      this.notificacionService.informar(['No se puede rechazar el formulario del apoderado. Rechace el préstamo desde la bandeja']);
      return;
    }
    let options: NgbModalOptions = {
      windowClass: 'modal-l',
      backdrop: 'static'
    };

    const modalRef = this.modalService.open(ModalRechazoFormularioComponent, options);
    this.form.controls['modalRechazo'] = ModalRechazoFormularioComponent.nuevoFormGroup(this.integrantesPrestamo.filter(
      (p) => p.idFormulario === this.idFormularioLinea)
      , this.prestamo.numeroPrestamo);
    modalRef.componentInstance.form = this.form.controls['modalRechazo'];
    modalRef.result.then((res) => {
      if (res) {
        this.rechazoIntegrante.emit(new ChecklistAceptarModel(this.idFormularioLinea, true, this.idEtapaVigente));
        this.editableEmitter.emit(new ChecklistEditableModel(this.idFormularioLinea, false));
        this.disminuyeCantidad.emit(true);
      }
    });
    } else {
      this.notificacionService.informar(['No se puede rechazar el formulario ya que el préstamo se encuentra en una etapa de pago/recupero']);
    }
  }

  private estadoValidoRechazo() :boolean {
    let integrante = this.integrantesPrestamo.find((p) => p.idFormulario === this.idFormularioLinea);

    return (this.prestamo.idEstado == this.estadosPrestamo.CREADO
      || this.prestamo.idEstado == this.estadosPrestamo.COMENZADO
      || this.prestamo.idEstado == this.estadosPrestamo.EVALUACION_TECNICA
      || this.prestamo.idEstado == this.estadosPrestamo.A_PAGAR
      || this.prestamo.idEstado == this.estadosPrestamo.A_PAGAR_ENVIADO_A_SUAF
      || this.prestamo.idEstado == this.estadosPrestamo.A_PAGAR_CON_SUAF
      || this.prestamo.idEstado == this.estadosPrestamo.A_PAGAR_CON_LOTE
      || this.prestamo.idEstado == this.estadosPrestamo.IMPAGO) && (integrante.estadoFormulario == "EN PRÉSTAMO" || integrante.estadoFormulario == "IMPAGO")
  }

  public guardar(etapaForm: FormGroup, notificar: boolean) {
    this.prestamoService.guardarChecklist(this.prepararPrestamo(etapaForm))
      .subscribe(() => {
        if (notificar) {
          this.notificacionService
            .informar(['El checklist se guardó con éxito.']).result
            .then(() => {
              this.consultarSeguimientos(0);
              this.dataSharedChecklistService.modificarObservaciones('');
            });
        }
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  private prepararPrestamo(etapaForm: FormGroup): Prestamo {
    this.prepararRequisitos();
    this.prestamo.id = this.idPrestamo;
    this.prestamo.totalFolios = this.dataSharedChecklistService.obtenerCantFolios() ? this.dataSharedChecklistService.obtenerCantFolios() : 0;
    this.subscriptionObservaciones = this.dataSharedChecklistService.getSubjecObservaciones().subscribe((observaciones: string) => {
        this.prestamo.observaciones = observaciones;
      }
    );
    this.prestamo.requisitos = this.filtrarRequisitosEtapa(etapaForm.get('requisitosEtapa').value);
    this.prestamo.idFormularioLinea = this.idFormularioLinea;
    return this.prestamo;
  }

  public aceptar(etapaForm: FormGroup) {
    if (this.tieneDeudaHistorica && etapaForm.value.idEtapa == 2) {
      let integrantesDeuda = this.integrantesPrestamo.filter((x) => x.tieneDeuda).map((y) => y.apellidoNombre);
      this.notificacionService
        .informar([`Los siguientes integrantes poseen deuda, debe normalizar esta situación para continuar:`].concat(integrantesDeuda));
    } else {
      if (!this.valido && etapaForm.value.idEtapa == 2) {
        if (this.mensajeAvisoPersonas.includes(',')) {
          this.notificacionService.informar([`Los integrantes ${this.mensajeAvisoPersonas} tienen más de un formulario en estado válido, debe normalizar esta situación para continuar.`]);
        } else {
          this.notificacionService.informar([`El integrante ${this.mensajeAvisoPersonas} tiene más de un formulario en estado válido, debe normalizar esta situación para continuar.`]);
        }
      } else {
        this.notificacionService.confirmar('¿Está seguro que desea cambiar de etapa?')
          .result
          .then((res) => {
            if (res) {
              this.prestamoService.aceptarChecklist(this.prepararPrestamo(etapaForm))
                .subscribe(() => {
                  this.notificacionService
                    .informar(['El checklist se aprobó con éxito.'])
                    .result
                    .then(() => {
                      this.reinicializarDatos();
                      let promesaDatos = new Promise((resolve) =>
                        this.prestamoService.consultarDatosPrestamo(this.idPrestamo)
                          .subscribe((datosPrestamo) => {
                            this.prestamo = datosPrestamo;
                            return resolve();
                          }));
                      let promesaRequisitos = new Promise((resolve) =>
                        this.prestamoService.consultarRequisitos(this.idPrestamo)
                          .subscribe((requisitos) => {
                            this.requisitosPrestamo = requisitos;
                            return resolve();
                          }));
                      let promesaRequisitosCargados = new Promise((resolve) =>
                        this.prestamoService.consultarRequisitosCargados(this.idPrestamo, this.idFormularioLinea)
                          .subscribe((requisitos) => {
                            this.requisitosCargados = requisitos;
                            return resolve();
                          }));
                      Promise.all([promesaDatos, promesaRequisitos, promesaRequisitosCargados])
                        .then(() => {
                          this.clasificarRequisitos();
                          this.consultarSeguimientos(0);
                        }).catch((error) => this.notificacionService.informar([error], true));
                    });
                }, (errores) => this.notificacionService.informar(errores, true));
            }
          });
      }
    }
  }

  private reinicializarDatos() {
    this.etapasPrestamo.splice(0, this.etapasPrestamo.length);
    this.areasPrestamo.splice(0, this.areasPrestamo.length);
  }

  public esEtapaVigente(etapa: FormGroup): boolean {
    return etapa.get('idEtapa').value == this.idEtapaVigente;
  }

  private filtrarRequisitosEtapa(requisitos: RequisitoPrestamo[]): RequisitoPrestamo [] {
    return this.requisitosPrestamo.filter((requisito) =>
      requisitos.find((req) => req.idItem == requisito.idItem && !!req.itemPadre));
  }

  /*Retorna true si hay mínimo un requisito seleccionado*/
  public validarSeleccionRequisitos(etapaForm: FormGroup): boolean {
    if (etapaForm) {
      return etapaForm.value.requisitosEtapa
        .filter((requisito) => requisito.ck_solicitante || requisito.ck_garante || requisito.ck_solicitanteGarante).length;
    }
  }

  /*Deshabilita Aceptar si no es editable, el formulario no es válido o no están todos los requisitos seleccionados*/
  public deshabilitarBotonAceptar(etapaForm: FormGroup): boolean {
    let todosSeleccionados = this.todosSeleccionados(etapaForm);
    let deshabilitar = !todosSeleccionados || !this.editable || this.form.invalid || !this.estoyEnEstadoDePaseEtapa() || this.esUltimaEtapaConfiguracion(etapaForm.value.idEtapa);
    this.checkListAceptar.deshabilitar = deshabilitar;
    this.checkListAceptar.idEtapa = etapaForm.value.idEtapa;
    this.deshabilitarAceptar.emit(this.checkListAceptar);
    return deshabilitar;
  }

  /*Deshabilita Guardar si no es editable, el formulario no es válido o no hay observaciones o requisitos que guardar*/
  public deshabilitarBotonGuardar(etapaForm: FormGroup): boolean {
    let unRequisito = this.validarSeleccionRequisitos(etapaForm);
    let hayObservaciones = this.form.get('observaciones').value;
    return (!unRequisito && !hayObservaciones) || !this.editable || this.form.invalid;
  }

  /* Deshabilita Rechazar Formulario si:
   * no es editable
   * es de una línea individual
   * es de una linea asociativa pero la cantidad de integrantes del préstamo es <= a la cantidad de integrantes
   * mínima definida en la configuración de la línea correspondiente.
   * el formulario no es válido o no requisitos que guardar
   */
  public deshabilitarBotonRechazar(etapaForm: FormGroup): boolean {
    let esLineaIndividual: boolean = this.tipoLinea == 1;
    let cumpleMinimo: boolean;
    if (this.cantMinIntIndividual && this.cantMinIntAsociativo && this.cantMinIntIndividual.valor && this.cantMinIntAsociativo.valor) {
      cumpleMinimo = this.tipoLinea == 1 ?
        this.cantidadFormularios > Number.parseInt(this.cantMinIntIndividual.valor) :
        this.cantidadFormularios > Number.parseInt(this.cantMinIntAsociativo.valor);
    }
    return !this.editable || this.form.invalid || esLineaIndividual || !cumpleMinimo;
  }

  private obtenerTipoSocioIntegranteLinea(): void {
    this.lineaService.consultarDetallePorIdLineaSinPaginar(this.prestamo.idLinea).subscribe(
      (res: DetalleLineaPrestamo[]) => {
        this.idIntegranteSocio = res[0].idSocioIntegrante;
      });
  };

  /*Retorna true si todos los requisitos de la etapa estan seleccionados*/
  public todosSeleccionados(etapaForm: FormGroup): boolean {
    let todosSeleccionados;
    if (etapaForm) {
      todosSeleccionados = etapaForm.value.requisitosEtapa
          .filter((requisito) => requisito.ck_solicitante && requisito.ck_garante && this.tipoLinea == 1 ||
            requisito.ck_solicitante && requisito.ck_solicitanteGarante || requisito.ck_solicitante && !requisito.ck_solicitanteGarante && requisito.ck_garante
            || requisito.ck_solicitanteGarante && this.tipoLinea == 1 || requisito.ck_solicitante && this.tipoLinea >= 2).length ===
        (etapaForm.value.requisitosEtapa
          .filter((requisito) => !!requisito.itemPadre).length);
    }
    return todosSeleccionados;
  }

  private prepararRequisitos(): void {
    let formModel = this.form.value;
    formModel.etapas.map((etapa) => {
      let indiceEtapa = this.etapasPrestamo.indexOf(this.etapasPrestamo.find((e) => e.id == etapa.idEtapa));
      etapa.requisitosEtapa
        .map((requisitoEtapa) => {
          let indiceRequisito = this.requisitosPrestamo.indexOf(this.requisitosPrestamo
            .find((req) => req.id == requisitoEtapa.id));
          this.requisitosPrestamo[indiceRequisito].esSolicitante = requisitoEtapa.ck_solicitante;
          this.requisitosPrestamo[indiceRequisito].esGarante = requisitoEtapa.ck_garante;
          this.requisitosPrestamo[indiceRequisito].esSolicitanteGarante = requisitoEtapa.ck_solicitanteGarante;

          let indiceRequisitoEtapa = this.etapasPrestamo[indiceEtapa].requisitosEtapa.indexOf(this.etapasPrestamo[indiceEtapa].requisitosEtapa
            .find((req) => req.idItem == requisitoEtapa.idItem));
          this.etapasPrestamo[indiceEtapa].requisitosEtapa[indiceRequisitoEtapa].esSolicitante = requisitoEtapa.ck_solicitante;
          this.etapasPrestamo[indiceEtapa].requisitosEtapa[indiceRequisitoEtapa].esGarante = requisitoEtapa.ck_garante;
          this.etapasPrestamo[indiceEtapa].requisitosEtapa[indiceRequisitoEtapa].esSolicitanteGarante = requisitoEtapa.ck_solicitanteGarante;
        });
    });
  }

  public seleccionarTodos(etapa: FormGroup): void {
    let check = !etapa.get('ck_seleccionarTodos').value;
    let todos = this.todosSeleccionados(etapa);
    if (!check && !todos) {
      check = !check;
    }
    (etapa.get('requisitosEtapa') as FormArray).controls.forEach((requisito) => {
      if (requisito.get('itemPadre').value) {
        if (!this.prestamo.esSolicGarante && this.tipoLinea == 1) {
          requisito.get('ck_solicitante').setValue(check);
          requisito.get('ck_garante').setValue(check);
        } else if (this.prestamo.esSolicGarante && this.tipoLinea == 1) {
          requisito.get('ck_solicitanteGarante').setValue(check);
        } else {
          requisito.get('ck_solicitante').setValue(check);
        }
      }
    });
  }

  public gestionaArchivos(requisito: RequisitoPrestamo): boolean {
    return requisito.subeArchivo || requisito.generaArchivo;
  }

  public abrirModalArchivo(item: any): void {
    let options: NgbModalOptions = {
      size: 'lg',
      windowClass: 'modal_archivo'
    };

    const modalRef = this.modalService.open(ModalArchivoComponent, options);
    modalRef.componentInstance.item = item;
    modalRef.componentInstance.idFormularioLinea = this.idFormularioLinea;
    modalRef.componentInstance.soloHistorial = !this.editable;
  }

  public estoyEnEstadoDePaseEtapa(): boolean {
    if (!this.etapasEstadosLinea.length || !this.prestamo.idEstado) {
      return false;
    }
    return this.etapasEstadosLinea.some((x) => x.idEstadoActual == this.prestamo.idEstado);
  }

  public esUltimaEtapaConfiguracion(idEtapa: number): boolean {
    if (!this.etapasEstadosLinea.length || !idEtapa) {
      return false;
    }
    return this.etapasEstadosLinea.sort((x) => x.orden)[this.etapasEstadosLinea.length - 1].idEtapaActual == idEtapa;
  }

  public esRechazadoOFinalizado(): boolean {
    return this.prestamo.idEstado == 7 || this.prestamo.idEstado == 3 || this.prestamo.idEstado == 20;
  }

  public generaAlerta(item: FormGroup): string {
    return item.get('esAlerta').value ? 'red' : item.get('urlRecurso').value ? '#0064bd' : 'none';
  }

  public deshabilitarItem(item: FormGroup): boolean {
    let integrante = this.integrantesPrestamo.find((p) => p.idFormulario === this.idFormularioLinea);
    if (integrante.tipoIntegrante == 3) {
      let url = item.get('urlRecurso').value;
      if (url && (url.includes('actualizar-sucursal') || url.includes('actualizar-fecha-pago')
        || url.includes('actualizar-plan-pagos') || url.includes('reporte-providencia'))) {
        return true;
      }
    }
    return false;
  }

  public generarSeguimiento(item: FormGroup): void {
    if (this.tieneGrupo) {
      let auditoria = new Auditoria();
      auditoria.idFormularioLinea = this.idFormularioLinea;
      auditoria.idPrestamoItem = item.get('idItem').value;
      this.auditoriaService.registrarSeguimiento(auditoria).subscribe((data) => {
      });
    }
  }

  public colapsar(etapa: AbstractControl): void {
    for (let e of this.etapasFormArray.controls) {
      if (e.get('idEtapa').value == etapa.get('idEtapa').value) {
        e.get('colapsable').setValue(!e.get('colapsable').value);
      }
    }
  }

  public navegar(urlRecurso): void {
    if (this.habilitarNavegacionItem(urlRecurso)) {
      this.notificacionGrupo();
    } else {
      window.open(urlRecurso, '_blank');
    }
  }

  public habilitarNavegacionItem(urlRecurso): boolean {
    return ((urlRecurso.includes('condicion-economica') || urlRecurso.includes('deuda-grupo-conviviente')
      || urlRecurso.includes('control-sintys') || urlRecurso.includes('situacion-domicilio')) && !this.tieneGrupo);
  }

  public notificacionGrupo(): void {
    this.notificacionService.informar(['La persona seleccionada no tiene grupo conviviente.']);
  }
}
