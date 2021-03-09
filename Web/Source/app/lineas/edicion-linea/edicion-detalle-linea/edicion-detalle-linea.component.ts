import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { DetalleLineaPrestamo } from '../../shared/modelo/detalle-linea-prestamo.model';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { List } from 'lodash';
import { MotivoBaja } from '../../../shared/modelo/motivoBaja.model';
import { MotivosBajaService } from '../../../shared/servicios/motivosbaja.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { IntegranteSocio } from '../../shared/modelo/integrante-socio.model';
import { TipoFinanciamiento } from '../../shared/modelo/tipo-financiamiento.model';
import { TipoInteres } from '../../shared/modelo/tipo-interes.model';
import { TipoGarantia } from '../../shared/modelo/tipo-garantia.model';
import { Convenio } from '../../../shared/modelo/convenio-model';
import { IntegrantesService } from '../../shared/integrantes.service';
import { TiposInteresService } from '../../shared/tipos-interes.service';
import { TiposFinanciamientoService } from '../../shared/tipos-financiamiento.service';
import { TiposGarantiaService } from '../../shared/tipos-garantia.service';
import { LineaDetalleComponent } from '../../shared/linea-detalle/linea-detalle.component';
import { isUndefined } from 'util';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bge-edicion-detalle-linea',
  templateUrl: './edicion-detalle-linea.component.html',
  styleUrls: ['./edicion-detalle-linea.component.scss']
})

export class EdicionDetalleLineaComponent implements OnInit {
  public detalleLinea: DetalleLineaPrestamo = new DetalleLineaPrestamo();
  public detalleForm: FormGroup;
  public motivosBaja: List<MotivoBaja> = [];
  public form: FormGroup;
  public fechaActual: Date = new Date();
  public integrantes: IntegranteSocio[] = [];
  public financiamientos: TipoFinanciamiento[] = [];
  public intereses: TipoInteres[] = [];
  public garantias: TipoGarantia[] = [];
  public convenios: Convenio[] = [];
  public conveniosPago: Convenio[] = [];
  public conveniosRecupero: Convenio[] = [];
  private titulo: string = 'Edición detalle de línea ' + TituloBanco.TITULO;

  constructor(private route: ActivatedRoute,
              private fb: FormBuilder,
              private motivoBajaService: MotivosBajaService,
              private notificacionService: NotificacionService,
              private integranteService: IntegrantesService,
              private tipoInteresService: TiposInteresService,
              private tipoFinanciamientoService: TiposFinanciamientoService,
              private tipoGarantiaService: TiposGarantiaService,
              private lineaService: LineaService,
              private router: Router,
              public titleService: Title) {
    this.titleService.setTitle(this.titulo);
  }

  public ngOnInit(): void {
    this.cargarCombos();
    this.consultarDetalle();
  }

  public get detalleLineaForm(): FormGroup {
    return this.form.get('detalle') as FormGroup;
  }

  private cargarCombos() {
    this.integranteService.consultarIntegrantes()
      .subscribe((integrante) => this.integrantes = integrante);
    this.tipoFinanciamientoService.consultarTiposFinanciamiento()
      .subscribe((financiamiento) => this.financiamientos = financiamiento);
    this.tipoInteresService.consultarTiposInteres()
      .subscribe((interes) => this.intereses = interes);
    this.tipoGarantiaService.consultarTiposGarantia()
      .subscribe((garantias) => this.garantias = garantias);
    this.lineaService.consultarConvenios()
      .subscribe((resultado) => {
        this.convenios = resultado;
        this.filtrarConvenios();
      });
    this.motivoBajaService.consultarMotivosBaja()
      .subscribe((motivos) => this.motivosBaja = motivos);
  }

  private filtrarConvenios() {
    this.conveniosPago = this.convenios.filter((c) => c.idTipoConvenio === 1);
    this.conveniosRecupero = this.convenios.filter((c) => c.idTipoConvenio === 2);
  }

