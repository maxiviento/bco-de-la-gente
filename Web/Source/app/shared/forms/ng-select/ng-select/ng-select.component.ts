import { AfterViewInit, Component, EventEmitter, forwardRef, Input, Output, ViewChild } from '@angular/core';
import { NgSelectItem } from './ng-select-item';
import { NgSelectResultsComponent } from '../ng-select-results/ng-select-results.component';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { Messages } from './messages';
import { isDefined } from '@ng-bootstrap/ng-bootstrap/util/util';

const KEY_CODE_DOWN_ARROW = 40;
const KEY_CODE_UP_ARROW = 38;
const KEY_CODE_ENTER = 13;
const KEY_CODE_TAB = 9;
const KEY_CODE_DELETE = 8;
const VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => NgSelectComponent),
  multi: true
};
const noop = () => {
};

@Component({
  selector: 'ng-select',
  templateUrl: 'ng-select.component.html',
  styleUrls: ['ng-select.component.scss'],
  providers: [VALUE_ACCESSOR]
})
export class NgSelectComponent<T> implements AfterViewInit, ControlValueAccessor {

  public MORE_RESULTS_MSG = 'Mostrando '
    + Messages.PARTIAL_COUNT_VAR + ' de '
    + Messages.TOTAL_COUNT_VAR + ' resultados. Redefina su búsqueda para mostrar más resultados.';
  public NO_RESULTS_MSG = 'No existen resultados para la búsqueda.';

  @Input() public dataSourceProvider: (term: string) => Observable<T[]>;
  @Input() public selectedProvider: (ids: string[]) => Observable<T[]>;
  @Input() public ngSelectItemAdapter: (entity: T) => NgSelectItem;
  @Input() public referenceMode: 'id' | 'entity' = 'id';
  @Input() public multiple = false;
  @Input() public searchDelay = 250;
  @Input() public css: string = 'form-control';
  @Input() public placeholder: string = 'Seleccione';
  @Input() public superiorDivClass: string = '';
  @Input() public minimumInputLength = 2;
  @Input() public disabled = false;
  @Input() public messages: Messages = {
    moreResultsAvailableMsg: this.MORE_RESULTS_MSG,
    noResultsAvailableMsg: this.NO_RESULTS_MSG
  };

  @Input()
  public set dataSource(values: any) {
    let result = NgSelectComponent.configurarSelect(values.list, values.id || 'id', values.name || 'nombre');
    if (this.fullDataList && !!this.fullDataList.length) {
      this.fullDataList = undefined;
    }
    this.dataSourceProvider = result.provider;
    this.ngSelectItemAdapter = result.adapter;
    this.selectedProvider = result.selected;
    if (result.provider && this.selectedValue) {
      this.writeValue(this.selectedValue);
    }
  }

  private selectedValue: number;

  @Input()
  public set selected(value: number) {
    this.selectedValue = value;
    if (value) {
      this.writeValue(value);
    } else {
      this.placeholderSelected = '';
    }
  };

  @Input() public resultsCount;
  @Input() public clientMode = false;
  @Output() public onSelect: EventEmitter<NgSelectItem> = new EventEmitter<NgSelectItem>();
  @Output() public onRemove: EventEmitter<NgSelectItem> = new EventEmitter<NgSelectItem>();
  @Output() public inputTextEmitter: EventEmitter<string> = new EventEmitter<string>();
  @ViewChild('termInput') public termInput;
  @ViewChild('results') public results: NgSelectResultsComponent;
  public term = new FormControl();
  public resultsVisible = false;
  public listData: NgSelectItem[];
  public fullDataList: NgSelectItem[];
  public selectedItems: NgSelectItem[] = [];
  public searchFocused = false;
  public placeholderSelected = '';
  public onTouchedCallback: () => void = noop;
  public onChangeCallback: (_: any) => void = noop;
  public inputText: string;
  private displayLength: number = null;
  @Input()
  public set cutDisplay(displayLength: number) {
    this.displayLength = displayLength;
  };

  public ngAfterViewInit() {
    this.subscribeToChangesAndLoadDataFromObservable();
  }

  private subscribeToChangesAndLoadDataFromObservable() {
    const observable = this.term
      .valueChanges
      .debounceTime(this.searchDelay)
      .distinctUntilChanged();
    this.subscribeToResults(observable);
  }

  private subscribeToResults(observable: Observable<string>): void {
    observable
      .do(() => this.resultsVisible = false)
      .filter((term) => term.length >= this.minimumInputLength || term.length === 0)
      .switchMap((term) => this.loadDataFromObservable(term))
      .map((items) => items.filter((item) => !(this.multiple && this.alreadySelected(item))))
      .do(() => this.resultsVisible = this.searchFocused)
      .subscribe((items) => this.listData = items);
  }

