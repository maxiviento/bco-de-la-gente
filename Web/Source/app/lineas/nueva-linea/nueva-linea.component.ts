import { LineaPrestamo } from '../shared/modelo/linea-prestamo.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { Router } from '@angular/router';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { LineaItemsComponent } from '../shared/linea-items/linea-items.component';
import { LineaDetalleComponent } from '../shared/linea-detalle/linea-detalle.component';
import { DetalleLineaPrestamo } from '../shared/modelo/detalle-linea-prestamo.model';
import { Requisito } from '../shared/modelo/requisito-linea';
import { SexoDestinatario } from '../shared/modelo/destinatario-prestamo.model';
import { MotivoDestino } from '../../motivo-destino/shared/modelo/motivo-destino.model';
import { isNull, isNullOrUndefined, isUndefined } from 'util';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IntegranteSocio } from '../shared/modelo/integrante-socio.model';
import { TipoFinanciamiento } from '../shared/modelo/tipo-financiamiento.model';
import { TipoInteres } from '../shared/modelo/tipo-interes.model';
import { TipoGarantia } from '../shared/modelo/tipo-garantia.model';
import { IntegrantesService } from '../shared/integrantes.service';
import { TiposFinanciamientoService } from '../shared/tipos-financiamiento.service';
import { TiposInteresService } from '../shared/tipos-interes.service';
import { TiposGarantiaService } from '../shared/tipos-garantia.service';
import { Convenio } from '../../shared/modelo/convenio-model';
import { ProgramaCombo } from '../shared/modelo/programa-combo.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import { OngLinea } from '../shared/modelo/ong-linea.model';

@Component({
  selector: 'bg-nueva-linea-prestamo',
  templateUrl: './nueva-linea.component.html',
  styleUrls: ['./nueva-linea.component.scss']
})

export class NuevaLineaComponent implements OnInit {
  public fechaActual: Date = new Date(Date.now());
  public linea: LineaPrestamo = new LineaPrestamo();
  public form: FormGroup;
  public pantallaActual: string;
  public bandera: boolean = false;
  public archivoLogo: any;
  public archivoPiePagina: any;
  public maxSize: number = 4;
  public departamentoIds: string[] = [];
  public localidadIds: string[] = [];
  public integrantes: IntegranteSocio[] = [];
  public financiamientos: TipoFinanciamiento[] = [];
  public intereses: TipoInteres[] = [];
  public garantias: TipoGarantia[] = [];
  public ocultarAceptar: boolean = true;
  public convenios: Convenio[] = [];
  public conveniosPago: Convenio[] = [];
  public conveniosRecupero: Convenio[] = [];
  public deptoLocalidad: boolean = false;
  public verDeptoLocalidad: boolean = true;
  public garantiasCompletas: TipoGarantia[] = [];
  public esEditable: boolean = true;
  public verOng: boolean = false;

  constructor(private integranteService: IntegrantesService,
              private tipoFinanciamientoService: TiposFinanciamientoService,
              private tipoInteresService: TiposInteresService,
              private tipoGarantiaService: TiposGarantiaService,
              private lineaService: LineaService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Registrar línea de micro-préstamo ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.cargarCombos();
    this.linea.detalleLineaPrestamo = [];
    this.crearForm();
    if (!this.pantallaActual) {
      this.pantallaActual = 'linea';
    }
  }

  private cargarCombos() {
    this.integranteService.consultarIntegrantes()
      .subscribe((integrante) => this.integrantes = integrante);
    this.tipoFinanciamientoService.consultarTiposFinanciamiento()
      .subscribe((financiamiento) => this.financiamientos = financiamiento);
    this.tipoInteresService.consultarTiposInteres()
      .subscribe((interes) => this.intereses = interes);
    this.lineaService.consultarConvenios()
      .subscribe((resultado) => {
        this.convenios = resultado;
        this.filtrarConvenios();
      });
    this.tipoGarantiaService.consultarTiposGarantia().subscribe((garantia) => this.garantiasCompletas = garantia);
  }

  private cargarComboGarantia() {
    let integranteSocio = (this.detalleLineaForm.get('integranteSocio') as FormControl);
    if (integranteSocio.value == 1) {
      this.tipoGarantiaService.consultarTiposGarantia()
        .subscribe((garantia) => this.garantias = garantia.filter((g) => (g.id == 1 || g.id == 2)));
    }
    if (integranteSocio.value == 2 || integranteSocio.value == 3) {
      this.tipoGarantiaService.consultarTiposGarantia()
        .subscribe((garantia) => this.garantias = garantia.filter((g) => (g.id == 3 || g.id == 4)));
    }
  }

  private filtrarConvenios() {
    this.conveniosPago = this.convenios.filter((c) => c.idTipoConvenio === 1);
    this.conveniosRecupero = this.convenios.filter((c) => c.idTipoConvenio === 2);
  }

