import { CustomValidators } from './../shared/forms/custom-validators';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificacionService } from '../shared/notificacion.service';
import { MotivoDestinoService } from './shared/motivo-destino.service';
import { ConsultaMotivoDestino } from './shared/modelo/consulta-motivo';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { MotivoDestino } from './shared/modelo/motivo-destino.model';
import { Title } from '@angular/platform-browser';
import { isNullOrUndefined } from "util";
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-motivos-destino',
  templateUrl: './motivos-destino.component.html',
  styleUrls: ['./motivos-destino.component.scss'],
})

export class MotivosDestinoComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public motivosDestino: MotivoDestino [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consultaMotivos: ConsultaMotivoDestino = new ConsultaMotivoDestino();
  public nombreMotivosDestino: string[] = [];
  private conjuntoResultados: number = 0;

  constructor(private motivoDestinoService: MotivoDestinoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Motivos destino ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.obtenerMotivosDestino();
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      nombre: [this.consultaMotivos.nombre, Validators.compose([
        Validators.maxLength(100),
        CustomValidators.validTextAndNumbers
      ])],
      incluirDadosBaja: [this.consultaMotivos.incluirDadosDeBaja]
    });
  }

  private obtenerMotivosDestino(): void {
    this.motivoDestinoService.consultarMotivosDestino().subscribe((motivos) => {
      motivos.forEach((x) => this.nombreMotivosDestino.push(x.descripcion));
    });
  }

  public searchMotivos = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .distinctUntilChanged()
      .map((term) => term.length < 2 ? []
        : this.nombreMotivosDestino.filter((motivo) =>
          motivo.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));
  public showMotivos = (motivo: any) => motivo;

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this.prepararConsultaMotivo();
        this.consultaMotivos.numeroPagina = params.numeroPagina;
        isNullOrUndefined(params.numeroPagina) ? this.conjuntoResultados = 0 : this.conjuntoResultados = params.numeroPagina * 10;
        return this.motivoDestinoService
          .consultarMotivosDestinoPaginados(this.consultaMotivos);
      }).share();
    (<Observable<MotivoDestino[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((motivosDestino) => {
        this.motivosDestino = motivosDestino;
        if (!this.form.valid) {
          this.notificacionService.informar(['El nombre no puede superar los 50 caracteres.']);
        } else {
          if (this.motivosDestino.length === 0) {
            this.notificacionService.informar(['No se encontraron registros para los criterios de búsqueda ingresados']);
          } else {
            MotivoDestinoService.guardarFiltros(this.consultaMotivos);
          }
        }
      });
  }

  public consultarMotivosDestino(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private prepararConsultaMotivo(): void {
    let formModel = this.form.value;
    this.consultaMotivos.nombre = formModel.nombre;
    this.consultaMotivos.incluirDadosDeBaja = formModel.incluirDadosBaja;
  }

  private reestablecerFiltros() {
    let filtrosGuardados = MotivoDestinoService.recuperarFiltros();
    if (filtrosGuardados) {
      this.consultaMotivos = filtrosGuardados;
      this.form.patchValue(this.consultaMotivos);
      this.consultarMotivosDestino();
    }
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('motivo')) {
      MotivoDestinoService.guardarFiltros(null);
    }
  }
}
