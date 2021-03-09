import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { noop } from 'rxjs/util/noop';
import { Persona } from '../../../shared/modelo/persona.model';
import { FormulariosService } from '../../shared/formularios.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { CustomValidators, isEmpty } from '../../../shared/forms/custom-validators';
import { ActualizarDatosDeContactoComando } from '../../shared/modelo/actualizar-datos-contacto-comando.model';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { ControlMessagesComponent } from '../../../shared/forms/control-messages.component';
import { GrupoFamiliarService } from '../../shared/grupo-familiar.service';

@Component({
  selector: 'bg-garante',
  templateUrl: './garante.html',
  styleUrls: ['./garante.component.scss'],
})

export class GaranteComponent extends CuadranteFormulario implements OnInit {
  public garante: Persona;
  public formDatosContacto: FormGroup;
  public seModificoDatosContacto: boolean = false;
  private banderaRecienInicia: boolean = true;
  public esGaranteSolicitante: boolean = false;
  public existeFormulario: boolean;
  private habiaGarante: boolean = false;

  constructor(private formulariosService: FormulariosService,
              private notificacionService: NotificacionService,
              private fb: FormBuilder,
              private formularioService: FormulariosService,
              private grupoFamiliarService: GrupoFamiliarService) {
    super();
  }

  public ngOnInit(): void {
    this.garante = this.formulario.garantes[0] || new Persona();
    this.habiaGarante = !isEmpty(this.garante.nroDocumento);
    this.validarTipoDeGarante();
    this.crearFormDatosContacto();
    this.banderaRecienInicia = false;
  }

  public actualizarDatos() {
    let garantes = this.obtenerGarantes();
    this.formularioService.actualizarGarantes(this.formulario.id, garantes)
      .subscribe(() => {
        this.formulario.garantes = garantes;
      });
  }

  private obtenerGarantes(): Persona[] {
    if (this.editable) {
      if (this.formulario.garantes) {
        this.formulario.garantes[0] = this.garante;
        //    this.formulario.garantes.forEach((garante) => this.formulariosService.almacenarPersona(garante));
        if (!this.banderaRecienInicia && this.seModificoDatosContacto) {
          this.actualizarDatosDeContacto();
          this.seModificoDatosContacto = false;
        }
        return this.formulario.garantes;
      }
    }
  }

  public esValido(): boolean {
    if (!this.editable) {  // estamos en el ver
      return true;
    } else if (this.garante.nroDocumento) {
      ControlMessagesComponent.validarFormulario(this.formDatosContacto);
      return this.formDatosContacto.valid;
    }
  }

  public personaConsultada(persona: Persona) {
    if (persona.fechaDefuncion) {
      this.notificacionService.informar(Array.of('La persona seleccionada se encuentra registrada como FALLECIDA'));
    }
    this.validarTipoDeGarante(persona);
  }

  private setearGarante(persona: Persona) {
    this.garante = persona;
    this.formulariosService.almacenarPersona(persona);
    this.obtenerDatosDeContacto();

    this.grupoFamiliarService
      .consultarExistenciaGrupo({
        dni: this.garante.nroDocumento,
        sexo: this.garante.sexoId,
        pais: this.garante.codigoPais
      }).subscribe(
      (resultado) => {
        this.formulario.tieneGrupoGarante = resultado;
      });
  }

