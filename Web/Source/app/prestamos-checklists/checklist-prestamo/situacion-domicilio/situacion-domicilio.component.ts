import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { GrupoDomicilioIntegranteResultado } from '../../shared/modelos/grupo-domicilio-integrante-resultado.model';
import { DomicilioIntegrante } from '../../shared/modelos/domicilio-integrante.model';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../../shared/titulo-banco';

@Component({
  selector: 'bg-situacion-domicilio',
  templateUrl: './situacion-domicilio.component.html'
})
export class SituacionDomicilioComponent implements OnInit {
  public form: FormGroup;
  public gruposIntegrantes: GrupoDomicilioIntegranteResultado [] = [];
  public formDomicilio: FormGroup;
  public domicilioIntegrante: DomicilioIntegrante = new DomicilioIntegrante();

  constructor(private fb: FormBuilder,
              private formularioService: FormulariosService,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Situación domicilio del integrante ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.form = this.fb.group({});
    this.formDomicilio = this.fb.group({});
    this.route.params
      .switchMap((params: Params) =>
        this.formularioService.consultarSituacionDomicilioIntegrante(+params['id']))
      .subscribe(
        (res) => {
          this.gruposIntegrantes = res;
        });
    this.route.params
      .switchMap((params: Params) =>
        this.formularioService.obtenerDomicilioIntegrante(+params['id'])).subscribe(
      (dom) => {
        if (dom) {
          this.domicilioIntegrante = dom;
        }
      }
    );
  }

  public cerrar(): void {
    window.close();
  }
}
