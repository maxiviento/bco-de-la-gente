export class ParametroCapacidadAsociativa {
  public cantMinIntegrantes: number;
  public cantMaxIntegrantes: number;

  constructor(cantMinIntegrantes?: number,
              cantMaxIntegrantes?: number) {

    this.cantMaxIntegrantes = cantMaxIntegrantes;
    this.cantMinIntegrantes = cantMinIntegrantes;
  }
}