  private crearFormDatosContacto(): void {
    const codigoArea = new FormControl(this.garante.codigoArea);
    const telefono = new FormControl(this.garante.telefono);
    const codigoAreaCelular = new FormControl(this.garante.codigoAreaCelular);
    const celular = new FormControl(this.garante.celular);

    const validatorCodigoArea = Validators.compose([
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(5),
      CustomValidators.number,
      CustomValidators.codArea
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
        this.garante.codigoArea = value;
        this.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    telefono.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.garante.telefono = value;
        this.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    codigoAreaCelular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.garante.codigoAreaCelular = value;
        this.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    celular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.garante.celular = value;
        this.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    const email = new FormControl(
      this.garante.email,
      Validators.compose(
        [
          Validators.required,
          Validators.minLength(7),
          CustomValidators.email,
        ]));
    email.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.garante.email = value;
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
  }

  private habilitarValidadores(value: any, codigo: FormControl, numero: FormControl, codigoValidator: ValidatorFn, numeroValidator: ValidatorFn, codigoAlternativo: FormControl, numeroAlternativo: FormControl): void {
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

  private habilitarActualizacionDatos(): void {
    this.seModificoDatosContacto = true;
  }

  private obtenerDatosDeContacto(): void {
    this.formulariosService.obtenerDatosDeContacto(this.garante).subscribe(
      (resultado) => {
        if (resultado) {
          this.garante.codigoArea = resultado.codigoArea;
          this.garante.telefono = resultado.telefono;
          this.garante.codigoAreaCelular = resultado.codigoAreaCelular;
          this.garante.celular = resultado.celular;
          if (resultado.mail) {
            this.garante.email = resultado.mail;
          }
          this.crearFormDatosContacto();
        }
      },
      (errores) => {
        this.notificacionService.informar(<string[]>errores, true);
      });
  }

  private actualizarDatosDeContacto(): void {
    if (this.formDatosContacto.valid) {
      let comando = this.armarComandoActualizarDatosDeContacto();
      if (comando) {
        this.formulariosService.actualizarDatosDeContacto(comando).subscribe(
          noop,
          (errores) => {
            this.notificacionService.informar(<string[]>errores, true);
          });
      }
    }
  }

  private armarComandoActualizarDatosDeContacto(): ActualizarDatosDeContactoComando {
    if (!this.banderaRecienInicia) {
      let comando = new ActualizarDatosDeContactoComando();
      comando.idSexo = this.garante.sexoId;
      comando.nroDocumento = this.garante.nroDocumento;
      comando.codigoPais = this.garante.codigoPais;
      comando.idNumero = this.garante.idNumero.toString();

      comando.codAreaTelefono = this.formDatosContacto.get('codigoArea').value;
      comando.nroTelefono = this.formDatosContacto.get('telefono').value;
      comando.codAreaCelular = this.formDatosContacto.get('codigoAreaCelular').value;
      comando.nroCelular = this.formDatosContacto.get('celular').value;
      comando.mail = this.formDatosContacto.get('email').value;
      return comando;
    }
    return null;
  }

  private validarTipoDeGarante(persona?: Persona): void {
    switch (this.formulario.detalleLinea.tipoGarantiaId) {
      case 1: // SOLICITANTE
        this.garanteSolicitante();
        break;

      case 2: // TERCEROS
        this.garanteTercero(persona);
        break;

      case 3: // SOCIO INTEGRANTE
        this.garanteSocioIntegrante();
        break;

      case 4: // SOLICITANTE Y TERCERO
        this.garanteSolicitanteTercero(persona);
        break;

      default:
        throw new Error('No existe el tipo de garantia');
    }
  }

  private garanteSolicitante(): boolean {
    this.garante = this.formulario.garantes[0] = this.formulario.solicitante;
    this.crearFormDatosContacto();
    this.banderaRecienInicia = false;
    this.esGaranteSolicitante = true;

    this.grupoFamiliarService
      .consultarExistenciaGrupo({
        dni: this.garante.nroDocumento,
        sexo: this.garante.sexoId,
        pais: this.garante.codigoPais
      }).subscribe(
      (resultado) => {
        this.formulario.tieneGrupoGarante = resultado;
      });
    return true;
  }

  private garanteTercero(persona: Persona): void {
    if (persona) {
      let dniSolicitante = this.formulario.solicitante.nroDocumento;
      let sexoSolicitante = this.formulario.solicitante.sexoId;
      let paisSolicitante = this.formulario.solicitante.codigoPais;

      let dniGarante = persona.nroDocumento;
      let sexoGarante = persona.sexoId;
      let paisGarante = persona.codigoPais;

      if (this.formulario.garantes[0]) {
        if (dniGarante === this.formulario.garantes[0].nroDocumento
          && sexoGarante === this.formulario.garantes[0].sexoId
          && paisGarante === this.formulario.garantes[0].codigoPais) {
          this.setearGarante(persona);
          return; // si es el mismo que ya esta cargado, no valido nada
        }
      }

      if (!(dniGarante === dniSolicitante
        && sexoGarante === sexoSolicitante
        && paisGarante === paisSolicitante)) {
        this.formulariosService.existeFormularioEnCursoParaPersona(persona)
          .subscribe((existeFormulario) => {
              this.existeFormulario = !!existeFormulario;
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            },
            () => {
              this.formulariosService.existeDeudaHistorica(persona).subscribe((poseeDeuda) => {
                if (poseeDeuda) {
                  this.notificacionService.informar(Array.of('La persona seleccionada posee deuda, debería normalizar la situación.'));
                }
                if (this.existeFormulario) {
                  this.notificacionService.informar(Array.of('La persona seleccionada ya es solicitante o garante de un formulario.'));
                }
                this.formulariosService.existeFormularioEnCursoParaGrupo(persona).subscribe((personas) => {
                  if (personas) {
                    if (personas[0] && (personas.length > 1 || (persona.apellido + ", " + persona.nombre) != personas[0])) {
                      let mensajes = personas.map((res) => `${res}`).join('; ');
                      this.notificacionService.informar(['Los siguientes integrantes del grupo poseen un formulario en curso: ', mensajes]);
                    }
                  } else {
                    this.notificacionService.informar(['Error al obtener los datos del grupo familiar.']);
                  }
                });
              });
            });
      }
      this.setearGarante(persona);
    }
    return;
  }

  private garanteSocioIntegrante(): boolean {
    return true;
  }

  private garanteSolicitanteTercero(persona: Persona): void {
    if (persona) {
      let dniSolicitante = this.formulario.solicitante.nroDocumento;
      let sexoSolicitante = this.formulario.solicitante.sexoId;
      let paisSolicitante = this.formulario.solicitante.codigoPais;

      let dniGarante = persona.nroDocumento;
      let sexoGarante = persona.sexoId;
      let paisGarante = persona.codigoPais;

      if (dniGarante === dniSolicitante
        && sexoGarante === sexoSolicitante
        && paisGarante === paisSolicitante) {
        this.setearGarante(persona);
        return; // si el garante es el solicitante no valido que haya uno ya en curso
      }

      if (this.formulario.garantes[0]) {
        if (dniGarante === this.formulario.garantes[0].nroDocumento
          && sexoGarante === this.formulario.garantes[0].sexoId
          && paisGarante === this.formulario.garantes[0].codigoPais) {
          this.setearGarante(persona);
          return; // si es el mismo que ya esta cargado, no valido nada
        }
      }

      this.formulariosService.existeFormularioEnCursoParaPersona(persona)
        .subscribe((existeFormulario) => {
            this.existeFormulario = !!existeFormulario;
          }, (errores) => {
            this.notificacionService.informar(errores, true);
          },
          () => {
            this.formulariosService.existeDeudaHistorica(persona).subscribe((poseeDeuda) => {
              if (poseeDeuda) {
                this.notificacionService.informar(Array.of('La persona seleccionada posee deuda, debería normalizar la situación.'));
              }
              if (this.existeFormulario) {
                this.notificacionService.informar(Array.of('La persona seleccionada ya es solicitante o garante de un formulario.'));
              }
              this.formulariosService.existeFormularioEnCursoParaGrupo(persona).subscribe((personas) => {
                if (personas) {
                  if (personas[0] && (personas.length > 1 || (persona.apellido + ", " + persona.nombre) != personas[0])) {
                    let mensajes = personas.map((res) => `${res}`).join('; ');
                    this.notificacionService.informar(['Los siguientes integrantes del grupo poseen un formulario en curso: ', mensajes]);
                  }
                } else {
                  this.notificacionService.informar(['Error al obtener los datos del grupo familiar.']);
                }
                this.setearGarante(persona);
              });
            });
          });

    }
    return;
  }

  inicializarDeNuevo(): boolean {
    return false;
  }

  public checkSolicitanteGarante(event): void {
    if (!this.editable) {
      return;
    }
    if (!this.esGaranteSolicitante && event.target.checked) {
      this.garanteSolicitante();
    }
    if (this.esGaranteSolicitante && !event.target.checked) {
      this.limpiarGarante();
      this.esGaranteSolicitante = false;
    }
  }

  private limpiarGarante(): void {
    this.garante = new Persona();
    this.formulario.garantes = [];
    this.crearFormDatosContacto();
  }

  public visualizarCheckSolicitanteEsGarante(): boolean {
    return this.editable && !this.habiaGarante;
  }

  public visualizarComponentePersona(): boolean {
    if (this.formulario.detalleLinea.tipoGarantiaId != 2) {
      return true;
    }
    if (this.habiaGarante) {
      return true;
    }
    if (!this.esGaranteSolicitante) {
      return true;
    }
    return !this.garante.nroDocumento;
  }
}
