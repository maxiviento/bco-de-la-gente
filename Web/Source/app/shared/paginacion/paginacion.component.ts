import { Component, Input, EventEmitter, Output, OnChanges, SimpleChanges } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Pagina } from './pagina-utils';

@Component({
  selector: 'paginacion',
  templateUrl: './paginacion.component.html',
  styleUrls: ['./paginacion.component.scss']
})
export class PaginacionComponent implements OnChanges {
  public ultimaPagina: boolean = true;
  public paginaActual: number = 0;

  @Input()
  public pagina: Pagina<any>;
  @Input()
  public mensajeAnterior: string = "Anterior";
  @Input()
  public mensajeSiguiente: string = "Siguiente";
  @Output()
  public paginaModificada: EventEmitter<number> = new EventEmitter<number>();

  constructor(protected location: Location,
              protected router: Router,
              protected route: ActivatedRoute) {
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.pagina.currentValue) {
      this.pagina = changes.pagina.currentValue;
      this.ultimaPagina = !this.pagina.tieneMasResultados;
      this.paginaActual = this.pagina.numeroPagina;
    }
  }

  public obtenerPagina(pagina: number) {
    this.paginaModificada.next(pagina);
  }

  public siguiente(): number {
    return this.paginaActual + 1;
  }

  public anterior(): number {
    return Math.max(0, this.paginaActual - 1);
  }
}
