import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { MotivosRechazoComponent } from './motivos-rechazo.component';
import { NuevoMotivoRechazoComponent } from './nuevo-motivo-rechazo/nuevo-motivo-rechazo.component';
import { ConsultaMotivoRechazoComponent } from './consulta-motivo-rechazo/consulta-motivo-rechazo.component';
import { EdicionMotivoRechazoComponent } from './edicion-motivo-rechazo/edicion-motivo-rechazo.component';
import { EliminacionMotivoRechazoComponent } from './eliminacion-motivo-rechazo/eliminacion-motivo-rechazo.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';

const ROUTES: Routes = [
  {
    path: 'motivos-rechazo',
    component: MotivosRechazoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'nuevo-motivo-rechazo',
    component: NuevoMotivoRechazoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consulta-motivo-rechazo/:idMotivo/:idAmbito',
    component: ConsultaMotivoRechazoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'eliminacion-motivo-rechazo/:idMotivo/:idAmbito',
    component: EliminacionMotivoRechazoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'edicion-motivo-rechazo/:idMotivo/:idAmbito',
    component: EdicionMotivoRechazoComponent,
    canActivate: [CanActivateAuthGuard]
  }

];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class MotivosRechazoRouting {

}
