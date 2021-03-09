import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';
import { FiltrosFormularioConsulta } from '../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { ActualizarPlanPagosComponent } from './actualizar-plan-pagos/actualizar-plan-pagos.component';
import { FiltrosFormulariosPlanPagosComponent } from './filtros-formularios-plan-pagos/filtros-formularios-plan-pagos.component';
import { GrillaFormulariosPlanPagosComponent } from './grilla-formularios-plan-pagos/grilla-formularios-plan-pagos.component';
import { VisualizarPlanPagosComponent } from './visualizar-plan-pagos/visualizar-plan-pagos.component';
import { OrigenPeticion } from '../shared/modelo/origen-peticion.enum';
import { FormBuilder, FormGroup } from "@angular/forms";
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-plan-pagos',
  templateUrl: './plan-pagos.component.html',
  styleUrls: ['./plan-pagos.component.scss']
})

export class PlanPagosComponent implements OnInit {
  public form: FormGroup;
  public filtrosBusqueda: FiltrosFormularioConsulta;
  public mostrarFiltros: boolean = false;
  public mostrarCuotasPlanPagos: boolean = false;
  public limpiarIdsSeleccionados: boolean = false;
  private idLote: number;
  private idFormularioLinea: number;
  public idsFormularios: number [] = [];
  public fechaInicioPagos: Date;
  public verPlan: boolean = false;
  public crearPlan: boolean = false;
  public deshabilitarCreacion: boolean = true;
  public origenPeticion: OrigenPeticion = OrigenPeticion.MENU;
  public quiereEditarPlan: boolean = false;

  @ViewChild(ActualizarPlanPagosComponent)
  public apartadoPlanPagos: ActualizarPlanPagosComponent;

  @ViewChild(VisualizarPlanPagosComponent)
  public apartadoVerPlanPagos: VisualizarPlanPagosComponent;

  @ViewChild(FiltrosFormulariosPlanPagosComponent)
  public apartadoFiltros: FiltrosFormulariosPlanPagosComponent;

  @ViewChild(GrillaFormulariosPlanPagosComponent)
  public apartadoGrillaFormularios: GrillaFormulariosPlanPagosComponent;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private fb: FormBuilder,
              private location: Location,
              private titleService: Title) {
    this.titleService.setTitle('Administrar plan de cuotas ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    /* Si no es proveniente de un préstamo, debo mostrar los filtros de búsqueda */
    this.mostrarFiltros = !this.router.url.includes('prestamo');

    if (this.router.url.includes('prestamo')) {
      this.origenPeticion = OrigenPeticion.PRESTAMO;
      this.mostrarCuotasPlanPagos = true;
      this.route.params.subscribe((params: Params) => {
        this.idFormularioLinea = +params['id'];
        this.buscarFormularios(new FiltrosFormularioConsulta());
      });
    } else if (this.router.url.includes('lote')) {
      this.origenPeticion = OrigenPeticion.LOTE;
      this.route.params.subscribe((params: Params) => {
        this.idLote = +params['id'];
        this.buscarFormularios(new FiltrosFormularioConsulta());
      });
    }
    this.crearForm();
    this.suscribirRadios();
  }

  private crearForm() {
    this.form = this.fb.group({
      radioPlan: [this.quiereEditarPlan]
    });
  }

  public buscarFormularios(filtrosBusqueda: FiltrosFormularioConsulta): void {
    this.resetearIdsFormulario(true);
    this.resetearPlanPagos(true);
    if (!this.mostrarComboLote()) {
      filtrosBusqueda.idLote = this.idLote || undefined;
      filtrosBusqueda.idFormularioLinea = this.idFormularioLinea || undefined;
    }
    this.filtrosBusqueda = filtrosBusqueda;
  }

  public almacenarFormularios(idsFormularios: number []): void {
    this.idsFormularios = idsFormularios;
    this.setBotonesPlanPagos();
  }

  public setFechaInicioPagos(fecha: Date): void {
    this.fechaInicioPagos = fecha;
  }

  public resetearDatos(limpiar: boolean): void {
    this.limpiarIdsSeleccionados = limpiar;
    this.filtrosBusqueda = Object.assign({}, this.filtrosBusqueda);
  }

  public resetearIdsFormulario(limpiar: boolean): void {
    this.limpiarIdsSeleccionados = limpiar;
  }

  public resetearPlanPagos(limpiar: boolean): void {
    if (this.apartadoPlanPagos && limpiar) {
      this.apartadoPlanPagos.limpiarPlanDePagos();
    }
    if (this.apartadoVerPlanPagos && limpiar) {
      this.apartadoVerPlanPagos.limpiarPlanDePagos();
    }
    this.setBotonesPlanPagos();
  }

  public volver(): void {
    if (this.router.url.includes('prestamo')) {
      // this.router.navigate(['actualizar-checklist', this.idPrestamo]);
      window.close();
    } else if (this.router.url.includes('lote')) {
      this.router.navigate(['bandeja-lotes']);
    } else {
      if (this.mostrarFiltros) {
        this.idsFormularios = [];
        this.apartadoGrillaFormularios.limpiarGrilla();
        this.apartadoFiltros.limpiarFiltros();
      } else {
        this.location.back();
      }
    }
  }

  private suscribirRadios(): void {
    this.form.get('radioPlan').valueChanges.distinctUntilChanged()
      .subscribe((value) => {
          this.apartadoGrillaFormularios.deseleccionarTodos();
          if (!value) {
            //Entra cuando selecciono Visualizar
            this.deshabilitarCreacion = true;
            this.apartadoGrillaFormularios.habilitarFormulariosNoPermitidos();
          } else {
            //Entra cuando selecciono Actualizar
            this.deshabilitarCreacion = false;
            this.apartadoGrillaFormularios.deshabilitarFormulariosNoPermitidos();
          }
        }
      );
  }

  public mostrarComboLote(): boolean {
    if (this.idLote || this.idFormularioLinea) {
      return false;
    }
    return true;
  }

  public setBotonesPlanPagos(): void {
    this.verPlan = false;
    this.crearPlan = false;
  }

  public crearPlanPagos(): void {
    this.verPlan = false;
    this.crearPlan = true;
  }

  public verPlanPagos(): void {
    this.crearPlan = false;
    this.verPlan = true;
  }
}
