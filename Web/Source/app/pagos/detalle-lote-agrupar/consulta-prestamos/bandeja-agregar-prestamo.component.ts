import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { Observable, Subject } from 'rxjs';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { BusquedaPorPersonaComponent } from '../../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { BandejaPagosResultado } from '../../shared/modelo/bandeja-pagos-resultado.model';
import { TasasLoteResultado } from '../../shared/modelo/tasas-lote-resultado.model';
import { BandejaPagosConsulta } from '../../shared/modelo/bandeja-prestamo-consulta.model';
import { Departamento } from '../../../formularios/shared/modelo/departamento.model';
import { OrigenPrestamo } from '../../../formularios/shared/modelo/origen-prestamo.model';
import { LineaCombo } from '../../../formularios/shared/modelo/linea-combo.model';
import { Localidad } from '../../../formularios/shared/modelo/localidad.model';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { CalculoMontoAcumulado } from '../../shared/modelo/calculo-monto-acumulado.model';
import { BandejaAsignarMontoDisponible } from '../../shared/modelo/bandeja-asignar-monto-disponible.model';
import { SimularLoteResultado } from '../../shared/modelo/simular-lote-resultado.model';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { PagosService } from '../../shared/pagos.service';
import { LocalidadComboServicio } from '../../../formularios/shared/localidad.service';
import { OrigenService } from '../../../formularios/shared/origen-prestamo.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { NgbDatepickerConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils } from '../../../shared/date-utils';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { BusquedaPorPersonaConsulta } from '../../../shared/modelo/busqueda-por-persona-consulta.model';
import { ModalFormulariosPrestamoComponent } from '../../modal-formularios-prestamo/modal-formularios-prestamo.component';
import { DetalleLote } from '../../monto-disponible/shared/modelo/detalle-lote.model';
import { MontoDisponibleService } from '../../monto-disponible/shared/monto-disponible.service';
import { MontoDisponible } from '../../monto-disponible/shared/modelo/monto-disponible.model';
import { AgregarPrestamoLoteComando } from '../../shared/modelo/agregar-prestamo-lote-comando.model';

@Component({
  selector: 'bg-bandeja-agregar-prestamo',
  templateUrl: './bandeja-agregar-prestamo.component.html',
  styleUrls: ['bandeja-agregar-prestamo.component.scss'],
})

