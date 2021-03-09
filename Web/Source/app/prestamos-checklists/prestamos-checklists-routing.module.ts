import { PrestamosChecklistsComponent } from './prestamos-checklists.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConformarPrestamosComponent } from './conformar-prestamos/conformar-prestamos.component';
import { VerChecklistComponent } from './checklist-prestamo/ver-checklist-prestamo/ver-checklist-prestamo.component';
import { ActualizarChecklistComponent } from './checklist-prestamo/actualizar-checlist-prestamo/actualizar-checklist-prestamo.component';
import { CargaNumeroControlInternoComponent } from './carga-numero-control-interno/carga-numero-control-interno.component';
import { ReporteComponent } from './checklist-prestamo/reportes/reporte.component';
import { GestionArchivosPrestamoComponent } from './gestion-archivos/gestion-archivos-prestamo.component';
import { FormularioComponent } from '../formularios/formularios.component';
import { ReactivacionComponent } from './reactivacion/reactivacion.component';
import { SituacionDomicilioComponent } from './checklist-prestamo/situacion-domicilio/situacion-domicilio.component';

const ROUTES: Routes = [
  {
    path: 'bandeja-prestamos',
    component: PrestamosChecklistsComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'conformar-prestamos',
    component: ConformarPrestamosComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'ver-checklist/:id',
    component: VerChecklistComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'actualizar-checklist/:id',
    component: ActualizarChecklistComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'cargar-numero-control-interno/:id',
    component: CargaNumeroControlInternoComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'condicion-economica/:id',
    component: ReporteComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'situacion-domicilio/:id',
    component: SituacionDomicilioComponent,
  }, {
    path: 'control-sintys/:id',
    component: ReporteComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'reporte-providencia/:id',
    component: ReporteComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'gestion-archivos/:id',
    component: GestionArchivosPrestamoComponent//TODO Manolo agregar CanActivate
  }, {
    path: 'prestamo/formulario/ver/:id',
    component: FormularioComponent,
    canActivate: [CanActivateAuthGuard]
  }, {
    path: 'reactivar-prestamo/:id',
    component: ReactivacionComponent,
    canActivate: [CanActivateAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule],
})
export class PrestamosChecklistsRoutingModule {

}
