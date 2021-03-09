import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { SeleccionFormulariosComponent } from './seleccion-formularios.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { ConsultaSituacionPersonasComponent } from './consulta-situacion-personas/consulta-situacion-personas.component';
import { ActualizarFechaPagoComponent } from './shared/componentes/actualizar-fecha-pago/actualizar-fecha-pago.component';

const ROUTES: Routes = [
  {
    path: 'actualizar-sucursal',
    component: SeleccionFormulariosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'actualizar-sucursal/prestamo/:id',
    component: SeleccionFormulariosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'actualizar-sucursal/lote/:id',
    component: SeleccionFormulariosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'imprimir-documentacion',
    component: SeleccionFormulariosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'imprimir-documentacion/prestamo/:id',
    component: SeleccionFormulariosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'imprimir-documentacion/lote/:id',
    component: SeleccionFormulariosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consultar-situacion-bge',
    component: ConsultaSituacionPersonasComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'actualizar-fecha-pago/:id',
    component: ActualizarFechaPagoComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule],
})
export class SeleccionFormulariosRoutingModule {

}
