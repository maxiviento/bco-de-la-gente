import { CanActivateAuthGuard } from '../../core/auth/can-activate-auth-guard';
import { MontoDisponibleComponent } from './monto-disponible.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { NuevoMontoDisponibleComponent } from './nuevo-monto-disponible/nuevo-monto-disponible.component';
import { BajaMontoDisponibleComponent } from './baja-monto-disponible/baja-monto-disponible.component';
import { EdicionMontoDisponibleComponent } from './edicion-monto-disponible/edicion-monto-disponible.component';
import { ConsultaMontoDisponibleComponent } from './consulta-monto-disponible/consulta-monto-disponible.component';

const ROUTES: Routes = [
  {
    path: 'monto-disponible',
    component: MontoDisponibleComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'nuevo-monto-disponible',
    component: NuevoMontoDisponibleComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'baja-monto-disponible/:id',
    component: BajaMontoDisponibleComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'edicion-monto-disponible/:id',
    component: EdicionMontoDisponibleComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consulta-monto-disponible/:id',
    component: ConsultaMontoDisponibleComponent,
    canActivate: [CanActivateAuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})

export class MontoDisponibleRoutingModule {

}
