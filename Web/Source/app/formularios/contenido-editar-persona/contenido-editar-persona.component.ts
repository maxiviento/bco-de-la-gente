import {Component, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Persona} from '../../shared/modelo/persona.model';
import AccionGrupoUnico from '../../grupo-unico/shared-grupo-unico/accion-grupo-unico.enum';

@Component({
  selector: 'ngb-editar-persona-modal',
  templateUrl: 'contenido-editar-persona.component.html',
})

export class ContenidoEditarPersonaComponent {

  @Input()
  public persona: Persona;

  public urlEditarPersona: AccionGrupoUnico = AccionGrupoUnico.PERSONA_MODIFICACION;

  constructor(public activeModal: NgbActiveModal) {

  }
}
