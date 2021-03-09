import { Component, OnInit } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../../shared/notificacion.service';
import { FormulariosService } from '../../shared/formularios.service';
import { PatrimonioSolicitante } from '../../shared/modelo/patrimonio-solicitante.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-patrimonio-solicitante',
  templateUrl: './patrimonio-solicitante.component.html',
  styleUrls: ['./patrimonio-solicitante.component.scss'],
})

export class PatrimonioSolicitanteComponent extends CuadranteFormulario implements OnInit {
  public patrimonioSolicitante: PatrimonioSolicitante;
  public form: FormGroup;
  public total: number;

  constructor(private formulariosService: FormulariosService,
              private notificacionService: NotificacionService,
              private fb: FormBuilder) {
    super();
  }

  public ngOnInit(): void {
    if (this.formulario.patrimonioSolicitante) {
      this.patrimonioSolicitante = this.formulario.patrimonioSolicitante;
    } else {
      this.patrimonioSolicitante = new PatrimonioSolicitante(false,
        0, false, 0, '0',
        0, 0, 0);
    }
    this.crearForm();
    this.calcularTotal();
  }

  private suscribirRadios(): void {
    this.form.get('tieneInmueble').valueChanges.distinctUntilChanged()
      .subscribe((value) => {
          if (value && this.editable) {
            this.form.get('valorInmueble').enable();
            this.form.get('valorInmueble').reset();
          } else {
            this.form.get('valorInmueble').disable();
            if (!value) {
              this.form.get('valorInmueble').patchValue(0);
            }
            this.calcularTotal();
          }
        }
      );

    this.form.get('tieneVehiculo').valueChanges.distinctUntilChanged()
      .subscribe((value) => {
          if (value && this.editable) {
            this.form.get('cantVehiculos').enable();
            this.form.get('cantVehiculos').reset();
            this.form.get('modeloVehiculos').enable();
            this.form.get('modeloVehiculos').reset();
            this.form.get('valorVehiculos').enable();
            this.form.get('valorVehiculos').reset();
          } else {
            this.form.get('cantVehiculos').disable();
            this.form.get('modeloVehiculos').disable();
            this.form.get('valorVehiculos').disable();
            if (!value) {
              this.form.get('cantVehiculos').patchValue(0);
              this.form.get('modeloVehiculos').patchValue(0);
              this.form.get('valorVehiculos').patchValue(0);
            }
            this.calcularTotal();
          }
        }
      );
  }

  private crearForm() {
    this.form = this.fb.group({
      tieneInmueble: [
        this.patrimonioSolicitante.inmueblePropio ? this.patrimonioSolicitante.inmueblePropio : false,
        Validators.required],
      valorInmueble: [
        { value: this.patrimonioSolicitante.valorInmueble, disabled: !this.patrimonioSolicitante.inmueblePropio },
        Validators.compose([
          Validators.required,
          CustomValidators.number,
          CustomValidators.minValue(1),
          Validators.maxLength(9),])],
      tieneVehiculo: [
        this.patrimonioSolicitante.vehiculoPropio ? this.patrimonioSolicitante.vehiculoPropio : false,
        Validators.required],
      cantVehiculos: [
        { value: this.patrimonioSolicitante.cantVehiculos, disabled: !this.patrimonioSolicitante.vehiculoPropio },
        Validators.compose([
          Validators.required,
          CustomValidators.number,
          CustomValidators.minValue(1),
          Validators.maxLength(3)])],
      modeloVehiculos: [
        { value: this.patrimonioSolicitante.modeloVehiculos, disabled: !this.patrimonioSolicitante.vehiculoPropio },
        Validators.compose([
          Validators.required,
          CustomValidators.validTextAndNumbers,
          Validators.maxLength(100)])],
      valorVehiculos: [
        { value: this.patrimonioSolicitante.valorVehiculos, disabled: !this.patrimonioSolicitante.vehiculoPropio },
        Validators.compose([
          Validators.required,
          CustomValidators.number,
          CustomValidators.minValue(1),
          Validators.maxLength(9)])],
      valorInmueblesMasVehiculos: [
        this.patrimonioSolicitante.valorInmueblesMasVehiculos],
      valorDeudas: [
        this.patrimonioSolicitante.valorDeudas,
        Validators.compose([
          CustomValidators.number,
          CustomValidators.minValue(0),
        Validators.maxLength(9)])],
    });
    this.suscribirRadios();
    if (!this.editable) {
      this.form.disable();
    }
  }

  public calcularTotal() {
    if (this.editable) {
      this.actualizarPatrimonioSolicitante();
    }
    this.calculoTotal();
  }

  private calculoTotal() {
    this.patrimonioSolicitante.valorInmueblesMasVehiculos = 0;
    this.total = 0;
    if (this.patrimonioSolicitante.valorInmueble) {
      this.patrimonioSolicitante.valorInmueblesMasVehiculos += +this.patrimonioSolicitante.valorInmueble;
      this.total = this.patrimonioSolicitante.valorInmueblesMasVehiculos;
    }
    if (this.patrimonioSolicitante.valorVehiculos) {
      this.patrimonioSolicitante.valorInmueblesMasVehiculos += +this.patrimonioSolicitante.valorVehiculos;
      this.total = this.patrimonioSolicitante.valorInmueblesMasVehiculos;
    }
  }

  public actualizarDatos() {
    let patrimonio = this.actualizarPatrimonioSolicitante();
    if (this.formulario.idEstado === 3) {
      this.formulariosService.actualizarPatrimonioSolicitante(this.formulario.id, patrimonio)
        .subscribe(() => {
            this.patrimonioSolicitante = patrimonio;
            this.formulario.patrimonioSolicitante = this.patrimonioSolicitante;
          },
          (errores) => {
            this.notificacionService.informar(errores, true);
          });
    } else {
      this.formulariosService.actualizarPatrimonioSolicitanteAsociativas(this.formulario.idAgrupamiento, patrimonio).subscribe(() => {
          this.patrimonioSolicitante = patrimonio;
          this.formulario.patrimonioSolicitante = this.patrimonioSolicitante;
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    }
  }

  private actualizarPatrimonioSolicitante(): PatrimonioSolicitante {
    let formModel = this.form.value;
    this.patrimonioSolicitante.valorInmueble = formModel.valorInmueble ? formModel.valorInmueble : 0;
    this.patrimonioSolicitante.cantVehiculos = formModel.cantVehiculos ? formModel.cantVehiculos : 0;
    this.patrimonioSolicitante.modeloVehiculos = formModel.modeloVehiculos ? formModel.modeloVehiculos : 0;
    this.patrimonioSolicitante.valorVehiculos = formModel.valorVehiculos ? formModel.valorVehiculos : 0;
    this.patrimonioSolicitante.valorDeudas = formModel.valorDeudas ? formModel.valorDeudas : 0;
    this.patrimonioSolicitante.inmueblePropio = formModel.tieneInmueble;
    this.patrimonioSolicitante.vehiculoPropio = formModel.tieneVehiculo;
    return this.patrimonioSolicitante;
  }

  public esValido(): boolean {
    if (!this.editable) {
      return true;
    } else {
      return this.form.valid;
    }
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }
}
