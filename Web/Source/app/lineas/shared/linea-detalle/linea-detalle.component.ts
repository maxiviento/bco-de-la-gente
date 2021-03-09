import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { DetalleLineaPrestamo } from '../modelo/detalle-linea-prestamo.model';
import { IntegranteSocio } from '../modelo/integrante-socio.model';
import { TipoFinanciamiento } from '../modelo/tipo-financiamiento.model';
import { TipoInteres } from '../modelo/tipo-interes.model';
import { TipoGarantia } from '../modelo/tipo-garantia.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { ParametroService } from '../../../soporte/parametro.service';
import { VigenciaParametro } from '../../../shared/modelo/vigencia-parametro.model';
import { VigenciaParametroConsulta } from '../../../shared/modelo/consultas/vigencia-parametro-consulta.model';
import { Subscription } from 'rxjs/Subscription';
import { Convenio } from '../../../shared/modelo/convenio-model';

@Component({
  selector: 'bg-linea-detalle',
  templateUrl: './linea-detalle.component.html',
  styleUrls: ['./linea-detalle.component.scss']
})

export class LineaDetalleComponent implements OnInit, OnDestroy {
  @Input('detalle') public form: FormGroup;
  @Input() public vistaModal: boolean = false;
  @Input() public mostrarAgregar: boolean = false;
  @Input() public integrantes: IntegranteSocio[] = [];
  @Input() public financiamientos: TipoFinanciamiento[] = [];
  @Input() public intereses: TipoInteres[] = [];
  @Input() public garantias: TipoGarantia[] = [];
  @Input() public conveniosPago: Convenio[] = [];
  @Input() public conveniosRecupero: Convenio[] = [];
  @Output() public detalleLinea = new EventEmitter<DetalleLineaPrestamo>();
  private subscripciones: Subscription = new Subscription();
  public convenios: Convenio[] = [];
  private cuotaIntegranteSocio: string;

  constructor(private parametroService: ParametroService) {
  }

  public cantMinIntParametro: VigenciaParametro;
  public cantMaxIntParametro: VigenciaParametro;

  public ngOnInit(): void {
    this.cargarParametros();
    if (this.vistaModal) {
      this.form.disable();
    }
  }

  public static nuevoFormGroup(detalleLinea: DetalleLineaPrestamo = new DetalleLineaPrestamo()): FormGroup {
    return new FormGroup({
      id: new FormControl(detalleLinea.id),
      apoderado: new FormControl(detalleLinea.apoderado),
      integranteSocio: new FormControl(detalleLinea.integranteSocio ? detalleLinea.integranteSocio.id : null, Validators.required),
      montoTope: new FormControl(detalleLinea.montoTope, Validators.compose([
        Validators.maxLength(8), CustomValidators.number])),
      montoPrestable: new FormControl(detalleLinea.montoPrestable, Validators.compose([
        Validators.required, Validators.maxLength(8), CustomValidators.minValue(0),
        CustomValidators.number])),
      cantidadMinimaIntegrantes: new FormControl(detalleLinea.cantidadMinimaIntegrantes, Validators.compose([
        Validators.required, Validators.maxLength(3), CustomValidators.number])),
      cantidadMaximaIntegrantes: new FormControl(detalleLinea.cantidadMaximaIntegrantes, Validators.compose([
        Validators.required, Validators.maxLength(3), CustomValidators.number])),
      plazoDevolucion: new FormControl(detalleLinea.plazoDevolucion, Validators.compose([
        Validators.required, Validators.maxLength(3), CustomValidators.minValue(1), CustomValidators.maxValue(99), CustomValidators.number])),
      tipoFinanciamiento: new FormControl(detalleLinea.tipoFinanciamiento ? detalleLinea.tipoFinanciamiento.id : null,
        Validators.required),
      tipoInteres: new FormControl(detalleLinea.tipoInteres ? detalleLinea.tipoInteres.id : null,
        Validators.required),
      tipoGarantia: new FormControl(detalleLinea.tipoGarantia ? detalleLinea.tipoGarantia.id : null,
        Validators.required),
      valorCuotaSolidaria: new FormControl(detalleLinea.valorCuotaSolidaria, Validators.compose([
        Validators.maxLength(8), CustomValidators.number])),
      visualizacion: new FormControl(detalleLinea.visualizacion, Validators.compose([
        Validators.maxLength(500), Validators.required])),
      idConvenioPago: new FormControl(detalleLinea.convenioPago ? detalleLinea.convenioPago.id : null,
        Validators.required),
      idConvenioRecupero: new FormControl(detalleLinea.convenioRecupero ? detalleLinea.convenioRecupero.id : null,
        Validators.required)
    });
  }

