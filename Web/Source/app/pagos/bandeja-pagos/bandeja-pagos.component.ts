import { Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { Router } from '@angular/router';
import { LocalidadComboServicio } from '../../formularios/shared/localidad.service';
import { OrigenService } from '../../formularios/shared/origen-prestamo.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { NgbDatepickerConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OrigenPrestamo } from '../../formularios/shared/modelo/origen-prestamo.model';
import { Localidad } from '../../formularios/shared/modelo/localidad.model';
import { Departamento } from '../../formularios/shared/modelo/departamento.model';
import { FormulariosService } from '../../formularios/shared/formularios.service';
import { BandejaPagosConsulta } from '../shared/modelo/bandeja-prestamo-consulta.model';
import { PagosService } from '../shared/pagos.service';
import { BandejaPagosResultado } from '../shared/modelo/bandeja-pagos-resultado.model';
import { BandejaAsignarMontoDisponible } from '../shared/modelo/bandeja-asignar-monto-disponible.model';
import { CalculoMontoAcumulado } from '../shared/modelo/calculo-monto-acumulado.model';
import { SimularLoteResultado } from '../shared/modelo/simular-lote-resultado.model';
import { MontoDisponibleConsulta } from '../shared/modelo/monto-disponible-consulta.model';
import { ConfirmarLoteComando } from '../shared/modelo/confirmar-lote-comando.model';
import { TasasLoteResultado } from '../shared/modelo/tasas-lote-resultado.model';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { ModalFormulariosPrestamoComponent } from '../modal-formularios-prestamo/modal-formularios-prestamo.component';
import { DateUtils } from '../../shared/date-utils';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { LineaCombo } from '../../formularios/shared/modelo/linea-combo.model';
import { BusquedaPorPersonaComponent } from '../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { BusquedaPorPersonaConsulta } from '../../shared/modelo/busqueda-por-persona-consulta.model';
import { ModalidadPagoComponent } from '../modalidad-pago/modalidad-pago.component';
import TiposLote from '../tipos-lote.enum';
import { ColumnasPagoEnum } from '../../formularios/shared/modelo/columnas-pago-enum.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-pagos',
  templateUrl: './bandeja-pagos.component.html',
  styleUrls: ['./bandeja-pagos.component.scss'],
})

export class BandejaPagosComponent implements OnInit, OnDestroy {

