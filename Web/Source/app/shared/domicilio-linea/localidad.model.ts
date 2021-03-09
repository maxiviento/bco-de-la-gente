export class Localidad {
  public idLocalidad: number;
  public localidad: string;
  public idDepartamento: number;
  constructor(idLocalidad?: number, localidad?: string, idDepartamento?: number) {
    this.idLocalidad = idLocalidad;
    this.localidad = localidad;
    this.idDepartamento = idDepartamento;
  }
}
