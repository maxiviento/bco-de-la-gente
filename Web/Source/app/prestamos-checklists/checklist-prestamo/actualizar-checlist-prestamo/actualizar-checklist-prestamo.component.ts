import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-actualizar-checklist-prestamo',
  templateUrl: './actualizar-checklist-prestamo.component.html'
})
export class ActualizarChecklistComponent {
  public titulo: string = 'ACTUALIZAR PRÉSTAMO';

  constructor(private titleService: Title) {
    this.titleService.setTitle('Editar préstamo ' + TituloBanco.TITULO);
  }
}
