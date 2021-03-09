import { CustomValidators } from '../../shared/forms/custom-validators';
import { Component, OnInit } from '@angular/core';
import { EtapasService } from '../shared/etapas.service';
import { Etapa } from '../shared/modelo/etapa.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EdicionEtapaComando } from '../shared/modelo/comando-edicion-etapa.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-etapa',
  templateUrl: './edicion-etapa.component.html',
  styleUrls: ['./edicion-etapa.component.scss'],
  providers: [EtapasService]
})

export class EdicionEtapaComponent implements OnInit {
  public etapa: Etapa = new Etapa();
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private etapasService: EtapasService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Edición de etapa ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) => this.etapasService.consultarEtapa(+params['id']))
      .subscribe((etapa: Etapa) => {
        this.etapa = etapa;
        this.crearForm();
      });

    this.crearForm();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      etapa: this.fb.group({
        nombre: [this.etapa.nombre,
          Validators.compose([
            Validators.required,
            Validators.maxLength(100),
            CustomValidators.validTextAndNumbers])
        ],
        descripcion: [this.etapa.descripcion,
          Validators.compose([
            Validators.required,
            Validators.maxLength(200),
            CustomValidators.validTextAndNumbers])
        ]
      })
    });
    this.form.markAsDirty();
  }

  public registrarModificacion(): void {
    let comando = this.prepararComando();
    let idEtapa = this.etapa.id;

    this.etapasService.modificarEtapa(idEtapa, comando)
      .subscribe(() => {
          this.notificacionService
            .informar(['La operación se realizó con éxito'])
            .result
            .then(() => {
              this.router.navigate(['/etapas', this.etapa.id]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private prepararComando(): EdicionEtapaComando {
    let formModel = this.form.value;
    let etapaForm = formModel.etapa;

    return new EdicionEtapaComando(
      etapaForm.nombre,
      etapaForm.descripcion
    );
  }
}
