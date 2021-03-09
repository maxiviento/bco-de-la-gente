import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PerfilesService } from '../shared-perfiles/perfiles.service';
import { Perfil } from '../shared-perfiles/modelo/perfil.model';
import { Funcionalidad } from '../shared-perfiles/modelo/funcionalidad.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-visualizacion-perfil',
  templateUrl: './visualizacion-perfil.component.html',
  styleUrls: ['./visualizacion-perfil.component.scss']
})

export class VisualizacionPerfilComponent implements OnInit {
  public form: FormGroup;
  public funcionalidadesCheck: Funcionalidad[] = [];
  public perfil: Perfil;

  constructor(private fb: FormBuilder,
              private perfilesService: PerfilesService,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver perfil ' + TituloBanco.TITULO);
    this.perfil = new Perfil();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerPerfil();
  }

  private crearForm() {
    this.form = this.fb.group({
      nombre: [this.perfil.nombre],
      motivoBaja: [null],
      funcionalidades: this.fb.array((this.funcionalidadesCheck || []).map((funcionalidad) => {
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
      .consultarFuncionalidadesPerfil(this.route.snapshot.params['id'])
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

}
