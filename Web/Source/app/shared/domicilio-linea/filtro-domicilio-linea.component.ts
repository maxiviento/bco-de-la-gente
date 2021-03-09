import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { SeleccionMultiple } from './seleccion-multiple.model';
import { IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FiltroDomicilioLineaService } from './filtro-domicilio-linea.service';
import { FormularioPrestamo } from '../../pagos/shared/modelo/formularios-prestamo.model';
import { Localidad } from './localidad.model';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'filtro-domicilio-linea',
  templateUrl: './filtro-domicilio-linea.component.html',
  styleUrls: ['./filtro-domicilio-linea.component.scss'],
  providers: [FiltroDomicilioLineaService]
})
export class FiltroDomicilioLineaComponent implements OnInit {

  public form: FormGroup;
  public departamentos: SeleccionMultiple[];
  public depto: SeleccionMultiple;
  public localidades: SeleccionMultiple[];
  public unique: number[] = [];

  @Output() public departamentosSeleccionados: EventEmitter<string> = new EventEmitter<string>();
  @Output() public localidadesSeleccionadas: EventEmitter<string> = new EventEmitter<string>();
  @Input() public localidadesInit: Localidad[] = [];
  @Input() public esEditable: boolean;

  public departamentosTexts: IMultiSelectTexts = {
    checkAll: 'Seleccionar todos',
    uncheckAll: 'Deseleccionar todos',
    checkedPlural: 'departamentos seleccionados',
    searchPlaceholder: 'Buscar',
    searchEmptyResult: 'Sin resultados',
    defaultTitle: 'Seleccione',
    allSelected: 'Todos seleccionados'
  };

  public localidadesTexts: IMultiSelectTexts = {
    checkAll: 'Seleccionar todas',
    uncheckAll: 'Deseleccionar todas',
    checkedPlural: 'localidades seleccionadas',
    searchPlaceholder: 'Buscar',
    searchEmptyResult: 'Sin resultados',
    defaultTitle: 'Seleccione',
    allSelected: 'Todas seleccionadas'
  };

  public settings: IMultiSelectSettings = {
    buttonClasses: 'form-control item-seleccionado',
    containerClasses: 'multiple-contenedor',
    itemClasses: 'pointer',
    showCheckAll: true,
    showUncheckAll: true
  };

  constructor(private fb: FormBuilder, private service: FiltroDomicilioLineaService) {

    this.departamentos = [];
    this.localidades = [];
  }

  public ngOnInit(): void {
    if (!this.esEditable) {
      this.settings.showCheckAll = false;
      this.settings.showUncheckAll = false;
      this.settings.itemClasses = 'pointerOff';
    } else {
      //this.settings.itemClasses = '{ pointer-events: auto; }';
    }
    this.crearForm();
    this.getDepartamentos();

    this.form.controls['departamentoId'].valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        // cambios de depto
        this.departamentosSeleccionados.emit(value);
        const valueAsArray = value as string[];

        // deselecciono localidades si se quita un departamento
        let localidadesSeleccionadasId = valueAsArray.length ? this.form.controls['localidadId'].value as string[] : [];
        let localidadesSeleccionadas = this.localidades.filter((loc) => {
          return localidadesSeleccionadasId.find((locId) => parseInt(locId, null) === loc.id);
        });
        let deptosSeleccioandosId = valueAsArray;
        let deptosSeleccionados = this.departamentos.filter((d) => {
          return deptosSeleccioandosId.find((deptoId) => parseInt(deptoId, null) === d.id);
        });
        let localidadesRestantes = localidadesSeleccionadas.filter((loc) => {
          return deptosSeleccionados.every((depto) => parseInt(loc.parentId, null) === -depto.id);
        });

        if (valueAsArray.length === 0) {
          this.form.controls['localidadId'].setValue(null);
        }
        if (localidadesRestantes.length > 0) {
          this.form.controls['localidadId'].setValue(localidadesRestantes.map((l) => l.id));
        }
        this.localidades = [];
        const ids = valueAsArray.map((id) => {
          this.depto = this.departamentos.find((d) => d.id === parseInt(id, null));
          if (this.depto) {
            this.getLocalidades(this.depto);
          }
          return id;
        });

      });

    this.form.controls['localidadId'].valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        // cambios de localidad
        this.localidadesSeleccionadas.emit(value);

      });
  }

  public getDepartamentos() {
    this.service
      .consultarDepartamentos()
      .subscribe((departamentos) => {
        this.departamentos = departamentos;
        if (this.localidadesInit && this.localidadesInit.length) {
          this.unique = Array.from(new Set(this.localidadesInit.map((item) => item.idDepartamento)));
          this.form.controls['departamentoId'].setValue(this.departamentos.filter((x) => this.unique.find((y) => y == x.id)).map((l) => l.id));
        }
      });
  }

  public getLocalidades(depto: SeleccionMultiple) {
    this.service.consultarLocalidades(depto)
      .subscribe((localidades) => {
        this.localidades = this.localidades.concat(localidades);
        if (!this.localidadesInit.length) {
          this.form.controls['localidadId'].setValue(this.localidades.filter((x) => x.id > 0).map((l) => l.id));
        } else {
          let locs = [];
          if (!this.unique.find((x) => x == depto.id)) {
            locs = this.localidades.filter((z) => parseInt(z.parentId, null) == -depto.id);
          }
          let assing = this.localidades.filter((x) => this.localidadesInit.find((y) => y.idLocalidad == x.id)).concat(locs).map((l) => l.id);
          this.form.controls['localidadId'].setValue(assing);
        }
      });
  }

  public crearForm() {
    this.form = this.fb.group({
      departamentoId: [null],
      localidadId: [null]
    });
  }
}
