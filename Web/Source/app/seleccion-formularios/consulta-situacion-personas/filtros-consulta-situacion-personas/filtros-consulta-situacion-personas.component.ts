import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { ConsultaSituacionPersonas } from '../../shared/modelos/consulta-situacion-personas.model';
import { BusquedaPorPersonaComponent } from '../../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-filtros-consulta-situacion-personas',
  templateUrl: './filtros-consulta-situacion-personas.component.html',
  styleUrls: ['./filtros-consulta-situacion-personas.component.scss']
})

export class FiltrosConsultaSituacionPersonasComponent implements OnInit {
  public filtros: ConsultaSituacionPersonas;
  public form: FormGroup;
  @Output() public filtrosBusqueda: EventEmitter<ConsultaSituacionPersonas> = new EventEmitter<ConsultaSituacionPersonas>();

  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;

  constructor(private fb: FormBuilder) {
  }

  public ngOnInit() {
    this.crearForm();
  }

  private crearForm() {
    this.componentePersona.crearForm();
    this.form = this.fb.group({
      numeroSticker: ['', Validators.compose([CustomValidators.number, Validators.maxLength(14)])],
      numeroFormulario: ['', Validators.compose([CustomValidators.number, Validators.maxLength(14)])],
      numeroPrestamo: ['', Validators.compose([CustomValidators.number, Validators.maxLength(8)])]
    });
  }

  public consultaValida(): boolean {
    return this.componentePersona.formValid();
  }

  public emitirConsulta(): void {
    let consultaPersona = this.componentePersona.prepararConsulta();
    this.filtros = new ConsultaSituacionPersonas(
      consultaPersona.tipoPersona,
      consultaPersona.cuil,
      consultaPersona.apellido,
      consultaPersona.nombre,
      consultaPersona.dni,
      this.form.value.numeroSticker,
      this.form.value.numeroFormulario,
      this.form.value.numeroPrestamo
    );
    this.filtrosBusqueda.emit(Object.assign({}, this.filtros));
  }

  public limpiarFiltros(): void {
    this.crearForm();
  }
}