export class BandejaAgregarPrestamoComponent implements OnInit, OnDestroy {

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
  public resultadoSimulacion: SimularLoteResultado;
  public bandejaSimulacion: boolean = false;
  public tasas: TasasLoteResultado;
  public mostrarBotonCancelar: boolean = false;
  public existeNroFormulario: boolean = false;
  public fechaMinimaParaSimulacionPago: any;
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public prestamosParaConformarLote: BandejaPagosResultado[] = [];
  public idsAgrupamientoSeleccionados: number[] = [];
  public idLote: number;
  public detalleLote: DetalleLote = new DetalleLote();
  public montoDisponible: MontoDisponible = new MontoDisponible();
  public existeMontoDisponible: boolean = false;
  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;

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
              private route: ActivatedRoute,
              private montoDisponibleService: MontoDisponibleService) {

    if (!this.consulta) {
      this.consulta = new BandejaPagosConsulta();
    }
    if (!this.montoAcumulado) {
      this.montoAcumulado = new CalculoMontoAcumulado();
    }
  }

  // INICIALIZACIÓN Y CONSULTAS

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
    this.fechaMinimaParaSimulacionPago = NgbUtils.obtenerNgbDateStruct(new Date());
    this.consulta.fechaInicioTramite = new Date(Date.now());
    this.consulta.fechaFinTramite = new Date(Date.now());
    this.reestablecerFiltros();
    this.crearForm();
    this.cargarCombos();
    this.configurarPaginacion();
    this.pagosService.obtenerCabeceraDetalleLote(this.idLote)
      .subscribe((detalleLote) => {
        this.detalleLote = detalleLote;
      });
    this.pagosService.obtenerTasas().subscribe((tasas) => {
      this.tasas = tasas;
    });
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
      .consultarLineasParaCombo(true)
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

    this.consulta.orderByDes = true;
    this.consulta.columnaOrderBy = -1;
    this.consulta.departamentoIds = this.departamentoIds;
    this.consulta.localidadIds = this.localidadIds;
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
    this.montoDisponibleService.obtenerPorNro(this.detalleLote.nroMonto)
      .subscribe((monto) => {
        this.montoDisponible = monto;
      });
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
    this.resultadoSimulacion = undefined;
  }

  public quitarFormulariosParaAgrupar(idAgrupamiento: number, toolTipQuitar: any): void {
    toolTipQuitar.close();
    this.prestamosParaConformarLote = this.prestamosParaConformarLote.filter(
      (x) => x.nroPrestamo !== idAgrupamiento);
    this.idsAgrupamientoSeleccionados = this.idsAgrupamientoSeleccionados.filter(
      (x) => x !== idAgrupamiento);
    this.contarPrestamos();
    this.resultadoSimulacion = undefined;
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
    this.resultadoSimulacion = undefined;
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

    let cantidadPrestamoLote = this.detalleLote.cantidadPrestamos;
    let montoPrestamoLote = this.detalleLote.montoLote;
    this.resultadoSimulacion = new SimularLoteResultado();
    this.resultadoSimulacion.descripcion = this.detalleLote.nombre;
    this.resultadoSimulacion.montoDisponible = this.montoDisponible.saldo;
    this.resultadoSimulacion.cantPrestamosActual = cantidadPrestamoLote;
    this.resultadoSimulacion.montoLoteActual = montoPrestamoLote;
    this.resultadoSimulacion.montoLote = this.montoAcumulado.montoAcumulado;
    this.resultadoSimulacion.cantPrestamos = this.montoAcumulado.cantidadPrestamos + cantidadPrestamoLote;
    this.resultadoSimulacion.comision = this.calcularComision(this.montoAcumulado.montoAcumulado);
    this.resultadoSimulacion.iva = this.calcularIva(this.resultadoSimulacion.comision);
    this.resultadoSimulacion.totalMontoLote = this.resultadoSimulacion.montoLote + this.resultadoSimulacion.montoLoteActual + this.resultadoSimulacion.comision + this.resultadoSimulacion.iva;
    this.resultadoSimulacion.diferencia = this.resultadoSimulacion.montoDisponible - this.montoAcumulado.montoAcumulado;
    this.mostrarBotonCancelar = false;

    if (this.montoDisponible.saldo > this.montoAcumulado.montoAcumulado) {

      this.existeMontoDisponible = true;
    } else {
      this.existeMontoDisponible = false;
      this.mostrarBotonCancelar = false;
      this.notificacionService.informar(['El monto disponible no es suficientes para la cantidad de préstamos seleccionados.']);
    }
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
        let comando = new AgregarPrestamoLoteComando();
        comando.idLote = this.idLote;
        let idsPrestamosAgregar = [];
        this.prestamosParaConformarLote.forEach((x) => {
          idsPrestamosAgregar.push(x.id);
        });
        idsPrestamosAgregar = Array.from(new Set(idsPrestamosAgregar));
        comando.idsPrestamo = idsPrestamosAgregar.join(',');
        comando.idMonto = this.montoDisponible.id;
        comando.monto = this.montoAcumulado.montoAcumulado;
        this.pagosService.agregarPrestamosLote(comando)
          .subscribe((resultado) => {
              if (resultado) {
                this.notificacionService.informar(Array.of('Préstamos agregados al lote con éxito'), false).result.then((res) => {
                  if (res) {
                    this.router.navigate(['/agregar-lote/' + this.idLote]);
                  }
                });
              }
            },
            (errores) => {
              this.notificacionService.informar(errores, true);
            });
      }
    });
  }
}
