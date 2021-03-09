import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { SituacionPersonasResultado } from '../../shared/modelos/situacion-personas-resultado.model';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { ConsultaSituacionPersonas } from '../../shared/modelos/consulta-situacion-personas.model';
import { SituacionPersonaService } from '../../shared/situacion-persona.service';
import { SituacionPersonasResultadoVista } from '../../shared/modelos/situacion-personas-resultado-vista.model';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { Persona } from '../../../shared/modelo/persona.model';
import { ContactoService } from '../../../shared/servicios/datos-contacto.service';

@Component({
  selector: 'bg-grilla-personas-situacion-bge',
  templateUrl: './grilla-personas-situacion-bge.component.html',
  styleUrls: ['./grilla-personas-situacion-bge.component.scss'],
  providers: [SituacionPersonaService]
})

export class GrillaPersonasSituacionBgeComponent implements OnInit, OnChanges {
  private _filtros: ConsultaSituacionPersonas;
  @Output() public emitPersonaSeleccionada: EventEmitter<SituacionPersonasResultado> = new EventEmitter<SituacionPersonasResultado>();
  public personas: SituacionPersonasResultado [] = [];
  public personasVista: SituacionPersonasResultadoVista [] = [];
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public form: FormGroup;
  public formVista: FormGroup;
  public personaSeleccionada: SituacionPersonasResultado;
  public usaVista: boolean = false;

  @Input()
  public set filtros(filtros: ConsultaSituacionPersonas) {
    this._filtros = filtros;
    this.configurarPaginacion();
  }

  constructor(private fb: FormBuilder,
              private situacionPersonaService: SituacionPersonaService,
              private formularioService: FormulariosService,
              private contactoService: ContactoService) {
  }

  public ngOnInit() {
    this.crearForm();
  }

  public ngOnChanges() {
    if (this._filtros) {
      this.usaVista = false;
      this.personasVista = [];
      this.personas = [];
      if (this._filtros.dni) {
        let p = new Persona();
        p.idNumero = this._filtros.tipoPersona;
        p.nroDocumento = this._filtros.dni;
        this.formularioService.existeDeudaHistorica(p).subscribe((result) => {
          if (result) {
            this.usaVista = true;
            this.emitPersonaSeleccionada.emit(null);
            let consulta = new ConsultaSituacionPersonas();
            consulta.dni = this._filtros.dni;
            this.obtenerVistaPersonas(consulta);
          }
        });
      }
      this.emitPersonaSeleccionada.emit(null);
      this.paginaModificada.next(0);
    }
  }

  private crearForm() {
    this.form = this.fb.group({
      personas: this.fb.array((this.personas || []).map((persona) => {
          let personaFc = new FormControl(persona);
          return this.fb.group({
            persona: personaFc,
            nombre: [persona.nombre],
            apellido: [persona.apellido],
            sexo: [persona.nombreSexo],
            cuil: [persona.cuil],
            dni: [persona.nroDocumento],
            departamento: [persona.departamento],
            localidad: [persona.localidad],
          });
        }
      ))
    });
    this.formVista = this.fb.group({
      personas: this.fb.array((this.personasVista || []).map((persona) => {
          let personaFc = new FormControl(persona);
          return this.fb.group({
            persona: personaFc,
            nombreApellido: [persona.nombreApellido],
            sexo: [persona.sexo],
            dni: [persona.numeroDocumento],
            numeroSolicitud: [persona.numeroSolicitud],
            fechaPago: [persona.fechaPago],
            producto: [persona.producto],
            condicion: [persona.condicion],
            montoAdeudado: [persona.montoAdeudado],
            montoCredito: [persona.montoCredito],
            montoAbonado: [persona.montoAbonado]
          });
        }
      )),
    });
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      }).flatMap((params: { numeroPagina: number }) => {
        this._filtros.numeroPagina = params.numeroPagina;
        return this.situacionPersonaService.consultarSituacionPersona(this._filtros);
      }).share();
    (<Observable<SituacionPersonasResultado[]>>this.pagina.pluck(ELEMENTOS))
      .subscribe((personas) => {
        this.personas = personas;
        if (!personas.length) {
          this.emitPersonaSeleccionada.emit(null);
        }
        this.crearForm();
      });
  }

  private obtenerVistaPersonas(consulta: ConsultaSituacionPersonas) {
    this.situacionPersonaService.consultarSituacionPersonaVista(consulta)
      .subscribe((personas) => {
        this.personasVista = personas;
        this.crearForm();
      });
  }

  public consultarSiguientesPersonas(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public get personasFa(): FormArray {
    return this.form.get('personas') as FormArray;
  }

  public get personasVistaFa(): FormArray {
    return this.formVista.get('personas') as FormArray;
  }

  public emitirPersona(seleccion: any): void {
    this.personaSeleccionada = this.personas.find((actual) => {
      let personaDelFormulario: SituacionPersonasResultado = seleccion.persona;
      return (actual.cuil === personaDelFormulario.cuil
        && actual.nroDocumento === personaDelFormulario.nroDocumento
        && actual.codigoPais === personaDelFormulario.codigoPais);
    });
    let personaContacto = new Persona();
    personaContacto.idNumero = this.personaSeleccionada.idNumero;
    personaContacto.sexoId = this.personaSeleccionada.sexoId;
    personaContacto.codigoPais = this.personaSeleccionada.codigoPais;
    personaContacto.nroDocumento = this.personaSeleccionada.nroDocumento;
    this.contactoService.obtenerDatosDeContacto(personaContacto)
      .subscribe((result) => {
        this.personaSeleccionada.codigoArea = result.codigoArea;
        this.personaSeleccionada.telefono = result.telefono;
        this.personaSeleccionada.codigoAreaCelular = result.codigoAreaCelular;
        this.personaSeleccionada.celular = result.celular;
        this.personaSeleccionada.email = result.mail;
        this.emitPersonaSeleccionada.emit(Object.assign({}, this.personaSeleccionada));
      });

  }

  public limpiarGrilla(): void {
    this.personas = [];
  }
}
