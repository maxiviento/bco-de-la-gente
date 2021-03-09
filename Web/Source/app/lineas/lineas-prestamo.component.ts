import { CustomValidators } from './../shared/forms/custom-validators';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificacionService } from '../shared/notificacion.service';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { MotivoDestinoService } from '../motivo-destino/shared/motivo-destino.service';
import { DestinatarioService } from './shared/destinatario.service';
import { LineaPrestamo } from './shared/modelo/linea-prestamo.model';
import { SexoDestinatario } from './shared/modelo/destinatario-prestamo.model';
import { MotivoDestino } from '../motivo-destino/shared/modelo/motivo-destino.model';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { DetalleLineaPrestamo } from './shared/modelo/detalle-linea-prestamo.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConsultaRequisitosModal } from './shared/requisitos/consulta-requisitos/consulta-requisitos.component';
import { ModalDetalleLineaComponent } from './modal-detalle-linea/modal-detalle-linea.component';
import { LineaDetalleComponent } from './shared/linea-detalle/linea-detalle.component';
import { TipoInteres } from './shared/modelo/tipo-interes.model';
import { TipoFinanciamiento } from './shared/modelo/tipo-financiamiento.model';
import { IntegranteSocio } from './shared/modelo/integrante-socio.model';
import { TipoGarantia } from './shared/modelo/tipo-garantia.model';
import { LineaCombo } from '../formularios/shared/modelo/linea-combo.model';
import { Convenio } from '../shared/modelo/convenio-model';
import { LineaConsulta } from './consulta-linea/linea-consulta.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-lineas-prestamo',
  templateUrl: './lineas-prestamo.component.html',
  styleUrls: ['./lineas-prestamo.component.scss']
})

export class LineasPrestamoComponent implements OnInit, OnDestroy {

  public form: FormGroup;
  public lineaConsulta: LineaConsulta;
  public detalleConsulta: DetalleLineaPrestamo;
  public lineasResultado: LineaPrestamo[] = [];
  public detallesResultado: DetalleLineaPrestamo[] = [];
  public destinatarios: SexoDestinatario[] = [];
  public motivosDestino: MotivoDestino[] = [];
  public convenios: Convenio[] = [];
  public conveniosPago: Convenio[] = [];
  public conveniosRecupero: Convenio[] = [];
  public paginaLinea: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaLineaModificada = new Subject<number>();
  public paginaDetalle: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaDetalleModificada = new Subject<number>();
  public detalle: DetalleLineaPrestamo;
  public mostrarDetalles: boolean;
  public lineaActual: LineaPrestamo = new LineaPrestamo();
  public nombreLineasObject: LineaCombo[] = [];
  public nombreLineas: string[] = [];
  public dadosDeBajaCheckeado: boolean = false;
  public filaSeleccionada: number;

  constructor(private lineaService: LineaService,
              private notificacionService: NotificacionService,
              private router: Router,
              private motivoDestinoService: MotivoDestinoService,
              private destinatarioService: DestinatarioService,
              private modalService: NgbModal,
              private titleService: Title) {
    this.titleService.setTitle('Configuración de líneas ' + TituloBanco.TITULO);

    if (!this.lineaConsulta) {
      this.lineaConsulta = new LineaConsulta();
    }

    if (!this.detalleConsulta) {
      this.detalleConsulta = new DetalleLineaPrestamo();
    }
  }

