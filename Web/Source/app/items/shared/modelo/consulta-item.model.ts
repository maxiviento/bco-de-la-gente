export class ConsultaItem {
  public nombre: string;
  public idItem: number;
  public incluirDadosBaja: boolean;
  public recurso: number;
  public esSubitem: boolean;
  public incluirHijos: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;


  constructor(nombre?: string,
              idItem?: number,
              incluirDadosDeBaja?: boolean,
              recurso?: number,
              esSubitem?: boolean,
              incluirHijos?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.nombre = nombre;
    this.idItem = idItem;
    this.incluirDadosBaja = incluirDadosDeBaja || false;
    this.recurso = recurso;
    this.esSubitem = esSubitem;
    this.incluirHijos = incluirHijos;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
