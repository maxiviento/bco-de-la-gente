import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-carga-manual-devengado',
  templateUrl: './carga-manual-devengado.component.html',
  styleUrls: ['./carga-manual-devengado.component.scss'],
})

export class CargaManualDevengadoComponent {

  constructor(private titleService: Title) {
    this.titleService.setTitle('Carga de devengado manual ' + TituloBanco.TITULO);
  }
}
