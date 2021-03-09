import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { noop } from 'rxjs/util/noop';
import { Persona } from '../../../shared/modelo/persona.model';
import { FormulariosService } from '../../shared/formularios.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { ActualizarDatosDeContactoComando } from '../../shared/modelo/actualizar-datos-contacto-comando.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { ControlMessagesComponent } from '../../../shared/forms/control-messages.component';
import { GrupoFamiliarService } from '../../shared/grupo-familiar.service';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'bg-solicitante',
  templateUrl: './solicitante.component.html',
  styleUrls: ['./solicitante.component.scss'],
})

export class SolicitanteComponent extends CuadranteFormulario implements OnInit {

  public seModificoDatosContacto: boolean = false;
  public solicitante: Persona;
  public formDatosContacto: FormGroup;
  public formFechaFormularioFisico: FormGroup;
  private personaEncontrada: boolean = false;
  private banderaRecienInicia: boolean = true;

  constructor(private formulariosService: FormulariosService,
              private notificacionService: NotificacionService,
              private fb: FormBuilder,
              private grupoFamiliarService: GrupoFamiliarService) {
    super();
  }

  public ngOnInit(): void {
    this.solicitante = this.formulario.solicitante || new Persona();
    this.validarGrupoGarante(); //si ya fue cargado el garante y es una edición
    this.crearFormFechaFormulario();
    this.crearFormDatosContacto();
    this.banderaRecienInicia = false;
  }

  public personaConsultada(persona: Persona) {
    if (this.editable && !this.formulario.id) { // es nuevo
      if (!this.validarSexo(persona.sexoId)) {
        this.notificacionService.informar(Array.of('La persona seleccionada no es del sexo correcto para esta línea'), true);
        return;
      }
      this.formulariosService.existeFormularioEnCursoParaPersona(persona)
        .subscribe((existeFormulario) => {
          if (persona.fechaDefuncion) {
            this.notificacionService.informar(Array.of('La persona seleccionada se encuentra registrada como FALLECIDA'));
          }
          if (existeFormulario) {
            this.notificacionService.informar(Array.of('La persona seleccionada ya es solicitante o garante de un formulario.'));
          }
          this.formulariosService.existeDeudaHistorica(persona).subscribe((poseeDeuda) => {
            if (poseeDeuda) {
              this.notificacionService.informar(Array.of('La persona seleccionada posee deuda, debería normalizar la situación.'));
            }
          });
          this.formulariosService.existeFormularioEnCursoParaGrupo(persona).subscribe((personas) => {
            if (personas) {
              if (personas[0] && (personas.length > 1 || (persona.apellido + ", " + persona.nombre) != personas[0])) {
                let mensajes = personas.map((res) => `${res}`).join('; ');
                this.notificacionService.informar(['Los siguientes integrantes del grupo poseen un formulario en curso: ', mensajes]);
              }
            } else {
              this.notificacionService.informar(['Error al obtener los datos del grupo familiar.']);
            }
            this.solicitante = persona;
            this.formulario.solicitante = persona;
            this.formulariosService.almacenarPersona(persona);
            this.personaEncontrada = true;
            if (!this.formulario.fechaForm) {
              this.formFechaFormularioFisico.patchValue({fechaFisica: NgbUtils.obtenerNgbDateStruct(new Date(Date.now()))});
            }
            this.obtenerDatosDeContacto();
          });
        });
    } else {
      this.solicitante = persona;
      this.formulario.solicitante = persona;
      this.formulariosService.almacenarPersona(persona);
      this.personaEncontrada = true;
      this.obtenerDatosDeContacto();
    }
  }

