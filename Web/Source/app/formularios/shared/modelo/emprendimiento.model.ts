export class Emprendimiento {
  public id: number;
  public idVinculo: number;
  public calle: string;
  public nroCalle: number;
  public torre: string;
  public nroPiso: number;
  public nroDpto: string;
  public manzana: string;
  public casa: string;
  public barrio: string;
  public idLocalidad: number;
  public localidad: string;
  public idDepartamento: number;
  public departamento: string;
  public codPostal: number;
  public nroCodArea: number;
  public nroTelefono: number;
  public email: string;
  public idTipoInmueble: string;
  public idActividad: number;
  public idRubro: number;
  public fechaInicioActividad: Date;
  public idTipoProyecto: number;
  public fechaActivo: Date;
  public idSectorDesarrollo: string;
  public tieneExperiencia: boolean;
  public tiempoExperiencia: string;
  public hizoCursos: boolean;
  public cursoInteres: string;
  public pidioCredito: boolean;
  public creditoFueOtorgado: boolean;
  public institucionSolicitante: string;
  public idTipoOrganizacion: number;


  constructor(id?: number,
              idVinculo?: number,
              calle?: string,
              nroCalle?: number,
              torre?: string,
              nroPiso?: number,
              nroDpto?: string,
              manzana?: string,
              casa?: string,
              barrio?: string,
              idLocalidad?: number,
              idDepartamento?: number,
              codPostal?: number,
              nroCodArea?: number,
              nroTelefono?: number,
              email?: string,
              idTipoInmueble?: string,
              idActividad?: number,
              idRubro?: number,
              fechaInicioActividad?: Date,
              idTipoProyecto?: number,
              fechaActivo?: Date,
              idSectorDesarrollo?: string,
              tieneExperiencia?: boolean,
              tiempoExperiencia?: string,
              hizoCursos?: boolean,
              cursoInteres?: string,
              pidioCredito?: boolean,
              creditoFueOtorgado?: boolean,
              institucionSolicitante?: string,
              idTipoOrganizacion?: number,
              localidad?: string,
              departamento?: string) {
    this.id = id;
    this.idVinculo = idVinculo;
    this.calle = calle;
    this.nroCalle = nroCalle;
    this.torre = torre;
    this.nroPiso = nroPiso;
    this.nroDpto = nroDpto;
    this.manzana = manzana;
    this.casa = casa;
    this.barrio = barrio;
    this.idLocalidad = idLocalidad;
    this.idDepartamento = idDepartamento;
    this.codPostal = codPostal;
    this.nroCodArea = nroCodArea;
    this.nroTelefono = nroTelefono;
    this.email = email;
    this.idTipoInmueble = idTipoInmueble;
    this.idActividad = idActividad;
    this.idRubro = idRubro;
    this.fechaInicioActividad = fechaInicioActividad;
    this.idTipoProyecto = idTipoProyecto;
    this.fechaActivo = fechaActivo;
    this.idSectorDesarrollo = idSectorDesarrollo;
    this.tieneExperiencia = tieneExperiencia;
    this.tiempoExperiencia = tiempoExperiencia;
    this.hizoCursos = hizoCursos;
    this.cursoInteres = cursoInteres;
    this.pidioCredito = pidioCredito;
    this.creditoFueOtorgado = creditoFueOtorgado;
    this.institucionSolicitante = institucionSolicitante;
    this.idTipoOrganizacion = idTipoOrganizacion;
    this.localidad = localidad;
    this.departamento = departamento;
  }
}
