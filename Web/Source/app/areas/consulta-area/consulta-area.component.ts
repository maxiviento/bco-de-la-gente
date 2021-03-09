import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { AreasService } from '../shared/areas.service';
import { Area } from '../shared/modelo/area.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-area',
  templateUrl: './consulta-area.component.html',
  styleUrls: ['./consulta-area.component.scss'],
  providers: [AreasService]
})
export class ConsultaAreaComponent implements OnInit {
  public area: Area = new Area();

  constructor(private route: ActivatedRoute,
              private areasService: AreasService,
              private titleService: Title) {
    this.titleService.setTitle('Ver área ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) => this.areasService.consultarArea(+params['id']))
      .subscribe((area: Area) => this.area = area);
  }
}
