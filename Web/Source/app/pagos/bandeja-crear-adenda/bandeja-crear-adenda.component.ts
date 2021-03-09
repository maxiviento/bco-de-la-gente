import {Component, OnInit} from '@angular/core';
import {Title} from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import {ActivatedRoute, Params} from "@angular/router";

@Component({
  selector: 'bg-bandeja-crear-adenda',
  templateUrl: './bandeja-crear-adenda.component.html',
  styleUrls: ['./bandeja-crear-adenda.component.scss'],
})

export class BandejaCrearAdendaComponent implements OnInit {

  public idLote: number;
  public nroDetalle: number;

  constructor(private route: ActivatedRoute,
              public titleService: Title) {
    this.titleService.setTitle('Bandeja crear adenda ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
  }

  public almacenarNroDetalle(nroDetalle: number) {
    this.nroDetalle = nroDetalle;
  }
}
