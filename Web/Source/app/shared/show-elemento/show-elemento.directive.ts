import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { UsuarioService } from '../../core/auth/usuario.service';

@Directive({selector: '[ifPermission]'})
export class IfPermissionDirective {
  private hasView = false;

  constructor(private templateRef: TemplateRef<any>,
              private viewContainer: ViewContainerRef,
              private usuarioService: UsuarioService) {
  }

  @Input() set ifPermission(path: any) {
    if (Array.isArray(path)) {
      this.usuarioService
        .tienePermisoLista(path).subscribe((tienePermiso) => {
        if (tienePermiso) {
          this.viewContainer.createEmbeddedView(this.templateRef);
          this.hasView = true;
        } else {
          this.viewContainer.clear();
          this.hasView = false;
        }
      });
    } else {
      this.usuarioService
        .tienePermiso(path).subscribe((tienePermiso) => {
        if (tienePermiso) {
          this.viewContainer.createEmbeddedView(this.templateRef);
          this.hasView = true;
        } else {
          this.viewContainer.clear();
          this.hasView = false;
        }
      });
    }
  }
}
