import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormulariosService } from '../shared/formularios.service';
import { PrestamoService } from '../../shared/servicios/prestamo.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { BandejaFormularioConsulta } from '../shared/modelo/bandeja-formulario-consulta.model';
import { BandejaFormularioResultado } from '../shared/modelo/bandeja-formulario-resultado.model';
import { OrigenPrestamo } from '../shared/modelo/origen-prestamo.model';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { OrigenService } from '../shared/origen-prestamo.service';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { DateUtils } from '../../shared/date-utils';
import { BusquedaPorPersonaComponent } from '../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';
import { BusquedaPorPersonaConsulta } from '../../shared/modelo/busqueda-por-persona-consulta.model';
import { ColumnasFormularioEnum } from '../shared/modelo/columnas-formulario-enum.model';
import { ArchivoService } from '../../shared/archivo.service';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { MultipleSeleccionComponent } from '../../shared/multiple-seleccion/multiple-seleccion.component';
import { FiltroDomicilioBandejaComponent } from '../../shared/domicilio-bandeja/filtro-domicilio-bandeja.component';
import EstadosFormulario from '../shared/modelo/estados-formulario.enum';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-formulario',
  templateUrl: './bandeja-formulario.component.html',
  styleUrls: ['./bandeja-formulario.component.scss'],
})

export class BandejaFormularioComponent implements OnInit, OnDestroy {

