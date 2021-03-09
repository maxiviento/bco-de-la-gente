export class FormularioidDocumento {
  public idFormulario: number;
  public nroDocumento: string;

  public constructor(idFormulario?: number,
                     nroDocumento?: string) {
    this.idFormulario = idFormulario;
    this.nroDocumento = nroDocumento;
  }
}
