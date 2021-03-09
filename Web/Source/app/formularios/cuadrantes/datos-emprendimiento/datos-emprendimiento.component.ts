import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FormulariosService } from '../../shared/formularios.service';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { EmprendimientoService } from '../../shared/emprendimiento.service';
import { TipoProyecto } from '../../shared/modelo/tipo-proyecto.model';
import { SectorDesarrollo } from '../../shared/modelo/sector-desarrollo.model';
import { TipoInmueble } from '../../shared/modelo/tipo-inmueble.model';
import { Emprendimiento } from '../../shared/modelo/emprendimiento.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { CustomValidators, isEmpty } from '../../../shared/forms/custom-validators';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { NgbDatepickerConfig, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { GrupoUnicoService } from '../../../grupo-unico/shared-grupo-unico/grupo-unico.service';
import AccionGrupoUnico from '../../../grupo-unico/shared-grupo-unico/accion-grupo-unico.enum';
import { ModalDomicilioComponent } from './modal-domicilio/modal-domicilio.component';
import { DomicilioService } from '../../../grupo-unico/shared-grupo-unico/domicilio.service';
import { Actividad } from '../../shared/modelo/actividad.model';
import { Rubro } from '../../shared/modelo/rubro.model';
import { AbstractControl } from '@angular/forms/src/model';
import { ComboInstituciones } from '../../shared/modelo/combo-instituciones.model';
import { FormUtils } from '../../../shared/forms/forms-utils';
import { DateUtils } from '../../../shared/date-utils';
import { PersonaService } from '../persona/persona.service';

@Component({
  selector: 'bg-datos-emprendimiento',
  templateUrl: './datos-emprendimiento.component.html',
  styleUrls: ['./datos-emprendimiento.component.scss'],
})

export class DatosEmprendimientoComponent extends CuadranteFormulario implements OnInit, OnDestroy {
  public form: FormGroup;
  public tiposProyecto: TipoProyecto [] = [];
  public tiposInmueble: TipoInmueble [] = [];
  public sectoresDesarrollo: SectorDesarrollo [] = [];
  public actividades: Actividad [] = [];
  public rubros: Rubro [] = [];
  public instituciones: ComboInstituciones [] = [];
  public emprendimiento: Emprendimiento = new Emprendimiento();
  private dni: string;
  private sexoId: string;
  private pais: string;
  private url: string;
  public mostrarDatosDomicilio: boolean = false;
  private idVinculo: number;

  constructor(private fb: FormBuilder,
              private emprendimientoService: EmprendimientoService,
              private formularioService: FormulariosService,
              private notificacionService: NotificacionService,
              private dateConfig: NgbDatepickerConfig,
              private grupoUnicoService: GrupoUnicoService,
              private domicilioService: DomicilioService,
              private modalService: NgbModal,
              private personaService: PersonaService) {
    super();
  }

  public ngOnInit(): void {

    if (this.formulario.datosEmprendimiento) {
      this.emprendimiento = this.formulario.datosEmprendimiento;
    }
    this.crearForm();
    this.consultarDatosGenerales();
    this.cargarCombos();
    if (!this.editable) {
      this.form.disable();
    }
  }

  public ngOnDestroy(): void {
    DateUtils.removeMaxDateDP(this.dateConfig);
  }