  public consultarDetalle(): void {
    this.route.params
      .switchMap((params: Params) => this.lineaService.consultarDetalleLineaPorIdDetalle(+params['id']))
      .subscribe((resultado) => {
        this.detalleLinea = resultado;
        this.detalleLinea.tipoInteres = new TipoInteres(resultado.idTipoInteres, resultado.nombreTipoInteres);
        this.detalleLinea.tipoFinanciamiento = new TipoFinanciamiento(resultado.idTipoFinanciamiento, resultado.nombreTipoFinanciamiento);
        this.detalleLinea.integranteSocio = new IntegranteSocio(resultado.idSocioIntegrante, resultado.nombreSocioIntegrante);
        this.detalleLinea.tipoGarantia = new TipoGarantia(resultado.idTipoGarantia, resultado.nombreTipoGarantia);
        this.detalleLinea.convenioPago = new Convenio(resultado.codConvenioPag, resultado.nombreConvPag, 1);
        this.detalleLinea.convenioRecupero = new Convenio(resultado.codConvenioRec, resultado.nombreConvRec, 2);
        this.detalleForm = LineaDetalleComponent.nuevoFormGroup(this.detalleLinea);
        this.form = new FormGroup({detalle: this.detalleForm});
        this.cargarComboGarantia();
        this.recargarComboGarantia();
      });
  }

  public confirmarApoderado(confirmacion: boolean) {
    this.form.get('apoderado').setValue(confirmacion, {emitEvent: false});
  }

  public guardarDetalle(): void {
    let detalle = LineaDetalleComponent.obtenerDetalleLinea((this.form.get('detalle') as FormGroup));
    if (detalle) {
      this.lineaService.modificarDetalle(detalle)
        .subscribe(() => {
          this.notificacionService
            .informar(['El detalle de la línea de préstamo se modificó con éxito.'])
            .result
            .then(() => {
              this.router.navigate(['/bandeja-lineas']);
            });
        }, (errores) => this.notificacionService.informar(errores, true));
    }
  }

  public esDetalleValido(): boolean {
    let montoTope = (this.detalleLineaForm.get('montoTope') as FormControl).value;
    let montoPrestable = (this.detalleLineaForm.get('montoPrestable') as FormControl).value;
    let cantMin = (this.detalleLineaForm.get('cantidadMinimaIntegrantes') as FormControl).value;
    let cantMax = (this.detalleLineaForm.get('cantidadMaximaIntegrantes') as FormControl).value;
    let tipoFinanciamiento = (this.detalleLineaForm.get('tipoFinanciamiento') as FormControl).value;
    let tipoGarantia = (this.detalleLineaForm.get('tipoGarantia') as FormControl).value;
    let tipoInteres = (this.detalleLineaForm.get('tipoInteres') as FormControl).value;
    if (montoTope) {
      return (this.form.valid &&
        (parseFloat(montoTope) <= parseFloat(montoPrestable)) && (parseFloat(cantMin) <= parseFloat(cantMax)) && (!isUndefined(tipoFinanciamiento) && !isUndefined(tipoGarantia) && !isUndefined(tipoInteres)));
    } else {
      return (this.form.valid && (parseFloat(cantMin) <= parseFloat(cantMax)) && (!isUndefined(tipoFinanciamiento) && !isUndefined(tipoGarantia) && !isUndefined(tipoInteres)));
    }
  }

  private cargarComboGarantia() {
    let integranteSocio = (this.detalleLineaForm.get('integranteSocio') as FormControl);

    if (integranteSocio.value === 1) {
      this.tipoGarantiaService.consultarTiposGarantia()
        .subscribe((garantia) => this.garantias = garantia.filter((g) => (g.id === 1 || g.id === 2)));
    }
    if (integranteSocio.value === 2 || integranteSocio.value === 3) {
      this.tipoGarantiaService.consultarTiposGarantia()
        .subscribe((garantia) => this.garantias = garantia.filter((g) => (g.id === 3 || g.id === 4)));
    }
  }

  private recargarComboGarantia() {
    (this.detalleLineaForm.get('integranteSocio') as FormControl).valueChanges.subscribe(() => {
      this.detalleLineaForm.get('apoderado').setValue(false);
      this.cargarComboGarantia();
    });
  }
}
