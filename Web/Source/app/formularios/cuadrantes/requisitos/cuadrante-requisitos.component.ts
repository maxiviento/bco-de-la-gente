import { Component, OnInit } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { LineaService } from '../../../shared/servicios/linea-prestamo.service';
import { RequisitoCuadrante } from '../../shared/modelo/requisito-cuadrante.model';

@Component({
  selector: 'bg-cuadrante-requisitos',
  templateUrl: './cuadrante-requisitos.component.html',
  styleUrls: ['./cuadrante-requisitos.component.scss']
})

export class CuadranteRequisitosComponent extends CuadranteFormulario implements OnInit {
  public requisitosSolicitante: RequisitoCuadrante[] = [];
  public requisitosGarante: RequisitoCuadrante[] = [];

  constructor(private lineaService: LineaService ) {
    super();
  }

  ngOnInit(): void {
    this.lineaService.consultarRequisitosLineaParaCuadrante(this.formulario.detalleLinea.lineaId).subscribe(
      (resultado) => {
        this.requisitosSolicitante = resultado.filter((x) => x.esSolicitante);
        this.requisitosGarante = resultado.filter((x) => x.esGarante);
      }
    );
  }

  public actualizarDatos() {
    return;
  }

  public esValido(): boolean {
    return true;
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }
}
