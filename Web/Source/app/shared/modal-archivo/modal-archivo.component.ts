import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificacionService } from '../notificacion.service';
import { Documento } from '../modelo/documento.model';
import { DocumentoService } from '../servicios/documento.service';
import { Documentacion } from '../modelo/documentacion.model';
import { DocumentoConsulta } from '../modelo/consultas/documento-consulta.model';
import * as FileSaver from 'file-saver';
import { ELEMENTOS, Pagina } from '../paginacion/pagina-utils';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'bg-modal-archivo',
  templateUrl: './modal-archivo.component.html',
  styleUrls: ['./modal-archivo.component.scss'],
})

export class ModalArchivoComponent implements OnInit {
  @Input() public item: any;
  @Input() public idFormularioLinea: number;
  @Input() public soloHistorial: boolean;

  public documentacion: Documentacion = new Documentacion();
  public documento: any;
  public maxSize: number = 4;
  public documentos: Documento[] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();

  constructor(public activeModal: NgbActiveModal,
              private notificacionService: NotificacionService,
              private documentoService: DocumentoService) {
  }

  public ngOnInit(): void {
    this.armarDocumentacion();
    this.configurarPaginacion();
    this.consultarDocumentacion();
  }

  public armarDocumentacion() {
    this.documentacion = new Documentacion();
    this.documentacion.idItem = this.item.idItem;
    this.documentacion.idFormularioLinea = this.idFormularioLinea;
  }

  public cancelar(): void {
    this.activeModal.close();
  }

  public archivoSeleccionado(archivo: File): void {
    this.documentacion.documento = archivo;
  }

  public guardar(): void {
    this.documentoService.guardarDocumento(this.documentacion).subscribe(
      (response) => {
        this.notificacionService.informar(["El archivo se subió correctamente."], false)
          .result
          .then(() => {
            this.consultarDocumentacion();
            this.armarDocumentacion();
          });
      },
      (errores) => {
        this.notificacionService.informar(errores, true);
      }
    );
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

  public descargarArchivo(idDocumentacionCdd: number, idItem: number, nombreArchivo?: string) {
    this.documentoService
      .consultarDocumento(idDocumentacionCdd, idItem)
      .subscribe((res) => {
          let arrayBytes = this.base64ToBytes(res.blob);
          let blob = new Blob([arrayBytes], {type: res.extension});
          FileSaver.saveAs(blob, res.fileName == "" ? nombreArchivo : res.fileName);
          this.armarDocumentacion();
        },
        (errores) => this.notificacionService.informar(errores, true));
  }

  public consultarDocumentacion(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = new DocumentoConsulta(this.item.idItem, this.idFormularioLinea);
        filtros.numeroPagina = params.numeroPagina;
        return this.documentoService
          .consultarHistorialArchivos(filtros);
      })
      .share();
    (<Observable<[Documento]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((documentos) => {
        this.documentos = documentos;
      });
  }
}
