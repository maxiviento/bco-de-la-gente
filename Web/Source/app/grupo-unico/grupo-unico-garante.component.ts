import {Component, OnInit} from '@angular/core';
import {GrupoUnicoService} from './shared-grupo-unico/grupo-unico.service';
import AccionGrupoUnico from './shared-grupo-unico/accion-grupo-unico.enum';
import {CuadranteFormulario} from '../formularios/cuadrantes/cuadrante-formulario';
import {NotificacionService} from '../shared/notificacion.service';

@Component({
  selector: 'grupo-unico-garante',
  template: '<iframe width="100%" frameborder="0" height="{{ancho}}" [src]="url | safe"></iframe>',
})
export class GrupoUnicoGaranteComponent extends CuadranteFormulario implements OnInit {
  public url: string = '';

  private recurso: AccionGrupoUnico = AccionGrupoUnico.GRUPO_FAMILIAR_MODIFICACION_INTERNA;
  private sexo: string;
  private dni: string;
  private pais: string;
  public ancho: number = 700;

  constructor(private grupoUnicoService: GrupoUnicoService,
              private notificacionService: NotificacionService) {
    super();
  }

  public ngOnInit(): void {
    if (this.formulario.garantes[0]) {
      if (this.formulario.garantes[0].nroDocumento) {
        this.dni = this.formulario.garantes[0].nroDocumento;
        this.sexo = this.formulario.garantes[0].sexoId;
        this.pais = this.formulario.garantes[0].codigoPais;

        this.grupoUnicoService
          .obtenerUrlAutorizada(this.recurso, this.dni, this.sexo, this.pais)
          .subscribe((url) => this.url = url);
      }
    } else {
      this.notificacionService.informar(Array.of('Debe seleccionar un garante.'), true);
    }
  }

  public actualizarDatos() {
    return;
  }

  public esValido(): boolean {
    return true;
  }

  inicializarDeNuevo(): boolean {
    return true;
  }
}
