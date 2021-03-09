import { Component, OnInit } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { PrecioVenta } from '../../shared/modelo/precio-venta.model';
import { isEmpty } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bge-resultado-estimado-mensual',
  templateUrl: 'resultado-estimado-mensual.component.html',
  styleUrls: ['resultado-estimado-mensual.component.scss']
})

export class ResultadoEstimadoMensualComponent extends CuadranteFormulario implements OnInit {
  public totalGastos: number = 0;
  public totalVentas: number = 0;
  public precioUnitarioProducto: number;
  public precioVenta: PrecioVenta = new PrecioVenta();

  constructor() {
    super();
  }

  public ngOnInit(): void {
    if (this.formulario.precioVenta && this.formulario.precioVenta.costos) {
      this.obtenerDatosCuadrante();
    }
  }

  public obtenerDatosCuadrante(): void {
    this.precioVenta = this.formulario.precioVenta;
    let sumatoriaCostos: number = 0;

    if (!(isEmpty(this.precioVenta.unidadesEstimadas) ||
      this.precioVenta.unidadesEstimadas === 0)) {
      this.precioVenta.costos.forEach((costo) => {
        sumatoriaCostos += Number(costo.valorMensual);
      });
      this.totalVentas = (sumatoriaCostos) + (this.precioVenta.gananciaEstimada * this.precioVenta.unidadesEstimadas);
      this.totalGastos = sumatoriaCostos;
      this.precioUnitarioProducto = (sumatoriaCostos / this.precioVenta.unidadesEstimadas) + Number(this.precioVenta.gananciaEstimada);
    }
  }

  public mostrarCuadrante(): boolean {
    return this.totalGastos === 0 || this.totalVentas === 0;
  }

  private sumarTotales(): number {
    return this.totalVentas - this.totalGastos;
  }

  public actualizarDatos() {
  }

  public inicializarDeNuevo(): boolean {
    return true;
  }

  public esValido(): boolean {
    return true;
  }
}
