export class OngFormulario {
  public idOng: number;
  public idFormulario: number;
  public nombreGrupo: string;
  public numeroGrupo: number;

  constructor(idOng?: number,
              idFormulario?: number,
              nombreGrupo?: string,
              numeroGrupo?: number) {
    this.idOng = idOng;
    this.idFormulario = idFormulario;
    this.nombreGrupo = nombreGrupo;
    this.numeroGrupo = numeroGrupo;
  }
}
