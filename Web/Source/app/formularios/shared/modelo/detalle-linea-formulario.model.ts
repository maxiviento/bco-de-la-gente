export class DetalleLineaFormulario {
  public id: number;
  public lineaId: number;
  public tipoIntegranteId: number;
  public montoPrestable: number;
  public montoTopeIntegrante: number;
  public cantidadMaximaIntegrantes: number;
  public cantidadMinimaIntegrantes: number;
  public plazoDevolucionMaximo: number;
  public valorCuotaSocioIndep: number;
  public valorCuotaSocioAsoc: number;
  public tipoGarantiaId: number;
  public visualizacion: string;
  public conOng: boolean;
  public conCurso: boolean;
  public conPrograma: boolean;
  public nombre: string;
  public descripcion: string;
  public sexoDestinatarioId: string;
  public color: string;
  public logo: any;
  public apoderado: boolean;
  public convenioPagoId: number;
  public convenioRecuperoId: number;

  constructor(id?: number,
              lineaId?: number,
              tipoIntegranteId?: number,
              montoPrestable?: number,
              cantidadMaximaIntegrantes?: number,
              cantidadMinimaIntegrantes?: number,
              plazoDevolucionMaximo?: number,
              valorCuotaSocioIndep?: number,
              valorCuotaSocioAsoc?: number,
              tipoGarantiaId?: number,
              visualizacion?: string,
              conOng?: boolean,
              conCurso?: boolean,
              conPrograma?: boolean,
              nombre?: string,
              descripcion?: string,
              sexoDestinatarioId?: string,
              color?: string,
              logo?: string,
              apoderado?: boolean,
              convenioPago?: number,
              convenioRecupero?: number,
              montoTopeIntegrante?: number) {
    this.id = id;
    this.lineaId = lineaId;
    this.tipoIntegranteId = tipoIntegranteId;
    this.montoPrestable = montoPrestable;
    this.cantidadMaximaIntegrantes = cantidadMaximaIntegrantes;
    this.cantidadMinimaIntegrantes = cantidadMinimaIntegrantes;
    this.plazoDevolucionMaximo = plazoDevolucionMaximo;
    this.valorCuotaSocioIndep = valorCuotaSocioIndep;
    this.valorCuotaSocioAsoc = valorCuotaSocioAsoc;
    this.tipoGarantiaId = tipoGarantiaId;
    this.visualizacion = visualizacion;
    this.conOng = conOng;
    this.conCurso = conCurso;
    this.conPrograma = conPrograma;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.sexoDestinatarioId = sexoDestinatarioId;
    this.color = color;
    this.logo = logo;
    this.apoderado = apoderado;
    this.convenioPagoId = convenioPago;
    this.convenioRecuperoId = convenioRecupero;
    this.montoTopeIntegrante = montoTopeIntegrante;
  }
}
