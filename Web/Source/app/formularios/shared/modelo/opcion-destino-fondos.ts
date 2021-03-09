import { DestinoFondos } from '../../../shared/modelo/destino-fondos.model';

export class OpcionDestinoFondos {
  public observaciones: string;
  public destinosFondo: DestinoFondos[];

  constructor(observaciones?: string,
              destinosFondos?: DestinoFondos[]) {
    this.observaciones = observaciones;
    this.destinosFondo = destinosFondos;
  }
}
