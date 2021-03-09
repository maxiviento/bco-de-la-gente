import { PerfilesRoutingModule } from './perfiles-routing.module';
import { PerfilesService } from './shared-perfiles/perfiles.service';
import { PerfilesComponent } from './perfiles.component';
import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';
import { EliminacionPerfilComponent } from './eliminacion-perfil/eliminacion-perfil.component';
import { VisualizacionPerfilComponent } from './visualizacion-perfil/visualizacion-perfil.component';
import { NuevoPerfilComponent } from './nuevo-perfil/nuevo-perfil.component';
import { EdicionPerfilComponent } from './edicion-perfil/edicion-perfil.component';

@NgModule({
  imports: [
    SharedModule,
    PerfilesRoutingModule
  ],
  declarations: [
    PerfilesComponent,
    EdicionPerfilComponent,
    EliminacionPerfilComponent,
    VisualizacionPerfilComponent,
    NuevoPerfilComponent
  ],
  providers: [PerfilesService],
  exports: [PerfilesRoutingModule],
})

export class PerfilesModule {
}
