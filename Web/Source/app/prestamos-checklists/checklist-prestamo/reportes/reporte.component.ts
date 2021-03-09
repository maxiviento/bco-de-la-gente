import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PrestamoService } from '../../../shared/servicios/prestamo.service';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { NotificacionService } from '../../../shared/notificacion.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProvidenciaComando } from '../../../pagos/shared/modelo/providencia-comando.model';
import { ModalFechaProvidenciaComponent } from '../../../pagos/modal-fecha-providencia/modal-fecha-providencia.component';
import { ArchivoService } from "../../../shared/archivo.service";
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  styleUrls: ['./reporte.component.scss'],
  templateUrl: './reporte.component.html'
})

export class ReporteComponent implements OnInit {
  public reporteSource: any;
  public pdfRentas = new BehaviorSubject<SafeResourceUrl>(null);
  public pdfProv = new BehaviorSubject<SafeResourceUrl>(null);
  public resultado: any;
  public idFormulario: number;
  public idPrestamoRequisito: number;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private prestamoService: PrestamoService,
              private sanitizer: DomSanitizer,
              private archivoService: ArchivoService,
              private notificacionService: NotificacionService,
              public modalService: NgbModal,
              private titleService: Title) {
    this.idFormulario = this.route.snapshot.params['id'];
  }

  public ngOnInit() {
    this.idPrestamoRequisito = Number.parseInt(localStorage.getItem('idPrestamoRequisito'));
    if (this.router.url.includes('condicion-economica')) {
      this.titleService.setTitle('Reporte de condición económica ' + TituloBanco.TITULO);
      this.prestamoService.generarPdfGrupoFamiliar(this.idFormulario, this.idPrestamoRequisito)
        .subscribe((resultado) => {
          this.pdfRentas.next(this.sanitizer.bypassSecurityTrustResourceUrl(resultado));
          this.reporteSource = this.pdfRentas.getValue();
        }, (error) => this.notificacionService.informar([error], true));
    }

    if (this.router.url.includes('control-sintys')) {
      this.titleService.setTitle('Reporte de SINTYS ' + TituloBanco.TITULO);
      this.prestamoService.generarReporteSintysGrupoFamiliar(this.idFormulario)
        .subscribe((resultado) => {
          this.pdfRentas.next(this.sanitizer.bypassSecurityTrustResourceUrl(resultado));
          this.reporteSource = this.pdfRentas.getValue();
        }, (error) => this.notificacionService.informar([error], true));
    }

    if (this.router.url.includes('providencia')) {
      this.titleService.setTitle('Reporte de providencia ' + TituloBanco.TITULO);
      this.crearModalProv().then();
    }
  }

  private async crearModalProv() {
    const modalRef = await this.modalService.open(ModalFechaProvidenciaComponent, {
      backdrop: 'static',
      size: 'lg',
      keyboard: false
    });
    modalRef.componentInstance.idFormulario = this.idFormulario;
    modalRef.result.then((res) => {
      if (res) {
        let comando = new ProvidenciaComando(this.idFormulario, res);
        this.prestamoService.generarReporteProvidencia(comando)
          .subscribe((resultado) => {
            this.pdfProv.next(this.sanitizer.bypassSecurityTrustResourceUrl(this.archivoService.getUrlPrevisualizacionArchivo(resultado.archivos[0])));
            this.reporteSource = this.pdfProv.getValue();
            this.archivoService.descargarArchivos(resultado.archivos);
          }, (error) => this.notificacionService.informar([error], true));
      } else {
        window.close();
      }
    });
  }

  public cerrar(): void {
    window.close();
  }
}
