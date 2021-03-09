import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from "@angular/forms";
import { EtapaEstadoLinea } from '../../prestamos-checklists/shared/modelos/etapa-estado-linea.model';
import { Etapa } from '../../etapas/shared/modelo/etapa.model';
import { EstadoPrestamo } from '../../prestamos-checklists/shared/modelos/estado-prestamo.model';
import { NotificacionService } from '../../shared/notificacion.service';

@Component({
  selector: 'bg-configuracion-etapa-estado-linea',
  templateUrl: 'configuracion-etapa-estado-linea.component.html',
  styleUrls: ['configuracion-etapa-estado-linea.component.scss']
})

export class ConfiguracionEtapaEstadoLineaComponent implements OnInit {
  public form: FormGroup;
  @Input() public etapas: Etapa[] = [];
  @Input() public estados: EstadoPrestamo[] = [];
  @Input() public etapasEstadosLinea: EtapaEstadoLinea[] = [];
  @Output() public idEtapaSeleccionada: EventEmitter<number> = new EventEmitter<number>();
  private idEtapa: number;
  private nuevaEtapa: EtapaEstadoLinea = new EtapaEstadoLinea();
  private existenPrestamosConVersionVigente: boolean = false;
  public habilitarNuevaEtapa: boolean = false;
  public isCollapsed: boolean = false;
  public idsEtapasEliminadas: number[] = [];
  @Input() public editable: boolean;

  constructor(private fb: FormBuilder,
              private notificacionService: NotificacionService) {
  }

  public ngOnInit() {
    this.crearForm();
    this.asignarEtapaYEstadoDefault();
  }

  private crearForm() {
    this.form = this.fb.group({
      nuevaEtapa: this.fb.group({
          orden: [this.nuevaEtapa.orden],
          etapaInicio: [this.nuevaEtapa.idEtapaActual, Validators.required],
          estadoInicio: [this.nuevaEtapa.idEstadoActual, Validators.required],
          estadoDePase: [this.nuevaEtapa.idEstadoSiguiente, Validators.required],
          etapaSiguiente: [this.nuevaEtapa.idEtapaSiguiente, Validators.required],
          seleccionado: [false]
        }),
      etapasEstados: this.fb.array((this.etapasEstadosLinea || []).map((etapaEstado) => {
          let seleccionadoFc = new FormControl(this.etapaSeleccionada(etapaEstado.idEtapaActual));
          let etapaInicioFc = new FormControl(etapaEstado.idEtapaActual, Validators.required);
          seleccionadoFc.valueChanges.subscribe((check) => {
            let idSeleccionado = this.idEtapa == etapaInicioFc.value;
            if (check && !idSeleccionado) {
              this.idEtapa = etapaInicioFc.value;
              this.idEtapaSeleccionada.emit(this.idEtapa);
            } else if (idSeleccionado) {
              this.idEtapa = null;
              this.idEtapaSeleccionada.emit(this.idEtapa);
            }
          });

          return this.fb.group({
            orden: [etapaEstado.orden],
            etapaInicio: etapaInicioFc,
            estadoInicio: [etapaEstado.idEstadoActual, Validators.required],
            estadoDePase: [etapaEstado.idEstadoSiguiente, Validators.required],
            etapaSiguiente: [etapaEstado.idEtapaSiguiente, Validators.required],
            seleccionado: seleccionadoFc
          });
        }
      ))
    });
  }

  public get etapasEstadosFa(): FormArray {
    return this.form.get('etapasEstados') as FormArray;
  }

  public etapaSeleccionada(idEtapa: number): boolean {
    return this.idEtapa == idEtapa;
  }

  public getComboEtapas(): Etapa[]{
    if(!this.etapasEstadosLinea.length) return this.etapas;
    let etapas = [];
    this.etapas.forEach(e => etapas.push(e));
    let etapasEstados = this.etapasEstadosLinea.sort(x => x.orden);
    let ordenActual = this.etapasEstadosLinea.length;
    // Filtro las etapas anteriores a la ultima fila cargada para no incluir esas etapas
    etapasEstados = etapasEstados.filter(x => x.orden < ordenActual);
    etapasEstados.forEach((etapaEstado) => {
      let et = etapas.find(e => e.id == etapaEstado.idEtapaActual);
      if(et){
        let i = etapas.indexOf(et);
        etapas.splice(i, 1);
      }
    });
    // Verifico que en la última etapa siga manteniendo en la etapa siguiente la misma para poder mostrarla
    let ultima = this.etapasEstadosLinea[this.etapasEstadosLinea.length - 1];
    if(ultima.idEtapaActual != ultima.idEtapaSiguiente){
      let et = etapas.find(e => e.id == ultima.idEtapaActual);
      if(et){
        let i = etapas.indexOf(et);
        etapas.splice(i, 1);
      }
    }
    return etapas;
  }

