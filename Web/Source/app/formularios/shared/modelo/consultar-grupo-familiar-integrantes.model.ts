import {Integrante} from "../../../shared/modelo/integrante.model";

export class ConsultarGrupoFamiliarIntegrantes {
  public integrantes: Integrante[];

  constructor(integrantes?: Integrante[]) {
    this.integrantes = integrantes;
  }
}
