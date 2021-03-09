import { Injectable } from '@angular/core';
import { Cuadrante } from './modelo/cuadrante.model';
import { FormulariosService } from './formularios.service';
import { Formulario } from './modelo/formulario.model';

@Injectable()
export class ActualizacionMasivaCuadrantesFormulariosService {
  private formulario: Formulario;
  public mensaje: string[];

  constructor(private formularioService: FormulariosService) {
  }

  public actualizarCuadrantes(cuadrantes: Cuadrante[], formulario: Formulario): string[] {
    this.formulario = formulario;
    this.mensaje = null;

    cuadrantes.forEach((cuadrante) => {
      switch (cuadrante.idCuadrante) {
        case 1:
          // solicitante
          break;
        case 2:
          // grupo unico no hay grupo unico
          break;
        case 3:
          // DestinoFondosComponent
          this.actualizarDestinoFondos();
          break;
        case 4:
          //  CondicionesSolicitadasComponent;
          this.actualizarCondicionesSolicitadas();
          break;
        case 5:
          //  CursoComponent;
          this.actualizarCursos();
          break;
        case 6:
          //  GaranteComponent;
          break;
        case 7:
          //  DatosEmprendimientoComponent
          break;
        case 8:
          // PatrimonioDelSolicitante
          this.actualizarPatrimonioSolicitante();
          break;
        case 9:
          //  OrganizacionIndividualComponent
          break;
        case 10:
          //  MercadoYComercializacionComponent
          break;
        case 15:
          // PrecioVentaComponent
          break;
        case 22:
          // IntegrantesPersonasComponent
          break;
        case 23:
          // OngComponent
          this.actualizarONG();
          break;
        default:
          return null;
      }
    });
    return this.mensaje;
  }

  private agregarMensaje(mensaje: string): void {
    if (!this.mensaje) {
      this.mensaje = [];
    }
    this.mensaje.push(mensaje);
  }

  private actualizarDestinoFondos(): void {
    if (this.formulario.destinosFondos) {
      if (this.formulario.destinosFondos.destinosFondo.length > 0) {
        this.formularioService.actualizarDestinosFondosAsociativas(this.formulario.idAgrupamiento, this.formulario.destinosFondos).subscribe(() => {
          return;
        });
      }
    }
  }

  private actualizarCondicionesSolicitadas() {
    if (this.formulario.condicionesSolicitadas) {
      this.formularioService.actualizarCondicionesSolicitadasAsociativas(this.formulario.idAgrupamiento, this.formulario.condicionesSolicitadas).subscribe(() => {
        return;
      });
    }
  }

  private actualizarCursos() {
    if (this.formulario.detalleLinea.conCurso) {
      if (this.formulario.solicitudesCurso) {
        this.formularioService.actualizarCursosAsociativas(this.formulario.idAgrupamiento, this.formulario.solicitudesCurso).subscribe(() => {
          return;
        });
      }
    }
  }

  private actualizarPatrimonioSolicitante() {
    this.formularioService.actualizarPatrimonioSolicitanteAsociativas(this.formulario.idAgrupamiento, this.formulario.patrimonioSolicitante).subscribe(() => {
      return;
    });
  }

  private actualizarONG() {
    if (this.formulario.idAgrupamiento) {
      if (!this.formulario.datosONG.numeroGrupo) {
        this.formularioService.obtenerNumeroGrupo(this.formulario.datosONG)
          .subscribe((nuevoNumero) => {
            this.formulario.datosONG.numeroGrupo = nuevoNumero;
            this.formularioService.registrarOngParaFormularios(this.formulario.idAgrupamiento, this.formulario.datosONG).subscribe(
              () => { return; });
          });
      } else {
        this.formularioService.registrarOngParaFormularios(this.formulario.idAgrupamiento, this.formulario.datosONG).subscribe();
      }
    }
  }
}
