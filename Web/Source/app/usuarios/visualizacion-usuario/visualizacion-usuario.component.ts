import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ItemUsuario } from '../shared-usuarios/modelo/item-usuario.model';
import { UsuariosService } from '../shared-usuarios/usuarios.service';
import { Title } from '@angular/platform-browser';
import TituloBanco from '../../shared/titulo-banco';

@Component({
  selector: 'bg-visualizacion-usuario',
  templateUrl: './visualizacion-usuario.component.html',
  styleUrls: ['./visualizacion-usuario.component.scss']
})

export class VisualizacionUsuarioComponent implements OnInit {
  public form: FormGroup;
  public usuario: ItemUsuario;

  constructor(private fb: FormBuilder,
              private usuariosService: UsuariosService,
              private route: ActivatedRoute,
              private titleService: Title) {
    this.titleService.setTitle('Ver usuario ' + TituloBanco.TITULO);
    this.usuario = new ItemUsuario();
  }

  public ngOnInit(): void {
    this.crearForm();
    this.consultarUsuario();
  }

  private crearForm() {
    this.form = this.fb.group({
      nombre: [''],
      apellido: [''],
      cuil: [''],
      fechaAlta: [''],
      fechaBaja: [''],
      nombreMotivoBaja: [''],
      nombrePerfil: ['']
    });
  }

  private consultarUsuario(): void {
    this.usuariosService
      .consultarUsuario(this.route.snapshot.params['id'])
      .subscribe((usuario) => {
        this.usuario = usuario;
      });
  }
}