  public formularioConsulta: BandejaFormularioConsulta;
  public formularioResultados: BandejaFormularioResultado[] = [];
  public form: FormGroup;
  public CBOrigen: OrigenPrestamo[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public banderaVerAcciones = false;
  private prestamoEnGeneracion = false;
  public columnasEnum = ColumnasFormularioEnum;
  public orderByDes = true;
  public columnaOrderBy = -1;
  public totalizador: number = 0;
  public pdfPagos = new BehaviorSubject<SafeResourceUrl>(null);
  public departamentoIds: string;
  public localidadIds: string;
  public estadosIds: string;
  public lineaIds: string;
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
              private formulariosService: FormulariosService,
              private origenesService: OrigenService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private router: Router,
              private prestamoService: PrestamoService,
              private archivoService: ArchivoService,
              private sanitizer: DomSanitizer,
              private titleService: Title) {
    this.titleService.setTitle('Bandeja de formularios ' + TituloBanco.TITULO);
    if (!this.formularioConsulta) {
      this.formularioConsulta = new BandejaFormularioConsulta();
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
    if (!(this.router.url.includes('formulario')
      || this.router.url.includes('seleccion-linea')
      || this.router.url.includes('actualizar-fecha-pago')
      || this.router.url.includes('actualizar-datos'))) {
      FormulariosService.guardarFiltros(null);
    }
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
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      origen: [this.formularioConsulta.idOrigen],
      numeroFormulario: [this.formularioConsulta.numeroFormulario,
        Validators.compose([CustomValidators.number, Validators.maxLength(14)])],
      nroPrestamo: [this.formularioConsulta.numeroPrestamo, Validators.compose([Validators.maxLength(8), CustomValidators.number])],
      nroSticker: [this.formularioConsulta.numeroSticker, Validators.compose([Validators.maxLength(14), CustomValidators.number])],
      dadoDeBaja: [false]
    });
  }

  private cargarCombos() {
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => this.CBOrigen = origenes);
  }

  private configurarPaginacion() {

    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this.formularioConsulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.formulariosService
          .consultarFormularios(filtros);
      })
      .share();
    (<Observable<BandejaFormularioResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((formularios) => {
        this.formularioResultados = formularios;
      });
  }

  public consultarPaginaSiguiente(pagina?: number) {
    this.prepararConsultaFormularios(pagina);
    if (this.formularioConsulta.dni == null || this.formularioConsulta.dni === '') {
      if ((this.formularioConsulta.fechaDesde == null
        || this.formularioConsulta.fechaHasta == null)) {
        this.notificacionService.informar(['Debe ingresar fecha desde y fecha hasta, o un número de dni.']);
      } else {
        if (this.formularioConsulta.fechaHasta < this.formularioConsulta.fechaDesde) {
          this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
        } else {
          FormulariosService.guardarFiltros(this.formularioConsulta);
          this.paginaModificada.next(pagina);
        }
      }
    } else {
      FormulariosService.guardarFiltros(this.formularioConsulta);
      this.paginaModificada.next(pagina);
    }
  }

  public consultarTotalizador(filtros: BandejaFormularioConsulta) {
    this.totalizador = 0;
    this.formulariosService
      .consultarTotalizador(filtros)
      .subscribe((num) => this.totalizador = num);
  }

  public consultarFormularios(consultarTotal: boolean, pagina?: number) {
    this.consultarPaginaSiguiente(pagina);
    if (consultarTotal) {
      this.consultarTotalizador(this.formularioConsulta);
    }
  }

  private prepararConsultaFormularios(pagina?: number) {
    let formModel = this.form.value;
    if (pagina) {
      this.formularioConsulta.numeroPagina = pagina;
    }
    this.formularioConsulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.formularioConsulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
    this.formularioConsulta.cuilSolicitante = formModel.cuil;
    this.formularioConsulta.departamentoIds = this.departamentoIds;
    this.formularioConsulta.localidadIds = this.localidadIds;
    this.formularioConsulta.idEstadoFormulario = this.estadosIds;
    this.formularioConsulta.numeroFormulario = formModel.numeroFormulario;
    this.formularioConsulta.idLinea = this.lineaIds;
    this.formularioConsulta.numeroPrestamo = formModel.nroPrestamo;
    this.formularioConsulta.numeroSticker = formModel.nroSticker;
    let consultaPersona = this.componentePersona.prepararConsulta();
    this.formularioConsulta.tipoPersona = consultaPersona.tipoPersona;
    this.formularioConsulta.cuil = consultaPersona.cuil;
    this.formularioConsulta.nombre = consultaPersona.nombre;
    this.formularioConsulta.apellido = consultaPersona.apellido;
    this.formularioConsulta.dni = consultaPersona.dni;
    this.formularioConsulta.idOrigen = formModel.origen;
    this.formularioConsulta.orderByDes = this.orderByDes;
    this.formularioConsulta.columnaOrderBy = this.columnaOrderBy;
  }

  public conformarPrestamo(id) {
    if (!this.prestamoEnGeneracion) {
      this.notificacionService.confirmar('¿Desea confirmar la generación del préstamo?')
        .result
        .then((res) => {
          if (res) {
            this.prestamoEnGeneracion = true;
            this.formulariosService
              .obtenerAgrupamientoFormulario(id)
              .subscribe((idAgrupamiento) =>
                  this.prestamoService.generarPrestamo(idAgrupamiento).subscribe((res) => {
                    let index = this.formularioResultados.findIndex((formulario) => formulario.id === id);
                    this.formularioResultados.splice(index, 1);
                    this.consultarFormularios(true);
                    this.notificacionService.informar([`El prestamo se generó con éxito con el número ${res.numeroPrestamo}.`]).result.then((resultado) => {
                      this.router.navigate(['/actualizar-checklist/' + res.idPrestamo]);
                    });
                  }, (errores) => {
                    this.prestamoEnGeneracion = false;
                    this.notificacionService.informar(errores, true);
                  }),
                (errores) => {
                  this.prestamoEnGeneracion = false;
                  this.notificacionService.informar(errores, true);
                });
          }
        });
    }
  }

  private limiteFecha() {
    DateUtils.setMaxDateDP(new Date(), this.config);
  }

  private reestablecerFiltros() {
    let filtrosGuardados = FormulariosService.recuperarFiltros();
    if (filtrosGuardados) {

      this.formularioConsulta = filtrosGuardados;
      this.comboEstado.setFiltros(this.formularioConsulta.idEstadoFormulario);
      this.comboLinea.setFiltros(this.formularioConsulta.idLinea);
      this.comboDptoLocalidad.setDeptos(this.formularioConsulta.departamentoIds);
      this.comboDptoLocalidad.setLocaldiades(this.formularioConsulta.localidadIds);
      this.componentePersona.setFiltros(new BusquedaPorPersonaConsulta(filtrosGuardados.tipoPersona, filtrosGuardados.cuil, filtrosGuardados.apellido, filtrosGuardados.nombre, filtrosGuardados.dni));
      this.formularioConsulta.fechaDesde = this.formularioConsulta.fechaDesde ? new Date(this.formularioConsulta.fechaDesde) : this.formularioConsulta.fechaDesde;
      this.formularioConsulta.fechaHasta = this.formularioConsulta.fechaHasta ? new Date(this.formularioConsulta.fechaHasta) : this.formularioConsulta.fechaHasta;
      this.localidadIds = this.formularioConsulta.localidadIds;
      this.departamentoIds = this.formularioConsulta.departamentoIds;
      this.estadosIds = this.formularioConsulta.idEstadoFormulario;
      this.lineaIds = this.formularioConsulta.idLinea;
      this.form.patchValue({
        fechaDesde: NgbUtils.obtenerNgbDateStruct(this.formularioConsulta.fechaDesde ? new Date(this.formularioConsulta.fechaDesde) : this.formularioConsulta.fechaDesde),
        fechaHasta: NgbUtils.obtenerNgbDateStruct(this.formularioConsulta.fechaHasta ? new Date(this.formularioConsulta.fechaHasta) : this.formularioConsulta.fechaHasta),
        origen: this.formularioConsulta.idOrigen,
        numeroFormulario: this.formularioConsulta.numeroFormulario,
        nroPrestamo: this.formularioConsulta.numeroPrestamo,
        nroSticker: this.formularioConsulta.numeroSticker
      });
      this.consultarFormularios(true, filtrosGuardados.numeroPagina);
    } else {
      this.form.patchValue({
        fechaDesde: NgbUtils.obtenerNgbDateStruct(new Date(Date.now())),
        fechaHasta: NgbUtils.obtenerNgbDateStruct(new Date(Date.now()))
      });
    }
  }

  public incluirDadosDeBaja() {
    let dadoDeBaja = this.form.get('dadoDeBaja').value;
    this.comboLinea.incluirDadosDeBaja(dadoDeBaja);
  }

  public imprimir(id: number) {
    this.router.navigate(['reporte-formulario/' + id]);
  }

  public reprogramar(id: number) {
    this.router.navigate(['actualizar-fecha-pago/' + id]);
  }

  public ordenarColumna(columna: number) {
    if (columna === this.columnaOrderBy) {
      this.orderByDes = !this.orderByDes;
      this.consultarFormularios(false);
    } else {
      this.orderByDes = true;
      this.columnaOrderBy = columna;
      this.consultarFormularios(false);
    }
  }

  public actualizarDatos(id: number) {
    this.router.navigate(['actualizar-datos/' + id]);
  }

  public imprimirBandeja(): void {
    this.generarExcel();
    this.generarPDF();
  }

  private generarExcel(): void {
    this.formulariosService
      .generarReporteBandejaExcel(this.formularioConsulta)
      .subscribe(
        (archivoReporte) => {
          this.archivoService.descargarArchivo(archivoReporte);
        }
      );
  }

  private generarPDF(): void {
    this.formulariosService.generarReporteBandejaPDF(this.formularioConsulta)
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
    this.estadosIds = estados ? estados.join(',') : null;
  }

  public guardarLineasSeleccionadas(lineas: string[]) {
    this.lineaIds = lineas ? lineas.join(',') : null;
  }
}
