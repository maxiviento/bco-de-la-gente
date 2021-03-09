import { Request } from '@angular/http';

export class Petition {
  public uuid: string;
  public req: Request;
  public isActive: boolean = true;

  private deltaTime: number = 1500; //TIEMPO PARA VOLVER A ENVIAR MISMA PETICIÓN (EN MILISEGUNDOS).
  private readonly DELTA_TIMER = 500;
  private interval;

  public constructor(uuid: string, req: Request) {
    this.uuid = uuid;
    this.req = req;
    this.startTimer();
    this.deltaTime = this.deltaTime - this.DELTA_TIMER; //CORECCIÓN DE TIEMPO.
  }

  private startTimer() {
    this.interval = setInterval(() => {
      if (this.deltaTime > 0) {
        this.deltaTime = this.deltaTime - this.DELTA_TIMER ;
      } else {
        clearInterval(this.interval);
        this.isActive = false;
      }
    }, this.DELTA_TIMER);
  }
}
