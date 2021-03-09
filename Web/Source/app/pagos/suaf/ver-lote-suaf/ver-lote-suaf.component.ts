import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-ver-lote-suaf',
  templateUrl: './ver-lote-suaf.component.html',
  styleUrls: ['./ver-lote-suaf.component.scss'],
})

export class VerLoteSuafComponent {

  constructor(private titleService: Title) {
    this.titleService.setTitle('Ver lote SUAF ' + TituloBanco.TITULO);
  }
}
