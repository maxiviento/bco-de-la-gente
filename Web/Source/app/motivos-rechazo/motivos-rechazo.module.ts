import {NgModule} from "@angular/core";
import {SharedModule} from "../shared/shared.module";
import {MotivosRechazoRouting} from "./motivos-rechazo.routing";
import {MotivosRechazoComponent} from "./motivos-rechazo.component";
import {MotivosRechazoService} from "./shared/motivos-rechazo.service";
import {ApartadoMotivoRechazoComponent} from "./shared/apartado-motivo-rechazo/apartado-motivo-rechazo.component";
import {NuevoMotivoRechazoComponent} from "./nuevo-motivo-rechazo/nuevo-motivo-rechazo.component";
import {ConsultaMotivoRechazoComponent} from "./consulta-motivo-rechazo/consulta-motivo-rechazo.component";
import {EdicionMotivoRechazoComponent} from "./edicion-motivo-rechazo/edicion-motivo-rechazo.component";
import {EliminacionMotivoRechazoComponent} from "./eliminacion-motivo-rechazo/eliminacion-motivo-rechazo.component";

@NgModule({
  imports: [
    SharedModule,
    MotivosRechazoRouting
  ],
  declarations: [
    MotivosRechazoComponent,
    ConsultaMotivoRechazoComponent,
    EdicionMotivoRechazoComponent,
    EliminacionMotivoRechazoComponent,
    NuevoMotivoRechazoComponent,
    ApartadoMotivoRechazoComponent
  ],
  providers: [MotivosRechazoService],
})

export class MotivosRechazoModule {
}
