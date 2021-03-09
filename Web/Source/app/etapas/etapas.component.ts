import { Component, OnDestroy, OnInit } from '@angular/core';
import { EtapasService } from './shared/etapas.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Etapa } from './shared/modelo/etapa.model';
import { ConsultaEtapa } from './shared/modelo/consulta-etapa.model';
import { NotificacionService } from '../shared/notificacion.service';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { Router } from '@angular/router';
import { CustomValidators } from '../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-etapas',
  templateUrl: './etapas.component.html',
  styleUrls: ['./etapas.component.scss']
})

export class EtapasComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public etapas: Etapa[] = [];
  public consulta: ConsultaEtapa = new ConsultaEtapa();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public nombreEtapas: string[] = [];

  constructor(private fb: FormBuilder,
              private etapasService: EtapasService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Etapas ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.obtenerNombreEtapas();
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('etapa')) {
      EtapasService.guardarFiltros(null);
    }
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsultaEtapa();
        this.consulta.numeroPagina = params.numeroPagina;
        return this.etapasService
          .consultarEtapas(this.consulta);
      })
      .share();
    (<Observable<Etapa[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((etapas) => {
        this.etapas = etapas;
        if (this.form.valid && this.etapas.length > 0) {
          EtapasService.guardarFiltros(this.consulta);
        }
      });
  }

  private obtenerNombreEtapas(): void {
    this.etapasService.consultarEtapasCombo().subscribe((etapas) => {
      etapas.forEach((x) => this.nombreEtapas.push(x.descripcion));
    });
  }

  public searchEtapa = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .distinctUntilChanged()
      .map((term) => term.length < 2 ? []
        : this.nombreEtapas.filter((etapa) =>
          etapa.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));
  public showEtapa = (etapa: any) => etapa;

  private crearForm(): void {
    this.form = this.fb.group({
      nombre: ['', Validators.compose([Validators.maxLength(100), CustomValidators.validTextAndNumbers])],
      incluirDadosDeBaja: false
    });
  }

  private prepararConsultaEtapa(): void {
    let formModel = this.form.value;
    this.consulta.nombre = formModel.nombre;
    this.consulta.incluirDadosDeBaja = formModel.incluirDadosDeBaja;
  }

  public consultarEtapas(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private reestablecerFiltros() {
    let filtrosGuardados = EtapasService.recuperarFiltros();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.form.patchValue(this.consulta);
      this.consultarEtapas();
    }
  }
}
