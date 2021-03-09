import {
  Component, Input, Output, EventEmitter, ViewChild, ElementRef, OnInit, OnChanges,
  SimpleChanges
} from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LineaPrestamo } from '../modelo/linea-prestamo.model';
import { SexoDestinatario } from '../modelo/destinatario-prestamo.model';
import { MotivoDestino } from '../../../motivo-destino/shared/modelo/motivo-destino.model';
import { DestinatarioService } from '../destinatario.service';
import { MotivoDestinoService } from '../../../motivo-destino/shared/motivo-destino.service';
import { ProgramaCombo } from '../modelo/programa-combo.model';
import { ProgramaService } from '../ProgramaService';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-linea-items',
  templateUrl: './linea-items.component.html',
  styleUrls: ['./linea-items.component.scss']
})

export class LineaItemsComponent implements OnInit, OnChanges {
  @Input('linea') public form: FormGroup;
  @Input() public editable: boolean = true;
  @Output() public requisitos: EventEmitter<string> = new EventEmitter<string>();
  @ViewChild('txt_color_linea') public txt_color_linea: ElementRef;

  constructor(private destinatarioService: DestinatarioService,
              private motivoDestinoService: MotivoDestinoService,
              private programaService: ProgramaService) {
  }

  public colorLineaSeleccionado: string;
  public destinatarios: SexoDestinatario[] = [];
  public motivosDestinos: MotivoDestino[] = [];
  public programas: ProgramaCombo[] = [];

  public   ngOnInit(): void {
    this.cargarCombos();
    if (this.form.get('color').value) {
      this.colorLineaSeleccionado = this.form.get('color').value;
      this.form.setControl('color', new FormControl(this.colorLineaSeleccionado));
    }
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes) {
      for (let propName in changes) {
        if (propName === 'form' && this.form.get('color').value) {
          this.colorLineaSeleccionado = this.form.get('color').value;
        }
      }
    }
  }

  public onChangeColor(color: string) {
    let colorHex = LineaItemsComponent.convertRgbToHex(color);

    if (colorHex) {
      this.colorLineaSeleccionado = colorHex;
    } else {
      this.colorLineaSeleccionado = color;
    }
    this.form.setControl('color', new FormControl(this.colorLineaSeleccionado));
  }

  public static nuevoFormGroup(linea: LineaPrestamo = new LineaPrestamo()): FormGroup {
    return new FormGroup({
      id: new FormControl(linea.id),
      descripcion: new FormControl(linea.descripcion, Validators.compose([
        Validators.required,
        Validators.maxLength(200),
        CustomValidators.validTextAndNumbers])),
      nombre: new FormControl(linea.nombre, Validators.compose([
        Validators.maxLength(100),
        Validators.required,
        CustomValidators.validTextAndNumbers])),
      conOng: new FormControl(linea.conOng),
      conCurso: new FormControl(linea.conCurso),
      programa: new FormControl(linea.idPrograma, linea.conPrograma ? Validators.required : null),
      conPrograma: new FormControl(linea.conPrograma),
      deptoLocalidad: new FormControl(linea.deptoLocalidad),
      destinatario: new FormControl(linea.idSexoDestinatario, Validators.required),
      motivoDestino: new FormControl(linea.idMotivoDestino, Validators.required),
      objetivo: new FormControl(linea.objetivo, Validators.compose([
        Validators.maxLength(200),
        Validators.required,
        CustomValidators.validTextAndNumbers])),
      color: new FormControl(linea.color)
    });
  }

  public clickColor() {
    this.txt_color_linea.nativeElement.click();
  }

  private cargarCombos() {
    this.destinatarioService
      .consultarDestinatarios()
      .subscribe((destinatario) => this.destinatarios = destinatario);
    this.motivoDestinoService
      .consultarMotivosDestino()
      .subscribe((motivoDestino) => this.motivosDestinos = motivoDestino);
    this.programaService
      .consultarProgramas()
      .subscribe((programa) => {
        this.programas = programa;
      });
  }

  private static convertRgbToHex(rgb: string): any {
    let regex = rgb.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);
    return (regex && regex.length === 4) ? '#' +
      ('0' + parseInt(regex[1], 10).toString(16)).slice(-2) +
      ('0' + parseInt(regex[2], 10).toString(16)).slice(-2) +
      ('0' + parseInt(regex[3], 10).toString(16)).slice(-2) : '';
  }
}
