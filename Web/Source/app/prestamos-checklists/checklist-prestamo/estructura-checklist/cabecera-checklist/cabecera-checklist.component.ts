import { ActivatedRoute, Params, Router } from '@angular/router';
import { PrestamoService } from '../../../../shared/servicios/prestamo.service';
import { IntegrantePrestamo } from '../../../../shared/modelo/integrante-prestamo.model';
import { Prestamo } from '../../../shared/modelos/prestamo.model';
import { Observable, Subject, Subscription } from 'rxjs';
import { Persona } from '../../../../shared/modelo/persona.model';
import { VigenciaParametro } from '../../../../shared/modelo/vigencia-parametro.model';
import { VigenciaParametroConsulta } from '../../../../shared/modelo/consultas/vigencia-parametro-consulta.model';
import { ConfiguracionChecklistService } from '../../../../configuracion-checklist/shared/configuracion-checklist.service';
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Pagina } from '../../../../shared/paginacion/pagina-utils';
import { ParametroService } from '../../../../soporte/parametro.service';
import { DetalleLineaPrestamo } from '../../../../lineas/shared/modelo/detalle-linea-prestamo.model';
import { EtapaEstadoLinea } from '../../../shared/modelos/etapa-estado-linea.model';
import { LineaService } from '../../../../shared/servicios/linea-prestamo.service';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { NgbUtils } from '../../../../shared/ngb/ngb-utils';
import { DataSharedChecklistService } from '../data-shared-checklist.service';
import { GarantePrestamo } from '../../../../shared/modelo/garante-prestamo.mode';

@Component({
  selector: 'bg-cabecera-checklist-prestamo',
  templateUrl: './cabecera-checklist.component.html',
  styleUrls: ['./cabecera-checklist.component.scss'],
  providers: [PrestamoService],
  encapsulation: ViewEncapsulation.None,
})
export class CabeceraChecklistComponent implements OnInit {
  @Input() public editable: boolean;
  @Input() public titulo: string;
  @Input() public integrantesPrestamo: IntegrantePrestamo [];
  @Input() public tipoLinea: number;
  public form: FormGroup;
  public idPrestamo: number;
  public prestamo: Prestamo = new Prestamo();
  @Input() public garantesPrestamo: GarantePrestamo [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public integrantesFueraDeEdadParam: Persona [] = [];
  public cantMinIntIndividual: VigenciaParametro;
  public cantMinIntAsociativo: VigenciaParametro;
  public idIntegranteSocio = 0;
  public etapasEstadosLinea: EtapaEstadoLinea[] = [];
  public subscription: Subscription;
  public subscriptionObservaciones: Subscription;

  constructor(private prestamoService: PrestamoService,
              private configuracionChecklistService: ConfiguracionChecklistService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private parametroService: ParametroService,
              private lineaService: LineaService,
              private fb: FormBuilder,
              private dataSharedChecklistService: DataSharedChecklistService) {
  }

  public ngOnInit(): void {
    this.form = this.fb.group({
      totalFolios: [this.prestamo.totalFolios, Validators.compose([CustomValidators.number, Validators.maxLength(3)])],
      observaciones: ['', Validators.maxLength(500)],
      modalRechazo: ['']
    });
    this.activatedRoute.params.subscribe((params: Params) => {
      this.idPrestamo = params['id'];
      this.integrantesFueraDeEdadParam = this.integrantesPrestamo.map((integrante) => {
        let persona = new Persona();
        persona.fechaNacimiento = integrante.fechaNacimiento;
        persona.nombre = integrante.apellidoNombre.split(',')[0];
        persona.apellido = integrante.apellidoNombre.split(',')[1];
        return persona;
      });
      this.parametroService
        .obtenerVigenciaParametro(new VigenciaParametroConsulta(1))
        .subscribe((min) => {
          this.cantMinIntIndividual = min;
          this.parametroService
            .obtenerVigenciaParametro(new VigenciaParametroConsulta(2))
            .subscribe((max) => {
              this.cantMinIntAsociativo = max;
            });
        });
    });
    let promesaDatos = new Promise((resolve) =>
      this.prestamoService.consultarDatosPrestamo(this.idPrestamo)
        .subscribe((datosPrestamo) => {
          this.prestamo = datosPrestamo;
          this.form.get('totalFolios').setValue(this.prestamo.totalFolios);
          this.obtenerTipoSocioIntegranteLinea();
          // Consulta el array de etapas estados linea con el parametro del id de la linea del préstamo
          this.configuracionChecklistService.consultarEtapasEstadosLinea(this.prestamo.idLinea, this.idPrestamo)
            .subscribe((etapas) => {
              this.etapasEstadosLinea = etapas;
              return resolve();
            });
        }));

    this.subscriptionObservaciones = this.dataSharedChecklistService.getSubjecObservaciones().subscribe((observaciones: string) => {
        this.form.get('observaciones').setValue(observaciones);
      }
    );

    this.subscription = this.dataSharedChecklistService.getSubjectRechazo().subscribe((rechazo: boolean) => {
        if (rechazo) {
          this.prestamoService.consultarIntegrantes(this.idPrestamo)
            .subscribe((integrantes) => {
              this.integrantesPrestamo = integrantes;
            });
        }
      }
    );
  }

  private obtenerTipoSocioIntegranteLinea(): void {
    this.lineaService.consultarDetallePorIdLineaSinPaginar(this.prestamo.idLinea).subscribe(
      (res: DetalleLineaPrestamo[]) => {
        this.idIntegranteSocio = res[0].idSocioIntegrante;
      });
  };

  public calcularEdad(fecha: Date): number {
    return NgbUtils.calcularEdad(fecha);
  }

  public actualizarObservaciones(): void {
    this.dataSharedChecklistService.modificarObservaciones(this.form.get('observaciones').value);
  }

  public actualizarFolios(): void {
    this.dataSharedChecklistService.modificarCantFolios(this.form.get('totalFolios').value);
  }

  public obtenerMotivosRechazo(integrante: IntegrantePrestamo): any {
    let motivos = integrante.motivoRechazo.split(',');
    return motivos;
  }
}
