import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BandejaRecuperoConsulta } from '../shared/modelo/bandeja-recupero-consulta.model';
import { RecuperoService } from '../shared/recupero.service';
import { Observable } from 'rxjs/Observable';
import { NotificacionService } from '../../shared/notificacion.service';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { Subject } from 'rxjs/Subject';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { ArchivoSuaf } from '../../etapas/shared/modelo/archivo-suaf.model';
import { BandejaResultadoBancoResultado } from '../shared/modelo/bandeja-resultado-banco-resultado.model';
import { Router } from '@angular/router';
import { DateUtils } from '../../shared/date-utils';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-resultado-banco',
  templateUrl: './bandeja-resultado-banco.component.html',
  styleUrls: ['./bandeja-resultado-banco.component.scss'],
})

export class BandejaResultadoBancoComponent implements OnInit, OnDestroy {
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consulta: BandejaRecuperoConsulta;
  public form: FormGroup;
  public bandejaResultados: BandejaResultadoBancoResultado[] = [];
  public formResultado: FormGroup;
  public archivo: any;

  constructor(private fb: FormBuilder,
              private recuperoService: RecuperoService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private router: Router,
              private configFechas: NgbDatepickerConfig,
              private titleService: Title) {
    this.titleService.setTitle('Bandeja de resultado de banco ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.consulta = new BandejaRecuperoConsulta();
    this.consulta.fechaDesde = new Date(Date.now());
    this.consulta.fechaHasta = new Date(Date.now());
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
    this.limitarFechas();
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    if (!(this.router.url.includes('ver-inconsistencia-resultado/'))) {
      RecuperoService.guardarFiltrosResultado(null);
    }
  }

  private limitarFechas() {
    DateUtils.setMaxDateDP(new Date(), this.configFechas);
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
          .consultarBandejaResultadoBanco(filtros);
      })
      .share();
    (<Observable<BandejaResultadoBancoResultado[]>> this.pagina.pluck(ELEMENTOS))
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
        RecuperoService.guardarFiltrosResultado(this.consulta);
        this.paginaModificada.next(pagina);
      }
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;

    this.consulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
  }

  private reestablecerFiltros() {
    let filtrosGuardados = RecuperoService.recuperarFiltrosResultado();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.consulta.fechaDesde = this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde;
      this.consulta.fechaHasta = this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta;
      this.crearForm();
      this.consultar();
    }
  }

  public get resultadoFormArray(): FormArray {
    return this.formResultado.get('resultados') as FormArray;
  }

  private crearFormResultado() {
    this.formResultado = this.fb.group({
      resultados: this.fb.array((this.bandejaResultados || []).map((resultado) =>
        this.fb.group({
          idCabecera: [resultado.idCabecera],
          fechaRecepcion: [resultado.fechaRecepcion],
          importe: [resultado.importe],
          periodo: [resultado.periodo],
          formaPago: [resultado.formaPago],
          tipoPago: [resultado.tipoPago],
          banco: [resultado.banco],
        })
      )),
    });
  }

  public archivoResultadoSeleccionado(archivo: File): void {
    let comando = new ArchivoSuaf();
    this.archivo = archivo;
    comando.Archivo = this.archivo;
    this.recuperoService.registrarArchivoResultadoBanco(comando).subscribe((res) => {
        if (res.coincidenMontos) {
          res.resultado = 'El monto de la cabecera coincide con la suma de los detalles';
        } else {
          res.resultado = 'El monto de la cabecera NO coincide con la suma de los detalles';
        }
        if (res.hayError) {
          res.resultado += ' y se encontraron errores en algunos registros';
        } else {
          res.resultado += ' y no se encontraron errores';
        }
        this.notificacionService.informar(
          [
            res.resultado
          ],
          false,
          'Archivo procesado con éxito');
      },
      (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public ver(idCabecera: number) {
    this.router.navigate(['/ver-inconsistencia-resultado/', idCabecera]);
  }
}