  private crearForm() {
    this.form = this.fb.group({
      itemLineaForm: LineaItemsComponent.nuevoFormGroup(this.linea),
      detalleLineaForm: LineaDetalleComponent.nuevoFormGroup()
    });

    this.itemLineaForm
      .valueChanges
      .subscribe((value) => {
        if (value) {
          this.bandera = !this.esValido();
        }
      });
    let conPrograma = this.itemLineaForm.get('conPrograma');
    let deptoLocalidad = this.itemLineaForm.get('deptoLocalidad');
    let conOng = this.itemLineaForm.get('conOng');
    let programa = this.itemLineaForm.get('programa');
    if (!conPrograma.value) {
      programa.setValue(null, {emitEvent: false});
      programa.disable();
    }

    deptoLocalidad
      .valueChanges
      .subscribe(() => {
        if (deptoLocalidad.value) {
          this.deptoLocalidad = deptoLocalidad.value;
        } else {
          this.deptoLocalidad = false;
        }
        this.verDeptoLocalidad = this.deptoLocalidad;
      });

    conOng
      .valueChanges
      .subscribe(() => {
        this.verOng = conOng.value;
        if (isNullOrUndefined(this.linea.lsOng)) {
          this.linea.lsOng = [];
        }
      });

    conPrograma
      .valueChanges
      .subscribe(() => {
        if (conPrograma.value) {
          programa.enable();
          programa.setValidators(Validators.required);
        } else {
          programa.disable();
          programa.setValue(null, {emitEvent: false});
          programa.clearValidators();
        }
      });
    let integranteSocio = (this.detalleLineaForm.get('integranteSocio') as FormControl);
    if ((integranteSocio) !== null) {
      (integranteSocio)
        .valueChanges
        .subscribe(() => {
          this.detalleLineaForm.get('tipoGarantia').setValue(null);
          this.cargarComboGarantia();
          this.detalleLineaForm.get('apoderado').setValue(false);
        });
    }
  }

  public esValido(): boolean {
    return (this.itemLineaForm.valid &&
      (this.linea.detalleLineaPrestamo && this.linea.detalleLineaPrestamo.length > 0)
      && (this.linea.requisitos && this.linea.requisitos.length > 0)
      && !isNull(this.archivoLogo)
      && !isNull(this.archivoPiePagina));
  }

  public get itemLineaForm(): FormGroup {
    return this.form.get('itemLineaForm') as FormGroup;
  }

  public get detalleLineaForm(): FormGroup {
    return this.form.get('detalleLineaForm') as FormGroup;
  }

  public registrarLinea() {
    this.obtenerLinea();
    if (!this.itemLineaForm.valid) {
      this.notificacionService.informar(['Debe llenar todos los campos solicitados']);
    } else {
      if (!this.linea.requisitos) {
        this.notificacionService.informar(['Debe asignar al menos un requisito']);
      } else {
        if (!this.archivoLogo) {
          this.notificacionService.informar(['Debe cargar un archivo como logo/imagen cabecera de línea.']);
        } else {
          if (!this.archivoPiePagina) {
            this.notificacionService.informar(['Debe cargar un archivo como firmantes/imagen pie de línea.']);
          } else {
            if (!this.ongValida()) {
              this.notificacionService.informar(['Debe asignar al menos una ONG a la línea.']);
            } else {
              if (this.itemLineaForm.get('programa') && !this.itemLineaForm.get('programa')) {
                this.notificacionService.informar(['Debe seleccionar un programa.']);
              } else {
                if (this.linea.deptoLocalidad && !this.localidadIds.length && !this.departamentoIds.length) {
                  this.notificacionService.informar(['Debe seleccionar al menos un departamento o localidad.']);
                } else {
                  this.linea.logo = this.archivoLogo;
                  this.linea.piePagina = this.archivoPiePagina;
                  this.linea.localidadIds = this.itemLineaForm.value.deptoLocalidad ? this.localidadIds.join(',') : undefined;
                  this.lineaService
                    .registrarLinea(this.linea).subscribe((idLinea) => {
                    this.notificacionService
                      .informar(['La línea de préstamo se registró con éxito.'])
                      .result
                      .then(() => this.router.navigate(['/consulta-linea', idLinea]));
                  }, (errores) => {
                    this.notificacionService.informar(errores, true);
                  });
                }
              }
            }
          }
        }
      }
    }
  }

  public archivoSeleccionadoLogo(archivo: File): void {
    let tamanioArchivo = ((archivo.size / 1024) / 1024);
    if (tamanioArchivo < this.maxSize) {
      this.archivoLogo = archivo;
    } else {
      this.notificacionService.informar([`El tamaño de la imagen no puede superar los ${this.maxSize} mb.`], true);
    }
  }

