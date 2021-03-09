import { Component, OnInit } from '@angular/core';
import { LineaService } from '../../shared/servicios/linea-prestamo.service';
import { LineaPrestamo } from '../shared/modelo/linea-prestamo.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { SexoDestinatario } from '../shared/modelo/destinatario-prestamo.model';
import { BajaComando } from '../shared/modelo/baja-comando.model';
import { MotivosBajaService } from '../../shared/servicios/motivosbaja.service';
import { List } from 'lodash';
import { MotivoBaja } from '../../shared/modelo/motivoBaja.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../shared/notificacion.service';
import { IntegranteSocio } from '../shared/modelo/integrante-socio.model';
import { TipoGarantia } from '../shared/modelo/tipo-garantia.model';
import { TipoFinanciamiento } from '../shared/modelo/tipo-financiamiento.model';
import { DetalleLineaPrestamo } from '../shared/modelo/detalle-linea-prestamo.model';
import { MotivoDestino } from '../../motivo-destino/shared/modelo/motivo-destino.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-eliminacion-linea',
  templateUrl: './eliminacion-linea.component.html',
  styleUrls: ['./eliminacion-linea.component.scss'],
  providers: [MotivosBajaService]
})

export class EliminacionLineaComponent implements OnInit {
  public lineaPrestamo: LineaPrestamo = new LineaPrestamo();
  public detallesLineaPrestamo: DetalleLineaPrestamo [] = [];
  public motivosBaja: List<MotivoBaja> = [];
  public form: FormGroup;
  public idLineaUrl: any;
  public fechaActual: Date = new Date();

  constructor(private route: ActivatedRoute,
              private fb: FormBuilder,
              private lineaService: LineaService,
              private motivoBajaService: MotivosBajaService,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Eliminar línea de préstamo ' + TituloBanco.TITULO);
    this.lineaPrestamo.sexoDestinatario = new SexoDestinatario();
    this.lineaPrestamo.motivoDestino = new MotivoDestino();
    this.crearForm();
  }

  public ngOnInit(): void {
    this.buscarIdUrl();
    this.consultarMotivosBaja();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      idMotivoBaja: [null, Validators.required]
    });
  }

  private consultarLinea(): void {
    this.lineaService.consultarLineaPorId(this.idLineaUrl)
      .subscribe((resultado) => {
        this.lineaPrestamo = resultado;
      }, (errores) => {
        if (errores) {
          this.notificacionService.informar(errores, true);
        }
      });
  }

  public hayDetallesDadosBaja(): boolean {
    return this.detallesLineaPrestamo.some((detalle) => !detalle.fechaBaja);
  }

  private consultarDetallesLinea(): void {
    this.lineaService.consultarDetallePorIdLineaSinPaginar(this.idLineaUrl)
      .subscribe((resultado) => {
        this.detallesLineaPrestamo = resultado;
        this.detallesLineaPrestamo.forEach((detalle) => {
          detalle.integranteSocio = new IntegranteSocio(null, detalle.nombreSocioIntegrante);
          detalle.tipoGarantia = new TipoGarantia(null, detalle.nombreTipoGarantia);
          detalle.tipoFinanciamiento = new TipoFinanciamiento(null, detalle.nombreTipoFinanciamiento);
        });
      }, (errores) => {
        if (errores) {
          this.notificacionService.informar(errores, true);
        }
      });
  }

  private buscarIdUrl(): void {
    this.route.params.subscribe((param: Params) => {
      this.idLineaUrl = param['id'];
      this.consultarLinea();
      this.consultarDetallesLinea();
    });
  }

  private consultarMotivosBaja(): void {
    this.motivoBajaService.consultarMotivosBaja()
      .subscribe((motivos) => this.motivosBaja = motivos);
  }

  public darDeBajaLinea(): void {

    let detallesEliminadas = this.detallesLineaPrestamo.filter((detalle) => detalle.fechaBaja);
    if (detallesEliminadas.length !== this.detallesLineaPrestamo.length) {
      this.notificacionService
        .informar(['La línea de préstamo debe tener todos sus detalles dados de baja para ser dada de baja.'], true);
    } else {
      let bajaComando = new BajaComando();
      bajaComando.idLinea = this.idLineaUrl;
      bajaComando.idMotivoBaja = this.form.get('idMotivoBaja').value;

      this.lineaService.darDeBajaLinea(bajaComando)
        .subscribe(() => {
          this.notificacionService
            .informar(['La línea de préstamo se dio de baja con éxito.'])
            .result
            .then(() => {
              this.router.navigate(['/consulta-linea', this.idLineaUrl]);
            });
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
    }
  }
}
