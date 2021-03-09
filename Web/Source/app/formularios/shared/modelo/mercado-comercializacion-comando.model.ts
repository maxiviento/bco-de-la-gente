import { EstimaCantClientes } from './estima-cant-clientes.model';
import { FormaPagoItem } from './forma-pago-item.model';

export class MercadoComercializacionComando {
  public idFormulario: number;
  public estimaClientes: EstimaCantClientes;
  public formasPago: FormaPagoItem[];
  public itemsCheckeados = [];

  constructor(idFormulario?: number,
              estimaClientes?: EstimaCantClientes,
              formasPago?: FormaPagoItem[],
              itemsCheckeados?: number[]) {
    this.idFormulario = idFormulario;
    this.estimaClientes = estimaClientes;
    this.formasPago = formasPago;
    this.itemsCheckeados = itemsCheckeados;
  }
}
