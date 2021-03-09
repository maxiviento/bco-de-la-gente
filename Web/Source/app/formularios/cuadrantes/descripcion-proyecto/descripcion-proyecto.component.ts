import { Component, OnInit } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../../shared/notificacion.service';
import { FormulariosService } from '../../shared/formularios.service';

@Component({
  selector: 'bg-descripcion-proyecto',
  templateUrl: './descripcion-proyecto.component.html',
  styleUrls: ['./descripcion-proyecto.component.scss'],
})

export class DescripcionProyectoComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;

  constructor(private formulariosService: FormulariosService,
              private notificacionService: NotificacionService,
              private fb: FormBuilder) {
    super();
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  public esValido(): boolean {
    return true;
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }

  private crearForm() {
    this.form = this.fb.group({
      descripcion: ['', Validators.required]
    });
  }

  public actualizarDatos() {
  }
}
