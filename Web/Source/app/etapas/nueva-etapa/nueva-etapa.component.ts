import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Etapa } from '../shared/modelo/etapa.model';
import { EtapasService } from '../shared/etapas.service';
import { Router } from '@angular/router';
import { NotificacionService } from '../../shared/notificacion.service';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nueva-etapa',
  templateUrl: './nueva-etapa.component.html',
  styleUrls: ['./nueva-etapa.component.scss'],
  providers: [EtapasService]
})

export class NuevaEtapaComponent implements OnInit {
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private etapasService: EtapasService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Nueva etapa ' + TituloBanco.TITULO);
  }

  ngOnInit(): void {
    this.crearForm();
  }

  public registrar(): void {
    let etapa = this.prepararEtapa();

    this.etapasService
      .registrarEtapa(etapa)
      .subscribe((nuevaEtapa) => {
          this.notificacionService
            .informar(['La operación se realizó con éxito'])
            .result
            .then(() => {
              this.router.navigate(['/etapas', nuevaEtapa.id]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private prepararEtapa(): Etapa {
    let formModel = this.form.value;
    let etapaForm = formModel.etapa;

    return new Etapa(
      0,
      etapaForm.nombre,
      etapaForm.descripcion
    );
  }

  private crearForm(): void {
    this.form = this.fb.group({
      etapa: this.fb.group({
        nombre: ['',
          Validators.compose([
            Validators.required,
            Validators.maxLength(100),
            CustomValidators.validTextAndNumbers])
        ],
        descripcion: ['',
          Validators.compose([
            Validators.required,
            Validators.maxLength(200),
            CustomValidators.validTextAndNumbers])
        ]
      })
    });
  }
}
