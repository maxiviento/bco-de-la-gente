import { Component, OnInit } from '@angular/core';
import { EmprendimientoService } from '../../shared/emprendimiento.service';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormulariosService } from '../../shared/formularios.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { ItemMercadoComercializacion } from '../../shared/modelo/item-mercado-comercializacion.model';
import { EstimaCantClientes } from '../../shared/modelo/estima-cant-clientes.model';
import { FormaPagoItem } from '../../shared/modelo/forma-pago-item.model';
import { MercadoComercializacionComando } from '../../shared/modelo/mercado-comercializacion-comando.model';
import { ItemsSeleccionados } from '../../shared/modelo/items-seleccionados.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { AlMenosUnItemSeleccionadoValidador } from './item-mercado-pag-seleccionado-validator';
import { FormasPago } from '../../shared/modelo/formas-pago.model';
import { MercadoComercializacion } from '../../shared/modelo/mercado-comercializacion.model';

@Component({
  selector: 'bg-mercado-comercializacion',
  templateUrl: './mercado-y-comercializacion.component.html',
  styleUrls: ['./mercado-y-comercializacion.component.scss'],
  providers: [EmprendimientoService],
})

export class MercadoYComercializacionComponent extends CuadranteFormulario implements OnInit {
  public formItems: FormGroup;
  public formEstimaClientes: FormGroup;
  public formFormaPagos: FormGroup;
  public items: ItemMercadoComercializacion[] = [];
  public itemsCheckeados = [];
  public estimaClientes: EstimaCantClientes = new EstimaCantClientes();
  public formasPago: FormasPago = new FormasPago();
  public comando: MercadoComercializacionComando = new MercadoComercializacionComando();

  public constructor(private fb: FormBuilder,
    private notificacionService: NotificacionService,
    private emprendimientoService: EmprendimientoService) {
    super();
  }

  public ngOnInit(): void {
    if (!this.formulario.mercadoComercializacion) {
      this.formulario.mercadoComercializacion = new MercadoComercializacion();
      this.formulario.mercadoComercializacion.estimaClientes = new EstimaCantClientes();
    }
    if (this.formulario.mercadoComercializacion.estimaClientes == null) {
      this.formulario.mercadoComercializacion.estimaClientes = new EstimaCantClientes();
    }

    this.comando = new MercadoComercializacionComando();
    this.comando.estimaClientes = new EstimaCantClientes();
    this.comando.itemsCheckeados = [];
    this.comando.formasPago = [];

    this.crearForm();
    this.marcarEstimaClientes();
    this.consultarItems();
  }

  public esValido(): boolean {
    this.formItems.markAsDirty();
    const categorias = this.formItems.get('categorias') as FormArray;
    for (let categoria of categorias.controls) {
      categoria.markAsDirty();
    }

    this.formEstimaClientes.markAsDirty();
    this.formFormaPagos.markAsDirty();

    if (!this.editable) {
      return true;
    } else {
      return this.formItems.valid && this.formFormaPagos.valid;
    }
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }

  private crearForm(items: ItemMercadoComercializacion[] = []) {
    let categorias = this.fb.array([]);
    items.forEach((item) => {
      let categoria = categorias.controls.find((cat) => cat.get('nombre').value === item.nombreTipo);
      if (!categoria) {
        categoria = this.fb.group({
          nombre: item.nombreTipo,
          items: this.fb.array([])
        },
          { validator: AlMenosUnItemSeleccionadoValidador }
        );
        categorias.push(categoria);
      }

      let otrosDescripcion = item.descripcion;
      if (item.nombre.toString() === 'OTROS') {
        otrosDescripcion = this.encontrarDescripcionOtros(item.idCategoria);
      }

      let itemGroup = this.fb.group({
        id: [item.id],
        nombre: [item.nombre],
        descripcion: [otrosDescripcion, Validators.compose([
          Validators.maxLength(100), CustomValidators.validTextAndNumbers
        ])],
        seleccionado: [item.seleccionado],
      });
      if (item.nombre.toString() === 'OTROS') {
        (<FormGroup>categoria).addControl('otros', itemGroup);
      } else {
        (<FormArray>categoria.get('items')).push(itemGroup);
      }
    });
    this.formItems = this.fb.group({
      categorias,
    });

    this.formEstimaClientes = this.fb.group({
      estimaClientes: [
        this.formulario.mercadoComercializacion.estimaClientes.estima,
        Validators.required
      ],
      cantidad: [
        this.formulario.mercadoComercializacion.estimaClientes.cantidad,
        Validators.compose([Validators.required, Validators.maxLength(10), CustomValidators.number])
      ]
    });

    this.crearFormFormasDePago();

    if (!this.editable) {
      this.formEstimaClientes.disable();
      this.formItems.disable();
      this.formFormaPagos.disable();
    }
  }