  public consulta: BandejaPagosConsulta;
  public bandejaResultados: BandejaPagosResultado[] = [];
  public form: FormGroup;
  public formPrestamosAPagar: FormGroup;
  public departamentos: Departamento[] = [];
  public CBLocalidad: Localidad[] = [];
  public CBOrigen: OrigenPrestamo[] = [];
  public CBLineas: LineaCombo[] = [];
  public lineas: LineaCombo[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public montoAcumulado: CalculoMontoAcumulado = new CalculoMontoAcumulado(0, 0);
  public seleccionarTodosCheckeado: boolean = false;
  public montosDisponibles: BandejaAsignarMontoDisponible[] = [];
  public formAsignarMontoDisponible: FormGroup;
  public formConfirmacionLote: FormGroup;
  public resultadoSimulacion: SimularLoteResultado;
  public bandejaSimulacion: boolean = false;
  public montoSeleccionado: BandejaAsignarMontoDisponible = new BandejaAsignarMontoDisponible();
  public tasas: TasasLoteResultado;
  public loteConfirmado: boolean = false;
  public mostrarBotonCancelar: boolean = false;
  public existeNroFormulario: boolean = false;
  public fechaMinimaParaSimulacionPago: any;
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public prestamosParaConformarLote: BandejaPagosResultado[] = [];
  public idsAgrupamientoSeleccionados: number[] = [];
  public columnasEnum = ColumnasPagoEnum;
  public orderByDes = true;
  public columnaOrderBy = -1;

  @Output() public emitirLoteConfirmado: EventEmitter<boolean> = new EventEmitter<boolean>();
  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;
  @ViewChild(ModalidadPagoComponent)
  public modalidad: ModalidadPagoComponent;

  constructor(private fb: FormBuilder,
              private formulariosService: FormulariosService,
              private lineasService: LineaService,
              private pagosService: PagosService,
              private localidadesService: LocalidadComboServicio,
              private origenesService: OrigenService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private router: Router,
              private modalService: NgbModal,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo lote de pagos ' + TituloBanco.TITULO);

    if (!this.consulta) {
      this.consulta = new BandejaPagosConsulta();
    }
    if (!this.montoAcumulado) {
      this.montoAcumulado = new CalculoMontoAcumulado();
    }
  }

  // INICIALIZACIÓN Y CONSULTAS

  public ngOnInit(): void {
    this.fechaMinimaParaSimulacionPago = NgbUtils.obtenerNgbDateStruct(new Date());
    this.consulta.fechaInicioTramite = new Date(Date.now());
    this.consulta.fechaFinTramite = new Date(Date.now());
    this.reestablecerFiltros();
    this.crearForm();
    this.cargarCombos();
    this.configurarPaginacion();
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    if (!(this.router.url.includes('pagos')
      || this.router.url.includes('nuevo-pago'))) {
      PagosService.guardarFiltros(null);
    }
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades;
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaInicioTramite),
      CustomValidators.maxDate(new Date()));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaFinTramite),
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
    let nroFormularioFc = new FormControl(this.consulta.nroFormulario,
      Validators.compose([Validators.maxLength(14), CustomValidators.number]));
    nroFormularioFc.valueChanges.subscribe((value) => {
      this.existeNroFormulario = !!value;
    });

    this.form = this.fb.group({
      fechaInicioTramite: fechaDesdeFc,
      fechaFinTramite: fechaHastaFc,
      departamento: [this.consulta.idDepartamento],
      localidad: [this.consulta.idLocalidad],
      nroPrestamoChecklist: [this.consulta.nroPrestamoChecklist, Validators.compose([
        CustomValidators.number,
        Validators.maxLength(8)
      ])],
      nroFormulario: nroFormularioFc,
      origen: [this.consulta.idOrigen],
      lugarOrigen: [this.consulta.idLugarOrigen],
      lineas: [this.consulta.idsLineas]
    });
    this.cargarLocalidades();
    (this.form.get('departamento') as FormControl).valueChanges
      .subscribe(() => {
        this.cargarLocalidades();
        (this.form.get('localidad') as FormControl).setValue(null);
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
    this.subscripcionLineas();
  }

  private cargarLocalidades(): void {
    if (this.form.get('departamento').value &&
      this.form.get('departamento').value !== 'null') {
      this.localidadesService.consultarLocalidades(this.form.get('departamento').value)
        .subscribe((localidades) => {
          this.CBLocalidad = localidades;
          if (this.CBLocalidad.length) {
            (this.form.get('localidad') as FormControl).enable();
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      this.CBLocalidad = [];
      (this.form.get('localidad') as FormControl).disable();
    }
  }

  private cargarCombos() {
    this.formulariosService
      .consultarDepartamentos()
      .subscribe((departamentos) => this.departamentos = departamentos,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => this.CBOrigen = origenes,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.lineasService
      .consultarLineasParaCombo()
      .subscribe((lineas) => {
        this.CBLineas = lineas;
        this.lineas = lineas;
        this.crearForm();
      });
  }

  private subscripcionLineas(): void {
    let linea = this.form.get('lineas') as FormControl;
    linea.valueChanges
      .distinctUntilChanged()
      .subscribe(() => {
        if (linea.enabled) {
          if (linea.value && linea.value.length) {
            let seleccionada = this.lineas.find((x) => x.id === linea.value[0]);
            this.CBLineas = this.lineas;
          } else {
            this.CBLineas = this.lineas;
          }
        }
      });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsulta();
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService
          .consultarBandeja(filtros);
      })
      .share();
    (<Observable<BandejaPagosResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormPretamosAPagar();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  private reestablecerFiltros() {
    let filtrosGuardados = PagosService.recuperarFiltros();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.componentePersona.setFiltros(new BusquedaPorPersonaConsulta(filtrosGuardados.tipoPersona, filtrosGuardados.cuil, filtrosGuardados.apellido, filtrosGuardados.nombre, filtrosGuardados.dni));
      this.consulta.fechaInicioTramite = this.consulta.fechaInicioTramite ? new Date(this.consulta.fechaInicioTramite) : this.consulta.fechaInicioTramite;
      this.consulta.fechaFinTramite = this.consulta.fechaFinTramite ? new Date(this.consulta.fechaFinTramite) : this.consulta.fechaFinTramite;
      this.crearForm();
      this.consultar(false);
    }
  }

  public consultar(esNuevaConsulta: boolean, pagina?: number) {
    if (esNuevaConsulta) {
      this.limpiarDatosPrevios();
    }
    this.prepararConsulta();
    if (!this.existeNroFormulario) {
      if (!this.consulta.idsLineas || !this.consulta.idsLineas.length) {
        this.notificacionService.informar(['Debe seleccionar una línea.']);
      } else {
        if (this.componentePersona && !this.componentePersona.documentoIngresado()) {
          if ((this.consulta.fechaInicioTramite == null
            || this.consulta.fechaFinTramite == null)) {
            this.notificacionService.informar(['Debe ingresar fecha inicio y fecha fin.']);
          } else {
            if (this.consulta.fechaFinTramite < this.consulta.fechaInicioTramite) {
              this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta.']);
            } else {
              PagosService.guardarFiltros(this.consulta);
              this.paginaModificada.next(pagina);
            }
          }
        } else {
          PagosService.guardarFiltros(this.consulta);
          this.paginaModificada.next(pagina);
        }
      }

    } else {
      PagosService.guardarFiltros(this.consulta);
      this.paginaModificada.next(pagina);
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;

    this.consulta.fechaInicioTramite = NgbUtils.obtenerDate(formModel.fechaInicioTramite);
    this.consulta.fechaFinTramite = NgbUtils.obtenerDate(formModel.fechaFinTramite);
    this.consulta.nroPrestamoChecklist = formModel.nroPrestamoChecklist;
    this.consulta.nroFormulario = formModel.nroFormulario;
    this.consulta.idDepartamento = formModel.departamento;
    this.consulta.idLocalidad = formModel.localidad;
    this.consulta.idOrigen = formModel.origen;
    this.consulta.idLugarOrigen = formModel.lugarOrigen;
    this.consulta.idsLineas = formModel.lineas;
    let consultaPersona = this.componentePersona.prepararConsulta();
    this.consulta.tipoPersona = consultaPersona.tipoPersona;
    this.consulta.cuil = consultaPersona.cuil;
    this.consulta.nombre = consultaPersona.nombre;
    this.consulta.apellido = consultaPersona.apellido;
    this.consulta.dni = consultaPersona.dni;
    this.consulta.departamentoIds = this.departamentoIds;
    this.consulta.localidadIds = this.localidadIds;
    this.consulta.orderByDes = this.orderByDes;
    this.consulta.columnaOrderBy = this.columnaOrderBy;
  }

  private limpiarDatosPrevios(): void {
    this.idsAgrupamientoSeleccionados = [];
    this.prestamosParaConformarLote = [];
  }

  public validarConsulta(): boolean {
    if (!this.form) {
      return true;
    }
    if (this.componentePersona) {
      if (this.componentePersona.documentoIngresado()) {
        return false;
      }
    }
    return !(this.form.value.fechaInicioTramite && this.form.value.fechaFinTramite);
  }

  private crearFormPretamosAPagar() {
    this.formPrestamosAPagar = this.fb.group({
      prestamos: this.fb.array((this.bandejaResultados || []).map((prestamo) =>
        this.fb.group({
          idPrestamo: [prestamo.id],
          linea: [prestamo.linea],
          departamento: [prestamo.departamento],
          localidad: [prestamo.localidad],
          origen: [prestamo.origen],
          cantFormularios: [prestamo.cantFormularios],
          nroPrestamo: [prestamo.nroPrestamo],
          montoOtorgado: [prestamo.montoOtorgado],
          fechaPedido: [prestamo.fechaPedido],
          seleccionado: [prestamo.seleccionado],
          apellidoYNombre: [prestamo.apellidoYNombre],
          nroFormulario: [prestamo.nroFormulario]
        })
      )),
    });
  }

  public agregarPrestamoParaAgrupar(idAgrupamiento: number): void {
    let filtros = this.consulta;
    filtros.nroPrestamoChecklist = idAgrupamiento + '';
    this.pagosService.consultarBandejaCompleta(filtros).subscribe(
      (formularios) => {
        if (formularios.elementos) {
          this.verificarFormularios(formularios.elementos, idAgrupamiento);
        }
      });
  }

  public quitarFormulariosParaAgrupar(idAgrupamiento: number, toolTipQuitar: any): void {
    toolTipQuitar.close();
    this.prestamosParaConformarLote = this.prestamosParaConformarLote.filter(
      (x) => x.nroPrestamo !== idAgrupamiento);
    this.idsAgrupamientoSeleccionados = this.idsAgrupamientoSeleccionados.filter(
      (x) => x !== idAgrupamiento);
    this.contarPrestamos();
  }

  private verificarFormularios(formularios: BandejaPagosResultado[],
                               idAgrupamiento?: number) {
    this.prestamosParaConformarLote.push(...formularios);
    if (idAgrupamiento) {
      this.idsAgrupamientoSeleccionados.push(idAgrupamiento);
    }
    this.contarPrestamos();
  }

  public estaSeleccionado(idAgrupamiento: number): boolean {
    const esSeleccionado = this.idsAgrupamientoSeleccionados.find(
      (id) => id === idAgrupamiento);
    return esSeleccionado !== undefined;
  }

  public clickEnSeleccionarTodos(checked: boolean) {
    this.seleccionarTodosCheckeado = checked;
    this.prestamosParaConformarLote = [];
    this.idsAgrupamientoSeleccionados = [];
    if (this.seleccionarTodosCheckeado) {
      this.prepararConsulta();
      let filtros = this.consulta;
      this.pagosService.consultarBandejaCompleta(filtros).subscribe
      ((bandeja) => {
        this.verificarFormularios(bandeja.elementos);
        let ids = new Set<number>();
        bandeja.elementos.forEach((prestamo) => ids.add(prestamo.nroPrestamo));
        ids.forEach((id) => this.idsAgrupamientoSeleccionados.push(id));
        this.contarPrestamos();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
    }
    this.contarPrestamos();
  }

  public contarPrestamos(): void {
    this.montoAcumulado.cantidadPrestamos = this.idsAgrupamientoSeleccionados.length;
    this.montoAcumulado.montoAcumulado = this.calcularMonto();
  }

  private calcularMonto(): number {
    let acumulador = 0;
    this.prestamosParaConformarLote.forEach((prestamo) => {
      acumulador += prestamo.montoOtorgado;
    });
    return acumulador;
  }

  public get prestamosAPagarFormArray(): FormArray {
    return this.formPrestamosAPagar.get('prestamos') as FormArray;
  }

  // SIMULACIÓN Y CONFIRMACIÓN.

  public verFormularioPrestamo(idPrestamo: number) {
    this.pagosService.obtenerFormulariosPorPrestamo(idPrestamo)
      .subscribe((formularios) => {
        const modalRef = this.modalService.open(ModalFormulariosPrestamoComponent, {backdrop: 'static', size: 'lg'});
        modalRef.componentInstance.formularioResultados = formularios;
      });
  }

  public simularLote() {
    this.pagosService.obtenerTasas().subscribe((tasas) => {
      this.tasas = tasas;
      this.pagosService.consultarMontosDisponibles(new MontoDisponibleConsulta(this.calcularMontoTotalLote())).subscribe(
        (montosDisponibles) => {
          if (montosDisponibles.length === 0) {
            this.notificacionService.informar(['No se encontraron montos disponibles con sufiente monto libre']);
          } else {
            this.montosDisponibles = montosDisponibles;
            this.crearFormAsignarMontoDisponible();
            this.bandejaSimulacion = true;
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    });
    this.mostrarBotonCancelar = true;
  }

  private calcularMontoTotalLote(): number {
    let comision = this.calcularComision(this.montoAcumulado.montoAcumulado);
    let iva = this.calcularIva(comision);
    this.montoAcumulado.montoAcumuladoIvaComision = this.montoAcumulado.montoAcumulado + comision + iva;
    return this.montoAcumulado.montoAcumuladoIvaComision;
  }

  public volver() {
    this.formAsignarMontoDisponible = undefined;
    this.montosDisponibles = [];
    this.resultadoSimulacion = undefined;
    this.bandejaSimulacion = false;
  }

  public crearFormAsignarMontoDisponible() {
    this.formAsignarMontoDisponible = this.fb.group({
      nombreLote: ['', Validators.compose([
        Validators.required,
        CustomValidators.validTextAndNumbers,
        Validators.maxLength(100)])],
      montos: this.fb.array((this.montosDisponibles || []).map((monto) =>
        this.fb.group({
          idMontoDisponible: [monto.idMontoDisponible],
          nroMonto: [monto.nroMontoDisponible],
          fechaAlta: [monto.fechaAlta],
          descripcion: [monto.descripcion],
          montoTotal: [monto.montoTotal],
          montoUsado: [monto.montoUsado],
          montoAUsar: [monto.montoAUsar = monto.montoTotal - monto.montoUsado],
          seleccionado: [monto.seleccionado]
        })
      )),
    });
  }

  public get asignarMontoDisponibleFormArray(): FormArray {
    return this.formAsignarMontoDisponible.get('montos') as FormArray;
  }

  public clickMonto(nroMonto: number) {
    let montoCheckeado = this.montosDisponibles.filter((x) => x.idMontoDisponible === nroMonto)[0];
    montoCheckeado.seleccionado = true;
    this.montoSeleccionado = montoCheckeado;
    let montosNoCheckeado = this.montosDisponibles.filter((x) => x.idMontoDisponible !== nroMonto);
    montosNoCheckeado.forEach((x) => x.seleccionado = false);
  }

  public simular() {
    if (!this.formAsignarMontoDisponible.valid || (this.formAsignarMontoDisponible.value.nombreLote.replace(/[^A-Za-z]/g, '').length === 0)) {
      this.notificacionService.informar(['Debe ingresar un nombre válido para el lote']);
      return;
    }

    if (!this.validarQueHayaSeleccionadoUnMonto()) {
      this.notificacionService.informar(['Debe seleccionar un monto disponible']);
      return;
    }

    this.resultadoSimulacion = new SimularLoteResultado();
    this.resultadoSimulacion.descripcion = this.formAsignarMontoDisponible.value.nombreLote;
    this.resultadoSimulacion.montoDisponible = this.getMontoLoteSeleccionado().montoAUsar;
    this.resultadoSimulacion.montoLote = this.montoAcumulado.montoAcumulado;
    this.resultadoSimulacion.cantPrestamos = this.montoAcumulado.cantidadPrestamos;
    this.resultadoSimulacion.comision = this.calcularComision(this.montoAcumulado.montoAcumulado);
    this.resultadoSimulacion.iva = this.calcularIva(this.resultadoSimulacion.comision);
    this.resultadoSimulacion.totalMontoLote = this.resultadoSimulacion.montoLote + this.resultadoSimulacion.comision + this.resultadoSimulacion.iva;
    this.resultadoSimulacion.diferencia = this.resultadoSimulacion.montoDisponible - this.resultadoSimulacion.totalMontoLote;
    this.mostrarBotonCancelar = false;
    this.crearFormAsignarFechaPagoLote();
  }

  private validarQueHayaSeleccionadoUnMonto(): boolean {
    let montosSeleccionados = this.montosDisponibles.filter((x) => x.seleccionado === true);
    return !(montosSeleccionados.length === 0);
  }

  private getMontoLoteSeleccionado(): BandejaAsignarMontoDisponible {
    return this.montosDisponibles.filter((x) => x.seleccionado === true)[0];
  }

  private calcularIva(monto: number): number {
    return monto * (this.tasas.iva / 100);
  }

  private calcularComision(monto: number): number {
    return monto * (this.tasas.comision / 1000);
  }

  public confirmarLote() {
    this.notificacionService.confirmar('Se creará un lote de pago. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        let comando = new ConfirmarLoteComando();
        comando.nombreLote = this.formAsignarMontoDisponible.get('nombreLote').value;
        comando.idMontoDisponible = this.getMontoLoteSeleccionado().idMontoDisponible;
        comando.iva = this.tasas.iva / 100;
        comando.comision = this.tasas.comision / 1000;
        comando.monto = this.montoAcumulado.montoAcumulado;
        comando.consulta = this.consulta;
        comando.idAgrupamientosSeleccionados = this.idsAgrupamientoSeleccionados;
        comando.idPrestamosSeleccionados = [];
        this.prestamosParaConformarLote.forEach((x) => {
          if (!comando.idPrestamosSeleccionados.some((id) => id === x.id)) {
            comando.idPrestamosSeleccionados.push(x.id);
          }
        });
        if (this.modalidad.obtenerModalidad()) {
          comando.modalidad = this.modalidad.obtenerModalidad();
        } else {
          this.notificacionService.informar(['Debe seleccionar una modalidad de pago']);
          return;
        }
        if (this.modalidad.obtenerElemento()) {
          comando.elemento = this.modalidad.obtenerElemento();
          if (comando.elemento === 1) {
            comando.idTipoLote = TiposLote.LOTE_PAGO_BANCO;
          } else {
            comando.idTipoLote = TiposLote.LOTE_PAGO_CHEQUE;
          }
        } else {
          this.notificacionService.informar(['Debe seleccionar un elemento de pago']);
          return;
        }
        if (this.modalidad.obtenerFechaPago()) {
          comando.fechaPago = this.modalidad.obtenerFechaPago();
        } else {
          this.notificacionService.informar(['Debe seleccionar una fecha de inicio de pago']);
          return;
        }
        if (this.modalidad.obtenerFechaFinPago()) {
          comando.fechaFinPago = this.modalidad.obtenerFechaFinPago();
        } else {
          this.notificacionService.informar(['Debe seleccionar una de fin de pago']);
          return;
        }
        if (this.modalidad.obtenerConvenio()) {
          comando.convenio = this.modalidad.obtenerConvenio();
        } else {
          this.notificacionService.informar(['Debe seleccionar un convenio de pago']);
          return;
        }
        if (this.modalidad.obtenerMesesGracia()) {
          comando.mesesGracia = this.modalidad.obtenerMesesGracia();
        } else {
          this.notificacionService.informar(['Debe ingresar mes/es de gracia.']);
          return;
        }
        if (this.modalidad.obtenerFechaFinPago() < this.modalidad.obtenerFechaPago()) {
          this.notificacionService.informar(['La fecha de fin de pago no puede ser menor a la fecha de inicio de pago.']);
          return;
        }
        this.pagosService.confirmarLote(comando)
          .subscribe((resultado) => {
            if (resultado) {
              this.notificacionService.informar(Array.of('Lote creado con éxito'), false)
                .result.then(() => {
                this.loteConfirmado = true;
                this.emitirLoteConfirmado.emit(Object.assign({}, this.loteConfirmado));
                this.formAsignarMontoDisponible.get('nombreLote').disable({emitEvent: true});
                this.asignarMontoDisponibleFormArray.disable({emitEvent: false});
                this.modalidad.disableForm();

              });
            }
          }, (errores) => {
            this.loteConfirmado = false;
            this.notificacionService.informar(errores, true);
          });
      }
    });
  }

  public salir() {
    this.ngOnDestroy();
    this.router.navigate(['/bandeja-lotes']);
  }

  public crearFormAsignarFechaPagoLote() {
    this.formConfirmacionLote = this.fb.group({
      fechaPago: new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaPago))
    });
  }

  public validarConsultaLote(): boolean {
    if (this.modalidad) {
      return !this.modalidad.esValido();
    }
    return true;
  }

  public ordenarColumna(columna: number) {
    if (columna === this.columnaOrderBy) {
      this.orderByDes = !this.orderByDes;
      this.consultar(false);
    } else {
      this.orderByDes = true;
      this.columnaOrderBy = columna;
      this.consultar(false);
    }
  }
}
