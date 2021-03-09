import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Persona } from '../../shared/modelo/persona.model';
import { MiembroEmprendimiento } from '../shared/modelo/miembro-emprendimiento.model';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { isNullOrUndefined } from 'util';
import { Vinculo } from '../shared/modelo/vinculo.model';

@Component({
  selector: 'bg-modal-integrante-emprendimiento',
  templateUrl: './modal-integrante-emprendimiento.component.html',
  styleUrls: ['./modal-integrante-emprendimiento.component.scss'],
})

export class ModalIntegranteEmprendimientoComponent implements OnInit {
  public form: FormGroup;
  @Input() public miembro: MiembroEmprendimiento = new MiembroEmprendimiento();
  @Input() public vinculos: Vinculo[] = [];
  public edicion: boolean = false;

  constructor(private fb: FormBuilder,
              private activeModal: NgbActiveModal) {
  }

  public ngOnInit(): void {
    this.edicion = !isNullOrUndefined(this.miembro.persona);
    this.crearForm();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      apellido: [this.miembro.persona ? this.miembro.persona.apellido : '', Validators.required],
      nombre: [this.miembro.persona ? this.miembro.persona.nombre : '', Validators.required],
      vinculo: [this.miembro.idVinculo ? this.miembro.idVinculo : null, this.miembro.esSolicitante ? null : Validators.required],
      edad: [this.miembro.persona ? this.miembro.persona.edad : ''],
      tarea: [this.miembro.tarea, Validators.compose([Validators.required, Validators.maxLength(50)])],
      horario: [this.miembro.horarioTrabajo, Validators.maxLength(50)],
      sueldo: [this.miembro.remuneracion,
      Validators.compose([
        CustomValidators.number,
        Validators.required,
        Validators.maxLength(8)])],
      antecedentesLaborales: [this.miembro.antecedentesLaborales]
    });
  }

  public personaConsultada(persona: Persona) {
    this.miembro.persona = persona;
    this.crearForm();
  }

  public personaSeleccionada(): boolean{
    return !isNullOrUndefined(this.miembro.persona);
  }

  private prepararForm(): MiembroEmprendimiento{
    let formModel = this.form.value;
    let vinculo = this.vinculos.find(x => x.id === formModel.vinculo);
    this.miembro.idVinculo = formModel.vinculo;
    this.miembro.vinculo = vinculo ? vinculo.nombre : null;
    this.miembro.tarea = formModel.tarea;
    this.miembro.horarioTrabajo = formModel.horario;
    this.miembro.remuneracion = formModel.sueldo;
    this.miembro.antecedentesLaborales = formModel.antecedentesLaborales;
    return this.miembro;
  }

  public aceptar(): void{
    this.activeModal.close({miembro: this.prepararForm()});
  }

  public cerrar(): void {
    this.activeModal.close();
  }
}