  private crearFormFormasDePago(): void {
    this.formFormaPagos = this.fb.group({
      porcentajeContadoEfectivoCompra: [
        this.formasPago.porcentajeContadoEfectivoCompra,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(100),
          Validators.maxLength(3)])
      ],
      porcentajeCreditoProveedoresCompra: [
        this.formasPago.porcentajeCreditoProveedoresCompra,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(100),
          Validators.maxLength(3)])
      ],
      creditoProveedoresPlazoPagoCompra: [
        this.formasPago.creditoProveedoresPlazoPagoCompra,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(31),
          Validators.maxLength(3)])
      ],
      porcentajeOtraFormaPagoCompra: [
        this.formasPago.porcentajeOtraFormaPagoCompra,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(100),
          Validators.maxLength(3)])
      ],
      otraFormaPagoPlazoCompra: [
        this.formasPago.otraFormaPagoPlazoCompra,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(31),
          Validators.maxLength(3)])
      ],
      porcentajeContadoEfectivoVenta: [
        this.formasPago.porcentajeContadoEfectivoVenta,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(100),
          Validators.maxLength(3)])
      ],
      porcentajeCreditoProveedoresVenta: [
        this.formasPago.porcentajeCreditoProveedoresVenta,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(100),
          Validators.maxLength(3)])
      ],
      creditoProveedoresPlazoPagoVenta: [
        this.formasPago.creditoProveedoresPlazoPagoVenta,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(31),
          Validators.maxLength(3)])
      ],
      porcentajeOtraFormaPagoVenta: [
        this.formasPago.porcentajeOtraFormaPagoVenta,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(100),
          Validators.maxLength(3)])
      ],
      otraFormaPagoPlazoVenta: [
        this.formasPago.otraFormaPagoPlazoVenta,
        Validators.compose([
          CustomValidators.minDecimalValue(0),
          CustomValidators.maxDecimalValue(31),
          Validators.maxLength(3)])
      ],
    });
  }


  public categorias() {
    return this.formItems.get('categorias') as FormArray;
  }

  private encontrarDescripcionOtros(idCategoria: number): string {
    if (this.formulario.mercadoComercializacion.itemsPorCategoria) {
      let seleccionadosCategoria = this.formulario.mercadoComercializacion.itemsPorCategoria.filter(
        (x) => x.tipoItem === idCategoria);
      return seleccionadosCategoria[0] ? seleccionadosCategoria[0].descripcion : '';
    } else {
      return '';
    }
  }

  private consultarItems() {
    this.emprendimientoService.consultarItemsMercadoYComercializacion()
      .subscribe(((res) => {
        this.items = res;
        if (this.formulario.mercadoComercializacion.itemsPorCategoria) {
          this.marcarItemsSeleccionados();
          this.marcarFormasPago();
        }
        this.crearForm(this.items);
      }),
        ((error) => {
          this.notificacionService.informar(error, true);
        }));
  }

  private marcarItemsSeleccionados() {
    if (this.formulario.mercadoComercializacion) {
      this.formulario.mercadoComercializacion.itemsPorCategoria.forEach((itemPorCategoria) => {
        itemPorCategoria.items.forEach((item) => {
          this.items.filter((x) => x.id === item)[0].seleccionado = true;
        });
      });
    }
  }

  private marcarEstimaClientes() {
    if (this.formulario.mercadoComercializacion) {
      this.estimaClientes = this.formulario.mercadoComercializacion.estimaClientes;
      this.comando.estimaClientes.estima = this.formulario.mercadoComercializacion.estimaClientes.estima;
    }
  }

  private marcarFormasPago() {
    if (this.formulario.mercadoComercializacion.formasPago.length !== 0) {
      this.formasPago.porcentajeContadoEfectivoCompra = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 1 && x.tipo === '1')[0].valor;
      this.formasPago.porcentajeCreditoProveedoresCompra = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 2 && x.tipo === '1')[0].valor;
      this.formasPago.creditoProveedoresPlazoPagoCompra = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 3 && x.tipo === '1')[0].valor;
      this.formasPago.porcentajeOtraFormaPagoCompra = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 4 && x.tipo === '1')[0].valor;
      this.formasPago.otraFormaPagoPlazoCompra = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 5 && x.tipo === '1')[0].valor;

      this.formasPago.porcentajeContadoEfectivoVenta = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 1 && x.tipo === '2')[0].valor;
      this.formasPago.porcentajeCreditoProveedoresVenta = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 2 && x.tipo === '2')[0].valor;
      this.formasPago.creditoProveedoresPlazoPagoVenta = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 3 && x.tipo === '2')[0].valor;
      this.formasPago.porcentajeOtraFormaPagoVenta = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 4 && x.tipo === '2')[0].valor;
      this.formasPago.otraFormaPagoPlazoVenta = this.formulario.mercadoComercializacion.formasPago.filter((x) => x.id === 5 && x.tipo === '2')[0].valor;
    }
  }

  public actualizarDatos() {
    this.comando.itemsCheckeados = [];
    this.comando.itemsCheckeados = this.armarComandoItemsSeleccionados();
    this.armarComandoFormasPago();
    this.armarComandoEstimaClientes();
    this.comando.idFormulario = this.formulario.id;
    if (this.esValido()) {
      this.emprendimientoService.guardarMercadoComercializacion(this.comando)
        .subscribe();
    }
  }

  private armarComandoItemsSeleccionados(): ItemsSeleccionados[] {
    let solicitudesCurso = [];
    let categorias = this.formItems.get('categorias').value;
    categorias.forEach((categoria) => {
      let seleccionItems = new ItemsSeleccionados();
      let itemsSeleccionados = categoria.items.filter((item) => item.seleccionado).map((item) => new ItemMercadoComercializacion(item.id, item.nombre));
      if (categoria.otros) {
        let otros = categoria.otros;
        if (otros.descripcion) {
          itemsSeleccionados.push(new ItemMercadoComercializacion(otros.id, 'OTROS'));
          seleccionItems.descripcion = otros.descripcion;
        }
      }
      seleccionItems.items = itemsSeleccionados;

      if (seleccionItems.items.length) {
        solicitudesCurso.push(seleccionItems);
      }
    });
    this.formulario.mercadoComercializacion.itemsPorCategoria = solicitudesCurso;
    return solicitudesCurso;
  }

  private armarComandoFormasPago() {
    this.comando.formasPago = [];
    this.comando.formasPago.push(new FormaPagoItem(1, this.formFormaPagos.get('porcentajeContadoEfectivoCompra').value, 'c'));
    this.comando.formasPago.push(new FormaPagoItem(2, this.formFormaPagos.get('porcentajeCreditoProveedoresCompra').value, 'c'));
    this.comando.formasPago.push(new FormaPagoItem(3, this.formFormaPagos.get('creditoProveedoresPlazoPagoCompra').value, 'c'));
    this.comando.formasPago.push(new FormaPagoItem(4, this.formFormaPagos.get('porcentajeOtraFormaPagoCompra').value, 'c'));
    this.comando.formasPago.push(new FormaPagoItem(5, this.formFormaPagos.get('otraFormaPagoPlazoCompra').value, 'c'));
    this.comando.formasPago.push(new FormaPagoItem(1, this.formFormaPagos.get('porcentajeContadoEfectivoVenta').value, 'v'));
    this.comando.formasPago.push(new FormaPagoItem(2, this.formFormaPagos.get('porcentajeCreditoProveedoresVenta').value, 'v'));
    this.comando.formasPago.push(new FormaPagoItem(3, this.formFormaPagos.get('creditoProveedoresPlazoPagoVenta').value, 'v'));
    this.comando.formasPago.push(new FormaPagoItem(4, this.formFormaPagos.get('porcentajeOtraFormaPagoVenta').value, 'v'));
    this.comando.formasPago.push(new FormaPagoItem(5, this.formFormaPagos.get('otraFormaPagoPlazoVenta').value, 'v'));
    this.formulario.mercadoComercializacion.formasPago = this.comando.formasPago;
  }

  private armarComandoEstimaClientes() {
    this.comando.estimaClientes.cantidad = this.formEstimaClientes.get('cantidad').value;
    this.formulario.mercadoComercializacion.estimaClientes = new EstimaCantClientes(
      this.formEstimaClientes.get('estimaClientes').value,
      this.formEstimaClientes.get('cantidad').value
    );
    this.formulario.mercadoComercializacion.estimaClientes = this.comando.estimaClientes;
  }

  public clickRadioEstima(value: boolean) {
    this.comando.estimaClientes.estima = value;
  }
}
