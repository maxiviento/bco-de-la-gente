export class SolicitudCursosSeleccionados {
  public cursos: number[];
  public descripcion: string;
  public nombreTipoCurso: string;

  constructor(cursos?: number[],
              descripcion?: string,
              nombreTipoCurso?: string) {
    this.cursos = cursos;
    this.descripcion = descripcion;
    this.nombreTipoCurso = nombreTipoCurso;
  }
}
