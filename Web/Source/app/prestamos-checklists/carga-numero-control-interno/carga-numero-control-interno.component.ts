import { Component, OnInit } from '@angular/core';
import { BandejaCargaNumeroControlInterno } from '../shared/modelos/bandeja-carga-numero-control-interno.model';
import { ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CargarNumeroControlInternoComando } from '../shared/modelos/cargar-numero-control-interno-comando.model';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { NotificacionService } from '../../shared/notificacion.service';
import { FormulariosService } from '../../formularios/shared/formularios.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-carga-numero-control.interno',
  templateUrl: './carga-numero-control-interno.component.html',
  styleUrls: ['./carga-numero-control-interno.component.scss'],
  providers: []
})

export class CargaNumeroControlInternoComponent implements OnInit {
  public formulariosControlInterno: BandejaCargaNumeroControlInterno = new BandejaCargaNumeroControlInterno();
  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private formularioService: FormulariosService,
              private route: ActivatedRoute,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Carga de número de control interno ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();

    this.route.params
      .switchMap((params: Params) =>
        this.formularioService.consultarBandejaCargaNumeroControlInterno(+params['id']))
      .subscribe(
        (res) => {
          this.formulariosControlInterno = res;
          this.crearForm();
        });
  }

  private crearForm(): void {
    let controlCambiosSticker = new FormControl(this.formulariosControlInterno.nroSticker,
      [Validators.compose([
        Validators.minLength(1),
        Validators.maxLength(14),
        Validators.required,
        CustomValidators.number])]);

    controlCambiosSticker.valueChanges.distinctUntilChanged()
      .subscribe((value) => {
        if (value.length === 14) {
          this.mandarSticker(false, value.toString());
        }
      });

    this.form = this.fb.group({
      idFomulario: this.formulariosControlInterno.idFormulario,
      numero: this.formulariosControlInterno.numero,
      apellidoNombre: this.formulariosControlInterno.apellidoNombreSolicitante,
      estado: this.formulariosControlInterno.estado,
      esPrestamo: this.formulariosControlInterno.estado,
      nroSticker: controlCambiosSticker,
      cuil: this.formulariosControlInterno.cuilSolicitante
    });

  }

  public mandarSticker(cargaManual: boolean = false, numeroDeSticker?: string): void {

    if (cargaManual) {
      numeroDeSticker = this.form.get('nroSticker').value;
    }
    if (!this.validarNumeros(numeroDeSticker)) {
      return;
    }

    let comando: CargarNumeroControlInternoComando = new CargarNumeroControlInternoComando(this.formulariosControlInterno.idFormulario, numeroDeSticker);
    this.formularioService.guardarNumeroControlInterno(comando)
      .subscribe((resultado) => {
        if (resultado) {
          this.notificacionService.informar(Array.of('Número de sticker registrado con éxito.'), false);
        } else {
          this.notificacionService.informar(Array.of('El número de sticker ingresado ya existe.'), false);
        }
      }, () => {
        this.notificacionService.informar(Array.of('Hubo un error de parte nuestra.'), true);
      });
  }

  public cerrar(): void {
    window.close();
  }

  private validarNumeros(numeroARevisar: string): boolean {
    return /^[0-9]*$/.test(numeroARevisar);
  }

  public deshabilitarRegistro(): boolean {
    return this.form.get('nroSticker').invalid;
  }
}
