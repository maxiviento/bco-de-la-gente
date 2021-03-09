import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PagosService } from '../../../shared/pagos.service';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { NgbUtils } from '../../../../shared/ngb/ngb-utils';
import { BandejaSuafConsulta } from '../../../shared/modelo/bandeja-suaf-consulta.model';
import { LoteCombo } from '../../../shared/modelo/lote-combo.model';
import { DateUtils } from '../../../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

@Component( {
  selector: 'bg-consulta-lote-suaf',
  templateUrl: 'consulta-lote-suaf.component.html',
  styleUrls: ['consulta-lote-suaf.component.scss']
})
export class ConsultaLoteSuafComponent implements OnInit{

  public consulta: BandejaSuafConsulta;
  public CBLote: LoteCombo[];
  public form: FormGroup;
  public reporteSource: any;
  @Output() public emitirConsulta: EventEmitter<BandejaSuafConsulta> = new EventEmitter<BandejaSuafConsulta>();
  @Output() public totalizador: EventEmitter<number> = new EventEmitter<number>();

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private configFechas: NgbDatepickerConfig) {
  }

  public ngOnInit(): void {
    if (!this.consulta) {
      this.consulta = new BandejaSuafConsulta();
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
    this.pagosService.obtenerComboLotesSuaf()
      .subscribe((lotes) => {
        this.CBLote = lotes;
      });
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaDesde),
      CustomValidators.maxDate(new Date()));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaHasta),
      CustomValidators.minDate(new Date()));
    fechaDesdeFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaHastaFc.clearValidators();
        let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec ? minDate = new Date(fechaDesdeMilisec) : minDate = new Date(fechaActualMilisec);
        fechaHastaFc.setValidators(Validators.compose([CustomValidators.minDate(minDate),
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
        fechaDesdeFc.setValidators(CustomValidators.maxDate(maxDate));
        fechaDesdeFc.updateValueAndValidity();
      }
    });

    this.form = this.fb.group({
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      idLote: [this.consulta.idLote]
    });
  }

  public consultar() {
    this.prepararConsulta();
    if ((this.consulta.fechaDesde == null
      || this.consulta.fechaHasta == null)) {
      this.notificacionService.informar(['Debe ingresar fecha desde y fecha hasta.']);
    } else {
      if (this.consulta.fechaHasta < this.consulta.fechaDesde) {
        this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
      } else {
        PagosService.guardarFiltrosSuaf(this.consulta);
        this.emitirConsulta.emit(Object.assign({}, this.consulta));
      }
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;
    let nuevaConsulta = new BandejaSuafConsulta(
      NgbUtils.obtenerDate(formModel.fechaDesde),
      NgbUtils.obtenerDate(formModel.fechaHasta),
      formModel.idLote === 'null' ? null : formModel.idLote);

    if (!((this.consulta.fechaDesde === nuevaConsulta.fechaDesde) &&
      (this.consulta.fechaHasta === nuevaConsulta.fechaHasta) &&
      (this.consulta.idLote === nuevaConsulta.idLote))) {
      this.consulta.numeroPagina = 0;
    }
    this.consulta.fechaDesde = nuevaConsulta.fechaDesde;
    this.consulta.fechaHasta = nuevaConsulta.fechaHasta;
    this.consulta.idLote = nuevaConsulta.idLote;
    this.consultarTotalizador(this.consulta);
  }

  public restablecerFiltros(consultaRecuperada: BandejaSuafConsulta): void {
    this.consulta = consultaRecuperada;
    this.crearForm();
    this.consultar();
  }

  public consultarTotalizador(filtros: BandejaSuafConsulta) {
    this.totalizador.emit(0);
    this.pagosService
      .consultarTotalizadorSuaf(filtros)
      .subscribe((num) => this.totalizador.emit(num));
  }
}
