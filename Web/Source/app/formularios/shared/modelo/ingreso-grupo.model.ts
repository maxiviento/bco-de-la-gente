export class IngresoGrupo{
  public id: number;
  public nombre: string;
  public descripcion: string;
  public idConcepto: number;
  public valor: number;

  constructor(id?: number,
              nombre?: string,
              descripcion?: string,
              idConcepto?: number,
              valor?: number) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.idConcepto = idConcepto;
    this.valor = valor;
  }
}
