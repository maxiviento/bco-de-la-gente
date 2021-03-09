import {DatosPersona} from './datos-persona.model';
import {List} from 'lodash';

export class BandejaComponerPrestamoResultado {
  public id: number;
  public localidad: string;
  public linea: string;
  public apellidoNombreSolicitante: string;
  public cuilSolicitante: string;
  public fechaEnvio: Date;
  public origen: string;
  public estado: string;
  public solicitante: DatosPersona;
  public garantes: List<DatosPersona>;

  constructor(id?: number,
              localidad?: string,
              linea?: string,
              apellidoNombreSolicitante?: string,
              cuilSolicitante?: string,
              fechaEnvio?: Date,
              origen?: string,
              estado?: string,
              solicitante?: DatosPersona,
              garantes?: List<DatosPersona>) {
    this.id = id;
    this.localidad = localidad;
    this.linea = linea;
    this.apellidoNombreSolicitante = apellidoNombreSolicitante;
    this.cuilSolicitante = cuilSolicitante;
    this.fechaEnvio = fechaEnvio;
    this.origen = origen;
    this.estado = estado;
    this.solicitante = solicitante;
    this.garantes = garantes;
  }
}
