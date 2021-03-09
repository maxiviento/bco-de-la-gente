import { MontoDisponibleService } from '../shared/monto-disponible.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { MontoDisponible } from '../shared/modelo/monto-disponible.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { BancoService } from '../../../shared/servicios/banco.service';
import { Router } from '@angular/router';
import { BusquedaSucursalBancariaComponent } from '../../../shared/componentes/busqueda-sucursal-bancaria/busqueda-sucursal-bancaria.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-monto-disponible',
  templateUrl: './nuevo-monto-disponible.component.html',
  styleUrls: ['./nuevo-monto-disponible.component.scss'],
  providers: [MontoDisponibleService]
})

export class NuevoMontoDisponibleComponent implements OnInit {
  public form: FormGroup;
  public formSucursales: FormGroup;
  public montoDisponible: MontoDisponible;
  @ViewChild(BusquedaSucursalBancariaComponent)
  public componenteSucursal: BusquedaSucursalBancariaComponent;
  public fechaMinimaDeposito: any;

  constructor(private fb: FormBuilder,
              private montoDisponibleService: MontoDisponibleService,
              private notificacionService: NotificacionService,
              private bancoService: BancoService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo monto disponible ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.fechaMinimaDeposito = NgbUtils.obtenerNgbDateStruct(new Date());
    this.montoDisponible = new MontoDisponible();
    this.crearForm();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      descripcion: [
        this.montoDisponible.descripcion,
        Validators.compose([
          Validators.required,
          Validators.maxLength(200)])
      ],
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
    });
  }

  public registrar(): void {
    if (!this.validarFechas()) {
      return;
    }

    if (this.form.valid && this.componenteSucursal.formValid()) {
      this.generarComando();
      this.montoDisponibleService.registrar(this.montoDisponible).subscribe(
        (montoDisponible) => {
          this.notificacionService.informar(Array.of('Registrado con éxito'), false);
          this.router.navigate(['/consulta-monto-disponible/', montoDisponible.idMontoDisponible]);
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        }
      );
    } else {
      this.form.updateValueAndValidity();
    }
  }

  private generarComando(): void {
    let formValue = this.form.value;
    this.montoDisponible.descripcion = formValue.descripcion;
    this.montoDisponible.fechaDepositoBancario = NgbUtils.obtenerDate(formValue.fechaDepositoBancario);
    this.montoDisponible.fechaInicioPago = NgbUtils.obtenerDate(formValue.fechaInicioPago);
    this.montoDisponible.fechaFinPago = NgbUtils.obtenerDate(formValue.fechaFinPago);
    this.montoDisponible.idBanco = this.componenteSucursal.getBancoId();
    this.montoDisponible.idSucursal = this.componenteSucursal.getSucursalId();
    this.montoDisponible.monto = formValue.monto;
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
      this.notificacionService.informar(Array.of('La fecha de inicio de pago debe ser menor a la de fin de pago.'), false);
      return false;
    }
    return true;
  }
}
