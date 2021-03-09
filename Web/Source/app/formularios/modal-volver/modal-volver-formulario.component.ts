import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'bg-modal-volver-formulario',
  templateUrl: './modal-volver-formulario.component.html',
  styleUrls: ['./modal-volver-formulario.component.scss'],
})

export class ModalVolverFormularioComponent {
  public title: string;
  public message: string;

  constructor(private activeModal: NgbActiveModal) {
    this.title = 'Atención';
    this.message = 'Los cambios que no hayan sido guardados se perderan';
  }

  public guardarYSalir() {
    this.activeModal.close(true);
  }

  public salir(): void {
    this.activeModal.close(false);
  }
}
