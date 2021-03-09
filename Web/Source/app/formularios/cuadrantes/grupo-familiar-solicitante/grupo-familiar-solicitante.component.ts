import { Component, OnInit } from '@angular/core';
import { GrupoFamiliarService } from '../../shared/grupo-familiar.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { CuadranteFormulario } from '../cuadrante-formulario';

@Component({
  selector: 'bg-grupo-familiar-solicitante',
  templateUrl: './grupo-familiar-solicitante.html',
  styleUrls: ['./grupo-familiar-solicitante.component.scss'],
  providers: [GrupoFamiliarService],
})

export class GrupoFamiliarSolicitanteComponent extends CuadranteFormulario implements OnInit {
  public detalleGrupoFamiliar: any;

  private dni: String;
  private sexoId: String;
  private pais: String;

  public busco: boolean = false;
  private totalIngresosFamiliares = 0.0;

  constructor(private notificacionService: NotificacionService,
              private grupoFamiliarService: GrupoFamiliarService) {
    super();
  }

  public ngOnInit(): void {
    this.dni = this.formulario.solicitante.nroDocumento;
    this.sexoId = this.formulario.solicitante.sexoId;
    this.pais = this.formulario.solicitante.codigoPais;

    this.consultarGrupoFamiliar();
  }

  public consultarGrupoFamiliar() {
    if (!this.formulario.tieneGrupo) {
      this.grupoFamiliarService.consultarExistenciaGrupo({dni: this.dni, sexo: this.sexoId, pais: this.pais}).subscribe(
        (resultado) => {
          this.formulario.tieneGrupo = resultado;
        }
      )
    }
    this.grupoFamiliarService.consultar({dni: this.dni, sexo: this.sexoId, pais: this.pais}).subscribe(
      (resultado) => {
        this.busco = true;
        if (resultado && resultado.grupo && resultado.grupo.integrantes) {
          this.detalleGrupoFamiliar = resultado.grupo.integrantes;
          this.detalleGrupoFamiliar.forEach((integrante) => {
            this.totalIngresosFamiliares += parseFloat(integrante.caracteristicas.reverse()[0].valor || '0');
            this.buscarDispacidad(integrante);
            this.buscarVinculo(integrante);
            this.buscarCondicionAcademica(integrante);
            this.buscarCondicionLaboral(integrante);
            //TODO: buscar una mejor forma
            if (integrante.fechaNacimiento) {
              integrante.edad = (new Date()).getFullYear() - new Date(integrante.fechaNacimiento).getFullYear();
            }
          });
        } else {
          this.notificacionService.informar(['No se encontro informacion sobre el Grupo familiar']);
        }
      },
      () => {
        this.notificacionService.informar(['Ocurrio un error al consultar la informacion de grupo familiar']);
      });
  }

  public actualizarDatos() {
    return;
  }

  public esValido(): boolean {
    return true;
  }

  private buscarDispacidad(integrante: any): void {
    if (integrante.caracteristicas.filter((x) => x.tipoCaracteristica.idTipoCaracteristica === '2014').length > 0) {
      integrante.discapacidad = true;
    }
  }

  private buscarVinculo(integrante: any): void {
    let vinculo = integrante.caracteristicas.filter((x) => x.tipoCaracteristica.idTipoCaracteristica === '2019');
    if (vinculo.length > 0) {
      integrante.relaciones = vinculo[0].descripcion;
    }
  }

  private buscarCondicionAcademica(integrante: any): void {
    let caracteristicaAsistencia = integrante.caracteristicas.filter((x) => x.tipoCaracteristica.idTipoCaracteristica === '2001');
    let caracteristicaNivel = integrante.caracteristicas.filter((x) => x.tipoCaracteristica.idTipoCaracteristica === '2004');

    if (caracteristicaNivel.length > 0) {
      integrante.nivelAlcanzado = caracteristicaNivel[0].descripcion;
    }
    if (caracteristicaAsistencia.length > 0) {
      integrante.asisteEstablecimientoEducativo = caracteristicaAsistencia[0].descripcion;
    }
  }

  private buscarCondicionLaboral(integrante: any): void {
    let caracteristica = integrante.caracteristicas.filter((x) => x.tipoCaracteristica.idTipoCaracteristica === '2003');
    if (caracteristica.length > 0) {
      integrante.trabaja = caracteristica[0].descripcion.toLowerCase().includes('no trabaja') ? 'N' : 'S';
      integrante.condicionLaboral = caracteristica[0].descripcion;
    }
  }

  inicializarDeNuevo(): boolean {
    return true;
  }
}
