export class Persona {
  public numeroId: number;
  public nombre: string;
  public apellido: string;
  public sexoId: string;
  public nombreSexo: string;
  public tipoDocumentoId: string;
  public tipoDocumento: string;
  public codigoPais: string;
  public nroDocumento: string;
  public fechaNacimiento: Date;
  public fechaDefuncion: Date;
  public domicilioIdVin: number;
  public domicilioGrupoFamiliar: string;
  public domicilioGrupoFamiliarLocalidad: string;
  public domicilioGrupoFamiliarDepartamento: string;
  public cuil: string;
  public codigoArea: string;
  public telefono: string;
  public codigoAreaCelular: string;
  public celular: string;
  public edad: string;
  public email: string;
  public nacionalidad: string;
  public idNumero: number;
  public domicilioReal: string;
  public domicilioRealLocalidad: string;
  public domicilioRealDepartamento: string;
  public domicilioRealIdVin: number;
  public barrio: string;
  public idGarante: number;

  constructor(numeroId?: number,
              nombre?: string,
              apellido?: string,
              idSexo?: string,
              nombreSexo?: string,
              tipoDocumentoId?: string,
              tipoDocumento?: string,
              codigoPais?: string,
              nroDocumento?: string,
              fechaNacimiento?: Date,
              fechaDefuncion?: Date,
              domicilioGrupoFamiliar?: string,
              domicilioGrupoFamiliarLocalidad?: string,
              domicilioGrupoFamiliarDepartamento?: string,
              cuil?: string,
              codigoArea?: string,
              telefono?: string,
              codigoAreaCelular?: string,
              celular?: string,
              edad?: string,
              email?: string,
              nacionalidad?: string,
              idNumero?: number,
              domicilioIdVin?: number,
              domicilioReal?: string,
              domicilioRealLocalidad?: string,
              domicilioRealDepartamento?: string,
              domicilioRealIdVin?: number,
              barrio?: string,
              idGarante?: number) {

    this.numeroId = numeroId === undefined ? null : numeroId;
    this.nombre = nombre === undefined ? null : nombre;
    this.apellido = apellido === undefined ? null : apellido;
    this.sexoId = idSexo === undefined ? null : idSexo;
    this.nombreSexo = nombreSexo === undefined ? null : nombreSexo;
    this.tipoDocumentoId = tipoDocumentoId === undefined ? null : tipoDocumentoId;
    this.tipoDocumento = tipoDocumento === undefined ? null : tipoDocumento;
    this.codigoPais = codigoPais === undefined ? null : codigoPais;
    this.nroDocumento = nroDocumento === undefined ? null : nroDocumento;
    this.fechaNacimiento = fechaNacimiento === undefined ? null : fechaNacimiento;
    this.fechaDefuncion = fechaDefuncion === undefined ? null : fechaDefuncion;
    this.domicilioGrupoFamiliar = domicilioGrupoFamiliar === undefined ? null : domicilioGrupoFamiliar;
    this.domicilioGrupoFamiliarLocalidad = domicilioGrupoFamiliarLocalidad === undefined ? null : domicilioGrupoFamiliarLocalidad;
    this.domicilioGrupoFamiliarDepartamento = domicilioGrupoFamiliarDepartamento === undefined ? null : domicilioGrupoFamiliarDepartamento;
    this.cuil = cuil === undefined ? null : cuil;
    this.codigoArea = codigoArea === undefined ? null : codigoArea;
    this.telefono = telefono === undefined ? null : telefono;
    this.codigoAreaCelular = codigoAreaCelular === undefined ? null : codigoAreaCelular;
    this.celular = celular === undefined ? null : celular;
    this.edad = edad === undefined ? null : edad;
    this.email = email === undefined ? null : email;
    this.nacionalidad = nacionalidad === undefined ? null : nacionalidad;
    this.idNumero = idNumero;
    this.domicilioIdVin = domicilioIdVin;
    this.domicilioRealIdVin = domicilioRealIdVin;
    this.domicilioReal = domicilioReal === undefined ? null : domicilioReal;
    this.domicilioRealLocalidad = domicilioRealLocalidad === undefined ? null : domicilioRealLocalidad;
    this.domicilioRealDepartamento = domicilioRealDepartamento === undefined ? null : domicilioRealDepartamento;
    this.barrio = barrio === undefined ? null : barrio;
    this.idGarante = idGarante;
  }
}
