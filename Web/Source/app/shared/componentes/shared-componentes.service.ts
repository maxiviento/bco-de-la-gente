import { NgModule } from '@angular/core';
import { ActualizarSucursalBancariaComponent } from './actualizar-sucursal-bancaria/actualizar-sucursal-bancaria.component';
import { SharedModule } from '../shared.module';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
    ActualizarSucursalBancariaComponent
  ],
  exports: [
    ActualizarSucursalBancariaComponent
  ]
})
export class SharedComponentesModule {
}
