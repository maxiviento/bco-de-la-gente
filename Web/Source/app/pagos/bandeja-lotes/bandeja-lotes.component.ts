import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { PagosService } from '../shared/pagos.service';
import { Router } from '@angular/router';
import { BandejaLotesConsulta } from '../shared/modelo/bandeja-lotes-consulta.model';
import { ConsultaBandejaLotesComponent } from './consulta-bandeja-lotes/consulta-bandeja-lotes.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-lotes',
  templateUrl: './bandeja-lotes.component.html',
  styleUrls: ['./bandeja-lotes.component.scss'],
})

export class BandejaLotesComponent implements OnInit, OnDestroy {
  public consulta: BandejaLotesConsulta;
  @ViewChild(ConsultaBandejaLotesComponent)
  public componenteConsulta: ConsultaBandejaLotesComponent;
  public totalizador: number;

  constructor(private pagosService: PagosService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Gestión de lotes de pagos ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.restablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!((this.router.url.includes('bandeja-lotes'))
      || (this.router.url.includes('pagos'))
      || (this.router.url.includes('detalle-lote-ver'))
      || (this.router.url.includes('desagrupar-lote'))
      || (this.router.url.includes('actualizar-sucursal'))
      || (this.router.url.includes('liberar-lote'))
      || (this.router.url.includes('agregar-lote'))
      || (this.router.url.includes('bandeja-crear-adenda'))
      || (this.router.url.includes('bandeja-agregar-prestamo')))) {
      PagosService.guardarFiltrosLotes(null);
    } else {
      PagosService.guardarFiltrosLotes(this.consulta);
    }
  }

  private restablecerFiltros() {
    let filtrosGuardados = PagosService.recuperarFiltrosLotes();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.consulta.fechaInicio = this.consulta.fechaInicio ? new Date(this.consulta.fechaInicio) : this.consulta.fechaInicio;
      this.consulta.fechaFin = this.consulta.fechaFin ? new Date(this.consulta.fechaFin) : this.consulta.fechaFin;
      this.componenteConsulta.restablecerFiltros(Object.assign({}, this.consulta));
    }
  }

  public almacenarConsulta(consultaBandejaLote?: BandejaLotesConsulta): void {
    this.consulta = consultaBandejaLote;
  }

  public calcularTotalizador(totalizador: number): void {
    this.totalizador = totalizador;
  }
}
