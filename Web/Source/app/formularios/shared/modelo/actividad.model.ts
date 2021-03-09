export class Actividad {
  public id: number;
  public nombre: string;
  public fechaInicio: Date;
  public idRubro: number;

  constructor(Id?: number,
              Nombre?: string,
              fechaInicio?: Date,
              IdRubro?: number) {
    this.id = Id;
    this.nombre = Nombre;
    this.fechaInicio = fechaInicio;
    this.idRubro = IdRubro;
  }
}
