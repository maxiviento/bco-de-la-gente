import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MotivoRechazo } from '../../formularios/shared/modelo/motivo-rechazo';
import { MotivosRechazoService } from '../shared/motivos-rechazo.service';
import { ActivatedRoute, Params } from '@angular/router';
import { ApartadoMotivoRechazoComponent } from '../shared/apartado-motivo-rechazo/apartado-motivo-rechazo.component';
import { Ambito } from '../shared/modelo/ambito.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-motivo-rechazo',
  templateUrl: './consulta-motivo-rechazo.component.html',
  styleUrls: ['./consulta-motivo-rechazo.component.scss'],
})

export class ConsultaMotivoRechazoComponent implements OnInit {
  public form: FormGroup;
  public motivoRechazo: MotivoRechazo = new MotivoRechazo();
  public ambitos: Ambito[];

  constructor(private motivoRechazoService: MotivosRechazoService,
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver motivo de rechazo ' + TituloBanco.TITULO);
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
  }

  public crearForm(): void {
    this.form = this.fb.group({
      apartadoMotivoRechazo: ApartadoMotivoRechazoComponent.nuevoFormGroup(this.motivoRechazo)
    });
  }

  public get apartadoMotivoRechazo(): FormGroup {
    return this.form.get('apartadoMotivoRechazo') as FormGroup;
  }

  private consultarAmbitos(): void {
    this.motivoRechazoService.consultarAmbitos()
      .subscribe((ambitos) => this.ambitos = ambitos);
  }

}
