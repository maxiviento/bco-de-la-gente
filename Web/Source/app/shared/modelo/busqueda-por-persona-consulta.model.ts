export class BusquedaPorPersonaConsulta {
  public tipoPersona: number;
  public cuil: string;
  public apellido: string;
  public nombre: string;
  public dni: string;

  constructor(tipoPersona?: number,
              cuil?: string,
              apellido?: string,
              nombre?: string,
              dni?: string) {
    this.tipoPersona = tipoPersona;
    this.cuil = cuil;
    this.apellido = apellido;
    this.nombre = nombre;
    this.dni = dni;
  }
}
