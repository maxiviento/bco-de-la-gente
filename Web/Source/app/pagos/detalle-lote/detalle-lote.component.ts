import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { NotificacionService } from '../../shared/notificacion.service';
import { PagosService } from '../shared/pagos.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { DetalleLote } from '../monto-disponible/shared/modelo/detalle-lote.model';
import { BandejaPagosResultado } from '../shared/modelo/bandeja-pagos-resultado.model';
import { DesagruparLoteComando } from '../shared/modelo/desagrupar-lote-comando.model';
import { HistorialLote } from '../shared/modelo/historial-lote.model';
import { Observable, Subject } from 'rxjs/Rx';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { PrestamosDetalleLoteConsulta } from '../shared/modelo/PrestamosDetalleLoteConsulta.model';
import { ModalFormulariosPrestamoComponent } from '../modal-formularios-prestamo/modal-formularios-prestamo.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'bg-detalle-lote',
  templateUrl: './detalle-lote.component.html',
  styleUrls: ['./detalle-lote.component.scss'],
})

export class DetalleLoteComponent implements OnInit {
  @Input() public esVer: boolean = false;
  @Input() public esDesagrupar: boolean = false;
  @Input() public esLiberar: boolean = false;
  @Input() public esAgregar: boolean = false;
  @Input() public idLote: number;
  public formLote: FormGroup;
  public formPrestamos: FormGroup;
  public formHistorial: FormGroup;
  public detalleLote: DetalleLote = new DetalleLote();
  public bandejaResultados: BandejaPagosResultado[] = [];
  public formulariosCheckeados: BandejaPagosResultado[] = [];
  public formulariosDescheckeados: BandejaPagosResultado[] = [];
  public bandejaHistorial: HistorialLote[] = [];
  public consultaPrestamos: PrestamosDetalleLoteConsulta = new PrestamosDetalleLoteConsulta();
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private pagosService: PagosService,
              private route: ActivatedRoute,
              private modalService: NgbModal,
              private router: Router) {
  }

  public ngOnInit(): void {
    this.crearFormCabecera();
    this.pagosService.obtenerCabeceraDetalleLote(this.idLote)
      .subscribe((detalleLote) => {
        this.detalleLote = detalleLote;
        this.crearFormCabecera();
      });

    this.configurarPaginacion();
    this.paginaModificada.next();

    this.pagosService.obtenerHistorialDetalleLote(this.idLote)
      .subscribe((historial) => {
        this.bandejaHistorial = historial;
        this.crearFormHistorial();
      });
  }

  private crearFormCabecera() {
    this.formLote = this.fb.group({
      numero: this.detalleLote.numero,
      nombre: this.detalleLote.nombre,
      fechaCreacion: this.detalleLote.fechaCreacionString,
      montoPrestamos: this.detalleLote.montoPrestamos,
      comision: this.detalleLote.comision,
      iva: this.detalleLote.iva,
      montoLote: this.detalleLote.montoLote,
      cantidadPretamos: this.detalleLote.cantidadPrestamos,
      cantidadBeneficiarios: this.detalleLote.cantidadBeneficiarios,
      nroMonto: this.detalleLote.nroMonto
    });
  }

  private crearFormPretamos() {
    this.marcarPrestamos();
    this.formPrestamos = this.fb.group({
      prestamos: this.fb.array((this.bandejaResultados || []).map((prestamo) =>
        this.fb.group({
          idPrestamo: [prestamo.id],
          linea: [prestamo.linea],
          departamento: [prestamo.departamento],
          localidad: [prestamo.localidad],
          apellidoYNombre: [prestamo.apellidoYNombre],
          nroFormulario: [prestamo.nroFormulario],
          origen: [prestamo.origen],
          cantFormularios: [prestamo.cantFormularios],
          nroPrestamo: [prestamo.nroPrestamo],
          montoOtorgado: [prestamo.montoOtorgado],
          fechaPedido: [prestamo.fechaPedido],
          seleccionado: [prestamo.seleccionado]
        })
      )),
    });
  }

  private crearFormHistorial() {
    this.formHistorial = this.fb.group({
      historial: this.fb.array((this.bandejaHistorial || []).map((historial) =>
        this.fb.group({
          fechaModificacionLote: [historial.fechaModificacionLote],
          nombre: [historial.nombre],
          cantPrestamos: [historial.cantPrestamos],
          cantBeneficiarios: [historial.cantBeneficiarios],
          montoTotalPrestamo: [historial.montoTotalPrestamo],
          montoComision: [historial.montoComision],
          montoIva: [historial.montoIva],
          montoTotalLote: [historial.montoTotalLote],
          usuarioModificacion: [historial.usuarioModificacion]
        })
      )),
    });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return { numeroPagina };
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.consultaPrestamos.idLote = this.idLote;
        this.consultaPrestamos.esVer = this.esVer;
        this.consultaPrestamos.numeroPagina = params.numeroPagina;
        return this.pagosService
          .obtenerPrestamosDetalleLote(this.consultaPrestamos);
      })
      .share();
    (<Observable<BandejaPagosResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormPretamos();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public consultarPrestamos(pagina?: number) {
    if (this.idLote) {
      this.prepararConsultaPrestamos();
      this.paginaModificada.next(pagina);
    }
  }

  private prepararConsultaPrestamos() {
    this.consultaPrestamos.idLote = this.idLote;
  }

  public get prestamosFormArray(): FormArray {
    return this.formPrestamos.get('prestamos') as FormArray;
  }

  public get historialFormArray(): FormArray {
    return this.formHistorial.get('historial') as FormArray;
  }

  public clickPrestamo(checked: boolean, idPrestamo: number) {
    let formulariosPorPrestamo = this.bandejaResultados.filter((x) => x.id === idPrestamo);
    if (checked === true) {
      formulariosPorPrestamo.forEach((form) => {
        form.seleccionado = true;
        this.formulariosCheckeados.push(form)
        this.formulariosDescheckeados = this.formulariosDescheckeados.filter((x) => x.id !== form.id);
      });
    } else {
      formulariosPorPrestamo.forEach((form) => {
        form.seleccionado = false;
        this.formulariosDescheckeados.push(form);
        this.formulariosCheckeados = this.formulariosCheckeados.filter((x) => x.id !== form.id);
      });
    }
    this.crearFormPretamos();
  }

  private marcarPrestamos() {
    this.marcarSeleccionados();
    this.marcarDeseleccionados();
  }

  private marcarSeleccionados(): void {
    this.formulariosCheckeados.forEach((elegido) => {
      let formulario = this.bandejaResultados.filter((form) => form.id === elegido.id);
      if (formulario.length !== 0) {
        formulario[0].seleccionado = true
      };
    });
  }

  private marcarDeseleccionados(): void {
    this.formulariosDescheckeados.forEach((elegido) => {
      let formulario = this.bandejaResultados.filter((form) => form.id === elegido.id);
      if (formulario.length !== 0) {
        formulario[0].seleccionado = false;
      };
    });
  }

  public verFormularioPrestamo(idPrestamo: number) {
    this.pagosService.obtenerFormulariosPorPrestamo(idPrestamo)
      .subscribe((formularios) => {
        const modalRef = this.modalService.open(ModalFormulariosPrestamoComponent, {backdrop: 'static', size: 'lg'});
        modalRef.componentInstance.formularioResultados = formularios;
      });
  }

  public armarComando(): DesagruparLoteComando {
    let comando = new DesagruparLoteComando();
    comando.idPrestamosDesagrupados = [];
    comando.idLote = this.idLote;
    let idsSeleccionados = new Set<number>();
    this.formulariosCheckeados.forEach((x) => idsSeleccionados.add(x.id));
    idsSeleccionados.forEach((x) => comando.idPrestamosDesagrupados.push(x));
    return comando;
  }

  public desagrupar() {
    this.notificacionService.confirmar('Se desagrupará el lote. ¿Desea continuar?')
      .result.then((result) => {
      if (result) {
        let comando = this.armarComando();
        if (comando.idPrestamosDesagrupados.length === 0) {
          this.notificacionService.informar(['Debe seleccionar al menos un prestamo para desagrupar.'], false);
        } else {
          this.pagosService.desagruparLote(comando).subscribe((res) => {
              this.notificacionService.informar(['El lote fue desagrupado con éxito.'], false);
              this.router.navigate(['/detalle-lote-ver/' + this.idLote]);
            },
            (errores) => {
              this.notificacionService.informar(errores, true);
            });
        }
      }
    });
  }
}
