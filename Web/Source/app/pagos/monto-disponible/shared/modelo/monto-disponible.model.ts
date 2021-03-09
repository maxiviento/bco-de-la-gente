import { MovimientosMontoDisponible } from './movimientos-monto-disponible.model';
import { JsonProperty } from '../../../../shared/map-utils';

export class MontoDisponible {
  public id: number;
  public descripcion: string;
  public idBanco: number;
  public idSucursal: number;
  public monto: number;
  public nroMonto: number;
  public fechaDepositoBancario: Date;
  public fechaInicioPago: Date;
  public fechaFinPago: Date;
  public idMotivoBaja?: number;
  public nombreMotivoBaja?: string;
  public fechaUltimaModificacion?: Date;
  public cuilUsuarioUltimaModificacion?: string;
  public saldo?: number;
  @JsonProperty({clazz: MovimientosMontoDisponible})
  public movimientos: MovimientosMontoDisponible[];

  constructor(id?: number,
              descripcion?: string,
              idBanco?: number,
              idSucursal?: number,
              monto?: number,
              fechaDepositoBancario?: Date,
              fechaInicioPago?: Date,
              fechaFinPago?: Date,
              nroMonto?: number,
              idMotivoBaja?: number,
              nombreMotivoBaja?: string,
              cuilUsuarioUltimaModificacion?: string,
              fechaUltimaModificacion?: Date,
              saldo?: number,
              movimientos?: MovimientosMontoDisponible[]) {
    this.id = id;
    this.descripcion = descripcion;
    this.idBanco = idBanco;
    this.idSucursal = idSucursal;
    this.monto = monto;
    this.fechaDepositoBancario = fechaDepositoBancario;
    this.fechaInicioPago = fechaInicioPago;
    this.fechaFinPago = fechaFinPago;
    this.nroMonto = nroMonto;
    this.idMotivoBaja = idMotivoBaja;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.cuilUsuarioUltimaModificacion = cuilUsuarioUltimaModificacion;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.movimientos = movimientos;
    this.saldo = saldo;
  }

  public estaDadoDeBaja(): boolean {
    return this.idMotivoBaja != null;
  }
}
