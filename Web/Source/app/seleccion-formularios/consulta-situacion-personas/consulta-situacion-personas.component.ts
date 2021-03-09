import { Component, ViewChild } from '@angular/core';
import { SituacionPersonasResultado } from '../shared/modelos/situacion-personas-resultado.model';
import { GrillaPersonasSituacionBgeComponent } from './grilla-personas-situacion-bge/grilla-personas-situacion-bge.component';
import { ConsultaSituacionPersonas } from '../shared/modelos/consulta-situacion-personas.model';
import { FiltrosConsultaSituacionPersonasComponent } from './filtros-consulta-situacion-personas/filtros-consulta-situacion-personas.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-consulta-situacion-personas',
  templateUrl: './consulta-situacion-personas.component.html',
  styleUrls: ['./consulta-situacion-personas.component.scss']
})

export class ConsultaSituacionPersonasComponent {
  public consultaSituacion: ConsultaSituacionPersonas;
  public personaSeleccionada: SituacionPersonasResultado;

  @ViewChild(FiltrosConsultaSituacionPersonasComponent)
  public apartadoFiltros: FiltrosConsultaSituacionPersonasComponent;
  @ViewChild(GrillaPersonasSituacionBgeComponent)
  public apartadoGrillaPersonas: GrillaPersonasSituacionBgeComponent;

  constructor(private titleService: Title) {
    this.titleService.setTitle('Consultar situación bge por persona ' + TituloBanco.TITULO);
  }

  public buscarPersonas(consultaSituacionPersona: ConsultaSituacionPersonas): void {
    if (!consultaSituacionPersona) {
      this.volver();
    } else {
      this.consultaSituacion = consultaSituacionPersona;
    }
  }

  public almacenarPersonaSeleccionada(personaDeGrilla?: SituacionPersonasResultado): void {
    this.personaSeleccionada = personaDeGrilla;
  }

  public volver(): void {
    this.personaSeleccionada = null;
    this.apartadoGrillaPersonas.limpiarGrilla();
    this.apartadoFiltros.limpiarFiltros();
  }
}
