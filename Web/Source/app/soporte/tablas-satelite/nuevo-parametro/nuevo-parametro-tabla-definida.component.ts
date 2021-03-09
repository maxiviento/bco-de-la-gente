import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TablasDefinidasService } from '../../tablas-definidas.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { ParametroTablaDefinida } from '../../modelo/parametro-tabla-definida';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-parametro-tabla-definida',
  templateUrl: './nuevo-parametro-tabla-definida.component.html',
  styleUrls: ['./nuevo-parametro-tabla-definida.component.scss'],
  providers: []
})
export class NuevaParametroTablaDefinidaComponent implements OnInit {
  public form: FormGroup;
  public idTabla: number;

  constructor(private fb: FormBuilder,
              private tablasDefinidasService: TablasDefinidasService,
              private notificacionService: NotificacionService,
              private router: Router,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo parámetro ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.idTabla = this.route.snapshot.params['id'];
  }

  public registrar(): void {
    let parametro = this.prepararParametro();
    this.tablasDefinidasService
      .registrarParametro(parametro, this.idTabla)
      .subscribe((idParametro) => {
          this.notificacionService
            .informar(['La operación se realizó con éxito'])
            .result
            .then(() => {
              this.router.navigate(['/consultar-parametro-tabla-definida', idParametro]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private prepararParametro(): ParametroTablaDefinida {
    let formModel = this.form.value;
    return new ParametroTablaDefinida(
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
