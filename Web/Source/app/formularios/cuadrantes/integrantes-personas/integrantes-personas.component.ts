import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { noop } from 'rxjs/util/noop';
import { Persona } from '../../../shared/modelo/persona.model';
import { FormulariosService } from '../../shared/formularios.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { ActualizarDatosDeContactoComando } from '../../shared/modelo/actualizar-datos-contacto-comando.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { GrupoFamiliarService } from '../../shared/grupo-familiar.service';
import { IntegrantesGrillaComponent } from '../integrantes-grilla/integrantes-grilla.component';
import { BusquedaPersonaComponent } from '../persona/persona.component';
import { Integrante } from '../../../shared/modelo/integrante.model';
import { ConsultarGrupoFamiliarIntegrantes } from '../../shared/modelo/consultar-grupo-familiar-integrantes.model';

@Component({
  selector: 'bg-integrantes-personas',
  templateUrl: './integrantes-personas.component.html',
  styleUrls: ['./integrantes-personas.component.scss'],
})

export class IntegrantesPersonasComponent extends CuadranteFormulario implements OnInit {

  public seModificoDatosContacto: boolean = false;
  public solicitante: Persona = new Persona();
  public formDatosContacto: FormGroup;
  private personaEncontrada: boolean = false;
  private banderaRecienInicia: boolean = true;
  public integrante: Integrante = new Integrante();
  public cantMinIntegrantes: number;
  public cantMaxIntegrantes: number;
  public integrantesRecuperados: Integrante[] = [];
  public edicionIntegrante: boolean = false;
  private tieneTelefonosCargados: boolean = false;
  private idFormularioEdicion = 0;

  @ViewChild(IntegrantesGrillaComponent) grilla: IntegrantesGrillaComponent;
  @ViewChild(BusquedaPersonaComponent) busqueda: BusquedaPersonaComponent;

  constructor(private formulariosService: FormulariosService,
              private notificacionService: NotificacionService,
              private fb: FormBuilder,
              private grupoFamiliarService: GrupoFamiliarService) {
    super();
  }

  public ngOnInit(): void {
    if (this.formulario.idEstado === 3) {
      this.editable = false;
    }
    this.cantMaxIntegrantes = this.formulario.detalleLinea.cantidadMaximaIntegrantes;
    this.cantMinIntegrantes = this.formulario.detalleLinea.cantidadMinimaIntegrantes;
    this.solicitante = new Persona();
    this.integrantesRecuperados = [];
    this.grilla.solicitanteFormulario = this.formulario.solicitante;
    if (!this.formulario.integrantes.length && !(this.formulariosService.formulario.idEstado === 6 || this.formulariosService.formulario.idEstado === 4)) {
      this.formulario.integrantes.push(this.mapearIntegrante(this.formulario.solicitante, true));
    }

    this.grilla.agregarIntegrantes(this.formulario.integrantes);
    this.busqueda.crearForm();
    this.crearFormDatosContacto();
    this.banderaRecienInicia = false;
    this.validarGrupoIntegrantes();
  }

