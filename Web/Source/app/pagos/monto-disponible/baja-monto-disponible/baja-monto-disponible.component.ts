import { MontoDisponibleService } from '../shared/monto-disponible.service';
import { Component, OnInit } from '@angular/core';
import { MontoDisponible } from '../shared/modelo/monto-disponible.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { BancoService } from '../../../shared/servicios/banco.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { MotivosBajaService } from '../../../shared/servicios/motivosbaja.service';
import { MotivoBaja } from '../../../shared/modelo/motivoBaja.model';
import { List } from 'lodash';
import { NotificacionService } from '../../../shared/notificacion.service';
import { BajaMontoDisponibleComando } from "../shared/modelo/baja-monto-disponible.model";
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-baja-monto-disponible',
  templateUrl: './baja-monto-disponible.component.html',
  styleUrls: ['./baja-monto-disponible.component.scss'],
  providers: [MontoDisponibleService]
})

export class BajaMontoDisponibleComponent implements OnInit {
  public form: FormGroup;
  public montoDisponible: MontoDisponible;
  public CBBanco: any;
  public CBSucursal: any;
  public motivosBaja: List<MotivoBaja> = [];

  constructor(private fb: FormBuilder,
              private montoDisponibleService: MontoDisponibleService,
              private bancoService: BancoService,
              private route: ActivatedRoute,
              private motivosBajaService: MotivosBajaService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Baja de monto disponible ' + TituloBanco.TITULO);
  }

  ngOnInit(): void {
    this.montoDisponible = new MontoDisponible();
    this.crearForm();
    this.bancoService.consultarBancos().subscribe((bancos) => {
      this.CBBanco = bancos;
      this.crearForm();

      this.route.params
        .switchMap((params: Params) =>
          this.montoDisponibleService.obtenerPorId(+params['id']))
        .subscribe((montoDisponible) => {
          this.montoDisponible = montoDisponible;
          this.crearForm();
          this.cargarSucursales();
        });

      this.motivosBajaService.consultarMotivosBaja()
        .subscribe((motivosBaja) => {
          this.motivosBaja = (motivosBaja);
        });
    });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      descripcion: [
        this.montoDisponible.descripcion],
      fechaDepositoBancario: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaDepositoBancario)],
      fechaInicioPago: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaInicioPago)],
      fechaFinPago: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaFinPago)],
      idBanco: [
        this.montoDisponible.idBanco],
      idSucursal: [
        this.montoDisponible.idSucursal],
      monto: [this.montoDisponible.monto],
      nroMonto: [this.montoDisponible.nroMonto],
      motivoBaja: ['', Validators.required],
    });
  }

  private cargarSucursales() {
    let idBanco = this.form.get('idBanco').value;
    if (idBanco) {
      this.bancoService.consultarSucursales(idBanco).subscribe((sucursales) => {
        this.CBSucursal = sucursales;
      });
    }
  }

  public darDeBaja(): void {
    let comando = new BajaMontoDisponibleComando(this.form.value.motivoBaja);
    this.montoDisponibleService.darDeBaja(this.montoDisponible.id, comando).subscribe(
      (dadoDeBaja) => {
        this.notificacionService
          .informar(['La operación se realizó con éxito.'])
          .result
          .then(() => {
            this.router.navigate(['/consulta-monto-disponible/', this.montoDisponible.id]);
          });
      },
      (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }
}
