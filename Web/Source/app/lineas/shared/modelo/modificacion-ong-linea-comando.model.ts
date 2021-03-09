import { OngLinea } from './ong-linea.model';

export class ModificacionOngLineaComando {
  public idLinea: number;
  public lsOngAgregadas: OngLinea[];
  public lsOngQuitadas: OngLinea[];

  public constructor(
    idLinea?: number,
    lsOngAgregadas?: OngLinea[],
    lsOngQuitadas?: OngLinea[]
  ) {
    this.idLinea = idLinea;
    this.lsOngAgregadas = lsOngAgregadas;
    this.lsOngQuitadas = lsOngQuitadas;
  }
}
