export class BandejaLoteResultado {
  public idLote: number;
  public fechaLote: Date;
  public nroLote: number;
  public nombreLote: string;
  public cantPrestamos: number;
  public cantBeneficiarios: number;
  public montoTotal: number;
  public comision: number;
  public iva: number;
  public idTipoLote: number;
  public permiteLiberar: boolean;

  constructor(idLote?: number,
              fechaLote?: Date,
              nroLote?: number,
              nombreLote?: string,
              cantPrestamos?: number,
              cantBeneficiarios?: number,
              montoTotal?: number,
              comision?: number,
              iva?: number,
              idTipoLote?: number,
              permiteLiberar?: boolean) {
    this.idLote = idLote;
    this.fechaLote = fechaLote;
    this.nroLote = nroLote;
    this.nombreLote = nombreLote;
    this.cantPrestamos = cantPrestamos;
    this.cantBeneficiarios = cantBeneficiarios;
    this.montoTotal = montoTotal;
    this.comision = comision;
    this.iva = iva;
    this.idTipoLote = idTipoLote;
    this.permiteLiberar = permiteLiberar;
  }
}
