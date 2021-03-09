import { Component } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { MotivoBaja } from '../../shared/modelo/motivoBaja.model';
import { List } from 'lodash';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BajaEtapaComando } from '../shared/modelo/comando-baja-etapa.modelo';
import { NotificacionService } from '../../shared/notificacion.service';
import { MotivosBajaService } from '../../shared/servicios/motivosbaja.service';
import { Area } from '../shared/modelo/area.model';
import { AreasService } from '../shared/areas.service';
import { BajaAreaComando } from '../shared/modelo/baja-area.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-area',
  templateUrl: 'eliminacion-area.component.html',
  styleUrls: ['eliminacion-area.component.scss'],
  providers: [AreasService]
})

export class EliminacionAreaComponent {
  public area: Area = new Area();
  public motivosBaja: List<MotivoBaja> = [];
  public form: FormGroup;
  public fechaActual: Date;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private route: ActivatedRoute,
              private areasService: AreasService,
              private motivosBajaService: MotivosBajaService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Eliminación de área ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.fechaActual = new Date();

    this.route.params
      .switchMap((params: Params) => this.areasService.consultarArea(+params['id']))
      .subscribe((area: Area) => this.area = area);

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
    let comando = new BajaAreaComando(this.form.value.motivoBaja);
    this.areasService.darDeBajaArea(this.area.id, comando)
      .subscribe((response) => {
          this.notificacionService
            .informar(['La operación se realizó con éxito.'])
            .result
            .then(() => {
              this.router.navigate(['/areas', this.area.id]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }
}
