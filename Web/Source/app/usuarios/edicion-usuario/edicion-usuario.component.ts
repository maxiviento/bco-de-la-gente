import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemUsuario } from '../shared-usuarios/modelo/item-usuario.model';
import { UsuariosService } from '../shared-usuarios/usuarios.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { Perfil } from '../shared-usuarios/modelo/perfil.model';
import { FormUtils } from '../../shared/forms/forms-utils';
import { UsuarioService } from '../../core/auth/usuario.service';
import { Usuario } from '../../core/auth/modelos/usuario.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-usuario',
  templateUrl: './edicion-usuario.component.html',
  styleUrls: ['./edicion-usuario.component.scss']
})

export class EdicionUsuarioComponent implements OnInit {
  public form: FormGroup;
  public usuario: ItemUsuario;
  public usuarioLogueado: Usuario;
  public perfiles: Perfil[];

  constructor(private fb: FormBuilder,
              private usuariosService: UsuariosService,
              private route: ActivatedRoute,
              private router: Router,
              private notificacionService: NotificacionService,
              private usuarioService: UsuarioService,
              private titleService: Title) {
    this.titleService.setTitle('Editar usuario ' + TituloBanco.TITULO);
    this.usuario = new ItemUsuario();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerPerfiles();
    this.obtenerUsuario();
    this.usuarioService.obtenerUsuarioActual().subscribe((x) => this.usuarioLogueado = x);
  }

  private crearForm() {
    this.form = this.fb.group({
      perfilId: [this.usuario.perfilId || '', Validators.required]
    });
  }

  private obtenerUsuario(): void {
    this.usuariosService
      .consultarUsuario(this.route.snapshot.params['id'])
      .subscribe((usuario) => {
        this.usuario = usuario;
        this.crearForm();
      });
  }

  public obtenerPerfiles() {
    this.usuariosService.obtenerPerfiles()
      .subscribe((perfiles) => {
        this.perfiles = perfiles;
      });
  }

  public modificarUsuario() {
    if (!this.form.valid) {
      FormUtils.validate(this.form);
      this.notificacionService
        .informar(['Hay errores en el formulario.']);
      return;
    } else {
      let formModel = this.form.value;
      this.usuariosService
        .modificarUsuario(this.route.snapshot.params['id'], formModel.perfilId)
        .subscribe(
          () => {
            this.notificacionService
              .informar(['La operación se realizó con éxito'])
              .result.then(() => {
              if (this.usuarioLogueado.cuil === this.usuario.cuil.toString()) {
                this.usuarioService.cerrarSesionCidi().subscribe((response) => {
                  localStorage.clear();
                  window.location.href = response;
                });
              } else {
                this.router.navigate(['/usuarios', this.route.snapshot.params['id'], 'visualizacion']);
              }
            });
          },
          (errores) => {
            this.notificacionService
              .informar(errores, true);
          }
        );
    }
    ;
  }
}
