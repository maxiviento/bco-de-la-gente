import { JsonProperty } from '../../../shared/map-utils';
import { IntegranteSocio } from './integrante-socio.model';
import { TipoFinanciamiento } from './tipo-financiamiento.model';
import { TipoGarantia } from './tipo-garantia.model';
import { TipoInteres } from './tipo-interes.model';
import { Convenio } from '../../../shared/modelo/convenio-model';

export class DetalleLineaPrestamo {
  public id: number;
  @JsonProperty({clazz: IntegranteSocio})
  public integranteSocio: IntegranteSocio;
  public montoPrestable: number;
  public montoTope: number;
  public cantidadMinimaIntegrantes: number;
  public cantidadMaximaIntegrantes: number;
  @JsonProperty({clazz: TipoFinanciamiento})
  public tipoFinanciamiento: TipoFinanciamiento;
  @JsonProperty({clazz: TipoInteres})
  public tipoInteres: TipoInteres;
  public plazoDevolucion: number;
  public valorCuotaSolidaria: string;
  @JsonProperty({clazz: TipoGarantia})
  public tipoGarantia: TipoGarantia;
  public idSocioIntegrante: number;
  public nombreSocioIntegrante: string;
  public numeroPagina: number;
  public tamañoPagina: number;
  public lineaId: number;
  public idTipoFinanciamiento: number;
  public nombreTipoFinanciamiento: string;
  public idTipoGarantia: number;
  public nombreTipoGarantia: string;
  public idTipoInteres: number;
  public nombreTipoInteres: string;
  public fechaBaja: Date;
  public fechaUltimaModificacion: Date;
  public idMotivoBaja: number;
  public dadosBaja: boolean;
  public nombreLinea: string;
  public nombreUsuario: string;
  public nombreMotivoBaja: string;
  public visualizacion: string;
  public apoderado: boolean;
  @JsonProperty({clazz: Convenio})
  public convenioPago: Convenio;
  @JsonProperty({clazz: Convenio})
  public convenioRecupero: Convenio;
  public nombreConvPag: string;
  public nombreConvRec: string;
  public codConvenioPag: number;
  public codConvenioRec: number;

  constructor(id?: number,
              integranteSocio?: IntegranteSocio,
              montoPrestable?: number,
              montoTope?: number,
              cantidadMinimaIntegrantes?: number,
              cantidadMaximaIntegrantes?: number,
              tipoFinanciamiento?: TipoFinanciamiento,
              tipoInteres?: TipoInteres,
              plazoDevolucion?: number,
              valorCuotaSolidaria?: string,
              tipoGarantia?: TipoGarantia,
              nombreSocioIntegrante?: string,
              numeroPagina?: number,
              tamañoPagina?: number,
              lineaId?: number,
              nombreTipoFinanciamiento?: string,
              nombreTipoGarantia?: string,
              nombreTipoInteres?: string,
              fechaBaja?: Date,
              fechaUltimaModificacion?: Date,
              idMotivoBaja?: number,
              dadosBaja?: boolean,
              nombreLinea?: string,
              nombreUsuario?: string,
              nombreMotivoBaja?: string,
              idTipoInteres?: number,
              idTipoGarantia?: number,
              idTipoFinanciamiento?: number,
              idSocioIntegrante?: number,
              visualizacion?: string,
              apoderado?: boolean,
              convenioPago?: Convenio,
              convenioRecupero?: Convenio,
              nombreConvPag?: string,
              nombreConvRec?: string,
              codCovenioRec?: number,
              codCovenioPag?: number) {

    this.id = id;
    this.integranteSocio = integranteSocio;
    this.montoPrestable = montoPrestable;
    this.montoTope = montoTope;
    this.cantidadMinimaIntegrantes = cantidadMinimaIntegrantes;
    this.cantidadMaximaIntegrantes = cantidadMaximaIntegrantes;
    this.tipoFinanciamiento = tipoFinanciamiento;
    this.tipoInteres = tipoInteres;
    this.plazoDevolucion = plazoDevolucion;
    this.valorCuotaSolidaria = valorCuotaSolidaria;
    this.tipoGarantia = tipoGarantia;
    this.nombreSocioIntegrante = nombreSocioIntegrante;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.lineaId = lineaId;
    this.nombreTipoFinanciamiento = nombreTipoFinanciamiento;
    this.nombreTipoGarantia = nombreTipoGarantia;
    this.nombreTipoInteres = nombreTipoInteres;
    this.fechaBaja = fechaBaja;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.idMotivoBaja = idMotivoBaja;
    this.dadosBaja = dadosBaja;
    this.nombreLinea = nombreLinea;
    this.nombreUsuario = nombreUsuario;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.idSocioIntegrante = idSocioIntegrante;
    this.idTipoFinanciamiento = idTipoFinanciamiento;
    this.idTipoGarantia = idTipoGarantia;
    this.idTipoInteres = idTipoInteres;
    this.visualizacion = visualizacion;
    this.apoderado = apoderado;
    this.convenioPago = convenioPago;
    this.convenioRecupero = convenioRecupero;
    this.nombreConvPag = nombreConvPag;
    this.nombreConvRec = nombreConvRec;
    this.codConvenioPag = codCovenioPag;
    this.codConvenioRec = codCovenioRec;
  }
}
