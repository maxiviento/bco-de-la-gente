import { RequisitoPrestamo } from '../../../prestamos-checklists/shared/modelos/requisito-prestamo';
export class Etapa {
  public id?: number;
  public nombre?: string;
  public colapsar?: boolean;
  public descripcion?: string;
  public idUsuarioAlta?: number;
  public cuilUsuarioAlta?: string;
  public idMotivoBaja?: number;
  public nombreMotivoBaja?: string;
  public fechaUltimaModificacion?: Date;
  public idUsuarioUltimaModificacion?: number;
  public cuilUsuarioUltimaModificacion?: string;
  public requisitosEtapa: RequisitoPrestamo [];
  public gestionaArchivos: boolean;

  constructor(id?: number,
              nombre?: string,
              descripcion?: string,
              idUsuarioAlta?: number,
              cuilUsuarioAlta?: string,
              idMotivoBaja?: number,
              nombreMotivoBaja?: string,
              fechaUltimaModificacion?: Date,
              idUsuarioUltimaModificacion?: number,
              cuilUsuarioUltimaModificacion?: string,
              gestionaArchivos?: boolean,
              requisitosEtapa?: RequisitoPrestamo [],
              colapsar?: boolean) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.idUsuarioAlta = idUsuarioAlta;
    this.cuilUsuarioAlta = cuilUsuarioAlta;
    this.idMotivoBaja = idMotivoBaja;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.idUsuarioUltimaModificacion = idUsuarioUltimaModificacion;
    this.cuilUsuarioUltimaModificacion = cuilUsuarioUltimaModificacion;
    this.requisitosEtapa = requisitosEtapa;
    this.gestionaArchivos = gestionaArchivos;
    this.colapsar = colapsar;
  }

  public estaDadaDeBaja(): boolean {
    return this.idMotivoBaja != null;
  }
}
