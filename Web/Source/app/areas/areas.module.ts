import { NgModule } from "@angular/core";

import { SharedModule } from "../shared/shared.module";
import { AreaRoutingModule } from "./areas-routing.module";

import { AreaComponent } from "./areas.component";
import { ConsultaAreaComponent } from "./consulta-area/consulta-area.component";
import { EdicionAreaComponent } from "./edicion-area/edicion-area.component";
import { EliminacionAreaComponent } from "./eliminacion-area/eliminacion-area.component";
import { NuevaAreaComponent } from "./nueva-area/nueva-area.component";

@NgModule({
  imports: [SharedModule,
    AreaRoutingModule
  ],
  declarations: [AreaComponent, ConsultaAreaComponent, EdicionAreaComponent, EliminacionAreaComponent, NuevaAreaComponent],
  exports: [AreaRoutingModule]
})

export class AreaModule {

}
