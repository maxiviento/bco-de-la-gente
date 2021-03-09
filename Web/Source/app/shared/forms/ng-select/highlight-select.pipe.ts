import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'highLight'})
export class HighlightPipe implements PipeTransform {
  public transform(value: string, query: string): any {
    if (query.length < 1) {
      return value;
    }
    if (query) {
      let tagRE = new RegExp('<[^<>]*>', 'ig');
      let tagList = value.match(tagRE);
      let tmpValue = value.replace(tagRE, '$!$');
      value = tmpValue.replace(new RegExp(escapeRegexp(query), 'gi'),
        '<strong style="font-weight: bold !important;">$&</strong>');
      for (let i = 0; value.indexOf('$!$') > -1; i++) {
        value = value.replace('$!$', tagList[i]);
      }
    }
    return value;
  }
}

function escapeRegexp(queryToEscape: string): string {
  return queryToEscape.replace(/([.?*+^$[\]\\(){}|-])/g, '\\$1');
}
