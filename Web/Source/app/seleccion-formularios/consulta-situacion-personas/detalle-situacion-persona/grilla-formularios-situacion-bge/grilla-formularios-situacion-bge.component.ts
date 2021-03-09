import { FormBuilder, FormGroup } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { ConsultaSituacionPersonas } from '../../../shared/modelos/consulta-situacion-personas.model';
import { SituacionPersonaService } from '../../../shared/situacion-persona.service';
import { FormulariosSituacionResultado } from '../../../shared/modelos/formularios-situacion-resultado.model';
import { ELEMENTOS, Pagina } from '../../../../shared/paginacion/pagina-utils';
import { SituacionPersonasResultado } from '../../../shared/modelos/situacion-personas-resultado.model';
import { MotivoRechazoReferencia } from '../../../../shared/modelo/motivo-rechazo-referencia.model';
import { MotivoRechazoReferenciaConsulta } from '../../../../shared/modelo/consultas/motivo-rechazo-referencia-consulta.model';
import { PagosService } from '../../../../pagos/shared/pagos.service';
import { DetallesPlanPagosConsulta } from '../../../../pagos/shared/modelo/detalles-plan-pagos-consulta.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalPlanPagosComponent } from './modal-plan-pagos/modal-plan-pagos.component';
import { ModalVerMotivosRechazoComponent } from './modal-ver-motivos-rechazo/modal-ver-motivos-rechazo.component';
import { ModalVerHistorialComponent } from './modal-ver-historial/modal-ver-historial.component';
import EstadosFormulario from '../../../../formularios/shared/modelo/estados-formulario.enum';
import EstadosPrestamo from '../../../../formularios/shared/modelo/estado-prestamo.enum';

@Component({
    selector: 'bg-grilla-formularios-situacion-bge',
    templateUrl: './grilla-formularios-situacion-bge.component.html',
    styleUrls: ['./grilla-formularios-situacion-bge.component.scss'],
    providers: [SituacionPersonaService]
  }
)
export class GrillaFormulariosSituacionBgeComponent implements OnInit {

  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public formulariosSituacion: FormulariosSituacionResultado [] = [];
  public form: FormGroup;
  private _filtros: ConsultaSituacionPersonas;
  public mensajeResultado: string;
  public estadosFormulario = EstadosFormulario;
  public estadosPrestamo = EstadosPrestamo;

  @Input()
  public set filtros(filtros: SituacionPersonasResultado) {
    this._filtros = new ConsultaSituacionPersonas(4, filtros.cuil, filtros.apellido, filtros.nombre, filtros.nroDocumento);
    this.formulariosSituacion = [];
    this.paginaModificada.next(0);
  }

  @Output()
  public emitirMotivos: EventEmitter<MotivoRechazoReferencia[]> = new EventEmitter<MotivoRechazoReferencia[]>();

  constructor(private fb: FormBuilder,
              private situacionPersonaService: SituacionPersonaService,
              private modalService: NgbModal,
              private pagosService: PagosService) {
    this.configurarPaginacion();
  }

  public ngOnInit() {
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this._filtros.numeroPagina = params.numeroPagina;
        this.mensajeResultado = 'Aguarde...';
        return this.situacionPersonaService.consultarFormulariosSituacionPersona(this._filtros);
      }).share();
    (<Observable<FormulariosSituacionResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((formulariosSituacion) => {
        this.formulariosSituacion = formulariosSituacion;
        this.consultarMotivosRechazo();
        if (this.formulariosSituacion.length === 0) {
          this.mensajeResultado = 'No hay formularios asignados a esta persona.';
        }
      }, () => {
        this.mensajeResultado = 'Falló la consulta de formularios para esta persona.';
      });
  }

  public paginaSiguiente(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public armarConsulta(): MotivoRechazoReferenciaConsulta {
    let conjunto: string = '';
    let conjuntoNumerico: number[] = [];

    this.formulariosSituacion.forEach((actual) => {
      if (actual.idRechFrom.length) {
        let listaIdsMotivosRechazos = actual.idRechFrom.split(',');
        listaIdsMotivosRechazos.forEach((id) => {
          if (conjuntoNumerico.indexOf(+id) === -1) {
            conjuntoNumerico.push(+id);
          }
        });
      }
      if (actual.idRechPrest.length) {
        let listaIdsMotivosRechazos = actual.idRechPrest.split(',');
        listaIdsMotivosRechazos.forEach((id) => {
          if (conjuntoNumerico.indexOf(+id) === -1) {
            conjuntoNumerico.push(+id);
          }
        });
      }
    });
    conjuntoNumerico.forEach((id) => {
      conjunto = conjunto.concat(id.toString()).concat(';');
    });
    return new MotivoRechazoReferenciaConsulta(conjunto);
  }

  public consultarMotivosRechazo() {
    this.situacionPersonaService.consultarMotivosRechazo(
      this.armarConsulta())
      .subscribe(
        (resultado) => {
          this.emitirMotivos.emit(resultado);
        }
      );
  }

  public consultarSeguimientos(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public consultarPlanDePagos(numeroFormulario?: number) {
    this.pagosService
      .obtenerDetallesPlanDePagos(this.prepararConsultaPlanDePagos(numeroFormulario))
      .subscribe((plan) => {
        this.mostrarModalPlanPagos(plan, numeroFormulario);
      });
  }

  private prepararConsultaPlanDePagos(numeroDeFormulario: number): DetallesPlanPagosConsulta {
    let consultaDeUnSoloFormulario = [];
    consultaDeUnSoloFormulario.push(numeroDeFormulario);
    return new DetallesPlanPagosConsulta(consultaDeUnSoloFormulario);
  }

  private mostrarModalPlanPagos(plan?: any, idFormulario?: number): void {
    const modalPagos = this.modalService.open(ModalPlanPagosComponent,
      {backdrop: 'static', windowClass: 'modal-xl'});
    modalPagos.componentInstance.planes = plan;
    modalPagos.componentInstance.idsFormularios = [idFormulario];
  }

  private mostrarModalMotivosRechazo(idFormulario: number, idPrestamo: number, numeroCaja: string): void {
    const modalMotivosRechazo = this.modalService.open(ModalVerMotivosRechazoComponent,
      {backdrop: 'static', windowClass: 'modal-l'});
    modalMotivosRechazo.componentInstance.idFormulario = idFormulario;
    modalMotivosRechazo.componentInstance.idPrestamo = idPrestamo;
    modalMotivosRechazo.componentInstance.numeroCaja = numeroCaja;
  }

  private mostrarModalVerHistorial(idPrestamo: number): void {
    const modalMotivosRechazo = this.modalService.open(ModalVerHistorialComponent,
      {backdrop: 'static', windowClass: 'modal-l'});
    modalMotivosRechazo.componentInstance.idPrestamo = idPrestamo;
  }
}
