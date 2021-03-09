import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { FiltrosPerfiles } from './shared-perfiles/modelo/filtros-perfiles.model';
import { PerfilesService } from './shared-perfiles/perfiles.service';
import { ItemPerfiles } from './shared-perfiles/modelo/item-perfiles.model';
import { Router } from '@angular/router';
import { isNullOrUndefined } from 'util';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-perfiles',
  templateUrl: './perfiles.component.html',
  styleUrls: ['./perfiles.component.scss']
})
export class PerfilesComponent implements OnInit, OnDestroy {
  public fechaActual: Date;
  public form: FormGroup;
  public perfiles: ItemPerfiles[];
  public mostrarBajas: boolean = false;
  private conjuntoResultados: number = 0;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  constructor(private fb: FormBuilder,
              private perfilesService: PerfilesService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Perfiles ' + TituloBanco.TITULO);
    this.fechaActual = new Date();
    this.perfiles = [];
  }

  public ngOnInit(): void {
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('perfil')) {
      PerfilesService.guardarFiltros(null);
    }
  }

  private crearForm(): void {
    this.form = this.fb.group({
      nombre: ['', Validators.compose([Validators.maxLength(200)])],
      incluirBajas: null
    });
  }

  public consultarPerfiles(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private configurarPaginacion() {
    let filtros;
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        filtros = this.prepararFiltrosRequisito();
        filtros.numeroPagina = params.numeroPagina;

        isNullOrUndefined(params.numeroPagina) ? this.conjuntoResultados = 0 : this.conjuntoResultados = params.numeroPagina * 10;

        return this.perfilesService
          .consultarPerfiles(filtros);
      })
      .share();

    (<Observable<ItemPerfiles[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((perfiles) => {
        this.perfiles = perfiles;
        PerfilesService.guardarFiltros(filtros);
      });
  }

  private prepararFiltrosRequisito(): FiltrosPerfiles {
    let formModel = this.form.value;

    this.mostrarBajas = formModel.incluirBajas;

    return new FiltrosPerfiles(
      formModel.nombre,
      formModel.incluirBajas
    );
  }

  private reestablecerFiltros() {
    let filtrosGuardados = PerfilesService.recuperarFiltros();
    if (filtrosGuardados) {
      this.form.patchValue(filtrosGuardados);
      this.consultarPerfiles();
    }
  }

  public numerarElemento(indice: number): number {
    return this.conjuntoResultados + ++indice;
  }
}
