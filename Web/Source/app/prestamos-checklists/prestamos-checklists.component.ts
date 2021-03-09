import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { NgbDatepickerConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbUtils } from '../shared/ngb/ngb-utils';
import { CustomValidators } from '../shared/forms/custom-validators';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { ConsultaBandejaPrestamos } from './shared/modelos/consulta-bandeja-prestamos.model';
import { BandejaPrestamoResultado } from './shared/modelos/bandeja-prestamo-resultado.model';
import { PrestamoService } from '../shared/servicios/prestamo.service';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { OrigenService } from '../formularios/shared/origen-prestamo.service';
import { Router } from '@angular/router';
import { ModalMotivoRechazoComponent } from '../formularios/modal-motivo-rechazo/modal-motivo-rechazo.component';
import { RechazarPrestamoComando } from './shared/modelos/rechazar-prestamo-comando.model';
import { NotificacionService } from '../shared/notificacion.service';
import { Departamento } from '../formularios/shared/modelo/departamento.model';
import { Localidad } from '../formularios/shared/modelo/localidad.model';
import { FormulariosService } from '../formularios/shared/formularios.service';
import { LocalidadComboServicio } from '../formularios/shared/localidad.service';
import { LineaCombo } from '../formularios/shared/modelo/linea-combo.model';
import { DateUtils } from '../shared/date-utils';
import { BusquedaPorPersonaComponent } from '../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { BusquedaPorPersonaConsulta } from '../shared/modelo/busqueda-por-persona-consulta.model';
import { ModalEditarNumeroCajaComponent } from '../formularios/modal-editar-numero-caja/modal-editar-numero-caja.component';
import { Title } from '@angular/platform-browser';
import { ColumnasPrestamoEnum } from '../formularios/shared/modelo/columnas-prestamo-enum.model';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ArchivoService } from '../shared/archivo.service';
import { FiltroDomicilioBandejaComponent } from '../shared/domicilio-bandeja/filtro-domicilio-bandeja.component';
import { MultipleSeleccionComponent } from '../shared/multiple-seleccion/multiple-seleccion.component';
import EstadosPrestamo from '../formularios/shared/modelo/estado-prestamo.enum';
import EstadosFormulario from '../formularios/shared/modelo/estados-formulario.enum';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-prestamos-checklists',
  templateUrl: './prestamos-checklists.component.html',
  styleUrls: ['./prestamos-checklists.component.scss']
})
export class PrestamosChecklistsComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public formularioConsulta: ConsultaBandejaPrestamos;
  public resultados: BandejaPrestamoResultado[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public estados = [];
  public origenes = [];
  public lineas: LineaCombo[] = [];
  public usuarios = [];
  public departamentos: Departamento[] = [];
  public CBLocalidad: Localidad[] = [];
  public columnasEnum = ColumnasPrestamoEnum;
  public orderByDes = true;
  public columnaOrderBy = -1;
  public totalizador: number = 0;
  public tipoConsulta: boolean = true;
  public pdfPagos = new BehaviorSubject<SafeResourceUrl>(null);
  public departamentoIds: string;
  public localidadIds: string;
  public estadosIds: string;
  public lineaIds: string;
  public estadosPrestamo = EstadosPrestamo;
  public estadosFormulario = EstadosFormulario;

  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;
  @ViewChild(MultipleSeleccionComponent)
  public comboEstado: MultipleSeleccionComponent;
  @ViewChild('lineas')
  public comboLinea: MultipleSeleccionComponent;
  @ViewChild(FiltroDomicilioBandejaComponent)
  public comboDptoLocalidad: FiltroDomicilioBandejaComponent;

  constructor(private fb: FormBuilder,
              private config: NgbDatepickerConfig,
              private prestamosService: PrestamoService,
              private lineasService: LineaService,
              private origenesService: OrigenService,
              private notificacionService: NotificacionService,
              private modalService: NgbModal,
              private formulariosService: FormulariosService,
              private localidadesService: LocalidadComboServicio,
              private router: Router,
              private archivoService: ArchivoService,
              private sanitizer: DomSanitizer,
              private titleService: Title) {
    this.titleService.setTitle('Bandeja de préstamos ' + TituloBanco.TITULO);
    if (!this.formularioConsulta) {
      this.formularioConsulta = new ConsultaBandejaPrestamos();
    }
  }

  public ngOnInit(): void {
    this.crearForm();
    this.cargarCombos();
    this.limiteFecha();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    if (!this.router.url.includes('prestamo') && !this.router.url.includes('checklist') && !this.router.url.includes('gestion-archivos')) {
      PrestamoService.guardarFiltros(null);
    }
  }

  private limiteFecha() {
    DateUtils.setMaxDateDP(new Date(), this.config);
  }

  private cargarCombos() {

    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => this.origenes = origenes);

    this.prestamosService
      .consultarUsuariosCombo()
      .subscribe((usuarios) => this.usuarios = usuarios);
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.formularioConsulta = this.prepararConsultaFormularios();
        this.formularioConsulta.numeroPagina = params.numeroPagina;
        return this.prestamosService.consultarBandejaPrestamo(this.formularioConsulta);
      })
      .share();
    (<Observable<BandejaPrestamoResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((formularios) => {
        this.resultados = formularios;
        PrestamoService.guardarFiltros(this.formularioConsulta);
      });
  }

  public consultarTotalizador(filtros: ConsultaBandejaPrestamos) {
    this.totalizador = 0;
    this.prestamosService
      .consultarTotalizador(filtros)
      .subscribe((num) => this.totalizador = num);
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.formularioConsulta.fechaDesde),
      CustomValidators.maxDate(new Date()));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.formularioConsulta.fechaHasta),
      CustomValidators.minDate(new Date()));
    fechaDesdeFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaHastaFc.clearValidators();
        let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec ? minDate = new Date(fechaDesdeMilisec) : minDate = new Date(fechaActualMilisec);
        fechaHastaFc.setValidators(Validators.compose([CustomValidators.minDate(minDate),
          CustomValidators.maxDate(new Date())]));
        fechaHastaFc.updateValueAndValidity();
      }
    });
    fechaHastaFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaDesdeFc.clearValidators();
        let fechaHastaMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let maxDate;
        fechaHastaMilisec <= fechaActualMilisec ? maxDate = new Date(fechaHastaMilisec) : maxDate = new Date(fechaActualMilisec);
        fechaDesdeFc.setValidators(CustomValidators.maxDate(maxDate));
        fechaDesdeFc.updateValueAndValidity();
      }
    });

    this.form = this.fb.group({
      nroFormulario: [this.formularioConsulta.NroFormulario, Validators.compose([Validators.maxLength(14), CustomValidators.number])],
      nroPrestamo: [this.formularioConsulta.NroPrestamo, Validators.compose([Validators.maxLength(8), CustomValidators.number])],
      nroSticker: [this.formularioConsulta.NroSticker, Validators.compose([Validators.maxLength(14), CustomValidators.number])],
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      origen: [this.formularioConsulta.IdOrigen],
      usuario: [this.formularioConsulta.IdUsuario],
      quiereReactivar: [this.formularioConsulta.quiereReactivar]
    });
  }

  public consultarPaginaSiguiente(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public consultarPrestamos(consultarTotal: boolean): void {
    this.consultarPaginaSiguiente();
    if (consultarTotal) {
      this.consultarTotalizador(this.formularioConsulta);
    }
  }

  private prepararConsultaFormularios(): ConsultaBandejaPrestamos {
    let consulta = new ConsultaBandejaPrestamos();
    consulta.NroSticker = this.form.value.nroSticker;
    consulta.NroFormulario = this.form.value.nroFormulario;
    consulta.NroPrestamo = this.form.value.nroPrestamo;
    consulta.fechaDesde = NgbUtils.obtenerDate(this.form.value.fechaDesde);
    consulta.fechaHasta = NgbUtils.obtenerDate(this.form.value.fechaHasta);
    consulta.idEstadoPrestamo = this.estadosIds;
    consulta.idLinea = this.lineaIds;
    consulta.IdOrigen = this.form.value.origen;
    consulta.IdUsuario = this.form.value.usuario;
    consulta.localidadIds = this.localidadIds;
    consulta.departamentoIds = this.departamentoIds;
    let consultaPersona = this.componentePersona.prepararConsulta();
    consulta.tipoPersona = consultaPersona.tipoPersona;
    consulta.cuil = consultaPersona.cuil;
    consulta.nombre = consultaPersona.nombre;
    consulta.apellido = consultaPersona.apellido;
    consulta.dni = consultaPersona.dni;
    consulta.quiereReactivar = this.form.value.quiereReactivar ? this.form.value.quiereReactivar : false;
    consulta.orderByDes = this.orderByDes;
    consulta.columnaOrderBy = this.columnaOrderBy;
    return consulta;
  }

  private reestablecerFiltros() {
    let filtrosGuardados = PrestamoService.recuperarFiltros();
    if (filtrosGuardados) {
      this.formularioConsulta = filtrosGuardados;
      this.comboEstado.setFiltros(this.formularioConsulta.idEstadoPrestamo);
      this.comboLinea.setFiltros(this.formularioConsulta.idLinea);
      this.comboDptoLocalidad.setDeptos(this.formularioConsulta.departamentoIds);
      this.comboDptoLocalidad.setLocaldiades(this.formularioConsulta.localidadIds);
      this.componentePersona.setFiltros(new BusquedaPorPersonaConsulta(filtrosGuardados.tipoPersona, filtrosGuardados.cuil, filtrosGuardados.apellido, filtrosGuardados.nombre, filtrosGuardados.dni));
      this.localidadIds = this.formularioConsulta.localidadIds;
      this.departamentoIds = this.formularioConsulta.departamentoIds;
      this.estadosIds = this.formularioConsulta.idEstadoPrestamo;
      this.lineaIds = this.formularioConsulta.idLinea;
      this.form.patchValue({
        cuil: this.formularioConsulta.cuil,
        nombre: this.formularioConsulta.nombre,
        apellido: this.formularioConsulta.apellido,
        nroFormulario: this.formularioConsulta.NroFormulario,
        nroPrestamo: this.formularioConsulta.NroPrestamo,
        nroSticker: this.formularioConsulta.NroSticker,
        fechaDesde: NgbUtils.obtenerNgbDateStruct(this.formularioConsulta.fechaDesde ? new Date(this.formularioConsulta.fechaDesde) : this.formularioConsulta.fechaDesde),
        fechaHasta: NgbUtils.obtenerNgbDateStruct(this.formularioConsulta.fechaHasta ? new Date(this.formularioConsulta.fechaHasta) : this.formularioConsulta.fechaHasta),
        origen: this.formularioConsulta.IdOrigen,
        usuario: this.formularioConsulta.IdUsuario,
        quiereReactivar: this.formularioConsulta.quiereReactivar
      });
      this.consultarPrestamos(true);
    } else {
      this.form.patchValue({
        fechaDesde: NgbUtils.obtenerNgbDateStruct(new Date(Date.now())),
        fechaHasta: NgbUtils.obtenerNgbDateStruct(new Date(Date.now()))
      });
    }
  }

  public rechazarPrestamo(idPrestamo: number) {
    const modalRechazo = this.modalService.open(ModalMotivoRechazoComponent, {backdrop: 'static', windowClass: 'modal-l'});
    modalRechazo.componentInstance.ambito = 'Prestamo';
    modalRechazo
      .result
      .then((comando) => {
        if (comando) {
          this.prestamosService.rechazarPrestamo(new RechazarPrestamoComando(idPrestamo, comando.motivosRechazo, comando.numeroCaja, comando.observaciones))
            .subscribe((resultado) => {
              if (resultado) {
                this.notificacionService.informar(Array.of('Préstamo rechazado con éxito.'), false)
                  .result.then(() => {
                  this.consultarPrestamos(true);
                });
              }
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      });
  }

  public validarRechazo(prestamo: BandejaPrestamoResultado): boolean {
    return (!(prestamo.idEstadoPrestamo == this.estadosPrestamo.CREADO
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.COMENZADO
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.EVALUACION_TECNICA
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.A_PAGAR
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.A_PAGAR_ENVIADO_A_SUAF
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.A_PAGAR_CON_SUAF
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.A_PAGAR_CON_LOTE
      || prestamo.idEstadoPrestamo == this.estadosPrestamo.IMPAGO));
  }

  public validarReactivacion(prestamo: BandejaPrestamoResultado): boolean {
    if (!this.form.value.quiereReactivar) return true;
    // idEstado 3 = Rechazado, idEstado 2 = Evaluacion tecnica
    return (!(!prestamo.esAsociativa && prestamo.idEstadoPrestamo === 3 && (prestamo.idEstadoPrestamoAnt === 1 || prestamo.idEstadoPrestamoAnt === 2 || prestamo.idEstadoPrestamoAnt === 4 || prestamo.idEstadoPrestamoAnt === 5 || prestamo.idEstadoPrestamoAnt === 9 || prestamo.idEstadoPrestamoAnt === 10 || prestamo.idEstadoPrestamoAnt === 11 || prestamo.idEstadoPrestamoAnt === 12)));
  }

  public validarRechazado(prestamo: BandejaPrestamoResultado): boolean {
    return (prestamo.idEstadoFormulario != this.estadosFormulario.RECHAZADO);
  }

  public editarNumeroCaja(idFormularioLinea: number, numeroCaja: string) {
    const modalEditarNumeroCaja = this.modalService.open(ModalEditarNumeroCajaComponent, {backdrop: 'static', size: 'lg'});
    modalEditarNumeroCaja.componentInstance.ambito = 'Prestamo';
    modalEditarNumeroCaja.componentInstance.idFormularioLinea = idFormularioLinea;
    modalEditarNumeroCaja.componentInstance.numeroCaja = numeroCaja;
    modalEditarNumeroCaja
      .result
      .then((comando) => {
        if (comando) {
          this.prestamosService.editarNumeroCaja(comando)
            .subscribe((resultado) => {
              if (resultado) {
                this.notificacionService.informar(Array.of('Número de caja actualizado con éxito.'), false)
                  .result.then(() => {
                  this.consultarPrestamos(false);
                });
              }
            }, (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      });
  }

  public validarConsulta(): boolean {
    if (this.componentePersona) {
      if (this.componentePersona.documentoIngresado()) {
        return false;
      }
      return !(this.form.value.fechaDesde && this.form.value.fechaHasta);
    } else {
      return !(this.form.value.fechaDesde && this.form.value.fechaHasta);
    }
  }

  public ordenarColumna(columna: number) {
    if (columna === this.columnaOrderBy) {
      this.orderByDes = !this.orderByDes;
      this.consultarPrestamos(false);
    } else {
      this.orderByDes = true;
      this.columnaOrderBy = columna;
      this.consultarPrestamos(false);
    }
  }

  public imprimirBandeja(): void {
    this.generarExcel();
    this.generarPDF();
  }

  private generarExcel(): void {
    this.prestamosService
      .generarReporteBandejaExcel(this.formularioConsulta)
      .subscribe(
        (archivoReporte) => {
          this.archivoService.descargarArchivo(archivoReporte);
        }
      );
  }

  private generarPDF(): void {
    this.prestamosService.generarReporteBandejaPDF(this.formularioConsulta)
      .subscribe((resultado) => {
        if (resultado) {
          if (resultado.errores && resultado.errores.length > 0) {
            this.notificacionService.informar(resultado.errores, true);
            return;
          }
          this.pdfPagos.next(
            this.sanitizer.bypassSecurityTrustResourceUrl(
              this.archivoService.getUrlPrevisualizacionArchivo(
                resultado.archivos[0])));
          this.archivoService.descargarArchivos(resultado.archivos, 'pdf.pdf');
        }
      });
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos ? departamentos.join(',') : null;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades ? localidades.join(',') : null;
  }

  public guardarEstadosSeleccionadas(estados: string[]) {
    this.estadosIds = estados ?  estados.join(',') : null;
  }

  public guardarLineasSeleccionadas(lineas: string[]) {
    this.lineaIds = lineas ? lineas.join(',') : null;
  }
}