  public personaConsultada(persona: Persona) {
    this.tieneTelefonosCargados = false;
    if (this.editable) { // es nuevo
      if (!this.validarSexo(persona.sexoId)) {
        this.notificacionService.informar(Array.of('La persona seleccionada no es del sexo correcto para esta línea'), true);
        return;
      }
      if (!this.edicionIntegrante) {
        this.formulariosService.existeFormularioEnCursoParaPersona(persona)
          .subscribe((existeFormulario) => {
            if (persona.fechaDefuncion) {
              this.notificacionService.informar(Array.of('La persona seleccionada se encuentra registrada como FALLECIDA.'));
            }
            if (!this.existeEnAgrupamiento(persona)) {
              this.formulariosService.existeFormularioEnCursoParaGrupo(persona).subscribe((personas) => {
                if (personas) {
                  if (personas[0] && (personas.length > 1 || (persona.apellido + ", " + persona.nombre) != personas[0])) {
                    let mensajes = personas.map((res) => `${res}`).join('; ');
                    this.notificacionService.informar(['Los siguientes integrantes del grupo poseen un formulario en curso: ', mensajes]);
                  }
                } else {
                  this.notificacionService.informar(['Error al obtener los datos del grupo familiar.']);
                }
                this.formulariosService.miembroGrupoPerteneceAgrupamiento(persona, this.formulario.idAgrupamiento).subscribe((familiares) => {
                  if (familiares.length === 1) {
                    let mensajes = familiares.map((res) => `${res}`).join('. ');
                    this.notificacionService.informar(['En el préstamo ya se encuentra el siguiente miembro del grupo conviviente de la persona seleccionada: ', mensajes]);
                  }
                  if (familiares.length > 1) {
                    let mensajes = familiares.map((res) => `${res}`).join('; ');
                    this.notificacionService.informar(['En el préstamo ya se encuentran los siguientes miembros del grupo conviviente de la persona seleccionada: ', mensajes]);
                  }

                  this.solicitante = persona;
                  this.formulariosService.almacenarPersona(persona);
                  this.personaEncontrada = true;
                  this.obtenerDatosDeContacto();
                });
              });
            } else {
              this.notificacionService.informar(Array.of('La persona seleccionada ya es parte del formulario.'), true);
              return;
            }
            this.formulariosService.existeDeudaHistorica(persona).subscribe((poseeDeuda) => {
              if (poseeDeuda) {
                this.notificacionService.informar(Array.of('La persona seleccionada posee deuda, debería normalizar la situación.'));
              }
            });
            if (existeFormulario) {
              this.notificacionService.informar(Array.of('La persona seleccionada ya es solicitante o garante de un formulario.'));
            }
          });
      } else {
        this.solicitante = persona;
        this.formulariosService.almacenarPersona(persona);
        this.personaEncontrada = true;
        this.obtenerDatosDeContacto();
      }
    }
    this.tieneTelefonosCargados = false;
  }

  private existeEnAgrupamiento(persona: Persona): boolean {
    for (let integrante of this.formulario.integrantes) {
      if (persona.nroDocumento === integrante.nroDocumento && integrante.estado !== 'ELIMINADO') {
        return true;
      }
    }
    return false;
  }

  public actualizarDatos() {
  }

  private registrarFormularios() {
    if (this.editable) {

      this.formulariosService.formulario.integrantes = this.grilla.solicitantes;

      if (this.formulario.id) {
        this.grilla.solicitantes[0].idFormulario = this.formulario.id;
      }

      this.formulariosService.RegistrarFormularios(this.formulariosService.formulario)
        .subscribe((idFormularios) => {
            idFormularios.forEach((idFormularios) => {
              this.formulario.integrantes.filter((i) => {
                if (i.nroDocumento === idFormularios.nroDocumento) {
                  i.idFormulario = idFormularios.idFormulario;
                }
              });
            });
            this.grilla.solicitantes = this.formulario.integrantes;
            if (this.formulario.datosEmprendimiento) {
              if (this.formulario.datosEmprendimiento.id) {
                this.formulariosService.actualizarEmprendimiento(this.formulario.idAgrupamiento, this.formulario.datosEmprendimiento).subscribe((idEmprendimiento) => {
                }, (errores) => {
                  this.notificacionService.informar(errores, true);
                });
              }
            }
            if (this.formulario.datosONG.numeroGrupo) {
              this.formulariosService.registrarOngParaFormularios(this.formulario.idAgrupamiento, this.formulario.datosONG).subscribe();
            }
          },
          (errores) => {
            this.notificacionService.informar(errores, true);
          });
    }
  }

  public esValido(): boolean {
    if (!this.editable) {  // estamos en el ver
      return true;
    }
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
    if (this.grilla.solicitantes) {
      let cantIntegrantes = this.grilla.solicitantes.filter((s) => s.idEstado != 4 && s.idEstado != 6).length;
      return cantIntegrantes >= this.cantMinIntegrantes && cantIntegrantes <= this.cantMaxIntegrantes;
    }
  }

  private actualizarDatosDeContacto(): void {
    let comando = this.armarComandoActualizarDatosDeContacto();
    if (comando) {
      this.formulariosService.actualizarDatosDeContacto(comando).subscribe(
        noop,
        (errores) => {
          this.notificacionService.informar(<string[]>errores, true);
        });
    }
  }

  public modificoGrupo(modifico: boolean) {
    this.validarGrupoIntegrantes();
  }

  private validarGrupoIntegrantes() {
    let comando = new ConsultarGrupoFamiliarIntegrantes();
    comando.integrantes = [];
    this.grilla.solicitantes.forEach((i) => {
      if (i.idEstado !== 4 && i.idEstado !== 6) {
        comando.integrantes.push(i);
      }
    });
    this.grupoFamiliarService.consultarGrupoFamiliarIntegrantes(comando).subscribe((res) => {
      this.formulario.integrantesTienenGrupo = res;
      this.grilla.integrantesTienenGrupo = res;
    });
  }

