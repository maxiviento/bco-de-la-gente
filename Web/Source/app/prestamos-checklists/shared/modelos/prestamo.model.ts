import { RequisitoPrestamo } from './requisito-prestamo';
import { JsonProperty } from '../../../shared/map-utils';
export class Prestamo {
  public id: number;
  public numeroPrestamo: number;
  public numDevengado: string;
  public version: string;
  public totalFolios: number;
  public fechaAlta: Date;
  public usuarioAlta: string;
  public idEstado: number;
  public esSolicGarante: boolean;
  public idTipoGarantia: number;
  public observaciones: string;
  public motivosRechazo: string;
  public requisitos: RequisitoPrestamo [];
  @JsonProperty('color')
  public colorLinea: string;
  @JsonProperty('nombre')
  public nombreLinea: string;
  @JsonProperty('descripcion')
  public descripcionLinea: string;
  public idTipoIntegrante: number;
  public idEtapaEstadoLinea: number;
  public idLinea: number;
  public idFormularioLinea: number;

  constructor() {
    this.id = undefined;
    this.numeroPrestamo = undefined;
    this.numDevengado = undefined;
    this.version = undefined;
    this.totalFolios = undefined;
    this.fechaAlta = undefined;
    this.usuarioAlta = undefined;
    this.idEstado = undefined;
    this.esSolicGarante = undefined;
    this.idTipoGarantia = undefined;
    this.observaciones = undefined;
    this.requisitos = [];
    this.colorLinea = undefined;
    this.nombreLinea = undefined;
    this.descripcionLinea = undefined;
    this.motivosRechazo = undefined;
    this.idTipoIntegrante = undefined;
    this.idEtapaEstadoLinea = undefined;
    this.idLinea = undefined;
    this.idFormularioLinea = undefined;
  }
}
