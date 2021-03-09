import {Cuadrante} from './cuadrante.model';
import {CondicionesPrestamo} from './condiciones-prestamo.model';
import { JsonProperty } from '../../../shared/map-utils';

export class InformacionLinea {
  public idLinea: number;
  public id: number;
  @JsonProperty({clazz: Cuadrante})
  public cuadrantes: Cuadrante[];
  public condicionesPrestamo: CondicionesPrestamo;
  public color: string;
  public idDetalleLinea: number;

  public constructor(idLinea?: number,
                     cuadrantes?: Cuadrante[],
                     condicionesPrestamo?: CondicionesPrestamo,
                     color?: string,
                     idDetalleLinea?: number) {
    this.idLinea = idLinea;
    this.cuadrantes = cuadrantes;
    this.condicionesPrestamo = condicionesPrestamo;
    this.color = color;
    this.idDetalleLinea = idDetalleLinea;
  }
}
