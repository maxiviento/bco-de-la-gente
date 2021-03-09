export class ImportacionSuafResultado {

  public cantidadDevengados: number;
  public cantidadNoProcesados: number;


  constructor(cantidadDevengados?: number, cantidadNoProcesados?: number) {
    this.cantidadDevengados = cantidadDevengados;
    this.cantidadNoProcesados = cantidadNoProcesados;
  }
}
