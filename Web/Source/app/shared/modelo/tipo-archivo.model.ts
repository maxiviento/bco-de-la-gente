export class TipoArchivo{
  public value: string;
  public type: string;
  public extension: string;


  constructor(value?: string,
              type?: string,
              extension?: string) {
    this.value = value;
    this.type = type;
    this.extension = extension;
  }
}
