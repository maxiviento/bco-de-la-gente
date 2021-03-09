export class SimularLoteResultado {
  public nroLote: string;
  public descripcion: string;
  public cantPrestamos: number;
  public montoLote: number;
  public comision: number;
  public iva: number;
  public totalMontoLote: number;
  public montoDisponible: number;
  public diferencia: number;
  public montoLoteActual: number;
  public cantPrestamosActual: number;

  constructor(nroLote?: string,
              descripcion?: string,
              cantPrestamos?: number,
              montoLote?: number,
              comision?: number,
              iva?: number,
              totalMontoLote?: number,
              montoDisponible?: number,
              diferencia?: number,
              montoLoteActual?: number,
              cantPrestamosActual?: number) {
    this.nroLote = nroLote;
    this.descripcion = descripcion;
    this.cantPrestamos = cantPrestamos;
    this.montoLote = montoLote;
    this.comision = comision;
    this.iva = iva;
    this.totalMontoLote = totalMontoLote;
    this.montoDisponible = montoDisponible;
    this.diferencia = diferencia;
    this.montoLoteActual = montoLoteActual;
    this.cantPrestamosActual = cantPrestamosActual;
  }
}
