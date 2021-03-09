import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MotivoRechazo } from '../formularios/shared/modelo/motivo-rechazo';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { NotificacionService } from '../shared/notificacion.service';
import { Router } from '@angular/router';
import { MotivosRechazoService } from './shared/motivos-rechazo.service';
import { ConsultaMotivosRechazo } from './shared/modelo/consulta-motivos-rechazo.model';
import { Ambito } from './shared/modelo/ambito.model';
import { CustomValidators } from '../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-motivos-rechazo',
  templateUrl: './motivos-rechazo.component.html',
  styleUrls: ['./motivos-rechazo.component.scss'],
})

export class MotivosRechazoComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public motivosRechazo: MotivoRechazo [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consultaMotivos: ConsultaMotivosRechazo = new ConsultaMotivosRechazo();
  public ambitos: Ambito[];
  public nombresMotivoRechazoInput: string[] = [];
  public todosMotivosRechazosBusqueda: MotivoRechazo [] = [];

  constructor(private motivoRechazoService: MotivosRechazoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Motivos de rechazo ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarAmbitos();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      abreviatura: [this.consultaMotivos.abreviatura, Validators.compose([
        Validators.maxLength(100),
        CustomValidators.validTextAndNumbers
      ])],
      codigo: [this.consultaMotivos.codigo, Validators.compose([Validators.maxLength(10)])],
      incluirDadosBaja: [this.consultaMotivos.incluirDadosDeBaja],
      ambito: [this.consultaMotivos.ambitoId, Validators.required],
    });

    (this.form.get('ambito') as FormControl).valueChanges
      .subscribe((value) => {
        this.obtenerMotivosRechazoPorAmbito(value);
      });
  }

  public obtenerMotivosRechazoPorAmbito(id?: number) {
    this.nombresMotivoRechazoInput = [];
    this.motivoRechazoService.consultarMotivosRechazoPorAmbito(id).subscribe(
      (motivosTraidos) => {
        this.todosMotivosRechazosBusqueda = motivosTraidos;
        this.todosMotivosRechazosBusqueda.forEach((motivo) => {
          if (!motivo.estaDadaDeBaja()) {
            this.nombresMotivoRechazoInput.push(motivo.nombre);
          }
        });
      });
  }

  public searchMotivos = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .distinctUntilChanged()
      .map((term) => term.length < 2 ? []
        : this.nombresMotivoRechazoInput.filter((motivo) =>
          motivo.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));
  public showMotivos = (motivo: any) => motivo;

  private consultarAmbitos(): void {
    this.motivoRechazoService.consultarAmbitos()
      .subscribe((ambitos) => this.ambitos = ambitos);
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this.prepararConsultaMotivo();
        this.consultaMotivos.numeroPagina = params.numeroPagina;
        return this.motivoRechazoService
          .consultarMotivosRechazoPaginados(this.consultaMotivos);
      }).share();
    (<Observable<MotivoRechazo[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((motivosRechazo) => {
        this.motivosRechazo = motivosRechazo;

        if (!this.form.valid) {
          this.notificacionService.informar(['El nombre no puede superar los 100 caracteres.']);
        } else {
          if (this.motivosRechazo.length !== 0) {
            MotivosRechazoService.guardarFiltros(this.consultaMotivos);
          }
        }
      });
  }

  public consultarMotivosRechazo(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private prepararConsultaMotivo(): void {
    let formModel = this.form.value;
    this.consultaMotivos.abreviatura = formModel.abreviatura;
    this.consultaMotivos.incluirDadosDeBaja = formModel.incluirDadosBaja;
    this.consultaMotivos.ambitoId = formModel.ambito;
    this.consultaMotivos.automatico = formModel.automatico;
    this.consultaMotivos.codigo = formModel.codigo;
  }

  private reestablecerFiltros() {
    let filtrosGuardados = MotivosRechazoService.recuperarFiltros();

    if (filtrosGuardados) {
      this.consultaMotivos = filtrosGuardados;
      this.form.patchValue({
        abreviatura: this.consultaMotivos.abreviatura,
        incluirDadosBaja: this.consultaMotivos.incluirDadosDeBaja,
        ambito: this.consultaMotivos.ambitoId,
        automatico: this.consultaMotivos.automatico,
        codigo: this.consultaMotivos.codigo
      });
      this.configurarPaginacion();
      this.consultarMotivosRechazo();
    }
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('motivo')) {
      MotivosRechazoService.guardarFiltros(null);
    }
  }
}
