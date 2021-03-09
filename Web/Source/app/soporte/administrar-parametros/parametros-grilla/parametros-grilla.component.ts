import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Parametro } from '../../modelo/parametro.model';

@Component({
  selector: 'cce-parametros-grilla',
  templateUrl: './parametros-grilla.component.html'
})

export class ParametrosGrillaComponent {
  @Input() public parametros: Parametro[];
  @Input() public mostrarFechaHasta: boolean;
  @Output() public modificado: EventEmitter<Parametro> = new EventEmitter<Parametro>();

  constructor() {
    this.parametros = [];
    this.mostrarFechaHasta = false;
  }

  public getValor(parametro : Parametro) : string {
    if(parametro.idTipoDato == 3){
      return parametro.valor == 'S' ? 'VERDADERO' : 'FALSO';
    }else{
      return parametro.valor;
    }

  }
}
