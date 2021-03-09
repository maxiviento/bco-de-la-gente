import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'ngbd-modal-content',
  templateUrl: './contenido-confirmacion.component.html',
  styleUrls: ['./contenido-confirmacion.component.scss'],
})
export class ContenidoConfirmacionComponent {
  @Input()
  public titulo: string = 'Confirmación';
  @Input()
  public mensaje: string;

  constructor(public activeModal: NgbActiveModal) {
  }

}