  public agregarPersona() {
    this.actualizarDatosDeContacto();
    let integrante = this.mapearIntegrante(this.solicitante, false);
    let edicion: boolean;
    if (this.grilla.solicitantes) {
      let cantIntegrantes = this.grilla.solicitantes.filter((s) => s.idEstado != 4 && s.idEstado != 6).length;
      edicion = this.grilla.solicitantes.some(s => s.nroDocumento === integrante.nroDocumento && s.idEstado != 4 && s.idEstado != 6);
      if (cantIntegrantes < this.cantMaxIntegrantes || edicion) {
        this.grilla.agregarIntegrante(integrante);
        this.solicitante = new Persona();
        this.integrante = new Integrante();
        this.crearFormDatosContacto();
        this.busqueda.limpiarForm();
      } else {
        this.notificacionService.informar(Array.of('Se ha excedido del límite máximo de integrantes'), true);
        this.solicitante = new Persona();
        this.crearFormDatosContacto();
        this.busqueda.limpiarForm();
      }
    } else {
      this.grilla.agregarIntegrante(this.integrante);
      this.solicitante = new Persona();
      this.integrante = new Integrante();
      this.crearFormDatosContacto();
      this.busqueda.limpiarForm();
    }
    this.edicionIntegrante = false;
    this.tieneTelefonosCargados = false;
    if (!edicion) {
      this.registrarFormularios();
    }
    this.validarGrupoIntegrantes();
    this.idFormularioEdicion = 0;
    this.busqueda.editable = true;
  }

  private mapearIntegrante(solicitante: Persona, esSolicitante: boolean): Integrante {
    let integrante = new Integrante();
    integrante.telFijo = solicitante.codigoArea + ' ' + solicitante.telefono;
    integrante.telCelular = solicitante.codigoAreaCelular + ' ' + solicitante.celular;
    integrante.pais = solicitante.codigoPais;
    integrante.idNumero = solicitante.idNumero;
    integrante.nroDocumento = solicitante.nroDocumento;
    integrante.sexo = solicitante.sexoId;
    integrante.nombre = solicitante.nombre;
    integrante.apellido = solicitante.apellido;
    integrante.mail = solicitante.email;
    integrante.idAgrupamiento = this.formulario.idAgrupamiento;
    integrante.solicitante = esSolicitante;
    integrante.idFormulario = this.idFormularioEdicion;
    if (esSolicitante) {
      integrante.idFormulario = this.formulario.id;
      integrante.idEstado = this.formulariosService.formulario.idEstado;
    }

    if (!integrante.idEstado) {
      integrante.idEstado = 1;
      integrante.estado = 'BORRADOR';
    }

    if (integrante.idEstado) {
      this.formulariosService.obtenerEstadoFormulario(integrante.idEstado).subscribe(e => integrante.estado = e);
    }
    return integrante;
  }

  private armarComandoActualizarDatosDeContacto(): ActualizarDatosDeContactoComando {
    if (!this.banderaRecienInicia) {
      let comando = new ActualizarDatosDeContactoComando();
      comando.idSexo = this.solicitante.sexoId;
      comando.nroDocumento = this.solicitante.nroDocumento;
      comando.codigoPais = this.solicitante.codigoPais;
      comando.idNumero = this.solicitante.idNumero.toString();
      comando.codAreaTelefono = this.formDatosContacto.get('codigoArea').value;
      comando.nroTelefono = this.formDatosContacto.get('telefono').value;
      comando.codAreaCelular = this.formDatosContacto.get('codigoAreaCelular').value;
      comando.nroCelular = this.formDatosContacto.get('celular').value;
      comando.mail = this.formDatosContacto.get('email').value;

      return comando;
    }
    return null;
  }

