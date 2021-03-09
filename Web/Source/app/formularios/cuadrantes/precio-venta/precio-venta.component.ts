import { CuadranteFormulario } from '../cuadrante-formulario';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../../shared/notificacion.service';
import { FormulariosService } from '../../shared/formularios.service';
import { Costo } from '../../shared/modelo/costo.model';
import { ActualizarPrecioVentaComando } from '../../shared/modelo/actualizar-precio-venta-comando.model';
import { EmprendimientoService } from '../../shared/emprendimiento.service';
import { TipoCosto } from '../../shared/modelo/tipo-costo.model';
import { ControlMessagesComponent } from '../../../shared/forms/control-messages.component';
import { PrecioVenta } from '../../shared/modelo/precio-venta.model';
import { CustomValidators, isEmpty } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-precio-venta',
  templateUrl: './precio-venta.component.html',
  styleUrls: ['./precio-venta.component.scss'],
})

export class PrecioVentaComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public costosFijos: Costo[] = [];
  public totalCostosFijos: number = 0;
  public costosVariables: Costo[] = [];
  public totalCostosVariables: number = 0;
  public ganancia: number = 0;
  public precioDeVentaUnitario: number = 0;
  public itemsPrecioVenta: TipoCosto[];
  public itemsPrecioVentaFijos: TipoCosto[] = [];
  public itemsPrecioVentaVariables: TipoCosto[] = [];
  public costosEliminados: number[] = [];

  public formNuevoCostoVariable: FormGroup;
  public formNuevoCostoFijo: FormGroup;

  constructor(private formulariosService: FormulariosService,
              private notificacionService: NotificacionService,
              private emprendimientoService: EmprendimientoService,
              private fb: FormBuilder) {
    super();
  }

  public ngOnInit(): void {
    this.actualizarListaCostos();
    this.crearFrom();
    this.consultarItemsPrecioVenta();
    if (!this.editable) {
      this.form.disable();
      this.formNuevoCostoFijo.disable();
      this.formNuevoCostoVariable.disable();
    }
  }

  private consultarItemsPrecioVenta() {
    this.emprendimientoService.consultarItemsPrecioVenta()
      .subscribe((items) => {
        this.itemsPrecioVenta = items;
        this.itemsPrecioVentaVariables = items.filter((x) => x.idTipo === 1);
        this.itemsPrecioVentaFijos = items.filter((x) => x.idTipo === 2);
        this.agregarCostoFijoPorPrestamo();
        this.calcularTotales();
        this.crearFrom();
      });
  }

  public actualizarDatos() {
    if (this.formulario.datosEmprendimiento.id) {
      let comando = this.armarComando();
      this.formulario.precioVenta.costos = comando.costos;
      this.formulariosService.actualizarCuadrantePrecioVenta(this.formulario.id, comando).subscribe(
        (res) => {
          this.formulario.precioVenta.idProducto = res;
          this.formulariosService.obtenerCostosActualizados(this.formulario.id).subscribe(
            (costosActualizados) => {
              this.formulario.precioVenta.costos = costosActualizados;
              this.actualizarListaCostos();
            }
          );
        }
      );
    }
  }

  public actualizarListaCostos() {
    if (this.formulario.precioVenta) {
      if (this.formulario.precioVenta.costos) {
        this.costosVariables = this.formulario.precioVenta.costos.filter((x) => x.idTipo === 1);
        this.costosFijos = this.formulario.precioVenta.costos.filter((x) => x.idTipo === 2);
        this.ganancia = this.formulario.precioVenta.gananciaEstimada;
      }
    } else {
      this.formulario.precioVenta = new PrecioVenta();
    }
  }

  public esValido(): boolean {
    return this.form.valid;
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }

  private crearFrom() {
    let gananciaFc = new FormControl(this.formulario.precioVenta.gananciaEstimada, Validators.compose([CustomValidators.minDecimalValue(0.1), Validators.maxLength(8)]));
    gananciaFc.valueChanges
      .distinctUntilChanged()
      .subscribe((value) => {
        if (gananciaFc.valid && gananciaFc.value) {
          this.ganancia = +value;
          this.guardarCabecera();
          this.calcularTotales();
        }
      });
    let unidadesEstimadasFC = new FormControl(
      this.formulario.precioVenta.unidadesEstimadas,
       Validators.compose([
         CustomValidators.minValue(1),
         Validators.maxLength(6)]));
    unidadesEstimadasFC.valueChanges
      .distinctUntilChanged()
      .subscribe(() => {
        if (unidadesEstimadasFC.valid && unidadesEstimadasFC.value) {
          this.guardarCabecera();
          this.recalcularCostosUnitarios();
          this.calcularTotales();
        }
      });
    let nombreProductoFC = new FormControl(this.formulario.precioVenta.producto,
      Validators.compose([
        Validators.maxLength(100),
        CustomValidators.validTextAndNumbers]));
    nombreProductoFC.valueChanges.distinctUntilChanged().subscribe(() => {
      if (nombreProductoFC.valid) {
        this.guardarCabecera();
      }
    });
    this.form = this.fb.group({
      unidadesEstimadas: unidadesEstimadasFC,
      producto: nombreProductoFC,
      costosVariables: this.fb.array(this.costosVariablesArray().map((costo) =>
        new FormGroup({
          id: new FormControl(costo.id),
          idTipo: new FormControl(costo.idTipo),
          nombreTipo: new FormControl(costo.nombre),
          detalle: new FormControl(costo.detalle),
          precioUnitario: new FormControl(costo.precioUnitario),
        })
      )),
      costosFijos: this.fb.array(this.costosFijosArray().map((costo) =>
        new FormGroup({
          id: new FormControl(costo.id),
          idTipo: new FormControl(costo.idTipo),
          nombreTipo: new FormControl(costo.nombre),
          detalle: new FormControl(costo.detalle),
          mensuales: new FormControl(costo.valorMensual),
          precioUnitario: new FormControl(costo.precioUnitario),
        })
      )),
      ganancia: gananciaFc,
    });
    this.formNuevoCostoVariable = this.fb.group({
      idTipo: ['', Validators.required],
      detalle: [''],
      mensuales: ['', Validators.compose([CustomValidators.minDecimalValue(0.1), Validators.maxLength(8), CustomValidators.decimalNumberWithTwoDigits])],
      precioUnitario: ['']
    });
    this.formNuevoCostoFijo = this.fb.group({
      idTipo: ['', Validators.required],
      detalle: [''],
      mensuales: ['', Validators.compose([CustomValidators.minDecimalValue(0.1), Validators.maxLength(8), CustomValidators.decimalNumberWithTwoDigits])],
      precioUnitario: [''],
    });
  }

  private costosVariablesArray(): Costo[] {
    if (!this.costosVariables) {
      return [];
    }
    if (!this.costosVariables.length) {
      return [];
    }
    return this.costosVariables;
  }

  private costosFijosArray(): Costo[] {
    if (!this.costosFijos) {
      return [];
    }
    if (!this.costosFijos.length) {
      return [];
    }
    return this.costosFijos;
  }

  private guardarCabecera() {
    this.formulario.precioVenta.unidadesEstimadas = this.form.get('unidadesEstimadas').value;
    this.formulario.precioVenta.producto = this.form.get('producto').value;
    this.formulario.precioVenta.gananciaEstimada = this.form.get('ganancia').value;
  }

  public agregarCostoVariable(form: any) {
    this.guardarCabecera();
    if (form.valid) {
      let detalle = form.get('detalle').value;
      let precioMensual = form.get('mensuales').value;
      let precioUnitario = 0;

      if (!(isEmpty(this.formulario.precioVenta.unidadesEstimadas) ||
        this.formulario.precioVenta.unidadesEstimadas === 0)) {
        precioUnitario = precioMensual / this.formulario.precioVenta.unidadesEstimadas;
      }
      let idTipoCosto = form.get('idTipo').value;
      let nombreTipo = this.itemsPrecioVentaVariables.filter((x) => x.id === idTipoCosto)[0].nombre;
      let costoNuevo = new Costo(null, 1, idTipoCosto, detalle, precioUnitario, precioMensual, nombreTipo);
      this.costosVariables.push(costoNuevo);
      this.crearFrom();
      this.calcularTotales();
    } else {
      ControlMessagesComponent.validarFormulario(form);
    }
  }

  public eliminarCostoVariable(costo: Costo) {
    this.costosVariables = this.costosVariables.filter((obj) => obj !== costo);
    if (costo.idTipo !== null) {
      this.costosEliminados.push(costo.id);
    }
    this.crearFrom();
    this.calcularTotales();
  }

  public agregarCostoFijo(form: any) {
    this.guardarCabecera();
    if (form.valid) {
      let detalle = form.get('detalle').value;
      let mensuales = form.get('mensuales').value;
      let precioUnitario = 0;

      if (!(isEmpty(this.formulario.precioVenta.unidadesEstimadas) ||
        this.formulario.precioVenta.unidadesEstimadas === 0)) {
        precioUnitario = mensuales / this.formulario.precioVenta.unidadesEstimadas;
      }

      let idTipoCosto = form.get('idTipo').value;
      let nombreTipo = this.itemsPrecioVentaFijos.filter((x) => x.id === idTipoCosto)[0].nombre;
      let costoNuevo = new Costo(null, 2, idTipoCosto, detalle, precioUnitario, mensuales, nombreTipo);
      this.costosFijos.push(costoNuevo);
      this.crearFrom();
      this.calcularTotales();
    } else {
      ControlMessagesComponent.validarFormulario(form);
    }
  }

  public eliminarCostoFijo(costo: Costo) {
    this.costosFijos = this.costosFijos.filter((obj) => obj != costo);
    if (costo.id != null) {
      this.costosEliminados.push(costo.id);
    }
    this.crearFrom();
    this.calcularTotales();
  }

  public calcularTotales() {
    this.totalCostosFijos = 0;
    this.totalCostosVariables = 0;
    this.costosFijos.forEach((x) => this.totalCostosFijos += x.precioUnitario);
    this.costosVariables.forEach((x) => this.totalCostosVariables += x.precioUnitario);
    this.precioDeVentaUnitario = this.totalCostosVariables + this.totalCostosFijos + this.ganancia;
  }

  public armarComando(): ActualizarPrecioVentaComando {
    let comando = new ActualizarPrecioVentaComando();
    comando.costos = [];
    comando.costosAEliminar = this.costosEliminados;
    comando.unidadesEstimadas = this.form.get('unidadesEstimadas').value;
    comando.producto = this.form.get('producto').value;
    comando.idEmprendimiento = this.formulario.datosEmprendimiento.id;
    comando.precioVenta = this.precioDeVentaUnitario;
    comando.gananciaEstimada = this.form.get('ganancia').value;
    comando.idProducto = this.formulario.precioVenta.idProducto;
    this.costosVariables.forEach((costo) => comando.costos.push(costo));
    this.costosFijos.forEach((costo) => comando.costos.push(costo));
    return comando;
  }

  private recalcularCostosUnitarios() {
    if (!(isEmpty(this.formulario.precioVenta.unidadesEstimadas) ||
      this.formulario.precioVenta.unidadesEstimadas === 0)) {
      for (let costo of this.costosVariables) {
        costo.precioUnitario = costo.valorMensual / this.formulario.precioVenta.unidadesEstimadas;
      }
      for (let costo of this.costosFijos) {
        costo.precioUnitario = costo.valorMensual / this.formulario.precioVenta.unidadesEstimadas;
      }
    }
  }

  private agregarCostoFijoPorPrestamo() {
    let existeCosto = this.costosFijos.find((x) => x.idItem === 9999);
    if (!existeCosto) {
      let costoPrestamo = this.itemsPrecioVentaFijos.find((x) => x.id === 9999);
      let cuotaPrestamo = 0;
      let costoUnitario = 0;

      if (!(isEmpty(this.formulario.condicionesSolicitadas.montoEstimadoCuota) ||
        this.formulario.condicionesSolicitadas.montoEstimadoCuota === 0)) {
        cuotaPrestamo = this.formulario.condicionesSolicitadas.montoEstimadoCuota;
      }

      if (!(isEmpty(this.formulario.precioVenta.unidadesEstimadas) ||
        this.formulario.precioVenta.unidadesEstimadas === 0)) {
        costoUnitario = cuotaPrestamo / this.formulario.precioVenta.unidadesEstimadas;
      }

      let costo = new Costo(null, costoPrestamo.idTipo, costoPrestamo.id, '', costoUnitario, cuotaPrestamo, costoPrestamo.nombre);
      this.costosFijos.push(costo);
    }
    let index = this.itemsPrecioVentaFijos.findIndex((x) => x.id === 9999);
    this.itemsPrecioVentaFijos.splice(index, 1);

  }
}