  private cargarLabelCuotaSolidaria() {
    let integranteSocioFormControl = this.form.get('integranteSocio');
    if (integranteSocioFormControl.value && integranteSocioFormControl.value == 1) {
      this.cuotaIntegranteSocio = 'Individual';
    } else if (integranteSocioFormControl.value && integranteSocioFormControl.value == 2) {
      this.cuotaIntegranteSocio = 'Solidario Independiente';
    } else if (integranteSocioFormControl.value && integranteSocioFormControl.value == 3) {
      this.cuotaIntegranteSocio = 'Solidario Asociativo';
    }
  }

  public inicializarForm(): void {
    let integranteSocioFormControl = this.form.get('integranteSocio');
    let montoTopeFormControl = this.form.get('montoTope');
    let montoPrestableFormControl = this.form.get('montoPrestable');
    let cantidadMinimaIntegrantesFormControl = this.form.get('cantidadMinimaIntegrantes');
    let cantidadMaximaIntegrantesFormControl = this.form.get('cantidadMaximaIntegrantes');
    let plazoDevolucionFormControl = this.form.get('plazoDevolucion');
    let valorCuotaSolidariaFormControl = this.form.get('valorCuotaSolidaria');

    montoTopeFormControl.disable();
    valorCuotaSolidariaFormControl.disable();
    this.subscripciones.add(montoPrestableFormControl
      .valueChanges
      .debounceTime(100)
      .subscribe((value) => {
        if (value) {
          if (integranteSocioFormControl.value && integranteSocioFormControl.value == 3) {
            montoTopeFormControl.clearValidators();
            montoTopeFormControl.setValidators(Validators.compose([
              Validators.required,
              Validators.maxLength(8),
              CustomValidators.minValue(0),
              CustomValidators.maxValue(+value)]));
            montoTopeFormControl.updateValueAndValidity();
          }
          if (cantidadMaximaIntegrantesFormControl) {
            let valorMontoTope: number = Math.round(+montoPrestableFormControl.value / +cantidadMaximaIntegrantesFormControl.value);
            montoTopeFormControl.setValue(valorMontoTope > 0 ? valorMontoTope : 0);
          }
        }
      }));

    this.subscripciones.add(montoTopeFormControl
      .valueChanges
      .debounceTime(100)
      .subscribe((value) => {
        if (value) {
          montoTopeFormControl.clearValidators();
          montoTopeFormControl
            .setValidators(Validators.compose([
              Validators.required,
              Validators.maxLength(8),
              CustomValidators.minValue(0),
              CustomValidators.maxValue(+montoPrestableFormControl.value || 0)
            ]));
          montoTopeFormControl.updateValueAndValidity();

          if (plazoDevolucionFormControl.value > 0) {
            let valorCuota: number = (+montoTopeFormControl.value / +plazoDevolucionFormControl.value);
            if (this.contarDecimales(valorCuota) > 1) {
              valorCuotaSolidariaFormControl.setValue((valorCuota > 0) ? this.truncar(valorCuota, 2) : 0);
            } else {
              valorCuotaSolidariaFormControl.setValue((valorCuota > 0) ? valorCuota : 0);
            }
          }
        }
      }));

    this.subscripciones.add(plazoDevolucionFormControl
      .valueChanges
      .debounceTime(100)
      .subscribe((value) => {
        if (value && montoTopeFormControl.value) {
          let valorCuota: number = (+montoTopeFormControl.value / +plazoDevolucionFormControl.value);
          if (this.contarDecimales(valorCuota) > 1) {
            valorCuotaSolidariaFormControl.setValue((valorCuota > 0) ? this.truncar(valorCuota, 2) : 0);
          } else {
            valorCuotaSolidariaFormControl.setValue((valorCuota > 0) ? valorCuota : 0);
          }
        }
      }));

    this.subscripciones.add(cantidadMinimaIntegrantesFormControl
      .valueChanges
      .debounceTime(100)
      .subscribe((value) => {
        if (value && integranteSocioFormControl.value &&
          (integranteSocioFormControl.value == 2 || integranteSocioFormControl.value == 3)) {
          LineaDetalleComponent.setValidadoresIntegrantes(
            cantidadMinimaIntegrantesFormControl,
            cantidadMaximaIntegrantesFormControl,
            this.cantMinIntParametro,
            this.cantMaxIntParametro);
        }
      }));

    this.subscripciones.add(cantidadMaximaIntegrantesFormControl
      .valueChanges
      .debounceTime(100)
      .subscribe((value) => {
        if (value && integranteSocioFormControl.value &&
          (integranteSocioFormControl.value == 2 || integranteSocioFormControl.value == 3)) {
          LineaDetalleComponent.setValidadoresIntegrantes(
            cantidadMinimaIntegrantesFormControl,
            cantidadMaximaIntegrantesFormControl,
            this.cantMinIntParametro,
            this.cantMaxIntParametro);
        }
        if (value && montoPrestableFormControl) {
          let valorMontoTope: number = Math.round(+montoPrestableFormControl.value / +cantidadMaximaIntegrantesFormControl.value);
          montoTopeFormControl.setValue(valorMontoTope > 0 ? valorMontoTope : 0);
        }
      }));

    this.subscripciones.add(integranteSocioFormControl
      .valueChanges
      .debounceTime(100)
      .subscribe((value) => {
        this.cargarLabelCuotaSolidaria();
        if (value) {
          if (+value === 1) {
            montoTopeFormControl.setValue(null);
            montoTopeFormControl.disable();
            montoTopeFormControl.clearValidators();
            cantidadMinimaIntegrantesFormControl.setValue(1);
            cantidadMinimaIntegrantesFormControl.disable();
            cantidadMaximaIntegrantesFormControl.setValue(1);
            cantidadMaximaIntegrantesFormControl.disable();
          } else {
            if (+value === 2) {
              montoTopeFormControl.setValue(null);
              montoTopeFormControl.disable();
              cantidadMinimaIntegrantesFormControl.enable();
              cantidadMinimaIntegrantesFormControl.clearValidators();
              cantidadMinimaIntegrantesFormControl.setValue(this.cantMinIntParametro.valor);
              cantidadMaximaIntegrantesFormControl.enable();
              cantidadMaximaIntegrantesFormControl.clearValidators();
              cantidadMaximaIntegrantesFormControl.setValue(this.cantMaxIntParametro.valor);
              LineaDetalleComponent.setValidadoresIntegrantes(
                cantidadMinimaIntegrantesFormControl,
                cantidadMaximaIntegrantesFormControl,
                this.cantMinIntParametro,
                this.cantMaxIntParametro);
            } else {
              if (+value === 3) {
                montoTopeFormControl.setValue(null);
                montoTopeFormControl.updateValueAndValidity();
                cantidadMinimaIntegrantesFormControl.enable();
                cantidadMinimaIntegrantesFormControl.clearValidators();
                cantidadMinimaIntegrantesFormControl.setValue(this.cantMinIntParametro.valor);
                cantidadMaximaIntegrantesFormControl.enable();
                cantidadMaximaIntegrantesFormControl.clearValidators();
                cantidadMaximaIntegrantesFormControl.setValue(this.cantMaxIntParametro.valor);

                montoTopeFormControl
                  .valueChanges
                  .debounceTime(100)
                  .subscribe((montoTope) => {
                    if (montoTope && montoPrestableFormControl.value) {
                      montoTopeFormControl
                        .setValidators(Validators.compose([
                          Validators.required, Validators.maxLength(8), CustomValidators.number,
                          CustomValidators.maxValue(+montoPrestableFormControl.value)
                        ]));
                    }
                  });
                LineaDetalleComponent.setValidadoresIntegrantes(
                  cantidadMinimaIntegrantesFormControl,
                  cantidadMaximaIntegrantesFormControl,
                  this.cantMinIntParametro,
                  this.cantMaxIntParametro);
              }
            }
          }
          montoTopeFormControl.markAsDirty();
          cantidadMinimaIntegrantesFormControl.markAsDirty();
          cantidadMaximaIntegrantesFormControl.markAsDirty();
        }
      }));

    if (+integranteSocioFormControl.value === 1) {
      montoTopeFormControl.reset({value: 0, disabled: true});
      cantidadMinimaIntegrantesFormControl.reset({value: 1, disabled: true});
      cantidadMaximaIntegrantesFormControl.reset({value: 1, disabled: true});
    }
  }

