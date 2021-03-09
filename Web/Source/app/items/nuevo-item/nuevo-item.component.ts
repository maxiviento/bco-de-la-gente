import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Item } from '../shared/modelo/item.model';
import { ItemsService } from '../shared/items.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { Router } from '@angular/router';
import { ApartadoItemComponent } from '../shared/apartado-item.component';
import { TipoItem } from '../shared/modelo/tipo-item.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-item',
  templateUrl: './nuevo-item.component.html',
  styleUrls: ['./nuevo-item.component.scss']
})

export class NuevoItemComponent implements OnInit {
  public item: Item;
  public form: FormGroup;
  public tiposItem: TipoItem [];

  constructor(private itemsService: ItemsService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo ítem ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.itemsService
      .consultarTiposItem()
      .subscribe((tiposItem) => {
        this.tiposItem = tiposItem;
        this.form = ApartadoItemComponent.nuevoFormGroup(this.item, this.tiposItem);
      });
  }

  public registrar(item: Item): void {
    this.itemsService
      .registrarItem(item)
      .subscribe((resultado) => {
        this.notificacionService.informar(['El item se registró con éxito.'])
          .result
          .then(() => this.router.navigate(['/items', resultado.id]));
      }, (errores) => this.notificacionService.informar(errores, true));
  }
}
