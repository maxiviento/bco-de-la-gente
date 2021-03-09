import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { DetallesPlanPagosConsulta } from '../../shared/modelo/detalles-plan-pagos-consulta.model';
import { GrillaPlanPagosComponent } from '../actualizar-plan-pagos/grilla-plan-pagos/grilla-plan-pagos.component';

@Component({
  selector: 'bg-visualizar-plan-pagos',
  templateUrl: 'visualizar-plan-pagos.component.html',
  styleUrls: ['visualizar-plan-pagos.component.scss']
})

export class VisualizarPlanPagosComponent implements OnInit {
  public filtros: FiltrosFormularioConsulta;
  public sinDetalles: boolean = true;
  @Input() public idLote: number;
  @Input() public idPrestamo: number;
  @Input() public idsFormularios: number [] = [];
  @Output() public limpiarIdsSeleccionados: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() public clickVolver: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild(GrillaPlanPagosComponent)
  public componenteDetalles: GrillaPlanPagosComponent;

  constructor(private pagosService: PagosService,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit() {
    this.obtenerDetallesPlanPagosFormulario();
  }

  public obtenerDetallesPlanPagosFormulario() {
    if (this.idsFormularios.length) {
      this.pagosService
        .obtenerDetallesPlanDePagos(this.prepararConsultaDetallesPlanPagos())
        .subscribe((planes) => {
          if (planes && planes.length) {
            this.sinDetalles = false;
            this.componenteDetalles.planes = planes;
            this.componenteDetalles.nuevoPlan();
          } else {
            this.sinDetalles = true;
            this.componenteDetalles.limpiarGrilla();
            this.notificacionService.informar(['No tiene un plan de cuotas generado.']);
          }
        });
    }
  }

  private prepararConsultaDetallesPlanPagos(): DetallesPlanPagosConsulta {
    return new DetallesPlanPagosConsulta(this.idsFormularios);
  }

  public limpiarPlanDePagos() {
    this.componenteDetalles.limpiarGrilla();
  }

  public volver(): void {
    this.clickVolver.emit();
  }
}