  public ongValida(): boolean {
    if (this.verOng) {
      return !!this.linea.lsOng.length;
    } else {
      this.linea.lsOng = [];
      return true;
    }
  }

  public archivoSeleccionadoPiePagina(archivo: File): void {
    let tamanioArchivo = ((archivo.size / 1024) / 1024);
    if (tamanioArchivo < this.maxSize) {
      this.archivoPiePagina = archivo;
    } else {
      this.notificacionService.informar([`El tamaño de la imagen no puede superar los ${this.maxSize} mb.`], true);
    }
  }

  public guardarLinea(datos: AbstractControl, pantalla: string) {
    if (datos) {
      this.itemLineaForm.get('programa').enable();
      this.itemLineaForm.setValue(datos.value);
      this.obtenerLinea();

    }
    if (pantalla == 'requisitos') {
      this.modificarPantalla('requisitos');
    } else {
      this.modificarPantalla('ong');
    }
  }

  private obtenerLinea() {
    let itemLineaModel = this.itemLineaForm.value;

    if (!isUndefined(itemLineaModel.id)) {
      this.linea.id = itemLineaModel.id;
    }
    if (!isUndefined(itemLineaModel.nombre)) {
      this.linea.nombre = itemLineaModel.nombre;
    }
    if (!isUndefined(itemLineaModel.descripcion)) {
      this.linea.descripcion = itemLineaModel.descripcion;
    }
    if (!isUndefined(itemLineaModel.conOng)) {
      this.linea.conOng = itemLineaModel.conOng || false;
    }
    if (!isUndefined(itemLineaModel.conCurso)) {
      this.linea.conCurso = itemLineaModel.conCurso || false;
    }
    if (!isUndefined(itemLineaModel.conPrograma)) {
      this.linea.conPrograma = itemLineaModel.conPrograma || false;
    }
    if (!isUndefined(itemLineaModel.deptoLocalidad)) {
      this.linea.deptoLocalidad = itemLineaModel.deptoLocalidad || false;
    }
    if (!isUndefined(itemLineaModel.destinatario)) {
      this.linea.idSexoDestinatario = itemLineaModel.destinatario;
    }
    if (!isUndefined(itemLineaModel.destinatario)) {
      this.linea.sexoDestinatario = new SexoDestinatario(itemLineaModel.destinatario);
    }
    if (!isUndefined(itemLineaModel.motivoDestino)) {
      this.linea.idMotivoDestino = itemLineaModel.motivoDestino;
    }
    if (!isUndefined(itemLineaModel.motivoDestino)) {
      this.linea.motivoDestino = new MotivoDestino(itemLineaModel.motivoDestino);
    }
    if (!isUndefined(itemLineaModel.objetivo)) {
      this.linea.objetivo = itemLineaModel.objetivo;
    }
    if (!isUndefined(itemLineaModel.color)) {
      this.linea.color = itemLineaModel.color;
    }
    if (!isUndefined(itemLineaModel.programa)) {
      this.linea.idPrograma = itemLineaModel.programa;
    } else {
      this.linea.idPrograma = -1;
    }
    if (!isUndefined(this.linea.idPrograma)) {
      this.linea.programa = new ProgramaCombo(this.linea.idPrograma);
    }
  }

  public modificarPantalla(pantallaActual: string) {
    switch (pantallaActual) {
      case 'linea': {
        this.pantallaActual = 'linea';
        this.verDeptoLocalidad = true;
        break;
      }
      case 'requisitos': {
        this.pantallaActual = 'requisitos';
        this.verDeptoLocalidad = false;
        break;
      }
      case 'ong': {
        this.pantallaActual = 'ong';
        this.verDeptoLocalidad = false;
        break;
      }
      default:
        break;
    }
  }

  public agregarOng(lsOng: OngLinea []) {
    this.linea.lsOng = lsOng;
    this.bandera = !this.esValido();
    this.modificarPantalla('linea');
    this.obtenerLinea();
    this.crearForm();
  }

  public agregarRequisitos(requisitos: Requisito []) {
    this.linea.requisitos = requisitos;
    this.bandera = !this.esValido();
    this.modificarPantalla('linea');
    this.obtenerLinea();
    this.crearForm();
  }

  public agregarDetalleLinea(detalleLinea: DetalleLineaPrestamo) {
    this.linea.detalleLineaPrestamo.push(detalleLinea);
    this.bandera = !this.esValido();
    this.ocultarAceptar = false;
  }

  public guardarDepartamentosSeleccionados(departamentos: string[]) {
    this.departamentoIds = departamentos;
  }

  public guardarLocalidadesSeleccionadas(localidades: string[]) {
    this.localidadIds = localidades;
  }
}
