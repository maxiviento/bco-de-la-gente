import { BrowserModule } from '@angular/platform-browser';
import { Http, HttpModule, RequestOptions, XHRBackend } from '@angular/http';
import { ApplicationRef, LOCALE_ID, NgModule } from '@angular/core';
import { createInputTransfer, createNewHosts, removeNgStyles } from '@angularclass/hmr';
/*
 * Platform and Environment providers/directives/pipes
 */
import { ENV_PROVIDERS } from './environment';
// App is our top level component
import { AppComponent } from './app.component';
import { APP_RESOLVER_PROVIDERS } from './app.resolver';
import { AppState, InternalStateType } from './app.service';
import { PageNotFoundComponent } from './not-found/not-found.component';
import { EtapasModule } from './etapas/etapas.module';
import { AreaModule } from './areas/areas.module';
import { ItemsModule } from './items/items.module';
// Modules
import { CoreModule } from './core/core.module';
import { AppRoutingModule } from './app-routing.module';
import { GrupoUnicoModule } from './grupo-unico/grupo-unico.module';
// Bootstrap
import { NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { HttpFactory } from './core/http/http-factory';
import { NgbDateShortParserFormatter } from './shared/ngb/ngb-date-formatter.service';

import '../styles/styles.scss';
import '../styles/headings.css';
import { SpinnerService } from './core/spinner/spinner.service';
import { SingleSpinnerService } from './core/single-spinner/single-spinner.service';
import { FormularioModule } from './formularios/formularios.module';
import { CanActivateAuthGuard } from './core/auth/can-activate-auth-guard';
import { LineaModule } from './lineas/lineas-prestamo.module';
import { PerfilesModule } from './perfiles/perfiles.module';
import { UsuariosModule } from './usuarios/usuarios.module';
import { MonitorProcesoModule } from './monitor-procesos/monitor-procesos.module';
import { SoporteModule } from './soporte/soporte.module';
import { PrestamosChecklistsModule } from './prestamos-checklists/prestamos-checklists.module';
import { MotivoDestinoModule } from './motivo-destino/motivo-destino.module';
import { ConfiguracionChecklistModule } from './configuracion-checklist/configuracion-checklist.module';
import { MotivosRechazoModule } from './motivos-rechazo/motivos-rechazo.module';
import { MontoDisponibleModule } from './pagos/monto-disponible/monto-disponible.module';
import { PagosModule } from './pagos/pagos.module';
import { SeleccionFormulariosModule } from './seleccion-formularios/seleccion-formularios.module';
import { ToastNoAnimation, ToastNoAnimationModule, ToastrModule } from 'ngx-toastr';
import { GrupoFamiliarModule } from './grupo-familiar/grupo-familiar.module';

// Application wide providers
const APP_PROVIDERS = [
  ...APP_RESOLVER_PROVIDERS,
  AppState,
  {
    provide: Http,
    useFactory: HttpFactory,
    deps: [XHRBackend, RequestOptions, SpinnerService, SingleSpinnerService]
  },
  {provide: NgbDateParserFormatter, useClass: NgbDateShortParserFormatter},
  CanActivateAuthGuard
];

type StoreType = {
  state: InternalStateType,
  restoreInputValues: () => void,
  disposeOldHosts: () => void
};

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    PageNotFoundComponent
  ],
  imports: [
    SeleccionFormulariosModule,
    MontoDisponibleModule,
    MotivoDestinoModule,
    MonitorProcesoModule,
    MotivosRechazoModule,
    PrestamosChecklistsModule,
    PerfilesModule,
    UsuariosModule,
    BrowserModule,
    HttpModule,
    CoreModule,
    ItemsModule,
    AreaModule,
    EtapasModule,
    FormularioModule,
    PagosModule,
    GrupoUnicoModule,
    LineaModule,
    GrupoFamiliarModule,
    SoporteModule,
    ConfiguracionChecklistModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    //BrowserAnimationsModule, // comentado porque duplica el app en el html cuando rebuildea
    /*ToastrModule.forRoot({ // se usa el toastr sin animación para que no duplique el app
      maxOpened: 1,
      autoDismiss: true,
      preventDuplicates: true
    })*/
    ToastNoAnimationModule,
    ToastrModule.forRoot({
      toastComponent: ToastNoAnimation,
      maxOpened: 1,
      autoDismiss: true,
      preventDuplicates: true
    }),
  ],
  providers: [
    {provide: LOCALE_ID, useValue: 'es-AR'},
    ENV_PROVIDERS,
    APP_PROVIDERS
  ]
})
export class AppModule {

  constructor(public appRef: ApplicationRef,
              public appState: AppState) {
  }

  public hmrOnInit(store: StoreType) {
    if (!store || !store.state) {
      return;
    }
    console.log('HMR store', JSON.stringify(store, null, 2));
    // set state
    this.appState._state = store.state;
    // set input values
    if ('restoreInputValues' in store) {
      let restoreInputValues = store.restoreInputValues;
      setTimeout(restoreInputValues);
    }

    this.appRef.tick();
    delete store.state;
    delete store.restoreInputValues;
  }

  public hmrOnDestroy(store: StoreType) {
    const cmpLocation = this.appRef.components.map((cmp) => cmp.location.nativeElement);
    // save state
    const state = this.appState._state;
    store.state = state;
    // recreate root elements
    store.disposeOldHosts = createNewHosts(cmpLocation);
    // save input values
    store.restoreInputValues = createInputTransfer();
    // remove styles
    removeNgStyles();
  }

  public hmrAfterDestroy(store: StoreType) {
    // display new elements
    store.disposeOldHosts();
    delete store.disposeOldHosts;
  }

}
