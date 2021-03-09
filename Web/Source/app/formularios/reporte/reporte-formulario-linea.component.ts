import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormulariosService } from '../shared/formularios.service';
import { BehaviorSubject } from 'rxjs';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { NotificacionService } from '../../shared/notificacion.service';
import { LineaCombo } from '../shared/modelo/linea-combo.model';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { DetalleLineaCombo } from '../../lineas/shared/modelo/detalle-linea-combo.model';
import { IdsDetalleLineas } from '../shared/modelo/ids-detalle-lineas-.model';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-reporte-formulario-linea',
  templateUrl: './reporte-formulario-linea.component.html',
  styleUrls: ['./reporte-formulario-linea.component.scss']
})

export class ReporteFormularioLineaComponent implements OnInit {
  public pdfFormulario = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;
  public lineas: LineaCombo[] = [];
  public detalles: DetalleLineaCombo[] = [];
  public form: FormGroup;
  public procesando: boolean = false;

  constructor(private fb: FormBuilder,
              private sanitizer: DomSanitizer,
              private router: Router,
              private notificacionService: NotificacionService,
              private lineaPrestamoService: LineaService,
              private formulariosService: FormulariosService,
              private titleService: Title) {
    this.titleService.setTitle('Imprimir formulario por línea ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.crearForm();

    this.lineaPrestamoService.consultarLineasParaCombo()
      .subscribe((lineas) => {
        this.lineas = (lineas);
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      lineaId: ['', Validators.required],
      detalleLineaId: ['', Validators.required]
    });

    this.form.get('lineaId').valueChanges.subscribe(() => {
      this.detalles = [];
      this.form.get('detalleLineaId').setValue('');
      let lineaSeleccionada = this.form.get('lineaId').value;
      if (lineaSeleccionada) {
        this.cargarDetalles(lineaSeleccionada);
      }
    });
  }

  private cargarDetalles(idLinea: number) {
    this.lineaPrestamoService.obtenerDetallesLineaCombo(idLinea).subscribe((res) => {
      this.detalles = res;
      if (this.detalles.length === 1 && idLinea) {
        let indice = this.detalles[0].idDetalleLinea;
        this.form.get('detalleLineaId').setValue(indice);
      }
    });
  }

  public imprimir() {
    if (this.form.valid) {
      let linea = this.form.get('lineaId') as FormControl;
      let detalleLinea = this.form.get('detalleLineaId') as FormControl;
      this.procesando = true;
      this.formulariosService.generarReporteFormularioLinea(new IdsDetalleLineas(detalleLinea.value, linea.value))
        .subscribe((resultado) => {
          this.procesando = false;
          this.pdfFormulario.next(this.sanitizer.bypassSecurityTrustResourceUrl(resultado));
          this.reporteSource = this.pdfFormulario.getValue();
        }, (errores) => {
          this.procesando = false;
          this.notificacionService.informar(errores, true);
        });
    }
  }

  public restablecer() {
    this.form.get('lineaId').setValue("");
    this.form.get('detalleLineaId').setValue("");
    this.reporteSource = null;
    this.pdfFormulario = new BehaviorSubject<SafeResourceUrl>(null);
    this.crearForm();
  }

  public volver(): void {
    this.router.navigate(['formularios']);
  }
}
