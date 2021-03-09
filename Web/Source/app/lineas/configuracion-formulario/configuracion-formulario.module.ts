import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { ConfiguracionFormularioComponent } from './configuracion-formulario.component';
import { ConfiguracionFormularioRouting } from './configuracion-formulario-routing';

@NgModule({
  imports: [
    SharedModule,
    ConfiguracionFormularioRouting
  ],
  declarations: [ConfiguracionFormularioComponent],
  providers: [LineaService],
  exports: [ConfiguracionFormularioRouting]
})

export class ConfiguracionFormularioModule {

}