  public getComboEstados(): EstadoPrestamo[]{
    if(!this.etapasEstadosLinea.length) return this.estados;
    let estados = [];
    this.estados.forEach(e => estados.push(e));
    let etapasEstados = this.etapasEstadosLinea.sort(x => x.orden);

    etapasEstados.forEach((etapaEstado) => {
      let estadoInicio = estados.find(e => e.clave == etapaEstado.idEstadoActual);
      if(estadoInicio){
        let i = estados.indexOf(estadoInicio);
        estados.splice(i, 1);
      }
      if(this.ultimaEtapa().orden > etapaEstado.orden){
        let estadoSiguiente = estados.find(e => e.clave == etapaEstado.idEstadoSiguiente);
        if(estadoSiguiente){
          let i = estados.indexOf(estadoSiguiente);
          estados.splice(i, 1);
        }
      }
    });

    return estados;
  }

  private ultimaEtapa(): EtapaEstadoLinea{
    if(!this.etapasEstadosLinea.length) return null;
    return this.etapasEstadosLinea.sort(x => x.orden)[this.etapasEstadosLinea.length - 1];
  }

  public etapaEditable(orden: number): boolean{
    if(this.existenPrestamosConVersionVigente) return false;
    if(!this.etapasEstadosLinea.length) return true;
    return this.ultimaEtapa().orden == orden;
  }

  public eliminarEtapa(): void{
    this.notificacionService.confirmar('Está seguro que desea eliminar la etapa seleccionada?')
      .result
      .then((res) => {
        if(res){
          let i = this.etapasEstadosLinea.indexOf(this.ultimaEtapa());
          let eliminado = this.etapasEstadosLinea.splice(i, 1);
          this.nuevaEtapa = this.prepararNuevaEtapa(); // guarda la nueva etapa para no perder los cambios
          this.asignarEtapaYEstadoDefault();
          if(eliminado[0].id) this.idsEtapasEliminadas.push(eliminado[0].id);
        }
      })
  }

  private prepararNuevaEtapa(): EtapaEstadoLinea{
    let formModel = this.form.get('nuevaEtapa').value;
    let ultimaEtapa = this.ultimaEtapa();
    return new EtapaEstadoLinea(
      null,
      (ultimaEtapa ? (ultimaEtapa.orden + 1) : 1),
      formModel.etapaInicio,
      formModel.estadoInicio,
      formModel.etapaSiguiente,
      formModel.estadoDePase
    );
  }

  public habNuevaEtapa(): void{
    this.habilitarNuevaEtapa = true;
  }

  public agregarNuevaEtapa(): void{
    if(!this.form.get('nuevaEtapa').valid){
      this.notificacionService.informar(['Seleccione todos los estados y etapas antes de agregar']);
      return;
    }
    this.etapasEstadosLinea.push(this.prepararNuevaEtapa());
    this.nuevaEtapa = new EtapaEstadoLinea();
    this.crearForm();
    let that = this;
    setTimeout(function () {
      that.asignarEtapaYEstadoDefault();
    }, 10);
    this.habilitarNuevaEtapa = false;
  }

  private asignarEtapaYEstadoDefault(): void{
    let ultima = this.ultimaEtapa();
    if(ultima){
      this.nuevaEtapa.idEtapaActual = ultima.idEtapaSiguiente;
      this.nuevaEtapa.idEstadoActual = ultima.idEstadoSiguiente;
      this.crearForm();
    }else{
      this.crearForm();
    }
  }

  public limpiarConfiguracion(): void{
    this.etapasEstadosLinea = [];
    this.nuevaEtapa = new EtapaEstadoLinea();
    this.crearForm();
  }

  public reset(etapasEstadosLinea: EtapaEstadoLinea[]): void{
    this.etapasEstadosLinea = etapasEstadosLinea;
    this.crearForm();
  }

  public visualizar(visualizar: boolean): void{
    this.isCollapsed = !visualizar;
  }

  public cancelarAgregadoEtapa(): void{
    this.habilitarNuevaEtapa = false;
    this.asignarEtapaYEstadoDefault();
  }

  public siguienteEtapa(): void{
    // Valida que haya etapas y que no sea la última
    let ultimaEtapa = this.ultimaEtapa();
    if(!ultimaEtapa) return;
    if(this.idEtapa == ultimaEtapa.idEtapaActual) return;

    // Obtengo el índice de la etapa actual
    let etapas = this.etapasEstadosLinea.sort(x => x.orden);
    let i = etapas.indexOf(etapas.find(x => x.idEtapaActual == this.idEtapa));

    // Voy indexando las siguientes etapas_estados hasta encontrar la siguiente etapa
    let sigEtapa = this.idEtapa;
    while(sigEtapa == this.idEtapa){
      i++;
      sigEtapa = etapas[i].idEtapaActual;
    }
    this.idEtapa = sigEtapa;
    this.idEtapaSeleccionada.emit(this.idEtapa);
  }

  public ultimaEtapaSeleccionada(): boolean {
    let ultimaEtapa = this.ultimaEtapa();
    if(!ultimaEtapa) return false;
    if(this.idEtapa == ultimaEtapa.idEtapaActual) return true;
  }

  public deseleccionarEtapa(): void {
    this.idEtapa = null;
    this.crearForm();
  }

  public generarNuevaVersion(): void {
    this.etapasEstadosLinea.forEach(x => x.id = null);
    this.crearForm();
  }

}
