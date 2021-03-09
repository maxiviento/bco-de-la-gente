import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { ItemsRoutingModule } from "./items-routing.module";
import { ItemsComponent } from "./items.component";
import { ItemsService } from "./shared/items.service";
import { NuevoItemComponent } from "./nuevo-item/nuevo-item.component";
import { ConsultaItemComponent } from "./consulta-item/consulta-item.component";
import { ApartadoItemComponent } from "./shared/apartado-item.component";
import { EdicionItemComponent } from "./edicion-item/edicion-item.component";
import { EliminacionItemComponent } from "./eliminacion-item/eliminacion-item.component";
import { TipoDocumentacionService } from '../shared/servicios/tipo-documentacion.service';

@NgModule({
  imports: [
    SharedModule,
    ItemsRoutingModule],
  declarations: [ItemsComponent,
    NuevoItemComponent,
    ConsultaItemComponent,
    ApartadoItemComponent,
    EliminacionItemComponent,
    EdicionItemComponent],
  providers: [ItemsService,
    TipoDocumentacionService],
  exports: [ItemsRoutingModule,
    ConsultaItemComponent,
    EliminacionItemComponent,
    NuevoItemComponent,
    ApartadoItemComponent,
    EdicionItemComponent],
})

export class ItemsModule {
}
