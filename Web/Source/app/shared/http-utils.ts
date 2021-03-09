import { isDate } from "util";
export class HttpUtils {

  static insertarPrefijo(filtros: any, prefijo: string = 'consulta'): object {
    if (typeof filtros === 'object') {
      let parametros = {};
      Object.getOwnPropertyNames(filtros)
        .map((key: string) => {
          if (filtros[key]) {
            if (isDate(filtros[key])) {
              filtros[key] = filtros[key].toISOString();
            }
            parametros[prefijo + '.' + key] = filtros[key]
          }
        });
      return parametros;
    }
  }

  static createFormData(object: Object, form?: FormData, namespace?: string): FormData {
    const formData = form || new FormData();
    if (object instanceof Array) {
      for (let i = 0; i < object.length; i++) {
        let formKey = namespace ? `${namespace}[${i}]` : `[${i}]`;
        this.createFormData(object[i], formData, formKey);
      }
    } else {
      for (let property in object) {
        if (!object.hasOwnProperty(property) || !object[property]) {
          continue;
        }
        let formKey = namespace ? `${namespace}.${property}` : property;
        if (object[property] instanceof Date) {
          formData.append(formKey, object[property].toISOString());
        } else if (typeof object[property] === 'object' && !(object[property] instanceof File)) {
          this.createFormData(object[property], formData, formKey);
        } else {
          formData.append(formKey, object[property]);
        }
      }
    }
    return formData;
  }
}
