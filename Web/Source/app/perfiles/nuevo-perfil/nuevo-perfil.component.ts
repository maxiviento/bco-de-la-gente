import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PerfilesService } from '../shared-perfiles/perfiles.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { Funcionalidad } from '../shared-perfiles/modelo/funcionalidad.model';
import { Perfil } from '../shared-perfiles/modelo/perfil.model';
import { Router } from '@angular/router';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-perfil',
  templateUrl: 'nuevo-perfil.component.html',
  styleUrls: ['nuevo-perfil.component.scss'],
  providers: [PerfilesService]
})

export class NuevoPerfilComponent implements OnInit {

  public form: FormGroup;
  public funcionalidadesCheck: Funcionalidad[] = [];
  public fechaActual: Date;

  public constructor(private fb: FormBuilder,
                     private perfilesService: PerfilesService,
                     private router: Router,
                     private notificacionService: NotificacionService,
                     private titleService: Title) {
    this.titleService.setTitle('Nuevo perfil ' + TituloBanco.TITULO);
    this.fechaActual = new Date(Date.now());
  }

  public ngOnInit(): void {
    this.obtenerFuncionalidades();
    this.crearForm();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      funcionalidades: this.fb.array((this.funcionalidadesCheck || []).map((funcionalidad) => {
        return this.prepararChecks(funcionalidad);
      })),
      nombre: ['', Validators.compose([Validators.maxLength(50), Validators.required, CustomValidators.validText])]
    });
  }

  public get funcionalidades(): FormArray {
    return this.form.get('funcionalidades') as FormArray;
  }

  public prepararChecks(funcionalidad: Funcionalidad): FormGroup {

    return new FormGroup({
      seleccionado: new FormControl(funcionalidad.seleccionado),
      id: new FormControl(funcionalidad.id),
      nombre: new FormControl(funcionalidad.nombre, Validators.compose([
        CustomValidators.validTextAndNumbers
      ])),
      codigo: new FormControl(funcionalidad.codigo)
    });
  }

  public obtenerFuncionalidades(): void {
    this.perfilesService
      .obtenerFuncionalidades()
      .subscribe((funcionalidades) => {
        this.funcionalidadesCheck = funcionalidades;
        this.crearForm();
      });
  }

  public aceptar(): void {
    let formGroups = this.funcionalidades.controls.filter((funcionalidad) => {
      if (funcionalidad.value.seleccionado) {
        return funcionalidad.value;
      }
    });
    let funcionalidadesAsociadas = formGroups.map((formGroup) => formGroup.value.id);
    if (funcionalidadesAsociadas.length !== 0 && this.form.value.nombre !== '') {

      this.perfilesService
        .registrarPerfil(this.prepararPerfil(funcionalidadesAsociadas))
        .subscribe(
          (perfilId) => {
            this.notificacionService
              .informar(['La operación se realizó con éxito'])
              .result.then(() => {
              this.router.navigate(['/perfiles', perfilId, 'visualizacion']);
            });
          },
          (errores) => {
            this.notificacionService
              .informar(errores, true);
          }
        );

    } else {
      if (funcionalidadesAsociadas.length === 0) {
        this.notificacionService
          .informar(['Debe seleccionar al menos una funcionalidad.']);
        return;
      }
      if (this.form.value.nombre === '') {
        this.notificacionService
          .informar(['Debe cargar el nombre.']);
        return;
      }
      if (!this.form.value.nombre.valid) {
        this.notificacionService
          .informar(['Debe ingresar un nombre valido']);
        return;
      }
    }
  }

  private prepararPerfil(funcionalidadesAsociadas: string[]): any {
    let formModel = this.form.value;
    return new Perfil(
      formModel.nombre,
      this.fechaActual,
      null,
      funcionalidadesAsociadas,
    );
  }
}
