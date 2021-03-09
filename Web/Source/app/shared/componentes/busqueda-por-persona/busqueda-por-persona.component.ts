import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../forms/custom-validators';
import { BusquedaPorPersonaConsulta } from '../../modelo/busqueda-por-persona-consulta.model';

@Component({
  selector: 'bg-busqueda-por-persona',
  templateUrl: './busqueda-por-persona.component.html',
  styleUrls: ['./busqueda-por-persona.component.scss'],
})
export class BusquedaPorPersonaComponent implements OnInit {

  public form: FormGroup;
  public tiposPersona: any[] = [];
  public consulta: BusquedaPorPersonaConsulta = new BusquedaPorPersonaConsulta();

  constructor(private fb: FormBuilder) {
    this.tiposPersona = [
      {id: '1', descripcion: 'Solicitante'},
      {id: '2', descripcion: 'Garante'},
      //{id: '3', descripcion: 'Apoderado'},
      {id: '4', descripcion: 'Todos'}
    ];
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      persona: [this.consulta.tipoPersona],
      cuil: [this.consulta.cuil, Validators.compose([Validators.maxLength(11), CustomValidators.number])],
      dni: [this.consulta.dni, Validators.compose([Validators.maxLength(8), CustomValidators.number])],
      apellido: [this.consulta.apellido, Validators.compose([
        Validators.minLength(3),
        Validators.maxLength(200),
        CustomValidators.validTextAndNumbers
      ])],
      nombre: [this.consulta.nombre, Validators.compose([
        Validators.minLength(3),
        Validators.maxLength(200),
        CustomValidators.validTextAndNumbers
      ])]
    });

    this.suscribirObligatoriedadFiltros();
  }

  public setFiltros(consulta: BusquedaPorPersonaConsulta): void {
    this.consulta = consulta;
    this.crearForm();
  }

  public prepararConsulta(): BusquedaPorPersonaConsulta {
    let formModel = this.form.value;
    return new BusquedaPorPersonaConsulta(
      formModel.persona,
      formModel.cuil,
      formModel.apellido,
      formModel.nombre,
      formModel.dni
    );
  }

  public esRequerido(control: AbstractControl): boolean {
    let formControl = control as FormControl;
    if (!formControl.validator) return false;
    let reqControl = new FormControl('', Validators.required);
    if (!formControl.validator(reqControl.value)) return false;
    let validator = formControl.validator(reqControl.value) as any;
    return validator.hasOwnProperty('required');
  }

  private suscribirObligatoriedadFiltros() {
    let cuil = this.form.get('cuil') as FormControl;
    let dni = this.form.get('dni') as FormControl;
    let apellido = this.form.get('apellido') as FormControl;
    let nombre = this.form.get('nombre') as FormControl;

    cuil.valueChanges.distinctUntilChanged()
      .subscribe(() => {
        this.verificarTipoPersonaObligatorio();
      });

    dni.valueChanges.distinctUntilChanged()
      .subscribe(() => {
        this.verificarTipoPersonaObligatorio();
      });

    apellido.valueChanges.distinctUntilChanged()
      .subscribe(() => {
        this.verificarTipoPersonaObligatorio();
      });

    nombre.valueChanges.distinctUntilChanged()
      .subscribe(() => {
        this.verificarTipoPersonaObligatorio();
      });
  }

  private verificarTipoPersonaObligatorio(): void {
    let cuil = this.form.get('cuil') as FormControl;
    let dni = this.form.get('dni') as FormControl;
    let apellido = this.form.get('apellido') as FormControl;
    let nombre = this.form.get('nombre') as FormControl;
    let tipoPersona = this.form.get('persona') as FormControl;
    let obligatorio = cuil.value || apellido.value || nombre.value || dni.value;
    if (obligatorio) {
      tipoPersona.clearValidators();
      tipoPersona.setValidators(Validators.required);
      tipoPersona.updateValueAndValidity();
    } else {
      tipoPersona.clearValidators();
      tipoPersona.updateValueAndValidity();
    }
  }

  public formValid(): boolean {
    return this.form.valid;
  }

  public documentoIngresado(): boolean {
    if (!this.form) return false;
    let formModel = this.form.value;
    return formModel && (formModel.cuil || formModel.dni);
  }
}