  public agregar(): void {
    this.detalleLinea.emit(LineaDetalleComponent.obtenerDetalleLinea(this.form));
    this.form.reset();
  }

  public confirmarApoderado(confirmacion: boolean) {
    this.form.get('apoderado').setValue(confirmacion, {emitEvent: false});
  }

  public static setValidadoresIntegrantes(cantidadMinima: AbstractControl, cantidadMaxima: AbstractControl,
                                          cantMinIntParametro: VigenciaParametro, cantMaxIntParametro: VigenciaParametro) {
    let max;
    let min;
    cantidadMinima.clearValidators();
    cantidadMaxima.clearValidators();

    if (+cantidadMaxima.value < +cantMaxIntParametro.valor) {
      max = +cantidadMaxima.value;
    } else {
      max = +cantMaxIntParametro.valor;
    }
    if (cantidadMaxima.value) {
      cantidadMinima.setValidators(
        Validators.compose([
          Validators.required, Validators.maxLength(3),
          CustomValidators.minValue(+cantMinIntParametro.valor),
          CustomValidators.maxValue(max)
        ]));
    } else {
      cantidadMinima.setValidators(
        Validators.compose([
          Validators.required, Validators.maxLength(3),
          CustomValidators.minValue(+cantMinIntParametro.valor),
          CustomValidators.maxValue(+cantMaxIntParametro.valor)
        ]));
    }

    if (+cantidadMinima.value < +cantMaxIntParametro.valor) {
      min = +cantidadMinima.value;
    } else {
      min = +cantMaxIntParametro.valor;
    }
    if (cantidadMinima.value) {
      cantidadMaxima.setValidators(
        Validators.compose([
          Validators.required, Validators.maxLength(3),
          CustomValidators.minValue(min),
          CustomValidators.maxValue(+cantMaxIntParametro.valor)
        ]));
    } else {
      cantidadMaxima.setValidators(
        Validators.compose([
          Validators.required, Validators.maxLength(3),
          CustomValidators.minValue(+cantMinIntParametro.valor),
          CustomValidators.maxValue(+cantMaxIntParametro.valor)
        ]));
    }

    cantidadMinima.updateValueAndValidity();
    cantidadMaxima.updateValueAndValidity();
  }

