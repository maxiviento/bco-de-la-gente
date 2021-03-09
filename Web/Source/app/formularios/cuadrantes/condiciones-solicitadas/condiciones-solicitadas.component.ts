import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CondicionesSolicitadas } from '../../shared/modelo/condiciones-solicitadas.model';
import { FormulariosService } from '../../shared/formularios.service';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { CuadranteFormulario } from '../cuadrante-formulario';

@Component({
  selector: 'bg-condiciones-solicitadas',
  templateUrl: './condiciones-solicitadas.component.html',
  styleUrls: ['./condiciones-solicitadas.component.scss'],
})

export class CondicionesSolicitadasComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private formularioService: FormulariosService) {
    super();
  }

  public actualizarDatos() {
    let condicionesSolicitadas = this.obtenerCondicionesSolicitadas();
    if (this.formulario.idEstado === 3 && this.editable) {
      this.formularioService.actualizarCondicionesSolicitadas(this.formulario.id, condicionesSolicitadas)
        .subscribe(() => {
          this.formulario.condicionesSolicitadas = condicionesSolicitadas;
        });
    } else if (this.formulario.detalleLinea.cantidadMaximaIntegrantes > 1 && this.editable) {
      this.formularioService.actualizarCondicionesSolicitadasAsociativas(this.formulario.idAgrupamiento, condicionesSolicitadas)
        .subscribe(() => {
          this.formulario.condicionesSolicitadas = condicionesSolicitadas;
        });
    }
  }

  public esValido(): boolean {
    if (!this.editable) {  // estamos en el ver
      return true;
    } else {
      return this.form.valid;
    }
  }

  public ngOnInit(): void {
    if (!this.formulario.condicionesSolicitadas || (!this.formulario.condicionesSolicitadas.cantidadCuotas && !this.formulario.condicionesSolicitadas.montoSolicitado)) {
      this.formulario.condicionesSolicitadas = new CondicionesSolicitadas();
      this.formulario.condicionesSolicitadas.montoSolicitado = this.formulario.detalleLinea.montoTopeIntegrante;
      this.formulario.condicionesSolicitadas.cantidadCuotas = this.formulario.detalleLinea.plazoDevolucionMaximo;
    }

    this.crearForm();
    this.CalcularMontoDeCuota();
    if (!this.editable) {
      this.form.disable();
    }
    this.actualizarDatos();
  }

  public crearForm() {
    this.form = this.fb.group({
      montoSolicitado: [this.formulario.condicionesSolicitadas.montoSolicitado ? this.formulario.condicionesSolicitadas.montoSolicitado : this.formulario.detalleLinea.montoTopeIntegrante,
        Validators.compose([
          Validators.required,
          CustomValidators.number,
          CustomValidators.maxValue(this.formulario.detalleLinea.montoTopeIntegrante),
          CustomValidators.minValue(1)])],
      cantidadCuotas: [this.formulario.condicionesSolicitadas.cantidadCuotas,
        Validators.compose([
          Validators.required,
          CustomValidators.number,
          CustomValidators.maxValue(this.formulario.detalleLinea.plazoDevolucionMaximo),
          CustomValidators.minValue(1)])],
      montoEstimadoCuota: [this.formulario.condicionesSolicitadas.montoEstimadoCuota],
    });
  }

  public obtenerCondicionesSolicitadas(): CondicionesSolicitadas {
    this.actualizarCondicionesSolicitadas();
    return this.formulario.condicionesSolicitadas;
  }

  public actualizarCondicionesSolicitadas() {
    let formModel = this.form.value;
    this.formulario.condicionesSolicitadas.montoSolicitado = formModel.montoSolicitado;
    this.formulario.condicionesSolicitadas.cantidadCuotas = formModel.cantidadCuotas;
    this.formulario.condicionesSolicitadas.montoEstimadoCuota = formModel.montoEstimadoCuota;
  }

  public CalcularMontoDeCuota(): void {
    if (this.form.valid) {
      this.actualizarCondicionesSolicitadas();
      if (this.formulario.condicionesSolicitadas.montoSolicitado && this.formulario.condicionesSolicitadas.cantidadCuotas) {
        let montoEstimado = this.formulario.condicionesSolicitadas.montoSolicitado / this.formulario.condicionesSolicitadas.cantidadCuotas;

        if (this.contarDecimales(montoEstimado) > 1) {
          this.formulario.condicionesSolicitadas.montoEstimadoCuota = this.truncar(montoEstimado, 2);
        } else {
          this.formulario.condicionesSolicitadas.montoEstimadoCuota = montoEstimado;
        }

        this.crearForm();
      }
    }
  }

  public contarDecimales(montoEstimado: number): number {
    var match = ('' + montoEstimado).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
    if (!match) {
      return 0;
    }
    return Math.max(0, (match[1] ? match[1].length : 0) - (match[2] ? +match[2] : 0));
  }

  public truncar(x: any, posiciones: any = 0) {
    var s = x.toString();
    var decimalLength = s.indexOf('.') + 1;
    var numStr = s.substr(0, decimalLength + posiciones);
    return Number(numStr);
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }
}

