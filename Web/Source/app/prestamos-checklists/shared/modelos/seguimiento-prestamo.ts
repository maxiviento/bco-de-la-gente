import { JsonProperty } from '../../../shared/map-utils';
export class SeguimientoPrestamo {
  public id: number;
  @JsonProperty('estadoNombre')
  public nombreEstado: string;
  @JsonProperty('usuarioCuil')
  public nombreUsuario: string;
  public fecha: Date;
  public observaciones: string;
  public nroFormulario: number;

  constructor() {
    this.id = undefined;
    this.nombreEstado = undefined;
    this.nombreUsuario = undefined;
    this.fecha = undefined;
    this.observaciones = undefined;
    this.nroFormulario = undefined;
  }

}
