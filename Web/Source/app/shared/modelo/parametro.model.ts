import { VigenciaParametro } from './vigencia-parametro.model';
import { JsonProperty } from '../map-utils';

export class Parametro {

  public nombre: string;
  public descripcion: string;
  @JsonProperty({clazz: VigenciaParametro})
  public vigenciasParametro: VigenciaParametro[];

  constructor(nombre?: string,
              descripcion?: string,
              vigenciasParametro?: VigenciaParametro[]) {

    this.nombre = nombre;
    this.descripcion = descripcion;
    this.vigenciasParametro = vigenciasParametro;

  }
}
