import { Component, OnDestroy, OnInit } from '@angular/core';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { BandejaRecuperoConsulta } from '../shared/modelo/bandeja-recupero-consulta.model';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BandejaRecuperoResultado } from '../shared/modelo/bandeja-recupero-resultado.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { RecuperoService } from '../shared/recupero.service';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { Router } from '@angular/router';
import { DateUtils } from '../../shared/date-utils';
import { NgbDatepickerConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboEntidadesRecupero } from '../shared/modelo/combo-entidades-recupero.model';
import { ModalTipoEntidadComponent } from '../modal-tipo-entidad/modal-tipo-entidad.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import { ArchivoRecupero } from "../shared/modelo/archivo-recupero.model";

@Component({
  selector: 'bg-bandeja-recupero',
  templateUrl: './bandeja-recupero.component.html',
  styleUrls: ['./bandeja-recupero.component.scss'],
})

export class BandejaRecuperoComponent implements OnInit, OnDestroy {
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consulta: BandejaRecuperoConsulta;
  public form: FormGroup;
  public CBTipoEntidad: ComboEntidadesRecupero[] = [];
  public bandejaResultados: BandejaRecuperoResultado[] = [];
  public formResultado: FormGroup;
  public archivo: any;

  constructor(private fb: FormBuilder,
              private recuperoService: RecuperoService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private modalService: NgbModal,
              private router: Router,
              private configFechas: NgbDatepickerConfig,
              private titleService: Title) {
    this.titleService.setTitle('Gestión de recupero ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.consulta = new BandejaRecuperoConsulta();
    this.consulta.fechaDesde = new Date(Date.now());
    this.consulta.fechaHasta = new Date(Date.now());
    this.cargarComboTipoEntidades();
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
    this.limitarFechas();
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    if (!(this.router.url.includes('ver-inconsistencia-recupero/'))) {
      RecuperoService.guardarFiltrosRecupero(null);
    }
  }

  private limitarFechas() {
    DateUtils.setMaxDateDP(new Date(), this.configFechas);
  }

  private cargarComboTipoEntidades() {
    this.recuperoService.obtenerComboTipoEntidadRecupero().subscribe((items) => {
      this.CBTipoEntidad = items;
    });
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
      idTipoEntidad: [this.consulta.idTipoEntidad]
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
        return this.recuperoService
          .consultarBandeja(filtros);
      })
      .share();
    (<Observable<BandejaRecuperoResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormResultado();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public consultar(pagina?: number) {
    this.prepararConsulta();
    if ((this.consulta.fechaDesde == null
      || this.consulta.fechaHasta == null)) {
      this.notificacionService.informar(['Debe ingresar fecha desde y fecha hasta.']);
    } else {
      if (this.consulta.fechaHasta < this.consulta.fechaDesde) {
        this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta']);
      } else {
        RecuperoService.guardarFiltrosRecupero(this.consulta);
        this.paginaModificada.next(pagina);
      }
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;

    this.consulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
    this.consulta.idTipoEntidad = formModel.idTipoEntidad;
  }

  private reestablecerFiltros() {
    let filtrosGuardados = RecuperoService.recuperarFiltrosRecupero();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.consulta.fechaDesde = this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde;
      this.consulta.fechaHasta = this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta;
      this.crearForm();
      this.consultar();
    }
  }

  private crearFormResultado() {
    this.formResultado = this.fb.group({
      resultados: this.fb.array((this.bandejaResultados || []).map((resultado) =>
        this.fb.group({
          idCabecera: [resultado.idCabecera],
          entidad: [resultado.entidad],
          fechaRecepcion: [resultado.fechaRecepcion],
          nombreArchivo: [resultado.nombreArchivo],
          cantTotal: [resultado.cantTotal],
          cantProc: [resultado.cantProc],
          cantEspec: [resultado.cantEspec],
          cantIncons: [resultado.cantIncons],
        })
      )),
    });
  }

  public get resultadoFormArray(): FormArray {
    return this.formResultado.get('resultados') as FormArray;
  }

  public archivoRecuperoSeleccionado(archivo: File): void {
    const modalTipoEntidad = this.modalService.open(ModalTipoEntidadComponent, {backdrop: 'static', size: 'lg'});
    modalTipoEntidad.componentInstance.nombreArchivo = archivo.name;
    modalTipoEntidad.result.then(
      (resultado) => {
        if (resultado) {
          let comando = new ArchivoRecupero();
          this.archivo = archivo;
          comando.Archivo = this.archivo;
          comando.idTipoEntidad = resultado.entidad;
          comando.fechaRecupero = resultado.fechaRecupero;
          comando.convenio = resultado.convenio;
          this.recuperoService.registrarArchivoRecupero(comando).subscribe((res) => {
              this.notificacionService.informar(
                [
                  'Cantidad total de registros: ' + res.cantTotal,
                  'Cantidad de registros procesados: ' + res.cantProc,
                  'Cantidad de registros inconsistentes: ' + res.cantIncons,
                  'Cantidad de registros especiales: ' + res.cantEspec,
                  'Monto procesado con éxito: $' + res.montoRecuperado
                ],
                false,
                'Archivo procesado con éxito');
            },
            (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      });
  }

  public ver(idCabecera: number) {
    this.router.navigate(['/ver-inconsistencia-recupero/', idCabecera]);
  }
}
