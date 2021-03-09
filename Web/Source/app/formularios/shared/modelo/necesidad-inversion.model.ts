import { InversionRealizada } from './inversion-realizada.model';

export class NecesidadInversion {
  public id: number;
  public montoMicroprestamo: number;
  public montoCapitalPropio: number;
  public montoOtrasFuentes: number;
  public idFuenteFinanciamiento: number;
  public inversionesRealizadas: InversionRealizada[];


  constructor(id?: number,
              montoMicroprestamo?: number,
              montoCapitalPropio?: number,
              montoOtrasFuentes?: number,
              idFuenteFinanciamiento?: number,
              inversionesRealizadas?: InversionRealizada[]) {

    this.id = id;
    this.montoMicroprestamo = montoMicroprestamo;
    this.montoCapitalPropio = montoCapitalPropio;
    this.montoOtrasFuentes = montoOtrasFuentes;
    this.idFuenteFinanciamiento = idFuenteFinanciamiento;
    this.inversionesRealizadas = inversionesRealizadas;
  }
}
