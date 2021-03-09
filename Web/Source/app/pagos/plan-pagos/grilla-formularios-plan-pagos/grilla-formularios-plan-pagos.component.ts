import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { FormularioSeleccionado } from '../../../seleccion-formularios/shared/modelos/formulario-seleccionado.model';
import { Pagina, ELEMENTOS } from '../../../shared/paginacion/pagina-utils';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { PagosService } from '../../shared/pagos.service';

@Component({
  selector: 'bg-grilla-formularios-plan-pagos',
  templateUrl: './grilla-formularios-plan-pagos.component.html',
  styleUrls: ['./grilla-formularios-plan-pagos.component.scss']
})

export class GrillaFormulariosPlanPagosComponent implements OnInit {
  private _filtros: FiltrosFormularioConsulta;
  public formularios: FormularioSeleccionado [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public form: FormGroup;
  public idsFormulariosSeleccionados: number [] = [];
  public idsFormulariosTotalVisualizar: number [] = [];
  public idsFormulariosTotalActualizar: number [] = [];
  public cantidadFormulariosTotalVisualizar: number = 0;
  public cantidadFormulariosTotalActualizar: number = 0;
  public creaPlan: boolean = false;
  @Output() public formulariosSeleccionados: EventEmitter<number []> = new EventEmitter<number []>();
  @Output() public fechaInicioPagos: EventEmitter<Date> = new EventEmitter<Date>();

  @Input()
  public set filtros(filtros: FiltrosFormularioConsulta) {
    this._filtros = filtros;
    if (this._filtros) {
      this.consultarTodosFormulariosFiltrados();
      this.paginaModificada.next(0);
    }
  }

  @Input()
  public set limpiarIds(value: boolean) {
    if (value) {
      this.idsFormulariosSeleccionados = [];
      this.cantidadFormulariosTotalVisualizar = 0;
      this.cantidadFormulariosTotalActualizar = 0;
      this.crearForm();
    }
  }

  constructor(private fb: FormBuilder,
              private pagosService: PagosService) {
    this.configurarPaginacion();
  }

  public ngOnInit() {
    this.crearForm();
  }

  private crearForm() {
    this.form = this.fb.group({
      seleccionarTodos: [this.todosSeleccionados()],
      formularios: this.fb.array((this.formularios || []).map((formulario) => {
          let seleccionadoFc = new FormControl(this.checkearFormulario(formulario.idFormulario));
          let idFormularioFc = new FormControl(formulario.idFormulario);
          seleccionadoFc.valueChanges.subscribe((check) => {
            let idAgregado = this.idsFormulariosSeleccionados.find((id) => id === idFormularioFc.value);
            if (check && !idAgregado) {
              this.idsFormulariosSeleccionados.push(idFormularioFc.value);
              this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);
              this.fechaInicioPagos.emit(this.obtenerFechaInicioPagos());
            } else if (idAgregado) {
              let indice = this.idsFormulariosSeleccionados.indexOf(idAgregado);
              this.idsFormulariosSeleccionados.splice(indice, 1);
              this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);
              this.fechaInicioPagos.emit(this.obtenerFechaInicioPagos());
            }
            this.form.get('seleccionarTodos').setValue(this.todosSeleccionados());
          });

          return this.fb.group({
            idFormulario: idFormularioFc,
            cuilDni: [formulario.cuilDni],
            apellidoNombre: [formulario.apellidoNombre],
            fechFinPago: [formulario.fechFinPago],
            nombreBanco: [formulario.nombreBanco],
            nombreSucursal: [formulario.nombreSucursal],
            nombreLinea: [formulario.nombreLinea],
            nroPrestamo: [formulario.nroPrestamo],
            nroFormulario: [formulario.nroFormulario],
            nombreDepartamento: [formulario.nombreDepartamento],
            nombreLocalidad: [formulario.nombreLocalidad],
            seleccionado: seleccionadoFc,
            montoPrestado: [formulario.monPres],
            cantidadCuotas: [formulario.canCuot],
            puedeCrearPlan: [formulario.puedeCrearPlan]
          });
        }
      ))
    });
  }

  private checkearFormulario(idFormulario: number): boolean {
    return !!this.idsFormulariosSeleccionados.find((id) => id === idFormulario);
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this._filtros.numeroPagina = params.numeroPagina;
        return this.pagosService.buscarFormulariosEnLoteFiltros(this._filtros);
      }).share();
    (<Observable<FormularioSeleccionado[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((formularios) => {
        this.formularios = formularios;
        this.crearForm();
      });
  }

  public consultarSiguientesFormularios(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public get formulariosFa(): FormArray {
    return this.form.get('formularios') as FormArray;
  }

  public todosSeleccionados(): boolean {
    if (!this.creaPlan) {
      return this.cantidadFormulariosTotalVisualizar !== 0 && this.idsFormulariosSeleccionados.length === this.cantidadFormulariosTotalVisualizar;
    } else {
      return this.cantidadFormulariosTotalActualizar !== 0 && this.idsFormulariosSeleccionados.length === this.cantidadFormulariosTotalActualizar;
    }
  }

  public seleccionarTodos(): void {
    this.idsFormulariosSeleccionados = this.form.get('seleccionarTodos').value ? [] : !this.creaPlan ? this.idsFormulariosTotalVisualizar.concat([]) : this.idsFormulariosTotalActualizar.concat([]);
    this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);
    this.fechaInicioPagos.emit(this.obtenerFechaInicioPagos());
    this.crearForm();
  }

  public ningunoHabilitado(): boolean {
    if (this.creaPlan) {
      if (this.cantidadFormulariosTotalActualizar > 0){
        return null;
      }
      return true;
    } else {
      return null;
    }
  }

  private consultarTodosFormulariosFiltrados(): void {
    this.limpiarGrilla();
    this.pagosService.buscarIdsFormulariosEnLoteFiltros(this._filtros)
      .subscribe((formularios) => {
        this.idsFormulariosTotalVisualizar = formularios.map((formulario) => formulario.idFormulario);
        this.cantidadFormulariosTotalVisualizar = this.idsFormulariosTotalVisualizar.length;
        let formulariosFiltradosActualizar = formularios.filter((formulario) => formulario.puedeCrearPlan);
        this.idsFormulariosTotalActualizar = formulariosFiltradosActualizar.map((formulario) => formulario.idFormulario);
        this.cantidadFormulariosTotalActualizar = this.idsFormulariosTotalActualizar.length;
      });
  }

  private obtenerFechaInicioPagos(): Date {
    for (let i = 0; i < this.formularios.length; i++) {
      if (this.formularios[i].fechFinPago) {
        return this.formularios[i].fechFinPago;
      }
      if (i === (this.formularios.length - 1)) {
        return new Date(1900, 0, 1);
      }
    }
  }

  public deseleccionarTodos() {
    this.idsFormulariosSeleccionados = [];
    this.formulariosSeleccionados.emit(this.idsFormulariosSeleccionados);
    this.fechaInicioPagos.emit(this.obtenerFechaInicioPagos());
    this.crearForm();
  }

  public deshabilitarFormulariosNoPermitidos() {
    this.creaPlan = true;
  }

  public habilitarFormulariosNoPermitidos() {
    this.creaPlan = false;
  }

  private deshabilitarCheck(formPuedeCrear: boolean): boolean {
    if (this.creaPlan) {
      if (formPuedeCrear) {
        return null;
      } else {
        return true;
      }
    } else {
      return null;
    }
  }

  public limpiarGrilla(): void {
    this.idsFormulariosSeleccionados = [];
    this.formulariosSeleccionados.emit(Object.assign([], this.idsFormulariosSeleccionados));
    this.cantidadFormulariosTotalActualizar = 0;
    this.cantidadFormulariosTotalVisualizar = 0;
    this.formularios = [];
    this.crearForm();
  }
}
