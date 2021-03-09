export class VersionChecklist{
  public id: number;
  public version: string;
  public enUso: boolean;

  constructor(id?: number,
              version?: string,
              enUso?: boolean) {
    this.id = id;
    this.version = version;
    this.enUso = enUso;
  }
}
