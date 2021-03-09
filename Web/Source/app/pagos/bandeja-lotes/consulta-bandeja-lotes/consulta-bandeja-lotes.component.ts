import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { BandejaLotesConsulta } from '../../shared/modelo/bandeja-lotes-consulta.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PagosService } from '../../shared/pagos.service';
import { MontoDisponibleService } from '../../monto-disponible/shared/monto-disponible.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { ItemCombo } from '../../../shared/modelo/item-combo.model';
import { LineaCombo } from '../../../formularios/shared/modelo/linea-combo.model';
import { BusquedaPorPersonaComponent } from '../../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { DateUtils } from '../../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'bg-consulta-bandeja-lotes',
    templateUrl: 'consulta-bandeja-lotes.component.html',
    styleUrls: ['consulta-bandeja-lotes.component.html']
  })
export class ConsultaBandejaLotesComponent implements OnInit {
  public consulta: BandejaLotesConsulta;
  public CBMontoDisponible: ItemCombo[] = [];
  public CBLineas: LineaCombo[] = [];
  public form: FormGroup;
  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;

  @Output() public emitirConsulta: EventEmitter<BandejaLotesConsulta> = new EventEmitter<BandejaLotesConsulta>();
  @Output() public totalizador: EventEmitter<number> = new EventEmitter<number>();

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private montoDisponibleService: MontoDisponibleService,
              private notificacionService: NotificacionService,
              private lineasService: LineaService,
              private configFechas: NgbDatepickerConfig) {
  }

  public ngOnInit(): void {
    if (!this.consulta) {
      this.consulta = new BandejaLotesConsulta();
      this.consulta.fechaInicio = new Date(Date.now());
      this.consulta.fechaFin = new Date(Date.now());
      this.crearForm();
    }
    this.cargarCombos();
    this.limitarFechas();
  }

  private limitarFechas() {
    DateUtils.setMaxDateDP(new Date(), this.configFechas);
  }

  private cargarCombos() {
    this.montoDisponibleService.consultarMontosParaCombo().subscribe(
      (res) => {
        this.CBMontoDisponible = res;
      });
    this.lineasService
      .consultarLineasParaCombo()
      .subscribe((lineas) => {
        this.CBLineas = lineas;
        this.crearForm();
      });
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaInicio),
      Validators.compose([CustomValidators.maxDate(new Date())]));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaFin),
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
      fechaInicio: fechaDesdeFc,
      fechaFin: fechaHastaFc,
      nroLoteDesde: new FormControl(this.consulta.nroLoteDesde,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      nroLoteHasta: new FormControl(this.consulta.nroLoteHasta,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      nroPrestamo: new FormControl(this.consulta.nroPrestamo,
        Validators.compose([CustomValidators.number, Validators.maxLength(8)])),
      linea: [this.consulta.idLinea]
    });
  }

  public consultar(esNuevaConsulta: boolean) {
    if (esNuevaConsulta) {
      this.prepararConsulta();
    }
    if ((this.consulta.fechaInicio == null
      || this.consulta.fechaFin == null)) {
      this.notificacionService.informar(['Debe ingresar fecha inicio y fecha fin.']);
    } else {
      if (this.consulta.fechaFin < this.consulta.fechaInicio) {
        this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
      } else {
        if (this.validarNroLote()) {
          PagosService.guardarFiltrosLotes(this.consulta);
          this.emitirConsulta.emit(Object.assign({}, this.consulta));
        }
      }
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;

    this.consulta.fechaInicio = NgbUtils.obtenerDate(formModel.fechaInicio);
    this.consulta.fechaFin = NgbUtils.obtenerDate(formModel.fechaFin);
    this.consulta.nroLoteDesde = formModel.nroLoteDesde;
    this.consulta.nroLoteHasta = formModel.nroLoteHasta;
    this.consulta.nroPrestamo = formModel.nroPrestamo;
    this.consulta.idLinea = formModel.linea === 'null' ? null : formModel.linea;
    let consultaPersona = this.componentePersona.prepararConsulta();
    this.consulta.tipoPersona = consultaPersona.tipoPersona;
    this.consulta.apellido = consultaPersona.apellido;
    this.consulta.nombre = consultaPersona.nombre;
    this.consulta.dni = consultaPersona.dni;
    this.consulta.cuil = consultaPersona.cuil;
    this.consultarTotalizador(this.consulta);
  }

  private validarNroLote(): boolean {
    if (this.consulta.nroLoteDesde || this.consulta.nroLoteHasta) {
      if (!this.consulta.nroLoteDesde) {
        this.notificacionService.informar(['Debe ingresar número de lote desde.']);
        return false;
      }
      if (!this.consulta.nroLoteHasta) {
        this.notificacionService.informar(['Debe ingresar número de lote hasta.']);
        return false;
      }
      if (Number(this.consulta.nroLoteDesde) > Number(this.consulta.nroLoteHasta)) {
        this.notificacionService.informar(['El número de lote desde no puede ser mayor al número de lote hasta.']);
        return false;
      }
    }
    return true;
  }

  public restablecerFiltros(consultaRecuperada: BandejaLotesConsulta): void {
    this.consulta = consultaRecuperada;
    this.crearForm();
    this.consultar(false);
  }

  public validarConsulta(): boolean {
    return this.form.invalid || !this.componentePersona.formValid();
  }

  public consultarTotalizador(filtros: BandejaLotesConsulta) {
    this.totalizador.emit(0);
    this.pagosService
      .consultarTotalizador(filtros)
      .subscribe((num) => this.totalizador.emit(num));
  }
}
