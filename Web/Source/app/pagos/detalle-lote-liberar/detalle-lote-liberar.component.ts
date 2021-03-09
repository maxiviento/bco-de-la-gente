import { FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PagosService } from '../shared/pagos.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { PermiteLiberarLoteResultado } from '../shared/modelo/permite-liberar-lote-resultado.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-detalle-lote-liberar',
  templateUrl: './detalle-lote-liberar.component.html',
  styleUrls: ['detalle-lote-liberar.component.scss'],
})

export class DetalleLoteLiberarComponent implements OnInit {
  public idLote: number;
  public puedeLiberarLote: boolean = true;

  constructor(private fb: FormBuilder,
              private route: ActivatedRoute,
              private router: Router,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Liberar lote de pagos ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idLote = +params['id'];
    });
    this.pagosService.permiteLiberarLote(this.idLote).subscribe(
      (res: PermiteLiberarLoteResultado) => {
        this.puedeLiberarLote = res.valor;
      }
    );
  }

  public liberar() {
    this.notificacionService.confirmar('Se liberará el lote. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        this.pagosService.liberarLote(this.idLote)
          .subscribe((res) => {
              if (res) {
                this.notificacionService.informar(['El lote fue liberado con éxito.'], false);
                this.router.navigate(['/bandeja-lotes']);
              }
            },
            ((errores) => {
              this.notificacionService.informar(errores, true);
            })
          );
      }
    });
  }

  public salir() {
    this.router.navigate(['/bandeja-lotes']);
  }
}
