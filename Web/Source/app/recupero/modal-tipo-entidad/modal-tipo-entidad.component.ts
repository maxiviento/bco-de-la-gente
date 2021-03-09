import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ComboEntidadesRecupero } from '../shared/modelo/combo-entidades-recupero.model';
import { RecuperoService } from '../shared/recupero.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbUtils } from "../../shared/ngb/ngb-utils";
import { Convenio } from "../../shared/modelo/convenio-model";
import { EntidadResultado } from "../../shared/modelo/consultas/entidad-resultado.model";
import { NotificacionService } from "../../shared/notificacion.service";

@Component({
  selector: 'bg-modal-tipo-entidad',
  templateUrl: './modal-tipo-entidad.component.html',
  styleUrls: ['./modal-tipo-entidad.component.scss'],
})

export class ModalTipoEntidadComponent implements OnInit {
  public title: string;
  public message: string;
  public form: FormGroup;
  public entidades: ComboEntidadesRecupero[] = [];
  public conveniosRecupero: Convenio[] = [];
  public resultadoModal: EntidadResultado;
  public nombreArchivo: string;


  constructor(private fb: FormBuilder,
              private recuperoService: RecuperoService,
              private notificacionService: NotificacionService,
              private activeModal: NgbActiveModal) {
    this.title = 'Seleccione información de recupero';
  }

  ngOnInit(): void {
    this.crearForm();

    this.recuperoService.obtenerComboTipoEntidadRecupero()
      .subscribe((entidades) => {
        this.entidades = (entidades);
      });
    this.recuperoService.consultarConveniosRecupero()
      .subscribe((convenios) => {
        this.conveniosRecupero = convenios;
      })
  }

  private crearForm(): void {
    let fechaRecupero = new FormControl(NgbUtils.obtenerNgbDateStruct(new Date(Date.now())), Validators.required);

    this.form = this.fb.group({
      entidad: ['', Validators.required],
      convenio: ['', Validators.required],
      fechaRecupero: fechaRecupero
    });
  }

  public aceptar(): void {
    if (this.form.valid) {
      if (this.validarConvenioSeleccionado()) {
        this.prepararCierreModal();
        this.activeModal.close(this.resultadoModal);
      } else {
        this.notificacionService.informar(['El convenio seleccionado es diferente al del archivo seleccionado'], true);
      }
    }
  }

  public validarConvenioSeleccionado(): boolean {
    if (this.form.value.entidad == 1 || this.form.value.entidad == 2) {
      return true;
    }

    let convenioSeleccionado = this.conveniosRecupero.find((convenio) => convenio.id == this.form.value.convenio).nombre;

    return this.nombreArchivo.includes(convenioSeleccionado);
  }

  private prepararCierreModal() {
    this.resultadoModal = new EntidadResultado(this.form.value.entidad, NgbUtils.obtenerDate(this.form.value.fechaRecupero), this.form.value.convenio);
  }

  public cerrar(): void {
    this.activeModal.close();
  }
}
