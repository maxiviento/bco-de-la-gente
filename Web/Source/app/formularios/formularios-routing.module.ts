import { BandejaFormularioComponent } from './bandeja-formulario/bandeja-formulario.component';
import { CanActivateAuthGuard } from '../core/auth/can-activate-auth-guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FormularioComponent } from './formularios.component';
import { SeleccionLineaComponent } from './seleccion-linea/seleccion-linea.component';
import { ReporteFormularioComponent } from './reporte/reporte-formulario.component';
import { ReporteFormularioLineaComponent } from './reporte/reporte-formulario-linea.component';
import { DatosEmprendimientoComponent } from './cuadrantes/datos-emprendimiento/datos-emprendimiento.component';
import { PatrimonioSolicitanteComponent } from './cuadrantes/patrimonio-solicitante/patrimonio-solicitante.component';
import { PrecioVentaComponent } from './cuadrantes/precio-venta/precio-venta.component';
import { ActualizarDatosComponent } from './actualizar-datos/actualizar-datos.component';
import { LineasOngComponent } from '../lineas/shared/linea-ong/lineas-ong.component';

const ROUTES: Routes = [
  {path: 'formularios', component: BandejaFormularioComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'seleccion-linea', component: SeleccionLineaComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'formularios/nuevo', component: FormularioComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'formularios/:id', component: FormularioComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'formularios/edicion/:id', component: FormularioComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'formularios/revision/iniciar/:id', component: FormularioComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'formularios/revision/rechazar/:id', component: FormularioComponent, canActivate: [CanActivateAuthGuard]},
  {path: 'reporte-formulario/:id', component: ReporteFormularioComponent},
  {path: 'reporte-formulario-linea', component: ReporteFormularioLineaComponent},
  {path: 'patrimonio-solicitante', component: PatrimonioSolicitanteComponent},
  {path: 'cuadrante', component: DatosEmprendimientoComponent},
  // {path: 'resultado-estimado-mensual', component: ResultadoEstimadoMensualComponent},
  {path: 'precioventa', component: PrecioVentaComponent},
  {path: 'actualizar-datos/:id', component: ActualizarDatosComponent, canActivate: [CanActivateAuthGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(ROUTES)],
  exports: [RouterModule],
})
export class FormularioRoutingModule {

}
