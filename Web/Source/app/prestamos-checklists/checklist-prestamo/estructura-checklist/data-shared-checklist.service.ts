import { Injectable } from '@angular/core';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ChecklistEditableModel } from '../../shared/modelos/checklist-editable.model';

@Injectable()
export class DataSharedChecklistService {
  private deshabilitarChange: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private rezhazoIntegrante: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private cantidadFormularios: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  private editableEmitter: BehaviorSubject<ChecklistEditableModel []> = new BehaviorSubject<ChecklistEditableModel []>(undefined);
  private areas: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private etapas: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private observaciones: BehaviorSubject<string> = new BehaviorSubject<string>('');

  private totalFolios: number;

  constructor() {
    this.deshabilitarChange.next(true);
  }

  public modificarEstado(value: boolean) {
    this.deshabilitarChange.next(value);
  }

  public modificarEditable(value: ChecklistEditableModel[]) {
    this.editableEmitter.next(value);
  }

  public modificarEtapas(value: boolean) {
    this.etapas.next(value);
  }

  public modificarCantidadFormularios(value: number) {
    this.cantidadFormularios.next(value);
  }

  public modificarRechazo(value: boolean) {
    this.rezhazoIntegrante.next(value);
  }

  public modificarAreas(value: boolean) {
    this.areas.next(value);
  }

  public modificarCantFolios(value: number) {
    this.totalFolios = value;
  }

  public modificarObservaciones(value: string) {
    this.observaciones.next(value);
  }

  public getSubject(): Observable<any> {
    return this.deshabilitarChange.asObservable();
  }

  public getSubjectCantidadFormularios(): Observable<any> {
    return this.cantidadFormularios.asObservable();
  }

  public getSubjectEditable(): Observable<any> {
    return this.editableEmitter.asObservable();
  }

  public getSubjecAreas(): Observable<any> {
    return this.areas.asObservable();
  }

  public getSubjectRechazo(): Observable<any> {
    return this.rezhazoIntegrante.asObservable();
  }

  public getSubjectEtapas(): Observable<any> {
    return this.etapas.asObservable();
  }

  public obtenerCantFolios() {
    return this.totalFolios;
  }

  public getSubjecObservaciones(): Observable<any> {
    return this.observaciones.asObservable();
  }

}
