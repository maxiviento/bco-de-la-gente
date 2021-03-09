import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConsultaArea } from './shared/modelo/consulta-area.model';
import { Area } from './shared/modelo/area.model';
import { AreasService } from './shared/areas.service';
import { NotificacionService } from '../shared/notificacion.service';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { Router } from '@angular/router';
import { DomSanitizer, Title } from '@angular/platform-browser';
import { CustomValidators } from '../shared/forms/custom-validators';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-areas',
  templateUrl: './areas.component.html',
  styleUrls: ['./areas.component.scss'],
  providers: [AreasService]
})
export class AreaComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public areas: Area[] = [];
  public consulta: ConsultaArea = new ConsultaArea();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public nombreAreas: string[] = [];

  constructor(private fb: FormBuilder,
              private areasService: AreasService,
              private sanitizer: DomSanitizer,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
  this.titleService.setTitle('Áreas' + TituloBanco.TITULO);
  }
  public ngOnInit(): void {
    this.obtenerNombreAreas();
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('area')) {
      AreasService.guardarFiltros(null);
    }
  }

  private crearForm(): void {
    this.form = this.fb.group({
      nombre: ['', Validators.compose([Validators.maxLength(100), CustomValidators.validTextAndNumbers])],
      incluirDadosDeBaja: [false]
    });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsulta();
        this.consulta.numeroPagina = params.numeroPagina;
        return this.areasService
          .consultarAreas(this.consulta);
      })
      .share();
    (<Observable<Area[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((areas) => {
        this.areas = areas;
        if (!this.form.valid) {
          this.notificacionService
            .informar(['El nombre no puede superar los 50 caracteres.']);
        } else {
          if (this.areas.length !== 0) {
            AreasService.guardarFiltros(this.consulta);
          }
        }
      });
  }

  private obtenerNombreAreas(): void {
    this.areasService.consultarAreasCombo().subscribe((areas) => {
      areas.forEach((x) => this.nombreAreas.push(x.descripcion));
    });
  }

  public searchArea = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .distinctUntilChanged()
      .map((term) => term.length < 2 ? []
        : this.nombreAreas.filter( (area) =>
        area.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));
  public showArea = (area: any) => area;

  public consultarAreas(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private prepararConsulta() {
    let form = this.form.value;
    this.consulta.nombre = form.nombre;
    this.consulta.incluirDadosDeBaja = form.incluirDadosDeBaja;
  }

  private reestablecerFiltros() {
    let filtrosGuardados = AreasService.recuperarFiltros();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.form.patchValue(this.consulta);
      this.consultarAreas();
    }
  }
}
