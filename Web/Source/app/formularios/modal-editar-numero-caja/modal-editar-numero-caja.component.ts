import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { NumeroCajaComando } from '../shared/modelo/numero-caja-comando.model';

@Component({
  selector: 'bg-modal-editar-numero-caja',
  templateUrl: './modal-editar-numero-caja.component.html',
  styleUrls: ['./modal-editar-numero-caja.component.scss'],
})

export class ModalEditarNumeroCajaComponent implements OnInit {
  public form: FormGroup;
  @Input() public ambito: string;
  @Input() public idFormularioLinea: number;
  @Input() public numeroCaja: string;

  constructor(private fb: FormBuilder,
              private activeModal: NgbActiveModal) {
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      numeroCaja: [this.numeroCaja, Validators.compose([CustomValidators.validTextAndNumbers,
        Validators.required, Validators.maxLength(9)])]
    });
  }

 private editarNumeroCaja() {
    let numeroCajaComando = new NumeroCajaComando(this.idFormularioLinea, this.form.get('numeroCaja').value);
    this.activeModal.close(numeroCajaComando);
  }

  public cerrar(): void {
    this.activeModal.close();
  }
}
