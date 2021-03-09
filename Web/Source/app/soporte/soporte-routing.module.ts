import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { AdministrarParametrosComponent } from './administrar-parametros/administrar-parametros.component';
import { CondicionEconomicaComponent } from './condicion-economica/condicion-economica.component';
import { ReporteRentasIndividualComponent } from './condicion-economica/reporte-rentas.component';
import { DeudaGrupoConvivienteComponent } from '../seleccion-formularios/shared/componentes/deuda-grupo-conviviente/deuda-grupo-conviviente.component';
import { ManualesComponent } from './manuales/manuales.component';
import { ConsultarTablasDefinidasComponent } from './tablas-satelite/consulta/consulta-tablas-definidas.component';
import { EditarTablaDefinidasComponent } from './tablas-satelite/edicion/editar-tabla-definidas.component';
import { NuevaParametroTablaDefinidaComponent } from './tablas-satelite/nuevo-parametro/nuevo-parametro-tabla-definida.component';
import { ConsultarParametroTablaDefinidaComponent } from './tablas-satelite/consulta-parametro/consultar-parametro-tabla-definida.component';
import { GenerarInformesBancoComponent } from './informes/generar-informes-banco.component';
import { MonitorProcesosComponent } from '../monitor-procesos/monitor-procesos.component';

const ROUTES: Routes = [
  {
    path: 'parametros',
    component: AdministrarParametrosComponent,
    canActivate: [CanActivateAuthGuard],
  },
  {
    path: 'condicion-economica',
    component: CondicionEconomicaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'condicion-economica-individual',
    component: ReporteRentasIndividualComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'control-sintys',
    component: CondicionEconomicaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'control-sintys-individual',
    component: ReporteRentasIndividualComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'deuda-grupo-conviviente',
    component: DeudaGrupoConvivienteComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'deuda-grupo-conviviente/prestamo/:id',
    component: DeudaGrupoConvivienteComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'manuales',
    component: ManualesComponent,
    canActivate: [CanActivateAuthGuard]
  },
/*  {
    path: 'consultar-tablas-definidas',
    component: ConsultarTablasDefinidasComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'editar-tabla-definida/:id',
    component: EditarTablaDefinidasComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'nuevo-parametro-tabla-definida/:id',
    component: NuevaParametroTablaDefinidaComponent,
    canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'consultar-parametro-tabla-definida/:id',
    component: ConsultarParametroTablaDefinidaComponent,
    canActivate: [CanActivateAuthGuard]
  },*/
  {
    path: 'generar-informes-banco',
    component: GenerarInformesBancoComponent,
    // canActivate: [CanActivateAuthGuard]
  },
  {
    path: 'monitor-procesos',
    component: MonitorProcesosComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule]
})
export class SoporteRoutingModule {
}
