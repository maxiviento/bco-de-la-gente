import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { PagosService } from '../../shared/pagos.service';
import { LocalidadComboServicio } from '../../../formularios/shared/localidad.service';
import { OrigenService } from '../../../formularios/shared/origen-prestamo.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { Localidad } from '../../../formularios/shared/modelo/localidad.model';
import { BandejaArmarLoteSuafConsulta } from '../../shared/modelo/bandeja-armar-lote-suaf-consulta.model';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { Observable, Subject } from 'rxjs/Rx';
import { BandejaArmarLoteSuafResultado } from '../../shared/modelo/bandeja-armar-lote-suaf-resultado.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { OrigenPrestamo } from '../../../formularios/shared/modelo/origen-prestamo.model';
import { Departamento } from '../../../formularios/shared/modelo/departamento.model';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { LineaCombo } from '../../../formularios/shared/modelo/linea-combo.model';
import { RegistrarLoteSuafComando } from '../../shared/modelo/registrar-lote-suaf-comando.model';
import { CargaDevengadoComandoModel } from '../../shared/modelo/carga-devengado-comando.model';
import { LoteCombo } from '../../shared/modelo/lote-combo.model';
import { BusquedaPorPersonaComponent } from '../../../shared/componentes/busqueda-por-persona/busqueda-por-persona.component';

@Component({
  selector: 'bg-bandeja-formularios-suaf',
  templateUrl: './bandeja-formularios-suaf.component.html',
  styleUrls: ['./bandeja-formularios-suaf.component.scss'],
})

export class BandejaFormulariosSuafComponent implements OnInit {
  @Input() public esCargaManual: boolean;
  @Input() public esArmarLote: boolean;
  @Input() public esVerLote: boolean;
  public consulta: BandejaArmarLoteSuafConsulta;
  public bandejaResultados: BandejaArmarLoteSuafResultado[] = [];
  public form: FormGroup;
  public CBLocalidad: Localidad[] = [];
  public CBOrigen: OrigenPrestamo[] = [];
  public CBDevengado: OrigenPrestamo[] = [];
  public departamentos: Departamento[] = [];
  public CBLinea: LineaCombo[] = [];
  public CBLote: LoteCombo[];
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public formFormulariosSuaf: FormGroup;
  public prestamosParaConformarLote: BandejaArmarLoteSuafResultado[] = [];
  public idsPrestamosSeleccionados: number[] = [];
  public all: BandejaArmarLoteSuafResultado[] = [];
  public formNombreLote: FormGroup;
  public seleccionarTodosCheckeado: boolean = false;
  public cantPrestamosActual = 0;
  public cantFormulariosActual = 0;
  public bloquearGenerar: boolean = false;

