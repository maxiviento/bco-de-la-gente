export class SituacionPersonasResultadoVista {
  public idNumero: number;
  public nombreApellido: string;
  public sexo: string;
  public numeroDocumento: string;
  public numeroSolicitud: number;
  public fechaPago: Date;
  public idProducto: number;
  public producto: string;
  public condicion: string;
  public montoAdeudado: number;
  public montoCredito: number;
  public montoAbonado: number;

  constructor(idNumero?: number,
              nombreApellido?: string,
              sexo?: string,
              nombreSexo?: string,
              tipoDocumento?: string,
              numeroDocumento?: string,
              numeroSolicitud?: number,
              fechaPago?: Date,
              idProducto?: number,
              producto?: string,
              condicion?: string,
              montoAdeudado?: number,
              montoCredito?: number,
              montoAbonado?: number) {
    this.idNumero = idNumero;
    this.nombreApellido = nombreApellido;
    this.sexo = sexo;
    this.numeroDocumento = numeroDocumento;
    this.numeroSolicitud = numeroSolicitud;
    this.fechaPago = fechaPago;
    this.idProducto = idProducto;
    this.producto = producto;
    this.condicion = condicion;
    this.montoAdeudado = montoAdeudado;
    this.montoCredito = montoCredito;
    this.montoAbonado = montoAbonado;
  }
}
