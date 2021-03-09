import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PrestamosChecklistsRoutingModule } from './prestamos-checklists-routing.module';
import { ConformarPrestamosComponent } from './conformar-prestamos/conformar-prestamos.component';
import { PrestamosChecklistsComponent } from './prestamos-checklists.component';
import { PrestamoService } from '../shared/servicios/prestamo.service';
import { ModalRechazoFormularioComponent } from './checklist-prestamo/modal-rechazo-formulario/modal-rechazo-formulario.component';
import { VerChecklistComponent } from './checklist-prestamo/ver-checklist-prestamo/ver-checklist-prestamo.component';
import { ActualizarChecklistComponent } from './checklist-prestamo/actualizar-checlist-prestamo/actualizar-checklist-prestamo.component';
import { CargaNumeroControlInternoComponent } from './carga-numero-control-interno/carga-numero-control-interno.component';
import { ReporteComponent } from './checklist-prestamo/reportes/reporte.component';
import { SharedComponentesModule } from '../shared/componentes/shared-componentes.service';
import { GestionArchivosPrestamoComponent } from './gestion-archivos/gestion-archivos-prestamo.component';
import { EtapasService } from '../etapas/shared/etapas.service';
import { CabeceraChecklistComponent } from './checklist-prestamo/estructura-checklist/cabecera-checklist/cabecera-checklist.component';
import { EstructuraChecklistComponent } from './checklist-prestamo/estructura-checklist/estructura-checklist.component';
import { WizardChecklistComponent } from './checklist-prestamo/estructura-checklist/checklist/wizard-checklist.component';
import { DataSharedChecklistService } from './checklist-prestamo/estructura-checklist/data-shared-checklist.service';
import { FormulariosService } from '../formularios/shared/formularios.service';
import { ReactivacionComponent } from './reactivacion/reactivacion.component';
import { SituacionDomicilioComponent } from './checklist-prestamo/situacion-domicilio/situacion-domicilio.component';
import { SingleSpinnerModule } from '../core/single-spinner/single-spinner.module';

@NgModule({
  imports: [
    SharedModule,
    PrestamosChecklistsRoutingModule,
    SharedComponentesModule,
    SingleSpinnerModule
  ],
  providers: [
    PrestamoService,
    EtapasService,
    DataSharedChecklistService,
    FormulariosService
  ],
  declarations: [
    ConformarPrestamosComponent,
    PrestamosChecklistsComponent,
    CabeceraChecklistComponent,
    WizardChecklistComponent,
    EstructuraChecklistComponent,
    ModalRechazoFormularioComponent,
    VerChecklistComponent,
    ActualizarChecklistComponent,
    CargaNumeroControlInternoComponent,
    ReporteComponent,
    GestionArchivosPrestamoComponent,
    ReactivacionComponent,
    SituacionDomicilioComponent,

  ],
  exports: [PrestamosChecklistsRoutingModule],
  entryComponents: [
    ModalRechazoFormularioComponent
  ]

})
export class PrestamosChecklistsModule {
}
