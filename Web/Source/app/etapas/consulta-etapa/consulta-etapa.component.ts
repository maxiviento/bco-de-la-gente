import { Component, OnInit } from '@angular/core';
import { EtapasService } from '../shared/etapas.service';
import { Etapa } from '../shared/modelo/etapa.model';
import { ActivatedRoute, Params } from '@angular/router';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-etapa',
  templateUrl: './consulta-etapa.component.html',
  styleUrls: ['./consulta-etapa.component.scss'],
  providers: [EtapasService]
})

export class ConsultaEtapaComponent implements OnInit {
  public etapa: Etapa = new Etapa();

  constructor(private route: ActivatedRoute,
              private etapasService: EtapasService,
              private titleService: Title) {
    this.titleService.setTitle('Ver etapa ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) => this.etapasService.consultarEtapa(+params['id']))
      .subscribe((etapa: Etapa) => this.etapa = etapa);
  }
}
