import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MotivoRechazo } from '../../formularios/shared/modelo/motivo-rechazo';
import { MotivoBaja } from '../../shared/modelo/motivoBaja.model';
import { MotivosRechazoService } from '../shared/motivos-rechazo.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { MotivosBajaService } from '../../shared/servicios/motivosbaja.service';
import { ApartadoMotivoRechazoComponent } from '../shared/apartado-motivo-rechazo/apartado-motivo-rechazo.component';
import { BajaMotivoRechazoComando } from '../shared/modelo/comando-baja-motivo-rechazo.model';
import { Ambito } from '../shared/modelo/ambito.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import List = _.List;

@Component({
  selector: 'bg-eliminacion-motivo-rechazo',
  templateUrl: './eliminacion-motivo-rechazo.component.html',
  styleUrls: ['./eliminacion-motivo-rechazo.component.scss'],
})

export class EliminacionMotivoRechazoComponent implements OnInit {
  public form: FormGroup;
  public motivoRechazo: MotivoRechazo = new MotivoRechazo();
  public motivosBaja: List<MotivoBaja> = [];
  public ambitos: Ambito[];

  constructor(private motivoRechazoService: MotivosRechazoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private route: ActivatedRoute,
              private motivosBajaService: MotivosBajaService,
              private titleService: Title) {
    this.titleService.setTitle('Baja de motivo de rechazo ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarAmbitos();
    this.route.params
      .switchMap((params: Params) => this.motivoRechazoService.consultarMotivoRechazo(+params['idMotivo'], +params['idAmbito']))
      .subscribe((motivo) => {
        this.motivoRechazo = motivo;
        this.crearForm();
      });
    this.motivosBajaService.consultarMotivosBaja()
      .subscribe((motivosBaja) => this.motivosBaja = motivosBaja);
  }

  public crearForm(): void {
    this.form = this.fb.group({
      apartadoMotivoRechazo: ApartadoMotivoRechazoComponent.nuevoFormGroup(this.motivoRechazo),
      motivoBaja: ['', Validators.required]
    });
  }

  public get apartadoMotivoRechazo(): FormGroup {
    return this.form.get('apartadoMotivoRechazo') as FormGroup;
  }

  public eliminarMotivoRechazo(): void {
    let comando = new BajaMotivoRechazoComando(this.motivoRechazo.id, this.form.value.motivoBaja, this.motivoRechazo.ambito.id);
    this.motivoRechazoService.darDeBajaMotivoRechazo(comando)
      .subscribe(() => {
        this.notificacionService.informar(['El motivo de rechazo se dio de baja con éxito.'])
          .result
          .then(() => this.router.navigate(['/consulta-motivo-rechazo', this.motivoRechazo.id, this.motivoRechazo.ambito.id]));
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  private consultarAmbitos(): void {
    this.motivoRechazoService.consultarAmbitos()
      .subscribe((ambitos) => this.ambitos = ambitos);
  }
}
