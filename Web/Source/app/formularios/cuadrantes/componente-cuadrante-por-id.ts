import { ComponentFactoryResolver } from '@angular/core';
import { SolicitanteComponent } from './solicitante/solicitante.component';
import { GrupoUnicoComponent } from '../../grupo-unico/grupo-unico.component';
import { DestinoFondosComponent } from './destino-fondos/destino-fondos.component';
import { CondicionesSolicitadasComponent } from './condiciones-solicitadas/condiciones-solicitadas.component';
import { CursoComponent } from './cursos/cursos.component';
import { GaranteComponent } from './garante/garante.component';
import { GrupoUnicoGaranteComponent } from '../../grupo-unico/grupo-unico-garante.component';
import { GrupoFamiliarSolicitanteComponent } from './grupo-familiar-solicitante/grupo-familiar-solicitante.component';
import { GrupoFamiliarGaranteComponent } from './grupo-familiar-garante/grupo-familiar-garante.component';
import { CuadranteRequisitosComponent } from './requisitos/cuadrante-requisitos.component';
import { PatrimonioSolicitanteComponent } from './patrimonio-solicitante/patrimonio-solicitante.component';
import { DatosEmprendimientoComponent } from './datos-emprendimiento/datos-emprendimiento.component';
import { OrganizacionIndividualComponent } from './organizacion-individual/organizacion-individual.component';
import { DescripcionProyectoComponent } from './descripcion-proyecto/descripcion-proyecto.component';
import { MercadoYComercializacionComponent } from './mercado-y-comercializacion/mercado-y-comercializacion.component';
import { InversionRealizadaComponent } from './inversion-realizada/inversion-realizada.component';
import { DeudaEmprendimientoComponent } from './deuda-emprendimiento/deuda-emprendimiento.component';
import { IngresosYGastosActualesComponent } from './ingresos-y-gastos-actuales/ingresos-y-gastos-actuales.component';
import { NecesidadesInversionComponent } from './necesidades-inversion/necesidades-inversion.component';
import { PrecioVentaComponent } from './precio-venta/precio-venta.component';
import { ResultadoEstimadoMensualComponent } from './resultado-estimado-mensual/resultado-estimado-mensual.component';
import { IntegrantesPersonasComponent } from './integrantes-personas/integrantes-personas.component';
import { ONGComponent } from './ong/ong.component';

export function componentePorId(idComponente: number, componentFactoryResolver: ComponentFactoryResolver) {
  let tipoComponente;
  switch (idComponente) {
    case 1:
      tipoComponente = SolicitanteComponent;
      break;
    case 2:
      tipoComponente = GrupoUnicoComponent;
      break;
    case 3:
      tipoComponente = DestinoFondosComponent;
      break;
    case 4:
      tipoComponente = CondicionesSolicitadasComponent;
      break;
    case 5:
      tipoComponente = CursoComponent;
      break;
    case 6:
      tipoComponente = GaranteComponent;
      break;
    case 7:
      tipoComponente = DatosEmprendimientoComponent;
      break;
    case 8:
      tipoComponente = PatrimonioSolicitanteComponent;
      break;
    case 9:
      tipoComponente = OrganizacionIndividualComponent;
      break;
    case 10:
      tipoComponente = MercadoYComercializacionComponent;
      break;
    case 11:
      tipoComponente = InversionRealizadaComponent;
      break;
    case 12:
      tipoComponente = DeudaEmprendimientoComponent;
      break;
    case 13:
      tipoComponente = NecesidadesInversionComponent;
      break;
    case 14:
      tipoComponente = DescripcionProyectoComponent;
      break;
    case 15:
      tipoComponente = PrecioVentaComponent;
      break;
    case 16:
      tipoComponente = ResultadoEstimadoMensualComponent;
      break;
    case 17:
      tipoComponente = IngresosYGastosActualesComponent;
      break;
    case 18:
      tipoComponente = GrupoUnicoGaranteComponent;
      break;
    case 19:
      tipoComponente = GrupoFamiliarSolicitanteComponent;
      break;
    case 20:
      tipoComponente = GrupoFamiliarGaranteComponent;
      break;
    case 21:
      tipoComponente = CuadranteRequisitosComponent;
      break;
    case 22:
      tipoComponente = IntegrantesPersonasComponent;
      break;
    case 23:
      tipoComponente = ONGComponent;
      break;
    default:
      return null;
  }
  return componentFactoryResolver.resolveComponentFactory(tipoComponente);
}
