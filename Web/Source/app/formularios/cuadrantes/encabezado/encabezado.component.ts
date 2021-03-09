import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'bg-encabezado',
  templateUrl: './encabezado.component.html',
  styleUrls: ['encabezado.component.scss'],
})

export class EncabezadoComponent {
  public form: FormGroup;
  @Input()
  public nombreLinea: string;
  @Input()
  public titulo: string;
  @Input()
  public logo?: string;
  @Input()
  public color: string;
}
