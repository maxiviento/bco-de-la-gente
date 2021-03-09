export class BandejaArmarLoteSuafConsulta {
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idLocalidad: number;
  public idDepartamento: number;
  public nroPrestamoChecklist: string;
  public idOrigen: number;
  public nombre: string;
  public apellido: string;
  public devengado: number;
  public nroFormulario: number;
  public nroDocumento: string;
  public idLoteSuaf: number;
  public idLinea: number;
  public tipoPersona: number;
  public cuil: string;
  public numeroPagina: number;
  public tamañoPagina: number;
  public esCargaDevengado: boolean;
  public departamentoIds: string[];
  public localidadIds: string[];

  constructor(fechaDesde?: Date,
              fechaHasta?: Date,
              idDepartamento?: number,
              idLocalidad?: number,
              nroPrestamoChecklist?: string,
              idOrigen?: number,
              nombre?: string,
              apellido?: string,
              devengado?: number,
              nroFormulario?: number,
              nroDocumento?: string,
              idLoteSuaf?: number,
              idLinea?: number,
              tipoPersona?: number,
              cuil?: string,
              departamentoIds?: string[],
              localidadIds?: string[],
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.idLocalidad = idLocalidad;
    this.idDepartamento = idDepartamento;
    this.nroPrestamoChecklist = nroPrestamoChecklist;
    this.idOrigen = idOrigen;
    this.nombre = nombre;
    this.apellido = apellido;
    this.devengado = devengado;
    this.nroFormulario = nroFormulario;
    this.nroDocumento = nroDocumento;
    this.idLoteSuaf = idLoteSuaf;
    this.idLinea = idLinea;
    this.departamentoIds = departamentoIds;
    this.localidadIds = localidadIds;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
