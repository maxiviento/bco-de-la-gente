export class RequisitoPrestamo {
  public id: number;
  public idEtapa: number;
  public idArea: number;
  public idItem: number;
  public idPrestamoRequisito: number;
  public nombreEtapa: string;
  public nombreArea: string;
  public nombreItem: string;
  public itemPadre: number;
  public urlRecurso: string;
  public esGarante: boolean;
  public esSolicitante: boolean;
  public esSolicitanteGarante: boolean;
  public esAlerta: boolean;
  public esChecklist: boolean;
  public idPrestamo: number;
  public nroOrden: number;
  public requisitosHijos: RequisitoPrestamo [];
  public subeArchivo: boolean;
  public generaArchivo: boolean;
  public idTipoDocumentacionCdd: number;
  public gestionaArchivo: boolean;

  constructor() {
    this.id = undefined;
    this.idEtapa = undefined;
    this.idArea = undefined;
    this.idItem = undefined;
    this.nombreEtapa = undefined;
    this.nombreArea = undefined;
    this.nombreItem = undefined;
    this.itemPadre = undefined;
    this.urlRecurso = undefined;
    this.esGarante = false;
    this.esSolicitante = false;
    this.esSolicitanteGarante = false;
    this.esChecklist = false;
    this.idPrestamo = undefined;
    this.nroOrden = undefined;
    this.requisitosHijos = [];
    this.subeArchivo = false;
    this.generaArchivo = false;
    this.idTipoDocumentacionCdd = undefined;
    this.gestionaArchivo = false;
    this.idPrestamoRequisito = undefined;
    this.esAlerta = false;
  }

  public static construirRequisitoChecklist(id: number,
                                            idItem: number,
                                            nombreItem: string,
                                            itemPadre: number,
                                            urlRecurso: string,
                                            idArea: number,
                                            idPrestamo: number,
                                            subeArchivo: boolean,
                                            generaArchivo: boolean,
                                            idTipoDocumentacionCdd: number,
                                            idPrestamoRequisito: number,
                                            esAlerta: boolean): RequisitoPrestamo {
    let requisito = new RequisitoPrestamo();
    requisito.id = id;
    requisito.idItem = idItem;
    requisito.nombreItem = nombreItem;
    requisito.itemPadre = itemPadre;
    requisito.urlRecurso = urlRecurso;
    requisito.idArea = idArea;
    requisito.idPrestamo = idPrestamo;
    requisito.subeArchivo = subeArchivo;
    requisito.generaArchivo = generaArchivo;
    requisito.idTipoDocumentacionCdd = idTipoDocumentacionCdd;
    requisito.idPrestamoRequisito = idPrestamoRequisito;
    requisito.esAlerta = esAlerta;
    return requisito;
  }
}
