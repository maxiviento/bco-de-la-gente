import { JsonProperty } from '../../../shared/map-utils';
export class FormularioSeleccionado {
  public idFormulario: number;
  public nroFormulario: number;
  public nroPrestamo: number;
  @JsonProperty('apellidoNombreSolicitante')
  public apellidoNombre: string;
  @JsonProperty('localidad')
  public nombreLocalidad: string;
  @JsonProperty('linea')
  @JsonProperty('departamento')
  public nombreLinea: string;
  public nombreDepartamento: string;
  @JsonProperty('cuilSolicitante')
  public cuilDni: string;
  @JsonProperty('banco')
  public nombreBanco: string;
  @JsonProperty('sucursal')
  public nombreSucursal: string;
  public idBanco: string;
  public idSucursal: string;
  public fechFinPago: Date;
  public monPres: number;
  public canCuot: number;
  public puedeCrearPlan: boolean;
  public tipoApoderado: number;

  constructor() {
    this.idFormulario = undefined;
    this.nroFormulario = undefined;
    this.nroPrestamo = undefined;
    this.apellidoNombre = undefined;
    this.nombreLinea = undefined;
    this.nombreLocalidad = undefined;
    this.nombreDepartamento = undefined;
    this.cuilDni = undefined;
    this.nombreBanco = undefined;
    this.nombreSucursal = undefined;
    this.idBanco = undefined;
    this.idSucursal = undefined;
    this.fechFinPago = undefined;
    this.monPres = undefined;
    this.canCuot = undefined;
    this.puedeCrearPlan = undefined;
    this.tipoApoderado = undefined;
  }
}
