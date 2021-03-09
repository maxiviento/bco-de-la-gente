import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LineaDetalleComponent } from '../shared/linea-detalle/linea-detalle.component';
import { IntegranteSocio } from '../shared/modelo/integrante-socio.model';
import { isUndefined } from 'util';
import { TiposFinanciamientoService } from '../shared/tipos-financiamiento.service';
import { IntegrantesService } from '../shared/integrantes.service';
import { TiposInteresService } from '../shared/tipos-interes.service';
import { TiposGarantiaService } from '../shared/tipos-garantia.service';
import { TipoFinanciamiento } from '../shared/modelo/tipo-financiamiento.model';
import { TipoInteres } from '../shared/modelo/tipo-interes.model';
import { TipoGarantia } from '../shared/modelo/tipo-garantia.model';
import { Convenio } from '../../shared/modelo/convenio-model';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';

@Component({
  selector: 'bg-modal-detalle-linea',
  templateUrl: './modal-detalle-linea.component.html',
  styleUrls: ['./modal-detalle-linea.component.scss'],
})

export class ModalDetalleLineaComponent implements OnInit {
  @Input() public detalleForm: FormGroup;
  @Input() public esEdicion: boolean = false;
  public form: FormGroup;
  public integrantes: IntegranteSocio[] = [];
  public financiamientos: TipoFinanciamiento[] = [];
  public intereses: TipoInteres[] = [];
  public garantias: TipoGarantia[] = [];
  public convenios: Convenio[] = [];
  public conveniosPago: Convenio[] = [];
  public conveniosRecupero: Convenio[] = [];

  constructor(public activeModal: NgbActiveModal,
              private integranteService: IntegrantesService,
              private tipoFinanciamientoService: TiposFinanciamientoService,
              private tipoInteresService: TiposInteresService,
              private tipoGarantiaService: TiposGarantiaService,
              private lineaService: LineaService) {
  }

  public ngOnInit(): void {
    this.cargarCombosDetalle();
    this.form = new FormGroup({
      detalle: this.detalleForm
    });
    this.cargarComboGarantia();
    this.recargarComboGarantia();
  }

  private recargarComboGarantia() {
    (this.detalleLineaForm.get('integranteSocio') as FormControl).valueChanges.subscribe(() => {
      this.detalleLineaForm.get('apoderado').setValue(false);
      (this.detalleLineaForm.get('tipoGarantia') as FormControl).setValue(null);
      this.cargarComboGarantia();
    });
  }

  private cargarComboGarantia() {
    let integranteSocio = (this.detalleLineaForm.get('integranteSocio') as FormControl);

    if (integranteSocio.value == 1) {
      this.tipoGarantiaService.consultarTiposGarantia()
        .subscribe((garantia) => this.garantias = garantia.filter((g) => (g.id == 1 || g.id == 2)));
    }
    if (integranteSocio.value == 2 || integranteSocio.value == 3) {
      this.tipoGarantiaService.consultarTiposGarantia()
        .subscribe((garantia) => this.garantias = garantia.filter((g) => (g.id == 3 || g.id == 4)));
    }
  }

  private cargarCombosDetalle() {
    this.lineaService.consultarConvenios()
      .subscribe((resultado) => {
        this.convenios = resultado;
        this.filtrarConvenios();
      });
    this.integranteService.consultarIntegrantes()
      .subscribe((integrante) => this.integrantes = integrante);
    this.tipoFinanciamientoService.consultarTiposFinanciamiento()
      .subscribe((financiamiento) => this.financiamientos = financiamiento);
    this.tipoInteresService.consultarTiposInteres()
      .subscribe((interes) => this.intereses = interes);
  }

  private filtrarConvenios() {
    this.conveniosPago = this.convenios.filter((c) => c.idTipoConvenio === 1);
    this.conveniosRecupero = this.convenios.filter((c) => c.idTipoConvenio === 2);
  }

  public cancelar(): void {
    this.activeModal.close();
  }

  public guardarDetalle(): void {
    let detalle = LineaDetalleComponent.obtenerDetalleLinea((this.form.get('detalle') as FormGroup));
    this.activeModal.close(detalle);
  }

  public get detalleLineaForm(): FormGroup {
    return this.form.get('detalle') as FormGroup;
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

}
