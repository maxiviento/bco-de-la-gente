import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { UsuariosComponent } from './usuarios.component';
import { VisualizacionUsuarioComponent } from './visualizacion-usuario/visualizacion-usuario.component';
import { EliminacionUsuarioComponent } from './eliminacion-usuario/eliminacion-usuario.component';
import { NuevoUsuarioComponent } from './nuevo-usuario/nuevo-usuario.component';
import { EdicionUsuarioComponent } from './edicion-usuario/edicion-usuario.component';

const ROUTES: Routes = [
  {
    path: 'usuarios',
    component: UsuariosComponent, canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'usuarios/:id/visualizacion',
    component: VisualizacionUsuarioComponent,
    canActivate: [CanActivateAuthGuard]
  }
  ,
  {
    path: 'usuarios/nuevo',
    component: NuevoUsuarioComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'usuarios/:id/edicion',
    component: EdicionUsuarioComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'usuarios/:id/eliminacion',
    component: EliminacionUsuarioComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})

export class UsuariosRoutingModule {
}
