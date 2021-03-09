import { SituacionDomicilioIntegrante } from './situacion-domicilio-integrante.model';

export class GrupoDomicilioIntegranteResultado {
  public id: number;
  public idGrupoUnico: number;
  public listadoPersonas: SituacionDomicilioIntegrante[];

  constructor(id?: number,
              idGrupoUnico?: number,
              listadoPersonas?: SituacionDomicilioIntegrante[]) {
    this.id = id;
    this.idGrupoUnico = idGrupoUnico;
    this.listadoPersonas = listadoPersonas;
  }
}
