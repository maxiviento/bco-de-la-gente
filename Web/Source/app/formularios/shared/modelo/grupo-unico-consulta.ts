import AccionGrupoUnico from '../../../grupo-unico/shared-grupo-unico/accion-grupo-unico.enum';

export class GrupoUnicoConsulta {
  public recurso: AccionGrupoUnico;
  public sexo: string;
  public dni: string;
  public pais: string;
  public ancho: number;

  constructor(recurso?: AccionGrupoUnico,
              sexo?: string,
              dni?: string,
              pais?: string,
              ancho?: number) {
    this.recurso = recurso;
    this.sexo = sexo;
    this.dni = dni;
    this.pais = pais;
    this.ancho = ancho;
  }
}
