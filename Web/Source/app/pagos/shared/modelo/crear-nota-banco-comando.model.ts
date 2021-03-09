export class CrearNotaBancoConsulta {
  public idLote: number;
  public nombre: string;
  public cc: string;

  constructor(idLote?: number,
              nombre?: string,
              cc?: string) {
    this.idLote = idLote;
    this.nombre = nombre;
    this.cc = cc;
  }
}
