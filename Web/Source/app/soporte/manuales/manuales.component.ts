import { Component, OnInit } from '@angular/core';
import { ApartadoParametroComponent } from './apartado-parametro/apartado-parametro.component';
import { ArchivoService } from '../../shared/archivo.service';
import { ManualesService } from '../manuales.service';
import { ConsultaManual } from '../modelo/consulta-manual.model';
import { NotificacionService } from '../../shared/notificacion.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-manuales',
  templateUrl: './manuales.component.html',
  styleUrls: ['./manuales.component.scss']
})
export class ManualesComponent implements OnInit {
  public manuales: string[] = [];

  constructor(private manualesService: ManualesService,
              private archivoService: ArchivoService,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Manuales de usuario ' + TituloBanco.TITULO);
  }

  public ngOnInit() {
    this.obtenerManuales();
  }

  private obtenerManuales(): void {
    this.manualesService
      .obtenerManuales()
      .subscribe((manuales) => {
        this.manuales = manuales;
      });
  }

  public descargarManual(manual: string): void {
    if (manual) {
      this.manualesService
        .descargarManual(new ConsultaManual(manual))
        .subscribe((archivo) => {
          if (archivo) {
            this.archivoService.descargarArchivo(archivo);
          } else {
            this.notificacionService.informar(['No se encontró el archivo del manual seleccionado'], true);
          }
        });
    }
  }
}
