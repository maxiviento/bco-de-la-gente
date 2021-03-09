import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { LineaAdendaResultado } from '../../shared/modelo/linea-adenda-resultado.model';
import { PagosService } from '../../shared/pagos.service';
import { GenerarAdendaComando } from '../../shared/modelo/generar-adenda-comando.model';
import { NotificacionService } from '../../../shared/notificacion.service';

@Component({
  selector: 'bg-modal-adenda',
  templateUrl: './modal-adenda.component.html',
  styleUrls: ['./modal-adenda.component.scss']
})

export class ModalAdendaComponent implements OnInit {
  @Input() public nroDetalle: number;
  public form: FormGroup;
  public lineas: LineaAdendaResultado[];
  public adendaConfirmada: boolean = false;
  public mensaje: string = '';
  public tieneRespuesta: boolean = false;
  public procesando: boolean = false;

  constructor(public activeModal: NgbActiveModal,
              private notificacionService: NotificacionService,
              private fb: FormBuilder,
              private pagosService: PagosService) {
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarLineas();
  }

  private consultarLineas(): void {
    this.pagosService.obtenerLineasAdenda(this.nroDetalle).subscribe(
      (res) => {
        this.lineas = res;
        this.crearForm();
      });
  }

  private crearForm() {
    this.form = this.fb.group({
      formularios: this.fb.array((this.lineas || []).map((linea) => {
        let porcentajeFC = new FormControl('',
          Validators.compose([
            Validators.required,
            Validators.maxLength(3),
            CustomValidators.minValue(1),
            CustomValidators.decimalNumberWithTwoDigits]));
        let montoFijoFC = new FormControl('',
          Validators.compose([
            Validators.required,
            Validators.maxLength(6),
            CustomValidators.minValue(1),
            CustomValidators.decimalNumberWithTwoDigits]));
        let cuotasFC = new FormControl('',
          Validators.compose([
            Validators.required,
            Validators.maxLength(2),
            CustomValidators.number,
            CustomValidators.minValue(1),
            CustomValidators.maxValue(linea.limiteCuota)]));

        porcentajeFC.valueChanges.subscribe((valorPorcentaje) => {
          if (valorPorcentaje) {
            montoFijoFC.disable({emitEvent: false});
            montoFijoFC.clearAsyncValidators();
          } else {
            montoFijoFC.enable({emitEvent: false});
            montoFijoFC.setValidators(Validators.compose([
              Validators.required,
              Validators.maxLength(6),
              CustomValidators.decimalNumberWithTwoDigits]));
          }
        });

        montoFijoFC.valueChanges.subscribe((valorMontoFijo) => {
          if (valorMontoFijo) {
            porcentajeFC.disable({emitEvent: false});
            porcentajeFC.clearValidators();
          } else {
            porcentajeFC.enable({emitEvent: false});
            porcentajeFC.setValidators(Validators.compose([
              Validators.required,
              Validators.maxLength(3),
              CustomValidators.decimalNumberWithTwoDigits]));
          }
        });

        return this.fb.group({
          nombreLinea: new FormControl(linea.nombreLinea),
          idLinea: new FormControl(linea.idLinea),
          porcentaje: porcentajeFC,
          montoFijo: montoFijoFC,
          cuotas: cuotasFC
        });
      }))
    });
  }

  public get lineasFormArray(): FormArray {
    return this.form.get('formularios') as FormArray;
  }

  public confirmarAdenda(): void {
    this.notificacionService.confirmar('¿Está seguro que desea crear la adenda para los préstamos seleccionados?')
      .result.then((value) => {
      if (value) {
        this.procesando = true;
        let comando = this.prepararComando();
        this.pagosService.generarAdenda(comando).subscribe(
          (res) => {
            this.tieneRespuesta = true;
            this.adendaConfirmada = res;
            if (this.adendaConfirmada) {
              this.form.disable();
              this.mensaje = 'Se confirmó la adenda exitosamente';
            } else {
              this.mensaje = 'Ocurrió un error al confirmar la adenda';
            }
          }
        );
      }
    });
  }

  private prepararComando(): GenerarAdendaComando {
    let formularios = this.lineasFormArray.controls;
    let cadenaComando: string = '';
    formularios.forEach(
      (form) => {
        cadenaComando = cadenaComando +
          form.get('idLinea').value
          + ',' + ((form.get('porcentaje').value === '') ? '-1' : form.get('porcentaje').value)
          + ',' + ((form.get('montoFijo').value === '') ? '-1' : form.get('montoFijo').value)
          + ',' + form.get('cuotas').value + ';';
      });
    return new GenerarAdendaComando(this.nroDetalle, cadenaComando);
  }

  public cerrar(confirmar: boolean): void {
    this.activeModal.close(confirmar);
  }
}