  private loadDataFromObservable(term: string): Observable<NgSelectItem[]> {
    this.inputText = term;
    this.inputTextEmitter.emit(this.inputText);
    return this.clientMode ? this.fetchAndfilterLocalData(term) : this.fetchData(term);
  }

  private fetchAndfilterLocalData(term: string): Observable<NgSelectItem[]> {
    if (!this.fullDataList) {
      return this.fetchData('')
        .flatMap((items) => {
          this.fullDataList = items;
          return this.filterLocalData(term);
        });
    } else {
      return this.filterLocalData(term);
    }
  }

  private filterLocalData(term: string): Observable<NgSelectItem[]> {
    return Observable.of(this.fullDataList.filter((item) => this.containsText(item, term)));
  }

  private containsText(item, term: string) {
    return item.text.toUpperCase().indexOf(term.toUpperCase()) !== -1;
  }

  private fetchData(term: string): Observable<NgSelectItem[]> {
    return this
      .dataSourceProvider(term)
      .map((items: T[]) => this.adaptItems(items));
  }

  private adaptItems(items: T[]): NgSelectItem[] {
    const convertedItems = [];
    items.map((item) => this.ngSelectItemAdapter(item))
      .forEach((ngSelectItem) => convertedItems.push(ngSelectItem));
    return convertedItems;
  }

  public writeValue(selectedValues: any): void {
    if (selectedValues) {
      if (this.referenceMode === 'id') {
        this.populateItemsFromIds(selectedValues);
      } else {
        this.populateItemsFromEntities(selectedValues);
      }
    } else {
      this.selectedItems = [];
    }
  }

  private populateItemsFromEntities(selectedValues: any) {
    if (this.multiple) {
      this.handleMultipleWithEntities(selectedValues);
    } else {
      const ngSelectItem = this.ngSelectItemAdapter(selectedValues);
      this.selectedItems = [ngSelectItem];
      this.placeholderSelected = ngSelectItem.text;
    }
  }

  private handleMultipleWithEntities(selectedValues: any) {
    selectedValues.forEach((entity) => {
      const item = this.ngSelectItemAdapter(entity);
      const ids = this.getSelectedIds();

      if (ids.indexOf(item.id) === -1) {
        this.selectedItems.push(item);
      }
    });
  }

  private populateItemsFromIds(selectedValues: any) {
    if (this.multiple) {
      this.handleMultipleWithIds(selectedValues);
    } else {
      this.handleSingleWithId(selectedValues);
    }
  }

  private handleMultipleWithIds(selectedValues: any) {
    if (selectedValues !== undefined && this.selectedProvider !== undefined) {
      const uniqueIds = [];
      selectedValues.forEach((id) => {
        if (uniqueIds.indexOf(id) === -1) {
          uniqueIds.push(id);
        }
      });

      this.selectedProvider(uniqueIds).subscribe((items: T[]) => {
        this.selectedItems = items.map(this.ngSelectItemAdapter);
      });
    }
  }

  private handleSingleWithId(id: any) {
    if (id !== undefined && this.selectedProvider !== undefined) {
      this.selectedProvider([id]).subscribe((items: T[]) => {
        items.forEach((item) => {
          const ngSelectItem = this.ngSelectItemAdapter(item);
          this.selectedItems = [ngSelectItem];
          this.placeholderSelected = ngSelectItem.text;
        });
      });
    }
  }

  public registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  public registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  private alreadySelected(item: NgSelectItem): boolean {
    let result = false;
    this.selectedItems.forEach((selectedItem) => {
      if (selectedItem.id === item.id) {
        result = true;
      }
    });
    return result;
  }

  public onItemSelected(item: NgSelectItem) {
    if (this.multiple) {
      this.selectedItems.push(item);
      const index = this.listData.indexOf(item, 0);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
    } else {
      this.selectedItems.length = 0;
      this.selectedItems.push(item);
    }

    this.onChangeCallback('id' === this.referenceMode ? this.getSelectedIds() : this.getEntities());
    this.term.patchValue('', {emitEvent: false});
    setTimeout(() => this.focusInput(), 1);
    this.resultsVisible = false;
    this.onSelect.emit(item);
    if (!this.multiple) {
      this.placeholderSelected = item.text;
    }
  }

  private getSelectedIds(): any {
    if (this.multiple) {
      const ids: string[] = [];

      this.selectedItems.forEach((item) => ids.push(item.id));

      return ids;
    } else {
      return this.selectedItems.length === 0 ? null : this.selectedItems[0].id;
    }
  }

  private getEntities(): T[] {
    if (this.multiple) {
      const entities = [];

      this.selectedItems.forEach((item) => {
        entities.push(item.entity);
      });

      return entities;
    } else {
      return this.selectedItems.length === 0 ? null : this.selectedItems[0].entity;
    }
  }

