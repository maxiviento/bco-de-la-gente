import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as FileSaver from 'file-saver';
import { PagosService } from '../shared/pagos.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs/Rx';
import { ArchivoService } from '../../shared/archivo.service';
import { TipoPagoCombo } from '../shared/modelo/tipo-pago-combo.model';
import { MultipleSeleccionComponent } from '../../shared/multiple-seleccion/multiple-seleccion.component';
import { NotificacionService } from '../../shared/notificacion.service';

@Component({
  selector: 'bg-modal-nota-pagos',
  templateUrl: './modal-tipo-pagos.component.html',
  styleUrls: ['./modal-tipo-pagos.component.scss'],
})

export class ModalTipoPagosComponent implements OnInit {
  @Input() public idLote: number;
  public form: FormGroup;
  public CBTipoPago: TipoPagoCombo[];
  public idTipoPago: number;
  public archivoTxt = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;

  @ViewChild(MultipleSeleccionComponent)
  public comboTipoPago: MultipleSeleccionComponent;

  constructor(public activeModal: NgbActiveModal,
              private fb: FormBuilder,
              private pagosService: PagosService,
              private sanitizer: DomSanitizer,
              private archivoService: ArchivoService,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit(): void {
    this.cargarCombo();
    this.crearForm();
  }

  public crearForm() {
    this.form = this.fb.group({
      tipoPago: ['', Validators.required]
    });
  }

  public cargarCombo() {
    this.pagosService.consultarTipoPago()
      .subscribe((tiposPago) => {
        this.CBTipoPago = tiposPago;
      });
  }

  public generarArchivoTxt(idLote: number) {
    this.idTipoPago = parseInt(this.form.get('tipoPago').value, null);
    this.pagosService.generarArchivoTxt(idLote, this.idTipoPago).subscribe((res) => {
      this.archivoTxt.next(this.sanitizer.bypassSecurityTrustResourceUrl(this.archivoService.getUrlPrevisualizacionTxt(res, this.base64ToBytes(res.blob))));
      this.reporteSource = this.archivoTxt.getValue();
      let arrayBytes = this.base64ToBytes(res.blob);
      let blob = new Blob([arrayBytes], {type: ''});
      FileSaver.saveAs(blob, res.fileName);
    });
    this.cancelar(false);
  }

  private base64ToBytes(base64) {
    let raw = window.atob(base64);
    let n = raw.length;
    let bytes = new Uint8Array(new ArrayBuffer(n));

    for (let i = 0; i < n; i++) {
      bytes[i] = raw.charCodeAt(i);
    }
    return bytes;
  }

  public cancelar(cerrar?: boolean): void {
    if (cerrar) {
      this.notificacionService.confirmar('¿Está seguro que desea cerrar sin generar el archivo txt?')
        .result.then((result) => {
        if (result) {
          this.activeModal.close();
        }
      });
    } else {
      this.activeModal.close();
    }
  }
}
