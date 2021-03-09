import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NotificacionService } from '../../../shared/notificacion.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'bg-linea-archivos-modal',
  templateUrl: './linea-archivos.modal.html',
  styleUrls: ['./linea-archivos.modal.scss']
})

export class LineaArchivosModal implements OnInit {
  @Input() public pathLogo: string;
  @Input() public pathPiePagina: string;
  public nombreLogo: string;
  public nombrePiePagina: string;
  public form: FormGroup;

  constructor(public activeModal: NgbActiveModal,
              public route: ActivatedRoute,
              public notificacionService: NotificacionService,
              private sanitizer: DomSanitizer,
              private authService: AuthService) {
  }

 public ngOnInit(): void {
    this.crearForm();
  }

  private crearForm() {
    this.form = new FormGroup({});

    this.nombreLogo = this.getNombreArchivo(this.pathLogo);
    this.nombrePiePagina = this.getNombreArchivo(this.pathPiePagina);
  }

  public getUrlDescarga(path: string) {
    return this.sanitizer
      .bypassSecurityTrustResourceUrl(`/api/lineasprestamo/descargarArchivo?path=${path}&access_token=${this.authService.token()}`);
  }

  public getNombreArchivo(path: string): string {
    let indiceBarra = path.lastIndexOf('\\');
    let indicePunto = path.lastIndexOf('.');
    let nombreArchivo = path.slice(indiceBarra + 1, indicePunto);
    return nombreArchivo;
  }
}
