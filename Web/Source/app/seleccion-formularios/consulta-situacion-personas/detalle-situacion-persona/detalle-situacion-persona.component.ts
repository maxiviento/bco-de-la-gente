import { Component, EventEmitter, Input, OnChanges, Output, ViewChild } from '@angular/core';
import { SituacionPersonasResultado } from '../../shared/modelos/situacion-personas-resultado.model';
import { FormulariosSituacionResultado } from '../../shared/modelos/formularios-situacion-resultado.model';
import { SituacionPersonaService } from '../../shared/situacion-persona.service';
import { GrillaFormulariosSituacionBgeComponent } from './grilla-formularios-situacion-bge/grilla-formularios-situacion-bge.component';
import { MotivoRechazoReferencia } from '../../../shared/modelo/motivo-rechazo-referencia.model';
import { ArchivoService } from '../../../shared/archivo.service';
import { ConsultaSituacionPersonas } from '../../shared/modelos/consulta-situacion-personas.model';

@Component({
  selector: 'bg-detalle-situacion-persona',
  templateUrl: 'detalle-situacion-persona.component.html',
  styleUrls: ['detalle-situacion-persona.component.scss'],
  providers: [SituacionPersonaService]
})

export class DetalleSituacionPersonaComponent implements OnChanges {

  @Input() public persona: SituacionPersonasResultado;
  @Input() public formulariosAImprimir: FormulariosSituacionResultado[];

  @Input()
  public set formularios(form: FormulariosSituacionResultado[]) {
    this.formulariosAImprimir = form;
  }

  public listadoMotivos: MotivoRechazoReferencia[];

  @ViewChild(GrillaFormulariosSituacionBgeComponent)
  public apartadoFiltros: GrillaFormulariosSituacionBgeComponent;

  @Output() public emitClickVolver: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private archivoService: ArchivoService,
              private situacionPersonaService: SituacionPersonaService) {
  }

  public ngOnChanges(): void {
    document.getElementById('detalle-persona').scrollIntoView({behavior: 'smooth'});
  }

  public almacenarListadoMotivos(listado: MotivoRechazoReferencia[]) {
    this.listadoMotivos = listado;
  }

  public emitirVolver() {
    this.emitClickVolver.emit(true);
  }

  public imprimir() {
    this.situacionPersonaService.obtenerReporteSituacionPersona(new ConsultaSituacionPersonas
    (4, this.persona.cuil, this.persona.apellido, this.persona.nombre, this.persona.nroDocumento))
      .subscribe(
        (archivoReporte) => {
          this.archivoService.descargarArchivo(archivoReporte);
        }
      );
  }

  public top() {
    document.getElementById('busqueda-persona').scrollIntoView({behavior: 'smooth'});
  }
}
