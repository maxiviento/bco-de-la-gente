import { Routes, RouterModule } from '@angular/router';
import { EtapasComponent } from './etapas.component';
import { NuevaEtapaComponent } from './nueva-etapa/nueva-etapa.component';
import { NgModule } from '@angular/core';
import { ConsultaEtapaComponent } from './consulta-etapa/consulta-etapa.component';
import { EliminacionEtapaComponent } from './eliminacion-etapa/eliminacion-etapa.component';
import { EdicionEtapaComponent } from './edicion-etapa/edicion-etapa.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';

const ROUTES: Routes = [
  {path: 'etapas', component: EtapasComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'nueva-etapa', component: NuevaEtapaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'etapas/:id', component: ConsultaEtapaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'eliminacion-etapa/:id', component: EliminacionEtapaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'edicion-etapa/:id', component: EdicionEtapaComponent, canActivate: [CanActivateAuthGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})

export class EtapasRoutingModule {

}
