import { Directive, ViewContainerRef } from "@angular/core";

@Directive({
  selector: '[bg-contenedor-cuadrantes]'
})
export class ContenedorCuadrantesDirective {
  constructor(public viewContainerRef: ViewContainerRef) {
  }
}
