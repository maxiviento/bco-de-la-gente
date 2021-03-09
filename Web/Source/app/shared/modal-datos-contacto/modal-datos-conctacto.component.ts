import { Component, Input, OnInit } from '@angular/core';
import { MotivosBajaService } from '../servicios/motivosbaja.service';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificacionService } from '../notificacion.service';
import { CustomValidators } from '../forms/custom-validators';
import { Persona } from '../modelo/persona.model';
import { ActualizarDatosDeContactoComando } from '../../formularios/shared/modelo/actualizar-datos-contacto-comando.model';
import { ContactoService } from '../servicios/datos-contacto.service';

@Component({
  selector: 'bg-modal-datos-contacto',
  templateUrl: './modal-datos-contacto.component.html',
  styleUrls: ['./modal-datos-contacto.component.scss'],
})

export class ModalDatosConctactoComponent implements OnInit {
  public formDatosContacto: FormGroup;

  public seModificoDatosContacto: boolean = false;

  @Input()
  public editable: boolean;
  @Input()
  public persona: Persona = new Persona();

  constructor(private fb: FormBuilder,
              private motivosBajaService: MotivosBajaService,
              private activeModal: NgbActiveModal,
              private notificacionService: NotificacionService,
              private contactoService: ContactoService) {
  }

  public ngOnInit(): void {
    this.crearFormDatosContacto();
    this.obtenerDatosDeContacto();
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

    codigoArea.setValidators(validatorCodigoArea);
    telefono.setValidators(validatorTelefono);
    codigoAreaCelular.setValidators(validatorCodigoArea);
    celular.setValidators(validatorTelefono);

    codigoArea.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.codigoArea = value;
        ModalDatosConctactoComponent.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);
      });

    telefono.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.telefono = value;
        ModalDatosConctactoComponent.habilitarValidadores(value, codigoArea, telefono, validatorCodigoArea, validatorTelefono, codigoAreaCelular, celular);

      });

    codigoAreaCelular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.codigoAreaCelular = value;
        ModalDatosConctactoComponent.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    celular.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        this.persona.celular = value;
        ModalDatosConctactoComponent.habilitarValidadores(value, codigoAreaCelular, celular, validatorCodigoArea, validatorTelefono, codigoArea, telefono);
      });

    const email = new FormControl(
      this.persona.email,
      Validators.compose(
        [
          Validators.required,
          CustomValidators.email,
          Validators.minLength(7),
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
    if (!this.editable) {
      this.formDatosContacto.disable();
    }

    this.formDatosContacto.get('telefono').setValue(this.formDatosContacto.get('telefono').value);
    this.formDatosContacto.get('celular').setValue(this.formDatosContacto.get('celular').value);
  }

  public cerrar(): void {
    this.activeModal.close();
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
    this.contactoService.obtenerDatosDeContacto(this.persona).subscribe(
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
          this.crearFormDatosContacto();
        }
      },
      (errores) => {
        this.notificacionService.informar(<string[]> errores, true);
      });
  }

  public actualizarDatosDeContacto(): void {
    if (this.formDatosContacto.valid) {
      let comando = this.armarComandoActualizarDatosDeContacto();
      if (comando) {
        this.contactoService.actualizarDatosDeContacto(comando).subscribe(
          (respuesta) => {
            if (respuesta) {
              this.notificacionService.informar(['Los datos de contacto se modificaron con éxito.']);
            }
          }, (errores) => {
            this.notificacionService.informar(errores, true);
          });
      }
    }
    this.activeModal.close();
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
}
