import { Routes, RouterModule } from '@angular/router';
import { ItemsComponent } from './items.component';
import { NuevoItemComponent } from './nuevo-item/nuevo-item.component';
import { NgModule } from '@angular/core';
import { ConsultaItemComponent } from './consulta-item/consulta-item.component';
import { EdicionItemComponent } from './edicion-item/edicion-item.component';
import { EliminacionItemComponent } from './eliminacion-item/eliminacion-item.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';

const ROUTES: Routes = [
  {path: 'items', component: ItemsComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'nuevo-item', component: NuevoItemComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'items/:id', component: ConsultaItemComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'edicion-item/:id', component: EdicionItemComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'eliminacion-item/:id', component: EliminacionItemComponent, canActivate: [CanActivateAuthGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})

export class ItemsRoutingModule {

}
