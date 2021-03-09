export class BandejaAdendaConsulta {
  public idLote: number;
  public nroDetalle: number;
  public nroPrestamoChecklist: string;
  public nroFormulario: string;
  public idsLineas: number[];
  public idOrigen: number;
  public tipoPersona: number;
  public cuil: string;
  public apellido: string;
  public nombre: string;
  public dni: string;
  public departamentoIds: string[];
  public localidadIds: string[];
  public seleccionarTodos: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(idLote?: number,
              nroDetalle?: number,
              nroPrestamoChecklist?: string,
              nroFormulario?: string,
              idsLineas?: number[],
              idOrigen?: number,
              tipoPersona?: number,
              cuil?: string,
              apellido?: string,
              nombre?: string,
              dni?: string,
              departamentoIds?: string[],
              localidadIds?: string[],
              seleccionarTodos?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.idLote = idLote;
    this.nroDetalle = nroDetalle;
    this.nroPrestamoChecklist = nroPrestamoChecklist;
    this.nroFormulario = nroFormulario;
    this.idsLineas = idsLineas;
    this.idOrigen = idOrigen;
    this.tipoPersona = tipoPersona;
    this.cuil = cuil;
    this.apellido = apellido;
    this.nombre = nombre;
    this.dni = dni;
    this.departamentoIds = departamentoIds;
    this.localidadIds = localidadIds;
    this.seleccionarTodos = seleccionarTodos;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
