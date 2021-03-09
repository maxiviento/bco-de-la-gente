export class CargaDatosChequeComando {
  public idFormulario: number;
  public nroCheque: string;
  public fechaVencimientoCheque: Date;

  constructor(idFormulario?: number,
              nroCheque?: string,
              fechaVencimientoCheque?: Date) {

    this.idFormulario = idFormulario;
    this.nroCheque = nroCheque;
    this.fechaVencimientoCheque = fechaVencimientoCheque;
  }
}
