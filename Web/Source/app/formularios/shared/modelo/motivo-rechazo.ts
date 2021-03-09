import { Ambito } from "../../../motivos-rechazo/shared/modelo/ambito.model";
import { MotivoBaja } from "../../../shared/modelo/motivoBaja.model";
import { Usuario } from "../../../core/auth/modelos/usuario.model";

export class MotivoRechazo {
  public id: number;
  public nombre: string;
  public descripcion: string;
  public abreviatura: string;

  public idUsuarioAlta: number;
  public motivoBaja: MotivoBaja;
  public nombreMotivoBaja: string;
  public fechaUltimaModificacion: Date;
  public idUsuarioUltimaModificacion: number;
  public cuilUsuarioUltimaModificacion: string;
  public ambito: Ambito;
  public esAutomatico: boolean;
  public esPredefinido: boolean;
  public usuarioUltimaModificacion: Usuario;
  public idMotivoBaja: number;
  public observaciones: string;
  public codigo: string;

  constructor() {
    this.id = undefined;
    this.nombre = undefined;
    this.descripcion = undefined;
    this.abreviatura = undefined;

    this.idUsuarioAlta = undefined;
    this.motivoBaja = undefined;
    this.nombreMotivoBaja = undefined;
    this.fechaUltimaModificacion = undefined;
    this.idUsuarioUltimaModificacion = undefined;
    this.cuilUsuarioUltimaModificacion = undefined;
    this.ambito = new Ambito();
    this.esAutomatico = undefined;
    this.usuarioUltimaModificacion = undefined;
    this.idMotivoBaja = undefined;
    this.esPredefinido = undefined;
    this.observaciones = undefined;
    this.codigo = '';
  }

  public estaDadaDeBaja(): boolean {
    return (this.motivoBaja != null || this.idMotivoBaja != null);
  }
}
