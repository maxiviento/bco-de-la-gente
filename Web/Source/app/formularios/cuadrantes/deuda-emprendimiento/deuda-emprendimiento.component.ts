import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { InversionRealizada } from '../modelo/inversion-realizada.model';
import { ItemInversion } from '../modelo/item-inversion.model';
import { DeudaEmprendimiento } from '../../shared/modelo/deuda.emprendimiento';
import { FormulariosService } from '../../shared/formularios.service';
import { TipoDeuda } from '../../shared/modelo/tipo.deuda';
import { TipoDeudaEmprendimiento } from '../../shared/modelo/tipo-deuda.emprendimiento';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { NotificacionService } from '../../../shared/notificacion.service';

@Component({
  selector: 'bg-deuda-emprendimiento.component',
  templateUrl: './deuda-emprendimiento.component.html',
  styleUrls: ['./deuda-emprendimiento.component.scss']
})

export class DeudaEmprendimientoComponent extends CuadranteFormulario implements OnInit {

  public form: FormGroup;
  public deudasEmprendimiento: DeudaEmprendimiento[] = [];
  public deudaEmprendimiento: DeudaEmprendimiento = new DeudaEmprendimiento();
  public tiposDeuda: TipoDeudaEmprendimiento[] = [];

  constructor(private formularioService: FormulariosService,
              private notificacionService: NotificacionService) {
    super();
  }

  ngOnInit(): void {
    this.crearForm();
    this.obtenerTiposDeuda();
    if (this.formulario.deudasEmprendimiento) {
      this.deudasEmprendimiento = this.formulario.deudasEmprendimiento;
    } else {
      this.deudasEmprendimiento = [];
    }
    if (!this.editable) {
      this.form.disable();
    }
  }

  public crearForm(): void {

    let montoDeuda = new FormControl(this.deudaEmprendimiento.monto,
      Validators.compose([Validators.maxLength(8),
        CustomValidators.decimalNumberWithTwoDigits,
        Validators.required]));
    let idTipoDeudaEmprendimiento = new FormControl(this.deudaEmprendimiento.idTipoDeudaEmprendimiento, Validators.required);

    this.form = new FormGroup({
      idTipoDeudaEmprendimiento: idTipoDeudaEmprendimiento,
      monto: montoDeuda
    })
  }

  public obtenerTiposDeuda() {
    this.formularioService.obtenerTiposDeuda()
      .subscribe((resultados) => {
        this.tiposDeuda = resultados;
      })
  }

  public agregarDeuda() {
    let deudaCargada = this.obtenerInversion();
    this.deudasEmprendimiento.push(deudaCargada);
    this.limpiarCampos();
  }

  public obtenerInversion(): DeudaEmprendimiento {
    let formModel = this.form.value;

    return new DeudaEmprendimiento(null, null, formModel.monto, formModel.idTipoDeudaEmprendimiento);
  }

  public obtenerNombreTipoDeuda(idTipoDeuda: number): string {

    if (this.tiposDeuda.length == 0) {
      return "";
    }
    else {
      let tipoDeuda = this.tiposDeuda.find(item => item.id == idTipoDeuda);

      return tipoDeuda == undefined ? "" : tipoDeuda.descripcion;
    }
  }

  public eliminarDetalle(indice: number) {
    this.deudasEmprendimiento.splice(indice, 1);
  }

  public sumarMontos(): number {
    let total = 0;
    this.deudasEmprendimiento.forEach((deuda) => {
      total += (Number(deuda.monto));
    });

    return total;
  }

  public limpiarCampos() {
    this.form.get("monto").setValue(null);
    this.form.get("monto").markAsPristine();
    this.form.get("idTipoDeudaEmprendimiento").setValue("null");
  }

  actualizarDatos() {
    this.formularioService.actualizarDeudaEmprendimiento(this.formulario.id, this.deudasEmprendimiento)
      .subscribe(() => {
          this.formulario.deudasEmprendimiento = this.deudasEmprendimiento;
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  esValido(): boolean {
    return this.deudasEmprendimiento.length > 0;
  }

  inicializarDeNuevo(): boolean {
    return false;
  }

}
