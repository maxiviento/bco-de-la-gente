import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NotificacionService } from './notificacion.service';
import { ContenidoInformativoComponent } from './contenidos/contenido-informativo.component';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ControlMessagesComponent } from './forms/control-messages.component';
import { ControlMessageComponent } from './forms/control-message.component';
import { ErrorFeedbackDirective } from './forms/error-feedback.directive';
import { ContenidoConfirmacionComponent } from './contenidos/contenido-confirmacion.component';
import { PaginacionComponent } from './paginacion/paginacion.component';
import { SafePipe } from './safe.pipe';
import { ColorPickerModule, ColorPickerService } from 'ngx-color-picker';
import { IfPermissionDirective } from './show-elemento/show-elemento.directive';
import { NgSelectModule } from './forms/ng-select/ng-select.module';
import { ValidacionEdadesComponent } from './componentes/validacion-edades/validacion-edades.component';
import { ArchivoService } from './archivo.service';
import { SeleccionArchivoComponent } from './seleccion-archivo/seleccion-archivo.component';
import { ModalArchivoComponent } from './modal-archivo/modal-archivo.component';
import { DocumentoService } from './servicios/documento.service';
import { BusquedaPorPersonaComponent } from './componentes/busqueda-por-persona/busqueda-por-persona.component';
import { BusquedaSucursalBancariaComponent } from './componentes/busqueda-sucursal-bancaria/busqueda-sucursal-bancaria.component';
import { MonedaPipe } from './pipes/moneda.pipe';
import { FiltroDomicilioComponent } from './domicilio/filtro-domicilio.component';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { FiltroDomicilioLineaComponent } from './domicilio-linea/filtro-domicilio-linea.component';
import { FiltroDomicilioBandejaComponent } from './domicilio-bandeja/filtro-domicilio-bandeja.component';
import { MultipleSeleccionComponent } from './multiple-seleccion/multiple-seleccion.component';
import { MonitorProcesoService } from './servicios/monitor-procesos.service';
import { BuscadorDropDownModule } from './buscador-drop-down/buscador-drop-down.module';

/*import { DropMenuPermissionDirective } from './dropbox-menu-permission/drop-menu-permission.directive';*/

@NgModule({
  imports: [CommonModule,
    ReactiveFormsModule,
    ColorPickerModule,
    NgSelectModule,
    MultiselectDropdownModule,
    FormsModule],
  declarations: [
    ContenidoInformativoComponent,
    ContenidoConfirmacionComponent,
    ControlMessagesComponent,
    ControlMessageComponent,
    ErrorFeedbackDirective,
    PaginacionComponent,
    SafePipe,
    IfPermissionDirective,
    ValidacionEdadesComponent,
    SeleccionArchivoComponent,
    ModalArchivoComponent,
    SeleccionArchivoComponent,
    BusquedaPorPersonaComponent,
    BusquedaSucursalBancariaComponent,
    FiltroDomicilioComponent,
    FiltroDomicilioLineaComponent,
    MonedaPipe,
    FiltroDomicilioBandejaComponent,
    MultipleSeleccionComponent
  ],
  providers: [
    NotificacionService,
    ColorPickerService,
    ArchivoService,
    MonitorProcesoService,
    DocumentoService,
    CurrencyPipe],
  exports: [
    CommonModule,
    RouterModule,
    SeleccionArchivoComponent,
    NgbModule,
    ReactiveFormsModule,
    ControlMessagesComponent,
    ControlMessageComponent,
    ErrorFeedbackDirective,
    PaginacionComponent,
    FiltroDomicilioComponent,
    FiltroDomicilioLineaComponent,
    FiltroDomicilioBandejaComponent,
    MultipleSeleccionComponent,
    SafePipe,
    ColorPickerModule,
    IfPermissionDirective,
    NgSelectModule,
    ValidacionEdadesComponent,
    ModalArchivoComponent,
    ValidacionEdadesComponent,
    BusquedaPorPersonaComponent,
    BusquedaSucursalBancariaComponent,
    MonedaPipe,
    BuscadorDropDownModule
  ],
  entryComponents: [
    ContenidoInformativoComponent,
    ContenidoConfirmacionComponent,
    ModalArchivoComponent,
    SeleccionArchivoComponent
  ]
})
export class SharedModule {
}
