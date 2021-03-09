export class LineaConsulta {
  public nombre: string;
  public conOng: boolean;
  public conPrograma: boolean;
  public conDepartamento: boolean;
  public idDestinatario: number;
  public idMotivoDestino: number;
  public detallesDadosBaja: boolean;
  public dadosBaja: boolean;
  public idMotivoBaja: number;
  public modalDetalle: boolean;
  public idConvenioPago: number;
  public idConvenioRecupero: number;


  constructor(nombre?: string,
              conOng?: boolean,
              conPrograma?: boolean,
              conDepartamento?: boolean,
              idDestinatario?: number,
              idMotivoDestinatario?: number,
              detallesDadosBaja?: boolean,
              dadosBaja?: boolean,
              idMotivoBaja?: number,
              modalDetalle?: boolean,
              idConvenioRecupero?: number,
              idConvenioPago?: number) {
    this.nombre = nombre;
    this.conOng = conOng;
    this.conDepartamento = conDepartamento;
    this.idDestinatario = idDestinatario;
    this.idMotivoDestino = idMotivoDestinatario;
    this.detallesDadosBaja = detallesDadosBaja;
    this.dadosBaja = dadosBaja;
    this.idMotivoBaja = idMotivoBaja;
    this.modalDetalle = modalDetalle;
    this.idConvenioPago = idConvenioPago;
    this.idConvenioRecupero = idConvenioRecupero;
  }
}
