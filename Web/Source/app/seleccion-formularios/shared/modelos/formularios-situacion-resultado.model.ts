export class FormulariosSituacionResultado {
  public lineaPrestamo: string;
  public origFormulario: string;
  public nroFormulario: string;
  public fecAlta: Date;
  public estFormulario: string;
  public motRechazoForm: string;
  public nroPrestamo: string;
  public estPrestamo: string;
  public motRechazoPrest: string;
  public montoPrestamo: string;
  public cantCuotas: string;
  public cantCuotasPagadas: string;
  public idRechFrom: string;
  public idRechPrest: string;
  public idFormulario: number;
  public idPrestamo: number;
  public numeroCaja: string;
  public situacionGarantia: string;
  public tienePlanCuotas: boolean;
  public fecSeguimiento: Date;
  public idEstadoFormulario: number;
  public idEstadoPrestamo: number;

  constructor(lineaPrestamo?: string,
              origFormulario?: string,
              nroFormulario?: string,
              fecAlta?: Date,
              estFormulario?: string,
              motRechazoForm?: string,
              nroPrestamo?: string,
              estPrestamo?: string,
              motRechazoPrest?: string,
              montoPrestamo?: string,
              cantCuotas?: string,
              cantCuotasPagadas?: string,
              idRechFrom?: string,
              idRechPrest?: string,
              idFormulario?: number,
              idPrestamo?: number,
              numeroCaja?: string,
              situacionGarantia?: string,
              tienePlanCuotas?: boolean,
              fecSeguimiento?: Date,
              idEstadoFormulario?: number,
              idEstadoPrestamo?: number) {
    this.lineaPrestamo = lineaPrestamo;
    this.origFormulario = origFormulario;
    this.nroFormulario = nroFormulario;
    this.fecAlta = fecAlta;
    this.estFormulario = estFormulario;
    this.motRechazoForm = motRechazoForm;
    this.nroPrestamo = motRechazoPrest;
    this.estPrestamo = estPrestamo;
    this.motRechazoPrest = motRechazoPrest;
    this.montoPrestamo = montoPrestamo;
    this.cantCuotas = cantCuotas;
    this.cantCuotasPagadas = cantCuotasPagadas;
    this.idRechFrom = idRechFrom;
    this.idRechPrest = idRechPrest;
    this.idFormulario = idFormulario;
    this.idPrestamo = idPrestamo;
    this.numeroCaja = numeroCaja;
    this.situacionGarantia = situacionGarantia;
    this.tienePlanCuotas = tienePlanCuotas;
    this.fecSeguimiento = fecSeguimiento;
    this.idEstadoFormulario = idEstadoFormulario;
    this.idEstadoPrestamo = idEstadoPrestamo;
  }
}
