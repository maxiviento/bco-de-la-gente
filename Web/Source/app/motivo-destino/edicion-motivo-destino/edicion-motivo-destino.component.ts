import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MotivoDestino } from '../shared/modelo/motivo-destino.model';
import { MotivoDestinoService } from '../shared/motivo-destino.service';
import { ApartadoMotivoDestinoComponent } from '../shared/apartado-motivo-destino/apartado-motivo-destino.component';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../shared/notificacion.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-motivo-destino',
  templateUrl: './edicion-motivo-destino.component.html',
  styleUrls: ['./edicion-motivo-destino.component.scss'],
})

export class EdicionMotivoDestinoComponent implements OnInit {
  public form: FormGroup;
  public motivoDestino: MotivoDestino = new MotivoDestino();

  constructor(private motivoDestinoService: MotivoDestinoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Editar motivo destino ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.route.params
      .switchMap((params: Params) => this.motivoDestinoService.consultarMotivoDestino(+params['id']))
      .subscribe((motivo) => {
        this.motivoDestino = motivo;
        this.crearForm();
      });
  }

  public crearForm(): void {
    this.form = this.fb.group({
      apartadoMotivoDestino: ApartadoMotivoDestinoComponent.nuevoFormGroup(this.motivoDestino)
    });
  }

  public get apartadoMotivoDestino(): FormGroup {
    return this.form.get('apartadoMotivoDestino') as FormGroup;
  }

  public editarMotivoDestino(): void {
    let motivo = ApartadoMotivoDestinoComponent.prepararForm(this.apartadoMotivoDestino);
    this.motivoDestinoService.editarMotivoDestino(motivo)
      .subscribe(() => {
        this.notificacionService.informar(['El motivo de destino se modificó con éxito.'])
          .result
          .then(() => this.router.navigate(['/consulta-motivo-destino', motivo.id]));
      }, (errores) => this.notificacionService.informar(errores, true));
  }
}
