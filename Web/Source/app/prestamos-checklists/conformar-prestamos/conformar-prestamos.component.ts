import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BandejaConformarPrestamoConsulta } from '../shared/modelos/consulta-bandeja-conformar-prestamo.model';
import { PrestamoService } from '../../shared/servicios/prestamo.service';
import { FormulariosService } from '../../formularios/shared/formularios.service';
import { CustomValidators, isEmpty } from '../../shared/forms/custom-validators';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { NotificacionService } from '../../shared/notificacion.service';
import { OrigenPrestamo } from '../../formularios/shared/modelo/origen-prestamo.model';
import { Localidad } from '../../formularios/shared/modelo/localidad.model';
import { Departamento } from '../../formularios/shared/modelo/departamento.model';
import { EstadoFormulario } from '../../formularios/shared/modelo/estado-formulario.model';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { EstadoFormularioService } from '../../formularios/shared/estado-formulario.service';
import { OrigenService } from '../../formularios/shared/origen-prestamo.service';
import { LocalidadComboServicio } from '../../formularios/shared/localidad.service';
import { DateUtils } from '../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { LineaCombo } from '../../formularios/shared/modelo/linea-combo.model';
import { BandejaConformarPrestamoResultado } from '../shared/modelos/bandeja-conformar-prestamo-resultado.model';
import { FormularioPrestamo } from '../../formularios/shared/modelo/formulario-prestamo.model';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import { MultipleSeleccionComponent } from '../../shared/multiple-seleccion/multiple-seleccion.component';
import { FiltroDomicilioBandejaComponent } from '../../shared/domicilio-bandeja/filtro-domicilio-bandeja.component';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'bg-conformar-prestamos',
  templateUrl: './conformar-prestamos.component.html',
  styleUrls: ['./conformar-prestamos.component.scss']
})

