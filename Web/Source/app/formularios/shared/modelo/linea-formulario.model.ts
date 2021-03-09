import { DetalleLineaFormulario } from './detalle-linea-formulario.model';

export class LineaFormulario {
  public color: string;
  public nombre: string;
  public descripcion: string;
  public objetivo: string;
  public detalles: DetalleLineaFormulario[];

  constructor(color?: string,
              nombre?: string,
              descripcion?: string,
              objetivo?: string,
              detalles?: DetalleLineaFormulario[]) {
    this.color = color;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.objetivo = objetivo;
    this.detalles = detalles;
  }
}
