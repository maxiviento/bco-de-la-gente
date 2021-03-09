import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Params, Router} from '@angular/router';
import {FormulariosService} from '../shared/formularios.service';
import {PersonaService} from '../cuadrantes/persona/persona.service';
import {Persona} from '../../shared/modelo/persona.model';
import {Integrante} from '../../shared/modelo/integrante.model';
import {ModalModificarGrupoIntegranteComponent} from '../modal-modificar-grupo-integrante/modal-modificar-grupo-integrante.component';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {Formulario} from '../shared/modelo/formulario.model';
import {NotificacionService} from '../../shared/notificacion.service';
import {GrupoFamiliarService} from '../shared/grupo-familiar.service';
import {CambiarGaranteComando} from '../shared/modelo/cambiar-garante-comando.model';
import {CustomValidators} from '../../shared/forms/custom-validators';
import {FormBuilder, FormControl, FormGroup, ValidatorFn, Validators} from '@angular/forms';
import {noop} from 'rxjs/util/noop';
import {ActualizarDatosDeContactoComando} from '../shared/modelo/actualizar-datos-contacto-comando.model';
import {Title} from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-actualizar-datos',
  templateUrl: './actualizar-datos.component.html',
  styleUrls: ['./actualizar-datos.component.scss'],
})

export class ActualizarDatosComponent implements OnInit {
  public solicitante: Persona = new Persona();
  public garante: Persona = new Persona();
  public persona: Persona = new Persona();
  public cambiaGarante: boolean = false;
  public idFormulario: number;
  public procesoFinalizado: boolean = false;
  public modificarDatos: boolean = false;
  public formulario: Formulario = new Formulario();
  public formDatosContacto: FormGroup;
  public formCheck: FormGroup;
  public ver: boolean = false;
  public titulo: string = 'Actualizar Datos ' + TituloBanco.TITULO;

  constructor(private route: ActivatedRoute,
              private formulariosService: FormulariosService,
              private personaService: PersonaService,
              private modalService: NgbModal,
              private notificacionService: NotificacionService,
              private grupoFamiliarService: GrupoFamiliarService,
              private router: Router,
              private fb: FormBuilder,
              public titleService: Title) {
    this.titleService.setTitle(this.titulo);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.crearFormDatosContacto();
    this.route.params
      .switchMap((params: Params) =>
        this.formulariosService.consultarFormulario(+params['id']))
      .subscribe(
        (formularioCargado) => {
          this.formulario = formularioCargado;
          this.idFormulario = this.formulario.id;
          let personaSolicitante: any = {
            nroDocumento: formularioCargado.solicitante.nroDocumento,
            codigoPais: formularioCargado.solicitante.codigoPais,
            sexoId: formularioCargado.solicitante.sexoId,
          };
          this.personaService.consultarPersona(personaSolicitante).subscribe(
            (resultado) => {
              if (resultado) {
                this.solicitante = resultado;
              }
            });
          let personaGarante: any = {
            nroDocumento: formularioCargado.garantes[0].nroDocumento,
            codigoPais: formularioCargado.garantes[0].codigoPais,
            sexoId: formularioCargado.garantes[0].sexoId,
          };
          this.personaService.consultarPersona(personaGarante).subscribe(
            (resultado) => {
              if (resultado) {
                this.garante = resultado;
                this.garante.idGarante = this.formulario.garantes[0].idGarante;
              }
            });
        });
  }

  private crearForm(): void {
    this.formCheck = this.fb.group({
      ck_solicitanteGarante: new FormControl(false)
    });
  }

  public cambiarGarante() {
    this.persona = new Persona();
    this.cambiaGarante = true;
    this.ver = false;
    this.formDatosContacto.enable();
    this.formDatosContacto.reset();
  }

  public verDatosPersona(persona: Persona) {
    this.cambiaGarante = false;
    this.modificarDatos = false;
    this.persona = persona;
    this.ver = true;
    this.obtenerDatosDeContacto();
    this.formDatosContacto.disable();
  }

