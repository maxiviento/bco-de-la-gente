import { Component, Input, OnInit, ViewChildren, QueryList } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { PrestamoService } from '../../../shared/servicios/prestamo.service';
import { ConfiguracionChecklistService } from '../../../configuracion-checklist/shared/configuracion-checklist.service';
import { AbstractControl } from '@angular/forms';
import { IntegrantePrestamo } from '../../../shared/modelo/integrante-prestamo.model';
import { WizardChecklistComponent } from './checklist/wizard-checklist.component';
import { ChecklistAceptarModel } from '../../shared/modelos/checklist-aceptar.model';
import { DataSharedChecklistService } from './data-shared-checklist.service';
import { Prestamo } from '../../shared/modelos/prestamo.model';
import { ChecklistEditableModel } from '../../shared/modelos/checklist-editable.model';
import { GarantePrestamo } from '../../../shared/modelo/garante-prestamo.mode';
import { Persona } from '../../../shared/modelo/persona.model';
import { FormulariosService } from '../../../formularios/shared/formularios.service';
import { Subscription } from 'rxjs';
import { PersonaService } from '../../../formularios/cuadrantes/persona/persona.service';

@Component({
  selector: 'bg-estructura-checklist',
  templateUrl: './estructura-checklist.component.html',
  styleUrls: ['./estructura-checklist.component.scss'],
  providers: [DataSharedChecklistService]
})
export class EstructuraChecklistComponent implements OnInit {
  @Input() public editable: boolean;
  @Input() public titulo: string;
  private tipoLinea: number;
  public valido: boolean = true;
  public tieneDeudaHistorica: boolean = false;
  private cantidadFormularios = 0;
  public idPrestamo: number;
  public prestamo: Prestamo = new Prestamo();
  public integrantesPrestamo: IntegrantePrestamo [] = [];
  public garantesPrestamo: GarantePrestamo [] = [];
  public editableList: ChecklistEditableModel [] = [];
  private checkListAceptar: ChecklistAceptarModel [] = [];
  public etapas: AbstractControl [];
  public subscription: Subscription;
  public mensajeAvisoPersonas = '';
  @ViewChildren(WizardChecklistComponent) public checkLists: QueryList<WizardChecklistComponent>;

