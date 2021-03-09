export class FormaPagoItem {
  public id: number;
  public valor: string;
  public tipo: string;

  constructor(id?: number,
              valor?: string,
              tipo?: string) {
    this.id = id;
    this.valor = valor;
    this.tipo = tipo;
  }
}
