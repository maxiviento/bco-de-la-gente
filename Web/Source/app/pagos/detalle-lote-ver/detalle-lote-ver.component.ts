import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-detalle-lote-ver',
  templateUrl: './detalle-lote-ver.component.html',
  styleUrls: ['detalle-lote-ver.component.scss'],
})

export class DetalleLoteVerComponent implements OnInit {
  public idLote: number;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Ver lote de pagos ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
  }

  public salir() {
    this.router.navigate(['/bandeja-lotes']);
  }
}
