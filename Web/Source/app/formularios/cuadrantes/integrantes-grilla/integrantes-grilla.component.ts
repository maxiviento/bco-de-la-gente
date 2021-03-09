import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NotificacionService } from '../../../shared/notificacion.service';
import { Integrante } from '../../../shared/modelo/integrante.model';
import { FormulariosService } from '../../shared/formularios.service';
import { RechazarFormularioComando } from '../../shared/modelo/rechazar-formulario-comando.model';
import { ModalModificarGrupoIntegranteComponent } from '../../modal-modificar-grupo-integrante/modal-modificar-grupo-integrante.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Persona } from '../../../shared/modelo/persona.model';
import { TipoApoderadoEnum } from '../../shared/modelo/tipo-apoderado-enum';

@Component({
  selector: 'bg-integrantes-grilla',
  templateUrl: './integrantes-grilla.component.html',
  styleUrls: ['./integrantes-grilla.component.scss'],
})

export class IntegrantesGrillaComponent implements OnInit {

  @Output()
  public integranteEditable: EventEmitter<Integrante> = new EventEmitter<Integrante>();
  @Output()
  public integranteGrupo: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output()
  public cambioApoderado: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Input()
  public editable: boolean;
  public solicitanteFormulario: Persona;
  public solicitante: Integrante = new Integrante();
  public solicitantes: Integrante[];
  public form: FormGroup;
  public integrantesTienenGrupo: boolean = true;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService,
              private formulariosService: FormulariosService,
              private modalService: NgbModal) {

  }

  public ngOnInit(): void {
    this.crearForm();
  }

  public editarIntegrante(integrante: Integrante) {
    this.integranteEditable.emit(integrante);
  }

  public modificarGrupoIntegrante(integrante: Integrante) {
    this.modalModificarGrupoFamiliar(integrante);
  }

  public modalModificarGrupoFamiliar(integrante: Integrante) {
    const modalRef = this.modalService.open(ModalModificarGrupoIntegranteComponent, {
      backdrop: 'static',
      windowClass: 'modal-xl'
    });
    modalRef.componentInstance.integrante = integrante;
    modalRef.result.then((res) => {
      this.integranteGrupo.emit(res);
    });
  }

  public eliminarDetalle(indice: number): void {
    this.notificacionService
      .confirmar('¿Está seguro que desea quitar el miembro?')
      .result
      .then((res) => {
        if (res) {
          let formularioId = this.solicitantes[indice].idFormulario;
          if (formularioId) {
            this.formulariosService.darDeBajaFormulario(new RechazarFormularioComando(formularioId, 1))
              .subscribe(() => {
                let sol = this.solicitantes.find(s => s.idFormulario === formularioId);
                sol.idEstado = 6;
                sol.estado = 'ELIMINADO';
                this.notificacionService.informar(Array.of('Formulario eliminado con éxito'), false);
              }, (errores) => {
                this.notificacionService.informar(errores, true);
              });
          } else {
            this.solicitantes.splice(indice, 1);
          }
        }
      });

  }

  public permiteAccionar(integrante: Integrante): boolean {
    if (integrante.nroDocumento === this.solicitanteFormulario.nroDocumento) {
      return false;
    }
    if (integrante.solicitante) {
      return false;
    }
    if (this.editable) {
      if (integrante.idEstado == 1) {
        return true;
      }
    }
    return false;
  }

  /*True cuando esta disabled. False en otro caso*/
  public noPuedeApoderar(integrante: Integrante): boolean {
    if (this.editable) {
      if (integrante.idEstado !== 4 && integrante.idEstado !== 6) {
        return false;
      }
    }
    return true;
  }

  public agregarIntegrantes(integrantes: Integrante[]) {
    this.solicitantes = integrantes;
  }

  public agregarIntegrante(integrante: Integrante) {
    this.solicitante = integrante;
    if (this.solicitantes && this.solicitantes.some((s) => s.nroDocumento === this.solicitante.nroDocumento)) {
      let i = this.solicitantes.indexOf(this.solicitantes.find((s) => s.nroDocumento === this.solicitante.nroDocumento));
      this.solicitantes[i] = this.solicitante;
    } else {
      this.solicitantes.push(this.solicitante);
    }
    this.crearForm();
  }

  public crearForm(): void {
    let tel: string = '';
    if (this.solicitante.telFijo) {
      tel = this.solicitante.telFijo;
    } else {
      if (this.solicitante.telCelular) {
        tel = this.solicitante.telCelular;
      }
    }
    let idEstado: string = 'No';
    if (this.solicitante.idEstado === 4 || this.solicitante.idEstado === 6) {
      idEstado = 'Si';
    }

    let nroDocumento = new FormControl(this.solicitante.nroDocumento);
    let nombre = new FormControl(this.solicitante.nombre);
    let apellido = new FormControl(this.solicitante.apellido);
    let telefono = new FormControl(tel);
    let mail = new FormControl(this.solicitante.mail);
    let estado = new FormControl(idEstado);

    this.form = new FormGroup({
      nroDocumento: nroDocumento,
      nombre: nombre,
      apellido: apellido,
      telefono: telefono,
      mail: mail,
      estado: estado
    });
  }

  public apoderadoChecked(indice: number) {
    this.solicitantes.forEach((s) => s.esApoderado = TipoApoderadoEnum.PertenecePeroNoApoderado);
    this.solicitantes[indice].esApoderado = TipoApoderadoEnum.EsApoderado;
    this.cambioApoderado.emit(true);
  }
}
