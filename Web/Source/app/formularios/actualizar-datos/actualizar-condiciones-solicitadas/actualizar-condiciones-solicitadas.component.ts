import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CondicionesSolicitadas} from '../../shared/modelo/condiciones-solicitadas.model';
import {FormulariosService} from '../../shared/formularios.service';
import {CustomValidators} from '../../../shared/forms/custom-validators';
import {Formulario} from '../../shared/modelo/formulario.model';
import {DetalleLineaFormulario} from '../../shared/modelo/detalle-linea-formulario.model';
import {Router} from '@angular/router';
import {NotificacionService} from "../../../shared/notificacion.service";

@Component({
  selector: 'bg-actualizar-condiciones-solicitadas',
  templateUrl: './actualizar-condiciones-solicitadas.component.html',
  styleUrls: ['./actualizar-condiciones-solicitadas.component.scss'],
})

export class ActualizarCondicionesSolicitadasComponent implements OnInit {
  public form: FormGroup;
  public formulario: Formulario = new Formulario();
  public condicionesSolicitadas: CondicionesSolicitadas = new CondicionesSolicitadas();
  public detalleLinea: DetalleLineaFormulario = new DetalleLineaFormulario();
  @Input() public idFormulario: number;

  constructor(private fb: FormBuilder,
              private formularioService: FormulariosService,
              private notificacionService: NotificacionService,
              private router: Router) {
  }

  public ngOnInit(): void {
    this.formulario.condicionesSolicitadas = this.condicionesSolicitadas;
    this.formulario.detalleLinea = this.detalleLinea;
    this.crearForm();
    this.formularioService.obtenerCondicionesSolicitadas(this.idFormulario).subscribe((res) => {
      this.formulario = res;
      this.crearForm();
    });
    this.CalcularMontoDeCuota();
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

  public actualizarCondicionesSolicitadas() {
    let condicionesSolcitadas = new CondicionesSolicitadas();
    condicionesSolcitadas.montoSolicitado = this.form.get('montoSolicitado').value;
    condicionesSolcitadas.cantidadCuotas = this.form.get('cantidadCuotas').value;
    this.formularioService.modificarCondicionesSolicitadas(this.idFormulario, condicionesSolcitadas).subscribe((mensaje) => {
      if (mensaje) {
        this.notificacionService
          .informar([mensaje])
          .result.then(() => {
         this.form.disable();
        });
      }
    }, (error) => {
      this.notificacionService
        .informar([error])
    });
  }

  public CalcularMontoDeCuota(): void {
    if (this.form.valid) {
      if (this.form.get('montoSolicitado').value && this.form.get('cantidadCuotas').value) {
        let montoEstimado = this.form.get('montoSolicitado').value / this.form.get('cantidadCuotas').value;
        if (this.contarDecimales(montoEstimado) > 1) {
          this.form.get('montoEstimadoCuota').setValue(this.truncar(montoEstimado, 2));
        } else {
          this.form.get('montoEstimadoCuota').setValue(montoEstimado);
        }
      }
    }
  }

  public contarDecimales(montoEstimado: number): number {
    let match = ('' + montoEstimado).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
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

  public volver() {
    this.router.navigate(['formularios']);
  }
}
