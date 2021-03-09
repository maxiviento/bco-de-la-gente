import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder } from '@angular/forms';
import { PlanDePagoGrillaResultado } from '../../../../../pagos/shared/modelo/plan-de-pago-grilla-resultado.model';
import { GrillaPlanPagosComponent } from '../../../../../pagos/plan-pagos/actualizar-plan-pagos/grilla-plan-pagos/grilla-plan-pagos.component';
import { PagosService } from '../../../../../pagos/shared/pagos.service';
import * as FileSaver from 'file-saver';
import { BehaviorSubject } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { DetallesPlanPagosConsulta } from '../../../../../pagos/shared/modelo/detalles-plan-pagos-consulta.model';

@Component({
  selector: 'bg-modal-plan-pagos',
  templateUrl: './modal-plan-pagos.component.html',
  styleUrls: ['./modal-plan-pagos.component.scss']
})
export class ModalPlanPagosComponent implements OnInit {
  @Input() public planes: PlanDePagoGrillaResultado[] = [];
  @Input() public idsFormularios: number[];
  @ViewChild(GrillaPlanPagosComponent)
  public componenteDetalles: GrillaPlanPagosComponent;
  public excel = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;

  constructor(private fb: FormBuilder,
              private activeModal: NgbActiveModal,
              private sanitizer: DomSanitizer,
              private pagosService: PagosService) {
  }

  public ngOnInit(): void {
      this.componenteDetalles.planes = this.planes;
      this.componenteDetalles.esUnModal = true;
  }

  public cerrar() {
    this.activeModal.close();
  }

  public imprimir() {
    this.pagosService.imprimirPlanCuotas(new DetallesPlanPagosConsulta(this.idsFormularios)).subscribe((res) => {
      this.excel.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
      this.reporteSource = this.excel.getValue();
      let arrayBytes = this.base64ToBytes(res.blob);
      let blob = new Blob([arrayBytes], {type: ''});
      FileSaver.saveAs(blob, res.fileName);
    });
  }

  private base64ToBytes(base64) {
    let raw = window.atob(base64);
    let n = raw.length;
    let bytes = new Uint8Array(new ArrayBuffer(n));

    for (let i = 0; i < n; i++) {
      bytes[i] = raw.charCodeAt(i);
    }
    return bytes;
  }
}
