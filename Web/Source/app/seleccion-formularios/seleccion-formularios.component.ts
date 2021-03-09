import { Component, OnInit, ViewChild } from '@angular/core';
import { FiltrosFormularioConsulta } from './shared/modelos/filtros-formulario-consulta.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';
import { GrillaFormulariosComponent } from './shared/componentes/grilla-formularios/grilla-formularios.component';
import { BusquedaFormulariosComponent } from './shared/componentes/busqueda-formularios/busqueda-formularios.component';
import { TipoConsultaFormulario } from './shared/modelos/tipo-consulta-formulario-enum';
import { OrigenPeticion } from '../pagos/shared/modelo/origen-peticion.enum';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'bg-seleccion-formularios',
  templateUrl: './seleccion-formularios.component.html'
})

export class SeleccionFormulariosComponent implements OnInit {
  public filtrosBusqueda: FiltrosFormularioConsulta;
  public mostrarFiltros: boolean = false;
  public mostrarSucursales: boolean = false;
  public mostrarDocumentacion: boolean = false;
  public limpiarIdsSeleccionados: boolean = false;
  public idLote: number;
  public idFormularioLinea: number;
  private tipoConsulta: number;
  public idsFormularios: number [] = [];
  public mostrarComboLotes: boolean = true;
  public origenPeticion: OrigenPeticion = OrigenPeticion.MENU;
  public esConApoderado: boolean = false;
  public totalizador: number;
  public esDocumentacion: boolean;

  @ViewChild(GrillaFormulariosComponent)
  public apartadoGrilla: GrillaFormulariosComponent;

  @ViewChild(BusquedaFormulariosComponent)
  public apartadoFiltros: BusquedaFormulariosComponent;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private location: Location,
              private titleService: Title) {
    this.titleService.setTitle('Actualizar sucursal bancaria ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    /* Si no es proveniente de un préstamo, debo mostrar los filtros de búsqueda */
    this.mostrarFiltros = !this.router.url.includes('prestamo');
    if (this.router.url.includes('sucursal')) {
      this.tipoConsulta = TipoConsultaFormulario.SUCURSAL_BANCARIA;
      this.titleService.setTitle('Actualizar sucursal bancaria ' + TituloBanco.TITULO);
      this.esDocumentacion = false;
    }
    if (this.router.url.includes('documentacion')) {
      this.tipoConsulta = TipoConsultaFormulario.DOCUMENTACION_PAGOS;
      this.titleService.setTitle('Imprimir documentación de pagos ' + TituloBanco.TITULO);
      this.esDocumentacion = true;
    }
    if (this.router.url.includes('prestamo')) {
      this.origenPeticion = OrigenPeticion.PRESTAMO;
      this.route.params.subscribe((params: Params) => {
        this.idFormularioLinea = +params['id'];
        this.buscarFormularios(new FiltrosFormularioConsulta());
      });
    }
    if (this.router.url.includes('lote')) {
      this.esDocumentacion = false;
      this.origenPeticion = OrigenPeticion.LOTE;
      this.mostrarComboLotes = false;
      this.route.params.subscribe((params: Params) => {
        this.idLote = +params['id'];
        this.buscarFormularios(new FiltrosFormularioConsulta());
      });
    }
    if (this.router.url.includes('sucursal')) {
      this.mostrarSucursales = true;
    }
    if (this.router.url.includes('documentacion')) {
      this.mostrarDocumentacion = true;
    }
  }

  public buscarFormularios(filtrosBusqueda: FiltrosFormularioConsulta): void {
    this.resetearIdsFormulario(true);
    filtrosBusqueda.idLote = this.idLote ? this.idLote : filtrosBusqueda.idLote;
    filtrosBusqueda.idFormularioLinea = this.idFormularioLinea || undefined;
    filtrosBusqueda.consulta = this.tipoConsulta || undefined;
    this.filtrosBusqueda = filtrosBusqueda;
  }

  public calcularTotalizador(totalizador: number): void {
    this.totalizador = totalizador;
  }

  public seleccionConApoderado(formularios: any []): void {
    this.esConApoderado = !!formularios.length;
  }

  public almacenarFormularios(idsFormularios: number []): void {
    this.idsFormularios = idsFormularios;
  }

  public resetearDatos(limpiar: boolean): void {
    this.limpiarIdsSeleccionados = limpiar;
    if (limpiar) {
      this.idsFormularios = [];
      if (this.apartadoGrilla) {
        this.apartadoGrilla.limpiarIds = true;
      }
    }
    this.filtrosBusqueda = Object.assign({}, this.filtrosBusqueda);
  }

  public resetearIdsFormulario(limpiar: boolean): void {
    this.limpiarIdsSeleccionados = limpiar;
  }

  public esPrestamo(): boolean {
    return this.router.url.includes('prestamo');
  }

  public volver(): void {
    if (this.router.url.includes('prestamo')) {
      // this.router.navigate(['actualizar-checklist', this.idPrestamo]);
      window.close();
    } else if (this.router.url.includes('lote')) {
      this.router.navigate(['bandeja-lotes']);
    } else {
      if (this.mostrarFiltros) {
        this.idsFormularios = [];
        this.apartadoGrilla.limpiarGrilla();
        this.apartadoFiltros.limpiarFiltros();
      } else {
        this.location.back();
      }
    }
  }
}
