import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RentasService } from '../rentas.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  template: `
    <iframe style="margin-top: -25px;"
            *ngIf="reporteSource"
            [src]="reporteSource"
            width="100%" height="700px">
    </iframe>

    <div class="row">
      <div class="col">
        <button type="button" class="btn btn-secondary pull-right" (click)="volver()">VOLVER</button>
      </div>
    </div>`
})

export class ReporteRentasIndividualComponent implements OnInit {
  public reporteSource: any;
  private urlReturn: string = '/';

  constructor(private router: Router,
              private titleService: Title) {
  }

  public ngOnInit() {
    if (this.router.url.includes('condicion-economica-individual')) {
      this.titleService.setTitle('Reporte condición económica ' + TituloBanco.TITULO);
      this.urlReturn = '/condicion-economica';
    } else if (this.router.url.includes('control-sintys-individual')) {
      this.titleService.setTitle('Reporte sintys ' + TituloBanco.TITULO);
      this.urlReturn = '/control-sintys';
    }
    this.reporteSource = RentasService.recuperarReporte();
  }

  public volver(): void {
    this.router.navigate([this.urlReturn]);
  }

}
