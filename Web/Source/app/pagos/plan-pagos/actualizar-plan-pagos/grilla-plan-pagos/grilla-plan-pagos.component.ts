import { Component, OnInit } from '@angular/core';
import { DetallePlanDePago } from '../../../shared/modelo/detalle-plan-pago.model';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { PlanDePagoGrillaResultado } from '../../../shared/modelo/plan-de-pago-grilla-resultado.model';
import { isNullOrUndefined } from 'util';
import { FormularioPlanDePagoResultado } from '../../../shared/modelo/formulario-plan-de-pago-resultado.model';
import { isEmpty } from '../../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-grilla-plan-pagos',
  templateUrl: 'grilla-plan-pagos.component.html',
  styleUrls: ['grilla-plan-pagos.component.scss']
})

export class GrillaPlanPagosComponent implements OnInit {
  public planes: PlanDePagoGrillaResultado[] = [];
  public formularios: FormularioPlanDePagoResultado[] = [];
  public planSeleccionado: FormularioPlanDePagoResultado = new FormularioPlanDePagoResultado();
  public form: FormGroup;
  public filas: number[] = [];
  public tablas: number[] = [];
  private numeroTablas: number = 3;
  public esUnModal: boolean = false;

  constructor(private fb: FormBuilder) {
    this.planes = [];
    for (let i = 1; i <= this.numeroTablas; i++) {
      this.tablas.push(i);
    }
    this.setFilas();
  }

  public ngOnInit() {
    this.crearForm();
    this.subscripcionPlan();

    if (this.esUnModal && this.planes.length > 0) {
      this.formularios = this.planes[0].formularios;
      this.planSeleccionado = this.planes[0].formularios[0];
      this.nuevoPlan(0);
    }
  }

  public crearForm() {
    this.form = this.fb.group({
      cuota: [''],
      formulario: ['']
    });
  }

  private subscripcionPlan() {
    let plan = this.form.get('cuota') as FormControl;
    let formulario = this.form.get('formulario') as FormControl;

    plan.valueChanges
      .subscribe(() => {
        if (plan.value >= 0) {
          if (plan && formulario && !isEmpty(plan.value) && this.planes) {
            this.formularios = this.planes[plan.value].formularios;
            formulario.setValue(0);
          }
        } else {
          this.formularios = [];
          this.setFilas();
        }
      });

    formulario.valueChanges
      .subscribe(() => {
        if (formulario.value >= 0) {
          this.nuevoPlan(formulario.value);
        }
      });
  }

  public inicializarCuotas(planes: PlanDePagoGrillaResultado[]): void {
    this.planes = planes;
    this.form.get('cuota').setValue(0);
    this.setFilas();
  }

  public nuevoPlan(index: number = 0) {
    this.planSeleccionado = this.formularios[index];
    this.setFilas();
  }

  public limpiarGrilla() {
    this.planes = [];
    this.setFilas();
  }

  public setFilas() {
    this.filas = [];
    for (let i = 0; i <= this.cantidadPorTabla() - 1; i++) {
      this.filas.push(i);
    }
  }

  public obtenerDetalles(): DetallePlanDePago[] {
    if (!this.planSeleccionado.detalles) {
      return [];
    }
    return this.planSeleccionado.detalles;
  }

  public cantidadPorTabla(): number {
    if (!this.planSeleccionado) {
      return 0;
    }
    if (!this.planSeleccionado.detalles) {
      return 0;
    }
    return Math.ceil(this.planSeleccionado.detalles.length / this.numeroTablas);
  }

  /* Método para obtener cada detalle de la tabla de cuotas obtenido del listado de detalles y mostrado en la tabla anidada */
  public getDetalle(tabla: number, index: number): DetallePlanDePago {
    if (!this.planes) {
      return null;
    }

    let i = index;

    // TODO: Si el n° de tabla es mayor a 1 debe considerar sumar los index anteriores
    if (tabla > 1) {
      i = index + (this.filas.length * (tabla - 1)) - (tabla - 1);
    }

    // TODO: Según cada tabla se verifica que el detalle buscado no exceda la pila de detalles y sino la devuelve
    if ((i + tabla) <= this.planSeleccionado.detalles.length) {
      return this.planSeleccionado.detalles[i + tabla - 1];
    }
    return new DetallePlanDePago();
  }

  public cuotaClass(detalle: DetallePlanDePago): string {
    let clases = [];
    let res = '';

    if (detalle.extraAlPlan) {
      clases.push('cuota-extra');
    } else {
      if (!detalle.fechaPago && detalle.nroCuota) {
        clases.push('cuota-superada');
      }
    }

    if (!clases.length) {
      return null;
    }
    for (let i = 0; i < clases.length; i++) {
      res += clases[i];
      if (i !== clases.length - 1) {
        res += ' ';
      }
    }
    return res;
  }

  public montoMostrable(detalle: DetallePlanDePago): number {
    if (detalle.extraAlPlan) {
      return detalle.montoCuotaReal;
    }
    return detalle.montoCuota;
  }

  public mostrarNombreCuotas(plan: PlanDePagoGrillaResultado): string {
    if (plan.montoCuota) {
      return plan.descripcion;
    }
    return 'Cuotas especiales';
  }

  public montoClass(): string {
    if (!this.planSeleccionado) {
      return null;
    }
    if (isNullOrUndefined(this.planSeleccionado.montoPagado) || isNullOrUndefined(this.planSeleccionado.montoTotal)) {
      return null;
    }
    if (this.planSeleccionado.montoTotal === this.planSeleccionado.montoPagado) {
      return 'monto-equilibrado';
    }
    if (this.planSeleccionado.montoTotal > this.planSeleccionado.montoPagado) {
      return 'monto-inferior';
    }
    if (this.planSeleccionado.montoTotal < this.planSeleccionado.montoPagado) {
      return 'monto-superior';
    }
    return null;
  }

  public mensaje(): string {
    if (this.planSeleccionado) {
      if (this.planSeleccionado.montoTotal < this.planSeleccionado.montoPagado) {
        return 'Monto sobrante: ';
      } else {
        return 'Monto pagado: ';
      }
    }
  }
}
