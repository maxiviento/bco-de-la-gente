import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../../../shared/ngb/ngb-utils';
import { ActualizarFechaPagoFormularioComando } from '../../../../formularios/shared/modelo/actualizar-fecha-pago-formulario.comando';
import { PrestamoService } from '../../../../shared/servicios/prestamo.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { FormulariosService } from '../../../../formularios/shared/formularios.service';
import { FormularioFechaPago } from '../../../../formularios/shared/modelo/formulario-fecha-pago.model';
import { ModalMotivoRechazoComponent } from '../../../../formularios/modal-motivo-rechazo/modal-motivo-rechazo.component';
import { RechazarFormularioComando } from '../../../../formularios/shared/modelo/rechazar-formulario-comando.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { Reprogramacion } from '../../../../formularios/shared/modelo/procesos-varios.model';
import { MotivoRechazoComando } from '../../../../formularios/shared/modelo/motivo-rechazo-comando.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../../shared/titulo-banco';

@Component({
  selector: 'bg-actualizar-fecha-pago',
  templateUrl: 'actualizar-fecha-pago.component.html',
  styleUrls: ['actualizar-fecha-pago.component.scss']
})
export class ActualizarFechaPagoComponent implements OnInit {
  public form: FormGroup;
  public formReprogramacion: FormGroup;
  public mensaje = '';
  public esReprogramacion: boolean = false;
  public tiempoReprogramacion: number;
  public formulario: FormularioFechaPago = new FormularioFechaPago();
  public comando: ActualizarFechaPagoFormularioComando = new ActualizarFechaPagoFormularioComando();
  public reprograma: boolean = false;
  public lsReprogramaciones: Reprogramacion[] = [];
  public procesoTerminado: boolean = false;

  constructor(private fb: FormBuilder,
              private prestamoService: PrestamoService,
              private route: ActivatedRoute,
              private notificationService: NotificacionService,
              private modalService: NgbModal,
              private formulariosService: FormulariosService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Reprogramación ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();

    this.route.params.switchMap(
      (params: Params) =>
        this.formulariosService.obtenerFormularioFechaPago(+params['id'])).subscribe(
      (formulario) => {
        this.formulario = formulario;
        this.formulariosService.obtenerTiempoReprogramacion().subscribe(
          (valorParametro) => {
            this.tiempoReprogramacion = valorParametro;
            this.crearForm();
          }
        );
      });
  }

