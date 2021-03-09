import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { ArchivoService } from '../../../../shared/archivo.service';
import { FormulariosService } from '../../../../formularios/shared/formularios.service';
import { FiltrosFormularioConsulta } from '../../modelos/filtros-formulario-consulta.model';
import { Persona } from '../../../../shared/modelo/persona.model';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';
import { ReporteResultado } from '../../../../shared/modelo/reporte-resultado.model';
import TituloBanco from '../../../../shared/titulo-banco';

@Component({
  selector: 'bg-deuda-grupo-conviviente',
  templateUrl: './deuda-grupo-conviviente.component.html',
  styleUrls: ['./deuda-grupo-conviviente.component.scss']
})

export class DeudaGrupoConvivienteComponent implements OnInit {
  @Output() public clickVolver: EventEmitter<boolean> = new EventEmitter<boolean>();
  public mostrarFiltros: boolean = false;
  private filtros: FiltrosFormularioConsulta = new FiltrosFormularioConsulta();
  public persona: Persona = new Persona();

  public pdfDeuda = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;
  public procesando: boolean = false;
  public reporteResultado: ReporteResultado;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer,
              private formularioService: FormulariosService,
              private archivoService: ArchivoService,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Deuda grupo conviviente ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.mostrarFiltros = !this.router.url.includes('prestamo');
    if (!this.mostrarFiltros) {
      this.route.params.subscribe((params: Params) => {
        this.filtros.nroFormulario = +params['id'];
        this.filtros.idPrestamoItem = Number.parseInt(localStorage.getItem('idPrestamoRequisitoGC'));
        this.consultarDeuda();
      });
    }
  }

  public personaConsultada(persona: Persona) {
    this.persona = persona;
    this.consultarDeuda();
  }

  public cerrar(): void {
    this.router.navigate(['/']);
  }

  public consultarDeuda() {
    if (this.persona.nroDocumento) {
      this.formularioService.existeGrupoPersona(this.persona).subscribe((tieneGrupo) => {
        if (tieneGrupo) {
          this.generarReporte()
        } else {
          this.notificacionService.informar(['La persona ingresada no pertenece a un grupo familiar.'], true);
        }
      }, (errores) => {
        this.procesando = false;
        this.notificacionService.informar(errores, true);
      });
    } else {
      //La variable se llama nroFormulario, pero lo que se guarda es el ID.
      this.formularioService.existeGrupoSolicitante(this.filtros.nroFormulario).subscribe((tieneGrupo) => {
        if (tieneGrupo) {
          this.generarReporte()
        } else {
          this.notificacionService.informar(['El solicitante no pertenece a un grupo familiar.'], true);
        }
      }, (errores) => {
        this.procesando = false;
        this.notificacionService.informar(errores, true);
      });
    }
  }

  public generarReporte() {
    this.reporteResultado = null;
    this.procesando = true;
    this.formularioService.reporteDeudaGrupoConviviente(this.prepararFiltros())
      .subscribe((resultado) => {
        if (resultado) {
          this.procesando = false;
          if (resultado.errores && resultado.errores.length > 0) {
            this.notificacionService.informar(resultado.errores, true);
            return;
          }
          this.pdfDeuda.next(this.sanitizer.bypassSecurityTrustResourceUrl(this.archivoService.getUrlPrevisualizacionArchivo(resultado.archivos[0])));
          this.reporteSource = this.pdfDeuda.getValue();
          this.reporteResultado = resultado;
        }
      }, (errores) => {
        this.procesando = false;
        this.notificacionService.informar(errores, true);
      });
  }

  private prepararFiltros(): FiltrosFormularioConsulta {
    if (this.mostrarFiltros) {
      this.filtros.dni = this.persona.nroDocumento;
    }
    return this.filtros;
  }

  public descargarArchivos(): void {
    if (this.reporteResultado && this.reporteResultado.archivos) {
      this.archivoService.descargarArchivos(this.reporteResultado.archivos);
    }
  }

  public volver(): void {
    if (this.router.url.includes('prestamo')) {
      window.close();
    } else {
      this.router.navigate(['/']);
    }
  }
}
