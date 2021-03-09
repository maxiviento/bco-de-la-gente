import { Component, Input, OnInit } from '@angular/core';
import { MotivosBajaService } from '../servicios/motivosbaja.service';
import { MotivoBaja } from '../modelo/motivoBaja.modelo';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DetalleLineaFormulario } from '../../shared/modelo/detalle-linea-formulario.model';
import { Router } from '@angular/router';
import { FormulariosService } from '../../shared/formularios.service';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { OrigenService } from '../../shared/origen-prestamo.service';
import { OrigenPrestamo } from '../../shared/modelo/origen-prestamo.model';

@Component({
  selector: 'bg-modal-seleccion-detalle',
  templateUrl: './modal-seleccion-detalle.component.html',
  styleUrls: ['./modal-seleccion-detalle.component.scss'],
})

export class ModalSeleccionDetalleComponent implements OnInit {
  @Input()
  public detallesLinea: DetalleLineaFormulario[];
  public form: FormGroup;
  public origenes: OrigenPrestamo[];
  public seleccionoDetalle: boolean;
  private detallesFormArray: FormArray;

  constructor(private fb: FormBuilder,
              private origenesService: OrigenService,
              private formulariosService: FormulariosService,
              private activeModal: NgbActiveModal,
              private router: Router) {
  }

  public ngOnInit(): void {
    this.detallesFormArray = new FormArray((this.detallesLinea || [])
      .map((detalle) => new FormGroup({
        id: new FormControl(detalle.id),
        color: new FormControl(detalle.color),
        visualizacion: new FormControl(detalle.visualizacion),
        seleccionado: new FormControl(false),
      })));
    this.form = this.fb.group({
      origen: ['', Validators.required],
      detalles: this.detallesFormArray,
    });
    this.origenesService
      .consultarOrigenes()
      .subscribe((origenes) => {
        this.origenes = origenes;
      });
  }

  public ngAfterContentInit(): void {
    this.form = this.fb.group({
      origen: ['4', Validators.required],
      detalles: this.detallesFormArray,
    });
  }

  public seleccionarDetalle(detalle: FormGroup): void {
    this.seleccionoDetalle = !detalle.get('seleccionado').value;
    detalle.patchValue({seleccionado: this.seleccionoDetalle});
    if (this.seleccionoDetalle) {
      let detallesArray = this.detallesArray();
      for (let det of detallesArray.controls) {
        if (detalle.get('id').value !== det.get('id').value) {
          det.patchValue({seleccionado: false});
        }
      }
    }
  }

  public aceptar() {
    let f = this.form.value;
    let origen = f.origen;

    let detalle = new DetalleLineaFormulario();
    if (f.detalles.length > 1) {
      detalle = f.detalles.filter((d) => d.seleccionado)[0];
    } else {
      detalle = f.detalles[0];
    }
    this.activeModal.close();
    this.formulariosService.inicializarFormularioNuevo(this.detallesLinea.filter((d) => d.id === detalle.id)[0], origen);
    this.router.navigate(['/formularios/nuevo']);
  }

  public cerrar() {
    this.activeModal.dismiss();
  }

  public detallesArray(): FormArray {
    return this.form.get('detalles') as FormArray;
  }

  public habilitarAceptar(): boolean {
    if (this.form.value.detalles.length > 1) {
      return this.form.get('origen').value === '' || !this.seleccionoDetalle;
    } else {
      return this.form.get('origen').value === '';
    }
  }
}
