import { Component, Input, OnInit } from '@angular/core';
import { GrupoUnicoService } from './shared-grupo-unico/grupo-unico.service';
import AccionGrupoUnico from './shared-grupo-unico/accion-grupo-unico.enum';
import { CuadranteFormulario } from '../formularios/cuadrantes/cuadrante-formulario';
import { GrupoFamiliarService } from '../formularios/shared/grupo-familiar.service';
import { GrupoUnicoConsulta } from '../formularios/shared/modelo/grupo-unico-consulta';
import { ConsultarGrupoFamiliarIntegrantes } from "../formularios/shared/modelo/consultar-grupo-familiar-integrantes.model";

@Component({
  selector: 'grupo-unico',
  template: '<iframe width="100%" frameborder="0" height="{{ancho}}" [src]="url | safe"></iframe>',
})
export class GrupoUnicoComponent extends CuadranteFormulario implements OnInit {
  public url: string = '';

  @Input() public recurso: AccionGrupoUnico;
  @Input() public sexo: string;
  @Input() public dni: string;
  @Input() public pais: string;
  @Input() public ancho: number;

  @Input()
  public set grupoUnico(grupoUnicoConsulta: GrupoUnicoConsulta) {
    this.consultarGrupoUnico(grupoUnicoConsulta);
  }

  constructor(private grupoUnicoService: GrupoUnicoService,
              private grupoFamiliarService: GrupoFamiliarService) {
    super();
  }

  public ngOnInit(): void {
    if (this.formulario) {
      this.recurso = AccionGrupoUnico.GRUPO_FAMILIAR_MODIFICACION_INTERNA;
      this.ancho = 700;
      this.dni = this.formulario.solicitante.nroDocumento;
      this.sexo = this.formulario.solicitante.sexoId;
      this.pais = this.formulario.solicitante.codigoPais;
    }
    this.grupoUnicoService
      .obtenerUrlAutorizada(this.recurso, this.dni, this.sexo, this.pais)
      .subscribe((url) => this.url = url);
  }

  public consultarGrupoUnico(grupoUnicoConsulta: GrupoUnicoConsulta): void {
    this.recurso = grupoUnicoConsulta.recurso;
    this.dni = grupoUnicoConsulta.dni;
    this.ancho = grupoUnicoConsulta.ancho;
    this.sexo = grupoUnicoConsulta.sexo;
    this.pais = grupoUnicoConsulta.pais;
    this.grupoUnicoService
      .obtenerUrlAutorizada(this.recurso, this.dni, this.sexo, this.pais)
      .subscribe((url) => this.url = url);
  }

  public actualizarDatos() {
    if (this.formulario.integrantes.length > 1) {
      let comando = new ConsultarGrupoFamiliarIntegrantes();
      comando.integrantes = [];
      this.formulario.integrantes.forEach((i) => {
        if (i.idEstado != 4 && i.idEstado != 6) {
          comando.integrantes.push(i);
        }
      });
      this.grupoFamiliarService.consultarGrupoFamiliarIntegrantes(comando).subscribe((res) => {
        this.formulario.integrantesTienenGrupo = res;
      });
    }
  }

  public esValido(): boolean {
    this.grupoFamiliarService
      .consultarExistenciaGrupo({dni: this.dni, sexo: this.sexo, pais: this.pais}).subscribe(
      (resultado) => {
        if (resultado === false) {
          this.formulario.tieneGrupo = false;
          return false;
        }
      });
    this.formulario.tieneGrupo = true;
    return true;
  }

  inicializarDeNuevo(): boolean {
    return false;
  }
}
