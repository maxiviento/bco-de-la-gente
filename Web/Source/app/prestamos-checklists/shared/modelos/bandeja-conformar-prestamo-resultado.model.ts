import { Persona } from '../../../shared/modelo/persona.model';

export class BandejaConformarPrestamoResultado{
  public id: number;
  public localidad: string;
  public linea: string;
  public idLinea: number;
  public apellidoNombreSolicitante: string;
  public cuilSolicitante: string;
  public fechaEnvio: Date;
  public origen: string;
  public estado: string;
  public solicitante: Persona;
  public garantes: Persona[];
  public nroAgrupamiento: number;
  public tipoIntegranteSocio: number;
  public nombreIntegranteSocio: string;
  public seleccionado: boolean;
  public esSeleccionable: boolean;
  public puedeGenerarPrestamo: boolean;
  public fechaInicio: Date;
  public nroFormulario: string;
  public fechaEstado: Date;

  constructor(id?: number,
              localidad?: string,
              linea?: string,
              idLinea?: number,
              apellidoNombreSolicitante?: string,
              cuilSolicitante?: string,
              fechaEnvio?: Date,
              origen?: string,
              estado?: string,
              solicitante?: Persona,
              garantes?: Persona[],
              nroAgrupamiento?: number,
              tipoIntegranteSocio?: number,
              nombreIntegranteSocio?: string,
              seleccionado?: boolean,
              esSeleccionable?: boolean,
              puedeGenerarPrestamo?: boolean,
              fechaInicio?: Date,
              nroFormulario?: string,
              fechaEstado?: Date) {
    this.id = id;
    this.localidad = localidad;
    this.linea = linea;
    this.idLinea = idLinea;
    this.apellidoNombreSolicitante = apellidoNombreSolicitante;
    this.cuilSolicitante = cuilSolicitante;
    this.fechaEnvio = fechaEnvio;
    this.origen = origen;
    this.estado = estado;
    this.solicitante = solicitante;
    this.garantes = garantes;
    this.nroAgrupamiento = nroAgrupamiento;
    this.tipoIntegranteSocio = tipoIntegranteSocio;
    this.nombreIntegranteSocio = nombreIntegranteSocio;
    this.seleccionado = seleccionado;
    this.esSeleccionable = esSeleccionable;
    this.puedeGenerarPrestamo = puedeGenerarPrestamo;
    this.fechaInicio = fechaInicio;
    this.nroFormulario = nroFormulario;
    this.fechaEstado = fechaEstado;
  }
}
