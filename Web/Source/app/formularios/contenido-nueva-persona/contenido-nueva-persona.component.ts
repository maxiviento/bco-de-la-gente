import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Persona } from '../../shared/modelo/persona.model';
import AccionGrupoUnico from '../../grupo-unico/shared-grupo-unico/accion-grupo-unico.enum';

@Component({
  selector: 'ngb-nueva-persona-modal',
  templateUrl: 'contenido-nueva-persona.component.html',
})

export class ContenidoNuevaPersonaComponent {

  @Input()
  public persona: Persona;

  public urlNuevaPersona: AccionGrupoUnico = AccionGrupoUnico.PERSONA_NUEVA;

  constructor(public activeModal: NgbActiveModal) {

  }
}
