import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Item } from '../shared/modelo/item.model';
import { ItemsService } from '../shared/items.service';
import { TipoItem } from '../shared/modelo/tipo-item.model';
import { Recurso } from '../shared/modelo/recurso.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-item',
  templateUrl: './consulta-item.component.html',
  styleUrls: ['./consulta-item.component.scss'],
  providers: [ItemsService]
})

export class ConsultaItemComponent implements OnInit {
  public item: Item = new Item();
  public tiposItem: TipoItem [] = [];
  public esChecklist: boolean = false;
  public items: Item[] = [];
  public recursos: Recurso [] = [];

  constructor(private route: ActivatedRoute,
              private itemsService: ItemsService,
              private titleService: Title) {
    this.titleService.setTitle('Ver ítem ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params
      .switchMap((params: Params) => this.itemsService.consultarItemPorId(+params['id']))
      .subscribe((item: Item) => {
        this.item = item;
        if (item.tiposItem.find((tipo) => tipo.nombre == 'CHECKLIST')) {
          this.esChecklist = true;
        }
        this.itemsService
          .consultarTiposItem()
          .subscribe((tiposItem) => {
            this.tiposItem = tiposItem;
            this.item.tiposItem.map((tipo) =>
              this.tiposItem.find((value) => value.id === tipo.id).esSeleccionado = true);

            this.itemsService.consultarItemsPadre()
              .subscribe((items) => {
                this.items = items;
                this.itemsService.consultarRecursos()
                  .subscribe((recursos) => this.recursos = recursos);
              });
          });
      });
  }
}

