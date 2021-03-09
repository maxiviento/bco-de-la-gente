import { Component, OnInit } from '@angular/core';
import { CuadranteFormulario } from '../cuadrante-formulario';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../../shared/notificacion.service';
import { FormulariosService } from '../../shared/formularios.service';
import { MiembroEmprendimiento } from '../../shared/modelo/miembro-emprendimiento.model';
import { Emprendimiento } from '../../shared/modelo/emprendimiento.model';
import { EmprendimientoService } from '../../shared/emprendimiento.service';
import { Vinculo } from '../../shared/modelo/vinculo.model';
import { TipoOrganizacion } from '../../shared/modelo/tipo-organizacion.model';
import { AbstractControl } from '@angular/forms/src/model';
import { isEmpty } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-organizacion-individual',
  templateUrl: './organizacion-individual.component.html',
  styleUrls: ['./organizacion-individual.component.scss'],
})

export class OrganizacionIndividualComponent extends CuadranteFormulario implements OnInit {
  public form: FormGroup;
  public tiposOrganizacion: TipoOrganizacion[];
  public emprendimiento: Emprendimiento = new Emprendimiento();
  public miembros: MiembroEmprendimiento[] = [];
  public vinculos: Vinculo[] = [];

  constructor(private formulariosService: FormulariosService,
              private emprendimientoService: EmprendimientoService,
              private notificacionService: NotificacionService,
              private fb: FormBuilder) {
    super();
  }

  public ngOnInit(): void {
    this.crearForm();
    if (this.formulario) {
      this.miembros = this.formulario.miembrosEmprendimiento || [];
      if (this.formulario.datosEmprendimiento) {
        this.emprendimiento = this.formulario.datosEmprendimiento;
      }
      this.procesarSolicitante();
    }
    this.consultarDatosBasicos();
    this.obtenerVinculos();
  }

  public esValido(): boolean {
    if (!this.editable) {
      return true;
    } else {
      return this.form.valid && !isEmpty(this.emprendimiento.id);
    }
  }

  public inicializarDeNuevo(): boolean {
    return false;
  }

  private crearForm() {
    this.form = this.fb.group({
      tipoOrganizacion: [this.emprendimiento.idTipoOrganizacion, Validators.required]
    });
    if (!this.editable) {
      this.form.disable();
    }
  }

  private consultarDatosBasicos(): void {
    let promesaTiposOrganizacion = new Promise((resolve) =>
      this.emprendimientoService.consultarTiposOrganizacion().subscribe((res) => {
        this.tiposOrganizacion = res;
        return resolve();
      }));

    Promise.all([promesaTiposOrganizacion]).then(() =>
      this.crearForm()).catch((error) =>
      this.notificacionService.informar([error], true));
  }

  private obtenerVinculos(): void {
    this.emprendimientoService
      .consultarVinculos()
      .subscribe((vinculos) => {
        this.vinculos = vinculos;
      });
  }

  private procesarSolicitante(): void {
    let solicitante = this.getSolicitanteComoMiembro();
    if (!this.miembros.length && solicitante) {
      this.miembros.push(solicitante);
      return;
    }
    if (this.miembros.length && solicitante) {
      let miembroSolicitante = this.miembros.find(x => x.persona.nroDocumento == solicitante.persona.nroDocumento);
      if (miembroSolicitante) {
        miembroSolicitante.esSolicitante = true;
      } else {
        if (!solicitante) {
          this.miembros.push(solicitante);
        }
      }
    }
  }

  private getSolicitanteComoMiembro(): MiembroEmprendimiento {
    if (this.formulario && this.formulario.solicitante) {
      return new MiembroEmprendimiento(this.formulario.solicitante, null, null, null, null, null, true);
    }
    return null;
  }

  public cargarMiembro(): void {
    this.emprendimientoService
      .modalMiembroEmprendimiento(this.vinculos, new MiembroEmprendimiento())
      .result
      .then((res) => {
        if (res && res.miembro) {
          if (this.miembros.some(m => m.persona.nroDocumento === res.miembro.persona.nroDocumento)) {
            let i = this.miembros.indexOf(this.miembros.find(m => m.persona.nroDocumento === res.miembro.persona.nroDocumento));
            this.miembros[i] = res.miembro;
          } else {
            this.miembros.push(res.miembro);
          }
        }
      });
  }

  public editarMiembro(miembro: MiembroEmprendimiento): void {
    this.emprendimientoService
      .modalMiembroEmprendimiento(this.vinculos, miembro)
      .result
      .then((res) => {
        if (res && res.miembro) {
          if (this.miembros.some(m => m.persona.nroDocumento === res.miembro.persona.nroDocumento)) {
            let i = this.miembros.indexOf(this.miembros.find(m => m.persona.nroDocumento === res.miembro.persona.nroDocumento));
            this.miembros[i] = res.miembro;
          } else {
            this.miembros.push(res.miembro);
          }
        }
      });
  }

  public quitarMiembro(index: number): void {
    this.notificacionService
      .confirmar('Está seguro que desea quitar el miembro del emprendimiento?')
      .result
      .then(res => {
        if (res) {
          this.miembros.splice(index, 1);
        }
      });
  }

  private obtenerDatosEmprendimiento(): Emprendimiento {
    let formModel = this.form.value;
    let emprendimiento = new Emprendimiento();
    emprendimiento.id = this.emprendimiento.id;
    emprendimiento.idTipoOrganizacion = formModel.tipoOrganizacion;
    return emprendimiento;
  }

  public actualizarDatos() {
    let emp = this.obtenerDatosEmprendimiento();
    let comando = {
      id: emp.id,
      idTipoOrganizacion: emp.idTipoOrganizacion,
      miembros: this.miembros
    };
    this.formulariosService.actualizarOrganizacionEmprendimiento(this.formulario.id, comando)
      .subscribe(() => {
          this.emprendimiento = emp;
          this.setDatosEmprendimientoFormulario(emp);
        },
        (errores) => {
          this.notificacionService.informar(errores, true);
        });
  }

  private setDatosEmprendimientoFormulario(emp: Emprendimiento): void {
    if (emp) {
      if (this.formulario && this.formulario.datosEmprendimiento) {
        this.formulario.datosEmprendimiento.id = emp.id;
        this.formulario.datosEmprendimiento.idTipoOrganizacion = emp.idTipoOrganizacion;
        this.formulario.miembrosEmprendimiento = this.miembros;
      } else {
        this.formulario.datosEmprendimiento = emp;
      }
    }
  }

  public radioChecked(fc: AbstractControl, radioValue: number) {
    return fc.value && radioValue == fc.value;
  }
}
