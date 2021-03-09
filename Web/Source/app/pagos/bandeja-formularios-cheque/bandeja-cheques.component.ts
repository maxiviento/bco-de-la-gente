import { Component, OnInit, ViewChild } from '@angular/core';
import { PagosService } from '../shared/pagos.service';
import { ConsultaBandejaChequeComponent } from './consulta-bandeja-cheques/consulta-bandeja-cheque.component';
import { BandejaChequeConsulta } from '../shared/modelo/bandeja-cheque-consulta.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-cheques',
  templateUrl: './bandeja-cheques.component.html',
  styleUrls: ['./bandeja-cheques.component.scss'],
})

export class BandejaChequesComponent implements OnInit {

  public consulta: BandejaChequeConsulta;
  @ViewChild(ConsultaBandejaChequeComponent)
  public componenteConsulta: ConsultaBandejaChequeComponent;
  public totalizador: number;

  constructor(public titleService: Title) {
    this.titleService.setTitle('Bandeja cheques ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.restablecerFiltros();
  }

  private restablecerFiltros() {
    let filtrosGuardados = PagosService.recuperarFiltrosCheque();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.consulta.fechaDesde = this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde;
      this.consulta.fechaHasta = this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta;
      this.componenteConsulta.restablecerFiltros(Object.assign({}, this.consulta));
    }
  }

  public almacenarConsulta(consultaBandejaCheques?: BandejaChequeConsulta): void {
    this.consulta = consultaBandejaCheques;
  }

  public calcularTotalizador(totalizador: number): void {
    this.totalizador = totalizador;
  }

}
