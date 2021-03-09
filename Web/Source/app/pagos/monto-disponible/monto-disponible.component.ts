import { MontoDisponibleService } from './shared/monto-disponible.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { BandejaMontoDisponibleConsulta } from './shared/modelo/consulta-bandeja-monto-disponible.model';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { Observable } from 'rxjs/Observable';
import { BandejaMontoDisponibleResultado } from './shared/modelo/resultado-bandeja-monto-disponible.model';
import { Subject } from 'rxjs/Subject';
import { NotificacionService } from '../../shared/notificacion.service';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-monto-disponible',
  templateUrl: './monto-disponible.component.html',
  styleUrls: ['./monto-disponible.component.scss'],
  providers: [MontoDisponibleService]
})

export class MontoDisponibleComponent implements OnInit {
  public form: FormGroup;
  public consulta: BandejaMontoDisponibleConsulta;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public bandejaResultados: BandejaMontoDisponibleResultado[] = [];

  constructor(private fb: FormBuilder,
              private montoDisponibleService: MontoDisponibleService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private titleService: Title) {
    this.titleService.setTitle('Consultar monto disponible ' + TituloBanco.TITULO);
    if (!this.consulta) {
      this.consulta = new BandejaMontoDisponibleConsulta();
    }
  }

  public ngOnInit(): void {
    this.consulta.fechaDesde = new Date(Date.now());
    this.consulta.fechaHasta = new Date(Date.now());
    this.crearForm();
    this.configurarPaginacion();
    // this.limiteFechaMaxima();
    this.reestablecerFiltros();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      fechaDesde: [NgbUtils.obtenerNgbDateStruct(this.consulta.fechaDesde)],
      fechaHasta: [NgbUtils.obtenerNgbDateStruct(this.consulta.fechaHasta)],
      nroMonto: new FormControl(this.consulta.nroMonto,
        Validators.compose([
          CustomValidators.number,
          Validators.maxLength(8)
        ])),
      incluirBaja: this.consulta.incluirBaja
    });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsulta();
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.montoDisponibleService
          .consultarBandeja(filtros);
      })
      .share();
    (<Observable<BandejaMontoDisponibleResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((montosDisponible) => {
        this.bandejaResultados = montosDisponible;
      });
  }

  private limiteFechaMaxima() {
    let hoy = new Date(Date.now());
    let elMesQueViene = hoy.getMonth() + 1;

    let jan312009 = new Date(Date.now());


    jan312009.setMonth(jan312009.getMonth() + 1);

    this.config.maxDate = {
      year: new Date(Date.now()).getFullYear(),
      month: jan312009.getMonth(),
      day: new Date(Date.now()).getDate(),
    };
  }

  private prepararConsulta(): void {
    let formValue = this.form.value;

    this.consulta.fechaDesde = NgbUtils.obtenerDate(formValue.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formValue.fechaHasta);
    this.consulta.nroMonto = formValue.nroMonto;
    this.consulta.incluirBaja = formValue.incluirBaja;
  }

  public consultar(pagina?: number) {
    this.prepararConsulta();
    if (this.consulta.nroMonto == null || this.consulta.nroMonto === 0) {
      if ((this.consulta.fechaDesde == null
        || this.consulta.fechaHasta == null)) {
        this.notificacionService.informar(['Debe ingresar fecha desde y fecha hasta.']);
      } else {
        if (this.consulta.fechaHasta < this.consulta.fechaDesde) {
          this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
        } else {
          MontoDisponibleService.guardarFiltros(this.consulta);
          this.paginaModificada.next(pagina);
        }
      }
    } else {
      MontoDisponibleService.guardarFiltros(this.consulta);
      this.paginaModificada.next(pagina);
    }
  }

  private reestablecerFiltros() {
    let filtrosGuardados = MontoDisponibleService.recuperarFiltros();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.consulta.fechaDesde = this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde;
      this.consulta.fechaHasta = this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta;
      this.crearForm();
      this.consultar();
    }
  }
}
