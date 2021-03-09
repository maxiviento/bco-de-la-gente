import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { PrestamoService } from '../../../../shared/servicios/prestamo.service';
import { ConsultaImprimirDocumentos } from '../../modelos/consulta-imprimir-documentos.model';

@Component({
  selector: 'bg-modal-fecha-documento',
  templateUrl: './modal-fecha-documento.component.html',
  styleUrls: ['./modal-fecha-documento.component.scss'],
})

export class ModalFechaDocumentosComponent implements OnInit {
  public form: FormGroup;
  public ingresoManual: boolean;
  public fecha: Date;
  public idPrestamo: number;
  public impresionComando = new ConsultaImprimirDocumentos();
  public fechaAprobacion: boolean = false;

  constructor(private prestamoService: PrestamoService,
              public activeModal: NgbActiveModal,
              private fb: FormBuilder) {
  }

  public ngOnInit(): void {
    this.ingresoManual = false;
    this.crearForm();
  }

  public crearForm() {
    let nuevaFechaFC = new FormControl(NgbUtils.obtenerNgbDateStruct(new Date(Date.now())));
    this.form = this.fb.group({fecha: nuevaFechaFC});
  }

  public ingresoManualFecha(): void {
    this.ingresoManual = true;
    let fechaManual = this.form.get('fecha') as FormControl;
    fechaManual.setValidators(Validators.compose([Validators.required, CustomValidators.minDate(new Date('01/01/1900'))]));
  }

  public utilizarFechaAprobacion(): void {
    this.ingresoManual = false;
    this.impresionComando.fechaAprobacion = true;
    this.activeModal.close(this.impresionComando);
  }

  public confirmarFecha() {
    let formModel = this.form.value;
    this.fecha = NgbUtils.obtenerDate(formModel.fecha);
    this.impresionComando.fechaAprobacion = false;
    this.impresionComando.fecha = NgbUtils.obtenerDate(formModel.fecha);
    this.activeModal.close(this.impresionComando);
  }

  public cancelar(): void {
    this.activeModal.close();
  }
}
