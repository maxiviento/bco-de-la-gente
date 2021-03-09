import { Component, OnInit, Input } from '@angular/core';
import { Parametro } from '../../modelo/parametro.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ParametroService } from '../../parametro.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ControlMessagesComponent } from '../../../shared/forms/control-messages.component';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { NgbUtils } from "../../../shared/ngb/ngb-utils";
import { DateUtils } from "../../../shared/date-utils";
import { VigenciaExistente } from '../../modelo/vigencia-existente.model';

@Component({
    selector: 'cce-apartado-parametro',
    templateUrl: './apartado-parametro.component.html',
    providers: [ParametroService]
})

export class ApartadoParametroComponent implements OnInit {
  public accion: string = 'Modificar parámetro';
  @Input() public parametro: Parametro;
  public form: FormGroup;

  constructor(private parametroService: ParametroService,
    private notificacionService: NotificacionService,
    private activeModal: NgbActiveModal) {
    this.parametro = new Parametro();
  }

  public ngOnInit() {
    this.parametro.configurarNuevaVigencia();
    this.crearForm();
  }

  public crearForm(): void {
    let controlValor: FormControl;
    if (typeof (this.parametro.valorConfigurado) === 'number') {
      controlValor = new FormControl(
        this.parametro.valorConfigurado,
        Validators.compose([Validators.required,
        CustomValidators.number,
        Validators.maxLength(50)]));
    } else if (this.parametro.valorConfigurado instanceof Date) {
      controlValor = new FormControl(
        NgbUtils.obtenerNgbDateStruct(this.parametro.valorConfigurado),
        Validators.compose([Validators.required]));
    } else {
      controlValor = new FormControl(
        this.parametro.valorConfigurado,
        Validators.compose([Validators.required,
        Validators.maxLength(50)]));
    }
    this.form = new FormGroup({
      valor: controlValor,
      fechaDesde: new FormControl(
        NgbUtils.obtenerNgbDateStruct(DateUtils.getManianaDate()),
        Validators.compose(
          [Validators.required,
          CustomValidators.minDate(DateUtils.getManianaDate())]))
    });
  }

  public registrarOModificar() {
    if (this.form.valid) {
      let formModel = this.form.value;
      let parametro = this.parametro.clone();
      parametro.fechaDesde = NgbUtils.obtenerDate(formModel.fechaDesde);

      let valor = formModel.valor;
      if (this.parametro.idTipoDato == 1) {
        valor = NgbUtils.obtenerStringDate(NgbUtils.obtenerDate(valor));
      } else if (this.parametro.idTipoDato == 3) {
        valor = Parametro.obtenerBooleanDB(valor);
      }
      parametro.valor = valor;

      this.modificarParametro(parametro);
    } else {
      ControlMessagesComponent.validarFormulario(this.form);
    }
  }

  public modificarParametro(parametro: Parametro) {
    this.parametroService
      .existeVigencia(parametro)
      .subscribe((vigencia) => {
        if (vigencia.idVigencia) {
          parametro.idVigencia = vigencia.idVigencia;
          this.parametroService
            .actualizarVigencia(parametro)
            .subscribe(
              () => {
                this.notificacionService
                  .informar(['El parámetro se actualizó con éxito.']);
                this.activeModal.close(parametro);
              },
              (errores) => {
                this.notificacionService
                  .informar(<string[]>errores, true);
              });
        }
        else {
          this.parametroService
            .modificarParametro(parametro)
            .subscribe(
              () => {
                this.notificacionService
                  .informar(['El parámetro se modificó con éxito.']);
                this.activeModal.close(parametro);
              },
              (errores) => {
                this.notificacionService
                  .informar(<string[]>errores, true);
              });
        }
      },
        (errores) => {
          this.notificacionService
            .informar(<string[]>errores, true);
        });
  }

  public cancelar() {
    this.activeModal.close();
  }
  public validarformulario(): boolean {
    return this.form.invalid && this.form.dirty;
  }
}
