import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { InversionRealizada } from '../modelo/inversion-realizada.model';
import { ItemInversion } from '../modelo/item-inversion.model';
import { FormulariosService } from '../formularios.service';

@Component({
  selector: 'bg-apartado-inversiones-emprendimiento',
  templateUrl: './apartado-inversiones-emprendimiento.component.html',
  styleUrls: ['./apartado-inversiones-emprendimiento.component.scss']
})

export class ApartadoInversionesEmprendimientoComponent implements OnInit {

  @Output()
  public inversionesSalida = new EventEmitter<InversionRealizada[]>();
  @Output()
  public detallesParaBorrar = new EventEmitter<number[]>();
  @Input()
  public inversionesRealizadasInput: InversionRealizada[] = [];
  @Input()
  public descripcionTotalResultado: string = '';
  @Input()
  public editable: boolean = true;

  public form: FormGroup;
  public inversionRealizada: InversionRealizada = new InversionRealizada();
  public inversionesRealizadas: InversionRealizada[] = [];
  public itemsInversion: ItemInversion[] = [];
  public descripcionHabilitada: boolean = false;
  public permitirAgregar: boolean = false;
  public listaIdsParaEliminar: number[] = [];

  constructor(private formularioService: FormulariosService) {
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerItemsInversion();
    if (this.inversionesRealizadasInput && this.inversionesRealizadasInput.length > 0) {
      this.inversionesRealizadas = this.inversionesRealizadasInput;
    }
    if (!this.editable) {
      this.form.disable();
    }
  }

  public crearForm(): void {
    let observaciones = new FormControl(this.inversionRealizada.observaciones, Validators.maxLength(200));
    let idItemInversion = new FormControl(this.inversionRealizada.idItemInversion, Validators.required);

    idItemInversion.valueChanges
      .distinctUntilChanged()
      .subscribe((value) => {
        if (value == 99) {
          this.descripcionHabilitada = true;
          observaciones.setValidators(Validators.compose([Validators.maxLength(200), Validators.required]));
          observaciones.updateValueAndValidity();
        } else {
          this.descripcionHabilitada = false;
          observaciones.setValidators(Validators.maxLength(200));
          observaciones.setValue('');
          observaciones.updateValueAndValidity();
        }
      });

    let precioNuevos = new FormControl(this.inversionRealizada.precioNuevos,
      Validators.compose([
        Validators.maxLength(8),
        CustomValidators.minDecimalValue(0.1),
        CustomValidators.decimalNumberWithTwoDigits]));
    this.validacionDatos(precioNuevos, 'cantidadNuevos', 'precioUsados', 'cantidadUsados');

    let precioUsados = new FormControl(this.inversionRealizada.precioUsados,
      Validators.compose([
        Validators.maxLength(8),
        CustomValidators.minDecimalValue(0.1),
        CustomValidators.decimalNumberWithTwoDigits]));
    this.validacionDatos(precioUsados, 'cantidadUsados', 'precioNuevos', 'cantidadNuevos');

    let cantidadUsados = new FormControl(this.inversionRealizada.cantidadUsados,
      Validators.compose([
        Validators.maxLength(8),
        CustomValidators.minDecimalValue(1),
        CustomValidators.decimalNumberWithTwoDigits]));
    this.validacionDatos(cantidadUsados, 'precioUsados', 'precioNuevos', 'cantidadNuevos');

    let cantidadNuevos = new FormControl(this.inversionRealizada.cantidadNuevos,
      Validators.compose([
        Validators.maxLength(8),
        CustomValidators.minDecimalValue(1),
        CustomValidators.decimalNumberWithTwoDigits]));
    this.validacionDatos(cantidadNuevos, 'precioNuevos', 'precioUsados', 'cantidadUsados');

    this.form = new FormGroup({
      idItemInversion: idItemInversion,
      descripcionInversion: observaciones,
      cantidadNuevos: cantidadNuevos,
      cantidadUsados: cantidadUsados,
      precioNuevos: precioNuevos,
      precioUsados: precioUsados
    });
  }

  public validacionDatos(formControl: FormControl, nombreFormControl: string, nombrePrecioCruzado: string, nombreCantidadCruzado: string) {
    formControl.valueChanges
      .distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          this.permitirAgregar = this.form.get(nombreFormControl).value && this.validacionDatosCruzados(nombrePrecioCruzado, nombreCantidadCruzado, false);
        } else {
          this.permitirAgregar = !this.form.get(nombreFormControl).value && this.validacionDatosCruzados(nombrePrecioCruzado, nombreCantidadCruzado, true);
        }
      });
  }

  public validacionDatosCruzados(precioString: string, cantidadString: string, soloConValor: boolean): boolean {
    let precioValor = this.form.get(precioString).value;
    let cantidadValor = this.form.get(cantidadString).value;

    if (soloConValor) {
      return (precioValor != '' && cantidadValor != '');
    }
    else {
      return ((precioValor == '' && cantidadValor == '') || (precioValor != '' && cantidadValor != ''));
    }
  }

  public obtenerItemsInversion() {
    this.formularioService.obtenerItemsInversion()
      .subscribe((resultados) => {
        this.itemsInversion = resultados;
      });
  }

  public agregarInversion() {
    let inversionCargada = this.obtenerInversion();
    this.inversionesRealizadas.push(inversionCargada);
    this.inversionesSalida.emit(this.inversionesRealizadas);
    this.crearForm();
  }

  public obtenerNombreItem(idItem: number): string {
    return this.itemsInversion.find((item) => item.id === idItem).descripcion;
  }

  public obtenerInversion(): InversionRealizada {
    let formModel = this.form.value;

    return new InversionRealizada(
      null,
      null,
      formModel.idItemInversion,
      null,
      formModel.observaciones ? formModel.observaciones : this.obtenerNombreItem(formModel.idItemInversion),
      formModel.cantidadNuevos,
      formModel.cantidadUsados,
      formModel.precioNuevos,
      formModel.precioUsados);
  }

  public eliminarDetalle(indice: number, id: number) {
    this.cargarDetallesParaEliminar(id);
    this.inversionesRealizadas.splice(indice, 1);
    this.inversionesSalida.emit(this.inversionesRealizadas);
    this.detallesParaBorrar.emit(this.listaIdsParaEliminar);
  }

  public cargarDetallesParaEliminar(id: number) {
    if(!id) return;
    let detalle = this.inversionesRealizadasInput.find((x) => x.id === id);
    if (detalle) {
      this.listaIdsParaEliminar.push(detalle.id);
    }
  }

  public sumarCantidades(cant1, cant2): number {
    if (!cant1 && !cant2) {
      return 0;
    }
    else if (!cant2) {
      return Number(cant1);
    }
    else if (!cant1) {
      return Number(cant2);
    }
    else {
      return Number(cant1) + Number(cant2);
    }
  }

  public sumarPrecios(): number {
    let total = 0;
    this.inversionesRealizadas.forEach((inv) => {
      total += this.calcularTotal(inv);
    });

    return total;
  }

  public calcularTotal(inversion: InversionRealizada){
    if(!inversion) return 0;
    let nuevos = Number(inversion.cantidadNuevos) * Number(inversion.precioNuevos);
    let usados = Number(inversion.cantidadUsados) * Number(inversion.precioUsados);
    return nuevos + usados;
  }
}
