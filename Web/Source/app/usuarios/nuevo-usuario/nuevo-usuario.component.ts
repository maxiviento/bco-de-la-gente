import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemUsuario } from '../shared-usuarios/modelo/item-usuario.model';
import { UsuariosService } from '../shared-usuarios/usuarios.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { Perfil } from '../shared-usuarios/modelo/perfil.model';
import { CustomValidators, isEmpty } from '../../shared/forms/custom-validators';
import { FormUtils } from '../../shared/forms/forms-utils';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-usuario',
  templateUrl: './nuevo-usuario.component.html',
  styleUrls: ['./nuevo-usuario.component.scss']
})

export class NuevoUsuarioComponent implements OnInit {
  public form: FormGroup;
  public usuario: ItemUsuario;
  public perfiles: Perfil[];
  public mostrarUsuario: boolean;

  constructor(private fb: FormBuilder,
              private usuariosService: UsuariosService,
              private route: ActivatedRoute,
              private router: Router,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo usuario ' + TituloBanco.TITULO);
    this.usuario = new ItemUsuario();
    this.mostrarUsuario = false;
  }

  public ngOnInit() {
    this.crearForm();
    this.obtenerPerfiles();
  }

  private crearForm() {
    this.form = this.fb.group({
      cuil: ['', Validators.compose([
        Validators.maxLength(11),
        Validators.minLength(11),
        CustomValidators.number])],
      perfilId: ['', Validators.required]
    });
  }

  public consultarUsuarioCidi(): void {
    if (!this.form.value.cuil) {
      this.notificacionService
        .informar(['Debe ingresar un CUIL']);
      return;
    }
    if (!this.form.get('cuil').valid) {
      FormUtils.validate(this.form.get('cuil'));
      this.notificacionService
        .informar(['Hay errores en la consulta.']);
      return;
    } else {
      this.usuariosService
        .consultarUsuarioCidi(this.form.value.cuil)
        .subscribe(
          (usuario) => {
            this.usuario = usuario;
            this.mostrarUsuario = true;
          },
          (errores) => {
            this.mostrarUsuario = false;
            this.notificacionService
              .informar(errores, true)
              .result.then(() => {
              this.form.get('cuil').setValue('')
            })
          }
        );
    }
  }

  public obtenerPerfiles() {
    this.usuariosService.obtenerPerfiles()
      .subscribe(
        (perfiles) => {
          this.perfiles = perfiles;
        }
      );
  }

  public registrarUsuario() {
    if (isEmpty(this.usuario.id)) {
      this.notificacionService
        .informar(['Debe buscar una persona por CUIL']);
      return;
    }
    if (!this.form.valid) {
      FormUtils.validate(this.form);
      this.notificacionService
        .informar(['Hay errores en el formulario.']);
      return;
    } else {
      this.usuariosService
        .registrarUsuario(this.prepararItemUsuario())
        .subscribe(
          (usuarioId) => {
            this.notificacionService
              .informar(['La operación se realizó con éxito'])
              .result.then(() => {
              this.router.navigate(['/usuarios', usuarioId, 'visualizacion']);
            });
          },
          (errores) => {
            this.notificacionService
              .informar(errores, true);
          }
        );
    }
  }

  private prepararItemUsuario(): ItemUsuario {
    let formModel = this.form.value;
    return new ItemUsuario(
      null,
      null,
      null,
      this.usuario.cuil,
      formModel.perfilId,
    );
  }
}
