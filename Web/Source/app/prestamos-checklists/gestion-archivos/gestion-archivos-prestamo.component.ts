import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { PrestamoService } from '../../shared/servicios/prestamo.service';
import { IntegrantePrestamo } from '../../shared/modelo/integrante-prestamo.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Etapa } from '../../etapas/shared/modelo/etapa.model';
import { RequisitoPrestamo } from '../shared/modelos/requisito-prestamo';
import { EtapasService } from '../../etapas/shared/etapas.service';
import { ModalArchivoComponent } from '../../shared/modal-archivo/modal-archivo.component';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { EncabezadoPrestamo } from '../shared/modelos/encabezado-prestamo.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-gestion-archivos-prestamo',
  templateUrl: './gestion-archivos-prestamo.component.html',
  styleUrls: ['./gestion-archivos-prestamo.component.scss']
})

export class GestionArchivosPrestamoComponent implements OnInit {

  public form: FormGroup;
  public integrantesPrestamo: IntegrantePrestamo [] = [];
  public idPrestamo: number;
  public etapasPrestamo: Etapa [] = [];
  public requisitosPrestamo: RequisitoPrestamo [] = [];
  public encabezadoPrestamo: EncabezadoPrestamo = new EncabezadoPrestamo();

  constructor(private prestamoService: PrestamoService,
              private etapasService: EtapasService,
              private route: ActivatedRoute,
              private modalService: NgbModal,
              private router: Router,
              private fb: FormBuilder,
              private titleService: Title) {
    this.titleService.setTitle('Gestión de archivos del préstamo ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params) => {
        this.crearForm();
        this.idPrestamo = params.id;
        this.prestamoService.obtenerIdPrestamo(this.idPrestamo).subscribe((res) => {
          this.prestamoService.consultarIntegrantes(res)
            .subscribe((integrantes) => {
              this.integrantesPrestamo = integrantes;
            });
          this.prestamoService.consultarRequisitos(res)
            .subscribe((requisitos) => {
              this.requisitosPrestamo = requisitos;
              this.requisitosPrestamo.forEach((requisito) => {
                requisito.urlRecurso = this.setIdPrestamoEnUrl(requisito.urlRecurso);
              });
              this.etapasService.consultarEtapasPorPrestamo(res)
                .subscribe((etapas) => {
                  this.etapasPrestamo = etapas;
                  this.buscarItemsPadreConGestionArchivos();
                  this.buscarEtapasConGestionArchivos();
                });
            });
          this.prestamoService.consultarEncabezadoPrestamo(this.idPrestamo)
            .subscribe((encabezado) => {
              this.encabezadoPrestamo = encabezado;
            });
        });
      }
    );
  }

  private crearForm(): void {
    this.form = this.fb.group({});
  }

  private setIdPrestamoEnUrl(urlRecurso: string): string {
    if (urlRecurso && urlRecurso.indexOf(':id') > 0) {
      urlRecurso = '#' + urlRecurso;
      return urlRecurso.replace(/:id/gi, this.idPrestamo.toString());
    }
    return urlRecurso;
  }

  public calcularEdad(fecha: Date): number {
    return NgbUtils.calcularEdad(fecha);
  }

  public gestionaArchivos(requisito: RequisitoPrestamo): boolean {
    return requisito.subeArchivo || requisito.generaArchivo;
  }

  public buscarItemsPadreConGestionArchivos() {
    this.requisitosPrestamo.forEach((itemPadre) => {
      if (!itemPadre.itemPadre) {
        let item2 = this.requisitosPrestamo.find((itemHijo) => {
          return (itemHijo.itemPadre == itemPadre.id && this.gestionaArchivos(itemHijo));
        });
        if (item2) {
          itemPadre.gestionaArchivo = true;
        }
      }
    });
  }

  public buscarEtapasConGestionArchivos() {
    this.etapasPrestamo.forEach((etapa) => {
      this.requisitosPrestamo.forEach((item) => {
        if (item.idEtapa == etapa.id && this.gestionaArchivos(item)) {
          etapa.gestionaArchivos = true;
        }
      });
    });
  }

  public cancelar() {
    this.router.navigate(['/bandeja-prestamos']);
  }

  public abrirModalArchivo(item: any): void {
    let options: NgbModalOptions = {
      size: 'lg',
      windowClass: 'modal_archivo'
    };

    const modalRef = this.modalService.open(ModalArchivoComponent, options);
    modalRef.componentInstance.item = item;
    modalRef.componentInstance.idFormularioLinea = this.idPrestamo;
    modalRef.componentInstance.soloHistorial = false;
  }

}
