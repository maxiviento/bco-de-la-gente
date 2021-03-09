import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Localidad } from '../../../../formularios/shared/modelo/localidad.model';
import { Departamento } from '../../../../formularios/shared/modelo/departamento.model';
import { FormulariosService } from '../../../../formularios/shared/formularios.service';
import { LocalidadComboServicio } from '../../../../formularios/shared/localidad.service';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { FiltrosFormularioConsulta } from '../../modelos/filtros-formulario-consulta.model';
import { LoteCombo } from '../../../../pagos/shared/modelo/lote-combo.model';
import { PagosService } from '../../../../pagos/shared/pagos.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'bg-busqueda-formularios',
  templateUrl: './busqueda-formularios.component.html',
  styleUrls: ['./busqueda-formularios.component.scss']
})

export class BusquedaFormulariosComponent implements OnInit {
  public filtros: FiltrosFormularioConsulta;
  public form: FormGroup;
  @Output() public filtrosBusqueda: EventEmitter<FiltrosFormularioConsulta> =
    new EventEmitter<FiltrosFormularioConsulta>();
  @Output() public totalizador: EventEmitter<number> = new EventEmitter<number>();
  @Input() public esDocumentacion: boolean;

  public departamentos: Departamento[] = [];
  public localidades: Localidad[] = [];
  public lotes: LoteCombo[] = [];
  @Input() public localidadObligatorio: boolean = false;
  @Input() public mostrarComboLotes: boolean;

  constructor(private formulariosService: FormulariosService,
              public localidadesService: LocalidadComboServicio,
              private pagosService: PagosService,
              private router: Router,
              private route: ActivatedRoute) {
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
        Validators.maxLength(8)
      ])),
      nroFormulario: new FormControl('', Validators.compose([
        CustomValidators.number,
        Validators.maxLength(14)
      ])),
      idDepartamento: new FormControl(''),
      idLocalidad: new FormControl(''),
      idLote: new FormControl('')
    });
    this.suscribirLocalidades();
  }

  private obtenerComboLotes() {
    this.pagosService.obtenerComboLotes()
      .subscribe((lotes) => {
        this.lotes = (lotes);
      });
  }

  private suscribirLocalidades() {
    this.form.get('idDepartamento').valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          if (this.localidadObligatorio) (this.form.get('idLocalidad') as FormControl).setValidators(Validators.required);
          this.localidadesService.consultarLocalidades(this.form.get('idDepartamento').value)
            .subscribe((localidades) => {
              this.localidades = localidades;
              if (this.localidades.length) {
                (this.form.get('idLocalidad') as FormControl).enable();
              } else {
                this.localidades = [];
                (this.form.get('idLocalidad') as FormControl).disable();
              }
            });
        } else {
          if (this.localidadObligatorio) (this.form.get('idLocalidad') as FormControl).clearValidators();
          (this.form.get('idLocalidad') as FormControl).setValue('');
        }
      });
  }

  public esRequerido(control: AbstractControl): boolean {
    let formControl = control as FormControl;
    if (!formControl.validator) {
      return false;
    }
    let reqControl = new FormControl('', Validators.required);
    if (!formControl.validator(reqControl.value)) {
      return false;
    }
    let validator = formControl.validator(reqControl.value) as any;
    return validator.hasOwnProperty('required');
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
    this.filtros.idLote = formValue.idLote;
    this.filtrosBusqueda.emit(Object.assign({}, this.filtros));
    this.consultarTotalizador(this.filtros);
  }

  public limpiarFiltros(): void {
    this.crearForm();
  }

  public consultarTotalizador(filtros: FiltrosFormularioConsulta) {
    if (this.esDocumentacion) {
      this.totalizador.emit(0);
      this.formulariosService
        .consultarTotalizadorDocumentacion(filtros)
        .subscribe((num) => this.totalizador.emit(num));
    } else {
      if (this.router.url.includes('actualizar-sucursal/lote')) {
        filtros.idLote = this.route.snapshot.params['id'];
      }
      this.totalizador.emit(0);
      this.formulariosService
        .consultarTotalizadorSucursalBancaria(filtros)
        .subscribe((num) => this.totalizador.emit(num));
    }
  }
}
