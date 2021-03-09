"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ContenidoInformativoComponent = (function () {
    function ContenidoInformativoComponent(activeModal) {
        this.activeModal = activeModal;
    }
    Object.defineProperty(ContenidoInformativoComponent.prototype, "getTitulo", {
        get: function () {
            return this.titulo ? this.titulo : (this.error ? 'Error' : 'Información');
        },
        enumerable: true,
        configurable: true
    });
    return ContenidoInformativoComponent;
}());
__decorate([
    core_1.Input()
], ContenidoInformativoComponent.prototype, "error", void 0);
__decorate([
    core_1.Input()
], ContenidoInformativoComponent.prototype, "titulo", void 0);
__decorate([
    core_1.Input()
], ContenidoInformativoComponent.prototype, "mensajes", void 0);
ContenidoInformativoComponent = __decorate([
    core_1.Component({
        selector: 'ngbd-modal-content',
        template: "\n    <div class=\"modal-header\">\n    <i [ngClass]=\"{'modal-icon-error':error}\" class=\"modal-icon material-icons\">{{error ? 'error': 'info'}}</i>\n      <h4 class=\"modal-title mr-auto\">{{getTitulo | uppercase}}</h4>\n    </div>\n    <div class=\"modal-body line-divider-bottom line-divider-top\">\n      <ul>\n        <li *ngFor=\"let mensaje of mensajes\">{{mensaje}}</li>\n      </ul>\n    </div>\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-primary\" (click)=\"activeModal.close('Close click')\">ACEPTAR</button>\n    </div>\n  "
    })
], ContenidoInformativoComponent);
exports.ContenidoInformativoComponent = ContenidoInformativoComponent;
