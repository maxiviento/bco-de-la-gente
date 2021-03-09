export class LineaArchivosResultado {

  public logoUrl: string;
  public logoNombre: string;
  public piePaginaUrl: string;
  public piePaginaNombre: string;


  constructor(logoUrl?: string, logoNombre?: string, piePaginaUrl?: string, piePaginaNombre?: string) {

    this.logoUrl = logoUrl;
    this.logoNombre = logoNombre;
    this.piePaginaUrl = piePaginaUrl;
    this.piePaginaNombre = piePaginaNombre;
  }
}