  constructor(private prestamoService: PrestamoService,
              private configuracionChecklistService: ConfiguracionChecklistService,
              private activatedRoute: ActivatedRoute,
              private dataSharedChecklistService: DataSharedChecklistService,
              private formularioService: FormulariosService,
              private personaService: PersonaService) {
  }

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe((params: Params) => {
      this.idPrestamo = params['id'];
      this.prestamoService.consultarIntegrantes(this.idPrestamo)
        .subscribe((integrantes) => {
          this.integrantesPrestamo = integrantes;
          let integrantesNoRechazados = this.integrantesPrestamo.filter((p) => p.estadoFormulario != 'RECHAZADO');
          integrantesNoRechazados.forEach((x) => {
            let persona = this.crearPersona(x.sexoId, x.nroDocumento, x.codigoPais, 0);
            this.formularioService.existeFormularioEnCursoParaPersona(persona).subscribe(
              (existe: number) => {
                if (existe && existe >= 2) {
                  this.componerMensajePersonas(persona);
                  this.valido = false;
                } else {
                  this.valido = true;
                }
              }
            );
            this.formularioService.existeDeudaHistorica(persona).subscribe(
              (tieneDeuda: boolean) => {
                if (tieneDeuda) {
                  this.tieneDeudaHistorica = true;
                  x.tieneDeuda = true;
                }
              }
            );
          });
          this.cantidadFormularios = Array.from(new Set(integrantes.map((item: IntegrantePrestamo) => item.idFormulario))).length;
          let rechazados = integrantes.filter((p) => p.estadoFormulario === 'RECHAZADO');
          this.cantidadFormularios = rechazados ? this.cantidadFormularios - rechazados.length : this.cantidadFormularios;
        });
      this.prestamoService.consultarDatosPrestamo(this.idPrestamo).subscribe(
        (prestamo) => {
          this.prestamo = prestamo;
          this.tipoLinea = prestamo.idTipoIntegrante;
          if (this.tipoLinea == 1) {
            this.prestamoService.consultarGarantesPrestamo(this.idPrestamo)
              .subscribe((garantes) => {
                this.garantesPrestamo = garantes;
              });
          }
        }
      );
    });
    this.subscription = this.dataSharedChecklistService.getSubjectRechazo().subscribe((rechazo: boolean) => {
        if (rechazo) {
          this.prestamoService.consultarIntegrantes(this.idPrestamo)
            .subscribe((integrantes) => {
              this.integrantesPrestamo = integrantes;
            });
        }
      }
    );
  }

  private componerMensajePersonas(persona): void {
    this.personaService.consultarPersona(persona).subscribe(
      (res) => {
        if (this.mensajeAvisoPersonas === '') {
          this.mensajeAvisoPersonas = `${res.apellido} ${res.nombre} (${res.nroDocumento})`;
        } else {
          this.mensajeAvisoPersonas += `, ${res.apellido} ${res.nombre} (${res.nroDocumento})`;
        }
      }
    );
  }

  public deshabilitarAceptar(checklistAceptarModel: ChecklistAceptarModel) {
    if (checklistAceptarModel) {
      if (!this.checkListAceptar.find((p) => p.idFormularioLinea == checklistAceptarModel.idFormularioLinea && p.idEtapa == checklistAceptarModel.idEtapa)) {
        this.checkListAceptar.push(checklistAceptarModel);
      }
      this.checkListAceptar.forEach((p) => {
          if (p.idFormularioLinea == checklistAceptarModel.idFormularioLinea && p.idEtapa == checklistAceptarModel.idEtapa) {
            p.deshabilitar = checklistAceptarModel.deshabilitar;
          }
        }
      );
    }
    if (this.checkListAceptar.filter((p) => p.deshabilitar == false) &&
      this.checkListAceptar.filter((p) => p.deshabilitar == false && p.idEtapa == checklistAceptarModel.idEtapa).length == this.cantidadFormularios) {
      this.dataSharedChecklistService.modificarEstado(false);
    } else {
      this.dataSharedChecklistService.modificarEstado(true);
    }
  }

  public editableEmitter(checklistEditable: ChecklistEditableModel) {
    if (checklistEditable) {
      this.editableList.push(checklistEditable);
      this.dataSharedChecklistService.modificarEditable(this.editableList);
    }
  }

  public cantidadFormulariosEvent(disminuye: boolean) {
    if (disminuye) {
      this.cantidadFormularios = this.cantidadFormularios - 1;
      this.dataSharedChecklistService.modificarCantidadFormularios(this.cantidadFormularios);
    }
  }

  public rechazoIntegrante(rechazo: ChecklistAceptarModel) {
    if (rechazo && rechazo.deshabilitar) {
      this.checkListAceptar.forEach((p) => {
        if (p.idFormularioLinea == rechazo.idFormularioLinea && p.idEtapa == rechazo.idEtapa) {
          p.deshabilitar = rechazo.deshabilitar;
        }
      });
      this.dataSharedChecklistService.modificarRechazo(true);
    }
  }

  public editableFunction(integrante: IntegrantePrestamo): boolean {
    return !(integrante.estadoFormulario == 'RECHAZADO') && this.editable;
  }

  private crearPersona(idSexo: string, numeroDocumento: string, codigoPais: string, idNumero: number): Persona {
    let persona = new Persona();
    persona.sexoId = idSexo;
    persona.nroDocumento = numeroDocumento;
    persona.codigoPais = codigoPais;
    persona.idNumero = idNumero;
    return persona;
  }
}
