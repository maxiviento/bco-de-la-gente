import { Component, OnInit, ViewChild } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormulariosService } from '../../shared/formularios.service';
import { CustomValidators, isEmpty } from '../../../shared/forms/custom-validators';
import { FuenteFinanciamiento } from '../../shared/modelo/fuente.financiamiento';
import { InversionEmprendimientoModel } from '../../shared/modelo/inversion-emprendimiento.model';
import { NecesidadInversion } from '../../shared/modelo/necesidad-inversion.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { Emprendimiento } from '../../shared/modelo/emprendimiento.model';
import { ApartadoInversionesEmprendimientoComponent } from '../../shared/apartado-inversiones-emprendimiento/apartado-inversiones-emprendimiento.component';

@Component({
  selector: 'bg-necesidades-inversion',
  templateUrl: './necesidades-inversion.component.html',
  styleUrls: ['./necesidades-inversion.component.scss']
})

export class NecesidadesInversionComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public fuentesFinanciamiento: FuenteFinanciamiento[] = [];
  public necesidadInversion: NecesidadInversion = new NecesidadInversion();
  public habilitarOtraFuente: boolean = false;
  public detallesParaEliminar: number[] = [];
  public emprendimiento: Emprendimiento = new Emprendimiento();

  @ViewChild(ApartadoInversionesEmprendimientoComponent)
  public apartadoInversiones: ApartadoInversionesEmprendimientoComponent;

  constructor(private formularioService: FormulariosService,
              private notificacionService: NotificacionService) {
    super();
  }

  ngOnInit(): void {
    if (this.formulario.necesidadInversion) {
      this.necesidadInversion = this.formulario.necesidadInversion;
    }
    if(this.formulario.datosEmprendimiento){
      this.emprendimiento = this.formulario.datosEmprendimiento;
    }
    this.crearForm();
    this.obtenerFuentesFinanciamiento();
  }

  public crearForm(): void {

    let montoOtrasFuentes = new FormControl(
      this.necesidadInversion.montoOtrasFuentes,
      Validators.compose([Validators.maxLength(8),CustomValidators.number]));

    let montoCapitalPropio = new FormControl(
      this.necesidadInversion.montoCapitalPropio,
      Validators.compose([Validators.maxLength(8),CustomValidators.number]));

    let montoMicroprestamo = new FormControl(
      this.necesidadInversion.montoMicroprestamo,
      Validators.compose([Validators.maxLength(8),CustomValidators.number]));

    let idFuenteFinanciamiento = new FormControl(this.necesidadInversion.idFuenteFinanciamiento);
    this.habilitarOtraFuente = !isEmpty(this.necesidadInversion) && !isEmpty(this.necesidadInversion.idFuenteFinanciamiento);

    idFuenteFinanciamiento.valueChanges.subscribe((value => {
      if (value == "null") {
        montoOtrasFuentes.setValue("");
        montoOtrasFuentes.setValidators(Validators.compose([Validators.maxLength(8), CustomValidators.number]));
        montoOtrasFuentes.updateValueAndValidity();
        this.habilitarOtraFuente = false;
      }
      else {
        this.habilitarOtraFuente = true;
      }
    }));

    if(!this.editable){
      idFuenteFinanciamiento.disable();
      montoCapitalPropio.disable();
      montoMicroprestamo.disable();
      montoOtrasFuentes.disable();
    }

    this.form = new FormGroup({
      idFuenteFinanciamiento: idFuenteFinanciamiento,
      montoCapitalPropio: montoCapitalPropio,
      montoMicroprestamo: montoMicroprestamo,
      montoOtrasFuentes: montoOtrasFuentes
    })
  }

  public obtenerFuentesFinanciamiento() {
    this.formularioService.obtenerFuentesFinanciamiento()
      .subscribe((resultados) => {
        this.fuentesFinanciamiento = resultados;
      })
  }

  public completarLista(listaInversiones) {
    this.necesidadInversion.inversionesRealizadas = listaInversiones;
  }

  public completarDetallesParaBorra(listaIds) {
    this.detallesParaEliminar = listaIds;
  }

  public obtenerDatosCargados() {
    let formModel = this.form.value;
    this.necesidadInversion.montoMicroprestamo = formModel.montoMicroprestamo;
    this.necesidadInversion.montoCapitalPropio = formModel.montoCapitalPropio;
    this.necesidadInversion.montoOtrasFuentes = formModel.montoOtrasFuentes;
    this.necesidadInversion.idFuenteFinanciamiento = formModel.idFuenteFinanciamiento == 'null' ? null : formModel.idFuenteFinanciamiento;
  }

  public actualizarDatos() {
    this.obtenerDatosCargados();
    this.formularioService.eliminarDetallesInversion(this.detallesParaEliminar).subscribe(() => {
      },
      (errores) => {
        this.notificacionService.informar(errores, true);
      },
      () => {
        this.formularioService.actualizarNecesidadInversion(this.formulario.id, this.necesidadInversion)
          .subscribe(() => {
              this.formulario.necesidadInversion = this.necesidadInversion;
            },
            (errores) => {
              this.notificacionService.informar(errores, true);
            },
            () => {
              this.necesidadInversion.inversionesRealizadas = [];
              this.detallesParaEliminar = [];
            });
      });
  }

  esValido(): boolean {
    if (!this.editable) {
      return true;
    } else {
      return this.form.valid && !isEmpty(this.formulario) && !isEmpty(this.formulario.datosEmprendimiento.id);
    }
  }

  inicializarDeNuevo(): boolean {
    return false;
  }

  public sumarMontos(): number {
    if(!this.form) return 0;
    let formValue = this.form.value;
    return Number(formValue.montoCapitalPropio) + Number(formValue.montoMicroprestamo) + Number(formValue.montoOtrasFuentes);
  }

  public diferenciaTotales(): number {
    return this.sumarMontos() - this.totalInversiones();
  }

  public totalInversiones(): number {
    if(!this.apartadoInversiones) return 0;
    return this.apartadoInversiones.sumarPrecios();
  }
}
