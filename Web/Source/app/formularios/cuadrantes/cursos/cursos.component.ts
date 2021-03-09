import { CustomValidators } from './../../../shared/forms/custom-validators';
import { Component, OnInit } from '@angular/core';
import { CursosService } from './cursos.service';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SolicitudCurso } from '../../shared/modelo/solicitud-curso.model';
import { Curso } from '../../shared/modelo/curso.model';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormulariosService } from '../../shared/formularios.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { AlMenosUnCursoSeleccionadoValidador } from './cursos-validator';

@Component({
  selector: 'bg-cursos',
  templateUrl: './cursos.component.html',
  styleUrls: ['./cursos.component.scss'],
  providers: [CursosService],
})

export class CursoComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public cursos: Curso[];

  constructor(private fb: FormBuilder,
              private cursosService: CursosService,
              private notificacionService: NotificacionService,
              private formularioService: FormulariosService) {
    super();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.cursosService.consultarCursos()
      .subscribe((cursos) => {
          this.cursos = cursos;
          this.marcarCursosSeleccionados();
          this.crearForm(this.cursos);
          if (!this.editable) {
            this.form.disable();
          }
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  public categorias() {
    return this.form.get('categorias') as FormArray;
  }

  public actualizarDatos() {
    let solicitudesCurso = this.getCursosSeleccionados();
    if (this.formulario.idEstado === 3) {
      this.formularioService.actualizarCursos(this.formulario.id, solicitudesCurso)
        .subscribe(() => {
          this.formulario.solicitudesCurso = solicitudesCurso;
        });
    } else {
      this.formularioService.actualizarCursosAsociativas(this.formulario.idAgrupamiento, solicitudesCurso).subscribe(() => {
        this.formulario.solicitudesCurso = solicitudesCurso;
      });
    }
  }

  public esValido(): boolean {
    this.form.markAsDirty({onlySelf: true});
    if (!this.editable) {  // estamos en el ver
      return true;
    } else {
      return this.form.valid;
    }
  }

  private crearForm(cursos: Curso[] = []): void {
    let categorias = this.fb.array([]);
    cursos.forEach((curso) => {
      let categoria = categorias.controls.find((cat) => cat.get('nombre').value === curso.nombreTipoCurso);
      if (!categoria) {
        categoria = this.fb.group({
          nombre: curso.nombreTipoCurso,
          cursos: this.fb.array([]),
        });
        categorias.push(categoria);
      }
      let cursoGroup = this.fb.group({
        id: [curso.id],
        nombre: [curso.nombre],
        seleccionado: [curso.seleccionado],
      });
      if (curso.nombre.includes('OTRO')) {
        let otrosDescripcion = this.encontrarDescripcionOtros(curso.nombreTipoCurso);
        cursoGroup.addControl('descripcion', new FormControl(otrosDescripcion, Validators.compose([Validators.maxLength(100),
        CustomValidators.validTextAndNumbers])));
        (<FormGroup>categoria).addControl('otros', cursoGroup);
      } else {
        (<FormArray>categoria.get('cursos')).push(cursoGroup);
      }
    });
    this.form = this.fb.group({
      categorias,
    }, this.formulario.detalleLinea.conCurso ? { validator: AlMenosUnCursoSeleccionadoValidador } : null);
  }

  private getCursosSeleccionados(): SolicitudCurso[] {
    let solicitudesCurso = [];
    let categorias = this.form.get('categorias').value;
    categorias.forEach((categoria) => {
      let solicitudCursos = new SolicitudCurso();
      let cursosSeleccionados = categoria.cursos.filter((curso) => curso.seleccionado).map((curso) => new Curso(curso.id));
      let otros = categoria.otros;
      if (otros.descripcion) {
        cursosSeleccionados.push(new Curso(otros.id));
        solicitudCursos.descripcion = otros.descripcion;
      }
      solicitudCursos.cursos = cursosSeleccionados;

      if (solicitudCursos.cursos.length) {
        solicitudesCurso.push(solicitudCursos);
      }
    });
    return solicitudesCurso;
  }

  private marcarCursosSeleccionados(): void {
    if (this.formulario.solicitudesCurso) {
      this.formulario.solicitudesCurso.forEach((solicitudCursos) => {
        solicitudCursos.cursos.forEach((cur) => {
          this.cursos.filter((curso) => curso.id === cur.id)[0].seleccionado = true;
        });
      });
    } else {
      this.formulario.solicitudesCurso = [];
    }
  }

  private encontrarDescripcionOtros(nombreCategoria: string): string {
    if (this.formulario.solicitudesCurso) {
      let seleccionadosCategoria = this.formulario.solicitudesCurso.filter(
        (x) => x.nombreTipoCurso === nombreCategoria);
      return seleccionadosCategoria && seleccionadosCategoria.length ? seleccionadosCategoria[0].descripcion : '';
    }
    return '';
  }

  inicializarDeNuevo(): boolean {
    return false;
  }
}
