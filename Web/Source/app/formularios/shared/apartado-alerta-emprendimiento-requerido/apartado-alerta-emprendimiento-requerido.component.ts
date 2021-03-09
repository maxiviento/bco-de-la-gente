import { Component, Input } from '@angular/core';
import { Emprendimiento } from '../modelo/emprendimiento.model';

@Component({
  selector: 'bg-apartado-alerta-emprendimiento-requerido',
  templateUrl: './apartado-alerta-emprendimiento-requerido.component.html',
  styleUrls: ['./apartado-alerta-emprendimiento-requerido.component.scss']
})

export class ApartadoAlertaEmprendimientoRequeridoComponent {
  @Input()
  public emprendimiento: Emprendimiento = new Emprendimiento();
  @Input()
  public marginTop: boolean = false;
  @Input()
  public clasesCss: string[] = [];
  public mensaje: string = 'Es requerido cargar los datos del emprendimiento para poder guardar los datos ingresados en este cuadrante.';

  public clasesParametro(): string {
    if (!this.clasesCss) return '';
    if (!this.clasesCss.length) return '';
    let res = '';
    for (let i = 0; i < this.clasesCss.length; i++) {
      res += this.clasesCss[i];
      if (i != this.clasesCss.length - 1) res += ' ';
    }
    return res;
  }
}
