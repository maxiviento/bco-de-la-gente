import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FormulariosService } from "../shared/formularios.service";
import { BehaviorSubject } from "rxjs";
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { NotificacionService } from "../../shared/notificacion.service";
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
        <button type="button" class="btn btn-primary pull-right" (click)="volver()">VOLVER</button>
      </div>
    </div>`
})

export class ReporteFormularioComponent implements OnInit {
  public pdfFormulario = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;

  constructor(private sanitizer: DomSanitizer,
              private route: ActivatedRoute,
              private notificacionService: NotificacionService,
              private formulariosService: FormulariosService,
              private titleService: Title) {
    this.titleService.setTitle('Reporte de formulario ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.route.params
      .switchMap((params: Params) =>
        this.formulariosService.generarReporteFormulario(+params['id']))
      .subscribe((resultado) => {
        this.pdfFormulario.next(this.sanitizer.bypassSecurityTrustResourceUrl(resultado));
        this.reporteSource = this.pdfFormulario.getValue();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public volver(): void {
    window.history.back();
  }

}
