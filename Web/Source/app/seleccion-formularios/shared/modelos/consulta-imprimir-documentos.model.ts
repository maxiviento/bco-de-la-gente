export class ConsultaImprimirDocumentos {
  public idsFormularios: number [];
  public idFormularioLinea: number;
  public idLote: number;
  public idOpcion: number;
  public idsReportesPagos: number [];
  public fecha: Date;
  public fechaAprobacion: boolean;

  constructor(idsFormularios?: number [],
              idFormularioLinea?: number,
              idLote?: number,
              idOpcion?: number,
              idsReportesPagos?: number [],
              fecha?: Date,
              fechaAprobacion?: boolean) {

    this.idsFormularios = idsFormularios;
    this.idFormularioLinea = idFormularioLinea;
    this.idLote = idLote;
    this.idOpcion = idOpcion;
    this.idsReportesPagos = idsReportesPagos;
    this.fecha = fecha;
    this.fechaAprobacion = fechaAprobacion;
  }
}
