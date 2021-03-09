import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DestinoFondosService } from './destino-fondos.service';
import { AlMenosUnoSeleccionadoValidador } from './destino-fondos-validator';
import { FormulariosService } from '../../shared/formularios.service';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { ModalDomicilioComponent } from './modal-domicilio/modal-domicilio.component';
import { IngresoGrupo } from '../../shared/modelo/ingreso-grupo.model';
import { GrupoFamiliarService } from '../../shared/grupo-familiar.service';
import { GastoGrupo } from '../../shared/modelo/gastos-grupo.model';
import { isEmpty } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-ingresos-y-gastos-actuales',
  templateUrl: './ingresos-y-gastos-actuales.component.html',
  styleUrls: ['./ingresos-y-gastos-actuales.component.scss'],
})

export class IngresosYGastosActualesComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public ingresosGrupo: IngresoGrupo [] = [];
  public gastosGrupo: GastoGrupo [] = [];
  public totalGastos: number = 0;
  public totalIngresos: number = 0;
  public idGrupo: number = 0;

  constructor(private fb: FormBuilder,
              private formularioService: FormulariosService,
              private grupoFamiliarService: GrupoFamiliarService) {
    super();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarGastosGrupo();
    this.consultarIngresosGrupo();
  }

  private crearForm(): void {
  }

  public capacidadAhorro(): number {
    return this.totalIngresos - this.totalGastos;
  }

  private consultarIngresosGrupo(): void {
    if (this.formulario.id) {
      this.formularioService.obtenerIngresosGrupoFamiliarFormulario(this.formulario.id)
        .subscribe((res) => {
          if (res) {
            this.ingresosGrupo = res.filter((x) => x.idConcepto === 1);
            this.calcularIngresosEmpremdimiento();
            this.crearForm();
          }
        });
    } else {
      this.formularioService.obtenerIngresosGrupoFamiliar()
        .subscribe((res) => {
          if (res) {
            this.ingresosGrupo = res;
            this.crearForm();
          }
        });
    }
  }

  public obtenerIngresosGrupo() {
    this.totalIngresos = 0;
    this.formularioService.obtenerIngresoTotalGrupoFamiliar(this.idGrupo).subscribe(
      (ingreso) => {
        let i = this.ingresosGrupo.findIndex(x => x.id == 99);
        this.ingresosGrupo[i].valor = ingreso;
        this.ingresosGrupo.forEach((x) => this.totalIngresos += x.valor);
      }
    );
  }

  public calcularIngresosEmpremdimiento() {
    let sumatoriaCostosMensuales: number = 0;
    let ingresoEmprendimiento: number = 0;
    this.totalIngresos = 0;

    if (!isEmpty(this.formulario.precioVenta.unidadesEstimadas) ||
      this.formulario.precioVenta.unidadesEstimadas === 0) {
      this.formulario.precioVenta.costos.forEach((costo) => {
        sumatoriaCostosMensuales += costo.valorMensual;
      });
      ingresoEmprendimiento = (sumatoriaCostosMensuales) + (this.formulario.precioVenta.gananciaEstimada * this.formulario.precioVenta.unidadesEstimadas);
    }
    let i = this.ingresosGrupo.findIndex(x => x.id == 1);
    this.ingresosGrupo[i].valor = ingresoEmprendimiento;
    this.ingresosGrupo.forEach((x) => this.totalIngresos += x.valor);
  }


  public actualizarDatos() {
  }

  public esValido(): boolean {
    return true;
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }

  public consultarGastosGrupo() {
    this.grupoFamiliarService
      .obtenerIdGrupoUnico({dni: this.formulario.solicitante.nroDocumento, sexo: this.formulario.solicitante.sexoId, pais: this.formulario.solicitante.codigoPais}).subscribe(
      (resultado) => {
        if (resultado) {
          this.idGrupo = resultado;
          this.obtenerGastos();
          this.obtenerIngresosGrupo();
        }
      });
  }

  public obtenerGastos() {
    this.formularioService.obtenerGastosGrupoFamiliar(this.idGrupo).subscribe(
      (res) => {
        this.gastosGrupo = res;
        this.gastosGrupo.forEach((x) => this.totalGastos += x.monto);
      });
  }
}
