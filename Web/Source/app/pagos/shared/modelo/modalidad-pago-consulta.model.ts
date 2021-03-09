export class ModalidadPagoConsulta{
  public fechaForm: Date;
  public elementoId: number;
  public modalidadId: number;
  public convenioId: number;
  public mesesGracia: number;


  constructor(fechaForm?: Date,
              elementoId?: number,
              modalidadId?: number,
              convenioId?: number,
              mesesGracia?: number) {
    this.fechaForm = fechaForm;
    this.elementoId = elementoId;
    this.modalidadId = modalidadId;
    this.convenioId = convenioId;
    this.mesesGracia = mesesGracia;
  }
}
