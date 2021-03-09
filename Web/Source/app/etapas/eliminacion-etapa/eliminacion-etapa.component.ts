import { Component } from '@angular/core';
import { EtapasService } from '../shared/etapas.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Etapa } from '../shared/modelo/etapa.model';
import { MotivoBaja } from '../../shared/modelo/motivoBaja.model';
import { List } from 'lodash';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BajaEtapaComando } from '../shared/modelo/comando-baja-etapa.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { MotivosBajaService } from '../../shared/servicios/motivosbaja.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-etapa',
  templateUrl: 'eliminacion-etapa.component.html',
  styleUrls: ['eliminacion-etapa.component.scss']
})

export class EliminacionEtapaComponent {
  public etapa: Etapa = new Etapa();
  public motivosBaja: List<MotivoBaja> = [];
  public form: FormGroup;
  public fechaActual: Date;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private route: ActivatedRoute,
              private etapasService: EtapasService,
              private motivosBajaService: MotivosBajaService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Baja etapa ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.fechaActual = new Date();

    this.route.params
      .switchMap((params: Params) => this.etapasService.consultarEtapa(+params['id']))
      .subscribe((etapa: Etapa) => this.etapa = etapa);

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
    let comando = new BajaEtapaComando(this.form.value.motivoBaja);
    this.etapasService.darDeBajaEtapa(this.etapa.id, comando)
      .subscribe(() => {
          this.notificacionService
            .informar(['La operación se realizó con éxito.'])
            .result
            .then(() => {
              this.router.navigate(['/etapas', this.etapa.id]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }
}
