import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder } from '@angular/forms';
import { MotivoRechazo } from '../../../../../formularios/shared/modelo/motivo-rechazo';
import { FormulariosService } from '../../../../../formularios/shared/formularios.service';
import { PrestamoService } from '../../../../../shared/servicios/prestamo.service';
import { DatosPrestamoReactivacionResultado } from '../../../../../formularios/shared/modelo/datos-prestamo-reactivacion-resultado.model';
import { MotivorechazoPrestamo } from '../../../../../formularios/shared/modelo/motivo-rechazo-prestamo.model';
import { MotivoRechazoFormulario } from '../../../../../formularios/shared/modelo/motivo-rechazo-formulario-model';

@Component({
  selector: 'bg-modal-ver-motivos-rechazo',
  templateUrl: './modal-ver-motivos-rechazo.component.html',
  styleUrls: ['./modal-ver-motivos-rechazo.component.scss']
})
export class ModalVerMotivosRechazoComponent implements OnInit {
  @Input() public idFormulario: number;
  @Input() public idPrestamo: number;
  @Input() public numeroCaja: string;

  public lsMotivosRechazoFormulario: MotivoRechazoFormulario[] = [];
  public lsMotivosRechazoPrestamo: MotivorechazoPrestamo[] = [];

  constructor(private fb: FormBuilder,
              private activeModal: NgbActiveModal,
              private formularioService: FormulariosService,
              private prestamoService: PrestamoService) {
  }

  public ngOnInit() {
    this.obtenerDatos();
  }

  public cerrar() {
    this.activeModal.close();
  }

  private obtenerDatos() {
    this.formularioService.obtenerMotivosRechazoFormulario(this.idFormulario).subscribe(
      (rechazos) => {
        this.lsMotivosRechazoFormulario = rechazos;
        if (this.lsMotivosRechazoPrestamo.length) {
          this.armarListaMotivosRechazoFormulario();
        }
      });
    this.prestamoService.obtenerMotivosRechazoPrestamo(this.idPrestamo).subscribe(
      (rechazos) => {
        this.lsMotivosRechazoPrestamo = rechazos;
        if (this.lsMotivosRechazoPrestamo.length) {
          this.armarListaMotivosRechazoPrestamo();
        }
      });
  }

  private armarListaMotivosRechazoPrestamo() {
    if (this.lsMotivosRechazoPrestamo.length) {
      let idSeguimiento = this.lsMotivosRechazoPrestamo[this.lsMotivosRechazoPrestamo.length - 1].idSeguimientoPrestamo;
      this.lsMotivosRechazoPrestamo.forEach((motivo) => {
        if (motivo.idSeguimientoPrestamo === idSeguimiento) {
          let motivoRechazo = new MotivoRechazo();
          motivoRechazo.id = motivo.idMotivo;
          motivoRechazo.descripcion = motivo.descripcion;
          motivoRechazo.observaciones = motivo.observaciones;
        }
      });
    }
  }

  private armarListaMotivosRechazoFormulario() {
    if (this.lsMotivosRechazoFormulario.length) {
      let idSeguimiento = this.lsMotivosRechazoFormulario[this.lsMotivosRechazoFormulario.length - 1].idSeguimientoFormulario;
      this.lsMotivosRechazoFormulario.forEach((motivo) => {
        if (motivo.idSeguimientoFormulario === idSeguimiento) {
          let motivoRechazo = new MotivoRechazo();
          motivoRechazo.id = motivo.idMotivo;
          motivoRechazo.descripcion = motivo.descripcion;
          motivoRechazo.observaciones = motivo.observaciones;
        }
      });
    }
  }
}