  public async actualizarDatos() {
    if (this.editable) {
      this.actualizarDatosDeContacto();
      this.editable = false;
      this.seModificoDatosContacto = false;
    } else if (this.seModificoDatosContacto && this.esValido()) {
      this.actualizarDatosDeContacto();
      this.seModificoDatosContacto = false;
    }
    this.formulario.solicitante = this.solicitante;
    if (!this.formFechaFormularioFisico.get('fechaFisica').value) {
      this.formFechaFormularioFisico.patchValue({fechaFisica: NgbUtils.obtenerNgbDateStruct(new Date(Date.now()))});
    }
    this.formulario.fechaForm = NgbUtils.obtenerDate(this.formFechaFormularioFisico.value.fechaFisica);
    if (!this.formulario.id) {
      await this.formulariosService.RegistrarFormulario(this.formulario)
        .subscribe(ids => {
          this.formulario.id = ids.idFormulario;
          this.formulario.idAgrupamiento = ids.idAgrupamiento;
          if (!isNullOrUndefined(this.formulario.datosONG.idOng) && this.formulario.datosONG.idOng != -1) {
            this.formulariosService.obtenerNumeroGrupo(this.formulario.datosONG).subscribe((numeroGrupo) => {
                this.formulario.datosONG.numeroGrupo = numeroGrupo;
                this.formulariosService.registrarOngParaFormularios(this.formulario.idAgrupamiento, this.formulario.datosONG).subscribe();
              }
            );
          }
        });
    }
  }

  public esValido(): boolean {
    if (!this.editable) {
      return true;
    } else {
      this.grupoFamiliarService
        .consultarExistenciaGrupo({
          dni: this.solicitante.nroDocumento,
          sexo: this.solicitante.sexoId,
          pais: this.solicitante.codigoPais
        }).subscribe(
        (resultado) => {
          if (resultado === false) {
            this.formulario.tieneGrupo = false;
          }
        });
      this.formulario.tieneGrupo = true;

      if (!this.personaEncontrada && !this.banderaRecienInicia) {
        this.notificacionService.informar(Array.of('Debe seleccionar una persona'), false);
      }
      ControlMessagesComponent.validarFormulario(this.formDatosContacto);
      return this.personaEncontrada && this.formDatosContacto.valid && this.formFechaFormularioFisico.valid;
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

  private validarGrupoGarante(): void {
    let garante = this.formulario.garantes[0];
    if (garante) {
      this.grupoFamiliarService
        .consultarExistenciaGrupo({
          dni: garante.nroDocumento,
          sexo: garante.sexoId,
          pais: garante.codigoPais
        }).subscribe(
        (resultado) => {
          this.formulario.tieneGrupoGarante = resultado;
        });
    }
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
        this.habilitarValidadores(codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    telefono.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.telefono = value;
        this.habilitarValidadores(codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    codigoAreaCelular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.codigoAreaCelular = value;
        this.habilitarValidadores(codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    celular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.solicitante.celular = value;
        this.habilitarValidadores(codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
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
      this.formFechaFormularioFisico.disable();
    }

    this.formDatosContacto.valueChanges
      .subscribe(() => {
        this.habilitarActualizacionDatos();
      });
  }

  private habilitarValidadores(codigo: FormControl, numero: FormControl, codigoValidator: ValidatorFn, numeroValidator: ValidatorFn, codigoAlternativo: FormControl, numeroAlternativo: FormControl): void {
    codigoAlternativo.setValidators(codigoValidator);
    numeroAlternativo.setValidators(numeroValidator);
    codigo.setValidators(codigoValidator);
    numero.setValidators(numeroValidator);

    if ((codigo.value && numero.value) && (!codigoAlternativo.value && !numeroAlternativo.value)) {
      codigoAlternativo.clearValidators();
      numeroAlternativo.clearValidators();
    } else if ((!codigo.value && !numero.value) && (codigoAlternativo.value && numeroAlternativo.value)) {
      codigo.clearValidators();
      numero.clearValidators();
    }
    if (numeroAlternativo.value) {
      codigoAlternativo.setValidators(codigoValidator);
    }
    if (codigoAlternativo.value) {
      numeroAlternativo.setValidators(numeroValidator);
    }
    if (numero.value) {
      codigo.setValidators(codigoValidator);
    }
    if (codigo.value) {
      numero.setValidators(numeroValidator);
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
          this.solicitante.codigoArea = resultado.codigoArea;
          this.solicitante.telefono = resultado.telefono;
          this.solicitante.codigoAreaCelular = resultado.codigoAreaCelular;
          this.solicitante.celular = resultado.celular;
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

  private crearFormFechaFormulario() {
    this.formFechaFormularioFisico = this.fb.group({
      fechaFisica: new FormControl(NgbUtils.obtenerNgbDateStruct(this.formulario.fechaForm), Validators.compose([Validators.required, CustomValidators.maxDate(new Date())]))
    });
  }
}
