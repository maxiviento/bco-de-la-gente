import { FormControl, FormGroup, Validators, Form, AbstractControl } from '@angular/forms';
import { Component, EventEmitter, OnInit, Output, Input } from '@angular/core';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { Departamento } from '../../../formularios/shared/modelo/departamento.model';
import { Localidad } from '../../../formularios/shared/modelo/localidad.model';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { LocalidadComboServicio } from '../../../formularios/shared/localidad.service';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { LoteCombo } from '../../shared/modelo/lote-combo.model';
import { PagosService } from '../../shared/pagos.service';

@Component({
  selector: 'bg-filtros-formularios-plan-pagos',
  templateUrl: './filtros-formularios-plan-pagos.component.html',
  styleUrls: ['./filtros-formularios-plan-pagos.component.scss']
})

export class FiltrosFormulariosPlanPagosComponent implements OnInit {
  public filtros: FiltrosFormularioConsulta;
  public form: FormGroup;
  @Output() public filtrosBusqueda: EventEmitter<FiltrosFormularioConsulta> =
    new EventEmitter<FiltrosFormularioConsulta>();

  public departamentos: Departamento[] = [];
  public localidades: Localidad[] = [];
  public lotes: LoteCombo[] = [];
  @Input() public mostrarComboLotes: boolean;

  constructor(private formulariosService: FormulariosService,
    public localidadesService: LocalidadComboServicio,
    private pagosService: PagosService) {
  }

  public ngOnInit() {
    this.crearForm();
    this.formulariosService
      .consultarDepartamentos()
      .subscribe((departamentos) => this.departamentos = departamentos);
    this.obtenerComboLotes();
  }

  public crearForm() {
    this.form = new FormGroup({
      cuil: new FormControl('', Validators.compose([Validators.maxLength(11), CustomValidators.number])),
      dni: new FormControl('', Validators.compose([Validators.maxLength(8), CustomValidators.number])),
      nroPrestamo: new FormControl('', Validators.compose([
        CustomValidators.number,
        Validators.maxLength(8)])),
      nroFormulario: new FormControl('', Validators.compose([
        CustomValidators.number,
        Validators.maxLength(14)])),
      idDepartamento: new FormControl(''),
      idLocalidad: new FormControl(''),
      idLote: this.mostrarComboLotes ? new FormControl('') : new FormControl('')
    });
    this.suscribirLocalidades();
    this.suscribirObligatoriedadFiltros();
  }

  private actualizarObligatoriedadFiltros(formControlObligatorio: string, todosRequeridos: boolean = false) {
    let cuil = this.form.get('cuil') as FormControl;
    let dni = this.form.get('dni') as FormControl;
    let nroPrestamo = this.form.get('nroPrestamo') as FormControl;
    let nroFormulario = this.form.get('nroFormulario') as FormControl;
    let idLote = this.form.get('idLote') as FormControl;

    cuil.clearValidators();
    dni.clearValidators();
    nroPrestamo.clearValidators();
    nroFormulario.clearValidators();
    idLote.clearValidators();

    cuil.setValidators(Validators.compose([Validators.maxLength(11), CustomValidators.number]));
    dni.setValidators(Validators.compose([Validators.maxLength(8), CustomValidators.number]));
    nroPrestamo.setValidators(Validators.compose([
      CustomValidators.number,
      Validators.maxLength(8)
    ]));
    nroFormulario.setValidators(Validators.compose([
      CustomValidators.number,
      Validators.maxLength(14)
    ]));

    cuil.updateValueAndValidity();
    dni.updateValueAndValidity();
    nroPrestamo.updateValueAndValidity();
    nroFormulario.updateValueAndValidity();
    idLote.updateValueAndValidity();
  }

  public consultaValida(): boolean {
    if (!this.mostrarComboLotes) {
      return true;
    }
    return this.form.valid;
  }

  private suscribirObligatoriedadFiltros() {
    let cuil = this.form.get('cuil') as FormControl;
    let dni = this.form.get('dni') as FormControl;
    let nroPrestamo = this.form.get('nroPrestamo') as FormControl;
    let nroFormulario = this.form.get('nroFormulario') as FormControl;
    let idLote = this.form.get('idLote') as FormControl;

    if (this.mostrarComboLotes) {
      idLote.valueChanges.distinctUntilChanged()
        .subscribe((value) => {
          if (value) {
            this.actualizarObligatoriedadFiltros('idLote');

          } else {
            if (cuil.value || dni.value || nroPrestamo.value || nroFormulario.value) {
              this.actualizarObligatoriedadFiltros(null);
            } else {
              this.actualizarObligatoriedadFiltros(null, true);
            }
          }
        });
    }

    cuil.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          this.actualizarObligatoriedadFiltros('cuil');
        } else {
          if (idLote.value || dni.value || nroPrestamo.value || nroFormulario.value) {
            this.actualizarObligatoriedadFiltros(null);
          } else {
            this.actualizarObligatoriedadFiltros(null, true);
          }
        }
      });

    dni.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          this.actualizarObligatoriedadFiltros('dni');

        } else {
          if (idLote.value || cuil.value || nroPrestamo.value || nroFormulario.value) {
            this.actualizarObligatoriedadFiltros(null);
          } else {
            this.actualizarObligatoriedadFiltros(null, true);
          }
        }
      });

    nroPrestamo.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          this.actualizarObligatoriedadFiltros('nroPrestamo');

        } else {
          if (idLote.value || cuil.value || dni.value || nroFormulario.value) {
            this.actualizarObligatoriedadFiltros(null);
          } else {
            this.actualizarObligatoriedadFiltros(null, true);
          }
        }
      });

    nroFormulario.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          this.actualizarObligatoriedadFiltros('nroFormulario');

        } else {
          if (idLote.value || cuil.value || dni.value || nroPrestamo.value) {
            this.actualizarObligatoriedadFiltros(null);
          } else {
            this.actualizarObligatoriedadFiltros(null, true);
          }
        }
      });

  }

  private suscribirLocalidades() {
    this.form.get('idDepartamento').valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          this.localidadesService.consultarLocalidades(this.form.get('idDepartamento').value)
            .subscribe((localidades) => {
              this.localidades = localidades;
              if (this.localidades.length) {
                (this.form.get('idLocalidad') as FormControl).enable();
              } else {
                this.localidades = [];
                (this.form.get('localidad') as FormControl).disable();
              }
            });
        }
      });
  }

  private obtenerComboLotes() {
    this.pagosService.obtenerComboLotes()
      .subscribe((lotes) => {
        this.lotes = (lotes);
      });
  }

  public prepararForm(): void {
    let formValue = this.form.value;
    this.filtros = new FiltrosFormularioConsulta();
    this.filtros.cuil = formValue.cuil;
    this.filtros.dni = formValue.dni;
    this.filtros.nroPrestamo = formValue.nroPrestamo;
    this.filtros.nroFormulario = formValue.nroFormulario;
    this.filtros.idDepartamento = formValue.idDepartamento;
    this.filtros.idLocalidad = formValue.idLocalidad;
    if (this.mostrarComboLotes) {
      this.filtros.idLote = formValue.idLote;
    }
    this.filtrosBusqueda.emit(Object.assign({}, this.filtros));
  }

  public limpiarFiltros(): void {
    this.crearForm();
  }
}
