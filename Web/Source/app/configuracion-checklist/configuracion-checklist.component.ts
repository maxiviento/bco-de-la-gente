import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ConsultaConfiguracionChecklist } from './shared/modelo/consulta-configuracion-checklist.modelo';
import { Etapa } from '../etapas/shared/modelo/etapa.model';
import { Area } from '../areas/shared/modelo/area.model';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { AreasService } from '../areas/shared/areas.service';
import { EtapasService } from '../etapas/shared/etapas.service';
import { ItemConfiguracionChecklist } from './shared/modelo/item-configuracion-checklist.model';
import { ConfiguracionChecklistService } from './shared/configuracion-checklist.service';
import { isBoolean } from 'util';
import { NotificacionService } from '../shared/notificacion.service';
import { EtapaConfiguracionChecklist } from './shared/modelo/etapa-configuracion-checklist';
import { NgbTabset } from '@ng-bootstrap/ng-bootstrap';
import { LineaCombo } from '../formularios/shared/modelo/linea-combo.model';
import { EtapaEstadoLinea } from '../prestamos-checklists/shared/modelos/etapa-estado-linea.model';
import { EstadoPrestamo } from '../prestamos-checklists/shared/modelos/estado-prestamo.model';
import { PrestamoService } from '../shared/servicios/prestamo.service';
import { ConfiguracionEtapaEstadoLineaComponent } from './configuracion-etapa-estado-linea/configuracion-etapa-estado-linea.component';
import { VersionChecklist } from './shared/modelo/version-checklist.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-configuracion-checklist',
  templateUrl: './configuracion-checklist.component.html',
  styleUrls: ['./configuracion-checklist.component.scss']
})

export class ConfiguracionChecklistComponent implements OnInit {
  /* itemsSinConfigurar: Items/Requisitos de la primer tabla*/
  public itemsSinConfigurar: ItemConfiguracionChecklist[] = [];
  /* etapasOrdenadas: etapas que contienen listas de requisitos de la segunda tabla*/
  public etapasOrdenadas: EtapaConfiguracionChecklist[] = [];
  public requisitosOrdenados: ItemConfiguracionChecklist[] = [];
  public requisitosPlanos: ItemConfiguracionChecklist[] = [];
  public lineasCombo: LineaCombo[] = [];
  public etapasCombo: Etapa[] = [];
  public etapa: Etapa;
  public areasCombo: Area[] = [];
  public consultaConfiguracionChecklist: ConsultaConfiguracionChecklist = new ConsultaConfiguracionChecklist();
  public form: FormGroup;
  public formItems: FormGroup;
  public formRequisitos: FormGroup;
  private seEliminaronRequisitos: boolean = false;
  public formEstados: FormGroup;
  public estadosCombo: EstadoPrestamo[] = [];
  @ViewChild('tabSet') public tabSet: NgbTabset;
  public etapasEstadosLinea: EtapaEstadoLinea[] = [];
  public verTablaEtapaEstados: boolean = false;
  public requisitosCollapsed: boolean = false;
  public itemsCollapsed: boolean = true;
  public version: VersionChecklist;
  public editable: boolean = false;

  @ViewChild(ConfiguracionEtapaEstadoLineaComponent)
  public apartadoEtapaEstadoLinea: ConfiguracionEtapaEstadoLineaComponent;

  constructor(private fb: FormBuilder,
              private configuracionChecklistService: ConfiguracionChecklistService,
              private etapasService: EtapasService,
              private lineasService: LineaService,
              private areasService: AreasService,
              private prestamosService: PrestamoService,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Configuración Check-List ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.cargarCombos();
    this.crearForm();
    this.actualizarFormularios();
  }

  public setEtapa(idEtapa: number): void {
    this.form.get('idEtapa').setValue(idEtapa);
  }

  private cargarCombos() {
    this.etapasService.consultarEtapasCombo()
      .subscribe((resultado) => this.etapasCombo = resultado);
    this.lineasService.consultarLineasParaCombo()
      .subscribe((resultado) => this.lineasCombo = resultado.filter((linea) => linea.dadoDeBaja === false));
    this.areasService.consultarAreasCombo()
      .subscribe((resultado) => this.areasCombo = resultado);
    this.prestamosService.consultarEstadosPrestamo()
      .subscribe((estadosPrestamo) => this.estadosCombo = estadosPrestamo);
  }

