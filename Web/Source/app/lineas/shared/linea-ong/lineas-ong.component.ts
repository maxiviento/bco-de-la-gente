import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OngLinea } from '../modelo/ong-linea.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { OngComboResultado } from '../../../formularios/shared/modelo/ong.model';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { NotificacionService } from '../../../shared/notificacion.service';

@Component({
  selector: 'bg-lineas-ong',
  templateUrl: 'lineas-ong.component.html',
  styleUrls: ['lineas-ong.component.scss']
})

export class LineasOngComponent implements OnInit {
  @Input() public lsOngLinea: OngLinea[];
  @Input() public esModificacion: boolean;
  @Output() public aceptado: EventEmitter<OngLinea[]> = new EventEmitter<OngLinea[]>();
  @Output() public ongAgregadas: EventEmitter<OngLinea[]> = new EventEmitter<OngLinea[]>();
  @Output() public ongEliminadas: EventEmitter<OngLinea[]> = new EventEmitter<OngLinea[]>();
  @Output() public cancelado: EventEmitter<any> = new EventEmitter<any>();

  public form: FormGroup;
  public lsOng: OngComboResultado[] = [];
  public ongSeleccionada = new OngComboResultado();
  public lsOngEliminadas: OngLinea[] = [];
  public lsOngAgregadas: OngLinea[] = [];
  public lsOngLineaLocal: OngLinea[] = [];

  public constructor(private fb: FormBuilder,
                     private lineaService: LineaService,
                     private notificacionService: NotificacionService) {
  }

  public ngOnInit() {
    this.lineaService.obtenerOngs().subscribe((res) => this.lsOng = res);
    this.lsOngLineaLocal.push(...this.lsOngLinea);
    this.crearForm();
  }

  public crearForm() {
    this.form = this.fb.group({
      porcentajeRecupero: ['', Validators.compose([CustomValidators.decimalNumberWithTwoDigits, Validators.maxLength(5), Validators.minLength(1), Validators.required])],
      porcentajePago: ['', Validators.compose([CustomValidators.decimalNumberWithTwoDigits, Validators.maxLength(5), Validators.minLength(1), Validators.required])],
    });
  }

  public agregarOngLinea() {
    let existe = this.lsOngLineaLocal.filter((x) => x.id === this.ongSeleccionada.idEntidad);
    if (existe.length) {
      this.notificacionService.informar(['La ONG seleccionada ya se encuentra asociada a la línea.']);
      return;
    }
    let formModel = this.form.value;
    let ongSeleccionada = new OngLinea();
    ongSeleccionada.id = this.ongSeleccionada.idEntidad;
    ongSeleccionada.nombre = this.ongSeleccionada.nombre;
    ongSeleccionada.porcentajeRecupero = formModel.porcentajeRecupero;
    ongSeleccionada.porcentajePago = formModel.porcentajePago;

    this.lsOngLineaLocal.push(ongSeleccionada);
    this.lsOngAgregadas.push(ongSeleccionada);
    this.ongSeleccionada = new OngComboResultado();
    this.crearForm();
  }

  public quitarOngSeleccionada(ong: OngLinea) {
    this.notificacionService.confirmar('Está seguro que desea quitar la ONG ' + ong.nombre + '?')
      .result.then((result) => {
      if (result) {
        if (this.esModificacion) {
          this.lsOngAgregadas = this.lsOngAgregadas.filter((x) => x.id !== ong.id);
          if (ong.idLineaOng) {
            this.lsOngEliminadas.push(this.lsOngLineaLocal.find((x) => x.idLineaOng === ong.idLineaOng));
          }
        }
        this.lsOngLineaLocal = this.lsOngLineaLocal.filter((x) => x.id !== ong.id);
      }
    });
  }

  public aceptar() {
    this.aceptado.emit(this.lsOngLineaLocal);
    if (this.esModificacion) {
      this.ongAgregadas.emit(this.lsOngAgregadas);
      this.ongEliminadas.emit(this.lsOngEliminadas);
    }
  }

  public cancelar() {
    if (this.lsOngAgregadas.length || this.lsOngEliminadas.length) {
      this.notificacionService.confirmar('Al cancelar se perderán los cambios, ¿Continuar?').result.then((result) => {
        if (result) {
          this.cancelado.emit();
        }
      });
    } else {
      this.cancelado.emit();
    }
  }

  public buscarOng(nombre: string) {
    this.lineaService.obtenerOngsPorNombre(nombre).subscribe((res) => this.lsOng = res);
  }

  public agregarOngSeleccionada(idOngSeleccionada: number) {
    if (idOngSeleccionada) {
      this.ongSeleccionada = this.lsOng.find((x) => x.idEntidad === idOngSeleccionada);
    } else {
      this.ongSeleccionada = new OngComboResultado();
    }
  }
}
