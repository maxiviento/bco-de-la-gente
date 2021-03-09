import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NuevaAreaComponent } from './nueva-area/nueva-area.component';
import { AreaComponent } from './areas.component';
import { ConsultaAreaComponent } from './consulta-area/consulta-area.component';
import { EdicionAreaComponent } from './edicion-area/edicion-area.component';
import { EliminacionAreaComponent } from './eliminacion-area/eliminacion-area.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';

const ROUTES: Routes = [
  {path: 'areas', component: AreaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'nueva-area', component: NuevaAreaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'areas/:id', component: ConsultaAreaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'edicion-area/:id', component: EdicionAreaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'eliminacion-area/:id', component: EliminacionAreaComponent, canActivate: [CanActivateAuthGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class AreaRoutingModule {

}
