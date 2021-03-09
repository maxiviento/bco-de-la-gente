import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewRef } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Formulario } from './shared/modelo/formulario.model';
import { ContenedorCuadrantesDirective } from './cuadrantes/contenedor-cuadrantes.directive';
import { FormulariosService } from './shared/formularios.service';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { NotificacionService } from '../shared/notificacion.service';
import { Cuadrante } from './shared/modelo/cuadrante.model';
import { RechazarFormularioComando } from './shared/modelo/rechazar-formulario-comando.model';
import { ModalMotivoBajaComponent } from '../shared/modal-motivo-baja/modal-motivo-baja.component';
import { CuadranteFormulario } from './cuadrantes/cuadrante-formulario';
import { componentePorId } from './cuadrantes/componente-cuadrante-por-id';
import { ModalVolverFormularioComponent } from './modal-volver/modal-volver-formulario.component';
import { ValidacionFormularioServicio } from './shared/validacion-formulario.service';
import { SolicitanteComponent } from './cuadrantes/solicitante/solicitante.component';
import { ModalMotivoRechazoComponent } from './modal-motivo-rechazo/modal-motivo-rechazo.component';
import { Persona } from '../shared/modelo/persona.model';
import { ToastrService } from 'ngx-toastr';
import { ActualizacionMasivaCuadrantesFormulariosService } from './shared/actualizacion-masiva-cuadrantes-formularios.service';
import { GrupoFamiliarService } from './shared/grupo-familiar.service';
import { PersonaService } from './cuadrantes/persona/persona.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';
import { ConsultarGrupoFamiliarIntegrantes } from "./shared/modelo/consultar-grupo-familiar-integrantes.model";

@Component({
  selector: 'bg-formularios',
  templateUrl: './formularios.component.html',
  styleUrls: ['./formularios.component.scss'],
})

export class FormularioComponent implements OnInit {
  public formulario: Formulario;
  @ViewChild(ContenedorCuadrantesDirective) public contenedorCuadrantes: ContenedorCuadrantesDirective;
  public cuadrantesPantalla: Cuadrante[] = [];
  public cuadrantesImpresion: Cuadrante[] = [];
  public indiceActual: number = 0;
  public esVer: boolean = false;
  public esRevisar: boolean = false;
  public logo: string;
  public titulo: string = 'FORMULARIO';
  public accion: number = 0; // 1 es iniciar y 2 es rechazar

  private vistaCuadrante: ViewRef[] = [];
  private instanciaCuadrante: CuadranteFormulario[] = [];
  private componentesEditables: boolean = true;
  private esRegistro: boolean;
  public integrantesFormulario: Persona[] = [];
  public nroSuac: string = '';

  constructor(private formulariosService: FormulariosService,
              private lineasPrestamoService: LineaService,
              private _componentFactoryResolver: ComponentFactoryResolver,
              private route: ActivatedRoute,
              private notificacionService: NotificacionService,
              private _router: Router,
              private modalService: NgbModal,
              private toastr: ToastrService,
              private validacionFormularioServicio: ValidacionFormularioServicio,
              private actualizacionMasivaService: ActualizacionMasivaCuadrantesFormulariosService,
              private grupoFamiliarService: GrupoFamiliarService,
              private personaService: PersonaService,
              private titleService: Title) {
  }

