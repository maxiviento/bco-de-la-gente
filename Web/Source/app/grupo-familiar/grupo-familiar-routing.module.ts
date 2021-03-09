import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { GrupoFamiliarComponent } from './grupo-familiar.component';

const ROUTES: Routes = [
  {
    path: 'grupo-familiar',
    component: GrupoFamiliarComponent, canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class GrupoFamiliarRoutingModule {

}
