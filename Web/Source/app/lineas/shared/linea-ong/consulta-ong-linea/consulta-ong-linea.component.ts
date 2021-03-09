import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { OngLinea } from '../../modelo/ong-linea.model';

@Component({
  selector: 'bg-consulta-ong-linea',
  templateUrl: './consulta-ong-linea.component.html',
  styleUrls: ['./consulta-ong-linea.component.scss']
})

export class ConsultaOngLineaModal {
  @Input() public lsOngLinea: OngLinea[];
  @Input() public nombreLinea: string;

  constructor(public activeModal: NgbActiveModal) {
  }
}
