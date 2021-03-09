import { IngresoGrupo } from './ingreso-grupo.model';

export class IngresoGrupoComando{
  public idFormulario: number;
  public ingresosGrupo: IngresoGrupo[];

  constructor(idFormulario?: number, ingresosGrupo?: IngresoGrupo[]) {
    this.idFormulario = idFormulario;
    this.ingresosGrupo = ingresosGrupo;
  }
}
