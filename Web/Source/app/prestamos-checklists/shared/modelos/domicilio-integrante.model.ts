export class DomicilioIntegrante {
  public calle: string;
  public altura: string;
  public piso: string;
  public depto: string;
  public torre: string;
  public manzana: string;
  public lote: string;
  public kilometro: string;
  public barrio: string;
  public localidad: string;
  public departamento: string;
  public provincia: string;

  constructor(calle?: string,
              altura?: string,
              piso?: string,
              depto?: string,
              torre?: string,
              manzana?: string,
              lote?: string,
              kilometro?: string,
              barrio?: string,
              localidad?: string,
              departamento?: string,
              provincia?: string) {
    this.calle = calle;
    this.altura = altura;
    this.piso = piso;
    this.depto = depto;
    this.torre = torre;
    this.lote = lote;
    this.kilometro = kilometro;
    this.manzana = manzana;
    this.barrio = barrio;
    this.localidad = localidad;
    this.departamento = departamento;
    this.provincia = provincia;
  }
}
