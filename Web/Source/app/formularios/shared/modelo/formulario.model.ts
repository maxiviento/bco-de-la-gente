import { CondicionesSolicitadas } from './condiciones-solicitadas.model';
import { SolicitudCurso } from './solicitud-curso.model';
import { OpcionDestinoFondos } from './opcion-destino-fondos';
import { Persona } from '../../../shared/modelo/persona.model';
import { DetalleLineaFormulario } from './detalle-linea-formulario.model';
import { PatrimonioSolicitante } from './patrimonio-solicitante.model';
import { Emprendimiento } from './emprendimiento.model';
import { ResultadoEstimadoMensual } from './resultado-estimado.mensual';
import { DeudaEmprendimiento } from './deuda.emprendimiento';
import { InversionRealizada } from './inversion-realizada.model';
import { NecesidadInversion } from './necesidad-inversion.model';
import { IngresoGrupo } from './ingreso-grupo.model';
import { MercadoComercializacion } from './mercado-comercializacion.model';
import { PrecioVenta } from './precio-venta.model';
import { JsonProperty } from '../../../shared/map-utils';
import { MiembroEmprendimiento } from './miembro-emprendimiento.model';
import { Integrante } from '../../../shared/modelo/integrante.model';
import { OngFormulario } from './ong-formulario.model';

export class Formulario {
  public id: number;
  public detalleLinea: DetalleLineaFormulario;
  public idOrigen: number;
  public solicitante: Persona;
  public garantes: Persona[];
  public solicitudesCurso: SolicitudCurso[];
  public destinosFondos: OpcionDestinoFondos;
  public condicionesSolicitadas: CondicionesSolicitadas;
  @JsonProperty({clazz: Emprendimiento})
  public datosEmprendimiento: Emprendimiento;
  public miembrosEmprendimiento: MiembroEmprendimiento[];
  public idEstado: number;
  public tieneGrupo: boolean = false;
  public tieneGrupoGarante: boolean = false;
  public motivoRechazo: string;
  public observaciones: string;
  public idBanco: number;
  public idSucursal: number;
  public patrimonioSolicitante: PatrimonioSolicitante;
  public mercadoComercializacion: MercadoComercializacion;
  public resultadoEstimadoMensual: ResultadoEstimadoMensual;
  public deudasEmprendimiento: DeudaEmprendimiento[];
  public inversionesRealizadas: InversionRealizada[];
  public necesidadInversion: NecesidadInversion;
  public ingresosGrupo: IngresoGrupo[];
  public precioVenta: PrecioVenta;
  public idAgrupamiento: number;
  public integrantes: Integrante[];
  public titularSolicitante: boolean;
  public fechaForm: Date;
  public datosONG: OngFormulario;
  public integrantesTienenGrupo: boolean;
  public numeroCaja: string;

  constructor(id?: number,
              detalleLinea?: DetalleLineaFormulario,
              idOrigen?: number,
              solicitante?: Persona,
              garantes?: Persona[],
              solicitudesCurso?: SolicitudCurso[],
              destinosFondos?: OpcionDestinoFondos,
              condicionesSolicitadas?: CondicionesSolicitadas,
              datosEmprendimiento?: Emprendimiento,
              miembrosEmprendimiento?: MiembroEmprendimiento[],
              idEstado?: number,
              tieneGrupo?: boolean,
              motivoRechazo?: string,
              observaciones?: string,
              idBanco?: number,
              idSucursal?: number,
              patrimonioSolicitante?: PatrimonioSolicitante,
              mercadoComercializacion?: MercadoComercializacion,
              ingresosGrupo?: IngresoGrupo[],
              resultadoEstimadoMensual?: ResultadoEstimadoMensual,
              deudasEmprendimiento?: DeudaEmprendimiento[],
              inversionesRealizadas?: InversionRealizada[],
              necesidadInversion?: NecesidadInversion,
              precioVenta?: PrecioVenta,
              fechaForm?: Date,
              idAgrupamiento?: number,
              integrantes?: Integrante[],
              titularSolicitante?: boolean,
              datosONG?: OngFormulario,
              integrantesTienenGrupo?: boolean,
              numeroCaja?: string) {

    this.id = id;
    this.detalleLinea = detalleLinea;
    this.idOrigen = idOrigen;
    this.solicitante = solicitante;
    this.garantes = garantes || [];
    this.solicitudesCurso = solicitudesCurso;
    this.destinosFondos = destinosFondos;
    this.condicionesSolicitadas = condicionesSolicitadas;
    this.datosEmprendimiento = datosEmprendimiento;
    this.miembrosEmprendimiento = miembrosEmprendimiento;
    this.idEstado = idEstado;
    this.tieneGrupo = tieneGrupo;
    this.motivoRechazo = motivoRechazo;
    this.observaciones = observaciones;
    this.idBanco = idBanco;
    this.idSucursal = idSucursal;
    this.patrimonioSolicitante = patrimonioSolicitante;
    this.mercadoComercializacion = mercadoComercializacion;
    this.ingresosGrupo = ingresosGrupo;
    this.resultadoEstimadoMensual = resultadoEstimadoMensual;
    this.deudasEmprendimiento = deudasEmprendimiento;
    this.inversionesRealizadas = inversionesRealizadas;
    this.necesidadInversion = necesidadInversion;
    this.precioVenta = precioVenta;
    this.idAgrupamiento = idAgrupamiento;
    this.integrantes = integrantes;
    this.titularSolicitante = titularSolicitante;
    this.fechaForm = fechaForm;
    this.datosONG = datosONG;
    this.integrantesTienenGrupo = integrantesTienenGrupo;
    this.numeroCaja = numeroCaja;
  }
}
