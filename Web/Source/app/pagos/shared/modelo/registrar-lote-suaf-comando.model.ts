import { BandejaArmarLoteSuafConsulta } from './bandeja-armar-lote-suaf-consulta.model';

export class RegistrarLoteSuafComando {
  public idPrestamosSeleccionados: number[];
  public nombreLote: string;
  public consulta: BandejaArmarLoteSuafConsulta;

  constructor(idPrestamosSeleccionados?: number[],
              nombreLote?: string,
              consulta?: BandejaArmarLoteSuafConsulta) {
    this.idPrestamosSeleccionados = idPrestamosSeleccionados;
    this.nombreLote = nombreLote;
    this.consulta = consulta;

  }
}