  public ngOnInit() {
    this.obtenerNombresLineas();
    this.crearForm();
    this.cargarCombos();
    this.configurarPaginacionLinea();
    this.configurarPaginacionDetalle();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!((this.router.url.includes('bandeja-lineas'))
      || (this.router.url.includes('nueva-linea'))
      || (this.router.url.includes('edicion-linea'))
      || (this.router.url.includes('consulta-linea'))
      || (this.router.url.includes('eliminacion-linea'))
      || (this.router.url.includes('edicion-detalle-linea'))
      || (this.router.url.includes('eliminacion-detalle-linea')))) {
      LineaService.guardarFiltros(null);
    }
  }

  private crearForm() {
    this.form = new FormGroup({
      nombre: new FormControl(this.lineaConsulta.nombre,
        Validators.compose([
          Validators.maxLength(100),
          CustomValidators.validTextAndNumbers])),
      conOng: new FormControl(this.lineaConsulta.conOng),
      conPrograma: new FormControl(this.lineaConsulta.conPrograma),
      conDepartamento: new FormControl(this.lineaConsulta.conDepartamento),
      idDestinatario: new FormControl(this.lineaConsulta.idDestinatario),
      idMotivoDestino: new FormControl(this.lineaConsulta.idMotivoDestino),
      detallesDadosBaja: new FormControl(this.lineaConsulta.detallesDadosBaja),
      dadosBaja: new FormControl(this.lineaConsulta.dadosBaja),
      modalDetalle: new FormControl(''),
      idMotivoBaja: new FormControl(this.lineaConsulta.idMotivoBaja),
      idConvenioPago: new FormControl(this.lineaConsulta.idConvenioPago),
      idConvenioRecupero: new FormControl(this.lineaConsulta.idConvenioRecupero)
    });

    this.form.get('detallesDadosBaja')
      .valueChanges
      .subscribe((value) => {
        this.lineaConsulta.detallesDadosBaja = value;
      });
  }

  private cargarCombos() {
    this.motivoDestinoService.consultarMotivosDestino()
      .subscribe((resultado) => this.motivosDestino = resultado);
    this.destinatarioService.consultarDestinatarios()
      .subscribe((resultado) => this.destinatarios = resultado);
    this.lineaService.consultarConvenios()
      .subscribe((resultado) => {
        this.convenios = resultado;
        this.filtrarConvenios();
      });
  }

  private filtrarConvenios() {
    this.conveniosPago = this.convenios.filter((c) => c.idTipoConvenio === 1);
    this.conveniosRecupero = this.convenios.filter((c) => c.idTipoConvenio === 2);
  }

  private configurarPaginacionLinea() {
    let filtros;
    this.paginaLinea = this.paginaLineaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsulta();
        filtros = this.lineaConsulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.lineaService
          .consultarLineasPorFiltros(filtros);
      }).share();
    (<Observable<LineaPrestamo[]>> this.paginaLinea.pluck(ELEMENTOS))
      .subscribe((lineasResultado) => {
        this.lineasResultado = lineasResultado;
        LineaService.guardarFiltros(filtros);
      });
  }

  private configurarPaginacionDetalle() {
    this.paginaDetalle = this.paginaDetalleModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        let filtros = this.detalleConsulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.lineaService
          .consultarDetalleLineaPorIdLinea(filtros);
      }).share();
    (<Observable<DetalleLineaPrestamo[]>> this.paginaDetalle.pluck(ELEMENTOS))
      .subscribe((detallesResultado) => {
        this.detallesResultado = detallesResultado;
      });
  }

  private obtenerNombresLineas() {
    this.lineaService.consultarNombresLineas().subscribe((lineas) => {
      lineas.forEach((x) => this.nombreLineasObject.push(x));
      this.cargarNombreLineas(false);
    });
  }

  public searchLinea = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .distinctUntilChanged()
      .map((term) => term.length < 2 ? []
        : this.nombreLineas.filter((linea) =>
          linea.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));
  public showLinea = (linea: any) => linea;

  public clickDadosDeBaja(checkeado: boolean) {
    this.cargarNombreLineas(checkeado);
  }

  private cargarNombreLineas(incluirDadasDeBaja: boolean) {
    this.nombreLineas = [];
    if (incluirDadasDeBaja) {
      this.nombreLineasObject.forEach((x) => this.nombreLineas.push(x.descripcion));
    } else {
      this.nombreLineasObject.filter((x) => !x.dadoDeBaja).forEach((y) => this.nombreLineas.push(y.descripcion));
    }
  }

  public verDetalle(idLinea?: number, pagina?: number, linea?: LineaPrestamo) {
    this.filaSeleccionada = idLinea;
    this.form.patchValue({idMotivoBaja: linea ? linea.idMotivoBaja : 0});

    this.detalleConsulta.lineaId = idLinea;
    LineaService.recuperarFiltros().id = idLinea;
    this.paginaDetalleModificada.next(pagina);
    this.mostrarDetalles = true;
    this.form.get('detallesDadosBaja').setValue(false);
  }

  private prepararConsulta() {
    this.detallesResultado = [];
    let formModel = this.form.value;
    this.lineaConsulta.nombre = formModel.nombre;
    this.lineaConsulta.conOng = formModel.conOng;
    this.lineaConsulta.conPrograma = formModel.conPrograma;
    this.lineaConsulta.conDepartamento = formModel.conDepartamento;

    this.lineaConsulta.idDestinatario = formModel.idDestinatario;
    this.lineaConsulta.idMotivoDestino = formModel.idMotivoDestino;
    this.lineaConsulta.dadosBaja = formModel.dadosBaja;
    this.lineaConsulta.idMotivoBaja = formModel.idMotivoBaja;
    this.lineaConsulta.idConvenioPago = formModel.idConvenioPago;
    this.lineaConsulta.idConvenioRecupero = formModel.idConvenioRecupero;
  }

  public consultarLineasPrestamo(pagina?: number) {
    this.mostrarDetalles = false;
    this.prepararConsulta();
    this.paginaLineaModificada.next(pagina);
  }

  public mostrarRequisitos(idLinea: number, nombre: string) {
    const modal = this.modalService.open(ConsultaRequisitosModal, {size: 'lg'});
    modal.componentInstance.idLinea = idLinea;
    modal.componentInstance.nombreLinea = nombre;
  }

  public hayDetallesDadosBaja(): boolean {
    return this.detallesResultado.some((detalle) => detalle.fechaBaja !== null);
  }

  public editarDetalle(idDetalle: number): void {


    this.lineaService.consultarDetalleLineaPorIdDetalle(idDetalle)
      .subscribe((detalle) => {

        /*this.filaSeleccionada = idLinea;
        this.form.patchValue({idMotivoBaja: linea ? linea.idMotivoBaja : 0});

        this.detalleConsulta.lineaId = idLinea;
        LineaService.recuperarFiltros().id = idLinea;
        this.paginaDetalleModificada.next(pagina);
        this.mostrarDetalles = true;
        this.form.get('detallesDadosBaja').setValue(false);*/

        this.detalle = detalle;
        this.detalle.tipoInteres = new TipoInteres(detalle.idTipoInteres, detalle.nombreTipoInteres);
        this.detalle.tipoFinanciamiento = new TipoFinanciamiento(detalle.idTipoFinanciamiento, detalle.nombreTipoFinanciamiento);
        this.detalle.integranteSocio = new IntegranteSocio(detalle.idSocioIntegrante, detalle.nombreSocioIntegrante);
        this.detalle.tipoGarantia = new TipoGarantia(detalle.idTipoGarantia, detalle.nombreTipoGarantia);
        this.detalle.convenioPago = new Convenio(detalle.codConvenioPag, detalle.nombreConvPag, 1);
        this.detalle.convenioRecupero = new Convenio(detalle.codConvenioRec, detalle.nombreConvRec, 2);
        const modalRef = this.modalService.open(ModalDetalleLineaComponent, {windowClass: 'modal-xl', backdrop: 'static'});

        this.form.controls['modalDetalle'] = LineaDetalleComponent.nuevoFormGroup(this.detalle);
        modalRef.componentInstance.detalleForm = this.form.controls['modalDetalle'];
        modalRef.componentInstance.esEdicion = true;
        modalRef.result.then((detalleModal) => {
          if (detalleModal) {
            detalleModal.id = idDetalle;
            detalleModal.lineaId = this.detalleConsulta.lineaId;
            this.lineaService.modificarDetalle(detalleModal)
              .subscribe(() => {
                this.notificacionService
                  .informar(['El detalle de la línea de préstamo se modificó con éxito.'])
                  .result
                  .then(() => {
                    this.verDetalle(this.detalleConsulta.lineaId, null);
                  });
              }, (errores) => this.notificacionService.informar(errores, true));
          }
        });
      });
  }

  public nuevoDetalle(): void {
    const modalRef = this.modalService.open(ModalDetalleLineaComponent, {backdrop: 'static', windowClass: 'modal-xl'});
    this.form.controls['modalDetalle'] = LineaDetalleComponent.nuevoFormGroup(new DetalleLineaPrestamo());
    modalRef.componentInstance.detalleForm = this.form.controls['modalDetalle'];
    modalRef.componentInstance.esEdicion = false;
    modalRef.result.then((detalleModal) => {
      if (detalleModal) {
        detalleModal.lineaId = this.detalleConsulta.lineaId;
        this.lineaService.modificarDetalle(detalleModal)
          .subscribe(() => {
            this.notificacionService
              .informar(['El detalle de la línea de préstamo se agregó con éxito.'])
              .result
              .then(() => {
                this.verDetalle(this.detalleConsulta.lineaId, null);
              });
          }, (errores) => this.notificacionService.informar(errores, true));
      }
    });
  }

  private reestablecerFiltros() {
    let filtrosGuardados = LineaService.recuperarFiltros();
    if (filtrosGuardados) {
      this.form.patchValue({
        nombre: filtrosGuardados.nombre,
        conOng: filtrosGuardados.conOng,
        conPrograma: filtrosGuardados.conPrograma,
        conDepartamento: filtrosGuardados.conDepartamento,
        idDestinatario: filtrosGuardados.destinatarioId,
        idMotivoDestino: filtrosGuardados.motivoDestinoId,
        detallesDadosBaja: filtrosGuardados.detallesDadosBaja,
        dadosBaja: filtrosGuardados.dadosBaja,
        idMotivoBaja: filtrosGuardados.idMotivoBaja,
      });
      this.consultarLineasPrestamo();
      if (filtrosGuardados.id) {
        this.verDetalle(filtrosGuardados.id);
      }
    }
  }

  public filtrarDetallesDadosDeBaja(detalles: DetalleLineaPrestamo []): DetalleLineaPrestamo [] {
    if (this.form.get('detallesDadosBaja').value) {
      return detalles;
    } else {
      return detalles.filter((detalle) => !detalle.fechaBaja);
    }
  }

  public asignarLocalidades(idLinea: number, deptoLocalidad: boolean) {
    if (deptoLocalidad) {
      this.lineaService.consultarLocalidadesLineaPorId(idLinea)
        .subscribe((resultado) => {
          this.lineaService.asignarLocalidad(resultado);
        }, (errores) =>
          this.notificacionService.informar(errores, true));
    }
  }

  public validarBajaLinea(): boolean {
    if (this.lineaActual) {
      return this.lineaActual.idMotivoBaja != 0;
    }
    return false;
  }

  public validarConsulta(): boolean {
    return !this.form.valid;
  }
}
