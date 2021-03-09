import { Component, OnInit } from '@angular/core';
import { MontoDisponibleService } from '../shared/monto-disponible.service';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { MontoDisponible } from '../shared/modelo/monto-disponible.model';
import { BancoService } from '../../../shared/servicios/banco.service';
import { MovimientosMontoDisponible } from '../shared/modelo/movimientos-monto-disponible.model';
import { MovimientoMontoConsulta } from '../shared/modelo/movimiento-monto-consulta.model';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-monto-disponible',
  templateUrl: './consulta-monto-disponible.component.html',
  styleUrls: ['./consulta-monto-disponible.component.scss'],
  providers: [MontoDisponibleService]
})

export class ConsultaMontoDisponibleComponent implements OnInit {
  public form: FormGroup;
  public montoDisponible: MontoDisponible;
  public movimientos: MovimientosMontoDisponible[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consulta: MovimientoMontoConsulta;
  public CBBanco: any;
  public CBSucursal: any;

  constructor(private fb: FormBuilder,
              private montoDisponibleService: MontoDisponibleService,
              private bancoService: BancoService,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver monto disponible ' + TituloBanco.TITULO);
    this.consulta = new MovimientoMontoConsulta();
  }

  public ngOnInit(): void {
    this.montoDisponible = new MontoDisponible();
    this.configurarPaginacion();
    this.crearForm();
    this.bancoService.consultarBancos().subscribe((bancos) => {
      this.CBBanco = bancos;
      this.crearForm();

      this.route.params
        .switchMap((params: Params) =>
          this.montoDisponibleService.obtenerPorId(+params['id']))
        .subscribe((montoDisponible) => {
          this.montoDisponible = montoDisponible;
          this.consulta.idMonto = this.montoDisponible.id;
          this.paginaModificada.next(0);
          this.crearForm();
          this.cargarSucursales();
        });
    });
  }

  public consultar(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.montoDisponibleService
          .obtenerMovimientosMonto(filtros);
      })
      .share();
    (<Observable<MovimientosMontoDisponible[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((movimientos) => {
        this.movimientos = movimientos;
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      descripcion: [
        this.montoDisponible.descripcion,
        Validators.required],
      fechaDepositoBancario: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaDepositoBancario),
        Validators.required],
      fechaInicioPago: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaInicioPago),
        Validators.required],
      fechaFinPago: [
        NgbUtils.obtenerNgbDateStruct(this.montoDisponible.fechaFinPago),
        Validators.required],
      idBanco: [
        this.montoDisponible.idBanco,
        Validators.required],
      idSucursal: [
        this.montoDisponible.idSucursal,
        Validators.required],
      monto: [this.montoDisponible.monto,
        Validators.compose([
          Validators.required,
          CustomValidators.number,
          CustomValidators.minValue(1)])],
      nroMonto: [this.montoDisponible.nroMonto],
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
}
