import { Component, Input } from '@angular/core';
import { FormularioPrestamo } from '../shared/modelo/formularios-prestamo.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'bg-modal-formularios-prestamo',
  templateUrl: './modal-formularios-prestamo.component.html',
  styleUrls: ['./modal-formularios-prestamo.component.scss'],
})

export class ModalFormulariosPrestamoComponent {
  @Input() public formularioResultados: FormularioPrestamo[] = [];

  constructor(public activeModal: NgbActiveModal) {
  }

  public cancelar(): void {
    this.activeModal.close();
  }
}
