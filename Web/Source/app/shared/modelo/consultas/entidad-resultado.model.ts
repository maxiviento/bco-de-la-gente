export class EntidadResultado {
  public entidad: number;
  public fechaRecupero: Date;
  public convenio: number;

  constructor(entidad?: number,
              fechaRecupero?: Date,
              convenio?: number) {
    this.entidad = entidad;
    this.fechaRecupero = fechaRecupero;
    this.convenio = convenio;
  }
}
