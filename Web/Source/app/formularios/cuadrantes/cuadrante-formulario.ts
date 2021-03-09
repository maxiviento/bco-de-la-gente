import { Formulario } from '../shared/modelo/formulario.model';

export abstract class CuadranteFormulario {

  public editable: boolean;

  public formulario: Formulario;

  public abstract actualizarDatos();

  public abstract esValido(): boolean;

  public abstract inicializarDeNuevo(): boolean;
}
