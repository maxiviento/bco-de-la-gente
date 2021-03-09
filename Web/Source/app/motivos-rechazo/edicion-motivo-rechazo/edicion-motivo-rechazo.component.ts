import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MotivoRechazo } from '../../formularios/shared/modelo/motivo-rechazo';
import { MotivosRechazoService } from '../shared/motivos-rechazo.service';
import { NotificacionService } from '../../shared/notificacion.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ApartadoMotivoRechazoComponent } from '../shared/apartado-motivo-rechazo/apartado-motivo-rechazo.component';
import { Ambito } from '../shared/modelo/ambito.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';
import { ModificarMotivoRechazoComando } from '../../formularios/shared/modelo/modificar-motivo-rechazo-comando.model';

@Component({
    selector: 'bg-edicion-motivo-rechazo',
    templateUrl: './edicion-motivo-rechazo.component.html',
    styleUrls: ['./edicion-motivo-rechazo.component.scss'],
})

export class EdicionMotivoRechazoComponent implements OnInit {
    public form: FormGroup;
    public motivoRechazo: MotivoRechazo = new MotivoRechazo();
    public ambitos: Ambito[];

    constructor(private motivoRechazoService: MotivosRechazoService,
                private fb: FormBuilder,
                private notificacionService: NotificacionService,
                private router: Router,
                private route: ActivatedRoute,
                private titleService: Title) {
        this.titleService.setTitle('Editar motivo de rechazo ' + TituloBanco.TITULO);
    }

    public ngOnInit(): void {
        this.crearForm();
        this.consultarAmbitos();
        this.route.params
            .switchMap((params: Params) => this.motivoRechazoService.consultarMotivoRechazo(+params['idMotivo'], +params['idAmbito']))
            .subscribe((motivo) => {
                this.motivoRechazo = motivo;
                this.crearForm();
            });
    }

    public crearForm(): void {
        this.form = this.fb.group({
            apartadoMotivoRechazo: ApartadoMotivoRechazoComponent.nuevoFormGroup(this.motivoRechazo)
        });
    }

    public get apartadoMotivoRechazo(): FormGroup {
        return this.form.get('apartadoMotivoRechazo') as FormGroup;
    }

    public guardar(): void {
        let motivo = ApartadoMotivoRechazoComponent.prepararForm(this.apartadoMotivoRechazo);
        let seCambioCodigo: boolean = this.motivoRechazo.codigo !== motivo.codigo;
        if (seCambioCodigo && motivo.codigo) {
            this.motivoRechazoService.verificarCodigoExistente(motivo.ambito.id, motivo.codigo)
                .subscribe((yaExiste) => {
                    if (yaExiste) {
                        this.notificacionService.informar(
                            [`El código "${motivo.codigo}" ya existe para el ámbito seleccionado.`],
                            true, 'Advertencia');
                    } else {
                        this.verificarDatosRestantes(motivo);
                    }
                });
        } else {
            this.verificarDatosRestantes(motivo);
        }
    }

    public verificarDatosRestantes(motivo: MotivoRechazo): void {
        let seCambioElNombre: boolean = this.motivoRechazo.nombre !== motivo.nombre;
        let seCambioAbreviatura: boolean = this.motivoRechazo.abreviatura !== motivo.abreviatura;

        if (seCambioElNombre && seCambioAbreviatura && motivo.abreviatura) {
            this.verificarAmbos(motivo);
            return;
        } else {
            if (seCambioAbreviatura && motivo.abreviatura) {
                this.verificarAbreviatura(motivo);
                return;
            }
            if (seCambioElNombre) {
                this.verificarNombre(motivo);
                return;
            }
        }
        this.editarMotivoRechazo(motivo);
    }

    private verificarAbreviatura(motivo: MotivoRechazo): void {
        this.motivoRechazoService.verificarAbreviaturaExistente(motivo.ambito.id, motivo.abreviatura)
            .subscribe((yaExiste) => {
                if (yaExiste) {
                    this.notificacionService.informar(
                        [`La abreviatura "${motivo.abreviatura}" ya existe para el ámbito seleccionado.`],
                        true, 'Advertencia');
                } else {
                    this.editarMotivoRechazo(motivo);
                }
            });
    }

    private verificarNombre(motivo: MotivoRechazo): void {
        this.motivoRechazoService.verificarNombreExistente(motivo.ambito.id, motivo.nombre)
            .subscribe((yaExiste) => {
                if (yaExiste) {
                    this.notificacionService.informar(
                        [`La abreviatura "${motivo.abreviatura}" ya existe para el ámbito seleccionado.`],
                        true, 'Advertencia');
                } else {
                    this.editarMotivoRechazo(motivo);
                }
            });
    }

    private verificarAmbos(motivo: MotivoRechazo): void {
        let mensajes: string[] = [];

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
                                mensajes.length ?
                                    this.notificacionService.informar(mensajes, true, 'Advertencia') :
                                    this.editarMotivoRechazo(motivo);
                            }
                        );
                });
    }

    public editarMotivoRechazo(motivo: MotivoRechazo): void {
        let motivoComando = this.crearComando(motivo);
        this.motivoRechazoService.editarMotivoRechazo(motivoComando)
            .subscribe((res) => {
                if (res) {
                    this.notificacionService.informar(['El motivo de rechazo se modificó con éxito.'])
                        .result
                        .then(() => this.router.navigate(['/consulta-motivo-rechazo', motivo.id, motivo.ambito.id]));
                } else {
                    this.notificacionService.informar(['Ocurrio un error al intentar modificar el motivo de rechazo.']);
                }
            }, (errores) => this.notificacionService.informar(errores, true));
    }

    private consultarAmbitos(): void {
        this.motivoRechazoService.consultarAmbitos()
            .subscribe((ambitos) => this.ambitos = ambitos);
    }

    public crearComando(motivoNuevo: MotivoRechazo): ModificarMotivoRechazoComando {
        let comando = new ModificarMotivoRechazoComando();
        comando.id = this.motivoRechazo.id;
        comando.nombreNuevo = motivoNuevo.nombre;
        comando.descripcionNueva = motivoNuevo.descripcion;
        comando.codigoNuevo = motivoNuevo.codigo;
        comando.abreviaturaNueva = motivoNuevo.abreviatura;
        comando.nombreOriginal = this.motivoRechazo.nombre;
        comando.descripcionOriginal = this.motivoRechazo.descripcion;
        comando.codigoOriginal = this.motivoRechazo.codigo;
        comando.abreviaturaOriginal = this.motivoRechazo.abreviatura;
        return comando;
    }
}
