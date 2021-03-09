import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { BandejaSuafConsulta } from '../../shared/modelo/bandeja-suaf-consulta.model';
import { DateUtils } from '../../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { ConsultaLoteSuafComponent } from './consulta-lote-suaf/consulta-lote-suaf.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-suaf',
  templateUrl: './bandeja-suaf.component.html',
  styleUrls: ['./bandeja-suaf.component.scss'],
})

export class BandejaSuafComponent implements OnInit, OnDestroy {

  public consulta: BandejaSuafConsulta;
  public form: FormGroup;
  public reporteSource: any;
  @ViewChild(ConsultaLoteSuafComponent)
  public componenteConsulta: ConsultaLoteSuafComponent;
  public totalizador: number;

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private config: NgbDatepickerConfig,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Gestión de lotes de SUAF ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.restablecerFiltros();
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    if (!((this.router.url.includes('bandeja-suaf'))
      || (this.router.url.includes('carga-manual-devengado'))
      || (this.router.url.includes('armar-lote-suaf'))
      || (this.router.url.includes('ver-lote-suaf'))
      || (this.router.url.includes('generar-lote-pago')))) {
      PagosService.guardarFiltrosSuaf(null);
    } else {
      PagosService.guardarFiltrosSuaf(this.consulta);
    }
  }

  private restablecerFiltros() {
    let filtrosGuardados = PagosService.recuperarFiltrosSuaf();
    if (filtrosGuardados) {
      this.consulta = filtrosGuardados;
      this.consulta.fechaDesde = this.consulta.fechaDesde ? new Date(this.consulta.fechaDesde) : this.consulta.fechaDesde;
      this.consulta.fechaHasta = this.consulta.fechaHasta ? new Date(this.consulta.fechaHasta) : this.consulta.fechaHasta;
      this.componenteConsulta.restablecerFiltros(Object.assign({}, this.consulta));
    }
  }

  public almacenarConsulta(consultaDeLoteSuaf?: BandejaSuafConsulta): void {
    this.consulta = consultaDeLoteSuaf;
  }

  public calcularTotalizador(totalizador: number): void {
    this.totalizador = totalizador;
  }
}
