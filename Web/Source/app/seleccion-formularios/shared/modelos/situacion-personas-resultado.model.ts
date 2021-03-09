export class SituacionPersonasResultado {
  public idNumero: number;
  public nombre: string;
  public apellido: string;
  public sexoId: string;
  public nombreSexo: string;
  public tipoDocumento: string;
  public codigoPais: string;
  public nroDocumento: string;
  public fechaNacimiento: Date;
  public edad: string;
  public departamento: string;
  public localidad: string;
  public cuil: string;
  public codigoArea: string;
  public telefono: string;
  public codigoAreaCelular: string;
  public celular: string;
  public email: string;

  constructor(idNumero?: number,
              nombre?: string,
              apellido?: string,
              sexoId?: string,
              nombreSexo?: string,
              tipoDocumento?: string,
              codigoPais?: string,
              nroDocumento?: string,
              fechaNacimiento?: Date,
              edad?: string,
              departamento?: string,
              localidad?: string,
              cuil?: string,
              codigoArea?: string,
              telefono?: string,
              codigoAreaCelular?: string,
              celular?: string,
              email?: string) {
    this.idNumero = idNumero;
    this.nombre = nombre;
    this.apellido = apellido;
    this.sexoId = sexoId;
    this.nombreSexo = nombreSexo;
    this.tipoDocumento = tipoDocumento;
    this.codigoPais = codigoPais;
    this.nroDocumento = nroDocumento;
    this.fechaNacimiento = fechaNacimiento;
    this.edad = edad;
    this.departamento = departamento;
    this.localidad = localidad;
    this.cuil = cuil;
    this.codigoArea = codigoArea;
    this.telefono = telefono;
    this.codigoAreaCelular = codigoAreaCelular;
    this.celular = celular;
    this.email = email;
  }
}
