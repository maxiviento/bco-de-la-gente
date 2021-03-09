import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';
import { SoporteRoutingModule } from './soporte-routing.module';

import { AdministrarParametrosComponent } from './administrar-parametros/administrar-parametros.component';
import { ParametrosGrillaComponent } from './administrar-parametros/parametros-grilla/parametros-grilla.component';
import { ApartadoParametroComponent } from './administrar-parametros/apartado-parametro/apartado-parametro.component';
import { ParametroService } from './parametro.service';
import { CondicionEconomicaComponent } from './condicion-economica/condicion-economica.component';
import { FormularioModule } from '../formularios/formularios.module';
import { ReporteRentasIndividualComponent } from './condicion-economica/reporte-rentas.component';
import { DeudaGrupoConvivienteComponent } from '../seleccion-formularios/shared/componentes/deuda-grupo-conviviente/deuda-grupo-conviviente.component';
import { ManualesComponent } from './manuales/manuales.component';
import { ManualesService } from './manuales.service';
import { ConsultarTablasDefinidasComponent } from './tablas-satelite/consulta/consulta-tablas-definidas.component';
import { TablasDefinidasService } from './tablas-definidas.service';
import { EditarTablaDefinidasComponent } from './tablas-satelite/edicion/editar-tabla-definidas.component';
import { NuevaParametroTablaDefinidaComponent } from './tablas-satelite/nuevo-parametro/nuevo-parametro-tabla-definida.component';
import { ConsultarParametroTablaDefinidaComponent } from './tablas-satelite/consulta-parametro/consultar-parametro-tabla-definida.component';
import { GenerarInformesBancoComponent } from './informes/generar-informes-banco.component';

@NgModule({
  imports: [
    FormularioModule,
    NgbModule,
    SoporteRoutingModule,
    SharedModule],
  declarations: [
    AdministrarParametrosComponent,
    ParametrosGrillaComponent,
    ApartadoParametroComponent,
    CondicionEconomicaComponent,
    ReporteRentasIndividualComponent,
    DeudaGrupoConvivienteComponent,
    ManualesComponent,
    ConsultarTablasDefinidasComponent,
    EditarTablaDefinidasComponent,
    NuevaParametroTablaDefinidaComponent,
    ConsultarParametroTablaDefinidaComponent,
    GenerarInformesBancoComponent
  ],
  providers: [ParametroService, ManualesService, TablasDefinidasService],
  exports: [],
  entryComponents: [ApartadoParametroComponent]
})
export class SoporteModule {
}
