import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { TablasDefinidasService } from '../../tablas-definidas.service';
import { TablaDefinida } from '../../modelo/tabla-definida.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-consultar-parametro-tabla-definida',
  templateUrl: './consultar-parametro-tabla-definida.component.html',
  styleUrls: ['./consultar-parametro-tabla-definida.component.scss'],
  providers: []
})
export class ConsultarParametroTablaDefinidaComponent implements OnInit {
  public tabla: TablaDefinida;

  constructor(private route: ActivatedRoute,
              private tablaDefinidaService: TablasDefinidasService,
              private titleService: Title) {
    this.titleService.setTitle('Consultar parámetros de tablas definidas ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) => this.tablaDefinidaService.consultarParametro(+params['id']))
      .subscribe((tabla: TablaDefinida) => this.tabla = tabla);
  }
}
