import { Curso } from './curso.model';
export class SolicitudCurso {
  public cursos: Curso[] = [];
  public nombreTipoCurso: string;
  public descripcion: string;

  constructor(cursos?: Curso[], descripcion?: string) {
    this.cursos = cursos;
    this.descripcion = descripcion;
  }
}
