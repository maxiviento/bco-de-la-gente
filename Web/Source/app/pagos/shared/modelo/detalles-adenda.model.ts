export class DetallesAdenda {
  public idLote: number;
  public nroDetalle: number;
  public nroPrestamoChecklist: number;
  public agrega: boolean;

  constructor(idLote?: number,
              nroDetalle?: number,
              nroPrestamoChecklist?: number,
              agrega?: boolean) {
    this.idLote = idLote;
    this.nroDetalle = nroDetalle;
    this.nroPrestamoChecklist = nroPrestamoChecklist;
    this.agrega = agrega;
  }
}
