import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MotivoDestino } from '../shared/modelo/motivo-destino.model';
import { MotivoDestinoService } from '../shared/motivo-destino.service';
import { ApartadoMotivoDestinoComponent } from '../shared/apartado-motivo-destino/apartado-motivo-destino.component';
import { ActivatedRoute, Params } from '@angular/router';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-motivo-destino',
  templateUrl: './consulta-motivo-destino.component.html',
  styleUrls: ['./consulta-motivo-destino.component.scss'],
})

export class ConsultaMotivoDestinoComponent implements OnInit {
  public form: FormGroup;
  public motivoDestino: MotivoDestino = new MotivoDestino();

  constructor(private motivoDestinoService: MotivoDestinoService,
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver motivo destino ' + TituloBanco.TITULO);
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

}
