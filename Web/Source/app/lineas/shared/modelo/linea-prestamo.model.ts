import { JsonProperty } from '../../../shared/map-utils';
import { SexoDestinatario } from './destinatario-prestamo.model';
import { DetalleLineaPrestamo } from './detalle-linea-prestamo.model';
import { Requisito } from './requisito-linea';
import { MotivoDestino } from '../../../motivo-destino/shared/modelo/motivo-destino.model';
import { ProgramaCombo } from './programa-combo.model';
import { SeleccionMultiple } from '../../../shared/domicilio/seleccion-multiple.model';
import { OngLinea } from './ong-linea.model';

export class LineaPrestamo {
  public id: number;
  public nombre: string;
  public descripcion: string;
  public objetivo: string;
  public color: string;
  public conOng: boolean;
  public conCurso: boolean;
  public conPrograma: boolean;
  public deptoLocalidad: boolean;
  public conDepartamento: boolean;
  public trabajaConLocalidad: boolean;
  public destinatarioId: number;
  public motivoDestinoId: number;
  @JsonProperty({clazz: MotivoDestino})
  public motivoDestino: MotivoDestino;
  @JsonProperty({clazz: SexoDestinatario})
  public sexoDestinatario: SexoDestinatario;
  @JsonProperty({clazz: DetalleLineaPrestamo})
  public detalleLineaPrestamo: DetalleLineaPrestamo[];
  @JsonProperty({clazz: Requisito})
  public requisitos: Requisito [];
  public dadosBaja: boolean;
  public detallesDadosBaja: boolean;
  public fechaBaja: Date;
  public idMotivoDestino: number;
  public nombreMotivoDestino: string;
  public idSexoDestinatario: number;
  public nombreSexoDestinatario: string;
  public numeroPagina: number;
  public tamañoPagina: number;
  public idMotivoBaja: number;
  public nombreMotivoBaja: string;
  public nombreUsuario: string;
  public fechaUltimaModificacion: Date;
  public logo: any;
  public piePagina: any;
  public logoCargado: any;
  public piePaginaCargado: any;
  @JsonProperty({clazz: ProgramaCombo})
  public programa: ProgramaCombo;
  public idPrograma: number;
  public nombrePrograma: string;
  public localidadIds: string;
  public localidades: SeleccionMultiple [];
  public lsOng: OngLinea[];

  constructor(id?: number,
              nombre?: string,
              descripcion?: string,
              objetivo?: string,
              color?: string,
              conOng?: boolean,
              conCurso?: boolean,
              conPrograma?: boolean,
              conDepartamento?: boolean,
              trabajaConLocalidad?: boolean,
              destinatarioId?: number,
              motivoDestinoId?: number,
              motivoDestino?: MotivoDestino,
              sexoDestinatario?: SexoDestinatario,
              detalleLineaPrestamo?: DetalleLineaPrestamo[],
              requisitos?: Requisito[],
              dadosBaja?: boolean,
              fechaBaja?: Date,
              nombreMotivoDestino?: string,
              numeroPagina?: number,
              tamañoPagina?: number,
              idMotivoBaja?: number,
              detallesDadosBaja?: boolean,
              nombreSexoDestinatario?: string,
              nombreMotivoBaja?: string,
              nombreUsuario?: string,
              fechaUltimaModificacion?: Date,
              idSexoDestinatario?: number,
              idMotivoDestino?: number,
              logo?: any,
              piePagina?: any,
              programa?: ProgramaCombo,
              idPrograma?: number,
              nombrePrograma?: string,
              deptoLocalidad?: boolean,
              localidadIds?: string,
              localidades?: SeleccionMultiple [],
              lsOng?: OngLinea[]) {

    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.objetivo = objetivo;
    this.color = color;
    this.conOng = conOng;
    this.conCurso = conCurso;
    this.conPrograma = conPrograma;
    this.conDepartamento = conDepartamento;
    this.trabajaConLocalidad = trabajaConLocalidad;
    this.destinatarioId = destinatarioId;
    this.motivoDestinoId = motivoDestinoId;
    this.motivoDestino = motivoDestino;
    this.sexoDestinatario = sexoDestinatario;
    this.detalleLineaPrestamo = detalleLineaPrestamo;
    this.requisitos = requisitos;
    this.dadosBaja = dadosBaja;
    this.fechaBaja = fechaBaja;
    this.nombreMotivoDestino = nombreMotivoDestino;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.idMotivoBaja = idMotivoBaja;
    this.detallesDadosBaja = detallesDadosBaja;
    this.nombreSexoDestinatario = nombreSexoDestinatario;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.nombreUsuario = nombreUsuario;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.logo = logo;
    this.piePagina = piePagina;
    this.idMotivoDestino = idMotivoDestino;
    this.idSexoDestinatario = idSexoDestinatario;
    this.programa = programa;
    this.idPrograma = idPrograma;
    this.nombrePrograma = nombrePrograma;
    this.deptoLocalidad = deptoLocalidad;
    this.localidadIds = localidadIds;
    this.localidades = localidades;
    this.lsOng = lsOng;
  }
}
