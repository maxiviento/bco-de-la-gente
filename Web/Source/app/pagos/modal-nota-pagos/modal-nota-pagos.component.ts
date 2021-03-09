import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as FileSaver from 'file-saver';
import { PagosService } from '../shared/pagos.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs/Rx';
import { CrearNotaBancoConsulta } from '../shared/modelo/crear-nota-banco-comando.model';
import { CustomValidators } from '../../shared/forms/custom-validators';

@Component({
  selector: 'bg-modal-nota-pagos',
  templateUrl: './modal-nota-pagos.component.html',
  styleUrls: ['./modal-nota-pagos.component.scss'],
})

export class ModalNotaPagosComponent implements OnInit {
  @Input() public idLote: number;
  public form: FormGroup;
  public excelBanco = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;

  constructor(public activeModal: NgbActiveModal,
              private fb: FormBuilder,
              private pagosService: PagosService,
              private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    this.crearForm();
  }

  public crearForm() {
    this.form = this.fb.group({
      nombre: ['', Validators.compose([
        Validators.required,
        Validators.maxLength(100),
        CustomValidators.validText]),
      ],
      cc: ['', Validators.compose([
        Validators.maxLength(100),
        CustomValidators.validText]),
      ]
    });
  }

  public crearNota() {
    this.pagosService.obtenerNotaPago(this.armarComando(this.idLote)).subscribe((res) => {
      this.excelBanco.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
      this.reporteSource = this.excelBanco.getValue();
      let arrayBytes = this.base64ToBytes(res.blob);
      let blob = new Blob([arrayBytes], {type: ''});
      FileSaver.saveAs(blob, res.fileName);
      this.cancelar();
    });
  }

  private armarComando(idLote: number): CrearNotaBancoConsulta {
    let formModel = this.form.value;
    let comando = new CrearNotaBancoConsulta();
    comando.nombre = formModel.nombre;
    comando.cc = formModel.cc;
    comando.idLote = idLote;
    return comando;
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

  public cancelar(): void {
    this.activeModal.close();
  }
}
