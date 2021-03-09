import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';
import { UsuariosRoutingModule } from './usuarios-routing.module';
import { UsuariosComponent } from './usuarios.component';
import { UsuariosService } from './shared-usuarios/usuarios.service';
import { EdicionUsuarioComponent } from './edicion-usuario/edicion-usuario.component';
import { EliminacionUsuarioComponent } from './eliminacion-usuario/eliminacion-usuario.component';
import { NuevoUsuarioComponent } from './nuevo-usuario/nuevo-usuario.component';
import { VisualizacionUsuarioComponent } from './visualizacion-usuario/visualizacion-usuario.component';

@NgModule({
  imports: [
    SharedModule,
    UsuariosRoutingModule,
  ],
  declarations: [
    UsuariosComponent,
    EdicionUsuarioComponent,
    EliminacionUsuarioComponent,
    NuevoUsuarioComponent,
    VisualizacionUsuarioComponent,
  ],
  providers: [UsuariosService],
  exports: [UsuariosRoutingModule]
})

export class UsuariosModule {
}
