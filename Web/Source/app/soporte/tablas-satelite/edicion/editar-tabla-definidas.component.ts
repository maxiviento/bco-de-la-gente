import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TablaDefinida } from '../../modelo/tabla-definida.model';
import { TablasDefinidasService } from '../../tablas-definidas.service';
import { ConsultaParametrosTablasDefinidas } from '../../modelo/consulta-parametros-tablas-definidas';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ParametroTablaDefinida } from '../../modelo/parametro-tabla-definida';
import { ModalMotivoRechazoComponent } from '../../../formularios/modal-motivo-rechazo/modal-motivo-rechazo.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RechazarParametroTablaDefinida } from '../../modelo/rechazar-parametro-tabla-definida';
import { NotificacionService } from '../../../shared/notificacion.service';
import { Observable, Subject } from '../../../../../node_modules/rxjs';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'editar-tabla-definida',
  templateUrl: './editar-tabla-definidas.component.html',
  styleUrls: ['./editar-tabla-definidas.component.scss'],
  providers: []
})
export class EditarTablaDefinidasComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public tablaDefinida: TablaDefinida = new TablaDefinida();
  public tablaConsulta: TablaDefinida = new TablaDefinida();
  public consulta: ConsultaParametrosTablasDefinidas = new ConsultaParametrosTablasDefinidas();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  public idTabla: number;
  public muestraDadosDeBaja: boolean = false;
  public parametro: ParametroTablaDefinida;

  constructor(private fb: FormBuilder,
              private tablaDefinidasService: TablasDefinidasService,
              private notificacionService: NotificacionService,
              private modalService: NgbModal,
              private route: ActivatedRoute,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Editar tablas definidas ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.crearForm();
    this.restablecerFiltros();
    this.configurarPaginacion();
  }

  private obtenerParametrosCombo(): void {
    let formModel = this.form.value;
    this.consulta.incluirDadosDeBaja = !formModel.incluirDadosDeBaja;
    this.consulta.idTabla = this.tablaConsulta.id;
    this.tablaDefinidasService.obtenerParametrosCombo(this.consulta).subscribe((parametros) => {
      this.tablaConsulta.parametros = parametros;
    });
  }

  private obtenerParametrosFiltrados(): void {
    this.tablaDefinidasService.obtenerParametrosCombo(this.consulta).subscribe((parametros) => {
      this.tablaDefinida.parametros = parametros;
    });
  }

  private prepararConsultaParametros(pagina?: number): void {
    let formModel = this.form.value;
    this.consulta.nombre = formModel.nombreParametro;
    this.consulta.idParametro = formModel.id;
    this.consulta.incluirDadosDeBaja = formModel.incluirDadosDeBaja;
    this.muestraDadosDeBaja = formModel.incluirDadosDeBaja;
    this.consulta.idTabla = this.tablaConsulta.id;
    TablasDefinidasService.guardarFiltrosParametros(this.consulta);
    this.paginaModificada.next(pagina);
  }

  private nuevoParametro() {
    this.router.navigate(['/nuevo-parametro-tabla-definida', this.tablaConsulta.id]);
  }

  private restablecerFiltros() {
    let filtrosGuardados = TablasDefinidasService.recuperarFiltrosParametros();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.form.patchValue({
        id: this.consulta.idParametro,
        nombre: this.consulta.nombre,
        incluirDadosDeBaja: this.consulta.incluirDadosDeBaja
      });
    }
  }

  private eliminarParametro(idParametro: number): void {
    const modalRechazo = this.modalService.open(ModalMotivoRechazoComponent, {backdrop: 'static', size: 'lg'});
    modalRechazo.componentInstance.ambito = 'Parametro_Tabla_Definida';
    modalRechazo.componentInstance.unSoloMotivo = true;
    modalRechazo.componentInstance.muestraObservaciones = false;
    modalRechazo
      .result
      .then((comando) => {
        if (comando) {
          this.tablaDefinidasService.rechazarParametro(new RechazarParametroTablaDefinida(idParametro, comando.motivosRechazo[0].id, comando.observaciones))
            .subscribe((resultado) => {
              if (resultado) {
                this.notificacionService.informar(Array.of('Parámetro dado de baja con éxito.'), false)
                  .result.then(() => {
                  this.prepararConsultaParametros();
                });
              }
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      });
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
          .obtenerParametrosFiltrados(filtros);
      })
      .share();
    (<Observable<ParametroTablaDefinida[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((parametros) => {
        this.tablaDefinida.parametros = parametros;
      });
  }

  private crearForm() {
    this.form = this.fb.group({
      id: -1,
      nombreParametro: ['', Validators.maxLength(50)],
      incluirDadosDeBaja: false
    });
    this.route.params
      .switchMap((params: Params) => this.tablaDefinidasService.obtenerDatosTablaDefinida(+params['id']))
      .subscribe(
        (tabla: TablaDefinida) => {
          this.tablaConsulta = tabla;
          if (!(this.consulta.idTabla == null)) {
            this.obtenerParametrosCombo();
            this.prepararConsultaParametros();
          }
        });
  }

  public ngOnDestroy(): void {
    if (!(this.router.url.includes('editar-tabla-definida')
      || this.router.url.includes('nuevo-parametro-tabla-definida')
      || this.router.url.includes('consultar-parametro-tabla-definida'))) {
      TablasDefinidasService.guardarFiltrosParametros(null);
    }
  }
}
