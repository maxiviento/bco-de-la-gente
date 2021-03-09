import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { SeleccionMultiple } from './seleccion-multiple.model';
import { IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { FiltroDomicilioBandejaService } from './filtro-domicilio-bandeja.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'filtro-domicilio-bandeja',
  templateUrl: './filtro-domicilio-bandeja.component.html',
  styleUrls: ['./filtro-domicilio-bandeja.component.scss'],
  providers: [FiltroDomicilioBandejaService]
})
export class FiltroDomicilioBandejaComponent implements OnInit {

  public form: FormGroup;
  public departamentos: SeleccionMultiple[];
  public depto: SeleccionMultiple;
  public localidades: SeleccionMultiple[];
  public seleccionDepto: number[] = [];
  public seleccionLocalidad: number[] = [];

  @Output() public departamentosSeleccionados: EventEmitter<string> = new EventEmitter<string>();
  @Output() public localidadesSeleccionadas: EventEmitter<string> = new EventEmitter<string>();

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
    showCheckAll: true,
    showUncheckAll: true,
    enableSearch: true,
    displayAllSelectedText: true
  };

  constructor(private fb: FormBuilder, private service: FiltroDomicilioBandejaService) {

    this.departamentos = [];
    this.localidades = [];
  }

  public ngOnInit(): void {
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
          return deptosSeleccionados.filter((depto) => parseInt(loc.parentId, null) === -depto.id);
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
    if (this.seleccionDepto) {
      this.getDepartamentos();
      this.form.controls['departamentoId'].setValue(this.seleccionDepto);
      if (this.seleccionLocalidad) {
        let selecccionLoc;
        this.seleccionDepto.map((id) => {
          this.depto = new SeleccionMultiple(id);
          this.getLocalidades(this.depto);
          selecccionLoc = this.seleccionLocalidad.filter((idLoc) => idLoc > 0);
        });
        this.form.controls['localidadId'].setValue(selecccionLoc);
      }
    }
  }

  public getDepartamentos() {
    this.service
      .consultarDepartamentos()
      .subscribe((departamentos) => this.departamentos = departamentos);
  }

  public getLocalidades(depto: SeleccionMultiple) {
    this.service.consultarLocalidades(depto)
      .subscribe((localidades) => {
        this.localidades = this.localidades.concat(localidades);
      });
  }

  public crearForm() {
    this.form = this.fb.group({
      departamentoId: [null],
      localidadId: [null]
    });
  }

  public setLocaldiades(idSeleccionados: string): void {
    if (idSeleccionados) {
      let locaSeleccion = idSeleccionados.split(',');
      locaSeleccion.forEach((id) => {
        if (id) {
          this.seleccionLocalidad.push(+id);
        }
      });
    }
    this.crearForm();
  }

  public setDeptos(idSeleccionados: string): void {
    if (idSeleccionados) {
      let deptoSeleccion = idSeleccionados.split(',');
      deptoSeleccion.forEach((id) => {
        if (id) {
          this.seleccionDepto.push(+id);
        }
      });
    }
    this.crearForm();
  }
}
