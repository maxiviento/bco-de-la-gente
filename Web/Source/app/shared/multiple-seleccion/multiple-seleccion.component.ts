import { Component, OnInit, EventEmitter, Output, Input } from "@angular/core";
import { SeleccionMultiple } from "./seleccion-multiple.model";
import {
  IMultiSelectSettings,
  IMultiSelectTexts
} from "angular-2-dropdown-multiselect";
import { MultipleSeleccionService } from "./multiple-seleccion.service";
import { FormBuilder, FormGroup } from "@angular/forms";

@Component({
  selector: "multiple-seleccion",
  templateUrl: "./multiple-seleccion.component.html",
  styleUrls: ["./multiple-seleccion.component.scss"],
  providers: [MultipleSeleccionService]
})
export class MultipleSeleccionComponent implements OnInit {
  public form: FormGroup;
  public seleccionCombo: SeleccionMultiple[];

  @Input() public colLabel: number;
  @Input() public colDiv: number;
  @Input() public titulo: string;
  @Input() public tipoCombo: string;
  @Input() public comboLinea: SeleccionMultiple[];
  public seleccionados: number[] = [];

  @Output() public opcionesSeleccionadas: EventEmitter<
    string
  > = new EventEmitter<string>();

  public comboTexts: IMultiSelectTexts = {
    checkAll: "Seleccionar todos",
    uncheckAll: "Deseleccionar todos",
    checkedPlural: "elementos seleccionados",
    searchPlaceholder: "Buscar",
    searchEmptyResult: "Sin resultados",
    defaultTitle: "Seleccione",
    allSelected: "Todos seleccionados"
  };

  public settings: IMultiSelectSettings = {
    buttonClasses: "form-control item-seleccionado",
    containerClasses: "multiple-contenedor",
    showCheckAll: true,
    showUncheckAll: true,
    enableSearch: true,
    displayAllSelectedText: true
  };

  constructor(
    private fb: FormBuilder,
    private service: MultipleSeleccionService
  ) {
    this.seleccionCombo = [];
  }

  public ngOnInit(): void {
    this.crearForm();
    this.getCombos();
    this.form.controls["comboMultipleId"].valueChanges
      .distinctUntilChanged()
      .subscribe(value => {
        this.opcionesSeleccionadas.emit(value);
      });
    if (this.seleccionados) {
      this.form.controls["comboMultipleId"].setValue(this.seleccionados);
    }
  }

  public getCombos() {
    if (this.tipoCombo === "comboEstadoFormulario") {
      this.service
        .consultarEstadosFormulario()
        .subscribe(combos => (this.seleccionCombo = combos));
    } else {
      if (this.tipoCombo === "comboLinea") {
        this.service.consultarLineas().subscribe(combos => {
          this.comboLinea = combos;
          this.seleccionCombo = this.comboLinea.filter(
            linea => linea.dadoDeBaja === false
          );
        });
      } else {
        if (this.tipoCombo === "comboEstadoPrestamo") {
          this.service
            .consultarEstadosPrestamo()
            .subscribe(combos => (this.seleccionCombo = combos));
        } else if (this.tipoCombo === "comboEstadoProceso") {
          this.service
            .obtenerEstadosProceso()
            .subscribe(combos => (this.seleccionCombo = combos));
        } else if (this.tipoCombo === "comboTipoProceso") {
          this.service
            .obtenerTiposProceso()
            .subscribe(combos => (this.seleccionCombo = combos));
        }
      }
    }
  }

  public crearForm() {
    this.form = this.fb.group({
      comboMultipleId: [null]
    });
  }

  public setFiltros(idSeleccionados: string): void {
    if (idSeleccionados) {
      let seleccion = idSeleccionados.split(",");
      seleccion.forEach(id => {
        if (id) {
          this.seleccionados.push(+id);
        }
      });
    }
    this.crearForm();
  }
  public incluirDadosDeBaja(dadasDeBaja: boolean) {
    if (dadasDeBaja) {
      this.seleccionCombo = this.comboLinea.filter(
        linea => linea.dadoDeBaja === false
      );
    } else {
      this.seleccionCombo = this.comboLinea;
    }
  }
}