  private crearForm(): void {
    if (this.formulario.estadoForm !== 11) {
      this.mensaje = 'No se pueden realizar acciones hasta que el formulario se encuentre en estado "impago".';
    } else {
      let fechaNuevaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.comando.nuevaFecha),
        Validators.compose([Validators.required, CustomValidators.minDate(this.formulario.fecFinPago), CustomValidators.date]));
      let fechaFinNuevaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.comando.fechaFinNueva), Validators.compose([Validators.required, CustomValidators.date, CustomValidators.minDate(new Date())])
      );
      fechaNuevaFc.valueChanges.distinctUntilChanged().subscribe((value) => {
        if (NgbUtils.obtenerDate(value)) {
          fechaFinNuevaFc.clearValidators();
          let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
          let fechaFormularioMilisec = Date.parse(this.formulario.fecFinPago.toISOString());
          let minDate;
          fechaDesdeMilisec <= fechaFormularioMilisec ? minDate = new Date(fechaFormularioMilisec) : minDate = new Date(fechaDesdeMilisec);
          fechaFinNuevaFc.setValidators(Validators.compose([Validators.required, CustomValidators.minDate(minDate), CustomValidators.date]));
          fechaFinNuevaFc.updateValueAndValidity();
        }
      });
      fechaFinNuevaFc.valueChanges.distinctUntilChanged().subscribe((value) => {
        if (NgbUtils.obtenerDate(value)) {
          fechaNuevaFc.clearValidators();
          let fechaHastaMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
          let fechaFormularioMilisec = Date.parse(this.formulario.fecFinPago.toISOString());
          let fechaNuevaInicioMilisec = Date.parse(this.comando.nuevaFecha ? this.comando.nuevaFecha.toISOString() : undefined);
          let fechaMayor;
          fechaFormularioMilisec <= fechaNuevaInicioMilisec ? fechaMayor = fechaNuevaInicioMilisec : fechaMayor = fechaFormularioMilisec;
          let maxDate;
          fechaHastaMilisec <= fechaMayor ? maxDate = new Date(fechaMayor) : maxDate = new Date(fechaHastaMilisec);
          fechaNuevaFc.setValidators(Validators.compose([Validators.required, CustomValidators.maxDate(maxDate), CustomValidators.minDate(this.formulario.fecFinPago), CustomValidators.date]));
          fechaNuevaFc.updateValueAndValidity();
        }
      });

      this.formReprogramacion = this.fb.group(
        {
          fechaNueva: fechaNuevaFc,
          fechaFinNueva: fechaFinNuevaFc,
          fechaActual: new FormControl(NgbUtils.obtenerNgbDateStruct(this.formulario.fecInicioPago)),
          fechaFinActual: new FormControl(NgbUtils.obtenerNgbDateStruct(this.formulario.fecFinPago))
        })
      ;
      this.realizarValidaciones();
    }
  }

  public cerrar(): void {
    let tieneFiltros = FormulariosService.recuperarFiltros();
    if (tieneFiltros) {
      this.router.navigate(['formularios']);
    } else {
      window.close();
    }
  }

  public realizarValidaciones() {
    if (this.formulario.fecInicioPago) {
      let fechaActual = new Date();
      let fechaPagoUTC = new Date(Date.UTC(this.formulario.fecFinPago.getFullYear(), this.formulario.fecFinPago.getMonth() - 1, this.formulario.fecFinPago.getDay()));
      let fechaActualUTC = new Date(Date.UTC(fechaActual.getFullYear(), fechaActual.getMonth() - 1, fechaActual.getDay()));
      const diferenciaDias = Math.floor(Math.abs(<any> fechaActualUTC - <any> fechaPagoUTC) / (1000 * 60 * 60 * 24));

      this.esReprogramacion = diferenciaDias <= this.tiempoReprogramacion;

      if (this.formulario.esAsociativa) {
        if (this.formulario.tipoApoderado === 2) {
          this.mensaje = 'El formulario pertenece al apoderado del préstamo y no es posible rechazarlo.';
        } else {
          if (this.esReprogramacion) {
            this.mensaje = 'El formulario pertenece a una línea asociativa, y tiene más de ' + this.tiempoReprogramacion + ' días, ¿Desea "Rechazarlo"?';
          } else {
            this.mensaje = 'El formulario pertenece a una línea asociativa, y tiene más de ' + diferenciaDias + ' días de impago, ¿Desea "Rechazarlo"?';
          }
        }
      } else {
        this.formulariosService.obtenerHistorialReprogramacion(this.formulario.idFormulario).subscribe(
          (historial) => this.lsReprogramaciones = historial);

        if (this.esReprogramacion) {
          this.mensaje = 'El formulario tiene ' + diferenciaDias + ' días de impago, ¿Desea reprogramar?';
        } else {
          this.mensaje = 'El formulario tiene más de ' + this.tiempoReprogramacion + ' días, ¿Desea reprogramar igualmente?';
        }
      }
    }
  }

  public rechazar() {
    if (this.formulario.esAsociativa && (this.formulario.cantForms === this.formulario.cantMinForms)) {
      let mensaje = 'No se pudo rechazar el formulario ya que el préstamo no puede tener menos de ' + this.formulario.cantMinForms + ' formularios activos.';
      this.notificationService.informar(Array.of(mensaje), false).result.then(() => {
        this.cerrar();
      });
      return;
    }
    const motivoRechazo = this.modalService.open(ModalMotivoRechazoComponent, {backdrop: 'static', size: 'lg'});
    motivoRechazo.componentInstance.ambito = 'Checklist';
    motivoRechazo
      .result
      .then((motivoRechazoComando: MotivoRechazoComando) => {
        if (motivoRechazoComando) {
          this.formulariosService.rechazarFormularioConPrestamo(new RechazarFormularioComando(this.formulario.idFormulario, undefined, motivoRechazoComando.motivosRechazo, motivoRechazoComando.numeroCaja))
            .subscribe((res) => {
              this.notificationService.informar(Array.of('Formulario rechazado con éxito.'), false)
                .result.then(() => {
                this.cerrar();
                this.procesoTerminado = true;
              });
            }, (errores) => {
              this.notificationService.informar(errores, true);
            });
        }
      });
  }

  public registrar() {
    if (!(NgbUtils.obtenerDate(this.formReprogramacion.get('fechaNueva').value) && NgbUtils.obtenerDate(this.formReprogramacion.get('fechaFinNueva').value))) {
      this.notificationService.informar(['Ingrese valores válidos en los campos de fecha.'], true);
      return;
    }
    let formModel = this.formReprogramacion.value;
    let nuevaFechaDePago = NgbUtils.obtenerDate(formModel.fechaNueva);
    let nuevaFechaFinDePago = NgbUtils.obtenerDate(formModel.fechaFinNueva);
    let idFormulario = this.formulario.idFormulario ? this.formulario.idFormulario : 1;
    let comando = new ActualizarFechaPagoFormularioComando(idFormulario, nuevaFechaDePago, nuevaFechaFinDePago);
    this.prestamoService.actualizarFechaPagoFormulario(comando).subscribe(
      (respuesta) => {
        if (respuesta) {
          this.notificationService.informar(Array.of('Fecha registrada con éxito.'), false);
          this.formReprogramacion.disable();
        } else {
          this.notificationService.informar(Array.of('No se pudo actualizar la fecha.'), true);
        }
      });
  }

  public activarReprogramacion() {
    this.reprograma = true;
  }
}
