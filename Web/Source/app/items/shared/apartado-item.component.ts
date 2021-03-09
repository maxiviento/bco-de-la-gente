import { CustomValidators } from './../../shared/forms/custom-validators';
import { Input, Component, Output, EventEmitter, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { Item } from './modelo/item.model';
import { TipoItem } from './modelo/tipo-item.model';
import { ItemsService } from './items.service';
import { Recurso } from './modelo/recurso.model';
import { TipoDocumentacionService } from '../../shared/servicios/tipo-documentacion.service';
import { TipoDocumentacion } from '../../shared/modelo/tipo.documentacion.model';

@Component({
  selector: 'bg-apartado-item',
  templateUrl: './apartado-item.component.html',
  styleUrls: ['./apartado-item.component.scss'],
})

export class ApartadoItemComponent implements OnInit {
  @Input('item') public form: FormGroup;
  @Input() public tiposItem: TipoItem [];
  @Output() public aceptado = new EventEmitter<Item>();
  @Input() public poseeHijos: boolean = false;
  public items: Item[] = [];
  public recursos: Recurso [] = [];
  public tiposDocumentacion: TipoDocumentacion [] = [];
  public static subeArchivoValido: boolean = false;

  constructor(private itemsService: ItemsService,
              private tiposDocumentacionService: TipoDocumentacionService) {
  }

  public ngOnInit(): void {
    this.itemsService.consultarItemsPadre()
      .subscribe((items) => {
        this.items = items;
        this.itemsService.consultarRecursos()
          .subscribe((recursos) => {
            this.recursos = recursos;
            this.tiposDocumentacionService.obtenerTiposDocumentacion()
              .subscribe((tiposDocumentacion) => {
                this.tiposDocumentacion = tiposDocumentacion;
              });
          });
      });
  }

  public static nuevoFormGroup(item: Item = new Item(), tiposItem: TipoItem []): FormGroup {
    let configControl = new FormControl(false);
    let esItemPadreControl = new FormControl(!!item.idItemPadre);
    let esItemLinkControl = new FormControl(!!item.idRecurso);
    let idItemPadreControl = new FormControl(item.idItemPadre ? item.idItemPadre : null);
    let idRecursoControl = new FormControl(item.idRecurso ? item.idRecurso : null);
    let tiposItemFormArray = new FormArray((tiposItem || [])
      .map((tipoItem) => {
        if (tipoItem.esSeleccionado && tipoItem.nombre == 'CHECKLIST') {
          configControl.setValue(true);
        }
        return new FormGroup({
          id: new FormControl(tipoItem.id),
          nombre: new FormControl(tipoItem.nombre),
          seleccionado: new FormControl(tipoItem.esSeleccionado)
        });
      }));
    let subeArchivoControl = new FormControl(item.subeArchivo || false);
    let generaArchivoControl = new FormControl(item.generaArchivo || false);
    let idTipoDocumentacionCddControl = new FormControl(item.idTipoDocumentacionCdd);

    tiposItemFormArray
      .valueChanges
      .subscribe((value) => {
        if (value.find((tipo) => tipo.nombre == 'CHECKLIST').seleccionado == true) {
          configControl.setValue(true);
        } else {
          esItemPadreControl.setValue(false);
          esItemLinkControl.setValue(false);
          configControl.setValue(false);
        }
      });

    esItemLinkControl
      .valueChanges
      .subscribe((value) => {
        idRecursoControl.clearValidators();
        if (!value) {
          idRecursoControl.setValue(null);
        } else {
          idRecursoControl.setValidators(Validators.required);
        }
        idRecursoControl.updateValueAndValidity();
      });

    esItemPadreControl
      .valueChanges
      .subscribe((value) => {
        idItemPadreControl.clearValidators();
        if (!value) {
          idItemPadreControl.setValue(null);
        } else {
          idItemPadreControl.setValidators(Validators.required);
        }
        idItemPadreControl.updateValueAndValidity();
      });

    return new FormGroup({
      id: new FormControl(item.id),
      descripcion: new FormControl(item.descripcion,
        Validators.compose([Validators.required, Validators.maxLength(200), CustomValidators.validTextAndNumbers])),
      nombre: new FormControl(item.nombre,
        Validators.compose([Validators.required, Validators.maxLength(100), CustomValidators.validTextAndNumbers])),
      tiposItem: tiposItemFormArray,
      esItemPadre: esItemPadreControl,
      esItemLink: esItemLinkControl,
      idItemPadre: idItemPadreControl,
      idRecurso: idRecursoControl,
      subeArchivo: subeArchivoControl,
      generaArchivo: generaArchivoControl,
      idTipoDocumentacionCdd: idTipoDocumentacionCddControl,
      mostrarConfig: configControl
    });
  }

  public static obtenerItem(formGroup: FormGroup): Item {
    let formModel = formGroup.value;
    let item = new Item();
    item.id = formModel.id;
    item.nombre = formModel.nombre;
    item.descripcion = formModel.descripcion;
    item.tiposItem = formModel.tiposItem
      .filter((tipoItem) => tipoItem.seleccionado)
      .map((tipoItem) => new TipoItem(tipoItem.id, tipoItem.nombre));
    item.idItemPadre = formModel.idItemPadre;
    item.idRecurso = formModel.idRecurso;
    item.generaArchivo = formModel.generaArchivo;
    item.subeArchivo = formModel.subeArchivo;
    item.idTipoDocumentacionCdd = this.obtenerIdTipoDocumentacionCdd(formModel.idTipoDocumentacionCdd, formModel.subeArchivo);
    return item;
  }

  public static obtenerIdTipoDocumentacionCdd(idTipoDocumentacion, subeArchivo) {
    if (subeArchivo == true) {
      return idTipoDocumentacion == "null" ? null : idTipoDocumentacion;
    }
    else {
      return null;
    }
  }

  public prepararTiposItem(): TipoItem [] {
    return this.form.value.tiposItem
      .filter((tipoItem) => tipoItem.seleccionado);
  }

  public aceptar(): void {
    this.aceptado.emit(ApartadoItemComponent.obtenerItem(this.form));
  }

  public get tiposItemFormArray(): FormArray {
    return this.form.get('tiposItem') as FormArray;
  }

  public esValidoSubeArchivo(): boolean {
    if (this.form.get('subeArchivo').value == true) {
      return this.form.get('idTipoDocumentacionCdd').value != "null" && this.form.get('idTipoDocumentacionCdd').value != null;
    }
    return true;
  }

  public quitarPadreLista() {
    let idItem = this.form.get('id').value;
    this.items = this.items.filter((x) => x.id !== idItem);
  }
}
