import { Component, OnInit } from '@angular/core';
import { ModalMotivoRechazoComponent } from '../../formularios/modal-motivo-rechazo/modal-motivo-rechazo.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificacionService } from '../../shared/notificacion.service';
import { ActivatedRoute } from '@angular/router';
import { PrestamoService } from '../../shared/servicios/prestamo.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegistrarRechazoReactivacionPrestamo } from '../../formularios/shared/modelo/registrar-rechazo-reactivacion-prestamo.model';
import { MotivoRechazoComando } from '../../formularios/shared/modelo/motivo-rechazo-comando.model';
import { ReactivarPrestamoComando } from '../../formularios/shared/modelo/reactivar-prestamo-comando.model';
import { DatosPrestamoReactivacionResultado } from '../../formularios/shared/modelo/datos-prestamo-reactivacion-resultado.model';
import { MotivorechazoPrestamo } from '../../formularios/shared/modelo/motivo-rechazo-prestamo.model';
import { MotivoRechazo } from '../../formularios/shared/modelo/motivo-rechazo';
import { FormulariosService } from '../../formularios/shared/formularios.service';
import { Persona } from '../../shared/modelo/persona.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-reactivacion',
  templateUrl: './reactivacion.component.html',
  styleUrls: ['./reactivacion.component.scss'],
})

export class ReactivacionComponent implements OnInit {
  public form: FormGroup;
  public procesoTerminado: boolean = false;
  public datosPrestamo = new DatosPrestamoReactivacionResultado();
  public lsMotivosRechazoPrestamo: MotivorechazoPrestamo[] = [];
  public lsMotivosRechazo: MotivoRechazo[] = [];

  constructor(private modalService: NgbModal,
              private notificationService: NotificacionService,
              private route: ActivatedRoute,
              private prestamoService: PrestamoService,
              private fb: FormBuilder,
              private formulariosService: FormulariosService,
              private titleService: Title) {
    this.titleService.setTitle('Reactivación ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.obtenerDatos();
    this.crearForm();
  }

  private obtenerDatos() {
    let idPrestamo = this.route.snapshot.params['id'];
    this.prestamoService.obtenerDatosReactivacion(idPrestamo).subscribe(
      (datosPrestamo) => {
        this.datosPrestamo = datosPrestamo;
      });
    this.prestamoService.obtenerMotivosRechazoPrestamo(idPrestamo).subscribe(
      (rechazos) => {
        this.lsMotivosRechazoPrestamo = rechazos;
        if (this.lsMotivosRechazoPrestamo.length) {
          this.armarListaMotivosRechazo();
        }
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      observacion: ['', Validators.compose([
        Validators.required,
        Validators.maxLength(500)
      ])]
    });
  }

  public rechazar() {
    const motivoRechazo = this.modalService.open(ModalMotivoRechazoComponent, {backdrop: 'static', windowClass: 'modal-l'});
    motivoRechazo.componentInstance.ambito = 'Prestamo';
    motivoRechazo.componentInstance.lsMotivosRechazoAnteriores = this.lsMotivosRechazo;
    motivoRechazo.componentInstance.nroCaja = this.datosPrestamo.nroCaja;
    motivoRechazo
      .result
      .then((motivoRechazoComando: MotivoRechazoComando) => {
        if (motivoRechazoComando) {
          let comando = new RegistrarRechazoReactivacionPrestamo(this.datosPrestamo.idPrestamo, motivoRechazoComando.numeroCaja, motivoRechazoComando.motivosRechazo);
          this.prestamoService.rechazarReactivacion(comando).subscribe((res) => {
            this.notificationService.informar(Array.of('Préstamo rechazado con éxito.'), false)
              .result.then(() => {
              this.procesoTerminado = true;
              this.form.get('observacion').disable();
              this.obtenerDatos();
            });
          }, (errores) => {
            this.notificationService.informar(errores, true);
          });
        }
      });
  }

  public reactivar() {
    if ((this.form.value.observacion.replace(/[^A-Za-z]/g, '').length === 0)) {
      this.notificationService.informar(['Debe ingresar una observación.']);
      return;
    }
    this.notificationService.confirmar('¿Está seguro que desea reactivar el préstamo?')
      .result
      .then((res) => {
        if (res) {
          let solicitante = this.crearPersonaParaReactivacion(this.datosPrestamo.idSexo, this.datosPrestamo.numeroDocumento, this.datosPrestamo.idNumero, this.datosPrestamo.codigoPais);
          this.formulariosService.existeFormularioEnCursoParaPersonaReactivacion(solicitante)
            .subscribe((existeFormulario) => {
              if (existeFormulario) {
                this.notificationService.informar(Array.of('No es posible reactivar, el solicitante posee un formulario en curso.'), true);
              } else {
                let garante = this.crearPersonaParaReactivacion(this.datosPrestamo.idSexoGarante, this.datosPrestamo.numeroDocumentoGarante, this.datosPrestamo.idNumeroGarante, this.datosPrestamo.codigoPaisGarante);
                this.formulariosService.existeFormularioEnCursoParaPersonaReactivacion(garante).subscribe((existeFormularioGarante) => {
                  if (existeFormularioGarante) {
                    this.notificationService.informar(Array.of('No es posible reactivar, el garante posee un formulario en curso.'), true);
                  } else {
                    let comando = this.crearComandoReactivacion();
                    this.prestamoService.reactivarPrestamo(comando).subscribe((res) => {
                      this.notificationService.informar(Array.of('Préstamo reactivado con éxito.'), false)
                        .result.then(() => {
                        this.procesoTerminado = true;
                        this.form.get('observacion').disable();
                        this.prestamoService.obtenerMotivosRechazoPrestamo(this.datosPrestamo.idPrestamo).subscribe(
                          (rechazos) => {
                            this.lsMotivosRechazoPrestamo = rechazos;
                          });
                      });
                    }, (errores) => {
                      this.notificationService.informar(errores, true);
                    });
                  }
                });
              }
            });
        } else {
          return;
        }
      });
  }

  private crearComandoReactivacion(): ReactivarPrestamoComando {
    let formModel = this.form.value;
    let observacion = formModel.observacion;
    return new ReactivarPrestamoComando(this.datosPrestamo.idFormulario, this.datosPrestamo.idPrestamo, observacion);
  }

  private armarListaMotivosRechazo() {
    if (this.lsMotivosRechazoPrestamo.length) {
      let idSeguimiento = this.lsMotivosRechazoPrestamo[this.lsMotivosRechazoPrestamo.length - 1].idSeguimientoPrestamo;
      this.lsMotivosRechazoPrestamo.forEach((motivo) => {
        if (motivo.idSeguimientoPrestamo == idSeguimiento) {
          let motivoRechazo = new MotivoRechazo();
          motivoRechazo.id = motivo.idMotivo;
          motivoRechazo.descripcion = motivo.descripcion;
          motivoRechazo.observaciones = motivo.observaciones;
          this.lsMotivosRechazo.push(motivoRechazo);
        }
      });
    }
  }

  private crearPersonaParaReactivacion(idSexo: string, numeroDocumento: string, idNumero: string, codigoPais: string): Persona {
    let persona = new Persona();
    persona.sexoId = idSexo;
    persona.nroDocumento = numeroDocumento;
    persona.idNumero = Number(idNumero);
    persona.codigoPais = codigoPais;
    return persona;
  }
}
