import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { UsuariosService } from './shared-usuarios/usuarios.service';
import { FiltrosUsuarios } from './shared-usuarios/modelo/filtros-usuarios.model';
import { Perfil } from './shared-usuarios/modelo/perfil.model';
import { ItemUsuario } from './shared-usuarios/modelo/item-usuario.model';
import { NotificacionService } from '../shared/notificacion.service';
import { CustomValidators } from '../shared/forms/custom-validators';
import { FormUtils } from '../shared/forms/forms-utils';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'vd-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.scss']

})

export class UsuariosComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public perfiles: Perfil[];
  public usuarios: ItemUsuario[];
  public mostrarBajas: boolean = false;

  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  constructor(private fb: FormBuilder,
              private usuariosService: UsuariosService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Usuarios ' + TituloBanco.TITULO);
    this.usuarios = [];
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerPerfiles();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('usuario')) {
      UsuariosService.guardarFiltros(null);
    }
  }

  private configurarPaginacion() {
    let filtros;
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        filtros = this.prepararFiltrosUsuarios();
        filtros.numeroPagina = params.numeroPagina;
        return this.usuariosService
          .consultarUsuarios(filtros);
      })
      .share();

    (<Observable<ItemUsuario[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((usuarios) => {
        this.usuarios = usuarios;
        UsuariosService.guardarFiltros(filtros);
      });
  }

  private crearForm() {
    this.form = this.fb.group({
      cuil: ['', Validators.compose([Validators.maxLength(11), Validators.minLength(11), CustomValidators.number])],
      perfilId: [''],
      incluyeBajas: null
    });
  }

  public consultarUsuarios(pagina?: number) {
    if (!this.form.valid) {
      FormUtils.validate(this.form);
      this.notificacionService
        .informar(['Hay errores en la consulta.']);
      return;
    } else {
      this.paginaModificada.next(pagina);
    }
  }

  private prepararFiltrosUsuarios(): FiltrosUsuarios {
    let formModel = this.form.value;

    this.mostrarBajas = formModel.incluyeBajas;

    return new FiltrosUsuarios(
      formModel.cuil,
      formModel.perfilId,
      formModel.incluyeBajas
    );
  }

  public obtenerPerfiles() {
    this.usuariosService.obtenerPerfiles()
      .subscribe((perfiles) => {
        this.perfiles = perfiles;
      });
  }

  private reestablecerFiltros() {
    let filtrosGuardados = UsuariosService.recuperarFiltros();
    if (filtrosGuardados) {
      this.form.patchValue(filtrosGuardados);
      this.consultarUsuarios();
    }
  }
}
