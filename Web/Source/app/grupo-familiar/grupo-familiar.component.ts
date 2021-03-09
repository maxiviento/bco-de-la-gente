import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../shared/forms/custom-validators';
import AccionGrupoUnico from '../grupo-unico/shared-grupo-unico/accion-grupo-unico.enum';
import { SexoService } from '../shared/servicios/sexo.service';
import { PaisService } from '../shared/servicios/pais.service';
import { Sexo } from '../shared/modelo/sexo.model';
import { Pais } from '../shared/modelo/pais.model';
import { ContenidoNuevaPersonaComponent } from '../formularios/contenido-nueva-persona/contenido-nueva-persona.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContenidoEditarPersonaComponent } from '../formularios/contenido-editar-persona/contenido-editar-persona.component';
import { GrupoUnicoConsulta } from '../formularios/shared/modelo/grupo-unico-consulta';
import { ModalDatosConctactoComponent } from '../shared/modal-datos-contacto/modal-datos-conctacto.component';
import { PersonaService } from '../formularios/cuadrantes/persona/persona.service';
import { NotificacionService } from '../shared/notificacion.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../shared/titulo-banco';

@Component({
  selector: 'grupo-familiar',
  templateUrl: './grupo-familiar.component.html',
  styleUrls: ['./grupo-familiar.component.scss']
})
export class GrupoFamiliarComponent implements OnInit {
  public form: FormGroup;
  public generos: Sexo[];
  public paises: Pais[];
  public urlGrupoFamiliar: AccionGrupoUnico = AccionGrupoUnico.GRUPO_FAMILIAR_MODIFICACION_INTERNA;
  public consultado = false;
  public persona: any;
  public grupoUnicoConsulta: GrupoUnicoConsulta;
  @Output()
  public emitirGrupoUnico: EventEmitter<GrupoUnicoConsulta> = new EventEmitter<GrupoUnicoConsulta>();

  constructor(private fb: FormBuilder,
              private sexoService: SexoService,
              private  paisService: PaisService,
              private modalService: NgbModal,
              private personaService: PersonaService,
              private notificacionService: NotificacionService,
              private titleService: Title) {
    this.titleService.setTitle('Modificar grupo familiar ' + TituloBanco.TITULO);
  }

  public ngOnInit(): void {
    this.crearForm();
    this.obtenerGenero();
    this.obtenerPais();
  }

  public crearForm() {
    this.form = this.fb.group({
      paisBeneficiario: ['', Validators.required],
      sexoBeneficiario: ['', Validators.required],
      documento: ['', Validators.compose(
        [Validators.maxLength(10),
          Validators.maxLength(8), CustomValidators.number, Validators.required])]
    });
  }

  get paisBeneficiario(): string {
    return this.form.get('paisBeneficiario').value;
  }

  get sexoBeneficiario(): string {
    return this.form.get('sexoBeneficiario').value;
  }

  get documento(): string {
    return this.form.get('documento').value;
  }

  public obtenerPais(): void {
    this.paisService.obtenerPaises()
      .subscribe((paises) => {
        this.paises = paises;
        let argentina = paises.find((p) => {
          return p.descripcion === 'ARGENTINA';
        });
        this.form.get('paisBeneficiario').setValue(argentina.id);
        this.form.get('paisBeneficiario').updateValueAndValidity();
      });
  }

  public obtenerGenero(): void {
    this.sexoService.obtenerSexos()
      .subscribe((sexos) => {
        this.generos = sexos;
      });
  }

  public registrarPersona() {
    const modalNuevaPersona = this.modalService.open(ContenidoNuevaPersonaComponent, {
      backdrop: 'static',
      size: 'lg',
      keyboard: false
    });
    this.extraerDatosConsulta();
    modalNuevaPersona.componentInstance.persona = this.persona;
    modalNuevaPersona.result.then(() => {
      this.consultado = false;
    });
  }

  public editarPersona() {
    const modalNuevaPersona = this.modalService.open(ContenidoEditarPersonaComponent, {
      backdrop: 'static',
      size: 'lg',
      keyboard: false
    });
    this.extraerDatosConsulta();
    modalNuevaPersona.componentInstance.persona = this.persona;
  }

  private extraerDatosConsulta(): void {
    let formModel = this.form.value;
    this.persona = {
      nroDocumento: formModel.documento,
      codigoPais: formModel.paisBeneficiario,
      sexoId: formModel.sexoBeneficiario,
    };
  }

  public consultarPersona() {
    if (this.form.valid) {
      this.consultado = true;
      this.grupoUnicoConsulta = new GrupoUnicoConsulta(this.urlGrupoFamiliar, this.sexoBeneficiario, this.documento, this.paisBeneficiario, 800);
      this.emitirGrupoUnico.emit(this.grupoUnicoConsulta);
    }
  }

  public editarDatosContacto() {
    if (this.form.valid) {
      this.extraerDatosConsulta();
      this.personaService.consultarPersona(this.persona).subscribe(
        (resultado) => {
          if (resultado) {
            const modalDatosContacto = this.modalService.open(ModalDatosConctactoComponent, {
              backdrop: 'static',
              size: 'lg',
              keyboard: false
            });
            modalDatosContacto.componentInstance.persona = resultado;
            modalDatosContacto.componentInstance.editable = true;
          }
        },
        (error) => {
          if (error) {
            this.notificacionService.informar(['La persona ingresada no existe.']);
          }
        });
    }
  }
}