  @ViewChild(BusquedaPorPersonaComponent)
  public componentePersona: BusquedaPorPersonaComponent;

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private localidadesService: LocalidadComboServicio,
              private origenesService: OrigenService,
              private notificacionService: NotificacionService,
              private router: Router,
              private formulariosService: FormulariosService,
              private lineaService: LineaService,
              private route: ActivatedRoute) {
    if (!this.consulta) {
      this.consulta = new BandejaArmarLoteSuafConsulta();
    }
  }

  public ngOnInit(): void {
    this.consulta.fechaDesde = new Date(Date.now());
    this.consulta.fechaHasta = new Date(Date.now());

    this.crearForm();
    this.crearFormNombreLote();
    this.cargarCombos();
    this.configurarPaginacion();

    if (this.esVerLote) {
      this.route.params.subscribe((params: Params) => {
        this.consulta.fechaDesde = new Date(Date.now());
        this.consulta.fechaHasta = new Date(Date.now());
        this.form.get('idLoteSuaf').patchValue(+params['id']);
        this.consultar(true);
      });
    }
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaDesde),
      CustomValidators.maxDate(new Date()));
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(this.consulta.fechaHasta),
      CustomValidators.minDate(new Date()));
    fechaDesdeFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaHastaFc.clearValidators();
        let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec ? minDate = new Date(fechaDesdeMilisec) : minDate = new Date(fechaActualMilisec);
        fechaHastaFc.setValidators(Validators.compose([CustomValidators.minDate(minDate),
          CustomValidators.maxDate(new Date())]));
        fechaHastaFc.updateValueAndValidity();
      }
    });
    fechaHastaFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaDesdeFc.clearValidators();
        let fechaHastaMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let maxDate;
        fechaHastaMilisec <= fechaActualMilisec ? maxDate = new Date(fechaHastaMilisec) : maxDate = new Date(fechaActualMilisec);
        fechaDesdeFc.setValidators(CustomValidators.maxDate(maxDate));
        fechaDesdeFc.updateValueAndValidity();
      }
    });

    this.form = this.fb.group({
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      localidad: [this.consulta.idLocalidad],
      departamento: [this.consulta.idDepartamento],
      nroPrestamoChecklist: [this.consulta.nroPrestamoChecklist, Validators.compose(
        [
          CustomValidators.number,
          Validators.maxLength(8)
        ]
      )],
      nroFormulario: [this.consulta.nroFormulario, Validators.compose(
        [
          CustomValidators.number,
          Validators.maxLength(14)
        ]
      )],
      nombre: [this.consulta.nombre],
      apellido: [this.consulta.apellido],
      origen: [this.consulta.idOrigen],
      idLoteSuaf: [this.consulta.idLoteSuaf],
      devengado: [this.consulta.devengado],
      idLinea: [this.consulta.idLinea],
    });
    this.cargarLocalidades();
    (this.form.get('departamento') as FormControl).valueChanges
      .subscribe(() => {
        this.cargarLocalidades();
        (this.form.get('localidad') as FormControl).setValue(null);
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades;
  }

  private cargarLocalidades(): void {
    if (this.form.get('departamento').value &&
      this.form.get('departamento').value !== 'null') {
      this.localidadesService.consultarLocalidades(this.form.get('departamento').value)
        .subscribe((localidades) => {
          this.CBLocalidad = localidades;
          if (this.CBLocalidad.length) {
            (this.form.get('localidad') as FormControl).enable();
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      this.CBLocalidad = [];
      (this.form.get('localidad') as FormControl).disable();
    }
  }

  private cargarCombos() {
    this.formulariosService
      .consultarDepartamentos()
      .subscribe((departamentos) => this.departamentos = departamentos,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => this.CBOrigen = origenes,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.lineaService
      .consultarLineasParaCombo()
      .subscribe((lineas) => this.CBLinea = lineas,
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
    this.pagosService.obtenerComboLotesSuaf()
      .subscribe((lotes) => {
        this.CBLote = lotes;
      });
    this.CBDevengado.push(new OrigenPrestamo(3, 'Con devengado'));
    this.CBDevengado.push(new OrigenPrestamo(2, 'Sin devengado'));
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.prepararConsulta();
        let filtros = this.consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService
          .consultarBandejaArmarLoteSuaf(filtros);
      })
      .share();
    (<Observable<BandejaArmarLoteSuafResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormFormulariosParaSuaf();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public consultar(esNuevaConsulta: boolean, pagina?: number) {

    if (esNuevaConsulta) {
      this.limpiarDatosPrevios();
    }
    this.prepararConsulta();
    if (this.componentePersona && !this.componentePersona.documentoIngresado()) {
      if ((this.consulta.fechaDesde == null
        || this.consulta.fechaHasta == null)) {
        this.notificacionService.informar(['Debe ingresar fecha desde y fecha hasta.']);
      } else {
        if (this.consulta.fechaHasta < this.consulta.fechaHasta) {
          this.notificacionService.informar(['La fecha desde no puede ser posterior a la fecha hasta.']);
        } else {
          this.paginaModificada.next(pagina);
        }
      }
    } else {
      this.paginaModificada.next(pagina);
    }
  }

  private prepararConsulta() {
    let formModel = this.form.value;

    this.consulta.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);
    this.consulta.fechaHasta = NgbUtils.obtenerDate(formModel.fechaHasta);
    this.consulta.nroPrestamoChecklist = formModel.nroPrestamoChecklist;
    this.consulta.idDepartamento = formModel.departamento;
    this.consulta.idLocalidad = formModel.localidad;
    this.consulta.idOrigen = formModel.origen;
    this.consulta.nombre = formModel.nombre;
    this.consulta.apellido = formModel.apellido;
    this.consulta.idLinea = formModel.idLinea;
    this.consulta.devengado = formModel.devengado;
    this.consulta.idLoteSuaf = formModel.idLoteSuaf;
    this.consulta.nroFormulario = formModel.nroFormulario;
    this.consulta.esCargaDevengado = this.esCargaManual;

    this.consulta.departamentoIds = this.departamentoIds;
    this.consulta.localidadIds = this.localidadIds;

    if (this.componentePersona) {
      let consultaPersona = this.componentePersona.prepararConsulta();
      this.consulta.tipoPersona = consultaPersona.tipoPersona;
      this.consulta.cuil = consultaPersona.cuil;
      this.consulta.nombre = consultaPersona.nombre;
      this.consulta.apellido = consultaPersona.apellido;
      this.consulta.nroDocumento = consultaPersona.dni;
    }
  }

  public get formulariosFormArray(): FormArray {
    return this.formFormulariosSuaf.get('formularios') as FormArray;
  }

  private crearFormNombreLote() {
    this.formNombreLote = this.fb.group({
      nombre: ['', Validators.compose([
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(80)
      ])],
    });
  }

  public generarLote() {
    if (this.validacionesParaGenerarPrestamo()) {
      this.bloquearGenerar = true;

      let comando = new RegistrarLoteSuafComando();
      let formModel = this.formNombreLote.value;

      comando.idPrestamosSeleccionados = this.idsPrestamosSeleccionados;
      comando.nombreLote = formModel.nombre;

      this.pagosService.registrarLoteSuaf(comando)
        .subscribe((res) => {
          this.notificacionService.informar(Array.of('Lote creado con éxito'), false).result.then((x) => {
            this.router.navigate(['/bandeja-suaf']);
          });
        }, (errores) => {
          this.bloquearGenerar = false;
          this.notificacionService.informar(errores, true);
        });
    }
  }

  private validacionesParaGenerarPrestamo(): boolean {
    if (this.idsPrestamosSeleccionados.length === 0 && !this.seleccionarTodosCheckeado) {
      this.notificacionService.informar(['Debe seleccionar algun formulario.']);
      return false;
    } else {
      if (this.formNombreLote.value.nombre === '' || (this.formNombreLote.value.nombre.replace(/[^a-zA-Z]/g, '').length === 0)) {
        this.notificacionService.informar(['Debe ingresar un nombre para el lote suaf.']);
        return false;
      } else {
        return true;
      }
    }
  }

  public clickEnSeleccionarTodos(checked: boolean) {
    this.seleccionarTodosCheckeado = checked;
    this.prestamosParaConformarLote = [];
    this.idsPrestamosSeleccionados = [];
    this.contarCantidadFormulariosYPrestamos();

    if (this.seleccionarTodosCheckeado) {
      this.prepararConsulta();
      let filtros = this.consulta;
      this.pagosService
        .consultarBandejaArmarLoteSuafSeleccionarTodos(filtros)
        .subscribe((bandeja) => {
          this.verificarFormularios(bandeja.elementos);
          let ids = new Set<number>();
          bandeja.elementos.forEach((prestamo) => {
            ids.add(prestamo.idPrestamo);
          });
          ids.forEach((a) => this.idsPrestamosSeleccionados.push(a));

          this.contarCantidadFormulariosYPrestamos();
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    }
  }

  private crearFormFormulariosParaSuaf() {
    this.formFormulariosSuaf = this.fb.group({
      formularios: this.fb.array((this.bandejaResultados || []).map((formulario) =>
        this.fb.group({
          id: [formulario.id],
          nroFormulario: [formulario.nroFormulario],
          idPrestamo: [formulario.idPrestamo],
          linea: [formulario.linea],
          departamento: [formulario.departamento],
          localidad: [formulario.localidad],
          origen: [formulario.origen],
          nroPrestamo: [formulario.nroPrestamo],
          montoOtorgado: [formulario.montoOtorgado],
          fechaPedido: [formulario.fechaPedido],
          devengado: [formulario.devengado],
          devengadoNuevo: [
            formulario.devengadoNuevo,
            Validators.compose([
              Validators.required,
              Validators.maxLength(20),
              CustomValidators.devengado])
          ],
          fechaDevengado: [formulario.fechaDevengado],
          apellidoYNombre: [formulario.apellidoYNombre],
          seleccionado: [formulario.seleccionado],
          idFormulario: [formulario.id]
        })
      )),
    });
  }

  public agregarPrestamoParaAgrupar(nroPrestamo: number): void {
    let filtros = Object.assign({}, this.consulta);
    filtros.nroPrestamoChecklist = nroPrestamo + '';
    filtros.numeroPagina = 0;
    this.pagosService.consultarBandejaArmarLoteSuaf(filtros).subscribe(
      (pagina: Pagina<BandejaArmarLoteSuafResultado>) => {
        if (pagina.elementos) {
          this.verificarFormularios(pagina.elementos, nroPrestamo);
        }
      }
    );
  }

  public quitarFormulariosParaAgrupar(idPrestamo: number, toolTipQuitar: any): void {
    toolTipQuitar.close();
    this.prestamosParaConformarLote = this.prestamosParaConformarLote.filter(
      (x) => x.idPrestamo !== idPrestamo);
    this.idsPrestamosSeleccionados = this.idsPrestamosSeleccionados.filter(
      (x) => x !== idPrestamo);
    this.contarCantidadFormulariosYPrestamos();
  }

  private verificarFormularios(formularios: BandejaArmarLoteSuafResultado[],
                               idPrestamo?: number) {
    this.prestamosParaConformarLote.push(...formularios);
    if (idPrestamo) {
      this.idsPrestamosSeleccionados.push(formularios[0].idPrestamo);
    }
    this.contarCantidadFormulariosYPrestamos();
  }

  public estaSeleccionado(idPrestamo: number): boolean {
    const esSeleccionado = this.idsPrestamosSeleccionados.find(
      (id) => id === idPrestamo);
    return esSeleccionado !== undefined;
  }

  public contarCantidadFormulariosYPrestamos(): void {
    this.cantFormulariosActual = this.prestamosParaConformarLote.length;
    this.cantPrestamosActual = this.contarPrestamos();
  }

  private contarPrestamos(): number {
    let idsPrestamos = [];
    this.prestamosParaConformarLote.forEach((formSuaf) => {
      idsPrestamos.push(formSuaf.idPrestamo);
    });
    return new Set(idsPrestamos).size;
  }

  private limpiarDatosPrevios(): void {
    this.seleccionarTodosCheckeado = false;
    this.idsPrestamosSeleccionados = [];
    this.prestamosParaConformarLote = [];
    this.cantFormulariosActual = 0;
    this.cantPrestamosActual = 0;
  }

  public validarConsulta(): boolean {
    if (!this.form) return true;
    if (this.componentePersona) {
      if (this.componentePersona.documentoIngresado()) return false;
      return !(this.form.value.fechaDesde && this.form.value.fechaHasta);
    } else {
      return !(this.form.value.fechaDesde && this.form.value.fechaHasta);
    }
  }

  public cambioDevengado(resultado: FormGroup) {
    if (resultado.valid) {
      let comando = new CargaDevengadoComandoModel();
      comando.nroFormulario = resultado.get('nroFormulario').value;
      comando.idPrestamo = resultado.get('idPrestamo').value;
      comando.devengado = resultado.get('devengadoNuevo').value;
      comando.idFormulario = resultado.get('id').value;

      this.pagosService.cargarDevengadoManual(comando)
        .subscribe((res) => {
          if (res) {
            let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === comando.nroFormulario)[0];
            formulario.devengado = comando.devengado;
            formulario.id = comando.idFormulario;
            formulario.fechaDevengado = new Date(Date.now());
            this.crearFormFormulariosParaSuaf();
          } else {
            this.notificacionService.informar(['El devengado ingresado ya existe para otro formulario.']);
          }
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    } else {
      resultado.updateValueAndValidity();
    }
  }

  public editarDevengado(resultado: any) {
    let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === resultado.get('nroFormulario').value)[0];
    formulario.devengadoNuevo = resultado.get('devengado').value;
    formulario.devengado = null;
    formulario.fechaDevengado = null;
    this.crearFormFormulariosParaSuaf();
  }

  public edicionDevengado(resultado: FormControl) {
    let nuevoDevengado = resultado.value;
    if (nuevoDevengado.length === 4) {
      if (!(nuevoDevengado.indexOf('/') >= 0)) {
        resultado.setValue(nuevoDevengado + '/');
      }
    }
  }

  public borrarDevengado(resultado: any) {
    let comando = new CargaDevengadoComandoModel();
    comando.nroFormulario = resultado.get('nroFormulario').value;
    comando.idPrestamo = resultado.get('idPrestamo').value;
    comando.idFormulario = resultado.get('id').value;

    this.pagosService.borrarDevengado(comando)
      .subscribe((res) => {
        let formulario = this.bandejaResultados.filter((x) => x.nroFormulario === resultado.get('nroFormulario').value)[0];
        formulario.devengadoNuevo = null;
        formulario.devengado = null;
        formulario.fechaDevengado = null;

        this.crearFormFormulariosParaSuaf();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }
}