  public static obtenerDetalleLinea(formGroup: FormGroup): DetalleLineaPrestamo {
    let formModel = formGroup.getRawValue();
    let detalle = new DetalleLineaPrestamo();

    detalle.id = formModel.id;
    detalle.apoderado = formModel.apoderado;
    detalle.montoPrestable = formModel.montoPrestable;
    detalle.montoTope = formModel.montoTope;
    detalle.cantidadMinimaIntegrantes = formModel.cantidadMinimaIntegrantes;
    detalle.cantidadMaximaIntegrantes = formModel.cantidadMaximaIntegrantes;
    detalle.plazoDevolucion = formModel.plazoDevolucion;
    detalle.valorCuotaSolidaria = formModel.valorCuotaSolidaria;
    detalle.integranteSocio = new IntegranteSocio(formModel.integranteSocio);
    detalle.tipoFinanciamiento = new TipoFinanciamiento(formModel.tipoFinanciamiento);
    detalle.tipoGarantia = new TipoGarantia(formModel.tipoGarantia);
    detalle.tipoInteres = new TipoInteres(formModel.tipoInteres);
    detalle.visualizacion = formModel.visualizacion;
    detalle.convenioRecupero = new Convenio(formModel.idConvenioRecupero);
    detalle.convenioPago = new Convenio(formModel.idConvenioPago);
    return detalle;
  }

  public cargarParametros(): void {
    this.parametroService
      .obtenerVigenciaParametro(new VigenciaParametroConsulta(2))
      .subscribe((min) => {
        this.cantMinIntParametro = min;
        this.parametroService
          .obtenerVigenciaParametro(new VigenciaParametroConsulta(3))
          .subscribe((max) => {
            this.cantMaxIntParametro = max;
            this.inicializarForm();
            this.cargarLabelCuotaSolidaria();
          });
      });
  }

  public ngOnDestroy(): void {
    this.subscripciones.unsubscribe();
  }

  public contarDecimales(montoEstimado: number): number {
    var match = ('' + montoEstimado).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
    if (!match) {
      return 0;
    }
    return Math.max(0, (match[1] ? match[1].length : 0) - (match[2] ? +match[2] : 0));
  }

  public truncar(x: any, posiciones: any = 0) {
    let s = x.toString();
    let decimalLength = s.indexOf('.') + 1;
    let numStr = s.substr(0, decimalLength + posiciones);
    return Number(numStr);
  }
}
