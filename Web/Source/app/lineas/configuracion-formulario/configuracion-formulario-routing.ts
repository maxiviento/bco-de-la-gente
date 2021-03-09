import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CanActivateAuthGuard } from '../../core/auth/can-activate-auth-guard';
import { ConfiguracionFormularioComponent } from './configuracion-formulario.component';

const ROUTES: Routes = [
  {
    path: 'configuracion-formulario',
    component: ConfiguracionFormularioComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})

export class ConfiguracionFormularioRouting {

}
