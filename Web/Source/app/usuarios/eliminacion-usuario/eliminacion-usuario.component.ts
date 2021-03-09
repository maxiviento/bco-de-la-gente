import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemUsuario } from '../shared-usuarios/modelo/item-usuario.model';
import { UsuariosService } from '../shared-usuarios/usuarios.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { MotivoBaja } from '../shared-usuarios/modelo/motivo-baja.model';
import { FormUtils } from '../../shared/forms/forms-utils';
import { Title } from '@angular/platform-browser';
import { UsuarioService } from '../../core/auth/usuario.service';
import { Usuario } from '../../core/auth/modelos/usuario.model';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-usuario',
  templateUrl: './eliminacion-usuario.component.html',
  styleUrls: ['./eliminacion-usuario.component.scss']
})

export class EliminacionUsuarioComponent implements OnInit {
  public form: FormGroup;
  public usuario: ItemUsuario;
  public motivosBaja: MotivoBaja[];
  public fechaActual: Date;
  public usuarioLogueado: Usuario;

  constructor(private fb: FormBuilder,
              private usuariosService: UsuariosService,
              private route: ActivatedRoute,
              private router: Router,
              private notificacionService: NotificacionService,
              private usuarioService: UsuarioService,
              private titleService: Title) {
    this.titleService.setTitle('Eliminar usuario ' + TituloBanco.TITULO);
    this.usuario = new ItemUsuario();
    this.fechaActual = new Date();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerMotivosBaja();
    this.obtenerUsuario();
    this.usuarioService.obtenerUsuarioActual().subscribe((x) => this.usuarioLogueado = x);
  }

  private crearForm() {

    this.form = this.fb.group({
      nombre: [''],
      apellido: [''],
      cuil: [''],
      fechaBaja: [''],
      nombrePerfil: [''],
      motivoBajaId: ['', Validators.required]
    });
  }

  private obtenerUsuario(): void {
    this.usuariosService
      .consultarUsuario(this.route.snapshot.params['id'])
      .subscribe((usuario) => {
        this.usuario = usuario;
      });
  }

  public obtenerMotivosBaja() {
    this.usuariosService
      .obtenerMotivosBaja()
      .subscribe((motiviosBaja) => {
        this.motivosBaja = motiviosBaja;
      });
  }

  public eliminarUsuario(): void {
    if (!this.form.valid) {
      FormUtils.validate(this.form);
      this.notificacionService
        .informar(['El motivo de baja es obligatorio.']);
      return;
    } else {
      let formModel = this.form.value;
      this.usuariosService
        .eliminarUsuario(this.route.snapshot.params['id'], formModel.motivoBajaId)
        .subscribe(
          () => {
            this.notificacionService
              .informar(['La operación se realizó con éxito'])
              .result.then(() => {
              if (this.usuarioLogueado.cuil === this.usuario.cuil.toString()) {
                this.usuarioService.cerrarSesionCidi().subscribe((response) => {
                  this.clearLocalStorage();
                });
              }
              this.router.navigate(['/usuarios', this.route.snapshot.params['id'], 'visualizacion']);
            });
          },
          (errores) => {
            this.notificacionService
              .informar(errores, true);
          }
        );
    }
  }

  public clearLocalStorage(): void {
    localStorage.clear();
    document.location.reload(true);
  }
}
