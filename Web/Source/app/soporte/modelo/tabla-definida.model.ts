import { ParametroTablaDefinida } from './parametro-tabla-definida';

export class TablaDefinida {
  public id: number;
  public nombre: string;
  public descripcion: string;
  public fechaDesde: Date;
  public parametros: ParametroTablaDefinida[];

  constructor(id?: number, nombre?: string, descripcion?: string,  fechaDesde?: Date, parametros?: ParametroTablaDefinida[]) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.fechaDesde = fechaDesde;
    this.parametros = parametros ? parametros : [];
  }
}
