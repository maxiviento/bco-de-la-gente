import { Component, OnInit } from '@angular/core';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { LineaPrestamo } from '../shared/modelo/linea-prestamo.model';
import { DetalleLineaPrestamo } from '../shared/modelo/detalle-linea-prestamo.model';
import { ActivatedRoute, Params } from '@angular/router';
import { SexoDestinatario } from '../shared/modelo/destinatario-prestamo.model';
import { MotivoDestino } from '../../motivo-destino/shared/modelo/motivo-destino.model';
import { ConsultaRequisitosModal } from '../shared/requisitos/consulta-requisitos/consulta-requisitos.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LineaArchivosModal } from '../shared/linea-archivos-modal/linea-archivos.modal';
import { NotificacionService } from '../../shared/notificacion.service';
import { Localidad } from '../../shared/domicilio-linea/localidad.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import { ConsultaOngLineaModal } from '../shared/linea-ong/consulta-ong-linea/consulta-ong-linea.component';

@Component({
  selector: 'bg-consulta-linea',
  templateUrl: './consulta-linea.component.html',
  styleUrls: ['./consulta-linea.component.scss']
})

export class ConsultaLineaComponent implements OnInit {

  public lineaPrestamo: LineaPrestamo = new LineaPrestamo();
  public detalleLinea: DetalleLineaPrestamo[] = [];
  public idLineaUrl: any;
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public localidades: Localidad[] = [];
  public deptoLocalidad: boolean = false;
  public esEditable: boolean = false;

  constructor(private route: ActivatedRoute,
              private lineaService: LineaService,
              private modalService: NgbModal,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Ver línea de préstamo ' + TituloBanco.TITULO);

    this.lineaPrestamo.sexoDestinatario = new SexoDestinatario();
    this.lineaPrestamo.motivoDestino = new MotivoDestino();

  }

  public ngOnInit() {
    this.buscarIdUrl();
    this.consultarLinea();
    this.consultarDetalle();
  }

  private consultarLinea() {
    this.lineaService.consultarLineaPorId(this.idLineaUrl)
      .subscribe((resultado) => {
        if (resultado) {
          this.lineaPrestamo = resultado;
          if (this.lineaPrestamo.deptoLocalidad) {
            this.consultarLocalidades();
            this.deptoLocalidad = true;
          }
          if (this.lineaPrestamo.conOng) {
            this.consultarOngLinea();
          }
        }
      }, (errores) => {
        if (errores) {
          this.notificacionService.informar(errores, true);
        }
      });
  }

  private consultarLocalidades() {
    this.lineaService.consultarLocalidadesLineaPorId(this.idLineaUrl)
      .subscribe((resultado) => {
        this.localidades = resultado;
      }, (errores) =>
        this.notificacionService.informar(errores, true));
  }

  private buscarIdUrl() {

    this.route.params.subscribe((param: Params) => {
      this.idLineaUrl = param['idLinea'];
    });

  }

  private consultarDetalle() {
    this.lineaService.consultarDetallePorIdLineaSinPaginar(this.idLineaUrl)
      .subscribe((resultado) => {
          if (resultado) {
            this.detalleLinea = resultado;
          }
        }, (errores) => {
          if (errores) {
            this.notificacionService.informar(errores, true);
          }
        }
      );
  }

  public mostrarRequisitos() {
    const modal = this.modalService.open(ConsultaRequisitosModal, {size: 'lg'});
    modal.componentInstance.idLinea = this.idLineaUrl;
    modal.componentInstance.nombreLinea = this.lineaPrestamo.nombre;
  }

  public mostrarArchivos() {
    const modal = this.modalService.open(LineaArchivosModal, {size: 'lg'});
    modal.componentInstance.pathLogo = this.lineaPrestamo.logo;
    modal.componentInstance.pathPiePagina = this.lineaPrestamo.piePagina;
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades;
  }

  public consultarOngLinea() {
    this.lineaService.consultarOngPorLinea(this.idLineaUrl)
      .subscribe((resultado) => {
        this.lineaPrestamo.lsOng = resultado;
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  public mostrarOng() {
    const modal = this.modalService.open(ConsultaOngLineaModal, {size: 'lg'});
    modal.componentInstance.lsOngLinea = this.lineaPrestamo.lsOng;
    modal.componentInstance.nombreLinea = this.lineaPrestamo.nombre;
  }
}
