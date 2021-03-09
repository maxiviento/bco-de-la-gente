import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { NotificacionService } from '../../shared/notificacion.service';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { FormGroup } from '@angular/forms';
import { RentasService } from '../rentas.service';
import { SintysService } from '../sintys.service';
import { CondicionEconomica } from '../modelo/condicion-economica.model';
import { Persona } from '../../shared/modelo/persona.model';
import { Router } from '@angular/router';
import { FormulariosService } from '../../formularios/shared/formularios.service';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-condicion-economica',
  styleUrls: ['./condicion-economica.component.scss'],
  templateUrl: './condicion-economica.component.html',
  providers: [RentasService, SintysService]
})

export class CondicionEconomicaComponent implements OnInit {
  public pdfRentas = new BehaviorSubject<SafeResourceUrl>(null);
  public datos: CondicionEconomica [] = [];
  public form: FormGroup;
  public garante: Persona = new Persona();
  public titulo: string = 'CONSULTA CONDICIÓN ECONÓMICA';
  private esRentas: boolean = true;

  constructor(private sanitizer: DomSanitizer,
              private notificacionService: NotificacionService,
              private rentasService: RentasService,
              private formularioService: FormulariosService,
              private sintysService: SintysService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Consulta condición económica ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    if (this.router.url.includes('control-sintys')) {
      this.titulo = 'CONTROL SINTYS';
      this.titleService.setTitle('Control Sintys ' + TituloBanco.TITULO);
      this.esRentas = false;
    }
  }

  public personaConsultada(persona: Persona) {
    this.formularioService.existeGrupoPersona(persona).subscribe((tieneGrupo) => {
      if (tieneGrupo) {
        this.garante = persona;
        if (this.esRentas) {
          this.generarReporteCondicionEconomica();
        } else {
          this.generarReporteControlSintys();
        }
      } else {
        this.notificacionService.informar(['La persona ingresada no pertenece a un grupo familiar.'], true);
      }
    }, (errores) => {
      this.notificacionService.informar(errores, true);
    });
  }

  public generarReporteCondicionEconomica() {
    this.rentasService.generarPdfGrupoFamiliar({
      dni: this.garante.nroDocumento,
      sexo: this.garante.sexoId,
      pais: this.garante.codigoPais
    }).subscribe((resultado) => {
      this.pdfRentas.next(this.sanitizer.bypassSecurityTrustResourceUrl(resultado));
      RentasService.guardarReporte(this.pdfRentas.getValue());
      this.router.navigate(['/condicion-economica-individual']);
    }, (error) => {
      this.notificacionService.informar(error, true);
    });
  }

  public generarReporteControlSintys() {
    this.sintysService.generarPdfSintys({
      dni: this.garante.nroDocumento,
      sexo: this.garante.sexoId,
      pais: this.garante.codigoPais
    }).subscribe((resultado) => {
      this.pdfRentas.next(this.sanitizer.bypassSecurityTrustResourceUrl(resultado));
      RentasService.guardarReporte(this.pdfRentas.getValue());
      this.router.navigate(['/control-sintys-individual']);
    }, (error) => {
      this.notificacionService.informar(error, true);
    });
  }
}
