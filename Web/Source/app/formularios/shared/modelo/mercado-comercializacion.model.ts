import { EstimaCantClientes } from './estima-cant-clientes.model';
import { FormaPagoItem } from './forma-pago-item.model';
import { ItemMercadoComerSeleccionado } from './ItemMercadoComerSeleccionado.model';

export class MercadoComercializacion {
  public estimaClientes: EstimaCantClientes;
  public itemsPorCategoria: ItemMercadoComerSeleccionado[];
  public formasPago: FormaPagoItem[];

  constructor(estimaClientes?: EstimaCantClientes,
              itemsPorCategoria?: ItemMercadoComerSeleccionado[],
              formasPago?: FormaPagoItem[]) {
    this.estimaClientes = estimaClientes;
    this.itemsPorCategoria = itemsPorCategoria;
    this.formasPago = formasPago;
  }
}
