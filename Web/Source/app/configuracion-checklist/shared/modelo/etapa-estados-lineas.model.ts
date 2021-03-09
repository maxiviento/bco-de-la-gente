import { EstadoPrestamo } from '../../../prestamos-checklists/shared/modelos/estado-prestamo.model';
import { Etapa } from '../../../etapas/shared/modelo/etapa.model';
import { LineaPrestamo } from '../../../lineas/shared/modelo/linea-prestamo.model';

export class EtapaEstadosLineas{
  public estadoAnterior: EstadoPrestamo;
  public estadoSiguiente: EstadoPrestamo;
  public etapaAnterior: Etapa;
  public etapaSiguiente: Etapa;
  public linea: LineaPrestamo;
  public orden: number;

  constructor(estadoAnterior?: EstadoPrestamo,
              estadoSiguiente?: EstadoPrestamo,
              etapaAnterior?: Etapa,
              etapaSiguiente?: Etapa,
              linea?: LineaPrestamo,
              orden?: number) {
    this.estadoAnterior = estadoAnterior;
    this.estadoSiguiente = estadoSiguiente;
    this.etapaAnterior = etapaAnterior;
    this.etapaSiguiente = etapaSiguiente;
    this.linea = linea;
    this.orden = orden;
  }
}
