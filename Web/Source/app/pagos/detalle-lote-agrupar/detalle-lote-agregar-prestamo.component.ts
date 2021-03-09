import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../shared/notificacion.service';
import { PagosService } from '../shared/pagos.service';
import { PermiteLiberarLoteResultado } from '../shared/modelo/permite-liberar-lote-resultado.model';
import { BandejaPagosResultado } from '../shared/modelo/bandeja-pagos-resultado.model';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'bg-detalle-lote-agregar',
  templateUrl: './detalle-lote-agregar-prestamo.component.html',
  styleUrls: ['detalle-lote-agregar-prestamo.component.scss'],
})

export class DetalleLoteAgregarPrestamoComponent implements OnInit {
  public idLote: number;
  public puedeAgregarLote: boolean = true;
  public formulariosCheckeados: BandejaPagosResultado[] = [];

  constructor(private route: ActivatedRoute,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Agregar préstamos al lote - Banco de la gente');
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
    this.pagosService.permiteLiberarLote(this.idLote).subscribe(
      (res: PermiteLiberarLoteResultado) => {
        this.puedeAgregarLote = res.valor;
      }
    );
  }

  public agregar() {
    this.router.navigate(['/bandeja-agregar-prestamo/' + this.idLote]);
  }
}
