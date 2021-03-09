import {
  NgModule,
  Optional,
  SkipSelf
} from '@angular/core';

import { CommonModule }      from '@angular/common';
import { RouterModule } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { LoggerService } from './logger.service';
import { SpinnerComponent } from './spinner/spinner.component';
import { SpinnerService } from './spinner/spinner.service';
import { NavbarComponent } from './navbar/navbar.component';
import { ErrorHandler } from './http/error-handler.service';
import { AuthService } from './auth/auth.service';
import { UsuarioService } from './auth/usuario.service';
import { HomeComponent } from './home/home.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NgbModule,
    SharedModule
  ],
  exports: [SpinnerComponent, NavbarComponent, HomeComponent],
  declarations: [SpinnerComponent, NavbarComponent, HomeComponent],
  providers: [
    LoggerService,
    SpinnerService,
    ErrorHandler,
    AuthService,
    UsuarioService,
  ]
})
export class CoreModule {

  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule is already loaded. Import it in the AppModule only');
    }
  }

}
