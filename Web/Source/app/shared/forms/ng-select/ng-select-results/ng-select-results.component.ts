import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { NgSelectItem } from '../ng-select/ng-select-item';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'ng-select-results',
  templateUrl: 'ng-select-results.component.html',
  styleUrls: ['ng-select-results.component.scss']
})
export class NgSelectResultsComponent {

  @Input() public items: NgSelectItem[];
  @Input() public selectedItems: NgSelectItem[];
  @Input() public inputText: string;
  @Output() public itemSelectedEvent: EventEmitter<any> = new EventEmitter();
  public activeIndex: number = 0;
  private ussingKeys = false;

  constructor(private sanitizer: DomSanitizer) {
  }

  public sanitize(html: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(html);
  }

  public onItemSelected(item: NgSelectItem) {
    this.itemSelectedEvent.emit(item);
  }

  public activeNext() {
    if (this.activeIndex >= this.items.length - 1) {
      this.activeIndex = this.items.length - 1;
    } else {
      this.activeIndex++;
    }
    this.scrollToElement();
    this.ussingKeys = true;
  }

  public activePrevious() {
    if (this.activeIndex - 1 < 0) {
      this.activeIndex = 0;
    } else {
      this.activeIndex--;
    }
    this.scrollToElement();
    this.ussingKeys = true;
  }

  public scrollToElement() {
    let element = document.getElementById('item_' + this.activeIndex);
    let container = document.getElementById('resultsContainer');

    if (element) {
      container.scrollTop = element.offsetTop;
    }
  }

  public selectCurrentItem() {
    if (this.items[this.activeIndex]) {
      this.onItemSelected(this.items[this.activeIndex]);
      this.activeIndex = 0;
    }
  }

  public onMouseOver(index: number) {
    if (!this.ussingKeys) {
      this.activeIndex = index;
    }
  }

  public onHovering() {
    this.ussingKeys = false;
  }

  public isSelected(currentItem) {
    let result = false;
    this.selectedItems.forEach((item) => {
      if (item.id === currentItem.id) {
        result = true;
      }
    });
    return result;
  }

}