  public registrarNuevoGarante() {
    let comando = new CambiarGaranteComando(this.persona.sexoId, this.persona.codigoPais, this.persona.nroDocumento, this.garante.idGarante);
    this.notificacionService.confirmar('¿Está seguro que desea cambiar el garante en el formulario?')
      .result.then((value) => {
        if (value) {
          this.formulariosService.cambiarGaranteFormulario(comando).subscribe((res) => {
            if (res) {
              this.notificacionService.informar(Array.of('El cambio de garante fue registrado con éxito.'));
            } else {
              this.notificacionService.informar(Array.of('Ha ocurrido un error al intentar registrar el cambio de garante.'), true);
            }
          });
          this.actualizarDatosDeContacto();
          this.garante = this.persona;
          this.persona = new Persona();
          this.procesoFinalizado = true;
          this.cambiaGarante = false;
          this.formDatosContacto.reset();
        }
      }
    );
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

  private armarComandoActualizarDatosDeContacto(): ActualizarDatosDeContactoComando {
    let comando = new ActualizarDatosDeContactoComando();
    comando.idSexo = this.persona.sexoId;
    comando.nroDocumento = this.persona.nroDocumento;
    comando.codigoPais = this.persona.codigoPais;
    comando.idNumero = this.persona.idNumero.toString();
    comando.codAreaTelefono = this.formDatosContacto.get('codigoArea').value;
    comando.nroTelefono = this.formDatosContacto.get('telefono').value;
    comando.codAreaCelular = this.formDatosContacto.get('codigoAreaCelular').value;
    comando.nroCelular = this.formDatosContacto.get('celular').value;
    comando.mail = this.formDatosContacto.get('email').value;

    return comando;
  }

  public modalModificarGrupoFamiliar(persona: Persona) {
    this.cambiaGarante = false;
    this.modificarDatos = false;
    let integrante = new Integrante();
    integrante.nroDocumento = persona.nroDocumento;
    integrante.sexo = persona.sexoId;
    integrante.pais = persona.codigoPais;

    const modalRef = this.modalService.open(ModalModificarGrupoIntegranteComponent, {
      backdrop: 'static',
      windowClass: 'modal-xl'
    });
    modalRef.componentInstance.integrante = integrante;
    modalRef.result.then((res) => {
    });
  }

  public personaConsultada(persona: Persona, event: any) {
    if (persona.nroDocumento === this.garante.nroDocumento && persona.sexoId === this.garante.sexoId) {
      this.notificacionService.informar(Array.of('La persona consultada es la que se encuentra como garante del formulario actualmente.'), true)
        .result.then(() => {
        if (event.target.checked) {
          this.formCheck.get('ck_solicitanteGarante').setValue(false);
        }
      });
      return;
    }
    if (!this.validarSexo(persona.sexoId)) {
      this.notificacionService.informar(Array.of('La persona seleccionada no es del sexo correcto para esta línea'), true);
      return;
    }
    this.formulariosService.existeFormularioEnCursoParaPersona(persona)
      .subscribe((existeFormulario) => {
        if (!(persona.nroDocumento === this.solicitante.nroDocumento && persona.sexoId === this.solicitante.sexoId && persona.codigoPais === this.solicitante.codigoPais)) {
          if (existeFormulario) {
            this.notificacionService.informar(Array.of('La persona seleccionada ya es solicitante o garante de un formulario.'));
            return;
          }
        }
        this.formulariosService.existeDeudaHistorica(persona).subscribe((poseeDeuda) => {
          if (poseeDeuda) {
            this.notificacionService.informar(Array.of('La persona seleccionada posee deuda, debería normalizar la situación.'));
            return;
          }
        });
        this.grupoFamiliarService
          .consultarExistenciaGrupo({
            dni: persona.nroDocumento,
            sexo: persona.sexoId,
            pais: persona.codigoPais
          }).subscribe(
          (resultado) => {
            if (resultado === false) {
              this.notificacionService.informar(Array.of('La persona seleccionada no posee grupo, debería normalizar la situación.'));
              return;
            }
          });
        this.formulariosService.existeFormularioEnCursoParaGrupo(persona).subscribe((personas) => {
          if (existeFormulario && personas.length > 1) {
            let mensajes = personas.map((res) => `${res}`).join(', ');
            this.notificacionService.informar(['Los siguientes integrantes del grupo poseen un formulario en curso: ', mensajes]);
          }
        });
        this.persona = persona;
        this.obtenerDatosDeContacto();
      });
  }

  private crearFormDatosContacto(): void {
    const codigoArea = new FormControl(this.persona.codigoArea);
    const telefono = new FormControl(this.persona.telefono);
    const codigoAreaCelular = new FormControl(this.persona.codigoAreaCelular);
    const celular = new FormControl(this.persona.celular);

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

    codigoArea.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.codigoArea = value;
        this.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    telefono.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.telefono = value;
        this.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    codigoAreaCelular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.codigoAreaCelular = value;
        this.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    celular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.celular = value;
        this.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    const email = new FormControl(
      this.persona.email,
      Validators.compose(
        [
          Validators.required,
          Validators.minLength(7),
          CustomValidators.email
        ]));
    email.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.email = value;
      });

    this.formDatosContacto = this.fb.group({
      codigoArea: codigoArea,
      telefono: telefono,
      codigoAreaCelular: codigoAreaCelular,
      celular: celular,
      email: email,
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

  private obtenerDatosDeContacto(): void {
    this.formulariosService.obtenerDatosDeContacto(this.persona).subscribe(
      (resultado) => {
        if (resultado) {
          this.crearFormDatosContacto();
          this.persona.codigoArea = resultado.codigoArea;
          this.persona.telefono = resultado.telefono;
          this.persona.codigoAreaCelular = resultado.codigoAreaCelular;
          this.persona.celular = resultado.celular;
          if (resultado.mail) {
            this.persona.email = resultado.mail;
          }
        }
        this.crearFormDatosContacto();
      },
      (errores) => {
        this.notificacionService.informar(<string[]>errores, true);
      });
  }

  public checkSolicitanteGarante(event) {
    if (event.target.checked) {
      this.personaConsultada(this.solicitante, event);
    } else {
      this.persona = new Persona();
    }
  }

  public modificarCondicionesSolicitadas(persona: Persona) {
    this.modificarDatos = true;
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

  public volver() {
    this.router.navigate(['formularios']);
  }
}
