export class ConsultaMonitorProcesos {
  public idsEstado: string;
  public idsTipo: string;
  public fechaAlta: Date;
  public numeroPagina: number;
  public tamañoPagina: number;
  public idUsuario: number;

  constructor(
    idsEstado?: string,
    idsTipo?: string,
    fechaAlta?: Date,
    numeroPagina?: number,
    tamañoPagina?: number,
    idUsuario?: number,
  ) {
    this.idsEstado = idsEstado;
    this.idsTipo = idsTipo;
    this.fechaAlta = fechaAlta;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.idUsuario = idUsuario;
  }
}
