import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Parametro } from '../modelo/parametro.model';
import { ParametroService } from '../parametro.service';
import { ApartadoParametroComponent } from './apartado-parametro/apartado-parametro.component';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { isDefined } from '@ng-bootstrap/ng-bootstrap/util/util';
import { ConsultaParametro } from '../modelo/consulta-parametro.model';
import { Observable, Subject } from 'rxjs';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'cce-administrar-parametros',
  templateUrl: './administrar-parametros.component.html',
  styleUrls: ['./administrar-parametros.scss'],
  providers: [ParametroService]
})
export class AdministrarParametrosComponent implements OnInit {
  public form: FormGroup;
  public parametros: Parametro[] = [];
  public parametrosCombo: Parametro[];
  public consulta: ConsultaParametro = new ConsultaParametro();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  private opcionesModal: NgbModalOptions = {
    size: 'lg',
    backdrop: 'static'
  };

  constructor(private parametrosService: ParametroService,
              private modalService: NgbModal,
              private titleService: Title) {
    this.titleService.setTitle('Parámetros ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.crearFormFiltros();
    this.obtenerParametros();
    this.configurarPaginacion();
  }

  private crearFormFiltros(): void {
    this.form = new FormGroup({
      id: new FormControl(),
      incluirNoVigentes: new FormControl(false)
    });
    this.consultar(0);
  }

  public obtenerParametros() {
    this.parametrosService.obtenerParametros().subscribe((parametros) => {
      this.parametrosCombo = parametros;
      this.parametrosCombo = this.parametrosCombo.splice(0);
    });
  }

  public consultar(numeroPagina?: number) {
    this.paginaModificada.next(numeroPagina);
  }

  public modificarParametro(parametro: Parametro): void {
    let nuevoParametro = parametro.clone();
    nuevoParametro.configurarValor();
    const editarParametro = this.modalService.open(ApartadoParametroComponent, this.opcionesModal);
    editarParametro.componentInstance.parametro = nuevoParametro;
    editarParametro.result.then((param) => {
      if (param instanceof Parametro) {
        this.consultar(this.consulta.numeroPagina);
      }
    });
  }

  public mostrarFechaHasta(): boolean {
    let parametroNoVigente = this.parametros.find((p) => p.tieneFechaHasta());
    return isDefined(parametroNoVigente);
  }

  private obtenerFiltros() {
    this.consulta.id = this.form.value.id;
    this.consulta.incluirNoVigentes = this.form.value.incluirNoVigentes;
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this.obtenerFiltros();
        this.consulta.numeroPagina = params.numeroPagina;
        return this.parametrosService
          .obtenerParametrosDetallado(this.consulta);
      })
      .share();
    (<Observable<Parametro[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((parametros) => {
        this.parametros = parametros;
      });
  }
}
