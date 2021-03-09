import { ActivatedRoute, Params, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { NotificacionService } from '../../shared/notificacion.service';
import { VerImportacionArchivo } from '../shared/modelo/ver-importacion-archivo.model';
import { Observable } from 'rxjs/Observable';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ELEMENTOS, Pagina } from '../../shared/paginacion/pagina-utils';
import { RecuperoService } from '../shared/recupero.service';
import { InconsistenciaArchivoConsulta } from '../shared/modelo/inconsistencia-archivo-consulta.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-ver-inconsistencia-recupero',
  templateUrl: './ver-inconsistencia-recupero.component.html',
  styleUrls: ['./ver-inconsistencia-recupero.component.scss'],
})

export class VerInconsistenciaRecuperoComponent implements OnInit {
  public elementos: VerImportacionArchivo[] = [];
  public form: FormGroup;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public consulta: InconsistenciaArchivoConsulta;
  public idCabecera: number;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private recuperoService: RecuperoService,
              private router: Router,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver inconsistencias de recupero ' + TituloBanco.TITULO);
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idCabecera = +params['id'];
    });
    this.consulta = new InconsistenciaArchivoConsulta();
    this.crearFormResultado();
    this.configurarPaginacion();
    this.paginaModificada.next();
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        this.consulta.idCabecera = this.idCabecera;
        this.consulta.numeroPagina = params.numeroPagina;
        return this.recuperoService
          .consultarInconsistenciaArchivoRecupero(this.consulta);
      })
      .share();
    (<Observable<VerImportacionArchivo[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.elementos = resultado;
        this.crearFormResultado();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public consultar(pagina?: number) {
    this.paginaModificada.next(pagina);
  }

  private crearFormResultado() {
    this.form = this.fb.group({
      resultados: this.fb.array((this.elementos || []).map((resultado) =>
        this.fb.group({
          nroLinea: [resultado.nroLinea],
          motivoRechazo: [resultado.motivoRechazo]
        })
      )),
    });
  }

  public get resultadoFormArray(): FormArray {
    return this.form.get('resultados') as FormArray;
  }

  public salir() {
    this.router.navigate(['/bandeja-recupero']);
  }
}
