import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PerfilesService } from '../shared-perfiles/perfiles.service';
import { Perfil } from '../shared-perfiles/modelo/perfil.model';
import { Funcionalidad } from '../shared-perfiles/modelo/funcionalidad.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { UsuarioService } from '../../core/auth/usuario.service';
import { UsuarioLogueado } from '../../core/auth/modelos/usuario-logueado.model';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-perfil',
  templateUrl: './edicion-perfil.component.html',
  styleUrls: ['./edicion-perfil.component.scss']
})

export class EdicionPerfilComponent implements OnInit {
  public form: FormGroup;
  public funcionalidadesCheck: Funcionalidad[];
  public funcionalidadesPerfil: Funcionalidad[];
  public perfil: Perfil;
  public usuarioLogueado: UsuarioLogueado;

  constructor(private fb: FormBuilder,
              private perfilesService: PerfilesService,
              private route: ActivatedRoute,
              private router: Router,
              private notificacionService: NotificacionService,
              private usuarioService: UsuarioService,
              private titleService: Title) {
    this.titleService.setTitle('Editar perfil ' + TituloBanco.TITULO);
    this.perfil = new Perfil();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerFuncionalidades();
    this.obtenerFuncionalidadesPerfil();
    this.obtenerPerfil();
    this.usuarioService.consultarUsuarioLogueado().subscribe((x) => this.usuarioLogueado = x);
  }

  private crearForm() {
    this.form = this.fb.group({
      nombre: [this.perfil.nombre, Validators.compose([Validators.required,
        CustomValidators.validTextAndNumbers,
        Validators.maxLength(200)])],
      funcionalidades: this.fb.array((this.funcionalidadesCheck || []).map((funcionalidad) => {
        if (this.funcionalidadesPerfil && this.funcionalidadesPerfil.some((funcionalidadPadre) => funcionalidadPadre.id === funcionalidad.id)) {
          funcionalidad.seleccionado = true;
        }
        return this.prepararChecks(funcionalidad);
      }))
    });
  }

  private obtenerPerfil(): void {
    this.perfilesService
      .consultarPerfil(this.route.snapshot.params['id'])
      .subscribe((perfil) => {
        this.perfil = perfil;
        this.obtenerFuncionalidades();
        this.crearForm();
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

  public prepararChecks(funcionalidad: Funcionalidad): FormGroup {

    return new FormGroup({
      seleccionado: new FormControl(funcionalidad.seleccionado),
      id: new FormControl(funcionalidad.id),
      nombre: new FormControl(funcionalidad.nombre),
      codigo: new FormControl(funcionalidad.codigo)
    });
  }

  public get funcionalidades(): FormArray {
    return this.form.get('funcionalidades') as FormArray;
  }

  public aceptar(): void {

    let formGroups = this.funcionalidades.controls.filter((funcionalidad) => {
      if (funcionalidad.value.seleccionado) {
        return funcionalidad.value;
      }
    });
    let funcionalidadesAsociadas = formGroups.map((formGroup) => formGroup.value.id);

    if (funcionalidadesAsociadas !== [] && this.form.value.nombre !== '') {
      this.perfilesService
        .modificarPerfil(this.route.snapshot.params['id'], this.prepararPerfil(funcionalidadesAsociadas))
        .subscribe(
          () => {
            this.notificacionService
              .informar(['La operación se realizó con éxito'])
              .result.then(() => {
              if (this.perfil.nombre === this.usuarioLogueado.nombrePerfil) {
                this.usuarioService.cerrarSesionCidi().subscribe((response) => {
                  localStorage.clear();
                  window.location.href = response;
                });
              } else {
                this.router.navigate(['/perfiles', this.route.snapshot.params['id'], 'visualizacion']);
              }
            });
          },
          (errores) => {
            this.notificacionService
              .informar(errores, true);
          }
        );
    } else {
      if (funcionalidadesAsociadas === []) {
        this.notificacionService
          .informar(['Debe seleccionar al menos una funcionalidad.']);
      }
      if (this.form.value.nombre === '') {
        this.notificacionService
          .informar(['Debe cargar el nombre.']);
      }
    }
  }

  private prepararPerfil(funcionalidadesAsociadas: string[]): Perfil {
    let formModel = this.form.value;
    return new Perfil(
      formModel.nombre,
      formModel.fecha,
      null,
      funcionalidadesAsociadas
    );
  }

  public obtenerFuncionalidadesPerfil(): void {
    this.perfilesService
      .consultarFuncionalidadesPerfil(this.route.snapshot.params['id'])
      .subscribe((funcionalidades) => {
        this.funcionalidadesPerfil = funcionalidades;
        this.crearForm();
      });
  }
}
