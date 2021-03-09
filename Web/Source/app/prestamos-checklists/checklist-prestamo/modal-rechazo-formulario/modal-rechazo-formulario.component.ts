import { Component, Input, OnInit } from '@angular/core';
import { Formulario } from '../../../formularios/shared/modelo/formulario.model';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IntegrantePrestamo } from '../../../shared/modelo/integrante-prestamo.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { RechazarFormularioComando } from '../../../formularios/shared/modelo/rechazar-formulario-comando.model';
import { MotivoRechazo } from '../../../formularios/shared/modelo/motivo-rechazo';
import { MotivoRechazoService } from '../../../formularios/shared/motivo-rechazo.service';
import { MotivoRechazoComando } from '../../../formularios/shared/modelo/motivo-rechazo-comando.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-modal-rechazo-formulario',
  templateUrl: './modal-rechazo-formulario.component.html',
  styleUrls: ['./modal-rechazo-formulario.component.scss'],
})
export class ModalRechazoFormularioComponent implements OnInit {
  @Input() public form: FormGroup;
  @Input() public ambito: string;
  public formularios: Formulario [] = [];
  public motivosRechazo: MotivoRechazo [] = [];
  public motivosSeleccionados: MotivoRechazo[] = [];

  constructor(public activeModal: NgbActiveModal,
              private motivoRechazoService: MotivoRechazoService,
              private notificacionService: NotificacionService,
              private formularioService: FormulariosService) {
  }

  public ngOnInit(): void {
    this.motivoRechazoService.consultarMotivoRechazo('Checklist')
      .subscribe((motivosRechazo) => {
        this.motivosRechazo = motivosRechazo;
        this.concatenarCodigoDescripcion();
      });
  }

  public static nuevoFormGroup(integrantes: IntegrantePrestamo [], nroPrestamo: number): FormGroup {
    return new FormGroup({
      integrantes: new FormArray((integrantes || []).map((integrante) => new FormGroup({
        idFormulario: new FormControl(integrante.idFormulario),
        nomApe: new FormControl(integrante.apellidoNombre),
        cuil: new FormControl(integrante.cuil),
        estadoForm: new FormControl(integrante.estadoFormulario),
        nroLinea: new FormControl(integrante.nroLinea),
        nroPrestamo: new FormControl(nroPrestamo),
        seleccionado: new FormControl(false)
      }))),
      observaciones: new FormControl('', Validators.compose([Validators.maxLength(500), CustomValidators.validTextAndNumbers])),
      idMotivoRechazo: new FormControl([null, Validators.required]),
      numeroCaja: new FormControl('', Validators.compose([CustomValidators.validTextAndNumbers, Validators.required, Validators.maxLength(10)]))
    });
  }

  public get integrantesForm(): FormArray {
    return this.form.get('integrantes') as FormArray;
  }

  public cancelar(): void {
    this.activeModal.close();
  }

  public rechazarFormularios(): void {
    let formularioComando = new RechazarFormularioComando();
    formularioComando.idFormulario = this.integrantesForm.controls[0].get('idFormulario').value;
    formularioComando.motivosRechazo = this.motivosSeleccionados;
    formularioComando.numeroCaja = this.form.value.numeroCaja;

    this.formularioService.rechazarGrupoFormularios(formularioComando)
      .subscribe(() => {
        this.notificacionService.informar(['Formulario/s rechazado/s con éxito.'], false)
          .result.then(() => this.activeModal.close(new MotivoRechazoComando(this.motivosSeleccionados)));
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  public rechazar(): void {
    if (this.motivosSeleccionados.length) {
      this.activeModal.close(new MotivoRechazoComando(this.motivosSeleccionados));
    }
  }

  public agregarMotivo() {
    if (this.motivosSeleccionados.find((motivo) => motivo.id === this.idMotivoRechazoFc.value)) {
      return null;
    }
    let motivo = this.motivosRechazo.find((m) => m.id === this.idMotivoRechazoFc.value);
    if (motivo) {
      motivo.observaciones = this.form.get('observaciones').value;
      this.motivosSeleccionados.push(motivo);
      this.form.get('idMotivoRechazo').setValue(null, {emitEvent: false});
      this.form.get('observaciones').setValue(null, {emitEvent: false});
    }
  }

  public get idMotivoRechazoFc(): FormControl {
    return this.form.get('idMotivoRechazo') as FormControl;
  }

  public quitarMotivo(idMotivo: number) {
    const index = this.motivosSeleccionados.findIndex((motivo) => idMotivo === motivo.id);
    this.motivosSeleccionados.splice(index, 1);
  }

  public concatenarCodigoDescripcion() {
    this.motivosRechazo.forEach((motivo) => {
      if (motivo.codigo) {
        motivo.descripcion = motivo.codigo + ' - ' + motivo.nombre;
      }
    });
  }
}
