import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BandejaAsignarMontoDisponible } from '../../shared/modelo/bandeja-asignar-monto-disponible.model';
import { MontoDisponibleConsulta } from '../../shared/modelo/monto-disponible-consulta.model';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { ModalidadPagoComponent } from '../../modalidad-pago/modalidad-pago.component';
import { ConfirmarLoteComando } from '../../shared/modelo/confirmar-lote-comando.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import TiposLote from '../../tipos-lote.enum';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-generar-lote-pago',
  templateUrl: './generar-lote-pago.component.html',
  styleUrls: ['./generar-lote-pago.component.scss'],
})

export class GenerarLotePagoComponent implements OnInit {
  public form: FormGroup;
  public montosDisponibles: BandejaAsignarMontoDisponible[] = [];
  public montoSeleccionado: BandejaAsignarMontoDisponible = new BandejaAsignarMontoDisponible();
  public formAsignarMontoDisponible: FormGroup;
  public monto: number;
  public idLoteSuaf: number;
  @ViewChild(ModalidadPagoComponent)
  public modalidad: ModalidadPagoComponent;

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private route: ActivatedRoute,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Generar lote de pago ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) =>
        this.pagosService.obtenerTotalLote(+params['id']))
      .subscribe((monto) => {
        this.monto = monto;
        this.consultarMontos(monto);
      });
    this.route.params.subscribe((params: Params) => {
      this.idLoteSuaf = +params['id'];
    });
    this.crearFormAsignarMontoDisponible();
    this.pagosService.habilitadoAdenda(this.idLoteSuaf).subscribe
    ((data) => {
      if (!data) {
        this.formAsignarMontoDisponible.get('nombreLote').disable({emitEvent: true});
        this.asignarMontoDisponibleFormArray.disable({emitEvent: false});
        this.modalidad.disableForm();
        this.notificacionService.informar(['Al menos uno de los préstamos del lote no tiene número de devengado, no es posible generar la adenda.']);
      }
    });
  }

  public clickMonto(nroMonto: number) {
    let montoCheckeado = this.montosDisponibles.filter((x) => x.idMontoDisponible === nroMonto)[0];
    montoCheckeado.seleccionado = true;
    this.montoSeleccionado = montoCheckeado;
    let montosNoCheckeado = this.montosDisponibles.filter((x) => x.idMontoDisponible !== nroMonto);
    montosNoCheckeado.forEach((x) => x.seleccionado = false);
  }

  public get asignarMontoDisponibleFormArray(): FormArray {
    return this.formAsignarMontoDisponible.get('montos') as FormArray;
  }

  public crearFormAsignarMontoDisponible() {
    this.formAsignarMontoDisponible = this.fb.group({
      nombreLote: ['', Validators.required],
      montos: this.fb.array((this.montosDisponibles || []).map((monto) =>
        this.fb.group({
          idMontoDisponible: [monto.idMontoDisponible],
          nroMonto: [monto.nroMontoDisponible],
          fechaAlta: [monto.fechaAlta],
          descripcion: [monto.descripcion],
          montoTotal: [monto.montoTotal],
          montoUsado: [monto.montoUsado],
          montoAUsar: [monto.montoAUsar = monto.montoTotal - monto.montoUsado],
          seleccionado: [monto.seleccionado]
        })
      )),
    });
  }

  public consultarMontos(monto: number) {
    this.pagosService.consultarMontosDisponibles(new MontoDisponibleConsulta(monto)).subscribe(
      (montosDisponibles) => {
        if (montosDisponibles.length === 0) {
          this.notificacionService.informar(['No se encontraron montos disponibles con sufiente monto libre']);
        } else {
          this.montosDisponibles = montosDisponibles;
          this.crearFormAsignarMontoDisponible();
        }
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public generar() {

    this.notificacionService.confirmar('Se creará un lote de pago. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        let comando = new ConfirmarLoteComando();
        comando.nombreLote = this.formAsignarMontoDisponible.get('nombreLote').value;
        comando.idMontoDisponible = this.getMontoLoteSeleccionado().idMontoDisponible;
        comando.modalidad = this.modalidad.obtenerModalidad();
        comando.elemento = this.modalidad.obtenerElemento();
        comando.fechaPago = this.modalidad.obtenerFechaPago();
        comando.fechaFinPago = this.modalidad.obtenerFechaFinPago();
        comando.convenio = this.modalidad.obtenerConvenio();
        comando.mesesGracia = this.modalidad.obtenerMesesGracia();

        if (comando.elemento === 1) {
          comando.idTipoLote = TiposLote.LOTE_PAGO_BANCO_ADENDA;
        } else {
          comando.idTipoLote = TiposLote.LOTE_PAGO_CHEQUE_ADENDA;
        }

        comando.monto = this.monto;
        comando.idLoteSuaf = this.idLoteSuaf;
        this.pagosService.confirmarLoteAdenda(comando)
          .subscribe((resultado) => {
            if (resultado) {
              this.notificacionService.informar(Array.of('Lote creado con éxito'), false)
                .result.then(() => {
                this.formAsignarMontoDisponible.get('nombreLote').disable({emitEvent: true});
                this.asignarMontoDisponibleFormArray.disable({emitEvent: false});
                this.modalidad.disableForm();
              });
            }
          }, (errores) => {
            this.notificacionService.informar(errores, true);
          });
      }
    });
  }

  public volver() {
    this.formAsignarMontoDisponible = undefined;
    this.montosDisponibles = [];
    this.router.navigate(['/bandeja-suaf']);
  }

  private getMontoLoteSeleccionado(): BandejaAsignarMontoDisponible {
    return this.montosDisponibles.filter((x) => x.seleccionado === true)[0];
  }

  private validarQueHayaSeleccionadoUnMonto(): boolean {
    let montosSeleccionados = this.montosDisponibles.filter((x) => x.seleccionado === true);
    return !(montosSeleccionados.length === 0);
  }
}
