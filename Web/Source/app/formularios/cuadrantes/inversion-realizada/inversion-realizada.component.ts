import { Component, OnInit } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormGroup } from '@angular/forms';
import { FormulariosService } from '../../shared/formularios.service';
import { InversionRealizada } from '../../shared/modelo/inversion-realizada.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { isEmpty } from '../../../shared/forms/custom-validators';
import { Emprendimiento } from '../../shared/modelo/emprendimiento.model';

@Component({
  selector: 'bg-inversion-realizada',
  templateUrl: './inversion-realizada.component.html',
  styleUrls: ['./inversion-realizada.component.scss']
})

export class InversionRealizadaComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public inversionesRealizadas: InversionRealizada[] = [];
  public detallesParaEliminar: number[] = [];
  public emprendimiento: Emprendimiento = new Emprendimiento();

  constructor(private formularioService: FormulariosService,
              private notificacionService: NotificacionService) {
    super();
  }

  ngOnInit(): void {
    if (this.formulario.inversionesRealizadas && this.formulario.inversionesRealizadas.length > 0) {
      this.inversionesRealizadas = this.formulario.inversionesRealizadas;
    }
    if(this.formulario.datosEmprendimiento){
      this.emprendimiento = this.formulario.datosEmprendimiento;
    }
  }

  public completarLista(listaInversiones) {
    this.inversionesRealizadas = listaInversiones;
  }

  public completarDetallesParaBorra(listaIds) {
    this.detallesParaEliminar = listaIds;
  }

  actualizarDatos() {
    this.formularioService.eliminarDetallesInversion(this.detallesParaEliminar).subscribe(() => {
        if(this.inversionesRealizadas.length > 0){
          this.formularioService.actualizarInversionesRealizadas(this.formulario.id, this.inversionesRealizadas)
            .subscribe((res) => {
                this.formulario.inversionesRealizadas = res;
                this.inversionesRealizadas = res;
              },
              (errores) => {
                this.notificacionService.informar(errores, true);
              });
        }else{
          this.formulario.inversionesRealizadas = this.inversionesRealizadas;
        }
      },
      (errores) => {
        this.notificacionService.informar(errores, true);
      })
  }

  esValido(): boolean {
    if (!this.editable) {
      return true;
    } else {
      return (this.detallesParaEliminar.length > 0 || this.inversionesRealizadas.length > 0) && !isEmpty(this.formulario) && !isEmpty(this.formulario.datosEmprendimiento.id);
    }
  }

  inicializarDeNuevo(): boolean {
    return false;
  }
}
