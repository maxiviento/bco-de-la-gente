import { Response } from "@angular/http";
import { MapUtils } from "../map-utils";

export const ELEMENTOS = 'elementos';

export interface Pagina<T> {
  elementos: T[];
  numeroPagina: number;
  elementosPorPagina: number;
  tieneMasResultados: boolean;
}

export class PaginaUtils {

  static extraerPagina<T>(clazz: {new(): T}, res: Response): Pagina<T> {

    let resultado = res.json().resultado;
    if (resultado) {
      let elementos = (resultado.elementos || [])
        .map((elemento) => MapUtils.deserialize(clazz, elemento));

      return <Pagina<T>>{
        elementos: elementos,
        numeroPagina: resultado.numeroPagina,
        elementosPorPagina: resultado.elementosPorPagina,
        tieneMasResultados: resultado.tieneMasResultados,
      };
    }
  }
}