  private crearForm(): void {
    let idEtapaFormControl = new FormControl(this.consultaConfiguracionChecklist.idEtapa, Validators.required);
    idEtapaFormControl.valueChanges
      .subscribe(() => this.validarRequisitosRegistrados());

    let idLineaFormControl = new FormControl(this.consultaConfiguracionChecklist.idLinea, Validators.required);
    idLineaFormControl.valueChanges
      .subscribe((value) => {
        if (!value) {
          this.itemsSinConfigurar = [];
          this.etapasOrdenadas = [];
          this.actualizarFormularios();
          this.etapasEstadosLinea = [];
          this.verTablaEtapaEstados = false;
          this.version = null;
        }
        if (value) {
          this.consultarEtapasEstadoLinea(value);
        }
        this.form.get('idEtapa').setValue(null, {emitEvent: false});
        this.validarRequisitosRegistrados();
      });
    this.form = this.fb.group({
      idLinea: idLineaFormControl,
      idEtapa: idEtapaFormControl
    });
  }

  private consultarEtapasEstadoLinea(idLinea: number): void {
    this.configuracionChecklistService.consultarEtapasEstadosLinea(idLinea).subscribe((etapasEstados) => {
      this.etapasEstadosLinea = etapasEstados;
      this.verTablaEtapaEstados = true;
      if (this.apartadoEtapaEstadoLinea) {
        this.apartadoEtapaEstadoLinea.reset(etapasEstados);
      }
    });
    this.consultarVersionChecklistLinea(idLinea);
  }

  /* Valida que no haya requisitos nuevos que no fueron registrados */
  public validarRequisitosRegistrados(): void {
    this.seEliminaronRequisitos = false;
    let hayRequisitosNuevos = this.etapasOrdenadasFormArray.controls
      .some((etapa) => (etapa.get('requisitosOrdenados') as FormArray).controls
        .some((requisito) => !requisito.get('idRequisito').value));
    if (hayRequisitosNuevos) {
      this.notificacionService.confirmar('Si no registran los items agregados, se perderá la configuración. ¿Desea continuar?')
        .result.then((value) => {
        if (value) {
          this.etapasOrdenadas = [];
          this.crearFormRequisitos(this.etapasOrdenadas);
          if (this.form.get('idLinea').value != null) {
            this.consultarItems();
          } else {
            this.itemsSinConfigurar = [];
            this.actualizarFormularios();
          }
        } else {
          this.form.get('idEtapa').setValue(this.consultaConfiguracionChecklist.idEtapa, {emitEvent: false});
          this.form.get('idLinea').setValue(this.consultaConfiguracionChecklist.idLinea, {emitEvent: false});
        }
      });
    } else {
      this.etapasOrdenadas = [];
      if (this.form.get('idLinea').value) {
        this.consultarItems();
      }
    }
  }

  private crearFormItems(items: ItemConfiguracionChecklist []): FormGroup {
    return this.fb.group({
      itemsSeleccionados: this.fb.array((items || []).map((item) =>
        this.crearItemsSeleccionadosForm(item))),
    });
  }

  private crearFormRequisitos(etapas: EtapaConfiguracionChecklist []): FormGroup {
    return this.formRequisitos = this.fb.group({
      etapas: this.fb.array((etapas || []).map((etapa) =>
        this.fb.group({
          id: [etapa.id],
          nombre: [etapa.nombre],
          requisitosOrdenados: this.fb.array((etapa.itemsEtapa || [])
            .map((item) => this.crearItemsSeleccionadosForm(item)))
        })))
    });
  }

  public getComboEtapasSiguientes(): Etapa[] {
    return this.etapasCombo.filter((e) => e.id != this.form.get('idEtapa').value);
  }

