import { RouterModule, Routes } from '@angular/router';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { NgModule } from '@angular/core';
import { PerfilesComponent } from './perfiles.component';
import { NuevoPerfilComponent } from './nuevo-perfil/nuevo-perfil.component';
import { EliminacionPerfilComponent } from './eliminacion-perfil/eliminacion-perfil.component';
import { VisualizacionPerfilComponent } from './visualizacion-perfil/visualizacion-perfil.component';
import { EdicionPerfilComponent } from './edicion-perfil/edicion-perfil.component';

const ROUTES: Routes = [
  {
    path: 'perfiles',
    component: PerfilesComponent, canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'perfiles/nuevo',
    component: NuevoPerfilComponent, canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'perfiles/:id/eliminacion',
    component: EliminacionPerfilComponent, canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'perfiles/:id/visualizacion',
    component: VisualizacionPerfilComponent, canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'perfiles/:id/edicion',
    component: EdicionPerfilComponent, canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class PerfilesRoutingModule {

}
