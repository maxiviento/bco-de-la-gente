import { ActivatedRoute, Params, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { RecuperoService } from '../shared/recupero.service';
import { Subject } from 'rxjs/Subject';
import { NotificacionService } from '../../shared/notificacion.service';
import { VerImportacionArchivo } from '../shared/modelo/ver-importacion-archivo.model';
import { Observable } from 'rxjs/Observable';
import { InconsistenciaArchivoConsulta } from '../shared/modelo/inconsistencia-archivo-consulta.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-ver-inconsistencia-resultado',
  templateUrl: './ver-inconsistencia-resultado.component.html',
  styleUrls: ['./ver-inconsistencia-resultado.component.scss'],
})

export class VerInconsistenciaResultadoComponent implements OnInit {
  public inconsistencias: VerImportacionArchivo [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consulta: InconsistenciaArchivoConsulta = new InconsistenciaArchivoConsulta();
  public idCabecera: number;

  constructor(private notificacionService: NotificacionService,
              private recuperoService: RecuperoService,
              private router: Router,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver inconsistencias de resultado ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idCabecera = +params['id'];

      this.consulta.idCabecera = this.idCabecera;

      this.configurarPaginacion();
      this.consultar();
    });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.consulta.idCabecera = this.idCabecera;
        this.consulta.numeroPagina = params.numeroPagina;
        return this.recuperoService.consultarInconsistenciaArchivoResultado(this.consulta);
      }).share();
    (<Observable<VerImportacionArchivo[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((inconsistencias) => {
        this.inconsistencias = inconsistencias;
      });
  }

  public consultar(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  public salir() {
    this.router.navigate(['/bandeja-resultado-banco']);
  }
}