  public ngOnInit(): void {
    this.esRegistro = this._router.url.toString().indexOf('nuevo') >= 0;
    if (this.esRegistro) {
      this.titleService.setTitle('Registrar formulario ' + TituloBanco.TITULO);
      this.formulario = this.formulariosService.formulario;
      this.formulariosService.formulario.integrantes = [];
      this.formulario.integrantes = [];
      this.obtenerLogo();
      this.lineasPrestamoService.consultarCuadrantesFormulario(this.formulario.detalleLinea.id)
        .subscribe(
          (cuadrantes) => {
            this.filtrarCuadrantesParaPantalla(cuadrantes);
            this.filtrarCuadrantesParaImpresion(cuadrantes);
            this.cambiarCuadrante(0);
          },
          (errores) => {
            this.notificacionService.informar(errores, true);
          });
    } else {
      this.route.params
        .switchMap((params: Params) =>
          this.formulariosService.consultarFormulario(+params['id']))
        .subscribe(
          (formularioCargado) => {
            this.formulario = this.formulariosService.inicializarFormularioCargado(formularioCargado);
            this.solicitanteTieneGrupo();
            this.integrantesTienenGrupo();
            this.obtenerLogo();
            this.consultarPersona();
            this.obtenerNroSuac(formularioCargado.id);
            this.lineasPrestamoService.consultarCuadrantesFormulario(this.formulario.detalleLinea.id)
              .subscribe(
                (cuadrantes) => {
                  this.filtrarCuadrantesParaPantalla(cuadrantes);
                  this.cambiarCuadrante(0);
                },
                (errores) => {
                  this.notificacionService.informar(errores, true);
                });
          },
          (errores) => {
            this.notificacionService.informar(errores, true);
          });

      this.componentesEditables = this.esRegistro || this._router.url.toString().indexOf('edicion') >= 0;
      this.esRevisar = this._router.url.toString().indexOf('revision') >= 0;
      this.esVer = !this.esRevisar && !this.componentesEditables;

      if (this.esRevisar) {
        if (this._router.url.toString().indexOf('iniciar') >= 0) {
          this.accion = 1;
          this.titleService.setTitle('Iniciar formulario ' + TituloBanco.TITULO);
        } else if (this._router.url.toString().indexOf('rechazar') >= 0) {
          this.accion = 2;
          this.titleService.setTitle('Rechazar formulario ' + TituloBanco.TITULO);
        }
      }
      this.setearTitulo();
      this.validarEdadIntegrantesFormulario();
    }
  }

  public consultarPersona() {
    let solicitante = this.formulario.solicitante;
    let persona: any = {
      nroDocumento: solicitante.nroDocumento,
      codigoPais: solicitante.codigoPais,
      sexoId: solicitante.sexoId,
    };
    this.personaService.consultarPersona(persona).subscribe(
      (resultado) => {
        if (resultado) {
          this.formulario.solicitante = resultado;
        } else {
          this.notificacionService.informar(['No se encontro el solicitante']);
        }
      },
      (errores) => {
        this.notificacionService.informar(<string[]>errores, true);
      });
  }

  public cambiarCuadrante(indice: number): void {
    let viewContainerRef = this.contenedorCuadrantes.viewContainerRef;
    let instanciaCuadranteActual = this.instanciaCuadrante[this.indiceActual];
    let puedeCambiarCuadrante = !instanciaCuadranteActual ||
      !(instanciaCuadranteActual instanceof SolicitanteComponent) ||
      instanciaCuadranteActual.esValido(); // permite cambiar de pestaña si la actual es invalida (excepto para Solicitante)

    if (puedeCambiarCuadrante || !this.componentesEditables) {
      if (instanciaCuadranteActual) { // la primera vez es null
        if (instanciaCuadranteActual.esValido()) {
          instanciaCuadranteActual.actualizarDatos();
        } else {
          let cuadrante = this.cuadrantesPantalla[this.indiceActual];
          if (!this.esVer) {
            this.mostrarAvisoDeCuadranteInvalido(cuadrante.nombre);
          }
        }
      }
      this.vistaCuadrante[this.indiceActual] = viewContainerRef.detach(0);

      if (this.vistaCuadrante[indice] && !this.instanciaCuadrante[indice].inicializarDeNuevo()) {
        viewContainerRef.insert(this.vistaCuadrante[indice]);
      } else {
        let cuadrante = this.cuadrantesPantalla[indice];
        let componentFactory = componentePorId(cuadrante.idCuadrante, this._componentFactoryResolver);

        let componenteCuadrante = <CuadranteFormulario>viewContainerRef.createComponent(componentFactory).instance;
        componenteCuadrante.editable = this.componentesEditables;
        componenteCuadrante.formulario = this.formulario;
        this.instanciaCuadrante[indice] = componenteCuadrante;
        this.vistaCuadrante[indice] = viewContainerRef.get(0);
      }
      this.indiceActual = indice;
    }
  }

