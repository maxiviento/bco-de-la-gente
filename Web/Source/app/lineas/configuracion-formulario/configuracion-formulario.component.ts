import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../shared/notificacion.service';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { InformacionLinea } from '../../formularios/shared/modelo/informacionLinea.model';
import { Cuadrante } from '../../formularios/shared/modelo/cuadrante.model';
import { LineaCombo } from '../../formularios/shared/modelo/linea-combo.model';
import { DetalleLineaCombo } from '../shared/modelo/detalle-linea-combo.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-configuracion-formulario',
  templateUrl: './configuracion-formulario.component.html',
  styleUrls: ['./configuracion-formulario.component.scss'],
})

export class ConfiguracionFormularioComponent implements OnInit {
  public form: FormGroup;
  public lineas: LineaCombo [] = [];
  public cuadrantesOrdenados: Cuadrante [] = [];
  public cuadrantes: Cuadrante [] = [];
  public idLinea: number;
  public idDetalle: number;
  public lsDetalles: DetalleLineaCombo[] = [];
  public editable: boolean = true;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private lineaService: LineaService,
              private titleService: Title) {
    this.titleService.setTitle('Configuración de formulario ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.consultarLineas();
    this.crearForm();
  }

  private crearForm(): void {
    let id = this.idLinea ? this.idLinea.toString() : null;
    let lineaFormControl = new FormControl(this.idLinea ? id : null, Validators.required);
    let detalleLineaFormControl = new FormControl(this.idDetalle, Validators.required);
    lineaFormControl.valueChanges.subscribe((value) => {
      this.lsDetalles = [];
      this.form.get('detalleLinea').setValue(null);
      if (value) {
        this.idLinea = value;
        this.lineaService.obtenerDetallesLineaCombo(value).subscribe((detalles) => {
          this.lsDetalles = detalles;
        });
      }
    });
    detalleLineaFormControl.valueChanges.subscribe((value) => {
      if (value) {
        this.idDetalle = value;
        this.consultarCuadrantes(value);
      }
    });

    this.form = this.fb.group({
      linea: lineaFormControl,
      detalleLinea: detalleLineaFormControl,
      cuadrantes: this.fb.array((this.cuadrantes || []).map((cuadrante) =>
        this.fb.group({
          idCuadrante: [cuadrante.idCuadrante],
          nombre: [cuadrante.nombre],
          descripcion: [cuadrante.descripcion],
          ck_impresion: [false],
          ck_pantalla: [false]
        })
      )),
      cuadrantesOrdenados: this.fb.array((this.cuadrantesOrdenados || []).map((cuadrante) =>
        this.fb.group({
          idCuadrante: [cuadrante.idCuadrante],
          nombre: [cuadrante.nombre],
          descripcion: [cuadrante.descripcion],
          ck_impresion: [(cuadrante.idTipoSalida == 3 || cuadrante.idTipoSalida == 2)],
          ck_pantalla: [(cuadrante.idTipoSalida == 3 || cuadrante.idTipoSalida == 1)]
        })
      )),
      ck_seleccionarTodos: [false]
    });
  }

  public get cuadrantesFormArray(): FormArray {
    return this.form.get('cuadrantes') as FormArray;
  }

  public get cuadrantesOrdenadosFormArray(): FormArray {
    return this.form.get('cuadrantesOrdenados') as FormArray;
  }

  private consultarLineas(): void {
    this.lineaService.consultarLineasParaCombo()
      .subscribe((lineas) => {
        this.lineas = lineas.filter((linea) => linea.dadoDeBaja === false);
      });
  }

  private consultarCuadrantes(id: number): void {
    this.lineaService.consultarCuadrantesLinea(id)
      .subscribe((cuadrantes) => {
        this.cuadrantesOrdenados = cuadrantes;
        this.lineaService.consultarCuadrantesDisponibles()
          .subscribe((cuadrantesDisponibles) => {
            this.cuadrantes = this.quitarCuadrantesOrdenados(cuadrantesDisponibles);
            this.crearForm();
          });
      });
  }

  public quitarCuadrantesOrdenados(cuadrantes: Cuadrante []): Cuadrante [] {
    this.cuadrantesOrdenados
      .forEach((cuadrante) => {
        let indice = cuadrantes.indexOf(cuadrantes.find((x) => x.idCuadrante == cuadrante.idCuadrante));
        cuadrantes.splice(indice, 1);
      });
    return cuadrantes;
  }

  public moverADerecha(indice: number): void {
    this.cuadrantesOrdenadosFormArray.push(this.cuadrantesFormArray.at(indice));
    this.cuadrantesFormArray.removeAt(indice);
  }

  public moverAIzquierda(indice: number): void {
    this.cuadrantesFormArray.push(this.cuadrantesOrdenadosFormArray.at(indice));
    this.cuadrantesOrdenadosFormArray.removeAt(indice);
  }

  public subir(indice: number): void {
    let cuadrante = this.cuadrantesOrdenadosFormArray.at(indice);
    this.cuadrantesOrdenadosFormArray.removeAt(indice);
    this.cuadrantesOrdenadosFormArray.insert(indice - 1, cuadrante);
  }

  public bajar(indice: number): void {
    let cuadrante = this.cuadrantesOrdenadosFormArray.at(indice);
    this.cuadrantesOrdenadosFormArray.removeAt(indice);
    this.cuadrantesOrdenadosFormArray.insert(indice + 1, cuadrante);
  }

  public guardarConfiguracion(): void {
    let linea = new InformacionLinea();
    this.cuadrantesOrdenados = [];
    this.cuadrantesOrdenadosFormArray.controls
      .map((cuadrante) =>
        this.cuadrantesOrdenados.push(new Cuadrante(null, cuadrante.value.nombre, cuadrante.value.idCuadrante)));

    for (let i = 0; i < this.cuadrantesOrdenadosFormArray.length; i++) {
      this.cuadrantesOrdenados[i].orden = i + 1;
      let ckImpresion = this.cuadrantesOrdenadosFormArray.controls[i].get('ck_impresion').value;
      let ckPantalla = this.cuadrantesOrdenadosFormArray.controls[i].get('ck_pantalla').value;
      if (ckImpresion && ckPantalla) {
        this.cuadrantesOrdenados[i].idTipoSalida = 3;
      } else if (ckImpresion) {
        this.cuadrantesOrdenados[i].idTipoSalida = 2;
      } else if (ckPantalla) {
        this.cuadrantesOrdenados[i].idTipoSalida = 1;
      }
    }
    linea.idLinea = this.idLinea;
    linea.idDetalleLinea = this.idDetalle;
    linea.cuadrantes = this.cuadrantesOrdenados;
    this.lineaService.registrarConfiguracion(linea)
      .subscribe(() => {
        this.notificacionService.informar(['La operación se realizó con éxito.']);
        this.limpiarFormulario();
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  public cortarNombre(nombre: string): string {
    let largo = 45;
    let res = nombre.substring(0, largo);
    if (nombre.length > largo) {
      res = res + '...';
    }
    return res;
  }

  public validarTiposSalida(): boolean {
    return this.cuadrantesOrdenadosFormArray.controls
      .filter((cuadrante) =>
        cuadrante.get('ck_impresion').value ||
        cuadrante.get('ck_pantalla').value).length === this.cuadrantesOrdenadosFormArray.controls.length;
  }

  private limpiarFormulario() {
    let lineaFormControl = this.form.get('linea');
    lineaFormControl.setValue(null);
    lineaFormControl.updateValueAndValidity();
    let detalleLineaFormControl = this.form.get('detalleLinea');
    detalleLineaFormControl.setValue(null);
    detalleLineaFormControl.updateValueAndValidity();
  }

  public seleccionarTodos(): void {
    let todos = this.todosSeleccionados();
    (this.form.get('cuadrantesOrdenados') as FormArray).controls.forEach((cuadrante) => {
      cuadrante.get('ck_impresion').setValue(!todos);
      cuadrante.get('ck_pantalla').setValue(!todos);
    });
  }

  /*Retorna true si todos los checks de los cuadrantes estan seleccionados*/
  public todosSeleccionados(): boolean {
    let todosSeleccionados;
    todosSeleccionados = this.form.value.cuadrantesOrdenados
        .filter((cuadrante) => cuadrante.ck_impresion && cuadrante.ck_pantalla).length ===
      this.form.value.cuadrantesOrdenados.length;
    return todosSeleccionados;
  }

  public hayCuadrantesOrdenados(): boolean {
    return !!(this.form.get('cuadrantesOrdenados') as FormArray).length;
  }
}
