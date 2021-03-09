import { MontoDisponibleService } from '../shared/monto-disponible.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { MontoDisponible } from '../shared/modelo/monto-disponible.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { BancoService } from '../../../shared/servicios/banco.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { EdicionMontoDisponibleComando } from '../shared/modelo/edicion-monto-disponible.model';
import { BusquedaSucursalBancariaComponent } from '../../../shared/componentes/busqueda-sucursal-bancaria/busqueda-sucursal-bancaria.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-monto-disponible',
  templateUrl: './edicion-monto-disponible.component.html',
  styleUrls: ['./edicion-monto-disponible.component.scss'],
  providers: [MontoDisponibleService]
})

export class EdicionMontoDisponibleComponent implements OnInit {
  public form: FormGroup;
  public montoDisponible: MontoDisponible;
  @ViewChild(BusquedaSucursalBancariaComponent)
  public componenteSucursal: BusquedaSucursalBancariaComponent;

  constructor(private fb: FormBuilder,
              private montoDisponibleService: MontoDisponibleService,
              private notificacionService: NotificacionService,
              private bancoService: BancoService,
              private route: ActivatedRoute,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Editar monto disponible ' + TituloBanco.TITULO);
  }

  ngOnInit(): void {
    this.montoDisponible = new MontoDisponible();
    this.crearForm();
    this.route.params
      .switchMap((params: Params) =>
        this.montoDisponibleService.obtenerPorId(+params['id']))
      .subscribe((montoDisponible) => {
        this.montoDisponible = montoDisponible;
        this.crearForm();
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      descripcion: [
        this.montoDisponible.descripcion,
        Validators.compose([
          Validators.required,
          Validators.maxLength(200)]
        )],
      fechaDepositoBancario: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaDepositoBancario),
        Validators.required],
      fechaInicioPago: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaInicioPago),
        Validators.required],
      fechaFinPago: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaFinPago),
        Validators.required],
      monto: [this.montoDisponible.monto,
        Validators.compose([
          Validators.required,
          CustomValidators.decimalNumberWithTwoDigits,
          CustomValidators.minDecimalValue(1),
          Validators.maxLength(11)])],
      nroMonto: [this.montoDisponible.nroMonto],
    });
    this.componenteSucursal.setBancoId(this.montoDisponible.idBanco);
    this.componenteSucursal.setSucursalId(this.montoDisponible.idSucursal);
  }

  public registrar(): void {
    if (!this.validarFechas()) {
      return;
    }

    if (this.form.valid && this.componenteSucursal.formValid()) {
      this.montoDisponibleService.modificar(this.montoDisponible.id, this.generarComando()).subscribe(
        (resultado) => {
          this.notificacionService.informar(Array.of(resultado.mensaje), false);
          if (resultado.ok) {
            this.router.navigate(['/consulta-monto-disponible/', resultado.idMontoDisponible]);
          }
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        }
      );
    } else {
      this.form.updateValueAndValidity();
    }
  }

  private generarComando(): EdicionMontoDisponibleComando {
    let comando = new EdicionMontoDisponibleComando();

    let formValue = this.form.value;
    comando.descripcion = formValue.descripcion;
    comando.fechaDepositoBancario = NgbUtils.obtenerDate(formValue.fechaDepositoBancario);
    comando.fechaInicioPago = NgbUtils.obtenerDate(formValue.fechaInicioPago);
    comando.fechaFinPago = NgbUtils.obtenerDate(formValue.fechaFinPago);
    comando.idBanco = this.componenteSucursal.getBancoId();
    comando.idSucursal = this.componenteSucursal.getSucursalId();
    comando.monto = formValue.monto;
    return comando;
  }

  private validarFechas(): boolean {
    let formValue = this.form.value;
    let fechaDepositoBancario = NgbUtils.obtenerDate(formValue.fechaDepositoBancario);
    let fechaInicioPago = NgbUtils.obtenerDate(formValue.fechaInicioPago);
    let fechaFinPago = NgbUtils.obtenerDate(formValue.fechaFinPago);

    if (fechaDepositoBancario > fechaInicioPago) {
      this.notificacionService.informar(Array.of('La fecha de deposito bancario no puede ser mayor a la de inicio de pago.'), false);
      return false;
    }

    if (fechaInicioPago > fechaFinPago) {
      this.notificacionService.informar(Array.of('La fecha de inicio de pago no puede ser mayor a la de fin de pago.'), false);
      return false;
    }
    return true;
  }
}
