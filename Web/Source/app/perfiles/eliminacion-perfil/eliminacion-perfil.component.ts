import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PerfilesService } from '../shared-perfiles/perfiles.service';
import { Perfil } from '../shared-perfiles/modelo/perfil.model';
import { Funcionalidad } from '../shared-perfiles/modelo/funcionalidad.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { Motivo } from '../shared-perfiles/modelo/motivo.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-perfil',
  templateUrl: './eliminacion-perfil.component.html',
  styleUrls: ['./eliminacion-perfil.component.scss']
})

export class EliminacionPerfilComponent implements OnInit {
  public form: FormGroup;
  public funcionalidadesCheck: Funcionalidad[];
  public perfil: Perfil;
  public motivoBajas: Motivo[];

  constructor(private fb: FormBuilder,
              private perfilesService: PerfilesService,
              private route: ActivatedRoute,
              private router: Router,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Eliminar perfil ' + TituloBanco.TITULO);
    this.perfil = new Perfil();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerFuncionalidades();
    this.obtenerPerfil();
    this.obtenerMotivos();
  }

  private crearForm() {
    this.form = this.fb.group({
      nombre: [this.perfil.nombre],
      funcionalidades: this.fb.array((this.funcionalidadesCheck || []).map((funcionalidad) => {
        return this.prepararChecks(funcionalidad);
      })),
      motivoBaja: ['', Validators.required]
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
      .consultarFuncionalidadesPerfil(this.route.snapshot.params['id'])
      .subscribe((funcionalidades) => {
        this.funcionalidadesCheck = funcionalidades;
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

  public obtenerMotivos(): void {
    this.perfilesService
      .obtenerMotivos()
      .subscribe((motivos) => {
        this.motivoBajas = motivos;
        this.crearForm();
      });
  }

  public eliminarPerfil(): void {
    let formModel = this.form.value;
    this.perfilesService
      .eliminarPerfil(this.route.snapshot.params['id'], formModel.motivoBaja)
      .subscribe(
        () => {
          this.notificacionService
            .informar(['La operación se realizó con éxito'])
            .result.then(() => {
            this.router.navigate(['/perfiles', this.route.snapshot.params['id'], 'visualizacion']);
          });
        },
        (errores) => {
          this.notificacionService
            .informar(errores, true);
        }
      );
  }
}
