import { NgModule } from '@angular/core';
import { AuthService } from './auth.service';
import { UsuarioService } from './usuario.service';

@NgModule({
  providers: [AuthService, UsuarioService]
})
export class AuthModule {

}
