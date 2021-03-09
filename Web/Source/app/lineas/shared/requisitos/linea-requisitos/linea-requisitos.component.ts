import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { isUndefined } from 'util';
import { Requisito } from '../../modelo/requisito-linea';
import { ItemsService } from '../../../../items/shared/items.service';
import { Item } from '../../../../items/shared/modelo/item.model';
import { TipoItem } from '../../../../items/shared/modelo/tipo-item.model';
import { NotificacionService } from '../../../../shared/notificacion.service';

@Component({
  selector: 'bg-linea-requisitos',
  templateUrl: 'linea-requisitos.component.html',
  styleUrls: ['linea-requisitos.component.scss']
})

export class LineaRequisitosComponent implements OnInit {

  @Input() public requisitos: Requisito [];
  @Input() public esCreacionDeLinea: boolean;
  @Output() public aceptado: EventEmitter<Requisito []> = new EventEmitter<Requisito []>();
  @Output() public cancelado: EventEmitter<any> = new EventEmitter<any>();

  public form: FormGroup;
  public requisitosSeleccionados: Requisito[] = [];
  public items: Item[] = [];
  public tiposItem: TipoItem[] = [];
  public tiposItemFiltrados: TipoItem[] = [];

  public constructor(private fb: FormBuilder,
                     private itemService: ItemsService,
                     private notificacionService: NotificacionService) {
  }

  public ngOnInit(): void {
    this.crearForm();
    if (isUndefined(this.requisitos)) {
      this.requisitos = [];
    }

    if (isUndefined(this.requisitosSeleccionados)) {
      this.requisitosSeleccionados = [];
    }

    this.itemService.consultarTiposItem()
      .subscribe((resultado) => {
        this.tiposItem = resultado;
        this.tiposItemFiltrados = this.tiposItem.filter(t => {
          if (t.nombre != "CHECKLIST") {
            return t;
          }
        });
        this.itemService.consultarItemsPorTipoItem(this.esCreacionDeLinea)
          .subscribe((res) => {
            this.items = res;
            this.checkRequisitosSeleccionados();
            this.crearForm();
          });
      });
  }

  public checkRequisitosSeleccionados(): void {
    this.requisitos.forEach((requisito) => {
      this.items.forEach((item) => {
        if (item.id == requisito.item) {
          item.esSeleccionado = true;
        }
      });
    });
  }

  public crearForm(): void {
    this.form = this.fb.group({
      listaRequisitos: this.fb.array(this.items.map((item) =>
        new FormGroup({
          seleccionado: new FormControl(item.esSeleccionado),
          nombre: new FormControl(item.nombre),
          id: new FormControl(item.id)
        })
      ))
    });
  }

  public get getItems(): FormArray {
    return this.form.get('listaRequisitos') as FormArray;
  }

  public enviarSeleccionados() {
    this.prepararRequisitos();
    if (this.requisitosSeleccionados.length == 0) {
      this.notificacionService.informar(['Debe seleccionar al menos un requisito.']);
    } else {
      this.aceptado.emit(this.requisitosSeleccionados);
    }
  }

  public prepararRequisitos() {
    let formValue = this.form.value;
    formValue.listaRequisitos.forEach((item) => {
      if (item.seleccionado) {
        this.requisitosSeleccionados.push(new Requisito(item.id));
      }
    });
  }

  public checkTipoItem(idItem: number, idTipoItem: number): boolean {
    let items = this.items.filter((valor) => valor.id == idItem);

    if (items.length == 1) {
      let existeTipoItem = items[0].tiposItem.filter((tipoItem) =>
        tipoItem.id == idTipoItem);
      return (existeTipoItem.length > 0);
    }
  }

  public validarCantidadRequisitos(): boolean {
    return this.getItems.controls.filter((item) => item.value.seleccionado).length == 0;
  }

  public clickSeleccionarTodo(checked: boolean) {
    this.items.forEach((item) => item.esSeleccionado = checked);
    this.crearForm();
  }
}