  private crearForm(): void {
    let tieneExperienciaFc = new FormControl(this.emprendimiento.tieneExperiencia);
    let tiempoExperienciaFc = new FormControl(this.emprendimiento.tiempoExperiencia, Validators.compose([Validators.maxLength(50),
      CustomValidators.validTextAndNumbers]));
    tieneExperienciaFc.valueChanges.subscribe((value) => {
      if (!value) {
        tiempoExperienciaFc.setValue('');
      }
    });
    let pidioCreditoFc = new FormControl(this.emprendimiento.pidioCredito);
    let creditoFueOtorgadoFc = new FormControl(this.emprendimiento.creditoFueOtorgado);
    let institucionSolicitanteFc = new FormControl(this.emprendimiento.institucionSolicitante);
    pidioCreditoFc.valueChanges.subscribe((value) => {
      if (!value) {
        creditoFueOtorgadoFc.setValue(false);
        institucionSolicitanteFc.setValue('');
        institucionSolicitanteFc.clearValidators();
        institucionSolicitanteFc.updateValueAndValidity();
      } else {
        institucionSolicitanteFc.setValidators(Validators.compose([Validators.required, Validators.maxLength(50)]));
        institucionSolicitanteFc.updateValueAndValidity();
      }
    });

    let fechaActivacionFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.emprendimiento.fechaActivo));
    let tipoProyectoFc = new FormControl(this.emprendimiento.idTipoProyecto);
    tipoProyectoFc.valueChanges
      .distinctUntilChanged()
      .subscribe((value) => {
        if (value === 2) { // 2 = EN MARCHA
          DateUtils.setMaxDateDP(new Date(), this.dateConfig);
          if (fechaActivacionFc.value) {
            let hoy = new Date();
            // Si la fecha de activación es mayor a hoy se la resetea
            if ((DateUtils.compareDate(NgbUtils.obtenerDate(fechaActivacionFc.value), new Date(hoy.getFullYear(), hoy.getMonth(), hoy.getDate())) === 1) && this.editable) {
              fechaActivacionFc.patchValue(null);
            }
          }
        } else {
          DateUtils.removeMaxDateDP(this.dateConfig);
        }
      });

    this.form = this.fb.group({
      calle: [this.emprendimiento.calle],
      nroCalle: [this.emprendimiento.nroCalle],
      torre: [this.emprendimiento.torre],
      nroPiso: [this.emprendimiento.nroPiso],
      nroDpto: [this.emprendimiento.nroDpto],
      manzana: [this.emprendimiento.manzana],
      casa: [this.emprendimiento.casa],
      barrio: [this.emprendimiento.barrio],
      localidad: [this.emprendimiento.localidad],
      departamento: [this.emprendimiento.departamento],
      codPostal: [this.emprendimiento.codPostal],
      nroCodArea: [this.emprendimiento.nroCodArea, Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(5), CustomValidators.codArea, CustomValidators.number])],
      nroTelefono: [this.emprendimiento.nroTelefono, Validators.compose([Validators.required, Validators.minLength(0), Validators.maxLength(9), CustomValidators.nroTelefono, CustomValidators.number])],
      email: [this.emprendimiento.email, Validators.compose([Validators.maxLength(50), Validators.minLength(7), CustomValidators.email])],
      tipoInmueble: [this.emprendimiento.idTipoInmueble, Validators.required],
      actividad: [this.emprendimiento.idActividad, Validators.required],
      rubro: [this.emprendimiento.idRubro],
      tipoProyecto: tipoProyectoFc,
      fechaActivacion: fechaActivacionFc,
      sectorDesarrollo: [this.emprendimiento.idSectorDesarrollo],
      tieneExperiencia: tieneExperienciaFc,
      tiempoExperiencia: tiempoExperienciaFc,
      hizoCursos: [this.emprendimiento.hizoCursos],
      cursoInteres: [this.emprendimiento.cursoInteres],
      pidioCredito: pidioCreditoFc,
      creditoFueOtorgado: creditoFueOtorgadoFc,
      institucionSolicitante: institucionSolicitanteFc
    });

    if (this.emprendimiento.calle) {
      this.mostrarDatosDomicilio = true;
    }

    this.cargarActividades();
    if (this.editable) {
      (this.form.get('rubro') as FormControl).valueChanges
        .subscribe(() => {
          this.cargarActividades();
          (this.form.get('actividad') as FormControl).setValue(null);
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    }
  }

  private consultarDatosGenerales(): void {
    let promesaInmuebles = new Promise((resolve) =>
      this.emprendimientoService.consultarTiposInmueble().subscribe((res) => {
        this.tiposInmueble = res;
        return resolve();
      }));
    let promesaProyectos = new Promise((resolve) =>
      this.emprendimientoService.consultarTiposProyecto().subscribe((res) => {
        this.tiposProyecto = res;
        return resolve();
      }));
    let promesaSectores = new Promise((resolve) =>
      this.emprendimientoService.consultarSectoresDesarrollo().subscribe((res) => {
        this.sectoresDesarrollo = res;
        return resolve();
      }));

    let promesaInstituciones = new Promise((resolve) =>
      this.emprendimientoService.consultarInstituciones().subscribe((res) => {
        this.instituciones = res;
        return resolve();
      }));

    if (this.formulario.solicitante) {
      if (this.formulario.solicitante.nroDocumento) {
        this.dni = this.formulario.solicitante.nroDocumento;
        this.sexoId = this.formulario.solicitante.sexoId;
        this.pais = this.formulario.solicitante.codigoPais;
        this.grupoUnicoService.obtenerUrlAutorizada(AccionGrupoUnico.DOMICILIO_ALTA, this.dni, this.sexoId, this.pais)
          .subscribe((url) => {
            this.url = url;
          });
      }
    }

    Promise.all([promesaInmuebles, promesaProyectos, promesaSectores, promesaInstituciones]).then(() => {
      this.obtenerDomicilioSolicitante();
      this.asignarTelefonoYMailSolicitante();
    }).catch((error) =>
      this.notificacionService.informar([error], true));
  }

  private cargarActividades(): void {
    let rubro = isEmpty(this.form.get('rubro').value) ? -1 : this.form.get('rubro').value;
    this.emprendimientoService.consultarActividades(rubro)
      .subscribe((actividades) => {
        this.actividades = actividades;
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  private cargarCombos() {
    this.emprendimientoService
      .consultarRubros()
      .subscribe((rubros) => this.rubros = rubros,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  public crearDomicilio(): void {
    let options: NgbModalOptions = {
      windowClass: 'modal-xl',
      backdrop: 'static',
      /*keyboard: false*/
    };
    const modalRef = this.modalService.open(ModalDomicilioComponent, options);
    modalRef.componentInstance.url = this.url;
    modalRef.result.then((res) => {
      if (res && res.confirmar) {
        this.consultarDomicilio();
      }
    });
  }

  private consultarDomicilio(): void {
    this.domicilioService.consultarDomicilio(AccionGrupoUnico.DOMICILIO_CONSULTA_GENERADO, this.dni, this.sexoId, this.pais)
      .subscribe((resultado) => {
        this.setDatosDomicilio(resultado);
      });
  }

  private obtenerDatosEmprendimiento(): Emprendimiento {
    let formModel = this.form.value;
    let emprendimiento = new Emprendimiento();
    emprendimiento.idVinculo = this.idVinculo;
    emprendimiento.calle = formModel.calle;
    emprendimiento.nroCalle = formModel.nroCalle;
    emprendimiento.id = this.emprendimiento.id;
    emprendimiento.email = formModel.email;
    emprendimiento.nroCodArea = formModel.nroCodArea ? formModel.nroCodArea : -1;
    emprendimiento.nroTelefono = formModel.nroTelefono ? formModel.nroTelefono : -1;
    emprendimiento.idTipoInmueble = formModel.tipoInmueble ? formModel.tipoInmueble : -1;
    emprendimiento.idTipoProyecto = formModel.tipoProyecto ? formModel.tipoProyecto : -1;
    emprendimiento.idSectorDesarrollo = formModel.sectorDesarrollo ? formModel.sectorDesarrollo : -1;
    emprendimiento.fechaActivo = formModel.fechaActivacion ? NgbUtils.obtenerDate(formModel.fechaActivacion) : null;
    emprendimiento.tieneExperiencia = formModel.tieneExperiencia || false;
    emprendimiento.tiempoExperiencia = formModel.tiempoExperiencia;
    if (formModel.actividad) {
      let actividad = this.actividades.find((x) => x.id === formModel.actividad);
      emprendimiento.idActividad = actividad ? actividad.id : -1;
      emprendimiento.fechaInicioActividad = actividad ? actividad.fechaInicio : null;
    }
    if (formModel.rubro) {
      let rubro = this.rubros.find((x) => x.id === formModel.rubro);
      emprendimiento.idRubro = rubro ? rubro.id : -1;
    }
    emprendimiento.hizoCursos = formModel.hizoCursos || false;
    emprendimiento.cursoInteres = formModel.cursoInteres;
    emprendimiento.pidioCredito = formModel.pidioCredito || false;
    emprendimiento.creditoFueOtorgado = formModel.creditoFueOtorgado;
    emprendimiento.institucionSolicitante = formModel.institucionSolicitante;
    return emprendimiento;
  }

  public actualizarDatos() {
    let emprendimiento = this.obtenerDatosEmprendimiento();
    this.formularioService.actualizarEmprendimiento(this.formulario.idAgrupamiento, emprendimiento)
      .subscribe((id) => {
        emprendimiento.id = id;
        this.emprendimiento.id = id;
        this.setDatosEmprendimientoFormulario(emprendimiento);
      });
  }

  private setDatosEmprendimientoFormulario(emp: Emprendimiento): void {
    if (emp) {
      if (this.formulario && this.formulario.datosEmprendimiento) {
        this.formulario.datosEmprendimiento.id = emp.id;
        this.formulario.datosEmprendimiento.idVinculo = emp.idVinculo;
        this.formulario.datosEmprendimiento.calle = emp.calle;
        this.formulario.datosEmprendimiento.nroCalle = emp.nroCalle;
        this.formulario.datosEmprendimiento.email = emp.email;
        this.formulario.datosEmprendimiento.nroCodArea = emp.nroCodArea;
        this.formulario.datosEmprendimiento.nroTelefono = emp.nroTelefono;
        this.formulario.datosEmprendimiento.idTipoInmueble = emp.idTipoInmueble;
        this.formulario.datosEmprendimiento.idTipoProyecto = emp.idTipoProyecto;
        this.formulario.datosEmprendimiento.idSectorDesarrollo = emp.idSectorDesarrollo;
        this.formulario.datosEmprendimiento.fechaActivo = emp.fechaActivo;
        this.formulario.datosEmprendimiento.tieneExperiencia = emp.tieneExperiencia;
        this.formulario.datosEmprendimiento.tiempoExperiencia = emp.tiempoExperiencia;
        this.formulario.datosEmprendimiento.idRubro = emp.idRubro;
        this.formulario.datosEmprendimiento.idActividad = emp.idActividad;
        this.formulario.datosEmprendimiento.fechaInicioActividad = emp.fechaInicioActividad;
        this.formulario.datosEmprendimiento.hizoCursos = emp.hizoCursos;
        this.formulario.datosEmprendimiento.cursoInteres = emp.cursoInteres;
        this.formulario.datosEmprendimiento.pidioCredito = emp.pidioCredito;
        this.formulario.datosEmprendimiento.creditoFueOtorgado = emp.creditoFueOtorgado;
        this.formulario.datosEmprendimiento.institucionSolicitante = emp.institucionSolicitante;
      } else {
        this.formulario.datosEmprendimiento = emp;
      }
    }
  }

  public esValido(): boolean {
    if (!this.editable) {
      return true;
    } else {
      return this.form.valid;
    }
  }

  public inicializarDeNuevo(): boolean {
    this.personaService.consultarPersona(this.formulario.solicitante).subscribe(
      (resultado) => {
        if (resultado.domicilioIdVin) {
          this.formulario.solicitante.domicilioIdVin = resultado.domicilioIdVin;
          this.obtenerDomicilioSolicitante();
        }
      });
    return false;
  }

  public radioChecked(fc: AbstractControl, radioValue: number) {
    return fc.value && radioValue == fc.value;
  }

  private setDatosDomicilio(res: any): void {
    if (res) {
      this.mostrarDatosDomicilio = true;
      this.idVinculo = res.idVin;
      this.form.get('calle').setValue(res.calle ? res.calle.nombre : '', {eventEmit: false});
      this.form.get('nroCalle').setValue(res.altura, {eventEmit: false});
      this.form.get('nroPiso').setValue(res.piso, {eventEmit: false});
      this.form.get('nroDpto').setValue(res.dpto, {eventEmit: false});
      this.form.get('manzana').setValue(res.manzana, {eventEmit: false});
      this.form.get('casa').setValue(res.lote, {eventEmit: false});
      this.form.get('torre').setValue(res.torre, {eventEmit: false});
      this.form.get('barrio').setValue(res.barrio ? res.barrio.nombre : '', {eventEmit: false});
      this.form.get('localidad').setValue(res.localidad ? res.localidad.nombre : '', {eventEmit: false});
      this.form.get('departamento').setValue(res.departamento ? res.departamento.nombre : '', {eventEmit: false});
      this.form.get('codPostal').setValue(res.codigoPostal, {eventEmit: false});
    } else {
      this.mostrarDatosDomicilio = false;
    }
  }

  private obtenerDomicilioSolicitante(): void {
    if (!this.emprendimiento.id && !this.idVinculo) {
      if (this.formulario && this.formulario.solicitante && this.formulario.solicitante.domicilioIdVin) {
        this.domicilioService.consultarDomicilioPorIdVin(AccionGrupoUnico.DOMICILIO_CONSULTA_CON_CARACTERISTICAS, this.formulario.solicitante.domicilioIdVin)
          .subscribe((resultado) => {
            this.setDatosDomicilio(resultado);
          });
      }
    }
  }

  private asignarTelefonoYMailSolicitante(): void {
    if (!this.emprendimiento.id && !this.emprendimiento.nroTelefono && !this.emprendimiento.email) {
      if (this.formulario && this.formulario.solicitante) {
        if (this.formulario.solicitante.codigoArea && this.formulario.solicitante.telefono) {
          this.form.get('nroCodArea').setValue(this.formulario.solicitante.codigoArea, {eventEmit: false});
          this.form.get('nroTelefono').setValue(this.formulario.solicitante.telefono, {eventEmit: false});
        } else {
          this.form.get('nroCodArea').setValue(this.formulario.solicitante.codigoAreaCelular, {eventEmit: false});
          this.form.get('nroTelefono').setValue(this.formulario.solicitante.celular, {eventEmit: false});
        }
        this.form.get('email').setValue(this.formulario.solicitante.email, {eventEmit: false});
        FormUtils.validate(this.form);
      }
    }
  }

}
