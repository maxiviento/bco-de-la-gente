import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup } from '@angular/forms';
import { PagosService } from '../shared/pagos.service';
import { ModalidadPagoComponent } from "../modalidad-pago/modalidad-pago.component";
import { ActualizarModalidadLoteComando } from "../shared/modelo/actualizar-modalidad-lote-comando.model";
import { NotificacionService } from "../../shared/notificacion.service";

@Component({
  selector: 'bg-modal-modalidad-pago',
  templateUrl: './modal-modalidad-pago.component.html',
  styleUrls: ['./modal-modalidad-pago.component.scss'],
})

export class ModalModalidadPagoComponent implements OnInit {
  @Input() public idLote: number;
  public form: FormGroup;
  public reporteSource: any;

  @ViewChild(ModalidadPagoComponent) modalidad: ModalidadPagoComponent;

  constructor(public activeModal: NgbActiveModal,
              private notificacionService: NotificacionService,
              private pagosService: PagosService) {
  }

  ngOnInit(): void {
  }

  public actualizarModalidad() {
    let comandoActualizar = new ActualizarModalidadLoteComando(this.idLote);
    if (this.modalidad) {
      comandoActualizar.fechaPago = this.modalidad.obtenerFechaPago();
      comandoActualizar.fechaFinPago = this.modalidad.obtenerFechaFinPago();
      comandoActualizar.elementoPago = this.modalidad.obtenerElemento();
      comandoActualizar.modalidadPago = this.modalidad.obtenerModalidad();
      comandoActualizar.convenioPago = this.modalidad.obtenerConvenio();
      comandoActualizar.mesesGracia = this.modalidad.obtenerMesGraciaModificado();
      comandoActualizar.generaPlanCuotas = this.modalidad.actualizaPlan();
    }

    this.pagosService.actualizarModalidadPago(comandoActualizar).subscribe((res) => {
      this.notificacionService.informar(['Lote actualizado con éxito']);
    }, (errores) => {
      this.notificacionService.informar(errores, true);
    });
    this.activeModal.close();
  }

  public cancelar(): void {
    this.activeModal.close();
  }
}
