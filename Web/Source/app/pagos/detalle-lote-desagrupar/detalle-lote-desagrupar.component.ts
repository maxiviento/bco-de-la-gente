import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-detalle-lote-desagrupar',
  templateUrl: './detalle-lote-desagrupar.component.html',
  styleUrls: ['detalle-lote-desagrupar.component.scss'],
})

export class DetalleLoteDesagruparComponent implements OnInit {
  public idLote: number;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Desagrupar lote de pagos ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
  }
}
