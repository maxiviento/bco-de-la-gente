import { Component, EventEmitter, Input, OnInit, Output, ViewChild, } from '@angular/core';
import { FormulariosService } from '../../../../formularios/shared/formularios.service';
import { FiltrosFormularioConsulta } from '../../modelos/filtros-formulario-consulta.model';
import { BancoService } from '../../../../shared/servicios/banco.service';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { BusquedaSucursalBancariaComponent } from '../../../../shared/componentes/busqueda-sucursal-bancaria/busqueda-sucursal-bancaria.component';

@Component({
  selector: 'bg-actualizar-sucursal',
  templateUrl: './actualizar-sucursal.component.html',
})

export class ActualizarSucursalComponent implements OnInit {
  public filtros: FiltrosFormularioConsulta;

  @Input() public idsFormularios: number [] = [];
  @Output() public limpiarIdsSeleccionados: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() public clickVolver: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input() public esPrestamo: boolean = false;

  @ViewChild(BusquedaSucursalBancariaComponent)
  public componenteSucursal: BusquedaSucursalBancariaComponent;

  constructor(private formularioService: FormulariosService,
              private bancoService: BancoService,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit() {

  }

  public validarAceptar(): boolean {
    return this.componenteSucursal.formValid();
  }

  public agregarSucursalFormularios(): void {
    let actSucursalComando = {
      idsFormularios: this.idsFormularios,
      idBanco: this.componenteSucursal.getBancoId(),
      idSucursal: this.componenteSucursal.getSucursalId()
    };

    this.formularioService.agregarSucursalFormularios(actSucursalComando)
      .subscribe((res) => {
        if (res) {
          this.notificacionService.informar(['La sucursal de el/los formulario/s se actualizó con éxito.'])
            .result
            .then(() => {
              this.limpiarIdsSeleccionados.emit(true);
              this.componenteSucursal.resetComponent();
            });
        }
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  public volver(): void {
    this.clickVolver.emit();
  }
}
