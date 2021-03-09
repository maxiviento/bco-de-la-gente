import { Component, Input, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { BandejaChequeResultado } from '../../shared/modelo/bandeja-cheque-resultado.model';
import { CargaDatosChequeComando } from '../../shared/modelo/carga-datos-cheque-comando.model';
import { BandejaChequeConsulta } from '../../shared/modelo/bandeja-cheque-consulta.model';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils } from '../../../shared/date-utils';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-grilla-bandeja-cheques',
  templateUrl: 'grilla-bandeja-cheques.component.html',
  styleUrls: ['grilla-bandeja-cheques.component.scss']
})
export class GrillaBandejaChequesComponent implements OnInit {
  private _consulta: BandejaChequeConsulta;
  public bandejaResultados: BandejaChequeResultado[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public reporteSource: any;
  private formFormulariosCheque: FormGroup;

  @Input() public totalizador: number;

  @Input()
  public set consulta(consulta: BandejaChequeConsulta) {
    this._consulta = consulta;
    if (this._consulta) {
      this.paginaModificada.next(this._consulta.numeroPagina);
    }
  }

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig) {

  }

  public ngOnInit(): void {
    this.configurarPaginacion();
    this.crearFormFormulariosConCheque();
    DateUtils.removeMaxDateDP(this.config);
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this._consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService.consultarBandejaCheques(filtros);
      })
      .share();
    (<Observable<BandejaChequeResultado[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormFormulariosConCheque();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public get formulariosFormArray(): FormArray {
    return this.formFormulariosCheque.get('formularios') as FormArray;
  }

  public consultarSiguientePagina(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  private crearFormFormulariosConCheque() {
    this.formFormulariosCheque = this.fb.group({
      formularios: this.fb.array((this.bandejaResultados || []).map((formulario) =>
        this.fb.group({
          idFormulario: [formulario.idFormulario],
          nroFormulario: [formulario.nroFormulario],
          idPrestamo: [formulario.idPrestamo],
          linea: [formulario.linea],
          departamento: [formulario.departamento],
          localidad: [formulario.localidad],
          nroPrestamo: [formulario.nroPrestamo],
          nroCheque: [formulario.nroCheque],
          nroChequeNuevo: [formulario.nroChequeNuevo, CustomValidators.validTextAndNumbers],
          fechaVencimientoCheque: [formulario.fechaVencimientoCheque],
          fechaChequeNuevo: [formulario.fechaChequeNuevo],
          apellidoNombreSolicitante: [formulario.apellidoNombreSolicitante],
          cuilSolicitante: [formulario.cuilSolicitante],
          origen: [formulario.origen],
        })
      )),
    });
  }

  public editarNumeroCheque(resultado: FormControl) {
    let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === resultado.get('nroFormulario').value)[0];
    formulario.nroChequeNuevo = resultado.get('nroCheque').value;
    formulario.nroCheque = null;
    this.crearFormFormulariosConCheque();
  }

  public borrarNumeroCheque(resultado: any) {
    let comando = new CargaDatosChequeComando();
    comando.idFormulario = resultado.get('idFormulario').value;
    // Si se quiere mantener el nro de cheque que ya tiene y solo modificar fecha se manda NULL en nro cheque.
    // Si se quiere borrar el nro de cheque que tiene, se manda "-1" en el nro de cheque.
    comando.nroCheque = '-1';
    comando.fechaVencimientoCheque = resultado.get('fechaVencimientoCheque').value;

    this.pagosService.cargarDatosCheque(comando)
      .subscribe(() => {
        let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === resultado.get('nroFormulario').value)[0];
        formulario.nroChequeNuevo = null;
        formulario.nroCheque = null;
        this.crearFormFormulariosConCheque();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public cambioNumeroCheque(resultado: FormGroup) {
    if (resultado.valid) {
      let comando = new CargaDatosChequeComando();
      comando.nroCheque = resultado.get('nroChequeNuevo').value;
      comando.idFormulario = resultado.get('idFormulario').value;
      comando.fechaVencimientoCheque = resultado.get('fechaVencimientoCheque').value;

      this.pagosService.cargarDatosCheque(comando)
        .subscribe((res) => {
          if (res) {
            let formulario = this.bandejaResultados.filter((x) => x.idFormulario === comando.idFormulario)[0];
            formulario.nroCheque = comando.nroCheque;
            formulario.idFormulario = comando.idFormulario;
            this.crearFormFormulariosConCheque();
          } else {
            this.notificacionService.informar(['El nro de cheque ingresado ya existe para otro formulario.']);
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      resultado.updateValueAndValidity();
    }
  }

  public borrarFechaCheque(resultado: any) {
    let comando = new CargaDatosChequeComando();
    comando.idFormulario = resultado.get('idFormulario').value;
    comando.nroCheque = null;
    comando.fechaVencimientoCheque = null;

    this.pagosService.cargarDatosCheque(comando)
      .subscribe(() => {
        let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === resultado.get('nroFormulario').value)[0];
        formulario.fechaVencimientoCheque = null;
        formulario.fechaChequeNuevo = null;
        this.crearFormFormulariosConCheque();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public editarFechaCheque(resultado: FormControl) {
    let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === resultado.get('nroFormulario').value)[0];
    formulario.fechaChequeNuevo = resultado.get('fechaVencimientoCheque').value;
    formulario.fechaVencimientoCheque = null;

    this.crearFormFormulariosConCheque();
  }

  public cambioFechaCheque(resultado: FormGroup) {
    if (resultado) {
      resultado.get('fechaChequeNuevo').valueChanges.subscribe((x) => {
          let comando = new CargaDatosChequeComando();
          comando.fechaVencimientoCheque = NgbUtils.obtenerDate(x);
          comando.idFormulario = resultado.get('idFormulario').value;
          comando.nroCheque = null;

          this.pagosService.cargarDatosCheque(comando)
            .subscribe(() => {
              let formulario = this.bandejaResultados.filter((y) => y.idFormulario === comando.idFormulario)[0];
              formulario.fechaVencimientoCheque = comando.fechaVencimientoCheque;
              formulario.idFormulario = comando.idFormulario;
              this.crearFormFormulariosConCheque();
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      );
    } else {
      resultado.updateValueAndValidity();
    }
  }
}
