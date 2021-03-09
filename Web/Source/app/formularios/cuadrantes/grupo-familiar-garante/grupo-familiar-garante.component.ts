import { Component, OnInit, Output } from '@angular/core';
import { GrupoFamiliarService } from '../../shared/grupo-familiar.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { CuadranteFormulario } from '../cuadrante-formulario';

@Component({
  selector: 'bg-grupo-familiar-garante',
  templateUrl: './grupo-familiar-garante.html',
  styleUrls: ['./grupo-familiar-garante.component.scss'],
  providers: [GrupoFamiliarService],
})

export class GrupoFamiliarGaranteComponent extends CuadranteFormulario implements OnInit {
  @Output()
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
    if (this.formulario.garantes[0]) {
      if (this.formulario.garantes[0].nroDocumento) {
        this.dni = this.formulario.garantes[0].nroDocumento;
        this.sexoId = this.formulario.garantes[0].sexoId;
        this.pais = this.formulario.garantes[0].codigoPais;

        this.consultarGrupoFamiliar();
      }
    }else {
      this.notificacionService.informar(Array.of('Debe seleccionar un garante.'), true);
    }
  }

  public consultarGrupoFamiliar() {
    if (!this.formulario.tieneGrupoGarante) {
      this.grupoFamiliarService.consultarExistenciaGrupo({dni: this.dni, sexo: this.sexoId, pais: this.pais}).subscribe(
        (resultado) => {
          this.formulario.tieneGrupoGarante = resultado;
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
      (errores) => {
        this.notificacionService.informar(errores, true);
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
