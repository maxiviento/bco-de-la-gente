import { Routes, RouterModule } from '@angular/router';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { NgModule } from '@angular/core';
import { ConfiguracionChecklistComponent } from './configuracion-checklist.component';

const ROUTES: Routes = [
  {
    path: 'configuracion-checklist',
    component: ConfiguracionChecklistComponent,
    canActivate: [CanActivateAuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})

export class ConfiguracionChecklistRoutingModule {

}
