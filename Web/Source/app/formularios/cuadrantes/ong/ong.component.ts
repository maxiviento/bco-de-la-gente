import { CuadranteFormulario } from '../cuadrante-formulario';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OngComboResultado } from '../../shared/modelo/ong.model';
import { FormulariosService } from '../../shared/formularios.service';
import { OngFormulario } from '../../shared/modelo/ong-formulario.model';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'bg-cuadrante-ong',
  templateUrl: './ong.component.html',
  styleUrls: ['./ong.component.scss']
})
export class ONGComponent extends CuadranteFormulario implements OnInit {

  public listaOngs: OngComboResultado[];
  public form: FormGroup;

  constructor(private fb: FormBuilder,
    private formulariosService: FormulariosService) {
    super();
  }

  public ngOnInit(): void {
    if (!this.formulario.datosONG) {
      this.formulario.datosONG = new OngFormulario();
    }
    this.formulario.datosONG.idFormulario = this.formulario.id ? this.formulario.id : null;
    this.crearFormulario();
    this.cargarComboONGS();
  }

  private crearFormulario(): void {
    this.form = this.fb.group(
      {
        nombre: [this.formulario.datosONG.nombreGrupo,
        Validators.compose([Validators.minLength(3),
        Validators.maxLength(100)])],
        numero: this.formulario.datosONG.numeroGrupo,
        idOng: [this.formulario.datosONG.idOng, Validators.required]
      }
    );
    if (!this.editable) {
      this.form.disable();
    }
  }

  private cargarComboONGS(): void {
    this.formulariosService.obtenerOngs(this.formulario.detalleLinea.lineaId).subscribe((comboOngs) => {
      this.listaOngs = comboOngs;
    });
  }

  public actualizarDatos() {
    let formModel = this.form.value;
    this.formulario.datosONG.nombreGrupo = formModel.nombre;
    this.formulario.datosONG.idOng = formModel.idOng === 'null' ? null : formModel.idOng;
    if (!isNullOrUndefined(this.formulario.id) && this.editable) {
      this.formulariosService.registrarOngParaFormularios(this.formulario.idAgrupamiento, this.formulario.datosONG).subscribe();
    }
  }

  public esValido(): boolean {
    if (!this.editable) {
      return true;
    }
    return this.form.valid;
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }
}
