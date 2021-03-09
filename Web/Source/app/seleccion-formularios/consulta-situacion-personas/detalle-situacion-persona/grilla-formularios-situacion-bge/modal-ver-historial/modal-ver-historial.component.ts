import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { SeguimientoPrestamo } from '../../../../../prestamos-checklists/shared/modelos/seguimiento-prestamo';
import { ELEMENTOS, Pagina } from '../../../../../shared/paginacion/pagina-utils';
import { ConsultaSeguimientosPrestamo } from '../../../../../prestamos-checklists/shared/modelos/consulta-seguimientos';
import { PrestamoService } from '../../../../../shared/servicios/prestamo.service';

@Component({
  selector: 'bg-modal-ver-historial',
  templateUrl: './modal-ver-historial.component.html',
  styleUrls: ['./modal-ver-historial.component.scss']
})
export class ModalVerHistorialComponent implements OnInit {
  @Input() public idPrestamo: number;
  public seguimientos: SeguimientoPrestamo [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consultaSeguimientos: ConsultaSeguimientosPrestamo = new ConsultaSeguimientosPrestamo();

  constructor(private fb: FormBuilder,
              private activeModal: NgbActiveModal,
              private prestamoService: PrestamoService) {
  }

  public cerrar() {
    this.activeModal.close();
  }

  public ngOnInit(): void {
    this.configurarPaginacion();
    this.consultarSeguimientos();
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.consultaSeguimientos.idPrestamo = this.idPrestamo;
        this.consultaSeguimientos.numeroPagina = params.numeroPagina;
        return this.prestamoService.consultarSeguimientoPrestamo(this.consultaSeguimientos);
      }).share();
    (<Observable<SeguimientoPrestamo[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((seguimientos) => {
        this.seguimientos = seguimientos;
      });
  }

  public consultarSeguimientos(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }
}
