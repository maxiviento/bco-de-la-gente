import { Component, OnInit } from '@angular/core';
import { FormulariosService } from '../shared/formularios.service';
import { LineaFormulario } from '../shared/modelo/linea-formulario.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalSeleccionDetalleComponent } from './modal-seleccion-detalle/modal-seleccion-detalle.component';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-seleccion-linea',
  templateUrl: './seleccion-linea.component.html',
  styleUrls: ['./seleccion-linea.component.scss'],
})
export class SeleccionLineaComponent implements OnInit {
  public lineasDisponibles: LineaFormulario[] = [];

  constructor(private formulariosService: FormulariosService,
              private modalService: NgbModal,
              private titleService: Title) {
    this.titleService.setTitle('Selección de línea ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.crearForm();
  }

  public seleccionarLinea(linea: LineaFormulario) {
    let modalRef = this.modalService.open(ModalSeleccionDetalleComponent, {size: 'lg', backdrop: 'static'});
    modalRef.componentInstance.detallesLinea = linea.detalles;
  }

  private crearForm() {
    this.formulariosService.obtenerDetallesLinea()
      .subscribe((detallesSeleccion) => {
        for (let detalle of detallesSeleccion) {
          let linea = this.lineasDisponibles.find((l) => l.nombre === detalle.nombre);
          if (!linea) {
            linea = new LineaFormulario(detalle.color, detalle.nombre, detalle.descripcion, detalle.objectivo, []);
            this.lineasDisponibles.push(linea);
          }
          linea.detalles.push(detalle);
        }
      });
  }
}
