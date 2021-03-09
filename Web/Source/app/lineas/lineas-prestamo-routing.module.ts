import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NuevaLineaComponent } from './nueva-linea/nueva-linea.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { LineasPrestamoComponent } from './lineas-prestamo.component';
import { ConsultaLineaComponent } from './consulta-linea/consulta-linea.component';
import { EliminacionLineaComponent } from './eliminacion-linea/eliminacion-linea.component';
import { ConsultaDetalleLineaComponent } from './consulta-linea/consulta-detalle-linea/consulta-detalle-linea.component';
import { EdicionLineaComponent } from './edicion-linea/edicion-linea.component';
import { EliminacionDetalleLineaComponent } from './eliminacion-linea/eliminacion-detalle-linea/eliminacion-detalle-linea.component';
import { EdicionDetalleLineaComponent } from './edicion-linea/edicion-detalle-linea/edicion-detalle-linea.component';

const ROUTES: Routes = [
  {
    path: 'nueva-linea',
    component: NuevaLineaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-lineas',
    component: LineasPrestamoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'eliminacion-linea/:id',
    component: EliminacionLineaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'eliminacion-detalle-linea/:id',
    component: EliminacionDetalleLineaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consulta-detalle-linea/:id',
    component: ConsultaDetalleLineaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'edicion-detalle-linea/:id',
    component: EdicionDetalleLineaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'edicion-linea/:id',
    component: EdicionLineaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consulta-linea/:idLinea',
    component: ConsultaLineaComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class LineaRoutingModule {

}
