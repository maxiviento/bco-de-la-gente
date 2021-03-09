import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MotivoRechazo } from '../../formularios/shared/modelo/motivo-rechazo';
import { MotivosRechazoService } from '../shared/motivos-rechazo.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { Router } from '@angular/router';
import { ApartadoMotivoRechazoComponent } from '../shared/apartado-motivo-rechazo/apartado-motivo-rechazo.component';
import { Ambito } from '../shared/modelo/ambito.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-nuevo-motivo-rechazo',
  templateUrl: './nuevo-motivo-rechazo.component.html',
  styleUrls: ['./nuevo-motivo-rechazo.component.scss'],
})

export class NuevoMotivoRechazoComponent implements OnInit {
  public form: FormGroup;
  public motivoRechazo: MotivoRechazo = new MotivoRechazo();
  public ambitos: Ambito[];

  constructor(private motivoRechazoService: MotivosRechazoService,
              private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private router: Router,
              private titleService: Title) {
    this.titleService.setTitle('Nuevo motivo de rechazo ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarAmbitos();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      apartadoMotivoRechazo: ApartadoMotivoRechazoComponent.nuevoFormGroup(this.motivoRechazo)
    });
  }

  public get apartadoMotivoRechazo(): FormGroup {
    return this.form.get('apartadoMotivoRechazo') as FormGroup;
  }

  private verificarYRegistrar(): void {
    let mensajes: string[] = [];
    let motivo = ApartadoMotivoRechazoComponent.prepararForm(this.apartadoMotivoRechazo);

    this.motivoRechazoService.verificarAbreviaturaExistente(motivo.ambito.id, motivo.abreviatura)
      .subscribe((respuesta) => {
          if (respuesta) {
            mensajes.push(`La abreviatura "${motivo.abreviatura}" ya existe para el ámbito seleccionado.`);
          }
        }, () => {
        },
        () => {
          this.motivoRechazoService.verificarNombreExistente(motivo.ambito.id, motivo.nombre)
            .subscribe((respuesta) => {
                if (respuesta) {
                  mensajes.push(`El nombre "${motivo.nombre}" ya existe para este el ámbito seleccionado.`);
                }
              }, () => {
              },
              () => {
                if (motivo.codigo) {
                  this.motivoRechazoService.verificarCodigoExistente(motivo.ambito.id, motivo.codigo)
                    .subscribe((respuesta) => {
                        if (respuesta) {
                          mensajes.push(`El código "${motivo.codigo}" ya existe para este el ámbito seleccionado.`);
                        }
                      }, () => {
                      },
                      () => {
                        mensajes.length ?
                          this.notificacionService.informar(mensajes, true, 'Advertencia') :
                          this.registrarMotivoRechazo(motivo);
                      });
                } else {
                  mensajes.length ?
                    this.notificacionService.informar(mensajes, true, 'Advertencia') :
                    this.registrarMotivoRechazo(motivo);
                }
              });
        }
      );
  }

  public registrarMotivoRechazo(motivo: MotivoRechazo): void {
    this.motivoRechazoService.registrarMotivoRechazo(motivo)
      .subscribe((res) => {
        this.notificacionService.informar(['El motivo de rechazo se registró con éxito.'])
          .result
          .then(() => this.router.navigate(['/consulta-motivo-rechazo', res.id, motivo.ambito.id]));
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  private consultarAmbitos(): void {
    this.motivoRechazoService.consultarAmbitos()
      .subscribe((ambitos) => this.ambitos = ambitos);
  }
}