  public enviar() {
    this.notificacionService.confirmar('Se guardará y enviará el formulario. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        if (!this.validarCuadranteActual()) {
          return;
        }
        if (!this.validarCuadrantes()) {
          return;
        }

        this.formulariosService.EnviarFormulario(this.formulario)
          .subscribe((resultado) => {
              this.formulario.id = resultado;
              this.formulariosService.ValidarEstadosFormulariosAgrupados(this.formulario.idAgrupamiento).subscribe((esValido) => {
                if (esValido) {
                  this.notificacionService.informar(Array.of('Formulario enviado con éxito'), false)
                    .result.then(() => {
                    this._router.navigate(['/formularios']);
                  });
                } else {
                  this.formulariosService.EnviarFormulario(this.formulario)
                    .subscribe((resultado) => {
                        this.formulario.id = resultado;
                        this.notificacionService.informar(Array.of('Formulario enviado con éxito'), false)
                          .result.then(() => {
                          this._router.navigate(['/formularios']);
                        });
                      },
                      (errores) => {
                        this.notificacionService.informar(errores, true);
                      });
                }
              })
            },
            (errores) => {
              this.notificacionService.informar(errores, true);
            });
      }
    })
  }

  public guardar() {
    this.notificacionService.confirmar('Se guardara el formulario. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        if (!this.validarCuadranteActual()) {
          return;
        }
        if (this.formulario.idEstado !== 3) {
          this.actualizacionMasivaService.actualizarCuadrantes(this.cuadrantesPantalla, this.formulario);
        }
        this.notificacionService.informar(Array.of('Formulario guardado con éxito'), false)
          .result
          .then(() => {
            this._router.navigate(['/formularios']);
          });
      }
    })
  }

  public darDeBajaFormulario(): void {
    const motivoBaja = this.modalService.open(ModalMotivoBajaComponent, {backdrop: 'static', size: 'lg'});
    motivoBaja.result.then(
      (idMotivoBaja) => {
        if (idMotivoBaja) {
          this.formulariosService.darDeBajaFormulario(new RechazarFormularioComando(this.formulario.id, idMotivoBaja))
            .subscribe((resultado) => {
              this.formulario.id = resultado;
              this.notificacionService.informar(Array.of('Formulario eliminado con éxito'), false)
                .result.then(() => {
                this._router.navigate(['/formularios']);
              });
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      });
  }

  public rechazarFormulario(): void {
    const motivoRechazo = this.modalService.open(ModalMotivoRechazoComponent, {backdrop: 'static', windowClass: 'modal-l'});
    motivoRechazo.componentInstance.ambito = 'Formulario';
    motivoRechazo
      .result
      .then((motivoRechazoComando) => {
        if (motivoRechazoComando) {
          this.formulariosService.rechazarFormulario(new RechazarFormularioComando(this.formulario.id, undefined, motivoRechazoComando.motivosRechazo, motivoRechazoComando.numeroCaja))
            .subscribe((resultado) => {
              // this.formulario.id = resultado;
              this.notificacionService.informar(Array.of('Formulario rechazado con éxito.'), false)
                .result.then(() => {
                this._router.navigate(['/formularios']);
              });
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      });
  }

  public iniciar() {
    this.notificacionService.confirmar('Se iniciará el formulario. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        this.formulariosService.IniciarFormulario(this.formulario)
          .subscribe((resultado) => {
              this.formulario.id = resultado;
              this.notificacionService.informar(Array.of('Formulario iniciado con éxito'), false)
                .result.then(() => {
                this._router.navigate(['/formularios']);
              });
            },
            (errores) => {
              this.notificacionService.informar(errores, true);
            });
      }
    })
  }

  public volver() {
    if (this._router.url.includes('prestamo/formulario')) {
      this._router.navigate(['/conformar-prestamos']);
      return;
    }
    if (!this.componentesEditables) { // estoy en el ver y no tengo que guardar
      this._router.navigate(['/formularios']);
    } else {
      let modalVolver = this.modalService.open(ModalVolverFormularioComponent, {backdrop: 'static', size: 'lg'});
      modalVolver.result.then(
        (guardar) => {
          if (guardar) {
            this.guardar();
          } else {
            this._router.navigate(['/formularios']);
          }
        });
    }
  }

  private filtrarCuadrantesParaPantalla(cuadrantes: Cuadrante[]): void {
    cuadrantes = cuadrantes.filter((c) => c.idTipoSalida !== 2); // Solo impresion
    cuadrantes = cuadrantes.filter((c) => c.idCuadrante !== 2 || !(this.esVer || this.esRevisar)); // Quito IFrame Solicitante
    this.cuadrantesPantalla = cuadrantes.filter((c) => c.idCuadrante !== 18 || !(this.esVer || this.esRevisar)); // Quito IFrame Garante
  }

  private filtrarCuadrantesParaImpresion(cuadrantes: Cuadrante[]): void {
    cuadrantes = cuadrantes.filter((c) => c.idTipoSalida !== 1);
    cuadrantes = cuadrantes.filter((c) => c.idCuadrante !== 2);
    this.cuadrantesImpresion = cuadrantes.filter((c) => c.idCuadrante !== 18);
  }

  private validarCuadranteActual(): boolean {
    let cuadranteActual = this.instanciaCuadrante[this.indiceActual];
    if (!cuadranteActual.esValido()) {
      this.notificacionService.informar(['Debe completar la pestaña de forma correcta para poder continuar'], false);
      return false;
    }
    cuadranteActual.actualizarDatos();
    return true;
  }

  private validarCuadrantes(): boolean {
    let mensaje = this.validacionFormularioServicio.validarCuadrantes(this.cuadrantesPantalla, this.formulario);
    if (mensaje) {
      this.notificacionService.informar(mensaje, false);
      return false;
    }
    mensaje = this.actualizacionMasivaService.actualizarCuadrantes(this.cuadrantesPantalla, this.formulario);
    if (mensaje) {
      this.notificacionService.informar(mensaje, false);
      return false;
    }
    return true;
  }

  private obtenerLogo() {
    this.formulariosService.obtenerLogo(this.formulario.detalleLinea.id)
      .subscribe((l) => this.logo = l);
  }

  private setearTitulo(): void {
    this.titulo = 'VER FORMULARIO';
    this.titleService.setTitle('Ver formulario ' + TituloBanco.TITULO);
    if (this._router.url.toString().indexOf('nuevo') >= 0) {
      this.titulo = 'REGISTRAR FORMULARIO';
    }
    if (this._router.url.toString().indexOf('edicion') >= 0) {
      this.titulo = 'EDITAR FORMULARIO';
      this.titleService.setTitle('Editar formulario ' + TituloBanco.TITULO);
    }
    if (this._router.url.toString().indexOf('revision') >= 0) {
      this.titulo = 'REVISAR FORMULARIO';
      this.titleService.setTitle('Revisar formulario ' + TituloBanco.TITULO);
    }
  }

  public imprimir(id: number) {
    this._router.navigate(['reporte-formulario/' + id]);
  }

  private validarEdadIntegrantesFormulario(): void {
    this.formulariosService.personaEncontrada$.subscribe((persona) => {
      // ToDo: al aplicar lineas asociativas las lineas debajo no funcionarían : resolver
      if (this.integrantesFormulario.length > 0) {
        this.integrantesFormulario = [];
      }
      /* Agrega el integrante sólo si no estaba en la lista */
      if (!this.integrantesFormulario.find((p) =>
        persona.sexoId === p.sexoId &&
        persona.nroDocumento === p.nroDocumento &&
        persona.tipoDocumento === p.tipoDocumento)) {
        this.integrantesFormulario.push(persona);
        this.integrantesFormulario = this.integrantesFormulario.slice(0);
      }
    });
  }

  private mostrarAvisoDeCuadranteInvalido(nombreCuadrante: string): void {
    if (!nombreCuadrante) {
      nombreCuadrante = '';
    }
    this.toastr.warning(`Los datos de la pestaña ${nombreCuadrante.toLowerCase()} no pudieron guardarse ya que faltan datos requeridos`, 'Advertencia', {
      timeOut: 5000,
      tapToDismiss: true,
      extendedTimeOut: 1000,
      positionClass: 'toast-bottom-right'
    });
  }

  private integrantesTienenGrupo(){
    let comando = new ConsultarGrupoFamiliarIntegrantes();
    comando.integrantes = [];
    this.formulario.integrantes.forEach((i) => {
      if (i.idEstado != 4 && i.idEstado != 6) {
        comando.integrantes.push(i);
      }
    });
    this.grupoFamiliarService.consultarGrupoFamiliarIntegrantes(comando).subscribe((res) => {
      this.formulario.integrantesTienenGrupo = res;
      if (!this.formulario.integrantesTienenGrupo) {
        return false;
      }
    });
  }

  private solicitanteTieneGrupo() {
    this.grupoFamiliarService
      .consultarExistenciaGrupo({dni: this.formulario.solicitante.nroDocumento, sexo: this.formulario.solicitante.sexoId, pais: this.formulario.solicitante.codigoPais}).subscribe(
      (resultado) => {
        this.formulario.tieneGrupo = resultado;
      });
  }

  private obtenerNroSuac(idFormularion: number): void {
    this.formulariosService.obtenerNroSuac(idFormularion)
      .subscribe((suac) => this.nroSuac = suac);
  }
}
