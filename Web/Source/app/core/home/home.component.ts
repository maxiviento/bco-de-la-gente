import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'home',
  templateUrl: 'home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  public versionGit = '2.4b212ef5.1505-1100';
  public titulo: string = 'Banco de la gente - Gobierno de la Provincia de Córdoba';

  constructor(private titleService: Title) {
    this.titleService.setTitle(this.titulo);
  }

}
