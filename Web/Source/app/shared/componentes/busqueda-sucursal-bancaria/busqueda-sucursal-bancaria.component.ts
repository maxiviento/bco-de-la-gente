import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { BancoService } from '../../servicios/banco.service';
import { NotificacionService } from '../../notificacion.service';
import { CustomValidators } from '../../forms/custom-validators';

@Component({
  selector: 'bg-busqueda-sucursal-bancaria',
  templateUrl: './busqueda-sucursal-bancaria.component.html',
  styleUrls: ['./busqueda-sucursal-bancaria.component.scss']
})

export class BusquedaSucursalBancariaComponent implements OnInit {
  public form: FormGroup;
  public formSucursal: FormGroup;
  public bancos: any = [];
  public sucursales: any = [];
  @Input('banco') public idBanco: number;
  @Input('sucursal') public idSucursal: number;
  @Input() public bancoRequerido: boolean = false;
  @Input() public sucursalRequerida: boolean = false;
  @Input() public edicion: boolean = false;
  @Output() public emitirFormularioSucursales: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

  constructor(private fb: FormBuilder,
              private bancoService: BancoService,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit() {
    this.crearForm();
    this.bancoService.consultarBancos().subscribe((bancos) => {
      this.bancos = bancos;
    });
    this.emitirFormularioSucursales.emit(this.form);
    this.form.valueChanges.subscribe(
      () => {
        this.emitirFormularioSucursales.emit(this.form);
      }
    );
  }

  private crearForm() {
    this.form = new FormGroup({
      idBanco: new FormControl(this.idBanco, this.bancoRequerido ? Validators.required : null),
      idSucursal: new FormControl(this.idSucursal, this.sucursalRequerida ? Validators.required : null)
    });
    this.suscribirseABanco();
  }

  private suscribirseABanco(): void{
    let banco = this.form.get('idBanco') as FormControl;
    banco.valueChanges
      .distinctUntilChanged()
      .subscribe(() => {
        (this.form.get('idSucursal') as FormControl).setValue(null);
        this.cargarSucursales();
      });
  }

  private cargarSucursales() {
    let idBanco = this.form.get('idBanco').value;
    if (idBanco) {
      this.bancoService.consultarSucursales(idBanco).subscribe((sucursales) => {
        this.sucursales = sucursales;
        if (this.sucursales.length) {
          (this.form.get('idSucursal') as FormControl).enable();
          if(this.edicion){
            setTimeout(()=>{
              this.form.get('idSucursal').setValue(this.idSucursal);
              this.form.get('idBanco').setValue(this.idBanco);
            },0.05);
            this.edicion = false;
          }
        } else {
          this.notificacionService.informar(Array.of('El banco seleccionado no posee sucursales.'), false);
        }
      });
    }else{
      this.sucursales = [];
    }
  }

  public formValid(): boolean {
    if(!this.form) return false;
    if(this.bancoRequerido || this.sucursalRequerida) return this.form.valid;
    return this.form.get('idBanco').value && this.form.get('idSucursal').value;
  }
  public setBancoId(id: number): void{
    if(!this.form) return;
    this.idBanco = id;
    this.form.get('idBanco').setValue(id);
  }
  public setSucursalId(id: number): void{
    if(!this.form) return;
    this.idSucursal = id;
    this.form.get('idSucursal').setValue(id);
  }
  public getBancoId(): number{
    if(!this.form) return null;
    return this.form.get('idBanco').value;
  }
  public getSucursalId(): number{
    if(!this.form) return null;
    return this.form.get('idSucursal').value;
  }

  public resetComponent(idBanco: number = null, idSucursal: number = null): void{
    this.idBanco = idBanco;
    this.idSucursal = idSucursal;
    this.sucursales = [];
    this.crearForm();
  }

}
