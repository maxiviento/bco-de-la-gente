import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { LineaCombo } from '../../../formularios/shared/modelo/linea-combo.model';
import { BusquedaPorPersonaComponent } from '../../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { DateUtils } from '../../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { BandejaChequeConsulta } from '../../shared/modelo/bandeja-cheque-consulta.model';
import { LoteCombo } from '../../shared/modelo/lote-combo.model';
import { Localidad } from '../../../formularios/shared/modelo/localidad.model';
import { Departamento } from '../../../formularios/shared/modelo/departamento.model';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { LocalidadComboServicio } from '../../../formularios/shared/localidad.service';
import { OrigenPrestamo } from '../../../formularios/shared/modelo/origen-prestamo.model';
import { OrigenService } from '../../../formularios/shared/origen-prestamo.service';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import TiposLote from '../../tipos-lote.enum';

@Component({
    selector: 'bg-consulta-bandeja-cheques',
    templateUrl: 'consulta-bandeja-cheques.component.html',
    styleUrls: ['consulta-bandeja-cheques.component.html']
  })
export class ConsultaBandejaChequeComponent implements OnInit {
  public consulta: BandejaChequeConsulta;
  public cBLineas: LineaCombo[] = [];
  public cBLotes: LoteCombo[] = [];
  public cBOrigen: OrigenPrestamo[] = [];
  public cBDepartamentos: Departamento[] = [];
  public cBLocalidades: Localidad[] = [];
  public form: FormGroup;
  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;

  @Output() public totalizador: EventEmitter<number> = new EventEmitter<number>();
  @Output() public emitirConsulta: EventEmitter<BandejaChequeConsulta> = new EventEmitter<BandejaChequeConsulta>();

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private lineasService: LineaService,
              private configFechas: NgbDatepickerConfig,
              private pagosService: PagosService,
              private formularioService: FormulariosService,
              private localidadesService: LocalidadComboServicio,
              private origenesService: OrigenService,
              ) {
  }

  public ngOnInit(): void {
    if (!this.consulta) {
      this.consulta = new BandejaChequeConsulta();
      this.consulta.fechaDesde = new Date(Date.now());
      this.consulta.fechaHasta = new Date(Date.now());
    }
    this.cargarCombos();
    this.crearForm();
    this.limitarFechas();
  }

  private limitarFechas() {
    DateUtils.setMaxDateDP(new Date(), this.configFechas);
  }

  private cargarCombos() {
    let tipoLote: number = 5;
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => {
        this.cBOrigen = origenes;
      });
    this.lineasService
      .consultarLineasParaCombo()
      .subscribe((lineas) => {
        this.cBLineas = lineas;
      });
    this.pagosService
      .obtenerComboLotes(TiposLote.LOTE_PAGO_CHEQUE)
      .subscribe((lotes) => {
        this.cBLotes = (lotes);
      });
    this.formularioService
      .consultarDepartamentos()
      .subscribe((departamentos) => this.cBDepartamentos = departamentos,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });

  }
  private cargarLocalidades(): void {
    if (this.form.get('departamento').value &&
      this.form.get('departamento').value !== 'null') {
      this.localidadesService.consultarLocalidades(this.form.get('departamento').value)
        .subscribe((localidades) => {
          this.cBLocalidades = localidades;
          if (this.cBLocalidades.length) {
            (this.form.get('localidad') as FormControl).enable();
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      this.cBLocalidades = [];
      (this.form.get('localidad') as FormControl).disable();
    }
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
      nroPrestamo: new FormControl(this.consulta.numeroPrestamo,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      nroFormulario: new FormControl(this.consulta.numeroFormulario,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      linea: [this.consulta.idLinea],
      lote: [this.consulta.idLote],
      departamento: [this.consulta.idDepartamento],
      localidad: [this.consulta.idLocalidad],
      origen: [this.consulta.idOrigen],
      cuil: [this.consulta.cuil, Validators.compose([Validators.maxLength(11), CustomValidators.number])],
      dni: [this.consulta.dni, Validators.compose([Validators.maxLength(8), CustomValidators.number])],
    });
    (this.form.get('departamento') as FormControl).valueChanges
      .subscribe(() => {
        this.cargarLocalidades();
        (this.form.get('localidad') as FormControl).setValue(null);
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });

  }

  public consultar(esNuevaConsulta: boolean) {
    if (esNuevaConsulta) {
      this.prepararConsulta();
    }
    if ((this.consulta.fechaDesde == null
      || this.consulta.fechaHasta == null)) {
      this.notificacionService.informar(['Debe ingresar fecha inicio y fecha fin.']);
    } else {
      if (this.consulta.fechaHasta < this.consulta.fechaDesde) {
        this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
      } else {
          PagosService.guardarFiltrosCheque(this.consulta);
          this.emitirConsulta.emit(Object.assign({}, this.consulta));
      }
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;

    this.consulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
    this.consulta.numeroPrestamo = formModel.nroPrestamo;
    this.consulta.numeroFormulario = formModel.nroFormulario;
    this.consulta.idLinea = formModel.linea === 'null' ? null : formModel.linea;
    this.consulta.dni = formModel.dni;
    this.consulta.cuil = formModel.cuil;
    this.consulta.idDepartamento = formModel.idDepartamento === 'null' ? null : formModel.departamento;
    this.consulta.idLocalidad = formModel.idLocalidad === 'null' ? null : formModel.localidad;
    this.consulta.idLote = formModel.idLote === 'null' ? null : formModel.lote;
    this.consulta.idOrigen = formModel.idOrigen === 'null' ? null : formModel.origen;
    this.consultarTotalizador(this.consulta);
  }

  public restablecerFiltros(consultaRecuperada: BandejaChequeConsulta): void {
    this.consulta = consultaRecuperada;
    this.crearForm();
    this.consultar(false);
  }

  public validarConsulta(): boolean {
    return this.form.invalid;
  }

  public consultarTotalizador(filtros: BandejaChequeConsulta) {
    this.totalizador.emit(0);
    this.pagosService
      .consultarTotalizadorCheque(filtros)
      .subscribe((num) => this.totalizador.emit(num));
  }

}
