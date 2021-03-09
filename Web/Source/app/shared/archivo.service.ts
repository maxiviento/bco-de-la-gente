import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import { Archivo } from './modelo/archivo.model';

@Injectable()
export class ArchivoService {

  public descargarArchivos(archivos: Archivo[], nombre?: string) {
    if (nombre) {
      for (let archivo of archivos) {
        this.descargarArchivoNombrado(archivo, nombre);
      }
    } else {
      for (let archivo of archivos) {
        this.descargarArchivo(archivo);
      }
    }
  }

  public descargarArchivo(arch: Archivo) {
    let arrayBytes = this.base64ToBytes(arch.archivo);
    let blob = new Blob([arrayBytes], { type: arch.tipo.type });
    FileSaver.saveAs(blob, arch.nombreConExtension);
  }

  public descargarArchivoNombrado(archivo: Archivo, nombreArchivo: string) {
    let arrayBytes = this.base64ToBytes(archivo.archivo);
    let blob = new Blob([arrayBytes], { type: archivo.tipo.type });
    FileSaver.saveAs(blob, archivo.nombre.concat('_', nombreArchivo));
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

  public getUrlPrevisualizacionArchivo(arch: Archivo): string{
    return 'data:' + arch.tipo.type + ';base64,' + arch.archivo;
  }

  public getUrlPrevisualizacionTxt(arch: any, base64: any): string{
    return 'data:' + 'text/plain' + ';base64,' + base64;
  }

  public getUrlPrevisualizacion(archivoEnBase64: string, tipoArchivo: string): string{
    return 'data:' + tipoArchivo +';base64,' + archivoEnBase64;
  }

}
