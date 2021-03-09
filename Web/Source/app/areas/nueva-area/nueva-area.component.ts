import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AreasService } from '../shared/areas.service';
import { Area } from '../shared/modelo/area.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { Router } from '@angular/router';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nueva-area',
  templateUrl: './nueva-area.component.html',
  styleUrls: ['./nueva-area.component.scss'],
  providers: [AreasService]
})
export class NuevaAreaComponent implements OnInit {
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private areasService: AreasService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Nueva área ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  public registrar(): void {
    let area = this.prepararArea();

    this.areasService
      .registrarArea(area)
      .subscribe((nuevaArea) => {
          this.notificacionService
            .informar(['La operación se realizó con éxito'])
            .result
            .then(() => {
              this.router.navigate(['/areas', nuevaArea.id]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private prepararArea(): Area {
    let formModel = this.form.value;

    return new Area(
      undefined,
      formModel.nombre,
      formModel.descripcion
    );
  }

  private crearForm(): void {
    this.form = this.fb.group({
      nombre: ['', Validators.compose([
        Validators.required,
        Validators.maxLength(100),
        CustomValidators.validTextAndNumbers]
      )],
      descripcion: ['', Validators.compose([
        Validators.required,
        Validators.maxLength(200),
        CustomValidators.validTextAndNumbers]
      )]
    });
  }
}
