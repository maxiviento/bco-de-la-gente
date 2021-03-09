export class BajaMotivoRechazoComando {
  public id: number;
  public idMotivoBaja: number;
  public idAmbito: number;

  constructor(id?: number, idMotivoBaja?: number, idAmbito?: number) {
    this.id = id;
    this.idMotivoBaja = idMotivoBaja;
    this.idAmbito = idAmbito;
  }
}
