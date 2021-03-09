import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MotivoDestino } from '../shared/modelo/motivo-destino.model';
import { MotivoDestinoService } from '../shared/motivo-destino.service';
import { ApartadoMotivoDestinoComponent } from '../shared/apartado-motivo-destino/apartado-motivo-destino.component';
import { Router } from '@angular/router';
import { NotificacionService } from '../../shared/notificacion.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-motivo-destino',
  templateUrl: './nuevo-motivo-destino.component.html',
  styleUrls: ['./nuevo-motivo-destino.component.scss'],
})

export class NuevoMotivoDestinoComponent implements OnInit {
  public form: FormGroup;
  public motivoDestino: MotivoDestino = new MotivoDestino();

  constructor(private motivoDestinoService: MotivoDestinoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo motivo destino ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      apartadoMotivoDestino: ApartadoMotivoDestinoComponent.nuevoFormGroup(this.motivoDestino)
    });
  }

  public get apartadoMotivoDestino(): FormGroup {
    return this.form.get('apartadoMotivoDestino') as FormGroup;
  }

  public registrarMotivoDestino(): void {
    let motivo = ApartadoMotivoDestinoComponent.prepararForm(this.apartadoMotivoDestino);
    this.motivoDestinoService.registrarMotivoDestino(motivo)
      .subscribe((res) => {
        this.notificacionService.informar(['El motivo de destino se registró con éxito.'])
          .result
          .then(() => {
            this.router.navigate(['/consulta-motivo-destino', res.id])
          });
      }, (errores) => this.notificacionService.informar(errores, true));
  }
}