export class ConformarPrestamosComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public consulta: BandejaConformarPrestamoConsulta;
  public departamentos: Departamento[] = [];
  public CBLocalidad: Localidad[] = [];
  public CBOrigen: OrigenPrestamo[] = [];
  public CBEstadoFormulario: EstadoFormulario[] = [];
  public CBLinea: LineaCombo[] = [];
  public formularioResultados: BandejaConformarPrestamoResultado[] = [];
  public formulariosParaConformarPrestamo: FormularioPrestamo[] = [];
  private primerFormularioSeleccionado = false;
  private idsAgrupamientoSeleccionados = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public departamentoIds: string;
  public localidadIds: string;
  public estadosIds: string;
  public lineaIds: string;
  public formulariosTxt = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;

  @ViewChild(MultipleSeleccionComponent)
  public comboEstado: MultipleSeleccionComponent;
  @ViewChild('lineas')
  public comboLinea: MultipleSeleccionComponent;
  @ViewChild(FiltroDomicilioBandejaComponent)
  public comboDptoLocalidad: FiltroDomicilioBandejaComponent;

  constructor(private fb: FormBuilder,
              private formulariosService: FormulariosService,
              private prestamoService: PrestamoService,
              private notificacionService: NotificacionService,
              private localidadesService: LocalidadComboServicio,
              private lineasService: LineaService,
              private estadosFormularioService: EstadoFormularioService,
              private origenesService: OrigenService,
              private config: NgbDatepickerConfig,
              private router: Router,
              private titleService: Title,
              private sanitizer: DomSanitizer) {
    this.titleService.setTitle('Conformar préstamos ' + TituloBanco.TITULO);
    if (!this.consulta) {
      this.consulta = new BandejaConformarPrestamoConsulta();
      this.consulta.fechaDesde = new Date();
      this.consulta.fechaHasta = new Date();
    }
  }

  public ngOnInit(): void {
    this.crearForm();
    this.limiteFecha();
    this.cargarCombos();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  private cargarCombos() {
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => this.CBOrigen = origenes);
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaDesde),
      CustomValidators.maxDate(new Date()));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaHasta),
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
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      origen: [this.consulta.idOrigen],
      numeroFormulario: [this.consulta.numeroFormulario,
        Validators.compose([CustomValidators.number, Validators.maxLength(14)])],
      cuil: [this.consulta.cuilSolicitante, Validators.compose([Validators.maxLength(11), CustomValidators.number])],
      dni: [this.consulta.dni, Validators.compose([Validators.maxLength(8), CustomValidators.number])],
      nombre: [this.consulta.nombre],
      apellido: [this.consulta.apellido]
    });
  }

  private limiteFecha() {
    DateUtils.setMaxDateDP(new Date(), this.config);
  }

  private configurarPaginacion(): void {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.prestamoService.consultarBandejaConformarPrestamo(filtros);
      }).share();
    (<Observable<BandejaConformarPrestamoResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((formularios) => {
        this.formularioResultados = formularios;
      });
  }

  private prepararConsultaFormularios() {
    let formModel = this.form.value;

    this.consulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
    this.consulta.cuilSolicitante = formModel.cuil;
    this.consulta.departamentoIds = this.departamentoIds;
    this.consulta.localidadIds = this.localidadIds;
    this.consulta.idEstadoFormulario = this.estadosIds;
    this.consulta.numeroFormulario = formModel.numeroFormulario;
    this.consulta.idLinea = this.lineaIds;
    this.consulta.cuil = formModel.cuil;
    this.consulta.nombre = formModel.nombre;
    this.consulta.apellido = formModel.apellido;
    this.consulta.dni = formModel.dni;
  }

  public agregarFormulariosParaAgrupar(idAgrupamiento: number) {
    this.prestamoService.obtenerFormulariosAgrupamiento(idAgrupamiento).subscribe((formularios) => {
      if (formularios) {
        this.verificarFormularios(formularios, idAgrupamiento);
      }
    });
  }

  public consultarFormularios(esNuevaConsulta: boolean, pagina?: number): void {
    if (esNuevaConsulta) {
      this.limpiarDatosPrevios();
    }
    this.primerFormularioSeleccionado = false;
    this.prepararConsultaFormularios();
    if (this.consulta.dni == null || this.consulta.dni === '') {
      if ((this.consulta.fechaDesde == null || this.consulta.fechaHasta == null)) {
        this.notificacionService.informar(['Debe ingresar fecha desde y fecha hasta, o un número de dni.']);
      } else {
        if (this.consulta.fechaHasta < this.consulta.fechaDesde) {
          this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
        } else {
          PrestamoService.guardarFiltrosConformacionPrestamo(this.consulta, this.formularioResultados, this.formulariosParaConformarPrestamo, this.idsAgrupamientoSeleccionados);
          this.paginaModificada.next(pagina);
        }
      }
    } else {
      PrestamoService.guardarFiltrosConformacionPrestamo(this.consulta, this.formularioResultados, this.formulariosParaConformarPrestamo, this.idsAgrupamientoSeleccionados);
      this.paginaModificada.next(pagina);
    }
  }

  public limpiarDatosPrevios() {
    this.formulariosParaConformarPrestamo = [];
    this.formularioResultados = [];
    this.idsAgrupamientoSeleccionados = [];
    this.primerFormularioSeleccionado = false;
  }

  public generarPrestamos() {
    if (!this.formulariosParaConformarPrestamo.length) {
      this.notificacionService.informar(['Debes seleccionar al menos un formulario']);
      return;
    }
    // Esto polemico, se agrega por si la persona hace muchos clicks en agregar préstamo en la grilla superior y carga el agrupamiento mas de una vez en la list que se envia al back.
    this.idsAgrupamientoSeleccionados = Array.from(new Set(this.idsAgrupamientoSeleccionados));
    this.notificacionService.confirmar('¿Desea confirmar la generación de los préstamos?')
      .result
      .then((res) => {
        if (res) {
          let observables = this.idsAgrupamientoSeleccionados.map((idAgrupamiento) => this.prestamoService.generarPrestamo(idAgrupamiento));
          Observable.forkJoin(observables).subscribe((results) => {
            let informarResultado = false;
            let idsAgrupamiento = results.map((result) => result.numeroPrestamo).join(',');
            let generados = this.idsAgrupamientoSeleccionados.filter((x) => idsAgrupamiento.includes(x));
            if (generados.length > 0) {
              this.imprimirTxt(generados.join(','), true);
              informarResultado = true;
            }
            let noGenerados = this.idsAgrupamientoSeleccionados.filter((x) => !idsAgrupamiento.includes(x));
            if (noGenerados.length > 0) {
              this.imprimirTxt(noGenerados.join(','), false);
            }
            if (informarResultado) {
              let mensaje = noGenerados.length > 0 ? 'Algunos préstamos se registraron con éxito.' : 'Todos los préstamos se registraron con éxito';
              this.notificacionService.informar([mensaje]).result.then(() => {
                this.router.navigate(['/bandeja-prestamos']);
              });
            } else {
              this.notificacionService.informar(['No se pudieron registrar los préstamos con éxito.']).result.then(() => {
                this.consultarFormularios(true);
              });
            }
          });
        }
      });
  }

  public imprimirTxt(idsAgrupamiento: string, generado: boolean) {
    this.prestamoService.imprimirTxtGenerarPrestamo(idsAgrupamiento, generado).subscribe((res) => {
      this.formulariosTxt.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
      this.reporteSource = this.formulariosTxt.getValue();
      let arrayBytes = this.base64ToBytes(res.blob);
      let blob = new Blob([arrayBytes], {type: ''});
      FileSaver.saveAs(blob, res.fileName);

    });
  }

  private base64ToBytes(base64) {
    let raw = window.atob(base64);
    let n = raw.length;
    let bytes = new Uint8Array(new ArrayBuffer(n));

    for (let i = 0; i < n; i++) {
      bytes[i] = raw.charCodeAt(i);
    }
    return bytes;
  }

  private verificarFormularios(formularios: FormularioPrestamo[], idAgrupamiento: number) {
    let mensajes = '';
    let esLineaConApoderado = false;

    formularios.forEach((formulario) => {
      if (!(formulario.idEstado === 3 || formulario.idEstado === 4 || formulario.idEstado === 6)) {
        if (mensajes) {
          mensajes += ', ';
        }
        mensajes += 'ID: ' + `${formulario.idFormulario}`;
        formularios.filter((x) => x.id === formulario.idFormulario);
      }
      if (formulario.esApoderado !== 1) {
        esLineaConApoderado = true;
      }
    });
    if (mensajes) {
      this.notificacionService.informar(['No se agregó el formulario seleccionado porque los siguientes formularios no se encuentran en estado iniciado: ', mensajes]);
      return;
    }

    if (esLineaConApoderado) {
      let poseeApoderado = false;
      formularios.forEach((formulario) => {
        if (formulario.esApoderado === 2) {
          poseeApoderado = true;
        }
      });
      if (!poseeApoderado) {
        this.notificacionService.informar(['El formulario seleccionado no posee un apoderado asignado.']);
        return;
      }
    }

    let formulariosIniciados = formularios.filter((x) => x.idEstado === 3).length;
    let minimoFormularios = formularios[0].minIntegrantes;
    if (formulariosIniciados < minimoFormularios) {
      this.notificacionService.informar(['Se necesitan al menos ' + minimoFormularios + ' formularios en estado iniciado para poder conformar el préstamo.']);
      return;
    }

    let existeForm = this.formulariosParaConformarPrestamo.find((x) => x.idFormulario === formularios[0].idFormulario);
    if (!existeForm) {
      this.formulariosParaConformarPrestamo.push(...formularios);
      this.idsAgrupamientoSeleccionados.push(idAgrupamiento);
    }
  }

  public quitarFormulariosParaConformarPrestamo(idAgrupamiento: number) {
    this.formulariosParaConformarPrestamo = this.formulariosParaConformarPrestamo.filter((x) => x.id !== idAgrupamiento);
    this.idsAgrupamientoSeleccionados = this.idsAgrupamientoSeleccionados.filter((x) => x !== idAgrupamiento);
  }

  public esSeleccionado(idAgrupamiento: number): boolean {
    const estaSeleccionado = this.idsAgrupamientoSeleccionados.find((x) => x === idAgrupamiento);
    return estaSeleccionado !== undefined;
  }

  private reestablecerFiltros() {
    let filtrosGuardados = PrestamoService.recuperarFiltrosConformacionPrestamo();
    if (!isEmpty(filtrosGuardados[0])) {
      this.consulta = filtrosGuardados[0];
      this.formularioResultados = filtrosGuardados[1];
      this.formulariosParaConformarPrestamo = filtrosGuardados[2];
      this.idsAgrupamientoSeleccionados = filtrosGuardados[3];

      this.consulta.fechaDesde = this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde;
      this.consulta.fechaHasta = this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta;
      this.crearForm();
      this.form.patchValue({
        fechaDesde: NgbUtils.obtenerNgbDateStruct(this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde),
        fechaHasta: NgbUtils.obtenerNgbDateStruct(this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta),
        origen: this.consulta.idOrigen,
        cuil: this.consulta.cuilSolicitante,
        dni: this.consulta.dni,
        nombre: this.consulta.nombre,
        apellido: this.consulta.apellido,
        linea: this.consulta.idLinea,
        departamento: this.consulta.departamentoIds,
        localidad: this.consulta.localidadIds,
        estadoFormulario: this.consulta.idEstadoFormulario
      });
      this.consultarFormularios(false);
    } else {
      this.form.patchValue({
        fechaDesde: NgbUtils.obtenerNgbDateStruct(new Date(Date.now())),
        fechaHasta: NgbUtils.obtenerNgbDateStruct(new Date(Date.now()))
      });
    }
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    if (!(this.router.url.includes('conformar-prestamos')
      || this.router.url.includes('prestamo/formulario'))) {
      PrestamoService.guardarFiltrosConformacionPrestamo(null, null, null, null);
    }
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos ? departamentos.join(',') : null;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades ? localidades.join(',') : null;
  }

  public guardarEstadosSeleccionadas(estados: string[]) {
    this.estadosIds = estados ? estados.join(',') : null;
  }

  public guardarLineasSeleccionadas(lineas: string[]) {
    this.lineaIds = lineas ? lineas.join(',') : null;
  }
}
