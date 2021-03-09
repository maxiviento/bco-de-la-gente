export class DetalleLote {
  public numero: number;
  public nombre: string;
  public fechaCreacionString: string;
  public montoPrestamos: number;
  public comision: number;
  public iva: number;
  public montoLote: number;
  public cantidadPrestamos: number;
  public cantidadBeneficiarios: number;
  public nroMonto: number;

  constructor(numero?: number,
              nombre?: string,
              fechaCreacionString?: string,
              montoPrestamos?: number,
              comision?: number,
              iva?: number,
              montoLote?: number,
              cantidadPrestamos?: number,
              cantidadBeneficiarios?: number,
              nroMonto?: number) {
    this.numero = numero;
    this.nombre = nombre;
    this.fechaCreacionString = fechaCreacionString;
    this.montoPrestamos = montoPrestamos;
    this.comision = comision;
    this.iva = iva;
    this.montoLote = montoLote;
    this.cantidadPrestamos = cantidadPrestamos;
    this.cantidadBeneficiarios = cantidadBeneficiarios;
    this.nroMonto = nroMonto;
  }
}
