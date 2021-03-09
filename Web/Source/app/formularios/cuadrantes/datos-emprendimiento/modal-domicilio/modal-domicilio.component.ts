import { Component, Input, HostListener } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificacionService } from '../../../../shared/notificacion.service';
@Component({
  selector: 'bg-modal-domicilio',
  templateUrl: './modal-domicilio.component.html',
})

export class ModalDomicilioComponent {
  private ESCAPE_KEYCODE = 27;
  @Input() public url: string;

  @HostListener('document: keydown', ['$event'])
  public handleKeyboardEvent(event: KeyboardEvent) {
    if (event.keyCode === this.ESCAPE_KEYCODE) {
      //this.activeModal.close();
      this.cerrar(false);
    }
  }

  constructor(private notificacionService: NotificacionService,
              private activeModal: NgbActiveModal) {
  }

  public cerrar(confirmar: boolean): void {
    this.activeModal.close({confirmar: confirmar});
  }
}
