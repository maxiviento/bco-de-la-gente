import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'buscador-drop-down',
  templateUrl: './buscador-drop-down.component.html',
  styleUrls: ['./buscador-drop-down.component.scss'],
  providers: []
})
export class BuscadorDropDownComponent implements OnInit {

  @Output() public stringIngresado: EventEmitter<string> = new EventEmitter<string>();
  @Output() public valorSeleccionado: EventEmitter<any> = new EventEmitter<any>();
  @Input() public lsValores: any[];
  @Input() public idObjeto: string;
  @Input() public nombreObjeto: string;

  public form: FormGroup;
  public model: any;
  public filtraEnMemoria = false;

  constructor(private fb: FormBuilder) {
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  public crearForm() {
    this.form = this.fb.group({
      objeto: [this.model, Validators.compose([Validators.minLength(3), Validators.required])]
    });

    this.form.get('objeto').valueChanges.distinctUntilChanged().subscribe((value) => {
      if (!value) {
        this.filtraEnMemoria = false;
      }
      this.valorSeleccionado.emit(value);
    });
  }

  private emitirEvento(seleccionado: string) {
    if (!seleccionado.length) {
      this.filtraEnMemoria = false;
    }
    if (!this.filtraEnMemoria) {
      if (seleccionado.length >= 3) {
        const filtrado = seleccionado.substr(0, 3);
        this.filtraEnMemoria = true;
        this.stringIngresado.emit(filtrado);
      }
    }
  }
}
