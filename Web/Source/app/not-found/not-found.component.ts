import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'not-found',
  template: `
    <div>
      <h1>404: Página no encontrada</h1>
    </div>
  `
})
export class PageNotFoundComponent {
  constructor(private titleService: Title) {
    this.titleService.setTitle('404 Página no encontrada ' + TituloBanco.TITULO);
  }
}
