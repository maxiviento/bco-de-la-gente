import { Component, OnInit } from '@angular/core';
import { Formulario } from '../../../formularios/shared/modelo/formulario.model';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NotificacionService } from '../../notificacion.service';
import { PrestamoService } from '../../servicios/prestamo.service';
import { IntegrantePrestamo } from '../../modelo/integrante-prestamo.model';
import { ActivatedRoute, Params } from '@angular/router';
import { BancoService } from '../../servicios/banco.service';
import { FormulariosService } from '../../../formularios/shared/formularios.service';

@Component({
  selector: 'bg-sucursal-bancaria',
  templateUrl: './actualizar-sucursal-bancaria.component.html',
  styleUrls: ['./actualizar-sucursal-bancaria.component.scss'],
})
export class ActualizarSucursalBancariaComponent implements OnInit {
  public formularios: Formulario [] = [];
  public form: FormGroup;
  public integrantesPrestamo: IntegrantePrestamo [] = [];
  public CBBanco: any = [];
  public CBSucursal: any = [];

  constructor(private route: ActivatedRoute,
              private fb: FormBuilder,
              private prestamoService: PrestamoService,
              private formularioService: FormulariosService,
              private bancoService: BancoService,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarIntegrantes();
    this.bancoService.consultarBancos().subscribe((bancos) => {
      this.CBBanco = bancos;
    });
  }

  private consultarIntegrantes(): void {
    this.route.params
      .switchMap((params: Params) =>
        this.prestamoService.consultarIntegrantes(+params['id']))
      .subscribe((integrantes) => {
        this.integrantesPrestamo = integrantes;
        this.crearForm();
      });
  }

  public crearForm(): void {
    let idBancoFc = new FormControl(null);
    idBancoFc.valueChanges.debounceTime(500)
      .subscribe(() => {
        this.cargarSucursales();
        (this.form.get('idSucursal') as FormControl).setValue(null, {emitEvent: false});
      });
    this.form = this.fb.group({
      integrantes: new FormArray((this.integrantesPrestamo || []).map((integrante) => new FormGroup({
        idFormulario: new FormControl(integrante.idFormulario),
        nomApe: new FormControl(integrante.apellidoNombre),
        cuil: new FormControl(integrante.cuil),
        estadoForm: new FormControl(integrante.estadoFormulario),
        nroLinea: new FormControl(integrante.nroLinea),
        nombreBanco: new FormControl(integrante.nombreBanco),
        nombreSucursal: new FormControl(integrante.nombreSucursal),
        seleccionado: new FormControl(false)
      }))),
      idBanco: idBancoFc,
      idSucursal: new FormControl(undefined)
    });
  }

  public get integrantesForm(): FormArray {
    return this.form.get('integrantes') as FormArray;
  }

  public cancelar(): void {
    window.close();
  }

  public agregarSucursalFormularios(): void {
    this.formularios = [];
    this.integrantesForm.controls.forEach((integrante) => {
      if (integrante.get('seleccionado').value) {
        let formulario = new Formulario();
        formulario.id = integrante.get('idFormulario').value;
        formulario.idBanco = this.form.get('idBanco').value;
        formulario.idSucursal = this.form.get('idSucursal').value;
        this.formularios.push(formulario);
      }
    });
    this.formularioService.agregarSucursalFormularios(this.formularios)
      .subscribe((res) => {
        if (res) {
          this.consultarIntegrantes();
          (this.form.get('idBanco') as FormControl).setValue(null, {emitEvent: false});
          (this.form.get('idSucursal') as FormControl).setValue(null, {emitEvent: false});
          this.notificacionService.informar(['La sucursal de el/los formulario/s se actualizó con éxito.']);
        }
      }, (errores) => this.notificacionService.informar(errores, true));
  }

  private cargarSucursales() {
    let idBanco = this.form.get('idBanco').value;
    if (idBanco) {
      this.bancoService.consultarSucursales(idBanco).subscribe((sucursales) => {
        this.CBSucursal = sucursales;
        if (this.CBSucursal.length) {
          (this.form.get('idSucursal') as FormControl).enable();
        } else {
          this.notificacionService.informar(Array.of('El banco seleccionado no posee sucursales.'), false);
        }
      });
    }
  }

  public validarAceptar(): boolean {
    return !this.integrantesForm.controls.some((form) => form.get('seleccionado').value)
      || !this.form.get('idBanco').value
      || !this.form.get('idSucursal').value;
  }
}
