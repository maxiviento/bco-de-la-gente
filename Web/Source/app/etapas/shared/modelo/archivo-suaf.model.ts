export class ArchivoSuaf {
  public Archivo: any;
  public LoteId: number;
  public idTipoEntidad: number;

  constructor(archivo?: any,
              loteId?: number,
              idTipoEntidad?: number) {
    this.Archivo = archivo;
    this.LoteId = loteId;
    this.idTipoEntidad = idTipoEntidad;
  }
}
