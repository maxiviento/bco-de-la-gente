import { Component, ViewChild } from '@angular/core';
import { BandejaCambioEstadoConsulta } from '../shared/modelo/bandeja-cambio-estado-consulta.model';
import { ConsultaBandejaCambioEstadoComponent } from './consulta-grilla-cambio-estado/consulta-bandeja-cambio-estado.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-bandeja-cambio-estado',
  templateUrl: './bandeja-cambio-estado.component.html',
  styleUrls: ['./bandeja-cambio-estado.component.scss'],
})

export class BandejaCambioEstadoComponent {

  constructor(public titleService: Title) {
    this.titleService.setTitle('Bandeja cambio estado ' + TituloBanco.TITULO);
  }


  public consulta: BandejaCambioEstadoConsulta;
  @ViewChild(ConsultaBandejaCambioEstadoComponent)
  public componenteConsulta: ConsultaBandejaCambioEstadoComponent;
  public totalizador: number;

  public almacenarConsulta(consultaBandejaCambioEstado?: BandejaCambioEstadoConsulta): void {
    this.consulta = consultaBandejaCambioEstado;
  }

  public calcularTotalizador(totalizador: number): void {
    this.totalizador = totalizador;
  }
}
