import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { MonitorProcesoRouting } from './monitor-procesos.routing';
import { MonitorProcesosComponent } from './monitor-procesos.component';
import { SingleSpinnerModule } from '../core/single-spinner/single-spinner.module';

@NgModule({
  imports: [
    SharedModule,
    MonitorProcesoRouting,
    SingleSpinnerModule
  ],
  declarations: [
    MonitorProcesosComponent
  ]
})

export class MonitorProcesoModule {
}
