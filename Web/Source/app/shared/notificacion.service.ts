import { Injectable } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ContenidoInformativoComponent } from './contenidos/contenido-informativo.component';
import { ContenidoConfirmacionComponent } from './contenidos/contenido-confirmacion.component';

@Injectable()
export class NotificacionService {

  constructor(private modalService: NgbModal) {
  }

  public informar(mensajes: string[], error?: boolean, titulo?: string): NgbModalRef {
    let modalRef = this.modalService.open(ContenidoInformativoComponent, {backdrop: 'static'});
    modalRef.componentInstance.mensajes = mensajes;
    modalRef.componentInstance.error = error;
    modalRef.componentInstance.titulo = titulo;
    return modalRef;
  }

  public confirmar(mensaje: string): NgbModalRef {
    let modalRef = this.modalService.open(ContenidoConfirmacionComponent, {backdrop: 'static'});
    modalRef.componentInstance.error = false;

    modalRef.componentInstance.mensaje = mensaje;
    return modalRef;
  }

}