  private crearFormDatosContacto(): void {
    const codigoArea = new FormControl(this.solicitante.codigoArea);
    const telefono = new FormControl(this.solicitante.telefono);
    const codigoAreaCelular = new FormControl(this.solicitante.codigoAreaCelular);
    const celular = new FormControl(this.solicitante.celular);

    const validatorCodigoArea = Validators.compose([
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(5),
      CustomValidators.number,
      CustomValidators.codArea,
    ]);
    const validatorTelefono = Validators.compose([
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(9),
      CustomValidators.number,
      CustomValidators.nroTelefono,
    ]);

    codigoArea.setValidators(validatorCodigoArea);
    telefono.setValidators(validatorTelefono);
    codigoAreaCelular.setValidators(validatorCodigoArea);
    celular.setValidators(validatorTelefono);

    codigoArea.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.codigoArea = value;
        IntegrantesPersonasComponent.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    telefono.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.telefono = value;
        IntegrantesPersonasComponent.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);

      });

    codigoAreaCelular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.codigoAreaCelular = value;
        IntegrantesPersonasComponent.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    celular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.celular = value;
        IntegrantesPersonasComponent.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    const email = new FormControl(
      this.solicitante.email,
      Validators.compose(
        [
          Validators.required,
          Validators.minLength(7),
          CustomValidators.email,
        ]));
    email.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.email = value;
      });

    this.formDatosContacto = this.fb.group({
      codigoArea: codigoArea,
      telefono: telefono,
      codigoAreaCelular: codigoAreaCelular,
      celular: celular,
      email: email,
    });

    if (!this.editable) {
      this.formDatosContacto.disable();
    }

    this.formDatosContacto.valueChanges
      .subscribe(() => {
        this.habilitarActualizacionDatos();
      });

    this.formDatosContacto.get('telefono').setValue(this.formDatosContacto.get('telefono').value);
    this.formDatosContacto.get('celular').setValue(this.formDatosContacto.get('celular').value);
  }

  private static habilitarValidadores(value: any, codigo: FormControl, numero: FormControl, codigoValidator: ValidatorFn, numeroValidator: ValidatorFn, codigoAlternativo: FormControl, numeroAlternativo: FormControl): void {
    codigoAlternativo.setValidators(codigoValidator);
    numeroAlternativo.setValidators(numeroValidator);
    codigo.setValidators(codigoValidator);
    numero.setValidators(numeroValidator);

    if (value || codigo.value || numero.value) {
      codigoAlternativo.clearValidators();
      numeroAlternativo.clearValidators();
    } else if (!(codigo.value || numero.value) && (codigoAlternativo.value || numeroAlternativo.value)) {
      codigo.clearValidators();
      numero.clearValidators();
    }

    codigo.updateValueAndValidity();
    numero.updateValueAndValidity();
    codigoAlternativo.updateValueAndValidity();
    numeroAlternativo.updateValueAndValidity();
  }

  private obtenerDatosDeContacto(): void {
    this.formulariosService.obtenerDatosDeContacto(this.solicitante).subscribe(
      (resultado) => {
        if (resultado) {
          this.crearFormDatosContacto();

          this.solicitante.codigoArea = resultado.codigoArea;
          this.solicitante.telefono = resultado.telefono;
          this.solicitante.codigoAreaCelular = resultado.codigoAreaCelular;
          this.solicitante.celular = resultado.celular;

          if ((this.solicitante.codigoArea && this.solicitante.telefono) || (this.solicitante.codigoAreaCelular && this.solicitante.celular)) {
            this.tieneTelefonosCargados = true;
          }
          if (resultado.mail) {
            this.solicitante.email = resultado.mail;
          }
          this.crearFormDatosContacto();
        }
      },
      (errores) => {
        this.notificacionService.informar(<string[]>errores, true);
      });
  }

  private habilitarActualizacionDatos(): void {
    this.seModificoDatosContacto = true;
  }

  inicializarDeNuevo(): boolean {
    return false;
  }

  private validarSexo(sexoId: string): boolean {
    let sexoLinea = this.formulario.detalleLinea.sexoDestinatarioId;
    switch (sexoLinea) {
      case '1':
        if (sexoId.indexOf('01') < 0) {
          return false;
        }
        break;
      case '2':
        if (sexoId.indexOf('02') < 0) {
          return false;
        }
        break;
    }
    return true;
  }

  private cambioApoderado(cambio: boolean) {
    if (cambio) {
      this.registrarFormularios();
    }
  }

  public editarIntegrante(integrante: Integrante) {
    this.busqueda.sexoId = integrante.sexo;
    this.busqueda.nroDocumento = integrante.nroDocumento;
    this.busqueda.codigoPais = integrante.pais;
    this.edicionIntegrante = true;
    this.tieneTelefonosCargados = false;
    this.busqueda.crearForm();
    this.idFormularioEdicion = integrante.idFormulario;
    this.busqueda.editable = false;
  }
}
