import { Component, Input } from '@angular/core';
import { DetalleLineaPrestamo } from '../modelo/detalle-linea-prestamo.model';
import { IntegranteSocio } from '../modelo/integrante-socio.model';
import { TipoFinanciamiento } from '../modelo/tipo-financiamiento.model';
import { TipoInteres } from '../modelo/tipo-interes.model';
import { TipoGarantia } from '../modelo/tipo-garantia.model';
import { Convenio } from '../../../shared/modelo/convenio-model';

@Component({
  selector: 'bg-linea-datos-seleccionados',
  templateUrl: './linea-datos-seleccionados.component.html',
  styleUrls: ['./linea-datos-seleccionados.component.scss']
})

export class LineaDatosSeleccionadosComponent {
  @Input('detalles') public detalles: DetalleLineaPrestamo[] = [];
  @Input() public integrantes: IntegranteSocio[] = [];
  @Input() public financiamientos: TipoFinanciamiento[] = [];
  @Input() public intereses: TipoInteres[] = [];
  @Input() public garantias: TipoGarantia[] = [];
  @Input() public conveniosPago: Convenio[] = [];
  @Input() public conveniosRecupero: Convenio[] = [];

  public eliminarDetalle(indice: number) {
    this.detalles.splice(indice, 1);
  }

  public obtenerDescripcion(id: number, lista: any): string {
    return lista.find((value) => value.id === id).descripcion;
  }
  public obtenerNombre(id: number, lista: any): string {
    return lista.find((value) => value.id === id).nombre;
  }
}