  private crearItemsSeleccionadosForm(item: ItemConfiguracionChecklist): FormGroup {
    let idAreaFormControl = new FormControl(item.idArea);
    let seleccionadoFormControl = new FormControl(!!item.idArea);
    idAreaFormControl.valueChanges.subscribe((value) => {
      seleccionadoFormControl.setValue(value !== 'null', {emitEvent: false});
      /*Si tiene hijos modificar todos sus hijos, sino su padre*/
      if (item.itemsHijos) {
        this.modificarSeleccionItemsHijos(item, value);
      } else {
        this.modificarSeleccionItemPadre(item.idItemPadre, value);
      }
    });
    seleccionadoFormControl.valueChanges.subscribe((value) => {
      if (!value) {
        idAreaFormControl.setValue(null, {emitEvent: false});
      }
      /*Si tiene hijos modificar todos sus hijos, sino su padre*/
      if (item.itemsHijos) {
        this.modificarSeleccionItemsHijos(item, value);
      }
    });
    return this.fb.group({
      id: [item.idItem],
      idRequisito: [item.idRequisito],
      idTipoRequisito: [item.idTipoRequisito],
      idItemPadre: [item.idItemPadre],
      nombre: [item.nombreItem],
      nombreEtapa: [item.nombreEtapa],
      nombreArea: [item.nombreArea],
      esSolic: [item.esDeSolicitante],
      esGarante: [item.esDeGarante],
      esChecklist: [item.esDeChecklist],
      idArea: idAreaFormControl,
      seleccionado: seleccionadoFormControl,
      itemsHijos: this.fb.array((item.itemsHijos || []).map((hijo) => {
        let idAreaHijoFormControl = new FormControl(hijo.idArea);
        let seleccionadoHijoFormControl = new FormControl(!!hijo.idArea);
        idAreaHijoFormControl.valueChanges.subscribe((value) => {
          seleccionadoHijoFormControl.setValue(value !== 'null', {emitEvent: false});
          this.modificarSeleccionItemPadre(hijo.idItemPadre, value);
        });

        seleccionadoHijoFormControl.valueChanges.subscribe((value) => {
          if (!value) {
            idAreaHijoFormControl.setValue(null, {emitEvent: false});
          }
          this.modificarSeleccionItemPadre(hijo.idItemPadre, value);
        });

        return this.fb.group({
          id: [hijo.idItem],
          idItemPadre: [hijo.idItemPadre],
          idRequisito: [hijo.idRequisito],
          idTipoRequisito: [hijo.idTipoRequisito],
          nombre: [hijo.nombreItem],
          nombreEtapa: [hijo.nombreEtapa],
          nombreArea: [hijo.nombreArea],
          esSolic: [hijo.esDeSolicitante],
          esGarante: [hijo.esDeGarante],
          esChecklist: [hijo.esDeChecklist],
          idArea: idAreaHijoFormControl,
          seleccionado: seleccionadoHijoFormControl,
          orden: [hijo.nroOrden]
        });
      }))
    });
  }

  private modificarSeleccionItemsHijos(itemPadre: ItemConfiguracionChecklist, value): void {
    if (isBoolean(value)) {
      value = 'null';
    }
    /*Modifico el combo de área y check de los hijos del item*/
    ((this.formItems.get('itemsSeleccionados') as FormArray)
      .controls.find((item) => item.get('id').value === itemPadre.idItem).get('itemsHijos') as FormArray)
      .controls.map((hijo) => {
      hijo.get('idArea').setValue(value, {emitEvent: false});
      hijo.get('seleccionado').setValue(value !== 'null', {emitEvent: false});
    });
  }

  private modificarSeleccionItemPadre(idItemPadre: number, value): void {
    let itemPadre = (this.formItems.get('itemsSeleccionados') as FormArray)
      .controls.find((item) => item.get('id').value === idItemPadre);
    let hijosSeleccionados = (itemPadre.get('itemsHijos') as FormArray).controls.filter((hijo) => hijo.get('seleccionado').value);
    /* Si hay un solo item hijo seleccionado, modifico al item padre */

    /* Si la cantidad de hijos seleccionados es 0 es porque se le deseleccionaron todos los hijos,
     * si es 1 hay que validar si es uno porque se selecciono al hijo
     * o si lo es porque quedó un solo hijo por deseleccionar otro */
    if ((hijosSeleccionados.length === 1 && this.validarSeleccionPadre(value)) || hijosSeleccionados.length === 0) {
      if (isBoolean(value)) {
        value = 'null';
      }
      /*Modifico el combo de área y check del item padre*/
      itemPadre.get('idArea').setValue(value, {emitEvent: false});
      itemPadre.get('seleccionado').setValue(value !== 'null', {emitEvent: false});
    }
  }

