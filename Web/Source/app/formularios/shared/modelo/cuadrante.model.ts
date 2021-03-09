import { JsonProperty } from '../../../shared/map-utils';
export class Cuadrante {
  public orden: number;
  public nombre: string;
  @JsonProperty('id')
  public idCuadrante: number;
  public idTipoSalida: number;
  public descripcion: string;

  public constructor(orden?: number, nombre?: string, idCuadrante?: number, idTipoSalida?: number, descripcion?: string) {
    this.orden = orden;
    this.nombre = nombre;
    this.idCuadrante = idCuadrante;
    this.idTipoSalida = idTipoSalida;
    this.descripcion = descripcion;
  }
}
