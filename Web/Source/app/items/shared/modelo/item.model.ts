import { TipoItem } from './tipo-item.model';
import { JsonProperty } from "../../../shared/map-utils";

export class Item {
  public id: number;
  public nombre: string;
  public descripcion: string;
  public idUsuarioAlta: number;
  public cuilUsuarioAlta: string;
  public idMotivoBaja: number;
  public nombreMotivoBaja: string;
  public fechaUltimaModificacion: Date;
  public idUsuarioUltimaModificacion: number;
  public cuilUsuarioUltimaModificacion: string;
  @JsonProperty({clazz: TipoItem})
  public tiposItem: TipoItem [];
  public esSeleccionado: boolean;
  public idItemPadre: number;
  public nombreItemPadre: string;
  public idRecurso: number;
  public urlRecurso: string;
  public idArea: number;
  public idEtapa: number;
  public subeArchivo: boolean;
  public generaArchivo: boolean;
  public idTipoDocumentacionCdd: number;
  public descripcionTipoDocumentacion: string;

  public requisitoChecklist: string;
  public requisitoSolicitante: string;
  public requisitoGarante: string;
  public tieneCDD: string;

  constructor(id?: number,
              nombre?: string,
              descripcion?: string,
              tiposItem?: TipoItem [],
              idUsuarioAlta?: number,
              cuilUsuarioAlta?: string,
              idMotivoBaja?: number,
              nombreMotivoBaja?: string,
              fechaUltimaModificacion?: Date,
              idUsuarioUltimaModificacion?: number,
              cuilUsuarioUltimaModificacion?: string,
              subeArchivo?: boolean,
              generaArchivo?: boolean,
              idTipoDocumentacionCdd?: number,
              descripcionTipoDocumentacion?: string,
              esSeleccionado?: boolean,
              requisitoChecklist?: string,
              requisitoSolicitante?: string,
              requisitoGarante?: string,
              tieneCDD?: string) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.tiposItem = tiposItem || [];
    this.idUsuarioAlta = idUsuarioAlta;
    this.cuilUsuarioAlta = cuilUsuarioAlta;
    this.idMotivoBaja = idMotivoBaja;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.idUsuarioUltimaModificacion = idUsuarioUltimaModificacion;
    this.cuilUsuarioUltimaModificacion = cuilUsuarioUltimaModificacion;
    this.esSeleccionado = esSeleccionado;
    this.idItemPadre = undefined;
    this.nombreItemPadre = undefined;
    this.idRecurso = undefined;
    this.urlRecurso = undefined;
    this.idArea = undefined;
    this.idEtapa = undefined;
    this.subeArchivo = subeArchivo;
    this.generaArchivo = generaArchivo;
    this.idTipoDocumentacionCdd = idTipoDocumentacionCdd;
    this.descripcionTipoDocumentacion = descripcionTipoDocumentacion;
    this.requisitoChecklist = requisitoChecklist;
    this.requisitoSolicitante = requisitoSolicitante;
    this.requisitoGarante = requisitoGarante;
    this.tieneCDD = tieneCDD;
  }

  public estaDadaDeBaja(): boolean {
    return this.idMotivoBaja != null;
  }
}
