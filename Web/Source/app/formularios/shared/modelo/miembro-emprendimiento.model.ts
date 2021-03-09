import { Persona } from '../../../shared/modelo/persona.model';
import { Vinculo } from './vinculo.model';

export class MiembroEmprendimiento{
  public persona: Persona;
  public idVinculo: string;
  public vinculo: string;
  public tarea: string;
  public horarioTrabajo: string;
  public remuneracion: number;
  public antecedentesLaborales: boolean;
  public esSolicitante: boolean;

  constructor(persona?: Persona,
              idVinculo?: string,
              vinculo?: string,
              tarea?: string,
              horarioTrabajo?: string,
              remuneracion?: number,
              antecedentesLaborales?: boolean,
              esSolicitante?: boolean) {
    this.persona = persona;
    this.idVinculo = idVinculo;
    this.vinculo = vinculo;
    this.tarea = tarea;
    this.horarioTrabajo = horarioTrabajo;
    this.remuneracion = remuneracion;
    this.antecedentesLaborales = antecedentesLaborales;
    this.esSolicitante = esSolicitante;
  }
}
