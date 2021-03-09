import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BandejaCambioEstadoConsulta } from '../../shared/modelo/bandeja-cambio-estado-consulta.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PagosService } from '../../shared/pagos.service';
import { MontoDisponibleService } from '../../monto-disponible/shared/monto-disponible.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { LineaCombo } from '../../../formularios/shared/modelo/linea-combo.model';
import { DateUtils } from '../../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { EstadoFormulario } from '../../../formularios/shared/modelo/estado-formulario.model';
import { EstadoFormularioService } from '../../../formularios/shared/estado-formulario.service';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { Departamento } from '../../../formularios/shared/modelo/departamento.model';
import { Localidad } from '../../../formularios/shared/modelo/localidad.model';
import { LocalidadComboServicio } from '../../../formularios/shared/localidad.service';
import { LoteCombo } from '../../shared/modelo/lote-combo.model';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';

@Component({
  selector: 'bg-consulta-bandeja-cambio-estado',
  templateUrl: 'consulta-bandeja-cambio-estado.component.html',
  styleUrls: ['consulta-bandeja-cambio-estado.component.scss']
})
export class ConsultaBandejaCambioEstadoComponent implements OnInit {
  public consulta: BandejaCambioEstadoConsulta;
  public CBLineas: LineaCombo[] = [];
  public comboEstados: EstadoFormulario[] = [];
  public comboElementosPago: LoteCombo[] = [];
  public departamentos: Departamento[] = [];
  public localidades: Localidad[] = [];
  public form: FormGroup;

  @Output() public totalizador: EventEmitter<number> = new EventEmitter<number>();
  @Output() public emitirConsulta: EventEmitter<BandejaCambioEstadoConsulta> = new EventEmitter<BandejaCambioEstadoConsulta>();

  constructor(private fb: FormBuilder,
              private montoDisponibleService: MontoDisponibleService,
              private notificacionService: NotificacionService,
              private lineasService: LineaService,
              private estadoFormularioService: EstadoFormularioService,
              private formularioService: FormulariosService,
              private localidadesService: LocalidadComboServicio,
              private pagosService: PagosService,
              private configFechas: NgbDatepickerConfig) {
  }

  public ngOnInit(): void {
    if (!this.consulta) {
      this.consulta = new BandejaCambioEstadoConsulta();
      this.consulta.fechaDesde = new Date(Date.now());
      this.consulta.fechaHasta = new Date(Date.now());
      this.crearForm();
    }
    this.cargarCombos();
    this.limitarFechas();
  }

  private limitarFechas() {
    DateUtils.setMaxDateDP(new Date(), this.configFechas);
  }

  private cargarCombos() {
    this.lineasService
      .consultarLineasParaCombo()
      .subscribe((lineas) => {
        this.CBLineas = lineas;
        this.crearForm();
      });
    this.estadoFormularioService
      .consultarEstadosFiltroComboCambioEstado()
      .subscribe((estados) => {
        this.comboEstados = estados;
        this.crearForm();
      });
    this.formularioService
      .consultarDepartamentos()
      .subscribe((departamentos) => this.departamentos = departamentos,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.pagosService
      .obtenerElementosPago()
      .subscribe((elementosPago) => this.comboElementosPago = elementosPago,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaDesde),
      Validators.compose([CustomValidators.maxDate(new Date())]));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaHasta),
      Validators.compose([CustomValidators.maxDate(new Date())]));

    fechaDesdeFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaHastaFc.clearValidators();
        let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec ? minDate = new Date(fechaDesdeMilisec) : minDate = new Date(fechaActualMilisec);
        fechaHastaFc.setValidators(Validators.compose([CustomValidators.minDate(minDate), Validators.required,
          CustomValidators.maxDate(new Date())]));
        fechaHastaFc.updateValueAndValidity();
      }
    });
    fechaHastaFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaDesdeFc.clearValidators();
        let fechaHastaMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let maxDate;
        fechaHastaMilisec <= fechaActualMilisec ? maxDate = new Date(fechaHastaMilisec) : maxDate = new Date(fechaActualMilisec);
        fechaDesdeFc.setValidators(Validators.compose([CustomValidators.maxDate(maxDate), Validators.required]));
        fechaDesdeFc.updateValueAndValidity();
      }
    });

    this.form = this.fb.group({
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      nroPrestamo: new FormControl(this.consulta.nroPrestamo,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      nroFormulario: new FormControl(this.consulta.nroFormulario,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      linea: [this.consulta.idLinea],
      elementoPago: [this.consulta.idElementoPago],
      departamento: [this.consulta.idDepartamento],
      localidad: [this.consulta.idLocalidad],
      nroSticker: [this.consulta.nroSticker, Validators.compose([Validators.maxLength(14), CustomValidators.number])],
      estadoFormulario: [this.consulta.idEstadoFormulario],
      cuil: [this.consulta.cuil, Validators.compose([Validators.maxLength(11), CustomValidators.number])],
      dni: [this.consulta.dni, Validators.compose([Validators.maxLength(8), CustomValidators.number])]
    });

    (this.form.get('departamento') as FormControl).valueChanges
      .subscribe(() => {
        this.cargarLocalidades();
        (this.form.get('localidad') as FormControl).setValue(null);
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public consultar() {
    this.prepararConsulta();
    if ((this.consulta.fechaDesde == null
      || this.consulta.fechaHasta == null)) {
      this.notificacionService.informar(['Debe ingresar fecha inicio y fecha fin.']);
    } else {
      if (this.consulta.fechaHasta < this.consulta.fechaDesde) {
        this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
      } else {
        this.emitirConsulta.emit(Object.assign({}, this.consulta));
      }
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;
    this.consulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
    this.consulta.idLinea = formModel.linea === 'null' ? null : formModel.linea;
    this.consulta.nroPrestamo = formModel.nroPrestamo === 'null' ? null : formModel.nroPrestamo;
    this.consulta.nroFormulario = formModel.nroFormulario === 'null' ? null : formModel.nroFormulario;
    this.consulta.idEstadoFormulario = formModel.estadoFormulario === 'null' ? null : formModel.estadoFormulario;
    this.consulta.idElementoPago = formModel.elementoPago === 'null' ? null : formModel.elementoPago;
    this.consulta.nroSticker = formModel.nroSticker === 'null' ? null : formModel.nroSticker;
    this.consulta.cuil = formModel.cuil === 'null' ? null : formModel.cuil;
    this.consulta.dni = formModel.dni === 'null' ? null : formModel.dni;
    this.consulta.idDepartamento = formModel.departamento === 'null' ? null : formModel.departamento;
    this.consulta.idLocalidad = formModel.localidad === 'null' ? null : formModel.localidad;
    this.consultarTotalizador(this.consulta);
  }

  public validarConsulta(): boolean {
    return this.form.invalid;
  }

  private cargarLocalidades(): void {
    if (this.form.get('departamento').value &&
      this.form.get('departamento').value !== 'null') {
      this.localidadesService.consultarLocalidades(this.form.get('departamento').value)
        .subscribe((localidades) => {
          this.localidades = localidades;
          if (this.localidades.length) {
            (this.form.get('localidad') as FormControl).enable();
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      this.localidades = [];
      (this.form.get('localidad') as FormControl).disable();
    }
  }

  public consultarTotalizador(consulta: BandejaCambioEstadoConsulta) {
    this.totalizador.emit(0);
    this.pagosService
      .consultarTotalizadorCambioEstado(consulta)
      .subscribe((num) => this.totalizador.emit(num));
  }
}
