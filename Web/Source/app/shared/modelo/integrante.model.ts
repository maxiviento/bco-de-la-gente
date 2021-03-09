export class Integrante {
  public sexo: string;
  public pais: string;
  public nroDocumento: string;
  public idNumero: number;
  public idFormulario: number;
  public nombre: string;
  public apellido: string;
  public telFijo: string;
  public telCelular: string;
  public mail: string;
  public idAgrupamiento: number;
  public idEstado: number;
  public solicitante: boolean;
  public titularSolicitante: boolean;
  public esApoderado: number;
  public estado: string;

  constructor(sexo?: string,
              pais?: string,
              nroDocumento?: string,
              idFormulario?: number,
              nombre?: string,
              apellido?: string,
              telFijo?: string,
              telCelular?: string,
              mail?: string,
              idAgrupamiento?: number,
              idEstado?: number,
              solicitante?: boolean,
              titularSolicitante?: boolean,
              esApoderado?: number,
              estado?: string,
              idNumero?: number) {
    this.nombre = nombre === undefined ? null : nombre;
    this.apellido = apellido === undefined ? null : apellido;
    this.sexo = sexo === undefined ? null : sexo;
    this.pais = pais === undefined ? null : pais;
    this.nroDocumento = nroDocumento === undefined ? null : nroDocumento;
    this.telFijo = telFijo === undefined ? null : telFijo;
    this.telCelular = telCelular === undefined ? null : telCelular;
    this.mail = mail === undefined ? null : mail;
    this.idFormulario = idFormulario;
    this.idAgrupamiento = idAgrupamiento;
    this.idEstado = idEstado;
    this.solicitante = solicitante;
    this.titularSolicitante = titularSolicitante;
    this.esApoderado = esApoderado;
    this.estado = estado;
    this.idNumero = idNumero;
  }
}