  private validarSeleccionPadre(value): boolean {
    if (isBoolean(value)) {
      return value;
    } else {
      return value !== 'null';
    }
  }

  public consultarItems() {
    this.prepararConsultaItem();
    return this.configuracionChecklistService
      .consultarItems({idLinea: this.form.get('idLinea').value, idEtapa: this.form.get('idEtapa').value})
      .subscribe((items) => {
        items.map((item) => {
          if (!item.idItemPadre) {
            /*Filtro todos los items hijos del padre de esa etapa*/
            item.itemsHijos = items.filter((itemHijo) =>
              itemHijo.idItemPadre === item.idItem); // && itemHijo.idEtapa === item.idEtapa
            /*Si el padre tiene una etapa asignada y el hijo también se incluyen los que coincidan, si el hijo no tiene etapa asignada se lo incluye también*/
            if (item.idEtapa) item.itemsHijos = item.itemsHijos.filter((itemHijo) => (itemHijo.idEtapa && itemHijo.idEtapa === item.idEtapa) || !itemHijo.idEtapa);
          }
        });
        items = items.filter((itemPadre) => !itemPadre.idItemPadre);
        /*Los items son los que no estan ordenados (no son requisitos)*/
        this.itemsSinConfigurar = items.filter((item) => !item.nroOrden);
        this.requisitosOrdenados = items.filter((item) => item.nroOrden);
        this.reclasificarRequisitosHijos();
        this.clasificarRequisitos();
        this.asignarOrdenesPadre();
        this.actualizarFormularios();
      });
  }

  /* Le asigno un orden, incrementados de a uno, a los items padre.
   Para que cuando toque ordenar a los padres, se los puedan mover de a uno.
   Luego se le asignará el orden correcto. */
  private asignarOrdenesPadre(): void {
    this.etapasOrdenadas.map((etapa) => {
      let i = 0;
      etapa.itemsEtapa.map((item) => {
        item.nroOrden = i++;
        // ordeno sus hijos tambíen si los tiene
        if (item.itemsHijos && item.itemsHijos.length) {
          item.itemsHijos.forEach((hijo) => hijo.nroOrden = i++);
        }
      });
    });
  }

  /* Si hay requisitos hijos que no estan configurados(con padres que sí),
   hay que crear al item padre con esos hijos dentro de itemsSinConfigurar*/
  private reclasificarRequisitosHijos(): void {
    this.requisitosOrdenados.filter((requisitoPadre) =>
      requisitoPadre.itemsHijos.some((requisitoHijo) =>
        !requisitoHijo.nroOrden)).forEach((requisito) => {
      let hijosSinConf = requisito.itemsHijos.filter((hijo) => !hijo.nroOrden);
      let hijosConfigurados = requisito.itemsHijos.filter((hijo) => hijo.nroOrden);
      hijosSinConf.forEach((x) => requisito.itemsHijos.splice(requisito.itemsHijos.indexOf(x)), 1);
      requisito.itemsHijos = hijosConfigurados;
      /* Copio el objeto requisito con sus hijos no configurados y lo agrego a itemsSinConfigurar */
      let requisitoSinConf = Object.assign({}, requisito);
      requisitoSinConf.idArea = undefined;
      requisitoSinConf.idEtapa = undefined;
      requisitoSinConf.nroOrden = undefined;
      requisitoSinConf.itemsHijos = hijosSinConf;
      this.itemsSinConfigurar.push(requisitoSinConf);
    });
  }

  /*Obtengo las etapas de los requisitos*/
  private clasificarRequisitos(): void {
    let unique = {};
    for (let i in this.requisitosOrdenados) {
      if (typeof(unique[this.requisitosOrdenados[i].idEtapa]) === 'undefined') {
        this.etapasOrdenadas.push(
          EtapaConfiguracionChecklist.construir(
            this.requisitosOrdenados[i].idEtapa, this.requisitosOrdenados[i].nombreEtapa));
      }
      unique[this.requisitosOrdenados[i].idEtapa] = 0;
    }
    this.obtenerEtapasConfiguradas();
  }

