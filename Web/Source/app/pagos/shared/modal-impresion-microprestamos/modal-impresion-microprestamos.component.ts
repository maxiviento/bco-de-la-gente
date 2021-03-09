import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'bg-modal-impresion-microprestamos',
  templateUrl: 'modal-impresion-microprestamos.component.html',
  styleUrls: ['modal-impresion-microprestamos.component.scss'],
})

export class ModalImpresionMicroprestamosComponent implements OnInit {
  public title: string;
  public message: string;
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              //private motivosBajaService: MotivosBajaService,
              private activeModal: NgbActiveModal) {
    this.title = 'Imprimir documentación del micropréstamo';
  }

  ngOnInit(): void {
    this.crearForm();

    /*this.motivosBajaService.consultarMotivosBaja()
      .subscribe((motivosBaja) => {
        this.motivosBaja = (motivosBaja);
      });*/
  }

  private crearForm(): void {
    this.form = this.fb.group({
      idOrden: ['', Validators.required],
    });
  }

  public cerrar(): void {
    this.activeModal.close(false);
  }

  public confirmarImpresion(){
    this.activeModal.close(this.preparaForm());
  }

  public cancelarImpresion(){
    //this.activeModal.close(this.preparaForm());
  }

  private preparaForm(){
    if(this.form.valid){
      let form = this.form.value;
      return form.idOrden;
    }
  }
}
