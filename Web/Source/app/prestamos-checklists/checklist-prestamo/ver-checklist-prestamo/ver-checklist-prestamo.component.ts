import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-ver-checklist-prestamo',
  templateUrl: './ver-checklist-prestamo.component.html'
})
export class VerChecklistComponent {
  public titulo: string = 'VER PRÉSTAMO';

  constructor(private titleService: Title) {
    this.titleService.setTitle('Ver préstamo ' + TituloBanco.TITULO);
  }
}
