 export class ConsultaSituacionPersonas {
   public tipoPersona: number;
   public cuil: string;
   public apellido: string;
   public nombre: string;
   public dni: string;
   public numeroSticker: number;
   public numeroFormulario: number;
   public numeroPrestamo: number;
   public numeroPagina: number;
   public tamañoPagina: number;

   constructor(tipoPersona?: number,
               cuil?: string,
               apellido?: string,
               nombre?: string,
               dni?: string,
               numeroSticker?: number,
               numeroFormulario?: number,
               numeroPrestamo?: number,
               numeroPagina?: number,
               tamañoPagina?: number) {
     this.tipoPersona = tipoPersona;
     this.cuil = cuil;
     this.apellido = apellido;
     this.nombre = nombre;
     this.dni = dni;
     this.numeroSticker = numeroSticker;
     this.numeroFormulario = numeroFormulario;
     this.numeroPrestamo = numeroPrestamo;
     this.numeroPagina = numeroPagina;
     this.tamañoPagina = tamañoPagina;
   }
 }
