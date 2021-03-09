export class Logo {
  public id:number;
  public nombre: string;
  public extension: string;
  public archivo: any;


  constructor(nombre?: string,
              archivo?: any,
              extencion?:string,
              id?:number) {
    this.id=id;
    this.nombre = nombre;
    this.extension=extencion;
    this.archivo = archivo;
  }
}
