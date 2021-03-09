import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MotivoBaja } from '../../shared/modelo/motivoBaja.model';
import { List } from 'lodash';
import { NotificacionService } from '../../shared/notificacion.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { MotivosBajaService } from '../../shared/servicios/motivosbaja.service';
import { ItemsService } from '../shared/items.service';
import { BajaItemComando } from '../shared/modelo/comando-baja-item.model';
import { Item } from '../shared/modelo/item.model';
import { TipoItem } from '../shared/modelo/tipo-item.model';
import { Recurso } from '../shared/modelo/recurso.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-item',
  styleUrls: ['./eliminacion-item.component.scss'],
  templateUrl: './eliminacion-item.component.html'
})

export class EliminacionItemComponent implements OnInit {
  public item: Item = new Item();
  public tiposItem: TipoItem [] = [];
  public motivosBaja: List<MotivoBaja> = [];
  public form: FormGroup;
  public fechaActual: Date;
  public esChecklist: boolean = false;
  public items: Item[] = [];
  public recursos: Recurso [] = [];

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private route: ActivatedRoute,
              private itemsService: ItemsService,
              private motivosBajaService: MotivosBajaService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Baja de ítem ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.fechaActual = new Date();

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
    this.motivosBajaService.consultarMotivosBaja()
      .subscribe((motivosBaja) => {
        this.motivosBaja = (motivosBaja);
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      motivoBaja: ['', Validators.required],
    });
  }

  public darDeBaja(): void {
    let comando = new BajaItemComando(this.form.value.motivoBaja);
    this.itemsService.darDeBajaItem(this.item.id, comando)
      .subscribe(() => {
          this.notificacionService
            .informar(['La operación se realizó con éxito.'])
            .result
            .then(() => {
              this.router.navigate(['/items', this.item.id]);
            });
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }
}
