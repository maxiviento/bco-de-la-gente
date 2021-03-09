import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DestinoFondosService } from './destino-fondos.service';
import { AlMenosUnoSeleccionadoValidador } from './destino-fondos-validator';
import { DestinoFondos } from '../../../shared/modelo/destino-fondos.model';
import { OpcionDestinoFondos } from '../../shared/modelo/opcion-destino-fondos';
import { FormulariosService } from '../../shared/formularios.service';
import { CuadranteFormulario } from '../cuadrante-formulario';

@Component({
  selector: 'bg-destino-fondos',
  templateUrl: './destino-fondos.component.html',
  styleUrls: ['./destino-fondos.component.scss'],
})

export class DestinoFondosComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public destinos: DestinoFondos[] = [];

  constructor(private fb: FormBuilder,
              private destinoFondosService: DestinoFondosService,
              private formularioService: FormulariosService) {
    super();
  }

  public actualizarDatos() {
    let destinosFondos = this.setDestinosSeleccionados();
    this.formulario.destinosFondos = destinosFondos;
    if (this.formulario.idEstado === 3) {
      this.formularioService.actualizarDestinosFondos(this.formulario.id, destinosFondos)
        .subscribe(() => {
          this.formulario.destinosFondos = destinosFondos;
        });
    } else {
      this.formularioService.actualizarDestinosFondosAsociativas(this.formulario.idAgrupamiento, destinosFondos).subscribe(() => {
        this.formulario.destinosFondos = destinosFondos;
      });
    }
  }

  public esValido(): boolean {
    this.form.markAsDirty({onlySelf: true});
    if (!this.editable) {  // estamos en el ver
      return true;
    } else {
      return this.form.valid;
    }
  }

  public ngOnInit(): void {
    this.crearForm();
    this.destinoFondosService.consultarDestinosFondos()
      .subscribe((destinos) => {
        this.destinos = destinos;
        this.marcarDestinosSeleccionados();
        this.crearForm();
        if (!this.editable) {
          this.form.disable();
        }
      });
  }

  public crearForm(): void {
    this.form = this.fb.group({
      detalles: this.fb.control({value: '', disabled: true}, Validators.maxLength(100)),
      destinos: this.fb.array((this.destinos || []).map((destino) => {
        let formGroup = new FormGroup({
          id: new FormControl(destino.id),
          descripcion: new FormControl(destino.descripcion),
          seleccionado: new FormControl(destino.seleccionado),
        });
        formGroup
          .valueChanges
          .subscribe((destinoFondo) => {
            if (destinoFondo && DestinoFondosComponent.esOtros(destinoFondo.id)) {
              if (destinoFondo.seleccionado) {
                this.form.get('detalles').enable();
              } else {
                this.form.get('detalles').disable();
                this.form.get('detalles').setValue('');
              }
            }
          });
        return formGroup;
      })),
    }, {validator: AlMenosUnoSeleccionadoValidador});
    this.ponerObservacion();
    if (!this.editable) {
      this.form.disable();
    }
  }

  public get destinosFormArray(): FormArray {
    return this.form.get('destinos') as FormArray;
  }

  private setDestinosSeleccionados(): OpcionDestinoFondos {
    let destinoFondos = new OpcionDestinoFondos('', new Array<DestinoFondos>());

    let destinos = this.form.get('destinos').value;
    let detalle = this.form.get('detalles').value;

    destinos.forEach((destino) => {
      if (destino.seleccionado) {
        if (DestinoFondosComponent.esOtros(destino.id)) {
          destinoFondos.observaciones = detalle;
        }
        destinoFondos.destinosFondo.push(new DestinoFondos(destino.id));
      }
    });
    return destinoFondos;
  }

  private marcarDestinosSeleccionados(): void {
    if (this.formulario.destinosFondos) {
      this.formulario.destinosFondos.destinosFondo.forEach((destSeleccionado) => {
        this.destinos.filter((x) => x.id === destSeleccionado.id)[0].seleccionado = true;
      });
    }
  }

  private ponerObservacion(): void {
    if (this.formulario.destinosFondos) {
      let destinoOtro = this.formulario.destinosFondos.destinosFondo.some((x) => x.id === 99);
      if (destinoOtro) {
        let detalle = this.form.get('detalles');
        detalle.setValue(this.formulario.destinosFondos.observaciones);
        detalle.enable();
      }
    }
  }

  private static esOtros(id: number): boolean {
    return id === 99;
  }

  inicializarDeNuevo(): boolean {
    return false;
  }
}
