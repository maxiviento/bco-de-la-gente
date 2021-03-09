import { CustomValidators } from "./../shared/forms/custom-validators";
import { Component, OnDestroy, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl
} from "@angular/forms";
import { Router } from "@angular/router";
import { NotificacionService } from "../shared/notificacion.service";
import { Title } from "@angular/platform-browser";
import TituloBanco from "../shared/titulo-banco";
import { NgbUtils } from "../shared/ngb/ngb-utils";
import { ConsultaMonitorProcesos } from "../prestamos-checklists/shared/modelos/consulta-procesos.model";
import { MonitorProcesoService } from "../shared/servicios/monitor-procesos.service";
import { Pagina, ELEMENTOS } from "../shared/paginacion/pagina-utils";
import { Observable, Subject } from "rxjs";
import { BandejaMonitorResultado } from "../prestamos-checklists/shared/modelos/bandeja-monitor-resultado.model";
import { NgbDatepickerConfig } from "@ng-bootstrap/ng-bootstrap";
import { DateUtils } from "../shared/date-utils";
import { ArchivoService } from '../shared/archivo.service';
import { PrestamoService } from '../shared/servicios/prestamo.service';

@Component({
  selector: "bg-monitor-procesos",
  templateUrl: "./monitor-procesos.component.html",
  styleUrls: ["./monitor-procesos.component.scss"]
})
export class MonitorProcesosComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public monitorConsulta: ConsultaMonitorProcesos;
  public resultados: BandejaMonitorResultado[] = [];
  public estados = [];
  public usuarios = [];
  public tipos = [];
  public idsTipo: string;
  public idsEstado: string;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public totalizador: number = 0;

  constructor(
    private fb: FormBuilder,
    private notificacionService: NotificacionService,
    private config: NgbDatepickerConfig,
    private router: Router,
    private archivoService: ArchivoService,
    private monitorService: MonitorProcesoService,
    private prestamoService: PrestamoService,
    private titleService: Title
  ) {
    this.titleService.setTitle("Monitor Procesos " + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    if (!this.monitorConsulta) {
      this.monitorConsulta = new ConsultaMonitorProcesos();
      this.monitorConsulta.fechaAlta = new Date(Date.now());
    }
    this.crearForm();
    this.configurarPaginacion();
    this.limiteFecha();
    this.prestamoService.consultarUsuariosCombo(
    ).subscribe((usuarios) => this.usuarios = usuarios);


  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.config);
    MonitorProcesoService.guardarFiltros(null);
  }

  private limiteFecha() {
    DateUtils.setMaxDateDP(new Date(), this.config);
  }

  public crearForm(): void {
    let fechaAltaFc = new FormControl(
      NgbUtils.obtenerNgbDateStruct(this.monitorConsulta.fechaAlta),
      Validators.compose([Validators.required, CustomValidators.maxDate(new Date())])
    );
    fechaAltaFc.valueChanges.debounceTime(500).subscribe(value => {
      if (NgbUtils.obtenerDate(value)) {
        let fechaDesdeMilisec = Date.parse(
          NgbUtils.obtenerDate(value).toISOString()
        );
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec
          ? (minDate = new Date(fechaDesdeMilisec))
          : (minDate = new Date(fechaActualMilisec));
      }
    });
    this.form = this.fb.group({
      fechaAlta: fechaAltaFc,
      estado: [this.monitorConsulta.idsEstado],
      tipo: [this.monitorConsulta.idsTipo],
      usuario: [this.monitorConsulta.idUsuario],
    });
  }

  public consultarProcesos(consultarTotal: boolean): void {
    this.consultarPaginaSiguiente();
    if (consultarTotal) {
      this.consultarTotalizador(this.monitorConsulta);
    }
  }

  public guardarEstadosSeleccionadas(estados: string[]) {
    this.idsEstado = estados ? estados.join(",") : null;
  }

  public guardarTiposSeleccionados(tipos: string[]) {
    this.idsTipo = tipos ? tipos.join(",") : null;
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map(numeroPagina => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.monitorConsulta = this.prepararConsulta();
        this.monitorConsulta.numeroPagina = params.numeroPagina;
        return this.monitorService.consultarBandejaMonitorProcesos(
          this.monitorConsulta
        );
      })
      .share();
    (<Observable<BandejaMonitorResultado[]>>(
      this.pagina.pluck(ELEMENTOS)
    )).subscribe(procesos => {
      this.resultados = procesos;
      MonitorProcesoService.guardarFiltros(this.monitorConsulta);
    });
  }

  public consultarTotalizador(filtros: ConsultaMonitorProcesos) {
    this.totalizador = 0;
    this.monitorService
      .consultarTotalizador(filtros)
      .subscribe(num => (this.totalizador = num));
  }

  public consultarPaginaSiguiente(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  private prepararConsulta(): ConsultaMonitorProcesos {
    let consulta = new ConsultaMonitorProcesos();
    consulta.fechaAlta = NgbUtils.obtenerDate(this.form.value.fechaAlta || this.monitorConsulta.fechaAlta);
    consulta.idsEstado = this.idsEstado;
    consulta.idsTipo = this.idsTipo;
    consulta.idUsuario = this.form.value.usuario;
    return consulta;
  }

  public descargarReporte(proceso: BandejaMonitorResultado) {
    this.monitorService
      .descargarReporte(proceso)
      .subscribe((archivo) => {
        if (archivo) {
          this.archivoService.descargarArchivo(archivo);
        } else {
          this.notificacionService.informar(['No se encontró el archivo seleccionado'], true);
        }
      });
  }

  public cancelarProceso(idProceso: number) {
    this.monitorService.cancelarProceso(idProceso).subscribe((mensaje) => {
      this.notificacionService.informar([mensaje], false);
      this.consultarProcesos(true);
    })
  }

  public validarCancelacion(proceso: BandejaMonitorResultado) {
    return proceso.idEstado !== 1;
  }
}
