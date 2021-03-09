import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../auth/usuario.service';
import { AuthService } from '../auth/auth.service';
import { Observable } from 'rxjs';
import { Usuario } from '../auth/modelos/usuario.model';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  public menuColapsado: boolean = true;
  public usuario: Usuario;
  public estaLogeado: Observable<boolean>;
  public usuarioActual: Observable<Usuario>;

  constructor(private usuarioService: UsuarioService,
              public authService: AuthService) {

    this.estaLogeado = authService.estaLogueado();
    this.usuarioActual = usuarioService.obtenerUsuarioActual();
  }

  public ngOnInit(): void {
    this.usuarioActual.subscribe((usuario) => {
      if (usuario) {
        this.usuario = usuario;
      }
    });
  }

  public clearLocalStorage(): void {
    localStorage.clear();
    document.location.reload(true);
  }

  public cerrarSesion() {
    this.usuarioService.cerrarSesionCidi().subscribe((response) => {
      localStorage.clear();
      window.location.href = response;
    });
  }
}
