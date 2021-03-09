import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MotivoDestino } from '../shared/modelo/motivo-destino.model';
import { MotivoDestinoService } from '../shared/motivo-destino.service';
import { ApartadoMotivoDestinoComponent } from '../shared/apartado-motivo-destino/apartado-motivo-destino.component';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../shared/notificacion.service';
import { MotivosBajaService } from '../../shared/servicios/motivosbaja.service';
import { MotivoBaja } from '../../shared/modelo/motivoBaja.model';
import { List } from 'lodash';
import { BajaMotivoDestinoComando } from '../shared/modelo/comando-baja-motivo-destino';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-motivo-destino',
  templateUrl: './eliminacion-motivo-destino.component.html',
  styleUrls: ['./eliminacion-motivo-destino.component.scss'],
})

export class EliminacionMotivoDestinoComponent implements OnInit {
  public form: FormGroup;
  public motivoDestino: MotivoDestino = new MotivoDestino();
  public motivosBaja: List<MotivoBaja> = [];

  constructor(private motivoDestinoService: MotivoDestinoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private route: ActivatedRoute,
              private motivosBajaService: MotivosBajaService,
              private titleService: Title) {
    this.titleService.setTitle('Baja de motivo destino ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.route.params
      .switchMap((params: Params) => this.motivoDestinoService.consultarMotivoDestino(+params['id']))
      .subscribe((motivo) => {
        this.motivoDestino = motivo;
        this.crearForm();
      });
    this.motivosBajaService.consultarMotivosBaja()
      .subscribe((motivosBaja) => this.motivosBaja = motivosBaja);
  }

  public crearForm(): void {
    this.form = this.fb.group({
      apartadoMotivoDestino: ApartadoMotivoDestinoComponent.nuevoFormGroup(this.motivoDestino),
      motivoBaja: ['', Validators.required]
    });
  }

  public get apartadoMotivoDestino(): FormGroup {
    return this.form.get('apartadoMotivoDestino') as FormGroup;
  }

  public eliminarMotivoDestino(): void {
    let comando = new BajaMotivoDestinoComando(this.form.value.motivoBaja);
    this.motivoDestinoService.darDeBajaArea(this.motivoDestino.id, comando)
      .subscribe(() => {
        this.notificacionService.informar(['El motivo se dio de baja con éxito.'])
          .result
          .then(() => this.router.navigate(['/consulta-motivo-destino', this.motivoDestino.id]));
      }, (errores) => this.notificacionService.informar(errores, true));
  }
}