  private obtenerEtapasConfiguradas(): void {
    this.etapasOrdenadas
      .map((etapa) =>
        etapa.itemsEtapa = this.requisitosOrdenados.filter((requisito) =>
          etapa.id == requisito.idEtapa).map((req) =>
          ItemConfiguracionChecklist.construirRequisitoConfiguracion(
            req.idRequisito,
            req.idTipoRequisito,
            req.idItem,
            req.nombreItem,
            req.idItemPadre,
            req.idArea,
            req.nombreArea,
            req.esDeSolicitante,
            req.esDeGarante,
            req.esDeChecklist,
            req.itemsHijos
          )));
  }

  public get itemsFormArray(): FormArray {
    return this.formItems.get('itemsSeleccionados') as FormArray;
  }

  public get etapasOrdenadasFormArray(): FormArray {
    return this.formRequisitos.get('etapas') as FormArray;
  }

  private prepararConsultaItem(): void {
    let formModel = this.form.value;
    this.consultaConfiguracionChecklist = new ConsultaConfiguracionChecklist(
      formModel.idLinea,
      formModel.idEtapa
    );
  }

  public esItemPadre(item: FormGroup): string {
    return item.get('idItemPadre').value ? 'normal' : 'bold';
  }

  public opacidadCheckbox(item: FormGroup): number {
    return item.get('idItemPadre').value ? 0.5 : 1;
  }

  public agregarRequisitos(): void {
    this.consultaConfiguracionChecklist.idEtapa = this.form.get('idEtapa').value;

    this.itemsFormArray.controls.filter((item) =>
      item.get('seleccionado').value).map((item) => {

      /*Si todos sus hijos fueron seleccionados lo quito de la primer lista,
       sino creo un nuevo item padre y reparto sus hijos en las listas*/
      let hijos = (item.get('itemsHijos') as FormArray).controls;
      let hijosSeleccionados = (item.get('itemsHijos') as FormArray).controls
        .filter((hijo) => hijo.get('seleccionado').value);

      let indicePadre = this.itemsSinConfigurar.indexOf(this.itemsSinConfigurar.find((itemSinConfigurar) =>
        itemSinConfigurar.idItem == item.get('id').value));

      if (hijos.length === hijosSeleccionados.length) {
        this.itemsSinConfigurar.splice(indicePadre, 1);
      } else {
        hijosSeleccionados.forEach((hijoSeleccionado) => {
          let indiceHijo = this.itemsSinConfigurar[indicePadre].itemsHijos.indexOf(
            this.itemsSinConfigurar[indicePadre].itemsHijos
              .find((hijo) => hijo.idItem == hijoSeleccionado.get('id').value));
          this.itemsSinConfigurar[indicePadre].itemsHijos.splice(indiceHijo, 1);
        });

        /*Dejo únicamente los hijos seleccionados del padre para que se agregen a la segunda lista*/
        hijos.filter((hijo) => !hijo.get('seleccionado').value)
          .forEach((hijoNoSeleccionado) =>
            (item.get('itemsHijos') as FormArray).removeAt(
              (item.get('itemsHijos') as FormArray).controls.indexOf(hijoNoSeleccionado)));
      }
      item.get('nombreArea').setValue(this.obtenerNombreArea(item.get('idArea').value));
      (item.get('itemsHijos') as FormArray).controls.map((hijo) =>
        hijo.get('nombreArea').setValue(this.obtenerNombreArea(item.get('idArea').value)));
      /*Veo si la etapa ya existe en la segunda lista y sino la creo*/
      let nuevoRequisito = ItemConfiguracionChecklist.construirItemConfiguracion(item as FormGroup);

      let indiceEtapa = this.etapasOrdenadas.indexOf(this.etapasOrdenadas.find((etapa) =>
        etapa.id == this.form.get('idEtapa').value));
      if (indiceEtapa !== -1) {
        /*Si el item padre ya existe en la segunda lista, solo agregarle los hijos formGroup item*/
        let indiceRequisitoPadreOrdenado = this.etapasOrdenadas[indiceEtapa].itemsEtapa.indexOf(
          this.etapasOrdenadas[indiceEtapa].itemsEtapa.find((requisitoOrdenado) =>
            requisitoOrdenado.idItem == item.get('id').value));
        if (indiceRequisitoPadreOrdenado !== -1) {
          nuevoRequisito.itemsHijos.forEach((hijo) =>
            this.etapasOrdenadas[indiceEtapa].itemsEtapa[indiceRequisitoPadreOrdenado].itemsHijos.push(hijo));
        } else {
          this.etapasOrdenadas[indiceEtapa].itemsEtapa.push(nuevoRequisito);
        }
      } else {
        let id = this.form.get('idEtapa').value;
        this.etapasOrdenadas.push(EtapaConfiguracionChecklist.construir(id, this.obtenerNombreEtapa(id), [nuevoRequisito]));
      }
    });
    this.asignarOrdenesPadre();
    this.actualizarFormularios();
  }

