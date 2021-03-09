import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { MonitorProcesosComponent } from './monitor-procesos.component';

const ROUTES: Routes = [
  {
    path: 'monitor-procesos',
    component: MonitorProcesosComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class MonitorProcesoRouting {

}
