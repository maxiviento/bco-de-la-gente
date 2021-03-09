export class Curso {
  public id: number;
  public nombre: string;
  public seleccionado: boolean;
  public idTipoCurso: number;
  public nombreTipoCurso: string;

  public constructor(id?: number,
                     nombre?: string,
                     seleccionado?: boolean,
                     idTipoCurso?: number,
                     nombreTipoCurso?: string) {
    this.id = id;
    this.nombre = nombre;
    this.seleccionado = seleccionado;
    this.idTipoCurso = idTipoCurso;
    this.nombreTipoCurso = nombreTipoCurso;
  }
}
