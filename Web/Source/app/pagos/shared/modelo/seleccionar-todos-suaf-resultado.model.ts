export class SeleccionarTodosSuafResultado {
  public cantPrestamos: number;
  public cantFormularios: number;

  constructor(cantPrestamos?: number,
              cantFormularios?: number) {
    this.cantPrestamos = cantPrestamos;
    this.cantFormularios = cantFormularios;
  }
}
