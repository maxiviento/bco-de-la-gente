import { Component, Input } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { BandejaCambioEstadoConsulta } from '../../shared/modelo/bandeja-cambio-estado-consulta.model';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { BandejaCambioEstadoResultado } from '../../shared/modelo/bandeja-cambio-estado-resultado.model';

@Component({
  selector: 'bg-grilla-bandeja-cambio-estado',
  templateUrl: 'grilla-bandeja-cambio-estado.component.html',
  styleUrls: ['grilla-bandeja-cambio-estado.component.scss']
})
export class GrillaBandejaCambioEstadoComponent {
  private _consulta: BandejaCambioEstadoConsulta;
  public bandejaResultados: BandejaCambioEstadoResultado[] = [];
  public formLotes: FormGroup;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public reporteSource: any;

  @Input() public totalizador: number;
  @Input()
  public set consulta(consulta: BandejaCambioEstadoConsulta) {
    this._consulta = consulta;
    if (this._consulta) {
      this.paginaModificada.next(this._consulta.numeroPagina);
    }
  }

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService) {
    this.configurarPaginacion();
    this.crearFormResultados();
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this._consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService.consultarBandejaCambioEstado(filtros);
      })
      .share();
    (<Observable<BandejaCambioEstadoResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultados) => {
        this.bandejaResultados = resultados;
        this.crearFormResultados();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  private crearFormResultados() {
    this.formLotes = this.fb.group({
      resultados: this.fb.array((this.bandejaResultados || []).map((resultado) =>
        this.fb.group({
          idFormulario: [resultado.idFormulario],
          nroFormulario: [resultado.nroFormulario],
          idLinea: [resultado.linea],
          nombreYApellido: [resultado.apellido + ' , ' + resultado.nombre],
          estadoFormulario: [resultado.estadoFormulario],
          nroPrestamo: [resultado.nroPrestamo],
          estadoPrestamo: [resultado.estadoPrestamo],
          localidad: [resultado.localidad],
          elementoPago: [resultado.elementoPago],
          fechaFormulario: [resultado.fechaFormulario]
        })
      )),
    });
  }

  public get lotesFormArray(): FormArray {
    return this.formLotes.get('resultados') as FormArray;
  }

  public consultarFormularios(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public cambiarEstadoFormulario(idFormulario: number): void {
    this.notificacionService.confirmar('¿Desea confirmar el cambio de estado?')
      .result.then((result) => {
      if (result) {
        this.pagosService.cambiarEstadoFormulario(idFormulario).subscribe((resultado) => {
          if (resultado) {
            this.notificacionService.informar(['El cambio de estado se realizó con éxito'])
              .result.then(() => {
              this.consultarFormularios(this._consulta.numeroPagina);
            });
          }
        });
      }
    });
  }
}
