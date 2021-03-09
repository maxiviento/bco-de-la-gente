import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { NuevoMotivoDestinoComponent } from './nuevo-motivo-destino/nuevo-motivo-destino.component';
import { MotivosDestinoComponent } from './motivos-destino.component';
import { EliminacionMotivoDestinoComponent } from './eliminacion-motivo-destino/eliminacion-motivo-destino.component';
import { ConsultaMotivoDestinoComponent } from './consulta-motivo-destino/consulta-motivo-destino.component';
import { EdicionMotivoDestinoComponent } from './edicion-motivo-destino/edicion-motivo-destino.component';

const ROUTES: Routes = [
  {
    path: 'nuevo-motivo-destino',
    component: NuevoMotivoDestinoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'edicion-motivo-destino/:id',
    component: EdicionMotivoDestinoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consulta-motivo-destino/:id',
    component: ConsultaMotivoDestinoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'eliminacion-motivo-destino/:id',
    component: EliminacionMotivoDestinoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'motivos-destino',
    component: MotivosDestinoComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class MotivoDestinoRouting {

}
