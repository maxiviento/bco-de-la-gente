import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BandejaPagosComponent } from './bandeja-pagos/bandeja-pagos.component';
import { BandejaLotesComponent } from './bandeja-lotes/bandeja-lotes.component';
import { DetalleLoteVerComponent } from './detalle-lote-ver/detalle-lote-ver.component';
import { DetalleLoteDesagruparComponent } from './detalle-lote-desagrupar/detalle-lote-desagrupar.component';
import { DetalleLoteLiberarComponent } from './detalle-lote-liberar/detalle-lote-liberar.component';
import { ArmarLoteSuafComponent } from './suaf/armar-lote-suaf/armar-lote-suaf.component';
import { PlanPagosComponent } from './plan-pagos/plan-pagos.component';
import { BandejaSuafComponent } from './suaf/bandeja-suaf/bandeja-suaf.component';
import { CargaManualDevengadoComponent } from './suaf/carga-manual-devengado/carga-manual-devengado.component';
import { BandejaRecuperoComponent } from '../recupero/bandeja-recupero/bandeja-recupero.component';
import { VerLoteSuafComponent } from './suaf/ver-lote-suaf/ver-lote-suaf.component';
import { BandejaResultadoBancoComponent } from '../recupero/bandeja-resultado-banco/bandeja-resultado-banco.component';
import { VerInconsistenciaRecuperoComponent } from '../recupero/ver-inconsistencia-recupero/ver-inconsistencia-recupero.component';
import { VerInconsistenciaResultadoComponent } from '../recupero/ver-inconsistencia-resultado/ver-inconsistencia-resultado.component';
import { GenerarLotePagoComponent } from './suaf/generar-lote-pago/generar-lote-pago.component';
import { BandejaCambioEstadoComponent } from './bandeja-cambio-estado/bandeja-cambio-estado.component';
import { BandejaChequesComponent } from './bandeja-formularios-cheque/bandeja-cheques.component';
import { DetalleLoteAgregarPrestamoComponent } from './detalle-lote-agrupar/detalle-lote-agregar-prestamo.component';
import { BandejaAgregarPrestamoComponent } from './detalle-lote-agrupar/consulta-prestamos/bandeja-agregar-prestamo.component';
import {BandejaCrearAdendaComponent} from "./bandeja-crear-adenda/bandeja-crear-adenda.component";

const ROUTES: Routes = [
  {
    path: 'pagos',
    component: BandejaPagosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-lotes',
    component: BandejaLotesComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-cheques',
    component: BandejaChequesComponent,
    canActivate: [CanActivateAuthGuard]

  },
  {
    path: 'detalle-lote-ver/:id',
    component: DetalleLoteVerComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'desagrupar-lote/:id',
    component: DetalleLoteDesagruparComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'agregar-lote/:id',
    component: DetalleLoteAgregarPrestamoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-agregar-prestamo/:id',
    component: BandejaAgregarPrestamoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-crear-adenda/:id',
    component: BandejaCrearAdendaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'liberar-lote/:id',
    component: DetalleLoteLiberarComponent,
    canActivate: [CanActivateAuthGuard]
  },  {
    path: 'bandeja-suaf',
    component: BandejaSuafComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'armar-lote-suaf',
    component: ArmarLoteSuafComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'carga-manual-devengado',
    component: CargaManualDevengadoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'ver-lote-suaf/:id',
    component: VerLoteSuafComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'generar-lote-pago/:id',
    component: GenerarLotePagoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'actualizar-plan-pagos',
    component: PlanPagosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'actualizar-plan-pagos/prestamo/:id',
    component: PlanPagosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'actualizar-plan-pagos/lote/:id',
    component: PlanPagosComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-recupero',
    component: BandejaRecuperoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-resultado-banco',
    component: BandejaResultadoBancoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'ver-inconsistencia-recupero/:id',
    component: VerInconsistenciaRecuperoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'ver-inconsistencia-resultado/:id',
    component: VerInconsistenciaResultadoComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'bandeja-cambio-estado',
    component: BandejaCambioEstadoComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule],
})
export class PagosRoutingModule {

}
