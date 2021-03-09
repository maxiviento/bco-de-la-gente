import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { Area } from "../shared/modelo/area.model";
import { AreasService } from "../shared/areas.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NotificacionService } from "../../shared/notificacion.service";
import { EdicionAreaComando } from "../shared/modelo/edicion-area.model";
import { CustomValidators } from '../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-area',
  templateUrl: './edicion-area.component.html',
  styleUrls: ['./edicion-area.component.scss'],
  providers: [AreasService]
})

export class EdicionAreaComponent implements OnInit {
  public area: Area = new Area();
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private areasService: AreasService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Edición de área ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) => this.areasService.consultarArea(+params['id']))
      .subscribe((area: Area) => {
        this.area = area;
        this.crearForm();
      });

    this.crearForm();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      area: this.fb.group({
        nombre: [this.area.nombre,
          Validators.compose([
            Validators.required,
            Validators.maxLength(100),
            CustomValidators.validTextAndNumbers])
        ],
        descripcion: [this.area.descripcion,
          Validators.compose([
            Validators.required,
            Validators.maxLength(200),
            CustomValidators.validTextAndNumbers])
        ]
      })
    });
  }

  public registrarModificacion(): void {
    let comando = this.prepararComando();
    let idArea = this.area.id;

    this.areasService.modificarArea(idArea, comando)
      .subscribe(() => {
          this.notificacionService
            .informar(['La operación se realizó con éxito'])
            .result
            .then(() => {
              this.router.navigate(['/areas', idArea]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private prepararComando(): EdicionAreaComando {
    let formModel = this.form.value;
    let areaForm = formModel.area;

    return new EdicionAreaComando(
      areaForm.nombre,
      areaForm.descripcion
    );
  }
}
