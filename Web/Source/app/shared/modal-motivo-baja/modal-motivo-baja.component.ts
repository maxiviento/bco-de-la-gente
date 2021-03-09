import { Component, OnInit } from '@angular/core';
import { MotivosBajaService } from '../servicios/motivosbaja.service';
import { MotivoBaja } from '../modelo/motivoBaja.model';
import { List } from 'lodash';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificacionService } from '../notificacion.service';

@Component({
  selector: 'bg-modal-motivo-baja',
  templateUrl: './modal-motivo-baja.component.html',
  styleUrls: ['./modal-motivo-baja.component.scss'],
})

export class ModalMotivoBajaComponent implements OnInit {
  public title: string;
  public message: string;
  public form: FormGroup;
  public motivosBaja: List<MotivoBaja> = [];


  constructor(private fb: FormBuilder,
              private motivosBajaService: MotivosBajaService,
              private activeModal: NgbActiveModal,
              private notificacionService: NotificacionService,) {
    this.title = 'Seleccione motivo de baja';
  }

  public ngOnInit(): void {
    this.crearForm();
    this.motivosBajaService.consultarMotivosBaja()
      .subscribe((motivosBaja) => {
        this.motivosBaja = (motivosBaja);
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      motivoBaja: ['', Validators.required],
    });
  }

  public darDeBaja(): void {
    if (this.form.valid) {
      this.activeModal.close(this.form.value.motivoBaja);
    } else {
      this.notificacionService.informar(['Debe seleccionar un motivo.']);
    }
  }

  public cerrar(): void {
    this.activeModal.close();
  }
}
