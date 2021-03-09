export class GastoGrupo {
  public id: number;
  public concepto: string;
  public monto: number;

  constructor(id?: number,
              nombre?: string,
              valor?: number) {
    this.id = id;
    this.concepto = nombre;
    this.monto = valor;
  }
}
