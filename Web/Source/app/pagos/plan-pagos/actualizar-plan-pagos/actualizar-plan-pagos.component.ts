import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { GrillaPlanPagosComponent } from './grilla-plan-pagos/grilla-plan-pagos.component';
import { FiltrosFormularioConsulta } from '../../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { GenerarPlanPagosComando } from '../../shared/modelo/generar-plan-pagos-comando.model';
import { DetallesPlanPagosConsulta } from '../../shared/modelo/detalles-plan-pagos-consulta.model';

@Component({
  selector: 'bg-actualizar-plan-pagos',
  templateUrl: 'actualizar-plan-pagos.component.html',
  styleUrls: ['actualizar-plan-pagos.component.scss']
})

export class ActualizarPlanPagosComponent implements OnInit {
  public filtros: FiltrosFormularioConsulta;

  @Input() public idLote: number;
  @Input() public idPrestamo: number;
  @Input() public idsFormularios: number [] = [];
  @Output() public limpiarIdsSeleccionados: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() public clickVolver: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() public mostrarCuotas: boolean = false;
  @Input() public fechaPago: Date;
  public form: FormGroup;

  @ViewChild(GrillaPlanPagosComponent)
  public componenteDetalles: GrillaPlanPagosComponent;

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit() {
    this.crearForm();
    if (this.idPrestamo && this.idsFormularios.length) {
      this.obtenerDetallesPlanPagosFormulario(this.prepararConsultaDetallesPlanPagos());
    }
  }

  public crearForm() {
    this.form = this.fb.group({
      mesesGracia: ['', Validators.compose([Validators.required,
        CustomValidators.number,
        Validators.min(0),
      Validators.maxLength(2)])],
      fechaPago: [NgbUtils.obtenerNgbDateStruct(this.fechaPago), Validators.required]
    });
  }

  public actualizarPlanPagos(): void {
    this.pagosService
      .actualizarPlanDePagos(this.prepararGeneracionPlanPagos())
      .subscribe((ok) => {
        if (ok) {
          this.obtenerDetallesPlanPagosFormulario(this.prepararConsultaDetallesPlanPagos());
          this.mostrarCuotas = true;
        }
      },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  public obtenerDetallesPlanPagosFormulario(consulta: DetallesPlanPagosConsulta) {
    this.pagosService
      .obtenerDetallesPlanDePagos(consulta)
      .subscribe((planes) => {
        this.componenteDetalles.inicializarCuotas(planes);
      });
  }

  private prepararGeneracionPlanPagos(): GenerarPlanPagosComando {
    let formModel = this.form.value;
    return new GenerarPlanPagosComando(
      formModel.mesesGracia,
      null,
      this.idsFormularios
    );
  }

  private prepararConsultaDetallesPlanPagos(): DetallesPlanPagosConsulta {
    return new DetallesPlanPagosConsulta(this.idsFormularios);
  }

  public limpiarPlanDePagos() {
    if (this.componenteDetalles) {
      this.componenteDetalles.limpiarGrilla();
    }
  }

  public volver(): void {
    this.clickVolver.emit();
  }
}