  public removeItem(item: NgSelectItem) {
    const index = this.selectedItems.indexOf(item, 0);

    if (index > -1) {
      this.selectedItems.splice(index, 1);
    }

    this.onChangeCallback('id' === this.referenceMode ? this.getSelectedIds() : this.getEntities());
    this.onRemove.emit(item);
    if (!this.multiple) {
      this.placeholderSelected = '';
    }
  }

  public onFocus() {
    this.searchFocused = true;
  }

  public onBlur() {
    this.term.patchValue('', {emitEvent: false});
    this.resultsVisible = false;
    this.onTouchedCallback();
  }

  public getInputWidth(): string {
    const searchEmpty = this.selectedItems.length === 0 && (this.term.value === null || this.term.value.length === 0);
    const length = this.term.value === null ? 0 : this.term.value.length;
    if (!this.multiple) {
      return '100%';
    } else {
      return searchEmpty ? '100%' : (1 + length * .6) + 'em';
    }
  }

  public onKeyUp(ev) {
    if (this.results) {
      if (ev.keyCode === KEY_CODE_DOWN_ARROW) {
        this.results.activeNext();
      } else if (ev.keyCode === KEY_CODE_UP_ARROW) {
        this.results.activePrevious();
      } else if (ev.keyCode === KEY_CODE_ENTER) {
        this.results.selectCurrentItem();
      }
    } else {
      if (this.minimumInputLength === 0) {
        if (ev.keyCode === KEY_CODE_ENTER || ev.keyCode === KEY_CODE_DOWN_ARROW) {
          this.focusInputAndShowResults();
        }
      }
    }
  }

  public onKeyDown(ev) {
    if (this.results) {
      if (ev.keyCode === KEY_CODE_TAB) {
        this.results.selectCurrentItem();
      }
    }

    if (ev.keyCode === KEY_CODE_DELETE) {
      if ((!this.term.value || this.term.value.length === 0) && this.selectedItems.length > 0) {
        this.removeItem(this.selectedItems[this.selectedItems.length - 1]);
      }
    }
  }

  public focusInput() {
    if (!this.disabled) {
      this.termInput.nativeElement.focus();
      this.resultsVisible = false;
    }
    this.searchFocused = !this.disabled;
  }

  public focusInputAndShowResults() {
    if (!this.disabled) {
      this.termInput.nativeElement.focus();
      this.subscribeToResults(Observable.of(''));
    }
    this.searchFocused = !this.disabled;
  }

  public onKeyPress(ev) {
    if (ev.keyCode === KEY_CODE_ENTER) {
      ev.preventDefault();
    }
  }

  public  getCss(): string {
    return 'ng-select-selection-container ' + (this.css === undefined ? '' : this.css);
  }

  public getMinHeight(): string {
    const isInputSm: boolean = this.css === undefined ? false : this.css.indexOf('input-sm') !== -1;
    return isInputSm ? '30px' : '34px';
  }

  public getPlaceholder(): string {
    return this.selectedItems.length > 0 ? this.placeholderSelected : this.placeholder;
  }

  public  isHideable(): boolean {
    return !this.multiple && this.placeholderSelected !== '';
  }

  public  focus(): void {
    this.termInput.nativeElement.focus();
  }

  public getCountMessage(): string {
    let msg = this.messages && this.messages.moreResultsAvailableMsg ? this.messages.moreResultsAvailableMsg : this.MORE_RESULTS_MSG;
    msg = msg.replace(Messages.PARTIAL_COUNT_VAR, String(this.listData.length));
    msg = msg.replace(Messages.TOTAL_COUNT_VAR, String(this.resultsCount));
    return msg;
  }

  /**
   * @param entities: Array para llenar el combo.
   * @param nameKey: Nombre del atributo del modelo por la que se hará la búsqueda en el select. (nombre o descripcion)
   * @param idKey: Nombre del atributo del modelo por la que se seleccionará un objeto. (id)
   */
  public static configurarSelect<T>(entities: T [], idKey: string, nameKey: string): any {
    let provider, adapter, selected;
    if (isDefined(entities) && entities != null) {
      provider = (term: string) =>
        Observable.of(entities.filter((obj) => obj[nameKey].indexOf(term) !== -1));

      adapter = (entity: any) => {
        return {
          id: entity[idKey],
          text: entity[nameKey],
          entity,
        };
      };

      selected = (ids: number[]) => {
        let seleccionados = [];
        for (let id of ids) {
          let seleccionado = entities.find((entity) => entity[idKey] == id);
          if (seleccionado) {
            seleccionados.push(seleccionado);
          }
        }
        return Observable.of(seleccionados);
      };
    }
    return {provider, adapter, selected};
  }

  public cutText(text: string): string{
    if(this.displayLength){
      return text.substr(0, this.displayLength);
    }
    return text;
  }
}
