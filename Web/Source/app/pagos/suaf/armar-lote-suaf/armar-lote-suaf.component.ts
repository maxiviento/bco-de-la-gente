import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-armar-lote-suaf',
  templateUrl: './armar-lote-suaf.component.html',
  styleUrls: ['./armar-lote-suaf.component.scss'],
})

export class ArmarLoteSuafComponent {
  constructor(private titleService: Title) {
    this.titleService.setTitle('Nuevo lote de SUAF ' + TituloBanco.TITULO);
  }
}
