import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Integrante } from "../../shared/modelo/integrante.model";
import AccionGrupoUnico from "../../grupo-unico/shared-grupo-unico/accion-grupo-unico.enum";
import { SexoService } from "../../shared/servicios/sexo.service";
import { PaisService } from "../../shared/servicios/pais.service";

@Component({
  selector: 'bg-modal-modificar-grupo-integrante',
  templateUrl: './modal-modificar-grupo-integrante.component.html',
  styleUrls: ['./modal-modificar-grupo-integrante.component.scss'],
})

export class ModalModificarGrupoIntegranteComponent implements OnInit {
  public form: FormGroup;
  @Input() public integrante: Integrante = new Integrante();
  public urlGrupoFamiliar: AccionGrupoUnico = AccionGrupoUnico.GRUPO_FAMILIAR_MODIFICACION_INTERNA;

  constructor(private fb: FormBuilder,
              private sexoService: SexoService,
              private  paisService: PaisService,
              private activeModal: NgbActiveModal) {
  }

  public ngOnInit(): void {
  }

  public cerrar(): void {
    this.activeModal.close(true);
  }
}
