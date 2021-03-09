import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { ELEMENTOS, Pagina } from '../../../../shared/paginacion/pagina-utils';
import { FormulariosService } from '../../../../formularios/shared/formularios.service';
import { FiltrosFormularioConsulta } from '../../modelos/filtros-formulario-consulta.model';
import { FormularioSeleccionado } from '../../modelos/formulario-seleccionado.model';
import { TipoConsultaFormulario } from '../../modelos/tipo-consulta-formulario-enum';

@Component({
  selector: 'bg-grilla-formularios',
  templateUrl: './grilla-formularios.component.html',
  styleUrls: ['./grilla-formularios.component.scss'],
})

export class GrillaFormulariosComponent implements OnInit {
  private _filtros: FiltrosFormularioConsulta;
  @Input()
  public set filtros(filtros: FiltrosFormularioConsulta) {
    this.limpiarGrilla();
    this._filtros = filtros;
    if (this._filtros) {
      this.paginaModificada.next(0);
    }
  }

  @Input()
  public set limpiarIds(value: boolean) {
    if (value) {
      this.idsFormulariosSeleccionados = [];
      this.crearForm();
    }
  }

  @Input() public esDocumentacion: boolean;
  @Input() public totalizador: number;
  @Output() public formulariosSeleccionados: EventEmitter<number []> = new EventEmitter<number []>();
  @Output() public formulariosSeleccionadosNoApoderado: EventEmitter<FormularioSeleccionado []> = new EventEmitter<FormularioSeleccionado []>();
  public formularios: FormularioSeleccionado [] = [];
  public formulariosNoApoderado: FormularioSeleccionado [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public form: FormGroup;
  public idsFormulariosSeleccionados: number [] = [];
  public total: number;


  constructor(private fb: FormBuilder,
              private formulariosService: FormulariosService) {
    this.configurarPaginacion();
  }

  public ngOnInit() {
    this.crearForm();
  }

  private crearForm() {
    this.form = this.fb.group({
      formularios: this.fb.array((this.formularios || []).map((formulario) => {
          let idFormularioFc = new FormControl(formulario.idFormulario);

          return this.fb.group({
            idFormulario: idFormularioFc,
            cuilDni: [formulario.cuilDni],
            apellidoNombre: [formulario.apellidoNombre],
            nombreBanco: [formulario.nombreBanco],
            nombreSucursal: [formulario.nombreSucursal],
            nombreLinea: [formulario.nombreLinea],
            nroPrestamo: [formulario.nroPrestamo],
            nroFormulario: [formulario.nroFormulario],
            nombreDepartamento: [formulario.nombreDepartamento],
            nombreLocalidad: [formulario.nombreLocalidad],
          });
        }
      ))
    });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this._filtros.numeroPagina = params.numeroPagina;
        return this.formulariosService.buscarFormulariosFiltros(this._filtros, this.getUriConsulta(this._filtros.consulta)[0]);
      }).share();
    (<Observable<FormularioSeleccionado[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((formularios) => {
        this.formularios = formularios;
        this.crearForm();
        if (!this.esDocumentacion) {
          this.formulariosService.consultarTotalizadorSucursalBancaria(this._filtros)
            .subscribe((total) => this.totalizador = total);
        }
      });
  }

  public consultarSiguientesFormularios(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public get formulariosFa(): FormArray {
    return this.form.get('formularios') as FormArray;
  }

  public limpiarGrilla(): void {
    this.idsFormulariosSeleccionados = [];
    this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados.concat([]));
    this.formularios = [];
    this.crearForm();
  }

  private getUriConsulta(tipoConsulta: number): string[] {
    if (tipoConsulta === TipoConsultaFormulario.SUCURSAL_BANCARIA) {
      return ['buscar-formularios-sucursal', 'buscar-ids-formularios-sucursal-filtro'];
    } else if (tipoConsulta === TipoConsultaFormulario.DOCUMENTACION_PAGOS) {
      return ['buscar-formularios', 'buscar-ids-formularios-filtro'];
    } else {
      return ['buscar-formularios', 'buscar-ids-formularios-filtro'];
    }
  }

  public agregarFormulario(idFormulario: number): void {
    this.idsFormulariosSeleccionados.push(idFormulario);
    this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);

    let formulario = this.formularios.find((form) => form.idFormulario === idFormulario);
    if (formulario.tipoApoderado === 3) {
      this.formulariosNoApoderado.push(formulario);
      this.formulariosSeleccionadosNoApoderado.emit(this.formulariosNoApoderado);
    }
  }

  public quitarFormulario(idFormulario: number, toolTipQuitar: any): void {
    toolTipQuitar.close();
    this.idsFormulariosSeleccionados = this.idsFormulariosSeleccionados.filter(
      (x) => x !== idFormulario);
    this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);
    let formulario = this.formularios.find((form) => form.idFormulario === idFormulario);
    if (formulario.tipoApoderado === 3) {
      this.formulariosNoApoderado = this.formulariosNoApoderado.filter((value) => value.idFormulario !== idFormulario);
      this.formulariosSeleccionadosNoApoderado.emit(this.formulariosNoApoderado);
    }
  }

  public estaSeleccionado(idFormulario: number): boolean {
    const esSeleccionado = this.idsFormulariosSeleccionados.find(
      (id) => id === idFormulario);
    return esSeleccionado !== undefined;
  }

  public clickSeleccionarTodos(checked: boolean) {
    if (checked) {
      this.formulariosService.buscarIdsFormulariosFiltros(this._filtros, this.getUriConsulta(this._filtros.consulta)[1])
        .subscribe((formularios) => {
          let ids = new Set<number>();
          formularios.forEach((formulario) => {
            ids.add(formulario.idFormulario);
          });
          ids.forEach((idForm) => this.idsFormulariosSeleccionados.push(idForm));
        });
    } else {
      this.idsFormulariosSeleccionados = [];
    }
    this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);
  }

  public todosSeleccionados(): boolean {
    return this.totalizador == this.idsFormulariosSeleccionados.length;
  }
}
