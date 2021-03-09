import { Component, OnDestroy, OnInit } from '@angular/core';
import { Item } from './shared/modelo/item.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ItemsService } from './shared/items.service';
import { NotificacionService } from '../shared/notificacion.service';
import { ConsultaItem } from './shared/modelo/consulta-item.model';
import { Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../shared/paginacion/pagina-utils';
import { Router } from '@angular/router';
import { TipoItem } from './shared/modelo/tipo-item.model';
import { CustomValidators, isEmpty } from '../shared/forms/custom-validators';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.scss']
})

export class ItemsComponent implements OnInit, OnDestroy {

  public items: Item[];
  public consultaItem: ConsultaItem;
  public form: FormGroup;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public nombreItems: string[] = [];
  public recursos: TipoItem[];
  public subItems: Item[];
  public itemsAFiltrar: Item[] = [];

  constructor(private fb: FormBuilder,
              private itemsService: ItemsService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Ítems ' + TituloBanco.TITULO);
    if (!this.items) {
      this.items = [];
    }
    if (!this.consultaItem) {
      this.consultaItem = new ConsultaItem();
    }
  }

  public ngOnInit() {
    this.obtenerNombreItems();
    this.obtenerRecursos();
    this.crearForm();
    this.configurarPaginacion();
    this.reestablecerFiltros();
  }

  public ngOnDestroy(): void {
    if (!this.router.url.includes('item')) {
      ItemsService.guardarFiltros(null);
    }
  }

  private crearForm(): void {
    let subItemFC = new FormControl(this.consultaItem.esSubitem);
    subItemFC.valueChanges.distinctUntilChanged().subscribe((value) => {
      if (value !== 'false') {
        this.form.get('incluirHijos').setValue(false);
        this.form.get('incluirHijos').disable();
        this.consultaItem.incluirHijos = false;
      } else {
        this.form.get('incluirHijos').enable();
      }
    });
    this.form = this.fb.group({
      nombre: [this.consultaItem.nombre,
        Validators.compose([
          Validators.maxLength(200),
          CustomValidators.validTextAndNumbers])],
      id: [this.consultaItem.idItem],
      incluirDadosBaja: [this.consultaItem.incluirDadosBaja],
      recurso: [this.consultaItem.recurso],
      esSubitem: subItemFC,
      incluirHijos: [this.consultaItem.incluirHijos]
    });


  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsultaItem();
        this.consultaItem.numeroPagina = params.numeroPagina;
        return this.itemsService
          .consultarItems(this.consultaItem);
      })
      .share();
    (<Observable<Item[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((items) => {
        this.items = items;
        if (!this.form.valid) {
          this.notificacionService
            .informar(['El nombre no puede superar los 50 caracteres.']);
        } else {
          if (this.items.length === 0) {
            this.notificacionService
              .informar(['No se encontraron registros para los criterios de búsqueda ingresados']);
          } else {
            ItemsService.guardarFiltros(this.consultaItem);
          }
        }
      });
  }

  private obtenerNombreItems(): void {
    this.itemsService.consultarItemsCombo().subscribe((items) => {
      items.forEach((x) => this.nombreItems.push(x.nombre));
      items.forEach((x) => {
        let it = new Item();
        it.nombre = x.nombre;
        it.id = x.id;
        this.itemsAFiltrar.push(it);
      });
    });
  }

  public searchItem = (text$: Observable<string>) =>
    text$
      .debounceTime(200)
      .distinctUntilChanged()
      .map((term) => term.length < 2 ? []
        : this.nombreItems.filter((item) =>
          item.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10));
  public showItem = (item: any) => item;

  public consultarItems(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private prepararConsultaItem(): void {
    let formModel = this.form.value;
    let id = -1;
    if (!isEmpty(formModel.nombre)) {
      let item = this.itemsAFiltrar.find((x) => x.nombre === formModel.nombre);
      if (!isEmpty(item)) {
        id = item.id;
      } else if (formModel.id !== -1) {
        id = formModel.id;
      }
    }

    this.consultaItem = new ConsultaItem(
      formModel.nombre,
      id,
      formModel.incluirDadosBaja,
      formModel.recurso,
      formModel.esSubitem,
      formModel.incluirHijos,
    );
  }

  private reestablecerFiltros() {
    let filtrosGuardados = ItemsService.recuperarFiltros();
    if (filtrosGuardados) {
      this.consultaItem = filtrosGuardados;
      this.form.patchValue(this.consultaItem);
      this.crearForm();
      this.consultarItems();
    }
  }

  private obtenerRecursos(): void {
    this.itemsService
      .consultarTiposItem()
      .subscribe((tiposItem) => {
        this.recursos = tiposItem;
      });
  }
}
