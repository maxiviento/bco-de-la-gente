import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Persona } from '../../../shared/modelo/persona.model';
import { PersonaService } from './persona.service';
import { Sexo } from '../../../shared/modelo/sexo.model';
import { Pais } from '../../../shared/modelo/pais.model';
import { NotificacionService } from '../../../shared/notificacion.service';
import { SexoService } from '../../../shared/servicios/sexo.service';
import { PaisService } from '../../../shared/servicios/pais.service';
import { CustomValidators } from '../../../shared/forms/custom-validators';
import { ContenidoNuevaPersonaComponent } from '../../contenido-nueva-persona/contenido-nueva-persona.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContenidoEditarPersonaComponent } from '../../contenido-editar-persona/contenido-editar-persona.component';

@Component({
  selector: 'bg-busqueda-persona',
  templateUrl: './persona.component.html',
  styleUrls: ['./persona.component.scss'],
  providers: [PersonaService],
})

export class BusquedaPersonaComponent implements OnInit {
  @Output()
  public personaConsultada: EventEmitter<Persona> = new EventEmitter<Persona>();
  @Input()
  public editable: boolean;
  @Input()
  public sexoId: string;
  @Input()
  public codigoPais: string = null;
  @Input()
  public nroDocumento: string = '';
  @Input()
  public class: string = '';
  @Input()
  public modal: boolean = false;
  @Input()
  public registraPersona: boolean = false;


  public form: FormGroup;
  public persona: any;
  public sexos: Sexo[];
  public paises: Pais[];

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private personaService: PersonaService,
              private sexoService: SexoService,
              private  paisService: PaisService,
              private modalService: NgbModal) {
  }

  public ngOnInit(): void {
    this.obtenerSexos();
    this.obtenerPaises();
    this.crearForm();
  }

  public crearForm(): void {
    this.form = this.fb.group({
      sexoId: new FormControl(
        this.sexoId,
        Validators.required),
      codigoPais: new FormControl(
        this.codigoPais || 'ARG',
        Validators.required),
      nroDocumento: new FormControl(
        this.nroDocumento,
        Validators.compose([
          CustomValidators.number,
          Validators.maxLength(12),
          Validators.required])),
    });
    this.consultarPersona();
  }

  public registrarPersona() {
    const modalNuevaPersona = this.modalService.open(ContenidoNuevaPersonaComponent, {backdrop: 'static', size: 'lg'});
    this.extraerDatosConsulta();
    modalNuevaPersona.componentInstance.persona = this.persona;
  }

  public editarPersona() {
    const modalNuevaPersona = this.modalService.open(ContenidoEditarPersonaComponent, {backdrop: 'static', size: 'lg'});
    this.extraerDatosConsulta();
    modalNuevaPersona.componentInstance.persona = this.persona;
    modalNuevaPersona.result.then((x) => this.consultarPersona());
  }

  public consultarPersona() {
    if (
      this.form.get('sexoId').value !== null
      && this.form.get('nroDocumento').value !== ''
      && this.form.get('codigoPais').value !== null) {
      this.extraerDatosConsulta();
      this.personaService.consultarPersona(this.persona).subscribe(
        (resultado) => {
          if (resultado) {
            this.persona = resultado;
            this.enviarPersona();
          } else {
            this.notificacionService.informar(['No se encontro el solicitante']);
          }
        },
        (errores) => {
          this.notificacionService.informar(<string[]>errores, true);
        });
    }
  }

  private enviarPersona(): void {
    this.personaConsultada.emit(this.persona);
  }

  private obtenerSexos() {
    this.sexoService.obtenerSexos()
      .subscribe((sexos) => {
        this.sexos = sexos;
      });
  }

  private obtenerPaises() {
    this.paisService.obtenerPaises()
      .subscribe((paises) => {
        this.paises = paises;
      });
  }

  private extraerDatosConsulta(): void {
    let formModel = this.form.value;
    this.persona = {
      nroDocumento: formModel.nroDocumento,
      codigoPais: formModel.codigoPais,
      sexoId: formModel.sexoId,
    };
  }

  public limpiarForm(): void {
    this.nroDocumento = '';
    this.sexoId = null;
    this.form = this.fb.group({
      sexoId: new FormControl(
        this.sexoId,
        Validators.required),
      codigoPais: new FormControl(
        this.codigoPais || 'ARG',
        Validators.required),
      nroDocumento: new FormControl(
        this.nroDocumento,
        Validators.compose([
          CustomValidators.number,
          Validators.maxLength(12),
          Validators.required])),
    });
  }
}
