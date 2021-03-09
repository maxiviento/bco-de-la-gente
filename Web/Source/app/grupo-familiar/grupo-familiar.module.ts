import { NgModule } from '@angular/core';

import { SharedModule } from '../shared/shared.module';
import { GrupoFamiliarComponent } from './grupo-familiar.component';
import { GrupoUnicoService } from '../grupo-unico/shared-grupo-unico/grupo-unico.service';
import { FormsModule } from '@angular/forms';
import { GrupoFamiliarRoutingModule } from './grupo-familiar-routing.module';
import { GrupoUnicoModule } from '../grupo-unico/grupo-unico.module';
import { ModalDatosConctactoComponent } from '../shared/modal-datos-contacto/modal-datos-conctacto.component';
import { ContactoService } from '../shared/servicios/datos-contacto.service';

@NgModule({
  imports: [
    FormsModule,
    SharedModule,
    GrupoFamiliarRoutingModule,
    GrupoUnicoModule
  ],
  declarations: [GrupoFamiliarComponent,
    ModalDatosConctactoComponent],
  providers: [GrupoUnicoService,
    ContactoService],
  exports: [
    SharedModule,
    GrupoFamiliarRoutingModule,
    ModalDatosConctactoComponent
  ],
  entryComponents: [
    ModalDatosConctactoComponent
  ]
})

export class GrupoFamiliarModule {

}
