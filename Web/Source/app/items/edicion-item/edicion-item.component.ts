import { Component, OnInit } from '@angular/core';
import { Item } from '../shared/modelo/item.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../shared/notificacion.service';
import { ItemsService } from '../shared/items.service';
import { FormGroup } from '@angular/forms';
import { ApartadoItemComponent } from '../shared/apartado-item.component';
import { TipoItem } from '../shared/modelo/tipo-item.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-edicion-item',
  templateUrl: './edicion-item.component.html',
  styleUrls: ['./edicion-item.component.scss']
})

export class EdicionItemComponent implements OnInit {

  public item: Item;
  public form: FormGroup;
  public tiposItem: TipoItem [];
  public poseeHijos: boolean = false;

  constructor(private route: ActivatedRoute,
              private itemsService: ItemsService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Editar ítem ' + TituloBanco.TITULO);
  }

  private crearForm(): void {
    this.form = ApartadoItemComponent.nuevoFormGroup(this.item, this.tiposItem);
  }

  public ngOnInit(): void {

    this.crearForm();
    this.route.params
      .switchMap((params: Params) => this.itemsService.consultarItemPorId(+params['id']))
      .subscribe((item: Item) => {
        this.itemsService.consultarTiposItem().subscribe((tiposItem) => {
          this.tiposItem = tiposItem;
          item.tiposItem.map((tipo) =>
            this.tiposItem.find((value) =>
              value.id === tipo.id).esSeleccionado = true);
          this.form = ApartadoItemComponent.nuevoFormGroup(item, this.tiposItem);
          if (!item.idItemPadre) {
            this.verificarExistenciaHijos(item.id);
          }
        });
      });
  }

  public editar(item: Item): void {
    this.itemsService
      .editarItem(item)
      .subscribe(() => {
        this.notificacionService.informar(['El item se modificó con éxito.'])
          .result
          .then(() => this.router.navigate(['/items', item.id]));
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  public verificarExistenciaHijos(idItem: number) {
    this.itemsService.poseeHijos(idItem).subscribe((res) => {
      this.poseeHijos = res;
    });
  }
}