  public obtenerNombreEtapa(idEtapa: number): string {
    return this.etapasCombo.find((etapa) => etapa.id == idEtapa).descripcion;
  }

  public obtenerNombreArea(idArea: number): string {
    return this.areasCombo.find((area) => area.id == idArea).descripcion;
  }

  public moverPadre(indicePadre: number, indiceEtapa: number, posicion: number): void {
    let itemPadre1 = this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre];
    let itemPadre2 = this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre + posicion];

    let ordenAnterior = itemPadre1.nroOrden;
    itemPadre1.nroOrden = itemPadre1.nroOrden + posicion;
    itemPadre2.nroOrden = ordenAnterior;
    this.ordenarItems(this.etapasOrdenadas[indiceEtapa].itemsEtapa);
    this.actualizarFormularios();
    this.seleccionarTabSet(indiceEtapa);
  }

  public moverHijo(indiceHijo: number, indicePadre: number, indiceEtapa: number, posicion: number): void {
    let item1 = this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos[indiceHijo];
    let item2 = this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos[indiceHijo + posicion];

    let ordenAnterior = item1.nroOrden;
    item1.nroOrden = item1.nroOrden + posicion;
    item2.nroOrden = ordenAnterior;
    this.ordenarItems(this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos);
    this.actualizarFormularios();
    this.seleccionarTabSet(indiceEtapa);
  }

  private seleccionarTabSet(indice: number): void {
    this.tabSet.select(indice.toString());
  }

  private ordenarItems(items: ItemConfiguracionChecklist[]): void {
    items.sort((item1, item2) => item1.nroOrden - item2.nroOrden);
  }

  public quitarRequisitoPadre(etapaFormGroup: FormGroup, requisitoFormGroup: FormGroup): void {
    /* Borro el requisito de la segunda tabla*/
    let indiceEtapa = this.etapasOrdenadas.indexOf(
      this.etapasOrdenadas.find((etapa) => etapa.id == etapaFormGroup.get('id').value));
    let indiceRequisito = this.etapasOrdenadas[indiceEtapa].itemsEtapa.indexOf(
      this.etapasOrdenadas[indiceEtapa].itemsEtapa.find((requisito) =>
        requisito.idItem == requisitoFormGroup.get('id').value));
    this.etapasOrdenadas[indiceEtapa].itemsEtapa.splice(indiceRequisito, 1);

    /* Borro la etapa si la etapa no tiene items*/
    if (this.etapasOrdenadas[indiceEtapa].itemsEtapa.length === 0) {
      this.etapasOrdenadas.splice(indiceEtapa, 1);
    }
    /* Agrego el mismo requisito y sus hijos a la primer tabla */
    this.reiniciarRequisito(requisitoFormGroup);
    (requisitoFormGroup.get('itemsHijos') as FormArray).controls
      .forEach((hijo) => this.reiniciarRequisito(hijo as FormGroup));

    /* Busco si existe el item padre en la primer tabla */
    let indiceItem = this.itemsSinConfigurar.indexOf(
      this.itemsSinConfigurar.find((item) => item.idItem == requisitoFormGroup.get('id').value));
    /* Entra si el item padre ya existe */
    if (indiceItem !== -1) {
      let hijos = (requisitoFormGroup.get('itemsHijos') as FormArray).controls.map((hijo) =>
        ItemConfiguracionChecklist.construirItemConfiguracion(hijo as FormGroup));
      hijos.forEach((hijo) => this.itemsSinConfigurar[indiceItem].itemsHijos.push(hijo));
    } else {
      /* Creo un nuevo padre y le pongo sus hijos */
      this.itemsSinConfigurar.push(ItemConfiguracionChecklist.construirItemConfiguracion(requisitoFormGroup));
    }
    this.asignarOrdenesPadre();
    this.actualizarFormularios();
    this.seEliminaronRequisitos = true;
  }

  public quitarRequisitoHijo(etapaFormGroup: FormGroup, requisitoPadre: FormGroup, requisitoHijo: FormGroup): void {
    /* Borro el requisito hijo de la segunda tabla*/
    let indiceEtapa = this.etapasOrdenadas.indexOf(
      this.etapasOrdenadas.find((etapa) => etapa.id == etapaFormGroup.get('id').value));
    let indicePadre = this.etapasOrdenadas[indiceEtapa].itemsEtapa.indexOf(
      this.etapasOrdenadas[indiceEtapa].itemsEtapa.find((requisito) =>
        requisito.idItem == requisitoPadre.get('id').value));
    let indiceHijo = this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos.indexOf(
      this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos.find((requisito) =>
        requisito.idItem == requisitoHijo.get('id').value));
    this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos.splice(indiceHijo, 1);

    /* Borro el item padre si no tiene mas hijos en la segunda tabla*/
    if (!this.etapasOrdenadas[indiceEtapa].itemsEtapa[indicePadre].itemsHijos.length) {
      this.etapasOrdenadas[indiceEtapa].itemsEtapa.splice(indicePadre, 1);
    }
    /* Borro la etapa si la etapa no tiene items*/
    if (this.etapasOrdenadas[indiceEtapa].itemsEtapa.length === 0) {
      this.etapasOrdenadas.splice(indiceEtapa, 1);
    }

    /* Creo un requisito padre con el hijo borrado y agrega a la primer tabla,
     si ya existia el item padre, agrega a sus hijos*/
    let indiceItem = this.itemsSinConfigurar.indexOf(this.itemsSinConfigurar.find((item) => item.idItem == requisitoPadre.get('id').value));
    /* Entra si el item padre ya existe */
    if (indiceItem !== -1) {
      this.reiniciarRequisito(requisitoHijo);
      this.itemsSinConfigurar[indiceItem].itemsHijos.push(ItemConfiguracionChecklist.construirItemConfiguracion(requisitoHijo));
    } else {
      /* Creo un nuevo padre y le pongo ese único hijo */
      this.reiniciarRequisito(requisitoPadre);
      this.reiniciarRequisito(requisitoHijo);
      let padre = ItemConfiguracionChecklist.construirItemConfiguracion(requisitoPadre);
      let hijo = ItemConfiguracionChecklist.construirItemConfiguracion(requisitoHijo);
      padre.itemsHijos = [hijo];
      this.itemsSinConfigurar.push(padre);
    }
    this.actualizarFormularios();
    this.seEliminaronRequisitos = true;
  }

  private reiniciarRequisito(requisito: FormGroup): void {
    requisito.patchValue({idArea: null, idEtapa: null, seleccionado: false, orden: null}, {emitEvent: false});
  }

  private actualizarFormularios(): void {
    this.formItems = this.crearFormItems(this.itemsSinConfigurar);
    this.formRequisitos = this.crearFormRequisitos(this.etapasOrdenadas);
  }

  private crearRequisitosOrdenados(): ItemConfiguracionChecklist[] {
    let requisitosPadre: ItemConfiguracionChecklist [] = [];
    this.etapasOrdenadasFormArray.controls.map((etapa) => {
      (etapa.get('requisitosOrdenados') as FormArray).controls.map((requisito) => {
        let item = ItemConfiguracionChecklist.construirItemConfiguracion(requisito as FormGroup);
        item.idEtapa = etapa.get('id').value;
        item.idLinea = this.form.get('idLinea').value;
        item.itemsHijos.map((hijo) => {
          hijo.idEtapa = etapa.get('id').value;
        });
        requisitosPadre.push(item);
      });
    });
    return requisitosPadre;
  }

  private ordenarRequisitos(): void {
    let nroOrden = 1;
    this.generarRequisitosPlano(this.crearRequisitosOrdenados());
    this.requisitosPlanos.map((requisito) => requisito.nroOrden = nroOrden++);
  }

  private generarRequisitosPlano(requisitosOrdenados: ItemConfiguracionChecklist[]): void {
    requisitosOrdenados.forEach((requisito) => {
      this.requisitosPlanos = this.requisitosPlanos.concat([requisito].concat(requisito.itemsHijos.map((hijo) => hijo)));
    });
  }

  public registrarConfiguracion(idEtapa: number, pasaEtapa?: boolean): void {
    if (!this.editable) {
      this.notificacionService.informar(['La versión actual está en uso, para poder registrar cambios debe crear una nueva versión']);
      return;
    }
    this.requisitosPlanos = [];
    this.ordenarRequisitos();
    this.requisitosPlanos = this.requisitosPlanos.filter((x) => x.idArea && x.idEtapa);
    this.configuracionChecklistService.registrarConfiguracion(this.requisitosPlanos, this.form.get('idLinea').value, idEtapa, this.apartadoEtapaEstadoLinea.etapasEstadosLinea, this.apartadoEtapaEstadoLinea.idsEtapasEliminadas)
      .subscribe(() => {
        this.obtenerNuevosIds();
        this.notificacionService.informar(['La configuración del checklist se registró con éxito.'])
          .result.then(() => {
          if (pasaEtapa) {
            this.apartadoEtapaEstadoLinea.siguienteEtapa();
          } else {
            this.form.get('idEtapa').setValue(null, {emitEvent: false});
            this.apartadoEtapaEstadoLinea.deseleccionarEtapa();
          }
          this.etapasOrdenadas = [];
          this.consultarItems();
        });
      }, (errores) => {
        this.notificacionService.informar(<string[]> errores, true);
      });
  }

  private obtenerNuevosIds(): void {
    this.etapasOrdenadas = [];
    this.crearFormRequisitos(this.etapasOrdenadas);
    if (this.form.get('idLinea').value != null) {
      this.consultarEtapasEstadoLinea(this.form.get('idLinea').value);
      this.consultarItems();
    } else {
      this.itemsSinConfigurar = [];
      this.actualizarFormularios();
    }
  }

  public pasarEtapa(): void {
    this.notificacionService.confirmar('Al pasar a configurar la siguiente etapa se guardarán los cambios automáticamente. ¿Desea pasar a la siguiente etapa?')
      .result
      .then((res) => {
        if (res) {
          this.registrarConfiguracion(this.form.get('idEtapa').value, true);
        }
      });
  }

  public actualizarConfiguracion(): void {
    /*-1 porque no tiene requisitos de una unica etapa*/
    this.registrarConfiguracion(-1);
  }

  /* Valida que haya algun item y en alguna etapa */
  public validarAceptar(): boolean {
    return !this.etapasOrdenadas.length || !this.etapasOrdenadas.some((etapa) => !!etapa.itemsEtapa.length);
  }

  public registrarValido(): boolean {
    return this.seAgregaronRequisitos() || this.seEliminaronRequisitos;
  }

  public pasarEtapaValido(): boolean {
    return this.registrarValido() && !this.apartadoEtapaEstadoLinea.ultimaEtapaSeleccionada();
  }

  private seAgregaronRequisitos(): boolean {
    let requisitos = this.crearRequisitosOrdenados();
    return requisitos.length && requisitos.length > 0;
  }

  private consultarVersionChecklistLinea(idLinea: number): void {
    this.configuracionChecklistService
      .consultarVersionChecklistVigente(idLinea)
      .subscribe((version) => {
        if (version) {
          this.version = version;
          this.editable = !version.enUso;
        } else {
          this.version = null;
          this.editable = true;
        }
      });
  }

  public nuevaVersion(): void {
    if (this.version && this.version.enUso) {
      this.editable = true;
      this.apartadoEtapaEstadoLinea.generarNuevaVersion();
    }
  }
}
