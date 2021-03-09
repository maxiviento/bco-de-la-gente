import {Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {PagosService} from '../../shared/pagos.service';
import {NotificacionService} from '../../../shared/notificacion.service';
import {LineaService} from '../../../shared/servicios/linea-prestamo.service';
import {CustomValidators} from '../../../shared/forms/custom-validators';
import {LineaCombo} from '../../../formularios/shared/modelo/linea-combo.model';
import {NgbDatepickerConfig, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {FormulariosService} from '../../../formularios/shared/formularios.service';
import {Departamento} from '../../../formularios/shared/modelo/departamento.model';
import {Localidad} from '../../../formularios/shared/modelo/localidad.model';
import {LocalidadComboServicio} from '../../../formularios/shared/localidad.service';
import {ActivatedRoute, Params, Router} from "@angular/router";
import {OrigenPrestamo} from "../../../formularios/shared/modelo/origen-prestamo.model";
import {Observable, Subject} from "rxjs";
import {ELEMENTOS, Pagina} from "../../../shared/paginacion/pagina-utils";
import {MontoDisponible} from "../../monto-disponible/shared/modelo/monto-disponible.model";
import {BusquedaPorPersonaComponent} from "../../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component";
import {OrigenService} from "../../../formularios/shared/origen-prestamo.service";
import {BandejaAdendaConsulta} from "../../shared/modelo/bandeja-adenda-consulta.model";
import {BandejaAdendaResultado} from "../../shared/modelo/bandeja-adenda-resultado.model";
import {DetallesAdenda} from "../../shared/modelo/detalles-adenda.model";
import {FormulariosSeleccionadosAdendaConsulta} from "../../shared/modelo/formularios-seleccionados-adenda-consulta.model";
import {FormulariosSeleccionadosAdendaResultado} from "../../shared/modelo/formularios-seleccionados-adenda-resultado.model";
import {ModalAdendaComponent} from "../../bandeja-lotes/modal-adenda/modal-adenda.component";

@Component({
  selector: 'bg-consulta-crear-adenda',
  templateUrl: 'consulta-crear-adenda.component.html',
  styleUrls: ['consulta-crear-adenda.component.scss']
})
export class ConsultaCrearAdendaComponent implements OnInit {

  public consulta: BandejaAdendaConsulta = new BandejaAdendaConsulta();
  public consultaSeleccionados: FormulariosSeleccionadosAdendaConsulta = new FormulariosSeleccionadosAdendaConsulta();
  public bandejaResultados: BandejaAdendaResultado[] = [];
  public formConsulta: FormGroup;
  public formResultados: FormGroup;
  public formSeleccionados: FormGroup;
  public departamentos: Departamento[] = [];
  public CBLocalidad: Localidad[] = [];
  public CBOrigen: OrigenPrestamo[] = [];
  public CBLineas: LineaCombo[] = [];
  public lineas: LineaCombo[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaSeleccionados: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public paginaModificadaSeleccionados = new Subject<number>();
  public existeNroFormulario: boolean = false;
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public idLote: number;
  public montoDisponible: MontoDisponible = new MontoDisponible();
  public todosSeleccionados: boolean = false;
  public detalleParaModificar: DetallesAdenda;
  public nroDetalle: number = -1;
  public bandejaSeleccionados: FormulariosSeleccionadosAdendaResultado[] = [];
  private adendaConfirmada: boolean = false;
  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;
  @Output() public emitirNroDetalle: EventEmitter<number> = new EventEmitter<number>();

  constructor(private fb: FormBuilder,
              private formulariosService: FormulariosService,
              private lineasService: LineaService,
              private pagosService: PagosService,
              private localidadesService: LocalidadComboServicio,
              private origenesService: OrigenService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private router: Router,
              private modalService: NgbModal,
              private route: ActivatedRoute) {
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
    this.crearFormConsulta();
    this.cargarCombos();
    this.prepararConsulta();
    this.configurarPaginacionConsulta();
    this.consultar();
  }

  private cargarCombos() {
    this.formulariosService
      .consultarDepartamentos()
      .subscribe((departamentos) => this.departamentos = departamentos,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => this.CBOrigen = origenes,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.lineasService
      .consultarLineasParaCombo(true)
      .subscribe((lineas) => {
        this.CBLineas = lineas;
        this.lineas = lineas;
        this.crearFormConsulta();
      });
  }

  private crearFormConsulta() {
    let nroFormularioFc = new FormControl(this.consulta.nroFormulario,
      Validators.compose([Validators.maxLength(14), CustomValidators.number]));
    nroFormularioFc.valueChanges.subscribe((value) => {
      this.existeNroFormulario = !!value;
    });

    this.formConsulta = this.fb.group({
      nroPrestamoChecklist: [this.consulta.nroPrestamoChecklist, Validators.compose([
        CustomValidators.number,
        Validators.maxLength(8)
      ])],
      nroFormulario: nroFormularioFc,
      origen: [this.consulta.idOrigen],
      lineas: [this.consulta.idsLineas],
      departamento: [this.consulta.departamentoIds],
      localidad: [this.consulta.localidadIds]
    });
    (this.formConsulta.get('departamento') as FormControl).valueChanges
      .subscribe(() => {
        this.cargarLocalidades();
        (this.formConsulta.get('localidad') as FormControl).setValue(null);
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
    this.subscripcionLineas();
  }

  private subscripcionLineas(): void {
    let linea = this.formConsulta.get('lineas') as FormControl;
    linea.valueChanges
      .distinctUntilChanged()
      .subscribe(() => {
        if (linea.enabled) {
          if (linea.value && linea.value.length) {
            this.CBLineas = this.lineas;
          }
        }
      });
  }

  private cargarLocalidades(): void {
    if (this.formConsulta.get('departamento').value &&
      this.formConsulta.get('departamento').value !== 'null') {
      this.localidadesService.consultarLocalidades(this.formConsulta.get('departamento').value)
        .subscribe((localidades) => {
          this.CBLocalidad = localidades;
          if (this.CBLocalidad.length) {
            (this.formConsulta.get('localidad') as FormControl).enable();
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      this.CBLocalidad = [];
      (this.formConsulta.get('localidad') as FormControl).disable();
    }
  }

  public consultar() {
    this.prepararConsulta();
    this.paginaModificada.next(0);
    this.paginaModificada.next(0);
  }

  public crearAdenda() {
    const modalAdenda = this.modalService.open(ModalAdendaComponent,
      {backdrop: 'static', windowClass: 'modal-l'});
    modalAdenda.componentInstance.nroDetalle = this.nroDetalle;
    modalAdenda.result.then((res) => {
      if (res) {
        this.adendaConfirmada = res;
      }
    });
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades;
  }

  private prepararConsulta() {
    let formModel = this.formConsulta.value;
    this.consulta.idLote = this.idLote;
    this.consulta.nroPrestamoChecklist = formModel.nroPrestamoChecklist;
    this.consulta.nroFormulario = formModel.nroFormulario;
    this.consulta.idOrigen = formModel.origen;
    this.consulta.idsLineas = formModel.lineas;
    this.consulta.nroDetalle = this.nroDetalle;
    if (this.componentePersona.form) {
      let consultaPersona = this.componentePersona.prepararConsulta();
      this.consulta.tipoPersona = consultaPersona.tipoPersona;
      this.consulta.cuil = consultaPersona.cuil;
      this.consulta.nombre = consultaPersona.nombre;
      this.consulta.apellido = consultaPersona.apellido;
      this.consulta.dni = consultaPersona.dni;
    }
    this.consulta.seleccionarTodos = false;
    this.consulta.departamentoIds = this.departamentoIds;
    this.consulta.localidadIds = this.localidadIds;
  }

  public get resultadoFormArray(): FormArray {
    return this.formResultados.get('resultados') as FormArray;
  }

  public get seleccionadosFormArray(): FormArray {
    return this.formSeleccionados.get('seleccionados') as FormArray;
  }

  private consultarPaginaActualConsulta(consultaTodos: boolean){
    if (consultaTodos){
      this.paginaModificada.next(0);
    } else {
      this.paginaModificada.next(this.consulta.numeroPagina);
    }
  }

  private configurarPaginacionConsulta() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.consulta.nroDetalle = this.nroDetalle;
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService
          .consultarPrestamosParaAdenda(filtros);
      })
      .share();
    (<Observable<BandejaAdendaResultado[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormResultados();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  private seleccionarTodos() {
    this.todosSeleccionados = !this.todosSeleccionados;
    this.consulta.seleccionarTodos = this.todosSeleccionados;
    this.consulta.nroDetalle = this.nroDetalle;
    this.pagosService.seleccionarTodosParaAdenda(this.consulta).subscribe((nroDetalle) => {
      this.nroDetalle = nroDetalle;
      this.modificarAgregadosHermanosSeleccionarTodos(this.todosSeleccionados);
      this.consultarPaginaActualConsulta(true);
      this.mostrarFormulariosSeleccionados();
    });
  }

  private agregarPrestamo(nroPrestamo: number) {
    this.prepararModificacionDetalle(nroPrestamo, true);
    this.pagosService.modificarPrestamoAdenda(this.detalleParaModificar).subscribe((nroDetalle) => {
      this.nroDetalle = nroDetalle;
      this.modificarAgregadosHermanosPrestamo(nroPrestamo, true);
      this.consultarPaginaActualConsulta(false);
      this.mostrarFormulariosSeleccionados();
    });
  }

  private quitarPrestamo(nroPrestamo: number) {
    this.prepararModificacionDetalle(nroPrestamo, false);
    this.pagosService.modificarPrestamoAdenda(this.detalleParaModificar).subscribe((nroDetalle) => {
      this.nroDetalle = nroDetalle;
      this.todosSeleccionados = false;
      this.modificarAgregadosHermanosPrestamo(nroPrestamo, false);
      this.consultarPaginaActualConsulta(false);
      this.mostrarFormulariosSeleccionados();
    });
  }

  private modificarAgregadosHermanosSeleccionarTodos(agregado: boolean) {
    let prestamosResultados = (this.formResultados.get('resultados') as FormArray).controls;
    prestamosResultados.forEach((prestamo) => {
      prestamo.get('agregado').setValue(agregado);
    })
  }


  private modificarAgregadosHermanosPrestamo(nroPrestamo: number, agregado: boolean) {
    let prestamosResultados = (this.formResultados.get('resultados') as FormArray).controls;
    prestamosResultados.forEach((prestamo) => {
      if (prestamo.get('nroPrestamo').value == nroPrestamo) {
        prestamo.get('agregado').setValue(agregado);
      }
    })
  }

  private mostrarFormulariosSeleccionados() {
    this.crearFormSeleccionados();
    this.configurarPaginacionConsultaFormulariosSeleccionados();
    this.paginaModificadaSeleccionados.next(0);
    this.paginaModificadaSeleccionados.next(0);
  }

  private prepararModificacionDetalle(nroPrestamo: number, agrega: boolean) {
    this.detalleParaModificar = new DetallesAdenda();
    this.detalleParaModificar.idLote = this.idLote;
    this.detalleParaModificar.nroDetalle = this.nroDetalle;
    this.detalleParaModificar.nroPrestamoChecklist = nroPrestamo;
    this.detalleParaModificar.agrega = agrega;
  }

  public consultarFormularios(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public consultarSiguientesSeleccionados(pagina?: number): void {
    this.paginaModificadaSeleccionados.next(pagina);
  }

  private crearFormResultados() {
    this.formResultados = this.fb.group({
      resultados: this.fb.array((this.bandejaResultados || []).map((resultado) =>
        this.fb.group({
          nroFormulario: [resultado.nroFormulario],
          linea: [resultado.linea],
          nombreYApellido: [resultado.apellidoYNombre],
          nroPrestamo: [resultado.nroPrestamo],
          localidad: [resultado.localidad],
          departamento: [resultado.departamento],
          montoOtorgado: [resultado.montoOtorgado],
          agregado: [resultado.agregado]
        })
      )),
    });
  }

  private crearFormSeleccionados() {
    this.formSeleccionados = this.fb.group({
      seleccionados: this.fb.array((this.bandejaSeleccionados || []).map((resultado) =>
        this.fb.group({
          nroFormulario: [resultado.nroFormulario],
          linea: [resultado.linea],
          nombreYApellido: [resultado.apellido + ", " + resultado.nombre],
          nroDocumento: [resultado.nroDocumento],
          nroPrestamo: [resultado.nroPrestamo],
          localidad: [resultado.localidad],
          departamento: [resultado.departamento],
          montoPrestamo: [resultado.montoPrestamo],
          cuil: [resultado.cuil],
          estadoFormulario: [resultado.estadoFormulario],
          estadoPrestamo: [resultado.estadoPrestamo]
        })
      )),
    });
  }

  private configurarPaginacionConsultaFormulariosSeleccionados() {
    this.paginaSeleccionados = this.paginaModificadaSeleccionados
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this.consultaSeleccionados;
        filtros.nroDetalle = this.nroDetalle;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService
          .obtenerFormulariosSeleccionados(filtros);
      })
      .share();
    (<Observable<FormulariosSeleccionadosAdendaResultado[]>>this.paginaSeleccionados.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaSeleccionados = resultado;
        this.crearFormSeleccionados();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }
}
