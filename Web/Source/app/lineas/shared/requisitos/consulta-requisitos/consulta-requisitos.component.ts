import { Component, Input, OnInit } from '@angular/core';
import { RequisitosResultado } from '../../modelo/resultado-requisitos.model';
import { LineaService } from '../../../../shared/servicios/linea-prestamo.service';
import { ActivatedRoute } from '@angular/router';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ItemsService } from '../../../../items/shared/items.service';
import { TipoItem } from '../../../../items/shared/modelo/tipo-item.model';

@Component({
  selector: 'bg-consulta-requisitos',
  templateUrl: './consulta-requisitos.component.html',
  styleUrls: ['./consulta-requisitos.component.scss']
})

export class ConsultaRequisitosModal implements OnInit {
  @Input() public idLinea: number;
  @Input() public nombreLinea: string;

  public requisitos: RequisitosResultado[] = [];
  public tiposItem: TipoItem[] = [];

  constructor(public activeModal: NgbActiveModal,
              public lineaService: LineaService,
              public itemService: ItemsService,
              public route: ActivatedRoute,
              public notificacionService: NotificacionService) {
  }

  public ngOnInit(): void {
    this.itemService.consultarTiposItem().subscribe((resultado) =>
      this.tiposItem = resultado
    );

    this.lineaService.consultarRequisitosPorLinea(this.idLinea)
      .subscribe((resultado) => {
          this.requisitos = resultado;
        }, (errores) => this.notificacionService.informar(errores, true)
      );
  }

  public checkTipoItem(idItem: number, idTipoItem: number): boolean {
    let items = this.requisitos.filter((requisito) => requisito.idItem == idItem);

    if (items.length == 1) {
      let existeTipoItem = items[0].tiposItems.filter((tipoItem) => tipoItem.id == idTipoItem);
      return (existeTipoItem.length > 0);
    }
  }
}
