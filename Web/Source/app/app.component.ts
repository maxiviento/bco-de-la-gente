import {
  Component,
  OnInit,
  ViewEncapsulation
} from '@angular/core';

import { Title } from '@angular/platform-browser';

import { AppState } from './app.service';
import 'rxjs/add/operator/do';

@Component({
  selector: 'app',
  encapsulation: ViewEncapsulation.None,
  styleUrls: [
    'app.component.scss'
  ],
  templateUrl: './app.component.html',
  providers: [
    Title
  ]
})
export class AppComponent implements OnInit {

  public titulo: string = 'Banco de la gente - Gobierno de la Provincia de Córdoba';
  public fechaActual: Date = new Date(Date.now());

  constructor(public appState: AppState,
    public titleService: Title) {
    this.titleService.setTitle(this.titulo);

  }

  public ngOnInit() {
    console.log('Initial App State', this.appState.state);
    this.titleService.setTitle(this.titulo);
  }
}

/*
 * Please review the https://github.com/AngularClass/angular2-examples/ repo for
 * more angular app examples that you may copy/paste
 * (The examples may not be updated as quickly. Please open an issue on github for us to update it)
 * For help or questions please contact us at @AngularClass on twitter
 * or our chat on Slack at https://AngularClass.com/slack-join
 */
