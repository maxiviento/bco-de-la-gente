import { BandejaPagosConsulta } from './bandeja-prestamo-consulta.model';

export class ConfirmarLoteComando {
  public idMontoDisponible: number;
  public nombreLote: string;
  public consulta: BandejaPagosConsulta;
  public idPrestamosSeleccionados: number[];
  public idAgrupamientosSeleccionados: number [];
  public monto: number;
  public iva: number;
  public comision: number;
  public fechaPago: Date;
  public fechaFinPago: Date;
  public elemento: number;
  public modalidad: number;
  public convenio: number;
  public mesesGracia: number;
  public idLoteSuaf: number;
  public idTipoLote: number;

  constructor(idMontoDisponible?: number,
              idPrestamosSeleccionados?: number[],
              idAgrupamientosSeleccionados?: number [],
              monto?: number,
              iva?: number,
              comision?: number,
              fechaPago?: Date,
              fechaFinPago?: Date,
              elemento?: number,
              modalidad?: number,
              convenio?: number,
              mesesGracia?: number,
              idLoteSuaf?: number,
              idTipoLote?: number) {
    this.idMontoDisponible = idMontoDisponible;
    this.idPrestamosSeleccionados = idPrestamosSeleccionados;
    this.idAgrupamientosSeleccionados = idAgrupamientosSeleccionados;
    this.monto = monto;
    this.iva = iva;
    this.comision = comision;
    this.fechaPago = fechaPago;
    this.fechaFinPago = fechaFinPago;
    this.elemento = elemento;
    this.modalidad = modalidad;
    this.convenio = convenio;
    this.mesesGracia = mesesGracia;
    this.idLoteSuaf = idLoteSuaf;
    this.idTipoLote = idTipoLote;
  }
}
