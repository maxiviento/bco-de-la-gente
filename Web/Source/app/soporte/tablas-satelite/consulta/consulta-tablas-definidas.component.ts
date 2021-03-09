import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TablaDefinida } from '../../modelo/tabla-definida.model';
import { TablasDefinidasService } from '../../tablas-definidas.service';
import { Router } from '@angular/router';
import { ConsultaTablasDefinidas } from '../../modelo/consulta-tablas-definidas';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'consulta-tablas-definidas',
  templateUrl: './consulta-tablas-definidas.component.html',
  styleUrls: ['./consulta-tablas-definidas.component.scss'],
  providers: []
})
export class ConsultarTablasDefinidasComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public tablasDefinidasCombo: TablaDefinida[] = [];
  public tablasConsultadas: TablaDefinida[] = [];
  public consulta: ConsultaTablasDefinidas = new ConsultaTablasDefinidas();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  constructor(private tablaDefinidasService: TablasDefinidasService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Tablas definidas ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.crearForm();
    this.obtenerTablas();
    this.restablecerFiltros();
    this.configurarPaginacion();
  }

  public obtenerTablas() {
    this.tablaDefinidasService.obtenerTablasDefinidas().subscribe((tablas) => {
      this.tablasDefinidasCombo = tablas;
    });
  }

  public prepararConsultaTablas(pagina?: number): void {
    let formModel = this.form.value;
    this.consulta.idTabla = formModel.id;
    this.consulta.nombre = '';
    TablasDefinidasService.guardarFiltrosTablasDefinidas(this.consulta);
    this.configurarPaginacion();
    this.paginaModificada.next(pagina);
  }

  private editarParametrosTabla(idTabla: number) {
    this.router.navigate(['/editar-tabla-definida', idTabla]);
  }

  private restablecerFiltros() {
    let filtrosGuardados = TablasDefinidasService.recuperarFiltrosTablasDefinidsa();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.form.patchValue({
        id: this.consulta.idTabla
      });
      this.prepararConsultaTablas();
    }
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.tablaDefinidasService
          .obtenerTablasDefinidasFiltradas(filtros);
      })
      .share();
    (<Observable<TablaDefinida[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((tablas) => {
        this.tablasConsultadas = tablas;
      });
  }

  private crearForm() {
    this.form = new FormGroup({
      id: new FormControl()
    });
  }

  public ngOnDestroy(): void {
    if (!(this.router.url.includes('editar-tabla-definida')
      || this.router.url.includes('consultar-tablas-definidas')
      || this.router.url.includes('nuevo-parametro-tabla-definida')
      || this.router.url.includes('consultar-parametro-tabla-definida'))) {
      TablasDefinidasService.guardarFiltrosTablasDefinidas(null);
    }
  }

  public consultarTablasDefinidas(pagina?: number) {
    this.paginaModificada.next(pagina);
  }
}
